using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetUserVehiclesAsync(int userId); // zwraca listę pojazdów użytkownika
        Task<List<VehicleType>> GetAllVehicleTypesAsync(); // zwraca listę wszystkich typów pojazdów
        Task<bool> CreateVehicleAsync(Vehicle vehicle); // tworzy nowy pojazd, zwraca true jeśli sukces
        Task<bool> UpdateVehicleAsync(Vehicle vehicle); // aktualizuje istniejący pojazd, zwraca true jeśli sukces
        Task<bool> DeleteVehicleAsync(int vehicleId); // usuwa pojazd na podstawie jego identyfikatora, zwraca true jeśli sukces
        Task<Vehicle?> GetVehicleByIdAsync(int vehicleId); // zwraca pojazd na podstawie jego identyfikatora, lub null jeśli nie znaleziono
        Task<bool> IsLicensePlateUniqueAsync(string licensePlate, int? excludeVehicleId = null); // sprawdza, czy numer rejestracyjny jest unikalny, opcjonalnie z wykluczeniem pojazdu o podanym identyfikatorze
    }
}