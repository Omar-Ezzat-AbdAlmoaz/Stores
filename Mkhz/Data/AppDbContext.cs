using Mkhz.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Mkhz.Data
{
    public class AppDbContext:IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Product> products { get; set; }
        public DbSet<Invoice> invoices { get; set; }
        public DbSet<ProductWithQuantity> productWithQuantities { get; set; }
        public DbSet<Expens> Expens { get; set; }
    }
}
