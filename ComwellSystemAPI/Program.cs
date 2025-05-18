using ComwellSystemAPI.Interfaces;
using ComwellSystemAPI.Repositories;
using MongoDB.Driver;
using Interface;

// Hvis man forbinder direkte i klassen, bryder vi 
// dependency injection ( som forbindelsestrenge )
var builder = WebApplication.CreateBuilder(args);
// Tilføj MongoDB-forbindelse
var mongoUri = "mongodb+srv://Brobolo:Bromus12344321@cluster0.k4kon.mongodb.net/";
var client = new MongoClient(mongoUri);
var database = client.GetDatabase("Comwell");
builder.Services.AddSingleton<IMongoDatabase>(database);
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
        builder.Services.AddSingleton<ILæring, LæringRepositoryMongoDB>();
// Add these lines to your services configuration
        builder.Services.AddSingleton<IGenereRapport, GenereRapportMongoDB>();
builder.Services.AddSingleton<IKommentar, KommentarRepository>();
builder.Services.AddSingleton<IDelmaalSkabelon, DelmaalSkabelonRepository>();



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
