using EmployeeEquipmentCheckoutSystem.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeEquipmentCheckoutSystem.Core
{
    /// <summary>
    /// EF Core DbContext for working with Employee Checkouts.
    /// </summary>
    public class CheckoutContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ICheckable> Equipment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(""); // TODO: Connection String
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ICheckable>()
                .HasKey(c => c.SerialNumber);
        }
    }
}