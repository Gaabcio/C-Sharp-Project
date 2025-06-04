using Microsoft.Extensions.DependencyInjection;
using ParkingManagementSystem.Models;
using ParkingManagementSystem.Views;
using System.Windows;

namespace ParkingManagementSystem
{
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private User? _currentUser;

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            ShowLoginPage();
        }

        public void ShowLoginPage()
        {
            var loginPage = _serviceProvider.GetRequiredService<LoginPage>();
            loginPage.LoginSuccessful += OnLoginSuccessful;
            MainFrame.Navigate(loginPage);
            UserTextBlock.Text = "";
        }

        public void ShowMainPage()
        {
            var mainPage = _serviceProvider.GetRequiredService<MainPage>();
            mainPage.LogoutRequested += OnLogoutRequested;
            mainPage.SetCurrentUser(_currentUser);
            MainFrame.Navigate(mainPage);
        }

        private void OnLoginSuccessful(User user)
        {
            _currentUser = user;
            UserTextBlock.Text = $"Zalogowany: {user.Username}";
            ShowMainPage();
            UpdateStatus("Zalogowano pomyślnie");
        }

        private void OnLogoutRequested()
        {
            _currentUser = null;
            ShowLoginPage();
            UpdateStatus("Wylogowano");
        }

        public void UpdateStatus(string message)
        {
            StatusTextBlock.Text = message;
        }
    }
}