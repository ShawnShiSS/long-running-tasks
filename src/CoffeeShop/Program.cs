using CoffeeShop.Interfaces;
using CoffeeShop.Services;

// Add services to the DI container
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Register our orders queue
builder.Services.AddSingleton<IOrdersQueue, OrdersQueueService>();

// Register our coffee maker service
builder.Services.AddHostedService<CoffeeMakerService>();

// Register signalR hubs for real time communication
builder.Services.AddSignalR();


// Configure the HTTP request pipeline.
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map SignalR hub 
app.MapHub<FrontdeskService>("/frontdesk");

app.Run();
