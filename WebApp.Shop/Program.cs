using DataLayer.Infrastructure;
using Microsoft.AspNetCore.Builder;
using WebApp.Shop.Neptons;
using WebApp.Shop.OdataControllers;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration).AddControllersWithViews().AddOdataConfig();
 
 
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}
app.UseReactCommunication();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default_arearoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapFallbackToFile("index.html");
app.Run();


