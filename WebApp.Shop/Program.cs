using Microsoft.AspNetCore.Builder;
using WebApp.Shop.Neptons;
using WebApp.Shop.OdataControllers;
using DataLayer.UnitOfWork;
using DataLayer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using DataLayer.UnitOfWork.Lanuages;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration).AddControllersWithViews().AddOdataConfig();

builder.Services.AddLocalization(option =>
{
    option.ResourcesPath = "Lanuages";
});

var app = builder.Build();
 

app.UseRequestLocalization(new RequestLocalizationOptions().SetDefaultCulture(ConstTypes.SupportedLanguages.enUS).
    AddSupportedCultures(ConstTypes.SupportedLanguages.List.Values.Select(x => x.Value).ToArray()));

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseReactCommunication();
app.UseGlobalMiddleware();
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
    pattern: "{controller=home}/{action=Index}/{id?}");
app.MapFallbackToFile("index.html");
app.Run();


