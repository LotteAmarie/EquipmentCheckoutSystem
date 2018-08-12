using Microsoft.EntityFrameworkCore;

namespace EmployeeEquipmentCheckoutSystem.Core.Data
{
    /// <summary>
    /// EF Core DbContext for working with Employee Checkouts.
    /// </summary>
    internal class CheckoutContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Equipment> Equipment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(""); // TODO: Connection String
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Equipment>()
                .HasKey(c => c.SerialNumber);
        }
    }
}