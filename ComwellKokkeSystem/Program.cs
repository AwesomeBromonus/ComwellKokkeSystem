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

        builder.Services.AddScoped<UserState>();


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
        



        await builder.Build().RunAsync();
    }
}
