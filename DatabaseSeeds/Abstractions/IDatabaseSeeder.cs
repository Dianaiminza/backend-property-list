namespace backend_property_list.DatabaseSeeds.Abstractions;

public interface IDatabaseSeeder
{
    int Priority { get; }
    void Initialize();
}
