using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YourNamespace;
using YourNamespace.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // Add MVC services
builder.Services.AddHttpClient<NewsService>(); // Register NewsService with HttpClient
builder.Services.AddSingleton<HttpCache>(); // Register HttpCache service as singleton

// Register the HttpClientFactory
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) // Check if environment is not development
{
    app.UseExceptionHandler("/Home/Error"); // Use custom error page
    app.UseHsts(); // Use HTTP Strict Transport Security (HSTS)
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseStaticFiles(); // Serve static files (like HTML, CSS, JS)

app.UseRouting(); // Enable routing

app.UseAuthorization(); // Enable authorization

// Map default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map routing for the proxy controller
app.MapControllerRoute(
    name: "proxy",
    pattern: "api/{controller=Proxy}/{*url}"); // Route to Proxy controller

app.Run(); // Execute the request pipeline
