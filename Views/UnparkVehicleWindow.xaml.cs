using ParkingManagementSystem.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParkingManagementSystem.Views
{
    public partial class UnparkVehicleWindow : Window
    {
        private readonly IParkingService _parkingService;
        public event Action? VehicleUnparked;

        public UnparkVehicleWindow(IParkingService parkingService)
        {
            InitializeComponent();
            _parkingService = parkingService;

            Loaded += UnparkVehicleWindow_Loaded;
        }

        private async void UnparkVehicleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadParkedVehicles();
        }

        private async Task LoadParkedVehicles() // Załadowanie pojazdow zaparkowanych
        {
            try
            {
                var licensePlates = await _parkingService.GetAllLicensePlatesAsync();

                if (licensePlates.Any())
                {
                    VehicleComboBox.ItemsSource = licensePlates;

                    if (licensePlates.Any())
                        VehicleComboBox.SelectedIndex = 0;
                }
                else
                {
                    ShowMessage("Brak pojazdów do wyparkowania", Colors.Orange);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd ładowania pojazdów: {ex.Message}", Colors.Red);
            }
        }

        private async void VehicleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (VehicleComboBox.SelectedItem is string selectedLicensePlate)
                {
                    var vehicleInfo = await _parkingService.SearchVehicleAsync(selectedLicensePlate);

                    if (!string.IsNullOrEmpty(vehicleInfo))
                    {
                        VehicleDetailsTextBlock.Text = vehicleInfo;
                        VehicleDetailsPanel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        VehicleDetailsPanel.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    VehicleDetailsPanel.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd ładowania szczegółów pojazdu: {ex.Message}", Colors.Red);
                VehicleDetailsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private async void UnparkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (VehicleComboBox.SelectedItem is not string licensePlate)
                {
                    ShowMessage("Wybierz pojazd do wyparkowania", Colors.Red);
                    return;
                }

                bool success = await _parkingService.RemoveVehicleAsync(licensePlate);

                if (success)
                {
                    ShowMessage($"Wyparkowano pojazd: {licensePlate}", Colors.Green);

                    VehicleUnparked?.Invoke();

                    await Task.Delay(1500);
                    Close();
                }
                else
                {
                    ShowMessage("Błąd wyparkowania pojazdu", Colors.Red);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Błąd: {ex.Message}", Colors.Red);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowMessage(string message, Color color)
        {
            MessageTextBlock.Text = message;
            MessageTextBlock.Foreground = new SolidColorBrush(color);
        }
    }
}