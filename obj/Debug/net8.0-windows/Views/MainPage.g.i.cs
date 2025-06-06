﻿#pragma checksum "..\..\..\..\Views\MainPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5DB982CEA9BCD5A3DAEC2929FB468CA07B057744"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ParkingManagementSystem.Views {
    
    
    /// <summary>
    /// MainPage
    /// </summary>
    public partial class MainPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 33 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox VehicleTypeComboBox;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LicensePlateTextBox;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ColumnComboBox;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox UserVehiclesComboBox;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock MessageTextBlock;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid ParkingGrid;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ParkedVehiclesListBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.5.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ParkingManagementSystem;component/views/mainpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\MainPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.VehicleTypeComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 34 "..\..\..\..\Views\MainPage.xaml"
            this.VehicleTypeComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.VehicleTypeComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LicensePlateTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.ColumnComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            
            #line 49 "..\..\..\..\Views\MainPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ParkButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.UserVehiclesComboBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 55 "..\..\..\..\Views\MainPage.xaml"
            this.UserVehiclesComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.UserVehiclesComboBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.MessageTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.ParkingGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.ParkedVehiclesListBox = ((System.Windows.Controls.ListBox)(target));
            return;
            case 9:
            
            #line 96 "..\..\..\..\Views\MainPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SearchButton_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 100 "..\..\..\..\Views\MainPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.UnparkButton_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 104 "..\..\..\..\Views\MainPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ManageVehiclesButton_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 108 "..\..\..\..\Views\MainPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.LogoutButton_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 112 "..\..\..\..\Views\MainPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ExitButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

