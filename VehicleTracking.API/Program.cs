using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using VehicleTracking.Persistence;
using VehicleTracking.Persistence.Infrastructure;

namespace VehicleTracking.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateWebHostBuilder(args).Build();

			using (var scope = host.Services.CreateScope())
			{
				try
				{
					var context = (VehicleTrackingDbContext) scope.ServiceProvider.GetService<IDbContext>();
					var unitOfWork = (UnitOfWork) scope.ServiceProvider.GetService<IUnitOfWork>();

					context.Database.Migrate();
					context.EnsureDatabaseCreated();
					VehicleTrackingDataSeeder.Initialize(unitOfWork);
				}
				catch (Exception ex)
				{
					var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while migrating or initializing the database.");
				}
			}

			host.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
				.UseStartup<Startup>()
				.UseUrls("http://localhost:5000");
	}
}
