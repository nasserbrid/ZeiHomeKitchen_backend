using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZeiHomeKichen_backend.MappingConfiguration;
using ZeiHomeKichen_backend.Models;
using ZeiHomeKichen_backend.Repositories;
using ZeiHomeKichen_backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//J'ajoute ici la configuration de la connexion avec la base de données.
var connectionString = builder.Configuration.GetConnectionString("SqlDbConnection");

//J'ajoute mon dbcontext dans program.cs
builder.Services.AddDbContext<ZeiHomeKitchenContext>(options => 
     options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDbConnection")));


builder.Services.AddIdentity<Utilisateur, IdentityRole<int>>()
    .AddEntityFrameworkStores<ZeiHomeKitchenContext>()
    .AddDefaultTokenProviders();


//J'ajoute IngredientRepository dans program.cs
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();

//J'ajoute IngredientService dans program.cs
builder.Services.AddScoped<IIngredientService, IngredientService>();
//J'ajoute AutoMapper dans Program.cs
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
