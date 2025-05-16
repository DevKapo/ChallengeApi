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
    options.UseMySql(conexionString!, ServerVersion.AutoDetect(conexionString)));

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

