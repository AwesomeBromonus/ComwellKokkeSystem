using BlazorDownloadFile;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ComwellKokkeSystem;
using Blazored.LocalStorage;
using ComwellKokkeSystem.Service;
using ComwellKokkeSystem.Service.Elev;
using ComwellKokkeSystem.Service.QuizService;



public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");


        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri("https://itakokkesystemapi.azurewebsites.net/")
        });

        builder.Services.AddBlazoredLocalStorage();

        // Applikationsservices
        builder.Services.AddScoped<IElevplanService, ElevplanService>();
        builder.Services.AddScoped<IPraktikperiodeService, PraktikperiodeService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IDelmaalService, DelmaalService>();
      
        builder.Services.AddScoped<IKommentarService, KommentarService>();
        builder.Services.AddScoped<IQuizService, QuizService>();

        builder.Services.AddScoped<ILæringService, LæringService>();
        builder.Services.AddBlazorDownloadFile();
        builder.Services.AddScoped<IAnmodningService, AnmodningService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUnderdelmaalService, UnderdelmaalService>();
        builder.Services.AddScoped<IDelmaalSkabelonService, DelmaalSkabelonService>();
        builder.Services.AddScoped<IUnderdelmaalSkabelonService, UnderdelmaalSkabelonService>();
        builder.Services.AddScoped<IQuizService, QuizService>();
        builder.Services.AddScoped<IRapportService, RapportService>();
        builder.Services.AddScoped<IUserStateService, UserStateService>();



        builder.Services.AddAuthorizationCore(); // Gør det muligt at bruge [Authorize] og AuthenticationStateProvider

        await builder.Build().RunAsync();

        var host = builder.Build();
        var userState = host.Services.GetRequiredService<IUserStateService>();
        await userState.InitializeAsync();
        await host.RunAsync();
    }
}
