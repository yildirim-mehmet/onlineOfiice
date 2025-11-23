using OfficeIMO.Collaborative.Web.Hubs;
using OfficeIMO.Collaborative.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC + SignalR
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// Document session management + background cleanup
builder.Services.AddSingleton<DocumentSessionService>();
builder.Services.AddHostedService<FileCleanupService>();

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

app.MapHub<CollaborativeHub>("/hub");

app.Run();
