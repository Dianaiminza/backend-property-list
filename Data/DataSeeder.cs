using backend_property_list.DatabaseSeeds.Abstractions;
using backend_property_list.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public class DataSeeder : IDatabaseSeeder
{
    private readonly DatabaseContext _context;

    public DataSeeder(DatabaseContext context)
    {
        _context = context;
    }

    public int Priority => 1;

    public void Initialize()
    {
        // Apply any pending migrations to ensure the database is up-to-date
        _context.Database.Migrate();

        // Call GenerateFakeProperties to seed the database
        SeedDatabase();
    }

    public void SeedDatabase(int numberOfRecords = 100)
    {
        try
        {
            
            if (!_context.Properties.Any())
            {
                var fakeProperties = GenerateFakeProperties(numberOfRecords);
                _context.Properties.AddRange(fakeProperties); // Add fake data
                _context.SaveChanges(); // Save to the database
                Console.WriteLine($"Successfully seeded {numberOfRecords} properties.");
            }
            else
            {
                Console.WriteLine("Properties already exist. Skipping seeding.");
            }
        }
        catch (Exception ex)
        {
            // Catch any errors during seeding
            Console.WriteLine($"Error during seeding: {ex.Message}");
        }
    }

    private IQueryable<RePropertyModel> GenerateFakeProperties(int count)
    {
        // Generate fake data for properties
        var properties = Enumerable.Range(1, count)
            .Select(_ => new RePropertyModel
            {
                PropertyName = Faker.Lorem.Sentence(),  // Fake property name
                PropertyType = Faker.Company.Name(),    // Fake property type
                StatusCode = Faker.RandomNumber.Next(1, 4).ToString() // Fake status code (between 1 and 3)
            })
            .AsQueryable();

        return properties;
    }
}
