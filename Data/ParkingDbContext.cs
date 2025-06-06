using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Data
{
    public class ParkingDbContext : DbContext // ParkingDbContext dziedziczy po DbContext (główna klasa do interakcji z bazą danych)
    {
        public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options)
        {

        }

        //Kazda wartosc DbSet<T> reprezentuje tabele w bazie danych (pozwala na wykonywanie operacji CRUD)
        public DbSet<User> Users { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ParkingSpace> ParkingSpaces { get; set; }
        public DbSet<ParkingReservation> ParkingReservations { get; set; }

    }
}