using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Tilføj services til containeren
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ComwellSystemAPI", Version = "v1" });
});

// 💥 Tilføj CORS-politik
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:5295", "http://localhost:5295")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});



        var app = builder.Build();

// 💡 Swagger kun i Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ComwellSystemAPI v1");
    });
}

// 💥 Aktiver CORS-politikken
app.UseCors("AllowBlazorClient");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();
