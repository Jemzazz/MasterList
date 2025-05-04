using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MasterList.Data;
using MasterList.Services;
using OfficeOpenXml;
// Set license context for EPPlus 6.x
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MasterListContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MasterListContext") ?? throw new InvalidOperationException("Connection string 'MasterListContext' not found.")));

builder.WebHost.UseUrls("http://*:$PORT");
// Add services to the container.
builder.Services.AddControllersWithViews();


// Register your Excel services
builder.Services.AddScoped<ExcelReaderService>();
builder.Services.AddScoped<ExcelWriterService>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MLists}/{action=Index}/{id?}");

app.Run();
