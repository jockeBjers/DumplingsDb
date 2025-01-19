using publisherData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MenuItem = publisherData.MenuItem;

namespace DumplingWPF
{
    public class MenuItemsViewModel : INotifyPropertyChanged
    {
        private readonly PubContext context;
        public ObservableCollection<MenuItem> FoodItems { get; set; }
        public ObservableCollection<MenuItem> DrinkItems { get; set; }

        public MenuItemsViewModel(PubContext _context)
        {
            context = _context;
            FoodItems = new ObservableCollection<MenuItem>();
            DrinkItems = new ObservableCollection<MenuItem>();
            GetFoodItems();
            GetDrinkItems();
        }


        void GetFoodItems()
        {
            var foodItems = context.MenuItems.Where(d => d.Category == "Food").ToList();
            FoodItems.Clear();
            foreach (var dish in foodItems)
            {
                FoodItems.Add(dish);
            }
        }

        void GetDrinkItems()
        {
            var drinkItems = context.MenuItems.Where(d => d.Category == "Drink").ToList();
            DrinkItems.Clear();
            foreach (var dish in drinkItems)
            {
                DrinkItems.Add(dish);
            }
        }

        public void AddFoodItem(string name, string description, decimal price)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(name) || price <= 0)
            {
                throw new ArgumentException("Namn måste vara ifyllt och pris större än noll.");
            }

            // Create a new food item
            var newFoodItem = new MenuItem
            {
                Name = name,
                Description = description,
                Price = price,
                Category = "Food"
            };

            context.MenuItems.Add(newFoodItem);
            context.SaveChanges();


            FoodItems.Add(newFoodItem);
        }

        public void AddDrinkItem(string name, string description, decimal price)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(name) || price <= 0)
            {
                throw new ArgumentException("Namn måste vara ifyllt och pris större än noll.");
            }

            // Create a new food item
            var newDrinkItem = new MenuItem
            {
                Name = name,
                Description = description,
                Price = price,
                Category = "Drink"
            };

            context.MenuItems.Add(newDrinkItem);
            context.SaveChanges();


            DrinkItems.Add(newDrinkItem);
        }




        /* SEARCH AND UPDATE*/

        public bool SearchFoodItem()
        {
            if (string.IsNullOrWhiteSpace(SearchName))
            {
                throw new ArgumentException("Sökfältet kan inte vara tomt.");
            }

            var item = context.MenuItems.FirstOrDefault(d =>
                d.Name.Equals(SearchName.ToLower()) );

            if (item != null)
            {
                EditName = item.Name;
                EditDescription = item.Description;
                EditPrice = item.Price;
                return true;
            }
            else
            {
                EditName = string.Empty;
                EditDescription = string.Empty;
                EditPrice = 0;
            }

            return false;
        }

        public void UpdateFoodItem()
        {
            var item = context.MenuItems.FirstOrDefault(d =>
                d.Name.Equals(SearchName.ToLower()) );


            if (item != null)
            {
                item.Name = EditName;
                item.Description = EditDescription;
                item.Price = EditPrice;

                context.SaveChanges();
                GetFoodItems();
                GetDrinkItems();
            }
            else
            {
                throw new InvalidOperationException("Föremål kan inte hittas.");
            }

        }

        public bool RemoveFoodItem()
        {

            var item = context.MenuItems.FirstOrDefault(d =>
                d.Name.Equals(SearchName.ToLower()) );

            if (item != null)
            {
                context.MenuItems.Remove(item);
                context.SaveChanges();
                GetFoodItems();
                GetDrinkItems();
                return true;
            }
            else
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string SearchName { get; set; }
        private string editName;
        public string EditName
        {
            get => editName;
            set
            {
                editName = value;
                OnPropertyChanged(nameof(EditName));
            }
        }

        private string editDescription;
        public string EditDescription
        {
            get => editDescription;
            set
            {
                editDescription = value;
                OnPropertyChanged(nameof(EditDescription));
            }
        }

        private decimal editPrice;
        public decimal EditPrice
        {
            get => editPrice;
            set
            {
                editPrice = value;
                OnPropertyChanged(nameof(EditPrice));
            }
        }
    }
}