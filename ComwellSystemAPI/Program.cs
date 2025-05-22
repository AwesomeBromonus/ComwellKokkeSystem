using ComwellSystemAPI.Interfaces;
using ComwellSystemAPI.Repositories;
using Interface;
using Modeller; // 👈 nødvendigt for Notification
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ComwellSystemAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// CORS – Allow Blazor frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy
            .WithOrigins("https://localhost:7139") // Din Blazor frontend-port
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Repositories
builder.Services.AddSingleton<IElevplan, ElevplanRepository>();
builder.Services.AddSingleton<IPraktikperiode, PraktikperiodeRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepositoryMongodb>();
builder.Services.AddSingleton<IDelmål, DelmålRepository>();
builder.Services.AddSingleton<IBesked, BeskedRepositoryMongoDB>();
builder.Services.AddSingleton<ILæring, LæringRepositoryMongoDB>();
builder.Services.AddSingleton<IUnderdelmaal, UnderdelmaalRepository>();
builder.Services.AddSingleton<IKommentar, KommentarRepository>();
builder.Services.AddSingleton<IDelmaalSkabelon, DelmaalSkabelonRepository>();
builder.Services.AddSingleton<IAnmodningRepository, AnmodningRepositoryMongo>();
builder.Services.AddSingleton<IUnderdelmaalSkabelon, UnderdelmaalSkabelonRepository>();

builder.Services.AddSingleton<IGenereRapport, GenereRapportMongoDB>();


// OpenAPI/Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

// Swagger GUI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazor");
app.UseAuthorization();

app.MapControllers();



app.Run();
