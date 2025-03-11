using DumplingsBlazor.Models;

namespace DumplingsBlazor.Components.Pages
{
    public partial class ItemMenu
    {

            private List<MenuItem> FoodItems = new()
            {
                new MenuItem { Id = 1, Name = "Pork Dumplings", Description = "classic dumplings", Price = 45, Category = "Food" },
                new MenuItem { Id = 2, Name = "Chicken Dumplings", Description = "Chicken and thai basil dumplings", Price = 45, Category = "Food" },
                new MenuItem { Id = 3, Name = "Shrimp Dumplings", Description = "shrimp dumplings", Price = 55, Category = "Food" },
                new MenuItem { Id = 4, Name = "Mushroom Dumplings", Description = "vegan alternative", Price = 45, Category = "Food" },
                new MenuItem { Id = 5, Name = "Spicy Pork Dumplings", Description = "a spicier alternative", Price = 45, Category = "Food" },
                new MenuItem { Id = 6, Name = "Karaage", Description = "Crispy fried chicken", Price = 55, Category = "Food" },
                new MenuItem { Id = 7, Name = "Spring Rolls", Description = "Delicious crispy rolls", Price = 45, Category = "Food" }
            };

            private List<MenuItem> DrinkItems = new()
            {
                new MenuItem { Id = 101, Name = "Coca Cola", Description = "Refreshing cola", Price = 25, Category = "Drink" },
                new MenuItem { Id = 102, Name = "Coca Cola Zero", Description = "Zero sugar cola", Price = 25, Category = "Drink" },
                new MenuItem { Id = 103, Name = "Sprite", Description = "Lemon-lime soda", Price = 25, Category = "Drink" },
                new MenuItem { Id = 104, Name = "Zingo", Description = "Fruity drink", Price = 25, Category = "Drink" },
                new MenuItem { Id = 105, Name = "Ice Tea", Description = "Chilled Ice Tea", Price = 28, Category = "Drink" }
            };
       /*  private List<MenuItem> FoodItems = new();
        private List<MenuItem> DrinkItems = new(); 
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var menuItems = await Http.GetFromJsonAsync<List<MenuItem>>("https://localhost:7172/api/menuitems");

                if (menuItems != null)
                {
                    FoodItems = menuItems.Where(m => m.Category == "Food").ToList();
                    DrinkItems = menuItems.Where(m => m.Category == "Drink").ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching menu items: {ex.Message}");
            }
        } */
    }
}


