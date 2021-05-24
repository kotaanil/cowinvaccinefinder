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
                   services.AddTransient<CoWINMetadataService>();
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
                    var coWINMetadataService = services.GetRequiredService<CoWINMetadataService>();

                    Console.WriteLine("Enter Mobile Number:");
                    await coWINAuthService.GenerateMobileOTPAsync("8341983981", cancellationToken);
                    Console.WriteLine("Enter otp:");
                    await coWINAuthService.ValidateMobileOTPAsync(Console.ReadLine(), cancellationToken);
                    var states = (await coWINMetadataService.GetStates(cancellationToken));
                    for (int i = 0; i < states.Count; i++) //item in districtsResponse.States)
                    {
                        if (i % 3 == 0) Console.WriteLine("");
                        var item = states[i];
                        Console.Write(" " +item.StateId + " " + item.StateName);
                    }
                    var stateId = 0;
                    do
                    {
                        Console.WriteLine("Please choose the state");
                        int.TryParse(Console.ReadLine(), out stateId );
                    }
                    while (!(stateId > 0 )); //&& stateId <= statesResponse.Total)
                    var districts = (await coWINMetadataService.GetDistricts(stateId.ToString(), cancellationToken));
                    for (int i=0; i< districts.Count; i++) //item in districtsResponse.States)
                    {
                        if (i % 3 == 0) Console.WriteLine("");
                        var item = districts[i];
                        Console.Write(" " + item.DistrictId + " " + item.DistrictName);
                    }
                    var districtId = 0;
                    do
                    {
                        Console.WriteLine("Please choose the district");
                        _ = int.TryParse(Console.ReadLine(), out districtId);
                    }
                    while (!(districtId > 0 )); //&& stateId <= districtsResponse.Total

                    var gendersResponse = await coWINMetadataService.GetBeneficiaryGenders(cancellationToken);
                    var idtypesResponse = await coWINMetadataService.GetBeneficiaryIdTypes(cancellationToken);

                    while (true)
                    {
                        if(DateTime.Compare(coWINUtilities.Token.ValidTo, DateTime.Now) < 0 )
                        {
                            await coWINAuthService.GenerateMobileOTPAsync(coWINUtilities.MobileNumber, cancellationToken);
                            Console.WriteLine("Earlier OTP expired. Enter the new OTP");
                            Console.WriteLine("Enter otp:");
                            await coWINAuthService.ValidateMobileOTPAsync(Console.ReadLine(), cancellationToken);
                        }

                        Console.WriteLine("Searching for Vaccine at: " + DateTime.Now);
                        var centers = await coWINAppointmentService.FetchSessionsByDistrictIdAndDate(districtId.ToString(),6,cancellationToken);
                        List<string> availableCentres = new List<string>();

                        foreach (Center center in centers)
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
