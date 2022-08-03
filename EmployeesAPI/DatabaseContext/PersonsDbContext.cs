using System.Diagnostics;
using EmployeesAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeesAPI.DatabaseContext;

public class PersonsDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationManager();

        config.AddJsonFile("appsettings.json");

        optionsBuilder.UseMySql(config.GetConnectionString("default"), new MySqlServerVersion(new Version(8, 0, 29)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>();
        modelBuilder.Entity<Person>().Property(e => e.Age).HasColumnName("Age");
        modelBuilder.Entity<Person>().Property(e => e.Password).HasColumnName("Company");
        modelBuilder.Entity<Person>().Property(e => e.Email).HasColumnName("Email");
        modelBuilder.Entity<Person>().Property(e => e.Id).HasColumnName("Id");
        modelBuilder.Entity<Person>().Property(e => e.FirstName).HasColumnName("FirstName");
        modelBuilder.Entity<Person>().Property(e => e.LastName).HasColumnName("LastName");
        
    }
}