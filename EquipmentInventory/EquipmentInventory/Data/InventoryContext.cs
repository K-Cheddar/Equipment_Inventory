using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EquipmentInventory.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipmentInventory.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }

        public DbSet<Equipment> Equipment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Equipment>().ToTable("Equipment");
        }
    }
}
