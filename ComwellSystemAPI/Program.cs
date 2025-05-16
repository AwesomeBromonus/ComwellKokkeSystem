using ComwellSystemAPI.Interfaces;
using ComwellSystemAPI.Repositories;
using Interface;

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
builder.Services.AddSingleton<ILæring, LæringRepositoryMongoDB>();
builder.Services.AddSingleton<IBesked, BeskedRepositoryMongoDB>();


builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazor"); // 🚨 skal være før MapControllers
app.UseAuthorization();

app.MapControllers();

app.Run();
