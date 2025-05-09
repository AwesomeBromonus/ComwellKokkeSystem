using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ComwellKokkeSystem;
using Blazored.LocalStorage;

using Service;
using ComwellKokkeSystem.Service;


public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        //  Root komponenter
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");




builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7013/") // <- din API-base
});

        //  Local Storage til state management
        builder.Services.AddBlazoredLocalStorage();


        //  Services
        builder.Services.AddScoped<IElevplanService, ElevplanService>();
        builder.Services.AddScoped<IPraktikperiodeService, PraktikperiodeService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddSingleton<UserState>();



        await builder.Build().RunAsync();
    }
}
