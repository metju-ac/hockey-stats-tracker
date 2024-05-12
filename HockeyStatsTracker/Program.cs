using DB;
using DB.Utils;
using HockeyStatsTracker.Components;
using HockeyStatsTracker.Controllers;
using Microsoft.AspNetCore.Components.Authorization;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddServerComponents();

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000") });

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());
// builder.Services.AddAuthorizationCore();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.AccessDeniedPath = "/auth/access-denied";
        options.LoginPath = "/login";
    });

builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
// app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddServerRenderMode();

// using (var context = new DatabaseContext())
// {
    // context.Database.EnsureCreated();
    // DatabaseSeeder.ResetDatabase(context);
// }


app.MapControllers();

app.Run();
