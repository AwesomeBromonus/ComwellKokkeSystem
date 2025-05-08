using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ComwellKokkeSystem;
using Blazored.LocalStorage;

using Service;


public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        //  Root komponenter
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddSingleton<UserState>();
        builder.Services.AddScoped<IElevplanService, ElevplanService>();


        builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7139/") // <- din API-base
});

builder.Services.AddScoped<IAuthService, AuthService>();

        await builder.Build().RunAsync();
    }
}
