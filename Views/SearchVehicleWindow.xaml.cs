using ParkingManagementSystem.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ParkingManagementSystem.Views
{
    public partial class SearchVehicleWindow : Window
    {
        private readonly IParkingService _parkingService;

        public SearchVehicleWindow(IParkingService parkingService)
        {
            InitializeComponent();
            _parkingService = parkingService;
            
            Loaded += (s, e) => LicensePlateTextBox.Focus();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            await PerformSearch();
        }

        private async void LicensePlateTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                await PerformSearch();
        }

        private async Task PerformSearch()
        {
            try
            {
                string licensePlate = LicensePlateTextBox.Text.Trim().ToUpper();
                
                if (string.IsNullOrEmpty(licensePlate))
                {
                    ResultTextBlock.Text = "Podaj numer rejestracyjny";
                    return;
                }

                var vehicle = await _parkingService.FindVehicleAsync(licensePlate);
                
                if (vehicle != null)
                {
                    var activeReservation = vehicle.ParkingReservations.FirstOrDefault(pr => pr.IsActive);
                    
                    if (activeReservation != null)
                    {
                        ResultTextBlock.Text = $"✅ POJAZD ZNALEZIONY\n\n" +
                                             $"📋 Typ: {vehicle.VehicleType.Name}\n" +
                                             $"🚗 Nr rejestracyjny: {vehicle.LicensePlate}\n" +
                                             $"📍 Kolumna: {activeReservation.ParkingSpace.Column}\n" +
                                             $"⏰ Zaparkowany: {activeReservation.StartTime:dd.MM.yyyy HH:mm}\n" +
                                             $"👤 Właściciel: {vehicle.User.FirstName} {vehicle.User.LastName}";
                        
                        if (!string.IsNullOrEmpty(vehicle.Brand) || !string.IsNullOrEmpty(vehicle.Model))
                        {
                            ResultTextBlock.Text += $"\n🚙 Pojazd: {vehicle.Brand} {vehicle.Model}";
                        }
                        
                        if (!string.IsNullOrEmpty(vehicle.Color))
                        {
                            ResultTextBlock.Text += $"\n🎨 Kolor: {vehicle.Color}";
                        }
                    }
                    else
                    {
                        ResultTextBlock.Text = $"Pojazd {licensePlate} istnieje w systemie, ale nie jest obecnie zaparkowany.";
                    }
                }
                else
                {
                    ResultTextBlock.Text = $"❌ Pojazd o numerze rejestracyjnym '{licensePlate}' nie został znaleziony w systemie.";
                }
            }
            catch (Exception ex)
            {
                ResultTextBlock.Text = $"Błąd wyszukiwania: {ex.Message}";
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}