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
    /// Interaction logic for MenuView.xaml
    /// </summary>
    /// 
    public partial class FoodView : MenuItemViewBase
    {
        public FoodView()
        {
            InitializeComponent();
            ViewModel = new MenuItemsViewModel(context);
            DataContext = ViewModel;
        }

        private void AddFoodItem_Click(object sender, RoutedEventArgs e)
        {
            AddMenuItem(DishTextBox.Text, DescriptionTextBox.Text, PriceTextBox.Text,
                () => ClearFields(DishTextBox, DescriptionTextBox, PriceTextBox));
        }

        private void CancelFoodButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields(DishTextBox, DescriptionTextBox, PriceTextBox);
        }

        private void SearchDish_Click(object sender, RoutedEventArgs e)
        {
            SearchItem();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges(searchTextBox, editNameTextBox, editDescriptionTextBox, editPriceTextBox);
        }

        private void RemoveFoodItem_Click(object sender, RoutedEventArgs e)
        {
            RemoveItem();
        }
    }







}
