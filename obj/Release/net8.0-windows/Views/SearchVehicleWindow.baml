   M S B A M L   `   `   `  ����   ParkingManagementSystem7    1ParkingManagementSystem.Views.SearchVehicleWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Wyszukiwarka pojazdu��$	��479��$	��604�� � WindowStartupLocation$ CenterOwner=����	#FF393E46.��� $��20q� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0�����	#FF222831 �� Padding$ 15q� �� CornerRadius$ 5��$��0,0,0,20q�._��� $��Wyszukiwarka pojazdu��$��18(�$
��Bold"����	#FF00ADB5$��Center=��� $��1�����	#FF222831$ 20q�$ 5��$��0,0,0,53q�$��2��._��� .+��� $7��1Znajdź swój pojazd podając numer rejestracyjny��
�������$��0,0,0,20q� �� TextAlignment$ Center=�� $��0,0,0,20q� � ColumnDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��$��Numer rejestracyjny:��
�������$��Center=�$��0,0,10,0q��� -    ��Name$ LicensePlateTextBox��$��1��$��35��$��14(�$��10q� �� CharacterCasing$ Upper=�	 �� 	MaxLength$	 20���� -   $��2��$��Wyszukaj��$��80��$��35�����	#FF00ADB5
�������$��0q�$��10,0,0,0q��� ���	#FF393E46���	#FF00ADB5$��2q�$ 5��$ 15q�$	��150��._��� $
��Auto=�.���� -   
 ��Name$
 ResultTextBlock��$5��/Tutaj pojawi się informacja o Twoim pojeździe��
�������$
��Wrap=��� -   $��2��$��Zamknij��$	��100��$��40�����	#FF6C757D
�������$��0q�$��Center=�$��0,20,0,0q�