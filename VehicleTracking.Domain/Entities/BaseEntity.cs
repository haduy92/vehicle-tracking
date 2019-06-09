using System;

namespace VehicleTracking.Domain.Entities
{
	public abstract class BaseEntity
	{
		protected BaseEntity()
		{
			Id = Guid.NewGuid();
			CreatedDate = DateTime.UtcNow;
		}

		public Guid Id { get; private set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}
