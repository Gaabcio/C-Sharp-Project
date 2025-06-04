using Microsoft.EntityFrameworkCore;
using ParkingManagementSystem.Data;
using ParkingManagementSystem.Models;


namespace ParkingManagementSystem.Services
{
    public class ParkingService : IParkingService
    {
        private readonly ParkingDbContext _context;
        private const int MaxColumns = 10;
        private const int MaxRows = 7;

        public ParkingService(ParkingDbContext context)
        {
            _context = context;
        }

        // NEW METHOD 1: ParkVehicleAsync
        public async Task<bool> ParkVehicleAsync(string licensePlate, string vehicleType, int column, int userId)
        {
            try
            {
                // Check if vehicle already exists
                var existingVehicle = await _context.Vehicles
                    .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);

                Vehicle vehicle;

                if (existingVehicle == null)
                {
                    // Get vehicle type
                    var vType = await _context.VehicleTypes
                        .FirstOrDefaultAsync(vt => vt.Name == vehicleType);

                    if (vType == null)
                        return false;

                    // Create new vehicle
                    vehicle = new Vehicle
                    {
                        LicensePlate = licensePlate.ToUpper(),
                        UserId = userId,
                        VehicleTypeId = vType.Id
                    };

                    _context.Vehicles.Add(vehicle);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    vehicle = existingVehicle;

                    // Check if already parked
                    var activeReservation = await _context.ParkingReservations
                        .FirstOrDefaultAsync(pr => pr.VehicleId == vehicle.Id && pr.IsActive);

                    if (activeReservation != null)
                        return false; // Already parked
                }

                // Get vehicle type details
                var vehicleTypeEntity = await _context.VehicleTypes
                    .FindAsync(vehicle.VehicleTypeId);

                if (vehicleTypeEntity == null)
                    return false;

                // Check if column is available
                if (!await IsColumnAvailableAsync(column, vehicleTypeEntity.Name))
                    return false;

                // Get allowed rows for this vehicle type
                var allowedRows = GetAllowedRows(vehicleTypeEntity.Name);
                var availableSpace = await GetAvailableParkingSpaceAsync(allowedRows, column, vehicleTypeEntity.SpacesRequired);

                if (availableSpace == null)
                    return false;

                // Create parking reservation
                var reservation = new ParkingReservation
                {
                    VehicleId = vehicle.Id,
                    UserId = userId,
                    ParkingSpaceId = availableSpace.Id,
                    StartTime = DateTime.Now,
                    IsActive = true
                };

                _context.ParkingReservations.Add(reservation);

                // Mark spaces as occupied
                await MarkSpacesAsOccupied(allowedRows, column, vehicleTypeEntity.SpacesRequired);

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }



        
        public async Task<List<ParkingReservation>> GetAllActiveReservationsAsync()
        {
            return await GetActiveReservationsAsync();
        }

        // EXISTING METHODS (keep all your existing methods here)
        public async Task<bool> AddVehicleAsync(Vehicle vehicle, int column)
        {
            try
            {
                // Check if vehicle already exists
                var existingVehicle = await _context.Vehicles
                    .FirstOrDefaultAsync(v => v.LicensePlate == vehicle.LicensePlate);

                if (existingVehicle != null)
                    return false;

                // Get vehicle type
                var vehicleType = await _context.VehicleTypes
                    .FindAsync(vehicle.VehicleTypeId);

                if (vehicleType == null)
                    return false;

                // Check if column is available
                if (!await IsColumnAvailableAsync(column, vehicleType.Name))
                    return false;

                // Get allowed rows for this vehicle type
                var allowedRows = GetAllowedRows(vehicleType.Name);
                var availableSpace = await GetAvailableParkingSpaceAsync(allowedRows, column, vehicleType.SpacesRequired);

                if (availableSpace == null)
                    return false;

                // Create vehicle
                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();

                // Create parking reservation
                var reservation = new ParkingReservation
                {
                    VehicleId = vehicle.Id,
                    UserId = vehicle.UserId,
                    ParkingSpaceId = availableSpace.Id,
                    StartTime = DateTime.Now,
                    IsActive = true
                };

                _context.ParkingReservations.Add(reservation);

                // Mark spaces as occupied
                await MarkSpacesAsOccupied(allowedRows, column, vehicleType.SpacesRequired);

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveVehicleAsync(string licensePlate)
        {
            try
            {
                var vehicle = await _context.Vehicles
                    .Include(v => v.VehicleType)
                    .Include(v => v.ParkingReservations)
                        .ThenInclude(pr => pr.ParkingSpace)
                    .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);

                if (vehicle == null)
                    return false;

                var activeReservation = vehicle.ParkingReservations
                    .FirstOrDefault(pr => pr.IsActive);

                if (activeReservation != null)
                {
                    // Free up parking spaces
                    var allowedRows = GetAllowedRows(vehicle.VehicleType.Name);
                    await MarkSpacesAsAvailable(allowedRows, activeReservation.ParkingSpace.Column, vehicle.VehicleType.SpacesRequired);

                    _context.ParkingReservations.Remove(activeReservation);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Vehicle?> FindVehicleAsync(string licensePlate)
        {
            return await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.User)
                .Include(v => v.ParkingReservations.Where(pr => pr.IsActive))
                    .ThenInclude(pr => pr.ParkingSpace)
                .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);
        }

        public async Task<string> SearchVehicleAsync(string licensePlate)
        {
            var vehicle = await FindVehicleAsync(licensePlate);

            if (vehicle == null)
                return "Pojazd o podanym numerze rejestracyjnym nie został znaleziony.";

            var activeReservation = vehicle.ParkingReservations.FirstOrDefault(pr => pr.IsActive);

            if (activeReservation == null)
                return "Pojazd nie jest obecnie zaparkowany.";

            var duration = DateTime.Now - activeReservation.StartTime;
            var durationText = $"{(int)duration.TotalHours}h {duration.Minutes}m";

            return $"📋 Typ: {vehicle.VehicleType.Name}\n" +
                   $"📍 Kolumna: {activeReservation.ParkingSpace.Column}\n" +
                   $"⏰ Zaparkowany: {activeReservation.StartTime:dd.MM.yyyy HH:mm}\n" +
                   $"⌚ Czas parkowania: {durationText}";
        }

        public async Task<List<string>> GetAllLicensePlatesAsync()
        {
            return await _context.Vehicles
                .Where(v => v.ParkingReservations.Any(pr => pr.IsActive))
                .Select(v => v.LicensePlate)
                .ToListAsync();
        }

        public async Task<List<int>> GetAvailableColumnsAsync(string vehicleType)
        {
            var occupiedColumns = await GetOccupiedColumnsAsync(vehicleType);
            var availableColumns = new List<int>();

            for (int col = 0; col < MaxColumns; col++)
            {
                if (!occupiedColumns.Contains(col))
                {
                    availableColumns.Add(col);
                }
            }

            return availableColumns;
        }

        public async Task<bool> IsColumnAvailableAsync(int column, string vehicleType)
        {
            var occupiedColumns = await GetOccupiedColumnsAsync(vehicleType);
            return !occupiedColumns.Contains(column);
        }

        public async Task<int> GetAvailableParkingSpacesAsync()
        {
            var totalSpaces = MaxRows * MaxColumns;
            var occupiedSpaces = await _context.ParkingSpaces
                .CountAsync(ps => ps.IsOccupied);

            return totalSpaces - occupiedSpaces;
        }

        public async Task<int> GetOccupiedParkingSpacesAsync()
        {
            return await _context.ParkingSpaces
                .CountAsync(ps => ps.IsOccupied);
        }

        //Zastosowano niwelowanie problemu N+1 zapytań poprzez użycie Include i AsNoTracking
        public async Task<List<ParkingReservation>> GetActiveReservationsAsync()
        {
            return await _context.ParkingReservations
                .Include(pr => pr.Vehicle)
                    .ThenInclude(v => v.VehicleType)  
                .Include(pr => pr.Vehicle)
                    .ThenInclude(v => v.User)         
                .Include(pr => pr.User)
                .Include(pr => pr.ParkingSpace)
                .Where(pr => pr.IsActive)
                .OrderBy(pr => pr.StartTime)
                .AsNoTracking()                       
                .ToListAsync();
        }

        public void DisplayParkingLayout(string vehicleType, int startRow, int rowCount)
        {
            // Implementation for console display - can be empty for WPF app
            System.Console.WriteLine($"Parking layout for {vehicleType}");
        }

        // PRIVATE HELPER METHODS
        private async Task<List<int>> GetOccupiedColumnsAsync(string vehicleType)
        {
            var allowedRows = GetAllowedRows(vehicleType);

            return await _context.ParkingSpaces
                .Where(ps => allowedRows.Contains(ps.Row) && ps.IsOccupied)
                .Select(ps => ps.Column)
                .Distinct()
                .ToListAsync();
        }

        private async Task<ParkingSpace?> GetAvailableParkingSpaceAsync(List<int> allowedRows, int column, int spacesRequired)
        {
            return await _context.ParkingSpaces
                .FirstOrDefaultAsync(ps => allowedRows.Contains(ps.Row) &&
                                         ps.Column == column &&
                                         !ps.IsOccupied);
        }

        private async Task MarkSpacesAsOccupied(List<int> allowedRows, int column, int spacesRequired)
        {
            var spacesToOccupy = await _context.ParkingSpaces
                .Where(ps => allowedRows.Contains(ps.Row) && ps.Column == column)
                .Take(spacesRequired)
                .ToListAsync();

            foreach (var space in spacesToOccupy)
            {
                space.IsOccupied = true;
            }
        }

        private async Task MarkSpacesAsAvailable(List<int> allowedRows, int column, int spacesRequired)
        {
            var spacesToFree = await _context.ParkingSpaces
                .Where(ps => allowedRows.Contains(ps.Row) && ps.Column == column && ps.IsOccupied)
                .Take(spacesRequired)
                .ToListAsync();

            foreach (var space in spacesToFree)
            {
                space.IsOccupied = false;
            }
        }

        private static List<int> GetAllowedRows(string vehicleType)
        {
            return vehicleType.ToLower() switch
            {
                "motocykl" => new List<int> { 0 },
                "samochód" => new List<int> { 1, 2 },
                "autobus" => new List<int> { 3, 4, 5, 6 },
                _ => new List<int>()
            };
        }
    }
}