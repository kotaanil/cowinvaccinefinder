using CoWINVaccineFinder.Application;
using CoWINVaccineFinder.BlazorApp.ViewModels;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoWINVaccineFinder.BlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddMudServices();
            builder.Services.AddHttpClient<CoWINApiClient>();
            builder.Services.AddSingleton<CoWINUtilities>();
            builder.Services.AddSingleton<IndexViewModel>();
            builder.Services.AddTransient<CoWINAuthService>();
            builder.Services.AddTransient<CoWINAppointmentService>();
            builder.Services.AddLocalization(options =>
             {
                 options.ResourcesPath = "Resources";
             });
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
            await builder.Build().RunAsync();
        }
    }
}
