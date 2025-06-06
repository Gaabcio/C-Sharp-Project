using FluentValidation;
using ParkingManagementSystem.Models;
using ParkingManagementSystem.Services;
using ParkingManagementSystem.Validators;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParkingManagementSystem.Views
{
    public partial class ManageVehiclesWindow : Window
    {
        private readonly IVehicleService _vehicleService;
        private readonly IParkingService _parkingService; 
        private readonly VehicleValidator _vehicleValidator;
        private User? _currentUser;
        private Vehicle? _editingVehicle;


        public ManageVehiclesWindow(IVehicleService vehicleService, IParkingService parkingService, VehicleValidator vehicleValidator)
        {
            InitializeComponent();
            _vehicleService = vehicleService;
            _parkingService = parkingService;
            _vehicleValidator = vehicleValidator;

            Loaded += ManageVehiclesWindow_Loaded;
        }

        public void SetCurrentUser(User user)
        {
            _currentUser = user;
        }

        private async void ManageVehiclesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadVehicleTypes();
            await LoadUserVehicles();
            ClearForm();
        }

        private async Task LoadVehicleTypes() // £aduje dostêpne typy pojazdów do ComboBox
        {
            try
            {
                var vehicleTypes = await _vehicleService.GetAllVehicleTypesAsync();
                VehicleTypeComboBox.ItemsSource = vehicleTypes;

                if (vehicleTypes.Any())
                    VehicleTypeComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ShowValidationMessage($"B³¹d ³adowania typów pojazdów: {ex.Message}", Colors.Red);
            }
        }

        private async Task LoadUserVehicles() // £aduje pojazdy u¿ytkownika do DataGrid
        {
            try
            {
                if (_currentUser == null) return;

                var vehicles = await _vehicleService.GetUserVehiclesAsync(_currentUser.Id);
                VehiclesDataGrid.ItemsSource = vehicles;
            }
            catch (Exception ex)
            {
                ShowValidationMessage($"B³¹d ³adowania pojazdów: {ex.Message}", Colors.Red);
            }
        }

        private void VehiclesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) // Obs³uguje zmianê zaznaczenia w DataGrid
        {
            if (VehiclesDataGrid.SelectedItem is Vehicle selectedVehicle)
            {
                LoadVehicleToForm(selectedVehicle);
            }
        }

        private void LoadVehicleToForm(Vehicle vehicle)  // Wczytuje dane pojazdu do formularza edycji
        {
            _editingVehicle = vehicle;
            FormTitleTextBlock.Text = "Edytuj pojazd";

            LicensePlateTextBox.Text = vehicle.LicensePlate;
            VehicleTypeComboBox.SelectedValue = vehicle.VehicleTypeId;
            BrandTextBox.Text = vehicle.Brand ?? "";
            ModelTextBox.Text = vehicle.Model ?? "";
            ColorTextBox.Text = vehicle.Color ?? "";
            YearTextBox.Text = vehicle.Year?.ToString() ?? "";

            SaveButton.Content = "Aktualizuj";
        }

        private void NewVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private async void DeleteVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VehiclesDataGrid.SelectedItem is not Vehicle selectedVehicle)
                {
                    ShowValidationMessage("Wybierz pojazd do usuniêcia", Colors.Orange);
                    return;
                }

                // SprawdŸ, czy pojazd jest aktualnie zaparkowany
                var activeReservations = await _parkingService.GetAllActiveReservationsAsync();
                bool isParked = activeReservations.Any(r => r.VehicleId == selectedVehicle.Id && r.IsActive);

                if (isParked)
                {
                    ShowValidationMessage($"Pojazd {selectedVehicle.LicensePlate} jest aktualnie zaparkowany i nie mo¿e zostaæ usuniêty. Najpierw wyparkuj pojazd.", Colors.Red);
                    return;
                }

                var result = MessageBox.Show(
                    $"Czy na pewno chcesz usun¹æ pojazd {selectedVehicle.LicensePlate}?",
                    "Potwierdzenie usuniêcia",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    bool success = await _vehicleService.DeleteVehicleAsync(selectedVehicle.Id);

                    if (success)
                    {
                        ShowValidationMessage($"Usuniêto pojazd: {selectedVehicle.LicensePlate}", Colors.Green);
                        await LoadUserVehicles();
                        ClearForm();
                    }
                    else
                    {
                        ShowValidationMessage("B³¹d usuwania pojazdu. SprawdŸ, czy pojazd nie jest powi¹zany z aktywnymi rezerwacjami.", Colors.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowValidationMessage($"B³¹d: {ex.Message}", Colors.Red);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentUser == null)
                {
                    ShowValidationMessage("Brak zalogowanego u¿ytkownika", Colors.Red);
                    return;
                }

                // Stwórz nowy obiekt Vehicle na podstawie danych z formularza
                var vehicle = new Vehicle
                {
                    Id = _editingVehicle?.Id ?? 0,
                    LicensePlate = LicensePlateTextBox.Text.Trim().ToUpper(),
                    UserId = _currentUser.Id,
                    VehicleTypeId = (int)(VehicleTypeComboBox.SelectedValue ?? 0),
                    Brand = string.IsNullOrWhiteSpace(BrandTextBox.Text) ? null : BrandTextBox.Text.Trim(),
                    Model = string.IsNullOrWhiteSpace(ModelTextBox.Text) ? null : ModelTextBox.Text.Trim(),
                    Color = string.IsNullOrWhiteSpace(ColorTextBox.Text) ? null : ColorTextBox.Text.Trim(),
                    Year = int.TryParse(YearTextBox.Text, out int year) ? year : null
                };

                // Walidacja pojazdu
                var validationResult = await _vehicleValidator.ValidateAsync(vehicle);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join("\n", validationResult.Errors.Select(e => e.ErrorMessage));
                    ShowValidationMessage(errors, Colors.Red);
                    return;
                }

                // Zapis lub aktualizacja pojazdu
                bool success;
                if (_editingVehicle == null)
                {
                    // Create new vehicle
                    success = await _vehicleService.CreateVehicleAsync(vehicle);
                    if (success)
                    {
                        ShowValidationMessage($"Dodano pojazd: {vehicle.LicensePlate}", Colors.Green);
                    }
                }
                else
                {
                    // Aktualizacja istniej¹cego pojazdu
                    vehicle.Id = _editingVehicle.Id;
                    success = await _vehicleService.UpdateVehicleAsync(vehicle);
                    if (success)
                    {
                        ShowValidationMessage($"Zaktualizowano pojazd: {vehicle.LicensePlate}", Colors.Green);
                    }
                }

                if (success)
                {
                    await LoadUserVehicles();
                    ClearForm();
                }
                else
                {
                    if (_editingVehicle != null && !success) 
                    { 
                        ShowValidationMessage("B³¹d zapisywania pojazdu", Colors.Red);
                    }
                    else if (_editingVehicle == null && !success) 
                    {
                        ShowValidationMessage("B³¹d tworzenia pojazdu", Colors.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowValidationMessage($"B³¹d zapisu: {ex.Message}{(ex.InnerException != null ? "\nSzczegó³y: " + ex.InnerException.Message : "")}", Colors.Red);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClearForm()
        {
            _editingVehicle = null;
            FormTitleTextBlock.Text = "Dodaj nowy pojazd";

            LicensePlateTextBox.Text = "";
            BrandTextBox.Text = "";
            ModelTextBox.Text = "";
            ColorTextBox.Text = "";
            YearTextBox.Text = "";

            if (VehicleTypeComboBox.Items.Count > 0)
                VehicleTypeComboBox.SelectedIndex = 0;

            SaveButton.Content = "Zapisz";
            HideValidationMessage();
        }

        private void ShowValidationMessage(string message, Color color)
        {
            ValidationTextBlock.Text = message;
            ValidationTextBlock.Foreground = new SolidColorBrush(color);
            ValidationTextBlock.Visibility = Visibility.Visible;
        }

        private void HideValidationMessage()
        {
            ValidationTextBlock.Visibility = Visibility.Collapsed;
        }

        private void VehicleTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}