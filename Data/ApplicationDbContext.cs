using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementMVC.Models;

namespace EmployeeManagementMVC.Data;

public class ApplicationDbContext : IdentityDbContext
{

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>()
            .HasMany(d => d.Employees)
            .WithOne(e => e.Department)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Employee>()
            .Property(e => e.Salary)
            .HasColumnType("decimal(18, 2)");

    }
    
}
