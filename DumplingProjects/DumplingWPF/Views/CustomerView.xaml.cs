using DumplingWPF.ViewModels;
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

namespace DumplingWPF
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {

        private CustomerViewModel _viewModel;

        public CustomerView()
        {
            InitializeComponent();
            _viewModel = new CustomerViewModel();
            this.DataContext = _viewModel;
        }

        /* Refresh the customer list */
        private void RefreshOrders_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadCustomers();
        }

    }
}
