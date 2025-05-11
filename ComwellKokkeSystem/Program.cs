using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ComwellKokkeSystem;
using Blazored.LocalStorage;
using Service;
using ComwellKokkeSystem.Service;
using MudBlazor;
using MudBlazor.Services;

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

        // MudBlazor services
        builder.Services.AddMudServices();

        // Custom theme
        var theme = new MudTheme()
        {
            Palette = new Palette()
            {
                Primary = "#007fa3",          // Comwell-blå
                Secondary = "#00c2cb",        // Frisk accent
                Background = "#f3f4f6",       // Lys, rolig baggrund
                Surface = "#ffffff",          // Kort baggrund
                AppbarBackground = "#003b4a", // Header
                TextPrimary = "#111827",
                DrawerBackground = "#003b4a",
                DrawerText = "#ffffff"
            },
            LayoutProperties = new LayoutProperties()
            {
                DefaultBorderRadius = "12px"
            }
        };

        builder.Services.AddSingleton<MudTheme>(theme);

        await builder.Build().RunAsync();
    }
}
