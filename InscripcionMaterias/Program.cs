using DinkToPdf.Contracts;
using DinkToPdf;
using InscripcionMaterias.Models;
using InscripcionMaterias.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GestionDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionGestion")));
builder.Services.AddScoped<ReporteService>();
builder.Services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));
builder.Services.AddDistributedMemoryCache();
// 2. Configura los servicios de sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Duración de la sesión inactiva
    options.Cookie.HttpOnly = true; // La cookie de sesión solo es accesible por el servidor
    options.Cookie.IsEssential = true; // La cookie de sesión es esencial para la funcionalidad
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Define el tiempo de inactividad de la sesión
    options.Cookie.HttpOnly = true; // Hace la cookie de sesión solo accesible por el servidor
    options.Cookie.IsEssential = true; // Marca la cookie como esencial (necesaria para el funcionamiento de la app)
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

app.UseRouting();
app.UseSession();

app.UseSession();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
