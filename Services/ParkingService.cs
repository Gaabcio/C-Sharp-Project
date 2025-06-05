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


        public async Task<bool> ParkVehicleAsync(string licensePlate, string vehicleType, int column, int userId) // Parkuje pojazd na podstawie numeru rejestracyjnego, typu pojazdu, kolumny i identyfikatora użytkownika
        {
            try
            {
                // sprawdzenie, czy pojazd o podanym numerze rejestracyjnym już istnieje
                var existingVehicle = await _context.Vehicles
                    .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);

                Vehicle vehicle;

                // jesli pojazd nie istnieje, tworzymy nowy pojazd
                if (existingVehicle == null)
                {
                    // Sprawdzenie, czy typ pojazdu istnieje
                    var vType = await _context.VehicleTypes
                        .FirstOrDefaultAsync(vt => vt.Name == vehicleType);

                    if (vType == null)
                        return false;

                    // Tworzenie nowego pojazdu
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

                    // sprawdzenie, czy pojazd jest już zaparkowany
                    var activeReservation = await _context.ParkingReservations
                        .FirstOrDefaultAsync(pr => pr.VehicleId == vehicle.Id && pr.IsActive);

                    if (activeReservation != null)
                        return false; // Pojazd jest już zaparkowany
                }

                // Zdobycie typu pojazdu
                var vehicleTypeEntity = await _context.VehicleTypes
                    .FindAsync(vehicle.VehicleTypeId);

                if (vehicleTypeEntity == null)
                    return false;

                // Sprawdzenie, czy kolumna jest dostępna
                if (!await IsColumnAvailableAsync(column, vehicleTypeEntity.Name))
                    return false;

                // Sprawdzenie dostępnych miejsc parkingowych
                var allowedRows = GetAllowedRows(vehicleTypeEntity.Name);
                var availableSpace = await GetAvailableParkingSpaceAsync(allowedRows, column, vehicleTypeEntity.SpacesRequired);

                if (availableSpace == null)
                    return false;

                // Tworzenie rezerwacji parkingowej
                var reservation = new ParkingReservation
                {
                    VehicleId = vehicle.Id,
                    UserId = userId,
                    ParkingSpaceId = availableSpace.Id,
                    StartTime = DateTime.Now,
                    IsActive = true
                };

                _context.ParkingReservations.Add(reservation);

                // Oznaczanie miejsc jako zajętych
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



        public async Task<bool> AddVehicleAsync(Vehicle vehicle, int column) // dodaje pojazd do systemu
        {
            try
            {
                // Sprawdzenie, czy pojazd już istnieje
                var existingVehicle = await _context.Vehicles
                    .FirstOrDefaultAsync(v => v.LicensePlate == vehicle.LicensePlate);

                if (existingVehicle != null)
                    return false;

                // Pobranie typu pojazdu
                var vehicleType = await _context.VehicleTypes
                    .FindAsync(vehicle.VehicleTypeId);

                if (vehicleType == null)
                    return false;

                // Sprawdzenie, czy kolumna jest dostępna
                if (!await IsColumnAvailableAsync(column, vehicleType.Name))
                    return false;

                // Pobranie dozwolonych rzędów dla tego typu pojazdu
                var allowedRows = GetAllowedRows(vehicleType.Name);
                var availableSpace = await GetAvailableParkingSpaceAsync(allowedRows, column, vehicleType.SpacesRequired);

                if (availableSpace == null)
                    return false;

                // Utworzenie pojazdu
                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();

                // Utworzenie rezerwacji parkingowej
                var reservation = new ParkingReservation
                {
                    VehicleId = vehicle.Id,
                    UserId = vehicle.UserId,
                    ParkingSpaceId = availableSpace.Id,
                    StartTime = DateTime.Now,
                    IsActive = true
                };

                _context.ParkingReservations.Add(reservation);

                // Oznaczenie miejsc jako zajętych
                await MarkSpacesAsOccupied(allowedRows, column, vehicleType.SpacesRequired);

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }



        public async Task<bool> RemoveVehicleAsync(string licensePlate) // Usuwa pojazd z systemu na podstawie numeru rejestracyjnego
        {
            try
            {
                // Sprawdzenie, czy pojazd istnieje
                var vehicle = await _context.Vehicles
                    .Include(v => v.VehicleType)
                    .Include(v => v.ParkingReservations)
                        .ThenInclude(pr => pr.ParkingSpace)
                    .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);

                // Jeśli pojazd nie istnieje, zwróć false
                if (vehicle == null)
                    return false;

                var activeReservation = vehicle.ParkingReservations // Sprawdzenie aktywnej rezerwacji
                    .FirstOrDefault(pr => pr.IsActive);

                if (activeReservation != null)
                {
                    // Zwolnienie miejsc parkingowych
                    var allowedRows = GetAllowedRows(vehicle.VehicleType.Name);
                    await MarkSpacesAsAvailable(allowedRows, activeReservation.ParkingSpace.Column, vehicle.VehicleType.SpacesRequired); // Oznaczenie miejsc jako dostępnych

                    _context.ParkingReservations.Remove(activeReservation); // Usunięcie aktywnej rezerwacji
                }

                await _context.SaveChangesAsync(); // Zapisanie zmian w bazie danych
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Vehicle?> FindVehicleAsync(string licensePlate) // Znajduje pojazd na podstawie numeru rejestracyjnego
        {
            return await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.User)
                .Include(v => v.ParkingReservations.Where(pr => pr.IsActive))
                    .ThenInclude(pr => pr.ParkingSpace)
                .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate);
        }

        public async Task<string> SearchVehicleAsync(string licensePlate) // Wyszukuje pojazd na podstawie numeru rejestracyjnego i zwraca informacje o nim
        {
            var vehicle = await FindVehicleAsync(licensePlate);

            if (vehicle == null)
                return "Pojazd o podanym numerze rejestracyjnym nie został znaleziony.";

            var activeReservation = vehicle.ParkingReservations.FirstOrDefault(pr => pr.IsActive);  // Sprawdzenie aktywnej rezerwacji

            if (activeReservation == null)
                return "Pojazd nie jest obecnie zaparkowany.";

            var duration = DateTime.Now - activeReservation.StartTime;
            var durationText = $"{(int)duration.TotalHours}h {duration.Minutes}m";

            return $"📋 Typ: {vehicle.VehicleType.Name}\n" +
                   $"📍 Kolumna: {activeReservation.ParkingSpace.Column}\n" +
                   $"⏰ Zaparkowany: {activeReservation.StartTime:dd.MM.yyyy HH:mm}\n" +
                   $"⌚ Czas parkowania: {durationText}";
        }

        public async Task<List<string>> GetAllLicensePlatesAsync()  // Pobiera wszystkie numery rejestracyjne pojazdów, które mają aktywne rezerwacje parkingowe
        {
            return await _context.Vehicles
                .Where(v => v.ParkingReservations.Any(pr => pr.IsActive))
                .Select(v => v.LicensePlate)
                .ToListAsync();
        }

        public async Task<List<int>> GetAvailableColumnsAsync(string vehicleType) // Pobiera dostępne kolumny dla danego typu pojazdu
        {
            var occupiedColumns = await GetOccupiedColumnsAsync(vehicleType);     // Pobranie zajętych kolumn dla danego typu pojazdu
            var availableColumns = new List<int>();

            for (int col = 0; col < MaxColumns; col++)
            {
                if (!occupiedColumns.Contains(col)) // Sprawdzenie, czy kolumna nie jest zajęta
                {
                    availableColumns.Add(col);      // Dodanie dostępnej kolumny do listy
                }
            }
            return availableColumns;
        }

        public async Task<bool> IsColumnAvailableAsync(int column, string vehicleType) // Sprawdza, czy kolumna jest dostępna dla danego typu pojazdu
        {
            var occupiedColumns = await GetOccupiedColumnsAsync(vehicleType); // Pobranie zajętych kolumn dla danego typu pojazdu
            return !occupiedColumns.Contains(column); //
        }

        // public async Task<int> GetAvailableParkingSpacesAsync()  // Zwraca liczbę dostępnych miejsc parkingowych
        // {
        //     var totalSpaces = MaxRows * MaxColumns;
        //     var occupiedSpaces = await _context.ParkingSpaces
        //         .CountAsync(ps => ps.IsOccupied);

        //     return totalSpaces - occupiedSpaces;
        // }

        // public async Task<int> GetOccupiedParkingSpacesAsync()
        // {
        //     return await _context.ParkingSpaces
        //         .CountAsync(ps => ps.IsOccupied);
        // }

        public async Task<List<ParkingReservation>> GetActiveReservationsAsync()  // Pobiera listę aktywnych rezerwacji parkingowych
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

        // public void DisplayParkingLayout(string vehicleType, int startRow, int rowCount)  //
        // {
        //     // Implementation for console display - can be empty for WPF app
        //     System.Console.WriteLine($"Parking layout for {vehicleType}");
        // }


        private async Task<List<int>> GetOccupiedColumnsAsync(string vehicleType) // Pobiera listę zajętych kolumn dla danego typu pojazdu
        {
            var allowedRows = GetAllowedRows(vehicleType);

            return await _context.ParkingSpaces
                .Where(ps => allowedRows.Contains(ps.Row) && ps.IsOccupied)
                .Select(ps => ps.Column)
                .Distinct()
                .ToListAsync();
        }

        private async Task<ParkingSpace?> GetAvailableParkingSpaceAsync(List<int> allowedRows, int column, int spacesRequired) // Pobiera dostępne miejsce parkingowe w określonej kolumnie i rzędach
        {
            return await _context.ParkingSpaces
                .FirstOrDefaultAsync(ps => allowedRows.Contains(ps.Row) &&
                                         ps.Column == column &&
                                         !ps.IsOccupied);
        }

        // Metoda do oznaczania miejsc parkingowych jako zajętych
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

        // Metoda do oznaczania miejsc parkingowych jako dostępnych
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

        // Metoda do pobierania dozwolonych rzędów dla danego typu pojazdu 
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