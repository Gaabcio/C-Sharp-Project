   M S B A M L   `   `   `  ����  ParkingManagementSystem7    1ParkingManagementSystem.Views.UnparkVehicleWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Wyparkuj pojazd��5   	   $	��331��6!   $	��550��6.    � WindowStartupLocation$ CenterOwner=�5   	   ���	#FF393E46.��5      � $��20q�6    � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   ���	#FF222831 �� Padding$ 15q�65    �� CornerRadius$ 5��6B   $��0,0,0,20q�6S   ._�5      �� $��Wyparkuj pojazd��6   $��18(�6/   $
��Bold"�6=   ���	#FF00ADB5$��Center=�5   .   5      �� 5   
   $��1��6   ���	#FF222831$ 20q�65   $ 5��6B   $��0,0,0,55q�6S   $��2��6e   ._�5      �� .+�5      �� $/��)Wybierz pojazd który chcesz wyparkować:��6   
�������$��0,0,0,15q�5   .   �� 5      -   5         ��Name$ VehicleComboBox��5      $��30��64   $��14(�6@   $	��440��5      �� 5      -   5         ��Name$ VehicleDetailsPanel��5      ���	#FF393E46$ 15q�5       $ 5��6%   $y�	Collapsed=�66   ._�5!      �� -   5         ��Name$ VehicleDetailsTextBlock��5!       
���   �
���   �$
��Wrap=�5"      5#      �� 5&      -   5        $ MessageTextBlock��5&      $
��Wrap=�66   $��0,15,0,0q�5'      $
��Bold"�6-   5(      5)      �� 5,   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   $��0,20,0,0q�5-      .+�5.      �� -   5        $��Anuluj��5.      $	��100��6&   $��40��62   ���	#FF6C757D
�������$��0q�5/   >   $��0,0,10,0q�50      6&   �� 52      -   5        $��Wyparkuj��52      $	��100��6(   $��40��64   ���	#FFDC3545
�������$��0q�53   >   54      55      56      57      