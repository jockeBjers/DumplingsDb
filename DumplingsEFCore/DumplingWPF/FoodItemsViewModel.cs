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


    public class FoodItemsViewModel : INotifyPropertyChanged
    {
        private readonly PubContext context;
        public ObservableCollection<MenuItem> FoodItems { get; set; }

        public FoodItemsViewModel(PubContext _context)
        {
            context = _context;
            FoodItems = new ObservableCollection<MenuItem>();
            GetFoodItems();
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

        public void AddFoodItem(string name, string description, decimal price)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(name) || price <= 0)
            {
                throw new ArgumentException("Name cannot be empty, and price must be greater than zero.");
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




        /* SEARCH AND UPDATE*/

        public bool SearchFoodItem()
        {
            if (string.IsNullOrWhiteSpace(SearchName))
            {
                throw new ArgumentException("Search term cannot be empty.");
            }

            var item = context.MenuItems.FirstOrDefault(d =>
                d.Name.Equals(SearchName.ToLower()) && d.Category == "Food");

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
                d.Name.Equals(SearchName.ToLower()) && d.Category == "Food");


            if (item != null)
            {
                item.Name = EditName;
                item.Description = EditDescription;
                item.Price = EditPrice;

                context.SaveChanges();
                GetFoodItems(); 
            }
            else
            {
                throw new InvalidOperationException("Food item not found for update.");
            }

        }

        public bool RemoveFoodItem()
        {
            
            var item = context.MenuItems.FirstOrDefault(d =>
                d.Name.Equals(SearchName.ToLower()) && d.Category == "Food");

            if (item != null)
            {
                context.MenuItems.Remove(item);  
                context.SaveChanges();           
                GetFoodItems();                  
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