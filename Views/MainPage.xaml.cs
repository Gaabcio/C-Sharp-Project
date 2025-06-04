using Microsoft.Extensions.DependencyInjection;
using ParkingManagementSystem.Models;
using ParkingManagementSystem.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParkingManagementSystem.Views
{
    public partial class MainPage : Page
    {
        private readonly IParkingService _parkingService;
        private readonly IVehicleService _vehicleService;
        private readonly IServiceProvider _serviceProvider;
        private User? _currentUser;

        public event Action? LogoutRequested;

        public MainPage(IParkingService parkingService, IVehicleService vehicleService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _parkingService = parkingService;
            _vehicleService = vehicleService;
            _serviceProvider = serviceProvider;

            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadVehicleTypes();
            await LoadUserVehicles();
            await RefreshParkingStatus();
        }

        public void SetCurrentUser(User? user)
        {
            _currentUser = user;
        }

        private async Task LoadVehicleTypes()
        {
            try
            {
                var vehicleTypes = await _vehicleService.GetAllVehicleTypesAsync();
                VehicleTypeComboBox.ItemsSource = vehicleTypes;
                VehicleTypeComboBox.DisplayMemberPath = "Name";
                VehicleTypeComboBox.SelectedValuePath = "Id";

                if (vehicleTypes.Any())
                {
                    VehicleTypeComboBox.SelectedIndex = 0;
                    await LoadAvailableColumns(vehicleTypes.First().Name);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd ładowania typów pojazdów: {ex.Message}", Colors.Red);
            }
        }

        private async Task LoadAvailableColumns(string vehicleTypeName)
        {
            try
            {
                var availableColumns = await _parkingService.GetAvailableColumnsAsync(vehicleTypeName);
                ColumnComboBox.ItemsSource = availableColumns;

                if (availableColumns.Any())
                    ColumnComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                //ShowMessage($"Błąd ładowania dostępnych kolumn: {ex.Message}", Colors.Red);
            }
        }

        private async void ParkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentUser == null) return;

                string licensePlate = LicensePlateTextBox.Text.Trim().ToUpper();

                if (string.IsNullOrEmpty(licensePlate))
                {
                    ShowMessage("Podaj numer rejestracyjny", Colors.Red);
                    return;
                }

                if (VehicleTypeComboBox.SelectedItem is not VehicleType selectedType)
                {
                    ShowMessage("Wybierz typ pojazdu", Colors.Red);
                    return;
                }

                if (ColumnComboBox.SelectedItem is not int selectedColumn)
                {
                    ShowMessage("Wybierz kolumnę", Colors.Red);
                    return;
                }

                // Check if vehicle exists or create new one
                var vehicles = await _vehicleService.GetUserVehiclesAsync(_currentUser.Id);
                var vehicle = vehicles.FirstOrDefault(v => v.LicensePlate == licensePlate);

                if (vehicle == null)
                {

                    // Create new vehicle
                    vehicle = new Vehicle
                    {
                        LicensePlate = licensePlate,
                        UserId = _currentUser.Id,
                        VehicleTypeId = selectedType.Id
                    };

                    if (!await _vehicleService.CreateVehicleAsync(vehicle))
                    {
                        ShowMessage("Błąd tworzenia pojazdu. Upewnij się że wprowadzono poprawny numer rejestracyjny", Colors.Red);
                        return;
                    }

                    // Get the created vehicle with ID
                    vehicles = await _vehicleService.GetUserVehiclesAsync(_currentUser.Id);
                    vehicle = vehicles.FirstOrDefault(v => v.LicensePlate == licensePlate);
                }

                if (vehicle != null)
                {
                    bool success = await _parkingService.ParkVehicleAsync(vehicle.LicensePlate, vehicle.VehicleType.Name, selectedColumn, _currentUser.Id);

                    if (success)
                    {
                        ShowMessage($"Zaparkowano pojazd: {licensePlate}", Colors.Green);
                        LicensePlateTextBox.Text = "";
                        await RefreshParkingStatus();
                        await LoadAvailableColumns(selectedType.Name);
                    }
                    else
                    {
                        ShowMessage("Błąd parkowania pojazdu. Sprawdź poprawność numeru rejestracji.", Colors.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd: {ex.Message}", Colors.Red);
            }
        }


        // Refreshes the parking status by updating the parking grid, parked vehicles list, and user vehicles.
        private async Task RefreshParkingStatus()
        {
            try
            {
                await UpdateParkingGrid(); // Update the parking grid with current reservations
                await UpdateParkedVehiclesList(); // Update the list of parked vehicles
                await LoadUserVehicles(); // Reload user vehicles to ensure the latest data is displayed
            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd odświeżania statusu: {ex.Message}", Colors.Red);
            }
        }

        private async Task UpdateParkingGrid() // Updates the parking grid with the current parking layout and active reservations.
        {
            ParkingGrid.Children.Clear();
            ParkingGrid.RowDefinitions.Clear();
            ParkingGrid.ColumnDefinitions.Clear();

            // Create grid structure (7 rows x 10 columns)
            for (int i = 0; i < 8; i++) // +1 for header
                ParkingGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });

            for (int i = 0; i < 11; i++) // +1 for row labels
                ParkingGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(25) });

            // Add column headers
            for (int colHeader = 0; colHeader < 10; colHeader++)
            {
                var header = new TextBlock
                {
                    Text = colHeader.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold
                };
                Grid.SetRow(header, 0);
                Grid.SetColumn(header, colHeader + 1);
                ParkingGrid.Children.Add(header);
            }

            var activeReservations = await _parkingService.GetAllActiveReservationsAsync();

            for (int currentRow = 0; currentRow < 7; currentRow++)
            {
                // Row label
                var rowLabel = new TextBlock
                {
                    Text = GetRowLabel(currentRow),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    FontSize = 8
                };
                Grid.SetRow(rowLabel, currentRow + 1);
                Grid.SetColumn(rowLabel, 0);
                ParkingGrid.Children.Add(rowLabel);

                for (int currentColumn = 0; currentColumn < 10; currentColumn++)
                {
                    bool isOccupied = false;
                    string toolTipText = "Wolne";
                    ParkingReservation? occupyingReservation = null;

                    foreach (var reservation in activeReservations) // Check each active reservation to see if it occupies the current space
                    {
                        if (reservation.Vehicle != null && reservation.Vehicle.VehicleType != null &&
                            reservation.ParkingSpace.Column == currentColumn && // Check if the reservation is for the current column
                            currentRow >= reservation.ParkingSpace.Row &&    // Check if the current row is at or below the vehicle's starting row
                            currentRow < (reservation.ParkingSpace.Row + reservation.Vehicle.VehicleType.SpacesRequired)) // Check if the current row is within the vehicle's vertical span
                        {
                            isOccupied = true;
                            occupyingReservation = reservation;
                            break;
                        }
                    }

                    if (isOccupied && occupyingReservation != null)
                    {
                        if (occupyingReservation.Vehicle != null && occupyingReservation.Vehicle.VehicleType != null)
                        {
                            toolTipText = $"Zajęte przez {occupyingReservation.Vehicle.LicensePlate} ({occupyingReservation.Vehicle.VehicleType.Name})";
                        }
                        else
                        {
                            toolTipText = "Zajęte (brak danych pojazdu)";
                        }
                    }

                    var space = new Border
                    {
                        Width = 20,
                        Height = 20,
                        Background = isOccupied ? Brushes.Red : Brushes.Green,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1),
                        ToolTip = toolTipText
                    };

                    Grid.SetRow(space, currentRow + 1);
                    Grid.SetColumn(space, currentColumn + 1);
                    ParkingGrid.Children.Add(space);
                }
            }
        }

        private string GetRowLabel(int row)
        {
            return row switch
            {
                0 => "M",  // Motorcycles
                1 or 2 => "S",  // Cars (Samochody)
                3 or 4 or 5 or 6 => "A",  // Buses (Autobusy)
                _ => "?"
            };
        }

        // Updates the list of parked vehicles in the UI.
        private async Task UpdateParkedVehiclesList()
        {
            var reservations = await _parkingService.GetAllActiveReservationsAsync();
            var items = reservations.Select(r =>
                $"{r.Vehicle.LicensePlate} ({r.Vehicle.VehicleType.Name}) - Kolumna {r.ParkingSpace.Column}")
                .ToList();

            ParkedVehiclesListBox.ItemsSource = items;
        }

        // Loads the vehicles associated with the current user into the UserVehiclesComboBox.
        private async Task LoadUserVehicles()
        {
            if (_currentUser == null) return;

            try
            {
                var userVehicles = await _vehicleService.GetUserVehiclesAsync(_currentUser.Id);
                UserVehiclesComboBox.ItemsSource = userVehicles;
                UserVehiclesComboBox.DisplayMemberPath = "LicensePlate"; // Można dostosować, np. $"{LicensePlate} ({VehicleType.Name})" jeśli VehicleType jest załadowany
                UserVehiclesComboBox.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd ładowania pojazdów użytkownika: {ex.Message}", Colors.Red);
            }
        }

        // Handles the selection change event for the UserVehiclesComboBox.
        private async void UserVehiclesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserVehiclesComboBox.SelectedItem is Vehicle selectedVehicle)
            {
                LicensePlateTextBox.Text = selectedVehicle.LicensePlate;
                VehicleTypeComboBox.SelectedValue = selectedVehicle.VehicleTypeId;

                if (VehicleTypeComboBox.SelectedItem is VehicleType selectedType)
                {
                    await LoadAvailableColumns(selectedType.Name);
                }
            }
        }

        private void VehicleTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadAvailableColumns(VehicleTypeComboBox.SelectedItem is VehicleType selectedType ? selectedType.Name : string.Empty).ConfigureAwait(false);
        }

        // Opens a search window to find vehicles by license plate.
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchWindow = _serviceProvider.GetRequiredService<SearchVehicleWindow>();
            searchWindow.Owner = Window.GetWindow(this);
            searchWindow.ShowDialog();
        }

        // Opens a window to unpark a vehicle.
        private async void UnparkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var activeReservations = await _parkingService.GetAllActiveReservationsAsync();

                if (activeReservations == null || !activeReservations.Any())
                {
                    ShowMessage("Brak zaparkowanych pojazdów do wyparkowania.", Colors.OrangeRed);
                    return;
                }

                var unparkWindow = _serviceProvider.GetRequiredService<UnparkVehicleWindow>();
                unparkWindow.Owner = Window.GetWindow(this);
                unparkWindow.VehicleUnparked += async () => await RefreshParkingStatus();
                unparkWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd: {ex.Message}", Colors.Red);
            }
        }

        // Opens a window to manage vehicles, allowing the user to add, edit, or remove vehicles.
        private async void ManageVehiclesButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                ShowMessage("Brak zalogowanego użytkownika, aby zarządzać pojazdami.", Colors.Red);
                return;
            }

            // Create a new scope for the ManageVehiclesWindow
            using (var scope = _serviceProvider.CreateScope())
            {
                var manageWindow = scope.ServiceProvider.GetRequiredService<ManageVehiclesWindow>();
                manageWindow.SetCurrentUser(_currentUser); // Pass the current user
                manageWindow.Owner = Window.GetWindow(this);
                manageWindow.ShowDialog();
                await RefreshParkingStatus(); // Refresh data after the window is closed
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LogoutRequested?.Invoke();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ShowMessage(string message, Color color)
        {
            MessageTextBlock.Text = message;
            MessageTextBlock.Foreground = new SolidColorBrush(color);
        }
    }
}