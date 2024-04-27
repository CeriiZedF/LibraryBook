using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ShopLibrary.Models;

namespace ShopLibrary.DAL.Data
{
    public class DataContext : IdentityDbContext
    {
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<ShopUser> ShopUsers { get; set; } = null!;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}
