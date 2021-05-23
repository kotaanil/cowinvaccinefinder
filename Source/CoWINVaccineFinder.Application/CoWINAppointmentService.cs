using CoWINVaccineFinder.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoWINVaccineFinder.Application
{
    public class CoWINAppointmentService
    {
        private readonly CoWINApiClient coWINApiClient;
        private CoWINUtilities coWINUtilities;
        private string transactionId = string.Empty;

        public CoWINAppointmentService(CoWINApiClient coWINApiClient, CoWINUtilities utilities)
        {
            this.coWINApiClient = coWINApiClient;
            this.coWINUtilities = utilities;
        }

        public async Task<SessionsResponse> FetchSessionsByPinAndDate(string pincode,string date, CancellationToken cancellationToken)
        {
            var queryString = new Dictionary<string, string>();
            queryString.Add("pincode", pincode);
            queryString.Add("date", date);

            return await coWINApiClient.FetchSessionsByPinAndDate(queryString, coWINUtilities.TokenText, cancellationToken);
        }

        public async Task<SessionsResponse> FetchSessionsByDistrictIdAndDate(string districtId, string date, CancellationToken cancellationToken)
        {
            var queryString = new Dictionary<string, string>();
            queryString.Add("district_id", districtId);
            queryString.Add("date", date);

            return await coWINApiClient.FetchSessionsByDistrictIdAndDate(queryString, coWINUtilities.TokenText, cancellationToken);
        }

        public async Task<List<Beneficiary>> FetchBeneficiaries(CancellationToken cancellationToken)
        {
            var fetchBeneficiaryResponse =  await coWINApiClient.FetchBeneficiaries(
                coWINUtilities.TokenText, cancellationToken);
            return fetchBeneficiaryResponse.Beneficiaries;
        }
    }
}
