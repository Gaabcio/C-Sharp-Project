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

        //################################################################
        public DbSet<User> Users { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ParkingSpace> ParkingSpaces { get; set; }
        public DbSet<ParkingReservation> ParkingReservations { get; set; }
        //################################################################



        // Metoda OnModelCreating jest wywoływana podczas tworzenia modelu bazy danych
        // Tutaj możemy skonfigurować encje, relacje i inne aspekty modelu
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Wywołanie metody bazowej, aby zapewnić poprawne działanie dziedziczenia,

            // Konfiguracja encji i relacji

            // Konfiguracja User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
            });

            // konfiguracja VehicleType
            modelBuilder.Entity<VehicleType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });

            // konfiguracja Vehicle
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.LicensePlate).IsUnique();
                entity.Property(e => e.LicensePlate).IsRequired().HasMaxLength(20);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);          // Usunięcie pojazdu usuwa również wszystkie jego rezerwacje

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.VehicleTypeId)
                    .OnDelete(DeleteBehavior.Restrict);         // Usunięcie typu pojazdu nie usuwa pojazdów jeśli są znim powiązane
            });

            // konfiguracja ParkingSpace
            modelBuilder.Entity<ParkingSpace>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.Row, e.Column }).IsUnique();
                entity.Property(e => e.Row).IsRequired();
                entity.Property(e => e.Column).IsRequired();
            });

            // konfiguracja ParkingReservation
            modelBuilder.Entity<ParkingReservation>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.ParkingReservations)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);        // Usunięcie pojazdu usuwa również wszystkie jego rezerwacje

                // entity.HasOne(d => d.User)
                //     .WithMany(p => p.ParkingReservations)
                //     .HasForeignKey(d => d.UserId)
                //     .OnDelete(DeleteBehavior.Restrict);     

                entity.HasOne(d => d.ParkingSpace)
                    .WithMany(p => p.ParkingReservations)
                    .HasForeignKey(d => d.ParkingSpaceId)
                    .OnDelete(DeleteBehavior.Cascade);      // Usunięcie miejsca parkingowego usuwa również wszystkie jego rezerwacje
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed VehicleTypes
            modelBuilder.Entity<VehicleType>().HasData(
                new VehicleType { Id = 1, Name = "Motocykl", Description = "Jednoślad", SpacesRequired = 1, AllowedRows = "0"},
                new VehicleType { Id = 2, Name = "Samochód", Description = "Samochód osobowy", SpacesRequired = 2, AllowedRows = "1,2"},
                new VehicleType { Id = 3, Name = "Autobus", Description = "Pojazd wieloosobowy", SpacesRequired = 4, AllowedRows = "3,4,5,6"}
            );

            // Seed dla przykladowych pojazdow
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle { Id = 1, LicensePlate = "ABC123", UserId = 1, VehicleTypeId = 2, Brand = "Toyota", Model = "Corolla", Color = "Czerwony", Year = 2020 },
                new Vehicle { Id = 2, LicensePlate = "XYZ789", UserId = 1, VehicleTypeId = 1, Brand = "Honda", Model = "CB500F", Color = "Czarny", Year = 2019 },
                new Vehicle { Id = 3, LicensePlate = "RST321", UserId = 1, VehicleTypeId = 3, Brand = "Mercedes", Model = "Extremum", Color = "Niebieski", Year = 2005 }
            );

            // Seed dla administratora
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                    Email = "admin@parking.com",
                    FirstName = "Administrator",
                    LastName = "System"
                }
            );

            // Seed miejsc parkingowych
            var parkingSpaces = new List<ParkingSpace>();
            int id = 1;
            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    parkingSpaces.Add(new ParkingSpace { Id = id++, Row = row, Column = col });
                }
            }
            modelBuilder.Entity<ParkingSpace>().HasData(parkingSpaces);
        }
    }
}