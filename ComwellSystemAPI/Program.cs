using System.Text;
using ComwellSystemAPI.Interfaces;
using ComwellSystemAPI.Repositories;
using Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Modeller; // 👈 nødvendigt for Notification
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Tilføj controller services (MVC/Web API)
builder.Services.AddControllers();

// Konfigurer CORS til at tillade anmodninger fra Blazor frontend (lokal host)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy
            .WithOrigins("https://localhost:7139")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Bind MongoDbSettings fra appsettings.json til POCO-klasse
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Registrér MongoClient som singleton for genbrug af forbindelse
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Registrér MongoDatabase som singleton, som kan injiceres i repositorier
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Registrér repositories som singletons — hver interface får sin konkrete implementation
builder.Services.AddSingleton<IElevplan, ElevplanRepository>();
builder.Services.AddSingleton<IPraktikperiode, PraktikperiodeRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepositoryMongodb>();
builder.Services.AddSingleton<IDelmål, DelmålRepository>();
builder.Services.AddSingleton<ILæring, LæringRepositoryMongoDB>();
builder.Services.AddSingleton<IUnderdelmaal, UnderdelmaalRepository>();
builder.Services.AddSingleton<IKommentar, KommentarRepository>();
builder.Services.AddSingleton<IDelmaalSkabelon, DelmaalSkabelonRepository>();
builder.Services.AddSingleton<IAnmodningRepository, AnmodningRepositoryMongo>();
builder.Services.AddSingleton<IUnderdelmaalSkabelon, UnderdelmaalSkabelonRepository>();
builder.Services.AddSingleton<IQuiz, QuizRepositoryMongoDB>();
builder.Services.AddSingleton<IQuestion, QuestionRepository>();
builder.Services.AddSingleton<IRapportRepository, RapportRepository>();

// Tilføj OpenAPI / Swagger til dokumentation og test i dev-miljø
builder.Services.AddOpenApi();

var app = builder.Build();

// Aktivér Swagger UI kun i development miljø
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Server statiske filer (fx wwwroot)
// Det er vigtigt, så Blazor kan hente css, js og billeder
app.UseStaticFiles();

app.UseHttpsRedirection();

// Brug CORS-politikken "AllowBlazor"
app.UseCors("AllowBlazor");

// Middleware for autorisation (bruges hvis du har auth/identity)
app.UseAuthorization();

// Map controller endpoints (API routes)
app.MapControllers();

// Start applikationen
app.Run();
