using Microsoft.EntityFrameworkCore;

namespace ServerSidePaginationApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    //// Seed 10 million records using random data
        //    var random = new Random();
        //    var employees = new Employee[10_000_00];
        //    for (int i = 0; i < employees.Length; i++)
        //    {
        //        employees[i] = new Employee
        //        {
        //            Id = -(i + 1), // Use negative Id values to avoid conflicts with non-seed data
        //            Name = $"Employee {i + 1}",
        //            Position = $"Position {random.Next(1, 5)}",
        //            Office = $"Office {random.Next(1, 10)}",
        //            Salary = random.Next(40_000, 120_000)
        //        };
        //    }
        //    modelBuilder.Entity<Employee>().HasData(employees);
        //}
    }
}
