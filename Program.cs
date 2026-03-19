using KertKerdes.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


// IDEIGLENES: InMemory adatbázis a stabil futtatáshoz
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("KertDb"));


// KÉSÕBB MSSQL-hez ezt kell visszakapcsolni:
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(
//         builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Kerdes}/{action=Index}/{id?}");

app.Run();