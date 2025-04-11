using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using ZeiHomeKitchen_backend.MappingConfiguration;
using ZeiHomeKitchen_backend.Models;
using ZeiHomeKitchen_backend.Repositories;
using ZeiHomeKitchen_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization; 




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

var jwtKey = builder.Configuration["Jwt:SecretKey"];
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
});
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

//builder.Services.AddScoped<IStatistiqueRepository, StatistiqueRepository>();
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();

builder.Services.AddScoped<IPaiementRepository, PaiementRepository>();

builder.Services.AddScoped<IStatistiqueRepository, StatistiqueRepository>();

//J'ajoute IngredientService dans program.cs
builder.Services.AddScoped<IIngredientService, IngredientService>();

//J'ajoute PlatService dans program.cs
builder.Services.AddScoped<IPlatService, PlatService>();

//J'ajoute ImagesService dans program.cs
builder.Services.AddScoped<IImagesService, ImagesService>();

//J'ajoute LoginService dans program.cs
builder.Services.AddScoped<ILoginService, LoginService>();
// Ajoutez les lignes suivantes dans la méthode ConfigureServices
builder.Services.AddScoped<ICreateReservationRepository, CreateReservationRepository>();
builder.Services.AddScoped<ICreateReservationService, CreateReservationService>();

//J'ajoute RegisterService dans program.cs
builder.Services.AddScoped<IRegisterService, RegisterService>();

//J'ajoute ReservationService dans program.cs
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();

builder.Services.AddScoped<IPaiementService, PaiementService>();

builder.Services.AddScoped<IStatistiqueService, StatistiqueService>();

builder.Services.AddHttpContextAccessor();

//builder.Services.AddScoped<IStatistiqueService, StatistiqueService>();

builder.Services.AddScoped<TokenService>(provider =>
{
    //Déjà configuré dans votre appsettings.json
    var secretKey = builder.Configuration["Jwt:SecretKey"]; 
    return new TokenService(secretKey);
});




//J'ajoute AutoMapper dans Program.cs
//builder.Services.AddAutoMapper(typeof(MappingProfile));

//builder.Services.AddControllers();
//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
//        options.JsonSerializerOptions.PropertyNamingPolicy = null;
//        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

//    });

//builder.Services.AddControllers()
//    .AddNewtonsoftJson(options =>
//    {
//        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
//        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
//    });

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        // Ajoutez cette ligne pour être sûr
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });
//Me permets d'avoir une taille plus conséquente pour les images en base64.
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxResponseBufferSize = 10 * 1024 * 1024; // Par exemple, 10 Mo
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ZeiHomeKitchen_backend", Version = "v1" });

    // Ajoute la configuration pour le JWT Bearer
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header
            },
            new List<string>()
        }
    });
});






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

app.UseDeveloperExceptionPage(); 

app.MapControllers();


// Création des rôles par défaut
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Utilisateur>>();

    // Ici j'ajoute des rôles Admin et User venant d'IdentityUser
    string[] roleNames = { "Admin", "User" };
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            //Je crée le rôle s'il n'existe pas
            roleResult = await roleManager.CreateAsync(new IdentityRole<int>(roleName));
        }
    }
}

app.Run();
