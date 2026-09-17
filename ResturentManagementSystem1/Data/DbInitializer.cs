using ResturentManagementSystem1.Models;
using System.Linq;

namespace ResturentManagementSystem1.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Gourmet Burgers", Price = 350, ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?auto=format&fit=crop&w=500&q=80" },
                    new Category { Name = "Crispy Chicken Bucket", Price = 450, ImageUrl = "https://images.unsplash.com/photo-1626645738196-c2a7c87a8f58?auto=format&fit=crop&w=500&q=80" },
                    new Category { Name = "Artisan Sandwiches", Price = 290, ImageUrl = "https://images.unsplash.com/photo-1528735602780-2552fd46c7af?auto=format&fit=crop&w=500&q=80" },
                    new Category { Name = "Arleen Combos", Price = 410, ImageUrl = "https://images.unsplash.com/photo-1555396273-367ea4eb4db5?auto=format&fit=crop&w=500&q=80" },
                    new Category { Name = "Sweet Treats Bakery", Price = 220, ImageUrl = "https://images.unsplash.com/photo-1578985545062-69928b1d9587?auto=format&fit=crop&w=500&q=80" },
                    new Category { Name = "Fresh Bowls & Platters", Price = 330, ImageUrl = "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?auto=format&fit=crop&w=500&q=80" }
                );
                context.SaveChanges();
            }

            if (!context.Combos.Any())
            {
                context.Combos.AddRange(
                    new Combo { Name = "Family Mega Feast", Subtitle = "2 Large Pizzas + 4 Burgers + 2L Soda", Price = 1850, ImageUrl = "https://images.unsplash.com/photo-1555396273-367ea4eb4db5?auto=format&fit=crop&w=600&q=80" },
                    new Combo { Name = "Burger Buddy Pack", Subtitle = "2 Double Burgers + Large Fries + 2 Cokes", Price = 750, ImageUrl = "https://images.unsplash.com/photo-1594212699903-ec8a3eca50f5?auto=format&fit=crop&w=600&q=80" },
                    new Combo { Name = "Crispy Chicken Box", Subtitle = "8 pcs Fried Chicken + Wedges + Dip", Price = 990, ImageUrl = "https://images.unsplash.com/photo-1527477396000-e27163b481c2?auto=format&fit=crop&w=600&q=80" }
                );
                context.SaveChanges();
            }
        }
    }
}