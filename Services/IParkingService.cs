using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services
{
    public interface IParkingService
    {
        // Existing methods
        Task<bool> AddVehicleAsync(Vehicle vehicle, int column);
        Task<bool> RemoveVehicleAsync(string licensePlate);
        Task<Vehicle?> FindVehicleAsync(string licensePlate);
        Task<string> SearchVehicleAsync(string licensePlate);
        Task<List<string>> GetAllLicensePlatesAsync();
        Task<List<int>> GetAvailableColumnsAsync(string vehicleType);
        Task<bool> IsColumnAvailableAsync(int column, string vehicleType);
        Task<int> GetAvailableParkingSpacesAsync();
        Task<int> GetOccupiedParkingSpacesAsync();
        Task<List<ParkingReservation>> GetActiveReservationsAsync();
        void DisplayParkingLayout(string vehicleType, int startRow, int rowCount);

        Task<bool> ParkVehicleAsync(string licensePlate, string vehicleType, int column, int userId);
        Task<List<ParkingReservation>> GetAllActiveReservationsAsync();
    }
}