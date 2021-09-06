using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductTypes> productTypes { get; set; }
        public DbSet<TagLists> tagLists { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderDetails> OrderDetailsorders { get; set; }

    }
}
