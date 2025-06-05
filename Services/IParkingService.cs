using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services
{
    public interface IParkingService
    {
        Task<bool> AddVehicleAsync(Vehicle vehicle, int column); // dodaje pojazd do określonej kolumny, zwraca true jeśli sukces
        Task<bool> RemoveVehicleAsync(string licensePlate); // usuwa pojazd na podstawie numeru rejestracyjnego, zwraca true jeśli sukces
        Task<Vehicle?> FindVehicleAsync(string licensePlate); // wyszukuje pojazd na podstawie numeru rejestracyjnego, zwraca pojazd lub null jeśli nie znaleziono
        Task<string> SearchVehicleAsync(string licensePlate); // wyszukuje pojazd na podstawie numeru rejestracyjnego, zwraca informację o pojeździe lub komunikat o błędzie
        Task<List<string>> GetAllLicensePlatesAsync();  // zwraca listę wszystkich numerów rejestracyjnych pojazdów
        Task<List<int>> GetAvailableColumnsAsync(string vehicleType); // zwraca listę dostępnych kolumn dla określonego typu pojazdu
        Task<bool> IsColumnAvailableAsync(int column, string vehicleType); // sprawdza, czy określona kolumna jest dostępna dla danego typu pojazdu

        // Task<int> GetAvailableParkingSpacesAsync(); // zwraca liczbę dostępnych miejsc parkingowych
        // Task<int> GetOccupiedParkingSpacesAsync(); // zwraca liczbę zajętych miejsc parkingowych
        Task<List<ParkingReservation>> GetActiveReservationsAsync(); // zwraca listę aktywnych rezerwacji parkingowych
        // void DisplayParkingLayout(string vehicleType, int startRow, int rowCount); // wyświetla układ parkingu dla określonego typu pojazdu, zaczynając od podanego wiersza i wyświetlając określoną liczbę wierszy
        Task<bool> ParkVehicleAsync(string licensePlate, string vehicleType, int column, int userId); // parkuje pojazd na podstawie numeru rejestracyjnego, typu pojazdu, kolumny i identyfikatora użytkownika, zwraca true jeśli sukces
        Task<List<ParkingReservation>> GetAllActiveReservationsAsync(); // zwraca listę wszystkich aktywnych rezerwacji parkingowych
    }
}