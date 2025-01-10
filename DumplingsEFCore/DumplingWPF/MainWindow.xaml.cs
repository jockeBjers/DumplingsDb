using System.Collections.ObjectModel;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using publisherData;
using MenuItem = publisherData.MenuItem;


namespace DumplingWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PubContext context;
        public FoodItemsViewModel FoodViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            context = new PubContext();
            FoodViewModel = new FoodItemsViewModel(context);
            DataContext = FoodViewModel;

            DataContext = this;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();  // Close
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {

                this.DragMove(); // drag window
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; // Minimizes
        }
    }
}