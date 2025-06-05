using Microsoft.EntityFrameworkCore;
using ParkingManagementSystem.Data;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ParkingDbContext _context;

        public VehicleService(ParkingDbContext context)
        {
            _context = context;
        }

        
public async Task<List<Vehicle>> GetUserVehiclesAsync(int userId) // zwraca listę pojazdów użytkownika (wyswietlanie pojazdów w panelu użytkownika, lista wyboru pojazdu przy parkowaniu)
{
        return await _context.Vehicles
            .Include(v => v.VehicleType)    // Eager loading - typ pojazdu
            .Include(v => v.User)           // Eager loading - właściciel
            .Where(v => v.UserId == userId) // Filtrowanie po użytkowniku
            .OrderBy(v => v.LicensePlate)   // Sortowanie alfabetyczne
            .AsNoTracking()                 // Bez śledzenia zmian = szybciej
            .ToListAsync();
}
        public async Task<List<VehicleType>> GetAllVehicleTypesAsync()  // zwraca listę wszystkich typów pojazdów (Dropdown/ComboBox przy dodawaniu pojazdu, Wybór typu w formularzach)
        {
            return await _context.VehicleTypes
                .OrderBy(vt => vt.Name)
                .ToListAsync();
        }

        public async Task<bool> CreateVehicleAsync(Vehicle vehicle)  // tworzy nowy pojazd, zwraca true jeśli sukces
        {
            try
            {
                vehicle.CreatedAt = DateTime.Now;
                vehicle.LicensePlate = vehicle.LicensePlate.ToUpper(); // Upewniamy się, że numer rejestracyjny jest zapisany wielkimi literami
                
                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateVehicleAsync(Vehicle vehicleDataFromForm) // aktualizuje pojazd, zwraca true jeśli sukces
        {
            try
            {
                
                var vehicleToUpdate = await _context.Vehicles.FindAsync(vehicleDataFromForm.Id);

                if (vehicleToUpdate == null)
                {
                    
                    System.Diagnostics.Debug.WriteLine($"Nie znaleziono pojazdu o ID: {vehicleDataFromForm.Id} do aktualizacji.");
                    return false;
                }

                
                vehicleToUpdate.LicensePlate = vehicleDataFromForm.LicensePlate.ToUpper();
                vehicleToUpdate.VehicleTypeId = vehicleDataFromForm.VehicleTypeId;
                vehicleToUpdate.Brand = vehicleDataFromForm.Brand;
                vehicleToUpdate.Model = vehicleDataFromForm.Model;
                vehicleToUpdate.Color = vehicleDataFromForm.Color;
                vehicleToUpdate.Year = vehicleDataFromForm.Year;


                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Blad podczas UpdateVehicleAsync: {ex.ToString()}");
                return false;
            }
        }


        public async Task<bool> DeleteVehicleAsync(int vehicleId)  // usuwa pojazd, zwraca true jeśli sukces
        {
            try
            {
                var vehicle = await _context.Vehicles.FindAsync(vehicleId);
                if (vehicle != null)
                {
                    _context.Vehicles.Remove(vehicle);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(int vehicleId)  // zwraca pojazd po ID, może być null jeśli nie znaleziono (Wyświetlanie szczegółów pojazdu, Edycja pojazdu (pobieranie aktualnych danych))
        {
            return await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == vehicleId);
        }

        public async Task<bool> IsLicensePlateUniqueAsync(string licensePlate, int? excludeVehicleId = null) // sprawdza, czy numer rejestracyjny jest unikalny, opcjonalnie z wykluczeniem pojazdu o podanym identyfikatorze
        {
            var query = _context.Vehicles.Where(v => v.LicensePlate == licensePlate.ToUpper());
            
            if (excludeVehicleId.HasValue)
                query = query.Where(v => v.Id != excludeVehicleId.Value);

            return !await query.AnyAsync();
        }
    }
}