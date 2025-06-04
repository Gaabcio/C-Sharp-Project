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

        //Dokonano optymalizacji poprzez dodanie Include dla VehicleType i User, aby unikn¹æ N+1 problemu
        public async Task<List<Vehicle>> GetUserVehiclesAsync(int userId)
        {
            return await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.User)                 
                .Where(v => v.UserId == userId)
                .OrderBy(v => v.LicensePlate)
                .AsNoTracking()                       
                .ToListAsync();
        }

        public async Task<List<VehicleType>> GetAllVehicleTypesAsync()
        {
            return await _context.VehicleTypes
                .OrderBy(vt => vt.Name)
                .ToListAsync();
        }

        public async Task<bool> CreateVehicleAsync(Vehicle vehicle)
        {
            try
            {
                vehicle.CreatedAt = DateTime.Now;
                vehicle.LicensePlate = vehicle.LicensePlate.ToUpper();
                
                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateVehicleAsync(Vehicle vehicleDataFromForm) 
        {
            try
            {
                // 1. ZnajdŸ istniej¹c¹, œledzon¹ encjê w bazie danych
                var vehicleToUpdate = await _context.Vehicles.FindAsync(vehicleDataFromForm.Id);

                if (vehicleToUpdate == null)
                {
                    // Pojazd o danym ID nie istnieje w bazie, to b³¹d
                    System.Diagnostics.Debug.WriteLine($"Nie znaleziono pojazdu o ID: {vehicleDataFromForm.Id} do aktualizacji.");
                    return false;
                }

                // 2. Zaktualizuj w³aœciwoœci œledzonej encji danymi z formularza
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
                System.Diagnostics.Debug.WriteLine($"B³¹d podczas UpdateVehicleAsync: {ex.ToString()}");
                return false;
            }
        }


        public async Task<bool> DeleteVehicleAsync(int vehicleId)
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

        public async Task<Vehicle?> GetVehicleByIdAsync(int vehicleId)
        {
            return await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == vehicleId);
        }

        public async Task<bool> IsLicensePlateUniqueAsync(string licensePlate, int? excludeVehicleId = null)
        {
            var query = _context.Vehicles.Where(v => v.LicensePlate == licensePlate.ToUpper());
            
            if (excludeVehicleId.HasValue)
                query = query.Where(v => v.Id != excludeVehicleId.Value);

            return !await query.AnyAsync();
        }
    }
}