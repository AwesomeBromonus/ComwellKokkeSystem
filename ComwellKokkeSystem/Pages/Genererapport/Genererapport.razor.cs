using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Modeller;

// --- VIGTIGT: RETTET NAMESPACE HER! ---
// Da din .razor fil ligger i 'Pages/Genererapport'
// skal namespacet være 'DitBlazorProjektsRootNamespace.Pages.Genererapport'
namespace ComwellKokkeSystem.Pages.Genererapport // <--- DETTE ER DEN VIGTIGE RETTELSE!
{
    public class Genererapport : ComponentBase // Sørg for 'partial' er her
    {
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private NavigationManager NavManager { get; set; }
        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        private List<RapportElevDelmålViewModel> reportData = new();
        private int selectedYear = DateTime.Now.Year;
        private bool isLoading;

        private List<Delmål> allDelmålWithUnderdelmaal = new();
        private List<UserModel> allUsers = new();
        private List<Praktikperiode> allPraktikperioder = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadReportData();
        }

        private async Task LoadReportData()
        {
            isLoading = true;
            reportData.Clear();
            StateHasChanged();

            try
            {
                allDelmålWithUnderdelmaal = await Http.GetFromJsonAsync<List<Delmål>>($"api/Rapport/delmaal-with-underdelmaal/{selectedYear}") ?? new List<Delmål>();
                allUsers = await Http.GetFromJsonAsync<List<UserModel>>($"api/Rapport/brugere/{selectedYear}") ?? new List<UserModel>();
                allPraktikperioder = await Http.GetFromJsonAsync<List<Praktikperiode>>($"api/Rapport/praktikperioder/{selectedYear}") ?? new List<Praktikperiode>();

                Console.WriteLine($"Hentede: {allDelmålWithUnderdelmaal.Count} delmål, {allUsers.Count} brugere, {allPraktikperioder.Count} praktikperioder for år {selectedYear}");

                foreach (var delmaal in allDelmålWithUnderdelmaal)
                {
                    var user = allUsers.FirstOrDefault(u => u.Id == delmaal.ElevId);
                    var praktikperiode = allPraktikperioder.FirstOrDefault(p => p.Id == delmaal.PraktikperiodeId);

                    if (user != null && praktikperiode != null)
                    {
                        var progressPercent = CalculateProgress(delmaal.UnderdelmaalList);
                        var progressText = $"{Math.Round(progressPercent, 0)}% Fuldført";

                        reportData.Add(new RapportElevDelmålViewModel
                        {
                            ElevId = user.Id,
                            Username = user.Username ?? string.Empty,
                            ElevNavn = user.Navn ?? string.Empty,
                            Rolle = user.Role ?? string.Empty,
                            HotelNavn = user.HotelNavn ?? string.Empty,
                            DelmålId = delmaal.Id,
                            DelmålBeskrivelse = delmaal.Beskrivelse ?? string.Empty,
                            DelmålStatus = delmaal.Status ?? string.Empty,
                            DelmålCalculatedStatus = delmaal.CalculatedStatus ?? string.Empty,
                            DelmålAnsvarlig = delmaal.Ansvarlig ?? string.Empty,
                            DelmålIgangsætter = delmaal.Igangsætter ?? string.Empty,
                            DelmålDeadline = delmaal.Deadline,
                            UnderdelmaalList = delmaal.UnderdelmaalList,
                            PraktikperiodeId = praktikperiode.Id,
                            PraktikperiodeNavn = praktikperiode.Navn ?? string.Empty,
                            DelmålProgressPercent = progressPercent,
                            DelmålProgressText = progressText
                        });
                    }
                }
                Console.WriteLine($"Genereret {reportData.Count} rapportposter.");
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Request Error during LoadReportData: {httpEx.Message}. Status Code: {httpEx.StatusCode}");
                await JsRuntime.InvokeVoidAsync("alert", $"Kunne ikke indlæse data. Tjek API'en og konsollen for fejl: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error during LoadReportData: {ex.Message}\nStackTrace: {ex.StackTrace}");
                await JsRuntime.InvokeVoidAsync("alert", $"Der opstod en fejl under indlæsning af rapporten: {ex.Message}");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private double CalculateProgress(List<Underdelmaal> underdelmaalList)
        {
            if (underdelmaalList == null || !underdelmaalList.Any())
            {
                return 0.0;
            }

            var totalUnderdelmaal = underdelmaalList.Count;
            var completedUnderdelmaal = underdelmaalList.Count(u => u.Status == "Fuldført");

            return (double)completedUnderdelmaal / totalUnderdelmaal * 100;
        }

        private async Task ExportToExcel()
        {
            if (!reportData.Any())
            {
                await JsRuntime.InvokeVoidAsync("alert", "Der er ingen data at eksportere baseret på de indlæste resultater.");
                return;
            }

            try
            {
                Console.WriteLine($"Forsøger at eksportere {reportData.Count} rækker til Excel.");
                var response = await Http.PostAsJsonAsync("api/Rapport/export/excel", reportData);

                if (response.IsSuccessStatusCode)
                {
                    var fileBytes = await response.Content.ReadAsByteArrayAsync();
                    var fileName = $"HR_Rapport_{selectedYear}.xlsx";
                    await JsRuntime.InvokeVoidAsync("saveAsFile", fileName, fileBytes);
                    await JsRuntime.InvokeVoidAsync("alert", "Excel-fil genereret og downloadet!");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    await JsRuntime.InvokeVoidAsync("alert", "Excel-filen blev genereret, men indeholdt ingen data (No Content fra serveren).");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API fejl ved eksport: Status {response.StatusCode}, Indhold: {errorContent}");
                    await JsRuntime.InvokeVoidAsync("alert", $"Fejl ved eksport til Excel: {response.ReasonPhrase}. Se browserkonsol for detaljer.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Generel fejl under Excel eksport: {ex.Message}\nStackTrace: {ex.StackTrace}");
                await JsRuntime.InvokeVoidAsync("alert", "Der opstod en uventet fejl under eksporten.");
            }
        }
    }
}