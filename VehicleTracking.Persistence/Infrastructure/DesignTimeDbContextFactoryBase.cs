using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace VehicleTracking.Persistence.Infrastructure
{
	public abstract class DesignTimeDbContextFactoryBase<TContext> :
        IDesignTimeDbContextFactory<TContext> where TContext : DbContext
	{
		private const string ConnectionStringName = "DefaultConnection";
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

		public TContext CreateDbContext(string[] args)
		{
			string basePath = Directory.GetCurrentDirectory() + string.Format("{0}..{0}VehicleTracking.API", Path.DirectorySeparatorChar);
			return Create(basePath, Environment.GetEnvironmentVariable(AspNetCoreEnvironment));
		}

		protected abstract TContext CreateNewInstance(string connectionString);

		private TContext Create(string basePath, string environmentName)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.Local.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

			var connectionString = configuration.GetConnectionString(ConnectionStringName);

			return Create(connectionString);
		}

		private TContext Create(string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"Connection string '{ConnectionStringName}' is null or empty.", nameof(connectionString));
            }

            return CreateNewInstance(connectionString);
		}
	}
}
