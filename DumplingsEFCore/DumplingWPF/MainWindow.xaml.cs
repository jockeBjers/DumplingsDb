﻿using System.Collections.ObjectModel;
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
        public MenuItemsViewModel FoodViewModel { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            context = new PubContext();
            FoodViewModel = new MenuItemsViewModel(context);
            DataContext = FoodViewModel;

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

        private void AddFoodItem_Click(object sender, RoutedEventArgs e)
        {
            string name = DishTextBox.Text;
            string description = DescriptionTextBox.Text;

            if (name.Any(char.IsDigit))
            {
                MessageBox.Show("Name cannot contain numbers. Please enter a valid name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                try
                {
                    FoodViewModel.AddFoodItem(name, description, price);

                    // Clear input fields
                    DishTextBox.Clear();
                    DescriptionTextBox.Clear();
                    PriceTextBox.Clear();

                    MessageBox.Show($"Maträtt {name} tillagd med pris: {price}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid price. Please enter a valid decimal value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddDrinkItem_Click(object sender, RoutedEventArgs e)
        {
            string name = DrinkTextBox.Text;
            string description = DrinkDescriptionTextBox.Text;

            if (name.Any(char.IsDigit))
            {
                MessageBox.Show("Name cannot contain numbers. Please enter a valid name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (decimal.TryParse(DrinkPriceTextBox.Text, out decimal price))
            {
                try
                {
                    FoodViewModel.AddDrinkItem(name, description, price);

                    // Clear input fields
                    DrinkTextBox.Clear();
                    DrinkDescriptionTextBox.Clear();
                    DrinkPriceTextBox.Clear();

                    MessageBox.Show($"Dryck {name} tillagd med pris: {price}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid price. Please enter a valid decimal value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CancelFoodButton_Click(object sender, RoutedEventArgs e)
        {
            DishTextBox.Clear();
            DescriptionTextBox.Clear();
            PriceTextBox.Clear();
            DrinkTextBox.Clear();
            DrinkDescriptionTextBox.Clear();
            DrinkPriceTextBox.Clear();
        }

        /* SEARCH AND UPDATE */
        private void SearchDish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var viewModel = (MenuItemsViewModel)DataContext;
                if (!viewModel.SearchFoodItem())
                {
                    MessageBox.Show("Food item not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Show a confirmation dialog to the user
                var result = MessageBox.Show(
                 "Are you sure you want to update this food item?",
                 "Confirm Update",
                 MessageBoxButton.YesNo,
                 MessageBoxImage.Question);
                var viewModel = (MenuItemsViewModel)DataContext;
                viewModel.UpdateFoodItem();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            searchTextBox.Clear();
            editNameTextBox.Clear();
            editDescriptionTextBox.Clear();
            editPriceTextBox.Clear();
            editDrinkNameTextBox.Clear();
            editDrinkDescriptionTextBox.Clear();
            editDrinkPriceTextBox.Clear();
        }

        /* REMOVE ITEM */
        private void RemoveFoodItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Show a confirmation dialog to the user
                var result = MessageBox.Show(
                    "Are you sure you want to remove this food item?",
                    "Confirm Removal",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    var viewModel = (MenuItemsViewModel)DataContext;

                    // Call the RemoveFoodItem method in the ViewModel
                    if (!viewModel.RemoveFoodItem())
                    {
                        MessageBox.Show("Food item not found or could not be removed.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }





    }
}
