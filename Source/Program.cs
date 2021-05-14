using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            Console.WriteLine("Starting CoWIN Vaccine Finder");

            //Authentication
            //Generate OTP
            Console.WriteLine("Please enter 10 digit mobile number: ");
            var generateMobileOTPRequestDTO = new GenerateMobileOTPRequestDTO();
            generateMobileOTPRequestDTO.mobile = Console.ReadLine();
            var generateMobileOTPRequestByteArray = new ByteArrayContent(
                System.Text.Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(generateMobileOTPRequestDTO)));
            generateMobileOTPRequestByteArray.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var generateMobileOTPResponse = 
                await client.PostAsync(
                    "https://cdn-api.co-vin.in/api/v2/auth/generateMobileOTP", generateMobileOTPRequestByteArray);
            generateMobileOTPResponse.EnsureSuccessStatusCode();
            string generateMobileOTPResponseString = await generateMobileOTPResponse.Content.ReadAsStringAsync();
            var generateMobileOTPResponseDTO = JsonConvert.DeserializeObject<GenerateMobileOTPResponseDTO>(generateMobileOTPResponseString);

            //Validate OTP
            var validateMobileOTPRequestDTO = new ValidateMobileOTPRequestDTO();
            Console.WriteLine("Enter otp:");
            validateMobileOTPRequestDTO.otp = ComputeSha256Hash(Console.ReadLine());
            validateMobileOTPRequestDTO.txnId = generateMobileOTPResponseDTO.txnId;
            var validateMobileOTPRequestByteArray = new ByteArrayContent(
                System.Text.Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(validateMobileOTPRequestDTO)));
            generateMobileOTPRequestByteArray.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var validateMobileOTPResponse = await client.PostAsync(
                "https://cdn-api.co-vin.in/api/v2/auth/validateMobileOtp", validateMobileOTPRequestByteArray);
            validateMobileOTPResponse.EnsureSuccessStatusCode();
            string validateMobileOTPResponseString = await validateMobileOTPResponse.Content.ReadAsStringAsync();
            var validateMobileOTPResponseDTO = JsonConvert.DeserializeObject<ValidateMobileOTPResponseDTO>(validateMobileOTPResponseString);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", validateMobileOTPResponseDTO.token);
            while (true)
            {
                await FindVaccineAsync(client);
                await Task.Delay(10000);
            }
        }

        private static async Task FindVaccineAsync(HttpClient client)
        {
            Console.WriteLine("Searching at: " + DateTime.Now);

            HttpResponseMessage response = await client.GetAsync("https://cdn-api.co-vin.in/api/v2/appointment/sessions/calendarByPin?pincode=500062&date=14-05-2021");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var responseDto = ResponseDto.FromJson(responseBody);
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
        }

        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
