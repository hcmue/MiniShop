using MiniShop.Web.Middlewares;
using MiniShop.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Config logging (.NET 10)
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Services.AddLogging();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IProductService, FakeProductService>(); // Scoped lifetime

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

app.UseMiddleware<ErrorHandlingMiddleware>(); // Trước UseRouting
app.UseMiddleware<RequestTimingMiddleware>(); // Trước UseRouting


app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
