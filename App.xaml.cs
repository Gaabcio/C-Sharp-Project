using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkingManagementSystem.Data;
using ParkingManagementSystem.Services;
using ParkingManagementSystem.Validators;
using ParkingManagementSystem.Views;
using System.IO;
using System.Windows;

namespace ParkingManagementSystem
{
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }
        public static IConfiguration? Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Build configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // Configure services
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // Ensure database is created
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ParkingDbContext>();
                dbContext.Database.EnsureCreated();
            }

            // Show main window
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Configuration
            services.AddSingleton(Configuration!);

            // Database - UPROSZCZONA wersja
            services.AddDbContext<ParkingDbContext>(options =>
            {
                var connectionString = Configuration!.GetConnectionString("DefaultConnection");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IParkingService, ParkingService>();

            // Validators
            services.AddScoped<VehicleValidator>();

            // Views
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginPage>();
            services.AddTransient<MainPage>();
            services.AddTransient<SearchVehicleWindow>();
            services.AddTransient<UnparkVehicleWindow>();
            services.AddTransient<ManageVehiclesWindow>();
        }
    }
}