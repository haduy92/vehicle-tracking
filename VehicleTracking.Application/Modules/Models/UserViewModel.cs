using System;
using System.Linq.Expressions;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Domain.ValueObjects;

namespace VehicleTracking.Application.Modules.Models
{
	public class UserViewModel
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public Address Address { get; set; }
		public bool EditEnabled { get; set; }
		public bool DeleteEnabled { get; set; }

		public string FullName
		{
			get { return $"{FirstName} {LastName}"; }
		}

		/// <summary>
		/// Map an User entity to UserViewModel in Select expression
		/// </summary>
		/// <example>
		/// <code>
		/// _unitOfWork.UserRepository.GetQueryable().Select(UserViewModel.Projection).ToList()
		/// </code>
		/// </example>
		public static Expression<Func<User, UserViewModel>> Projection
		{
			get
			{
				return entity => new UserViewModel
				{
					Id = entity.Id.ToString(),
					FirstName = entity.FirstName,
					LastName = entity.LastName,
					Address = entity.Address
				};
			}
		}

		public static UserViewModel Create(User entity)
		{
			return Projection.Compile().Invoke(entity);
		}
	}
}
