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

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

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


// Repositories
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



// Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

// Swagger GUI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStaticFiles(); // 🧠 VIGTIG LINJE!

app.UseHttpsRedirection();
app.UseCors("AllowBlazor");
app.UseAuthorization();

app.MapControllers();
app.Run();
