   M S B A M L   `   `   `  ����   ParkingManagementSystem,    &ParkingManagementSystem.Views.MainPage   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       N� 
Background  �	#FF393E46$	��800��$	��990��$	��990��$	��801��.��� $��20q� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��$3��-Główne menu systemu zarządzania parkingiem��$��19(�$
��Bold"����	#FF00ADB5$��Center=�$��0,0,0,20q�� $��1�� � ColumnDefinitions �� $��* ��� $��20 ��� $��* �.+��� $��0�����	#FF222831 �� Padding$ 20q� �� CornerRadius$ 5��._��� .+��� $��Zarządzanie pojazdami��$��16(�$
��Bold"����	#FF00ADB5$��0,0,0,20q��� $��Typ pojazdu:��
�������$��0,0,0,5q��� -    ��Name$ VehicleTypeComboBox��$��35��$��14(�$��0,0,0,15q��� $��Numer rejestracyjny:��
�������$��0,0,0,5q��� -    ��Name$ LicensePlateTextBox��$��35��$��14(�$��0,0,0,15q� �� CharacterCasing$ Upper=� �� 	MaxLength$ 20���� $��Numer kolumny:��
�������$��0,0,0,5q��� -   $ ColumnComboBox��$��35��$��14(�$��0,0,0,20q��� -   $��Zaparkuj pojazd��$��40�����	#FF28A745
�������$��0q�$��14(��� -   	 ��Name$	 MessageTextBlock��$
��Wrap=�$��0,10,0,0q�$
��Bold"��� $��2�����	#FF222831$ 20q�$ 5��._��� .+��� $��Status parkingu��$��16(�$
��Bold"����	#FF00ADB5$��0,0,0,20q��� $	��300��$
��Auto=�.��� -   
 �Name$
 ParkingGrid��$��0,0,0,20q��� $��Zaparkowane pojazdy:��
�������$��0,0,0,10q��� -    ��Name$ ParkedVehiclesListBox��$	��190�����	#FF393E46
�������$
��Auto=�$	��190���� $��2�� �� Orientation$ 
Horizontal=�$��Center=�$��0,20,0,0q�.+��� -   $��Wyszukaj pojazd��$	��130��$��40��$��0,0,10,0q����	#FF17A2B8
�������$��0q��� -	   $��Wyparkuj pojazd��$	��135��$��40��$��0,0,10,0q����	#FFDC3545
�������$��0q��� -
   $��Zarządzaj pojazdami��$	��155��$��40��$��0,0,10,0q����	#FF6F42C1
�������$��0q��� -   $��Wyloguj��$��80��$��40��$��0,0,10,0q����	#FF6C757D
�������$��0q��� -   $��Wyjście��$��80��$��40�����	#FF6C757D
�������$��0q�