using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkingManagementSystem.Data;
using ParkingManagementSystem.Models;
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

        protected override async void OnStartup(StartupEventArgs e)
        {
            // Ładowanie konfiguracji z pliku appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // Konfiguracja usług DI
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Inicjalizacja bazy danych
            await InitializeDatabaseAsync();

            // Show main window
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        private async Task InitializeDatabaseAsync() // inicjalizacja bazy danych
        {
            using (var scope = ServiceProvider!.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ParkingDbContext>();

                try
                {
                    // Zastosuj migracje
                    await context.Database.MigrateAsync();

                    // Zaseeduj dane
                    await DataSeeder.SeedAsync(context);

                    // miejsce na usuwanie, dodawanie lub modyfikowanie danych w bazie po uruchomieniu

                    //context.Vehicles.Remove(await context.Vehicles.FindAsync(2));
                    //await context.SaveChangesAsync();

                    context.Vehicles.Add(new Vehicle { LicensePlate = "bbb", UserId = 1, VehicleTypeId = 1, Id=4, CreatedAt = DateTime.Now });
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas inicjalizacji bazy danych: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ConfigureServices(ServiceCollection services)
        {
           
            services.AddSingleton(Configuration!);
            
            services.AddDbContext<ParkingDbContext>(options =>  // konfiguracja połączenia do bazy danych
            {
                var connectionString = Configuration!.GetConnectionString("DefaultConnection");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            // Rejestracja serwisów
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IParkingService, ParkingService>();

            // Rejestracja walidatorów
            services.AddScoped<VehicleValidator>();

            // Rejestracja widoków
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginPage>();
            services.AddTransient<MainPage>();
            services.AddTransient<SearchVehicleWindow>();
            services.AddTransient<UnparkVehicleWindow>();
            services.AddTransient<ManageVehiclesWindow>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (ServiceProvider is IDisposable disposableProvider)
            {
                disposableProvider.Dispose();
            }
            base.OnExit(e);
        }
    }
}




















//context.Vehicles.Remove(await context.Vehicles.FindAsync(1));
//await context.SaveChangesAsync();

//context.Vehicles.Remove(context.Vehicles.First(v => v.LicensePlate == "ABC123"));
//await context.SaveChangesAsync();

// Dodanie pojazdu (wersja skrócona)
//context.Vehicles.Add(new Vehicle { LicensePlate = "ABC123", UserId = 1, VehicleTypeId = 1 });
//await context.SaveChangesAsync();

// Edycja pojazdu (wersja skrócona)
//var vehicle = context.Vehicles.First(v => v.LicensePlate == "WA12345");
//vehicle.Color = "Czarny";

//await context.SaveChangesAsync();