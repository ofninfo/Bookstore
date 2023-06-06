using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Bookstore.Models;

namespace Bookstore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ClientModel> Clients  { get; set; }
        public DbSet<BookModel> Books { get; set; }
        public DbSet<SaleItemModel> SaleItems  { get; set; }
        public DbSet<SaleModel> Sales  { get; set; }
    }
}