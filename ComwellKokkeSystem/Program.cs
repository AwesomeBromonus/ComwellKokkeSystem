using BlazorDownloadFile;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ComwellKokkeSystem;
using Blazored.LocalStorage;
using ComwellKokkeSystem.Service;
using ComwellKokkeSystem.Service.Elev;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddSingleton<UserState>();

        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7013/")
        });

        builder.Services.AddBlazoredLocalStorage();

        // Applikationsservices
        builder.Services.AddScoped<IElevplanService, ElevplanService>();
        builder.Services.AddScoped<IPraktikperiodeService, PraktikperiodeService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IDelmaalService, DelmaalService>();
        builder.Services.AddScoped<IBeskedService, BeskedService>();
        builder.Services.AddScoped<IKommentarService, KommentarService>();
        builder.Services.AddScoped<IGenereRapportService, GenereRapportService>();
        builder.Services.AddScoped<ILæringService, LæringService>();
        builder.Services.AddBlazorDownloadFile();
        builder.Services.AddScoped<IAnmodningService, AnmodningService>();
        builder.Services.AddScoped<IUserService, UserService>();




        await builder.Build().RunAsync();

        var host = builder.Build();
        var userState = host.Services.GetRequiredService<UserState>();
        await userState.InitializeAsync(); // <- Henter fra localStorage ved start
        await host.RunAsync();
    }
}
