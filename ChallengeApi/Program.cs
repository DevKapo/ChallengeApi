using Microsoft.EntityFrameworkCore;
using ChallengeApi.Data;
using Microsoft.AspNetCore.OData;
using ChallengeApi.Entities;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using ChallengeApi.DTOs;

var builder = WebApplication.CreateBuilder(args);




builder.WebHost.UseUrls("http://*:5046");

// Agregar servicios de Swagger
builder.Services.AddEndpointsApiExplorer(); // Necesario para generar la documentación de la API
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo{
        Title = "API de Peliculas",
        Version = "v1", // Versión de la API
        Description = "Una API para gestionar peliculas y sus caracteristicas.",
    });

    // Configurar el archivo de documentación Swagger en formato OpenAPI 3.0
    options.DescribeAllParametersInCamelCase(); // Opcional: para describir los parámetros en notación camelCase
});

builder.Services.AddAutoMapper(typeof(Program));
// Crear variable para conexión
var conexionString = builder.Configuration.GetConnectionString("Conexion");

// Agregar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        conexionString,
        ServerVersion.AutoDetect(conexionString),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null
        )
    )
);


// Agregar servicios de OData
builder.Services.AddControllers()
    .AddOData(options =>
    {
        options.Select().Filter().OrderBy().Count().Expand().SetMaxTop(100);
        options.AddRouteComponents("odata", GetEdmModel());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true;
    });

//  Fuerza la cultura con punto decimal
var cultureInfo = new System.Globalization.CultureInfo("en-US");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;




var app = builder.Build();


// Reintento de conexión y migración de base de datos
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var retries = 10;
    var delay = TimeSpan.FromSeconds(3);

    while (retries > 0)
    {
        try
        {
            dbContext.Database.Migrate();
            break;
        }
        catch (Exception ex)
        {
            retries--;
            if (retries == 0)
            {
                Console.WriteLine("No se pudo conectar a la base de datos. Detalles: " + ex.Message);
                throw;
            }

            Console.WriteLine("Esperando conexión a la base de datos... Reintentando en 3 segundos.");
            Thread.Sleep(delay);
        }
    }
}

// Configuración de Swagger UI
app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = string.Empty;  // dejo esto vacio para que swagger cargue en la raiz 
    });

// Crear el modelo EDM para OData
static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<PeliculaDto>("Peliculas");
    builder.EntityType<PeliculaDto>().ComplexProperty(p => p.Portada);
    builder.EntitySet<ProductoraDto>("Productoras");
    builder.EntitySet<GeneroDto>("Generos");
    builder.EntitySet<ActorDto>("Actores");
    return builder.GetEdmModel();
}


app.UseAuthorization();
app.MapControllers();

app.Run();

