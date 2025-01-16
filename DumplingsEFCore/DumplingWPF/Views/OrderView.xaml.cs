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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using DumplingWPF;
using publisherData;

namespace DumplingWPF
{
    public partial class OrderView : UserControl
    {
        private OrdersViewModel _viewModel;

        public OrderView()
        {
            InitializeComponent();
            _viewModel = new OrdersViewModel();
            this.DataContext = _viewModel;
        }

        /* Button to move item from incoming list to in progress */

        private void MoveToInProgress_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = (Order)IncomingOrdersListBox.SelectedItem;
            if (selectedOrder != null)
            {
                _viewModel.MoveToInProgress(selectedOrder);
            }
        }
        /* Method to handle double clicks to move an item from incoming list to in progress */
        private void IncomingOrdersListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedOrder = (Order)IncomingOrdersListBox.SelectedItem;
            if (selectedOrder != null)
            {
                _viewModel.MoveToInProgress(selectedOrder);
            }
        }

/* Removes the item from the list */
        private void MarkAsDone_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = (Order)InProgressOrdersListBox.SelectedItem;
            if (selectedOrder != null)
            {
                _viewModel.MarkAsDone(selectedOrder);
            }
        }

        /* Refreshes the list if something new has been added to the database.
         * To do: Look at other options, SignalR?
         */
        private void RefreshOrders_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadOrders();
        }

    }

}
