using CoreLibrary.Data;
using CoreLibrary.Services;
using CoreLibrary.Services.Interfaces;
using CoreLibrary.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IErrorLogService, ErrorLogService>();
builder.Services.AddScoped<IInscripcionService, InscripcionService>();
builder.Services.AddScoped<IHomeService, HomeService>();

builder.Services.AddDbContext<EventCorpContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("EventCorpContext"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/eventos", async (IEventoService eventos) =>
{
    IEnumerable<EventoModel> eventosEncontrados = await eventos.Listado();
    var resultado = new List<Dictionary<string, object>> { };

    foreach (EventoModel eventoModel in eventosEncontrados)
    {
        resultado.Add(new Dictionary<string, object>
        {
            {"Id", eventoModel.Id},
            {"Titulo", eventoModel.Titulo},
            {"Descripcion", eventoModel.Descripcion},
            {"Categoria", eventoModel.Categoria != null ? eventoModel.Categoria.Nombre : ""},
            {"Fecha", eventoModel.Fecha},
            {"Hora", eventoModel.Hora},
            {"Duracion", eventoModel.Duracion},
            {"Ubicacion", eventoModel.Ubicacion},
            {"CupoMaximo", eventoModel.CupoMaximo},
            {"FechaRegistro", eventoModel.FechaRegistro},
            {"UsuarioId", eventoModel.UsuarioRegistroId},
            {"UsuarioUsername", eventoModel.UsuarioRegistro != null ? eventoModel.UsuarioRegistro.NombreUsuario : ""},
            {"UsuarioNombre", eventoModel.UsuarioRegistro != null ? eventoModel.UsuarioRegistro.NombreCompleto : ""},
            {"UsuarioCorreo", eventoModel.UsuarioRegistro != null ? eventoModel.UsuarioRegistro.Correo : ""},
            {"UsuarioTelefono", eventoModel.UsuarioRegistro != null ? eventoModel.UsuarioRegistro.Telefono : ""}
        });
    }
    return resultado.Count > 0 ? Results.Ok(resultado) : Results.NoContent();
})
.WithName("GetEventos")
.WithOpenApi();

app.MapGet("/evento/{id}", async (int id, IEventoService eventos) =>
{
    var eventoModel = await eventos.ObtenerEvento(id);
    if (eventoModel == null)
        return Results.NotFound();

    var resultado = new Dictionary<string, object>
    {
        {"Id", eventoModel.Id},
        {"Titulo", eventoModel.Titulo},
        {"Descripcion", eventoModel.Descripcion},
        {"Categoria", eventoModel.Categoria != null ? eventoModel.Categoria.Nombre : ""},
        {"Fecha", eventoModel.Fecha},
        {"Hora", eventoModel.Hora},
        {"Duracion", eventoModel.Duracion},
        {"Ubicacion", eventoModel.Ubicacion},
        {"CupoMaximo", eventoModel.CupoMaximo},
        {"FechaRegistro", eventoModel.FechaRegistro},
        {"UsuarioId", eventoModel.UsuarioRegistroId},
        {"UsuarioUsername", eventoModel.UsuarioRegistro != null ? eventoModel.UsuarioRegistro.NombreUsuario : ""},
        {"UsuarioNombre", eventoModel.UsuarioRegistro != null ? eventoModel.UsuarioRegistro.NombreCompleto : ""},
        {"UsuarioCorreo", eventoModel.UsuarioRegistro != null ? eventoModel.UsuarioRegistro.Correo : ""},
        {"UsuarioTelefono", eventoModel.UsuarioRegistro != null ? eventoModel.UsuarioRegistro.Telefono : ""}
    };

    return Results.Ok(resultado);
})
.WithName("GetEventoPorId")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
