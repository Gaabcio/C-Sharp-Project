   M S B A M L   `   `   `  ����   ParkingManagementSystem8    2ParkingManagementSystem.Views.ManageVehiclesWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Zarządzanie pojazdami��$	��743��$	��914�� � WindowStartupLocation$ CenterOwner=����	#FF393E46.��� $��20q� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0�����	#FF222831 �� Padding$ 15q� �� CornerRadius$ 5��$��0,0,0,20q�._��� $��Zarządzanie pojazdami��$��18(�$
��Bold"����	#FF00ADB5$��Center=�� $��1�� � ColumnDefinitions �� $��* ��� $��20 ��� $	��300 �.+��� $��0�����	#FF222831$ 20q�$ 5��._��� .+��� $��Twoje pojazdy:��$��16(�$
��Bold"����	#FF00ADB5$��0,0,0,15q�&   System.Windows.Controls.DataGrid  -     Name$ VehiclesDataGrid��   AutoGenerateColumns$ False�����	#FF393E46
�������   GridLinesVisibility$ 
Horizontal=�	   HeadersVisibility$	 Column=�
   Columns
 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Nr rejestracyjny��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 120 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *LicensePlate+  $	 Typ��$	 100  �� *VehicleType.Name+  $ Marka��$	 100  �� *Brand+  $ Model��$	 100  �� *Model+  $ Kolor��$ 80  �� *Color+��  �� Orientation$ 
Horizontal=�$��Center=�$��0,20,0,0q�.+��� -   $��Nowy pojazd��$	��100��$��35�����	#FF28A745
�������$��0q�$��0,0,10,0q��� -   $��Edytuj��$��80��$��35�����	#FF17A2B8
�������$��0q�$��0,0,10,0q��� -   $��Usuń��$��80��$��35�����	#FFDC3545
�������$��0q��� $��2�����	#FF222831$ 20q�$ 5��._��� -    ��Name$ VehicleFormPanel��.+��� -    ��Name$ FormTitleTextBlock��$��Dodaj nowy pojazd��$��16(�$
��Bold"����	#FF00ADB5$��0,0,0,20q��� $��Numer rejestracyjny:��
�������$��0,0,0,5q��� -    ��Name$ LicensePlateTextBox��$��35��$��14(�$��0,0,0,15q� �� CharacterCasing$ Upper=� �� 	MaxLength$ 20���� $��Typ pojazdu:��
�������$��0,0,0,5q��� -    ��Name$ VehicleTypeComboBox��$��35��$��14(�$��0,0,0,15q� �� DisplayMemberPath$
 Name�� �� SelectedValuePath$ Id���� $��Marka:��
�������$��0,0,0,5q��� -	   $ BrandTextBox��$��35��$��14(�$��0,0,0,15q�$ 50���� $��Model:��
�������$��0,0,0,5q��� -
   $ ModelTextBox��$��35��$��14(�$��0,0,0,15q�$ 50���� $��Kolor:��
�������$��0,0,0,5q��� -   $ ColorTextBox��$��35��$��14(�$��0,0,0,15q�$ 20���� $��Rok produkcji:��
�������$��0,0,0,5q��� -   $ YearTextBox��$��35��$��14(�$��0,0,0,20q�$ 4���� -   $ ValidationTextBlock��$
��Wrap=�
���  ��$��0,0,0,15q�$y�	Collapsed=��� $ 
Horizontal=�$��Center=�.+��� -    ��Name$ 
SaveButton��$��Zapisz��$��80��$��35�����	#FF28A745
�������$��0q�$��0,0,10,0q��� -   $��Anuluj��$��80��$��35�����	#FF6C757D
�������$��0q��� -   $��2��$��Zamknij��$	��100��$��40�����	#FF6C757D
�������$��0q�$��Center=�$��0,20,0,0q�