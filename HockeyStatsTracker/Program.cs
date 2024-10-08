using DB;
using DB.Utils;
using HockeyStatsTracker.Components;
using HockeyStatsTracker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddServerComponents();

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000") });

builder.Services.AddControllers();

builder.Services.AddSingleton<AuthService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
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