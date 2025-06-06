using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ParkingManagementSystem.Data
{
    public class ParkingDbContextFactory : IDesignTimeDbContextFactory<ParkingDbContext>
    {
        public ParkingDbContext CreateDbContext(string[] args)
        {
            // Załaduj konfigurację z appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Utwórz opcje DbContext
            var optionsBuilder = new DbContextOptionsBuilder<ParkingDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new ParkingDbContext(optionsBuilder.Options);
        }
    }
}