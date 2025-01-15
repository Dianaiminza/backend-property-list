

using backend_property_list.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDatabaseSeeders();
builder.Services.AddControllers();
builder.Services.AddCorsConfiguration();
builder.Services.AddDatabaseContext(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    // Default route configuration
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Property}/{action=PropertyList}/{id?}");

    
});

app.MigrateDatabase();
app.InitializeDatabaseData();

app.Run();
