using ComwellSystemAPI.Interfaces;
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


        builder.Services.AddSingleton<IElevplan, ElevplanRepository>();

        builder.Services.AddSingleton<IPraktikperiode, PraktikperiodeRepository>();
        builder.Services.AddSingleton<IUserRepository,UserRepositoryMongodb>();
        builder.Services.AddSingleton<IDelmål, DelmålRepository>();
        builder.Services.AddSingleton<IBesked, BeskedRepositoryMongoDB>();
        // Add these lines to your services configuration
        builder.Services.AddSingleton<IGenereRapport, GenereRapportMongoDB>();
builder.Services.AddSingleton<IKommentar, KommentarRepository>();



        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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
