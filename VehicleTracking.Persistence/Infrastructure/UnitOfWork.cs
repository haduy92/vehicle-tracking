﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking.Common;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Domain.Enumerations;
using VehicleTracking.Persistence.Repositories;

namespace VehicleTracking.Persistence.Infrastructure
{
	public interface IUnitOfWork : IDisposable
	{
		IAuditLogRepository AuditLogRepository { get; }
		IUserRepository UserRepository { get; }
		IVehicleRepository VehicleRepository { get; }
		ITrackingRepository TrackingRepository { get; }
		ITrackingSnapshotRepository TrackingSnapshotRepository { get; }

		int Commit(bool acceptAllChangesOnSuccess = true);
		Task<int> CommitAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default(CancellationToken));
		void Rollback();
	}

	public class UnitOfWork : IUnitOfWork
	{
		private readonly IDbContext _dbContext;
		private readonly IDateTime _dateTime;
		private IVehicleRepository _vehicleRepository;
		private IAuditLogRepository _auditLogRepository;
		private IUserRepository _userRepository;
		private ITrackingRepository _trackingRepository;
		private ITrackingSnapshotRepository _trackingSnapshotRepository;

		public UnitOfWork(IDbContext dbContext, IDateTime dateTime)
		{
			_dbContext = dbContext;
			_dateTime = dateTime;
		}

		public IVehicleRepository VehicleRepository
		{
			get { return _vehicleRepository ?? (_vehicleRepository = new VehicleRepository(_dbContext, _dateTime)); }
		}

		public IAuditLogRepository AuditLogRepository
		{
			get { return _auditLogRepository ?? (_auditLogRepository = new AuditLogRepository(_dbContext, _dateTime)); }
		}

		public IUserRepository UserRepository
		{
			get { return _userRepository ?? (_userRepository = new UserRepository(_dbContext, _dateTime)); }
		}

		public ITrackingRepository TrackingRepository
		{
			get { return _trackingRepository ?? (_trackingRepository = new TrackingRepository(_dbContext, _dateTime)); }
		}

		public ITrackingSnapshotRepository TrackingSnapshotRepository
		{
			get { return _trackingSnapshotRepository ?? (_trackingSnapshotRepository = new TrackingSnapshotRepository(_dbContext, _dateTime)); }
		}

		public int Commit(bool acceptAllChangesOnSuccess = true)
		{
			var auditEntries = OnBeforeCommit();
			var result = _dbContext.SaveChanges(acceptAllChangesOnSuccess);
			OnAfterCommit(auditEntries).Wait();
			return result;
		}

		public Task<int> CommitAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default(CancellationToken))
		{
			var auditEntries = OnBeforeCommit();
			var result = _dbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
			OnAfterCommit(auditEntries).Wait();
			return result;
		}

		private List<AuditEntry> OnBeforeCommit()
		{
			_dbContext.ChangeTracker.DetectChanges();

			var auditEntries = new List<AuditEntry>();

			foreach (var entry in _dbContext.ChangeTracker.Entries())
			{
				if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
				{
					continue;
				}

				var auditEntry = new AuditEntry(entry, _dateTime);
				auditEntry.TableName = entry.Metadata.Relational().TableName;
				auditEntries.Add(auditEntry);

				foreach (var property in entry.Properties)
				{
					if (property.IsTemporary)
					{
						// value will be generated by the database, get the value after saving
						auditEntry.TemporaryProperties.Add(property);
						continue;
					}

					string propertyName = property.Metadata.Name;
					if (property.Metadata.IsPrimaryKey())
					{
						auditEntry.KeyValues[propertyName] = property.CurrentValue;
						continue;
					}

					switch (entry.State)
					{
						case EntityState.Added:
							auditEntry.Action = AuditAction.Create;
							auditEntry.NewValues[propertyName] = property.CurrentValue;
							break;

						case EntityState.Deleted:
							auditEntry.Action = AuditAction.Delete;
							auditEntry.OldValues[propertyName] = property.OriginalValue;
							break;

						case EntityState.Modified:
							if (property.IsModified)
							{
								auditEntry.Action = AuditAction.Update;
								auditEntry.OldValues[propertyName] = property.OriginalValue;
								auditEntry.NewValues[propertyName] = property.CurrentValue;
							}
							break;
					}
				}
			}

			// Save audit entities that have all the modifications
			foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
			{
				AuditLogRepository.Create(auditEntry.ToAuditLog());
			}

			// keep a list of entries where the value of some properties are unknown at this step
			return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
		}

		private Task OnAfterCommit(List<AuditEntry> auditEntries)
		{
			if (auditEntries == null || auditEntries.Count == 0)
			{
				return Task.CompletedTask; 
			}

			foreach (var auditEntry in auditEntries)
			{
				// Get the final value of the temporary properties
				foreach (var prop in auditEntry.TemporaryProperties)
				{
					if (prop.Metadata.IsPrimaryKey())
					{
						auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
					}
					else
					{
						auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
					}
				}

				// Save the Audit entry
				AuditLogRepository.Create(auditEntry.ToAuditLog());
			}

			return CommitAsync();
		}

		public void Rollback()
		{
			_dbContext
				.ChangeTracker
				.Entries()
				.ToList()
				.ForEach(x => x.Reload());
		}

		private bool _disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_dbContext.Dispose();
				}
			}
			_disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}