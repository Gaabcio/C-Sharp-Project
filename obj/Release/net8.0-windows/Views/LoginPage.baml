   M S B A M L   `   `   `  ����   ParkingManagementSystem-    'ParkingManagementSystem.Views.LoginPage   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       N� 
Background  �	#FF393E46.���  � RowDefinitions �� $��* ��� $
��Auto ��� $��* �.+��� $��1��$��Center=�$��Center=�$	��400�����	#FF222831$��20q�.+���  �� Padding$ 40q�._��� .+���  �� Orientation$ 
Horizontal=�$��Center=�$��0,0,0,30q�.+��� $��/Views/icons8-key-50.png��$��50��$��50��$��0,0,10,0q��� $��Logowanie do systemu��$��18(�$
��Bold"����	#FF00ADB5$��Center=��� $��Nazwa użytkownika:��
�������$��0,0,0,5q��� -    ��Name$ UsernameTextBox��$��35��$��14(�$��0,0,0,15q�$��5q����	#FF00ADB5$��2q��� $��Hasło:��
�������$��0,0,0,5q�G� -    G�Name$ PasswordBox��$��35��$��14(�$��0,0,0,25q�$��5q����	#FF00ADB5$��2q��� -    ��Name$ ErrorTextBlock��
���  ��$
��Wrap=�$��0,0,0,15q�$y�	Collapsed=��  � ColumnDefinitions �� $��* ��� $��10 ��� $��* �.+��� -   $��0��$��Wyjście��$��40�����	#FF6C757D
�������$��0q�$��14(��� -   $��2��$��Zaloguj��$��40�����	#FF00ADB5
�������$��0q�$��14(� �� 	IsDefault$
 True��