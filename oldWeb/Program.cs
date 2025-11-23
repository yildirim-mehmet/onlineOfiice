using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Bizim servisler
builder.Services.AddScoped<OnlineOfficeWeb.Services.WordRenderService>();
builder.Services.AddScoped<OnlineOfficeWeb.Services.ExcelRenderService>();
builder.Services.AddScoped<OnlineOfficeWeb.Services.PdfRenderService>();
builder.Services.AddScoped<OnlineOfficeWeb.Services.PptRenderService>();
builder.Services.AddScoped<OnlineOfficeWeb.Services.VisioRenderService>();

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

app.Run();
