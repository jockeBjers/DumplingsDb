using publisherData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace DumplingWPF
{
    public class MenuItemViewBase : UserControl
    {
        public readonly PubContext context;
        public MenuItemsViewModel ViewModel;

        public MenuItemViewBase()
        {
            context = new PubContext();
        }

        protected void AddMenuItem(string name, string description, string priceTextBox, Action clearFields)
        {
            if (name.Any(char.IsDigit))
            {
                MessageBox.Show("Name cannot contain numbers. Please enter a valid name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (decimal.TryParse(priceTextBox, out decimal price))
            {
                try
                {
                    ViewModel.AddFoodItem(name, description, price);
                    clearFields?.Invoke();
                    MessageBox.Show($"Item {name} added with price: {price}");
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

        protected void ClearFields(params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                textBox.Clear();
            }
        }

        protected void SearchItem()
        {
            try
            {
                if (!ViewModel.SearchFoodItem())
                {
                    MessageBox.Show("Item not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        protected void SaveChanges(params TextBox[] textBoxesToClear)
        {
            try
            {
                var result = MessageBox.Show(
                    "Are you sure you want to update this item?",
                    "Confirm Update",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ViewModel.UpdateFoodItem();
                    ClearFields(textBoxesToClear);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        protected void RemoveItem()
        {
            try
            {
                var result = MessageBox.Show(
                    "Are you sure you want to remove this item?",
                    "Confirm Removal",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    if (!ViewModel.RemoveFoodItem())
                    {
                        MessageBox.Show("Item not found or could not be removed.");
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
