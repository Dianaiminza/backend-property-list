using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Data;

namespace backend_property_list.Models;

public class DatabaseContext : DbContext
{

    public DbContextOptions<DatabaseContext> Options { get; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
          : base(options)
    {

        Options = options;
    }

    public DbSet<RePropertyModel> Properties { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);




        builder.Entity<RePropertyModel>(entity =>
        {
            entity.Property(e => e.Id)
              .ValueGeneratedOnAdd();
        });
        AddEnumConstraints(builder);
        SetDecimalDefaultPrecision(builder);
    }


    private static void AddEnumConstraints(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.GetProperties();

            foreach (var property in properties)
            {
                var nullableSubType = Nullable.GetUnderlyingType(property.ClrType);
                var propertyType = nullableSubType ?? property.ClrType;

                if (propertyType.IsEnum)
                {
                    var enumValues = Enum.GetValues(propertyType).Cast<int>().ToList();
                    var enumValuesString = string.Join(", ", enumValues);
                    var tableName = entityType.GetTableName();

                    modelBuilder
                        .Entity(entityType.ClrType)
                        .HasCheckConstraint(
                            $"CK_{entityType.GetTableName()}_{property.GetColumnName(StoreObjectIdentifier.Table(tableName))}",
                            $"\"{property.GetColumnName(StoreObjectIdentifier.Table(tableName))}\" IN ({enumValuesString})");
                }
            }
        }
    }

    private static void SetDecimalDefaultPrecision(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(13,2)");
        }
    }


}
