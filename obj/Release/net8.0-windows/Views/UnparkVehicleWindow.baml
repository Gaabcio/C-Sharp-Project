   M S B A M L   `   `   `  ����   ParkingManagementSystem7    1ParkingManagementSystem.Views.UnparkVehicleWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Wyparkuj pojazd��$	��300��$	��550�� � WindowStartupLocation$ CenterOwner=����	#FF393E46.��� $��20q� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0�����	#FF222831 �� Padding$ 15q� �� CornerRadius$ 5��$��0,0,0,20q�._��� $��Wyparkuj pojazd��$��18(�$
��Bold"����	#FF00ADB5$��Center=��� $��1�����	#FF222831$ 20q�$ 5��$��0,0,0,55q�$��2��._��� .+��� $/��)Wybierz pojazd który chcesz wyparkować:��
�������$��0,0,0,15q��� -    ��Name$ VehicleComboBox��$��40��$��14(� �� DisplayMemberPath$ LicensePlate��$��0,0,0,20q��� -    ��Name$ VehicleDetailsPanel�����	#FF393E46$ 15q�$ 5��$y�	Collapsed=�._��� -    ��Name$ VehicleDetailsTextBlock��
�������$
��Wrap=��� -   $ MessageTextBlock��$
��Wrap=�$��0,15,0,0q�$
��Bold"��� $��2��	 �� Orientation$	 
Horizontal=�$��Center=�$��0,20,0,0q�.+��� -   $��Anuluj��$	��100��$��40�����	#FF6C757D
�������$��0q�$��0,0,10,0q��� -   $��Wyparkuj��$	��100��$��40�����	#FFDC3545
�������$��0q�