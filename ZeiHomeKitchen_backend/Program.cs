using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ZeiHomeKitchen_backend.MappingConfiguration;
using ZeiHomeKitchen_backend.Models;
using ZeiHomeKitchen_backend.Repositories;
using ZeiHomeKitchen_backend.Services;



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


//J'ajoute PlatRepository dans program.cs
builder.Services.AddScoped<IPlatRepository, PlatRepository>();

//J'ajoute LoginRepository dans program.cs
builder.Services.AddScoped<ILoginRepository, LoginRepository>();

//J'ajoute RegisterRepository dans program.cs
builder.Services.AddScoped<IRegisterRepository, RegisterRepository>();

//J'ajoute ReservationRepository dans program.cs
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();


//J'ajoute IngredientService dans program.cs
builder.Services.AddScoped<IIngredientService, IngredientService>();

//J'ajoute PlatService dans program.cs
builder.Services.AddScoped<IPlatService, PlatService>();

//J'ajoute ImagesService dans program.cs
builder.Services.AddScoped<IImagesService, ImagesService>();

//J'ajoute LoginService dans program.cs
builder.Services.AddScoped<ILoginService, LoginService>();

//J'ajoute RegisterService dans program.cs
builder.Services.AddScoped<IRegisterService, RegisterService>();

//J'ajoute ReservationService dans program.cs
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<TokenService>(provider =>
{
    var secretKey = builder.Configuration["Jwt:SecretKey"]; // Assurez-vous que cela est configuré dans votre appsettings.json
    return new TokenService(secretKey);
});




//J'ajoute AutoMapper dans Program.cs
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();

//Me permets d'avoir une taille plus conséquente pour les images en base64.
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxResponseBufferSize = 10 * 1024 * 1024; // Par exemple, 10 Mo
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Mise en place d'une sécurité de mon api (on ne touche pas à mon api ok)
//builder.Services.AddSwaggerGen(c => {
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "ZeiHomeKitchen_backend",
//        Version = "v1"
//    });
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
//        {
//            new OpenApiSecurityScheme {
//                Reference = new OpenApiReference {
//                    Type = ReferenceType.SecurityScheme,
//                        Id = "Bearer"
//                }
//            },
//            new string[] {}
//        }
//    });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// Créer les rôles par défaut
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Utilisateur>>();

    // Ici ajout des rôles Admin et User venant d'IdentityUser
    string[] roleNames = { "Admin", "User" };
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            // Créer le rôle si il n'existe pas
            roleResult = await roleManager.CreateAsync(new IdentityRole<int>(roleName));
        }
    }
}

app.Run();
