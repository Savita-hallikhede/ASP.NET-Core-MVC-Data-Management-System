using Entity;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Service;
using ServiceContracts.DTO;
using ServiceContracts.Interface;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var con = builder.Configuration.GetConnectionString("MVC_Con");

builder.Services.AddDbContext<DbContextdata>(A => A.UseSqlServer(con,b=>b.MigrationsAssembly("Entity")));



//builder.Services.AddDbContext<Dbcontextdata>(A => A.UseSqlServer(builder.Configuration.GetConnectionString("MVC_Con")));

builder.Services.AddScoped<ICountrySC, CountryService>();

builder.Services.AddScoped<IPersonSC, PersonService>();

builder.Services.AddScoped<IFileData, FilesFormatData>();
 

var app = builder.Build();
RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");


app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Person}/{action=PersonData1}/{id?}");
app.Run();
