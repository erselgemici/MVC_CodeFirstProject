using MyAcademy_MVC_CodeFirst.Data.Entities;
using System.Data.Entity;

namespace MyAcademy_MVC_CodeFirst.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
