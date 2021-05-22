using CoWINVaccineFinder.Application.DTOs;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoWINVaccineFinder.Application
{
    public class CoWINApiClient
    {
        private readonly HttpClient client;

        public CoWINApiClient(HttpClient client)
        {
            this.client = client;
            this.client.BaseAddress = new Uri("https://cdn-api.co-vin.in");
            this.client.Timeout = new TimeSpan(0, 0, 30);
            this.client.DefaultRequestHeaders.Clear();
        }

        public async Task<GenerateMobileOTPResponse> GenerateMobileOTPAsync(GenerateMobileOTPRequest generateMobileOTPRequest, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v2/auth/generateMobileOTP");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(generateMobileOTPRequest)));

            using (var response = await this.client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GenerateMobileOTPResponse>(responseString);
            }
        }

        public async Task<FetchIdTypesResponse> FetchIdTypes(string token, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/v2/registration/beneficiary/idTypes");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);


            using (var response = await this.client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FetchIdTypesResponse>(responseString);
            }
        }

        public async Task<FetchGendersResponse> FetchGenders(string token, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/v2/admin/location/states");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using (var response = await this.client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FetchGendersResponse>(responseString);
            }
        }

        public async Task<ValidateMobileOTPResponse> ValidateMobileOTPAsync(ValidateMobileOTPRequest validateMobileOTPRequest, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v2/auth/validateMobileOtp");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(validateMobileOTPRequest)));

            using (var response = await this.client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ValidateMobileOTPResponse>(responseString);
            }
        }

        public async Task<FetchStatesResponse> FetchStates(string token, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/v2/admin/location/states");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using (var response = await this.client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FetchStatesResponse>(responseString);
            }
        }

        public async Task<FetchDistrictsResponse> FetchDistricts(string stateId, string token, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/v2/admin/location/districts/" + stateId);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using (var response = await this.client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FetchDistrictsResponse>(responseString);
            }
        }

        public async Task<SessionsResponse> FetchSessionsByDistrictIdAndDate(Dictionary<string, string> queryString, string token, CancellationToken cancellationToken)
        {
            var requestUri = QueryHelpers.AddQueryString("api/v2/appointment/sessions/calendarByDistrict", queryString);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using (var response = await this.client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SessionsResponse>(responseString);
            }
        }

        public async Task<SessionsResponse> FetchSessionsByPinAndDate(Dictionary<string, string> queryString, string token, CancellationToken cancellationToken)
        {
            var requestUri = QueryHelpers.AddQueryString("api/v2/appointment/sessions/calendarByPin", queryString);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using (var response = await this.client.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SessionsResponse>(responseString);
            }
        }
    }
}
