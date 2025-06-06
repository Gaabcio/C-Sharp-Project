using Microsoft.EntityFrameworkCore;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ParkingDbContext context)
        {
            // Sprawdź czy dane już istnieją
            if (await context.VehicleTypes.AnyAsync())
                return; // Dane już są zaseedowane

            // Seed VehicleTypes
            var vehicleTypes = new[]
            {
                new VehicleType { Name = "Motocykl", Description = "Jednoślad", SpacesRequired = 1, AllowedRows = "0"},
                new VehicleType { Name = "Samochód", Description = "Samochód osobowy", SpacesRequired = 2, AllowedRows = "1,2"},
                new VehicleType { Name = "Autobus", Description = "Pojazd wieloosobowy", SpacesRequired = 4, AllowedRows = "3,4,5,6"}
            };

            await context.VehicleTypes.AddRangeAsync(vehicleTypes);
            await context.SaveChangesAsync();

            // Seed dla administratora
            var adminUser = new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                Email = "admin@parking.com",
                FirstName = "Administrator",
                LastName = "System"
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();

            // Seed miejsc parkingowych
            var parkingSpaces = new List<ParkingSpace>();
            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    parkingSpaces.Add(new ParkingSpace { Row = row, Column = col });
                }
            }

            await context.ParkingSpaces.AddRangeAsync(parkingSpaces);
            await context.SaveChangesAsync();

            // Seed dla przykładowych pojazdów (po zapisaniu user i vehicleTypes)
            var motocyklType = await context.VehicleTypes.FirstAsync(vt => vt.Name == "Motocykl");
            var samochodType = await context.VehicleTypes.FirstAsync(vt => vt.Name == "Samochód");
            var autobusType = await context.VehicleTypes.FirstAsync(vt => vt.Name == "Autobus");

            var vehicles = new[]
            {
                new Vehicle { LicensePlate = "ABC123", UserId = adminUser.Id, VehicleTypeId = samochodType.Id, Brand = "Toyota", Model = "Corolla", Color = "Czerwony", Year = 2020 },
                new Vehicle { LicensePlate = "XYZ789", UserId = adminUser.Id, VehicleTypeId = motocyklType.Id, Brand = "Honda", Model = "CB500F", Color = "Czarny", Year = 2019 },
                new Vehicle { LicensePlate = "RST321", UserId = adminUser.Id, VehicleTypeId = autobusType.Id, Brand = "Mercedes", Model = "Extremum", Color = "Niebieski", Year = 2005 }
            };

            await context.Vehicles.AddRangeAsync(vehicles);
            await context.SaveChangesAsync();
        }
    }
}