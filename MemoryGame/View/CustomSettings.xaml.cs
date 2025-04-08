﻿using MemoryGame.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MemoryGame.View
{
    /// <summary>
    /// Interaction logic for CustomSettings.xaml
    /// </summary>
    public partial class CustomSettings : Window
    {
        public CustomSettings()
        {
            InitializeComponent();

            this.DataContext = new CustomSettingsView(App.SharedMenuCommands);
        }
    }
}
