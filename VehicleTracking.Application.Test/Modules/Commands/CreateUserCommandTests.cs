using MediatR;
using Moq;
using System.Threading;
using VehicleTracking.Application.Modules.Commands;
using VehicleTracking.Application.Modules.Notifications;
using VehicleTracking.Application.Test.Infrastructure;
using Xunit;

namespace VehicleTracking.Application.Test.Modules.Commands
{
	public class CreateUserCommandTests : CommandTestBase
	{
		[Fact]
		public void Handle_GivenValidRequest_ShouldRaiseCustomerCreatedNotification()
		{
			// Arrange
			var mediatorMock = new Mock<IMediator>();
			var sut = new CreateUserCommand.Handler(_context, mediatorMock.Object);
			const string newUserEmail = "test@email.com";

			// Act
			var result = sut.Handle(new CreateUserCommand { Email = newUserEmail, Password = "abc@123" }, CancellationToken.None);

			// Assert
			mediatorMock.Verify(m => m.Publish(It.Is<UserCreated>(cc => cc.Email == newUserEmail), It.IsAny<CancellationToken>()), Times.Once);
		}
	}
}
