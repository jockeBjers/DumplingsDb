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
                MessageBox.Show("Ange ett korrekt namn", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (decimal.TryParse(priceTextBox, out decimal price))
            {
                try
                {
                    ViewModel.AddFoodItem(name, description, price);
                    clearFields?.Invoke();
                    MessageBox.Show($"Föremål {name} tillagd med pris: {price}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Ogiltigt pris. Var god och ange korrekt värde", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show("Kunde inte hitta föremål.");
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
                    "Är du säker på att du vill uppdatera detta föremål?",
                    "Bekräfta uppdatering",
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
                    "Är du säker på att du vill ta bort detta föremål?",
                    "Bekräfta borttagelse",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    if (!ViewModel.RemoveFoodItem())
                    {
                        MessageBox.Show("Föremål kunde inte hittas eller tas bort.");
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
