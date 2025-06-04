using ParkingManagementSystem.Models;
using ParkingManagementSystem.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ParkingManagementSystem.Views
{
    public partial class LoginPage : Page
    {
        private readonly IUserService _userService;
        public event Action<User>? LoginSuccessful;

        public LoginPage(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
            
            // Set focus to username textbox
            Loaded += (s, e) => UsernameTextBox.Focus();
            
            // Handle Enter key in password box
            PasswordBox.KeyDown += async (s, e) =>
            {
                if (e.Key == Key.Enter)
                    await AttemptLogin();
            };
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await AttemptLogin();
        }

        private async Task AttemptLogin()
        {
            try
            {
                HideError();
                
                string username = UsernameTextBox.Text.Trim();
                string password = PasswordBox.Password;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ShowError("Podaj nazwę użytkownika i hasło");
                    return;
                }

                var user = await _userService.AuthenticateAsync(username, password);
                
                if (user != null)
                {
                    LoginSuccessful?.Invoke(user);
                }
                else
                {
                    ShowError("Nieprawidłowa nazwa użytkownika lub hasło");
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                //ShowError($"Błąd logowania: {ex.Message}");
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.Visibility = Visibility.Visible;
        }

        private void HideError()
        {
            ErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void ClearFields()
        {
            UsernameTextBox.Text = "";
            PasswordBox.Password = "";
            UsernameTextBox.Focus();
        }
    }
}