using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Sales_Inventory.Models;
using Sales_Inventory.Repository.Implementations;
using Sales_Inventory.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<salesinventory_dbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbConn") ?? throw new InvalidOperationException("Connection string not found")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<salesinventory_dbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbConn")));
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
builder.Services.AddTransient<IReceivableRepository, ReceivableRepository>();
builder.Services.AddTransient<IReportRepository, ReportRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

//Session Config
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(s =>
{
    s.Cookie.Name = "UserSession";
    s.IdleTimeout = TimeSpan.FromHours(10);
    s.Cookie.IsEssential = true;
});

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

app.UseStaticFiles(new StaticFileOptions
{

    FileProvider = new PhysicalFileProvider(
        // Path.Combine(@"D:\worskpace\asp-net-core\sales-and-inevntory\Sales&Inventory\wwwroot", "reports")),
        Path.Combine(builder.Environment.WebRootPath, "reports")),
    RequestPath = "/reports"
});

app.UseRouting();

app.UseAuthorization();

app.UseSession();
app.Use(async (context, next) =>
{
    string session = context.Session.GetString("UserSession");

    if (!context.Request.Path.Value.Contains("/users/login"))
    {
        if (string.IsNullOrEmpty(session))
        {
            var path = $"/users/login";

            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = 401; //UnAuthorized
            context.Response.Redirect(path);
        }

    }
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
