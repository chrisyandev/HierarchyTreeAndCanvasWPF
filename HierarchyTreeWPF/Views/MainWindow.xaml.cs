﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HierarchyTreeWPF.Extensions;
using HierarchyTreeWPF.Models;
using HierarchyTreeWPF.ViewModels;

namespace HierarchyTreeWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CanvasLeftClicked(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("canvas clicked");

            Canvas canvas = sender as Canvas;

            (DataContext as MainWindowViewModel).AddShape(canvas);
        }
    }
}
