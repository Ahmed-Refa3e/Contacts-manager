using Microsoft.EntityFrameworkCore;

namespace Entities;

public class ContactsDbContext(DbContextOptions<ContactsDbContext> options) : DbContext(options)
{

    public DbSet<Country> Countries { get; set; }
    public DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>().ToTable("Countries");
        modelBuilder.Entity<Person>().ToTable("Persons");

        //Seed to Countries
        string countriesJson = System.IO.File.ReadAllText("countries.json");
        List<Country>? countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

        foreach (Country country in countries!)
            modelBuilder.Entity<Country>().HasData(country);
    }

}
