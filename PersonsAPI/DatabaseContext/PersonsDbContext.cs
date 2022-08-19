using System.Diagnostics;
using EmployeesAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeesAPI.DatabaseContext;

public class PersonsDbContext : DbContext
{
    public DbSet<Employee> Persons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationManager();

        config.AddJsonFile("appsettings.json");

        optionsBuilder.UseMySql(config.GetConnectionString("default"), new MySqlServerVersion(new Version(8, 0, 29)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>();
        modelBuilder.Entity<Employee>().Property(e => e.Age).HasColumnName("Age");
        modelBuilder.Entity<Employee>().Property(e => e.Password).HasColumnName("Company");
        modelBuilder.Entity<Employee>().Property(e => e.Email).HasColumnName("Email");
        modelBuilder.Entity<Employee>().Property(e => e.Id).HasColumnName("Id");
        modelBuilder.Entity<Employee>().Property(e => e.FirstName).HasColumnName("FirstName");
        modelBuilder.Entity<Employee>().Property(e => e.LastName).HasColumnName("LastName");
        
    }
}