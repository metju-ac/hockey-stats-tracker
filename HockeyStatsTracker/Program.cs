using DB;
using DB.Utils;
using HockeyStatsTracker.Components;
using HockeyStatsTracker.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddServerComponents();

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000") });


// Add the MatchesController
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddServerRenderMode();

using (var context = new DatabaseContext())
{
    context.Database.EnsureCreated();
    DatabaseSeeder.ResetDatabase(context);
}


app.MapControllers();

app.Run();
