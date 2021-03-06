﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestRunnerLib;
using System.Collections.ObjectModel;

namespace TestRunnerAppWpf
{
    
    public partial class DetailsView : UserControl
    {
        public DetailsView()
        {
            InitializeComponent();

            DetailsTabs.SelectedIndex = 3;


        }

        private void RowDetails_Toggle(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
        }

        private void ResultsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Collections.IList items = (System.Collections.IList)detailsGrid.SelectedItems;
            var itemsCast = items.Cast<RunModel>();
            model.selectedItems = new ObservableCollection<RunModel>(itemsCast);
        }

        private void CyclesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Collections.IList items = (System.Collections.IList)cyclesGrid.SelectedItems;
            var itemsCast = items.Cast<CycleModel>();
            model.selectedCycleItems = new ObservableCollection<CycleModel>(itemsCast);
        }

        //private void TabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    logTextBox.ScrollToEnd();
        //}

        private void LogTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            logTextBox.ScrollToEnd();
        }

        private void LogTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            logTextBox.ScrollToEnd();
        }

        private void LogTextBox_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            logTextBox.Clear();
        }

        private void CycleRunDataGridSingle_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }
    
        private void CycleRunDataGridSingle_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }


    }

    
}
