using publisherData;
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
using MenuItem = publisherData.MenuItem;

namespace DumplingWPF
{
    /// <summary>
    /// Interaction logic for DrinkView.xaml
    /// </summary>
    public partial class DrinkView : MenuItemViewBase
    {
        public DrinkView()
        {
            InitializeComponent();
            ViewModel = new MenuItemsViewModel(context);
            DataContext = ViewModel;
        }

        private void AddDrinkItem_Click(object sender, RoutedEventArgs e)
        {
            AddMenuItem(DrinkTextBox.Text, DrinkDescriptionTextBox.Text, DrinkPriceTextBox.Text,
                () => ClearFields(DrinkTextBox, DrinkDescriptionTextBox, DrinkPriceTextBox));
        }

        private void CancelDrinkButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields(DrinkTextBox, DrinkDescriptionTextBox, DrinkPriceTextBox);
        }

        private void SearchDish_Click(object sender, RoutedEventArgs e)
        {
            SearchItem();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges(editDrinkNameTextBox, editDrinkDescriptionTextBox, editDrinkPriceTextBox);
        }

        private void RemoveDrinkItem_Click(object sender, RoutedEventArgs e)
        {
            RemoveItem();
        }
    }

}
