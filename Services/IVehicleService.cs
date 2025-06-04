using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetUserVehiclesAsync(int userId);
        Task<List<VehicleType>> GetAllVehicleTypesAsync();
        Task<bool> CreateVehicleAsync(Vehicle vehicle);
        Task<bool> UpdateVehicleAsync(Vehicle vehicle);
        Task<bool> DeleteVehicleAsync(int vehicleId);
        Task<Vehicle?> GetVehicleByIdAsync(int vehicleId);
        Task<bool> IsLicensePlateUniqueAsync(string licensePlate, int? excludeVehicleId = null);
    }
}