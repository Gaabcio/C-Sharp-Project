   M S B A M L   `   `   `  ����  ParkingManagementSystem7    1ParkingManagementSystem.Views.SearchVehicleWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Wyszukiwarka pojazdu��5   	   $	��500��6&   $	��600��63    � WindowStartupLocation$ CenterOwner=�5   	   ���	#FF393E46.��5      � $��20q�6    � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   ���	#FF222831 �� Padding$ 15q�65    �� CornerRadius$ 5��6B   $��0,0,0,20q�6S   ._�5      �� $��Wyszukiwarka pojazdu��6   $��18(�64   $
��Bold"�6B   ���	#FF00ADB5$��Center=�5   .   5      �� 5   
   $��1��6   ���	#FF222831$ 20q�65   $ 5��6B   $��0,0,0,53q�6S   $��2��6e   ._�5      �� .+�5      �� $7��1Znajdź swój pojazd podając numer rejestracyjny��6   
�������$��0,0,0,20q�5   .    �� TextAlignment$ Center=�6@   � 5      $��0,0,0,20q�6    � ColumnDefinitions 5      �� 5      $
��Auto �6+   �� 5      $��* �6+   �� 5      $
��Auto �6+   .+�5"      �� $��0��6    $��Numer rejestracyjny:��60   
�������$��Center=�5#   2   $��0,0,10,0q�6M   �� 5%      -   5         ��Name$ LicensePlateTextBox��5%   .   $��1��6   $��35��6K   $��14(�5&      $��5q�6+    �� CharacterCasing$ Upper=�67   	 �� 	MaxLength$	 20��6O   5'      �� 5)      -   5        $��2��5)      $��Wyszukaj��6-   $��80��6@   $��35��6K   ���	#FF00ADB5
�������$��0q�5*   F   $��10,0,0,0q�5+      6.   5,      �� 5/      ���	#FF393E46���	#FF00ADB5$��2q�6H   $ 5��50      $ 15q�6)   $	��150��66   ._�51      �� $
��Auto=�6#   .��52      �� -   5        
 ��Name$
 ResultTextBlock��52   $   $5��/Tutaj pojawi się informacja o Twoim pojeździe��53   #   
�������$
��Wrap=�54   6   55      56      57      58      �� 5;   
   -   5        $��2��5;      $��Zamknij��6   $	��100��60   $��40��6<   ���	#FF6C757D
�������$��0q�5<   :   $��Center=�5=      $��0,20,0,0q�6-   5>      5?      5@      