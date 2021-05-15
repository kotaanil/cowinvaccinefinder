using CoWINVaccineFinder.Application;
using CoWINVaccineFinder.Application.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoWINVaccineFinder.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Building CoWIN Vaccine Finder Host");
            var host = new HostBuilder()
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddHttpClient<CoWINApiClient>();
                   services.AddSingleton<CoWINUtilities>();
                   services.AddTransient<CoWINAuthService>();
                   services.AddTransient<CoWINAppointmentService>();
               }).UseConsoleLifetime().Build();

            Console.WriteLine("Starting CoWIN Vaccine Finder Host");

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var cancellationToken = new CancellationTokenSource().Token;
                    var coWINAuthService = services.GetRequiredService<CoWINAuthService>();
                    var coWINAppointmentService = services.GetRequiredService<CoWINAppointmentService>();
                    var coWINUtilities = services.GetRequiredService<CoWINUtilities>();

                    Console.WriteLine("Enter Mobile Number:");
                    await coWINAuthService.GenerateMobileOTPAsync(Console.ReadLine(), cancellationToken);
                    Console.WriteLine("Enter otp:");
                    await coWINAuthService.ValidateMobileOTPAsync(Console.ReadLine(), cancellationToken);

                    while (true)
                    {
                        if(DateTime.Compare(coWINUtilities.Token.ValidTo.AddMinutes(-1), DateTime.UtcNow) < 0 )
                        {
                            await coWINAuthService.GenerateMobileOTPAsync(coWINUtilities.MobileNumber, cancellationToken);
                            Console.WriteLine();
                            Console.WriteLine("Earlier OTP expired. Enter the new OTP");
                            Console.WriteLine("Enter OTP:");
                            await coWINAuthService.ValidateMobileOTPAsync(Console.ReadLine(), cancellationToken);
                        }

                        Console.Write("\rSearching for Vaccine at: " + DateTime.Now);
                        var responseDto = await coWINAppointmentService.FetchSessionsByDistrictIdAndDate("581", "15-05-2021",cancellationToken);
                        List<string> availableCentres = new List<string>();

                        foreach (Center center in responseDto.Centers)
                        {
                            foreach (Session session in center.Sessions)
                            {
                                if (session.AvailableCapacity > 0)
                                {
                                    Console.WriteLine(
                                        "Center Name: " + center.Name + Environment.NewLine +
                                        "Minimum Age Limit: " + session.MinAgeLimit + Environment.NewLine +
                                        "Vaccinie Type: " + session.Vaccine.ToString() + Environment.NewLine +
                                        "Available: " + session.AvailableCapacity);
                                    Console.WriteLine();
                                }
                            }
                        }
                        await Task.Delay(10000);
                    }
                }
                catch
                {
                    Console.WriteLine("Error Occured");
                }
            }
        }
    }
}
