   M S B A M L   `   `   `  ����  ParkingManagementSystem-    'ParkingManagementSystem.Views.LoginPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        N� 
Background  �	#FF393E46.��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5	      $
��Auto �6   �� 5
      $��* �6   .+�5   
   �� $��1��6   $��Center=�6"   $��Center=�6?   $	��400��5      ���	#FF222831$��20q�67   .+�5      ��  �� Padding$ 40q�6   ._�5      �� .+�5      ��  �� Orientation$ 
Horizontal=�6!   $��Center=�6:   $��0,0,0,30q�6W   .+�5      �� $��/Views/icons8-key-50.png��6    $��50��6B   $��50��6M   $��0,0,10,0q�6Y   �� 5      $��Logowanie do systemu��6$   $��18(�5   #   $
��Bold"�61   ���	#FF00ADB5$��Center=�5   #   5      �� 5      $��Nazwa użytkownika:��6    
�������$��0,0,0,5q�6M   �� 5      -   5         ��Name$ UsernameTextBox��5      $��35��67   $��14(�6C   $��0,0,0,15q�6Q   $��5q�5      ���	#FF00ADB5$��2q�6A   �� 5      $��Hasło:��6    
�������$��0,0,0,5q�6A   G� 5       -   5         G�Name$ PasswordBox��5    "   $��35��67   $��14(�6C   $��0,0,0,25q�6Q   $��5q�5!       ���	#FF00ADB5$��2q�6D   �� 5$      -   5         ��Name$ ErrorTextBlock��5$       
���  ��$
��Wrap=�5%      $��0,0,0,15q�63   $y�	Collapsed=�5&      � 5)       � ColumnDefinitions 5*      �� 5+      $��* �6/   �� 5,      $��10 �6/   �� 5-      $��* �6/   .+�50      �� -   5        $��0��50   !   $��Wyjście��61   $��40��6C   ���	#FF6C757D
�������$��0q�52       $��14(�64   53       �� 55      -   5        $��2��55   !   $��Zaloguj��61   $��40��6C   ���	#FF00ADB5
�������$��0q�57       $��14(�64    �� 	IsDefault$
 True��58   :   59      5:      5;      5<      5=      5>      