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

        public async Task<List<Center>> FetchSessionsByPinAndDate(string pincode, int noOfWeeks, CancellationToken cancellationToken)
        {
            if (noOfWeeks < 0)
                throw new Exception("Invalid number of weeks specified");

            var queryString = new Dictionary<string, string>();
            queryString.Add("pincode", pincode);
            var centers = new List<Center>();
            for (int i = 0; i < noOfWeeks; i++)
            {
                if (queryString.ContainsKey("date"))
                    queryString.Add("date", DateTime.Today.AddDays(i * 7).ToString("dd-MM-yyyy"));
                else
                    queryString["date"] = DateTime.Today.AddDays(i * 7).ToString("dd-MM-yyyy");
                var response = await coWINApiClient.FetchSessionsByPinAndDate(queryString, coWINUtilities.TokenText, cancellationToken);
                if(response.Centers != null)
                    centers.AddRange(response.Centers);
            }
            return centers;
        }

        public async Task<List<Center>> FetchSessionsByDistrictIdAndDate(string districtId, int noOfWeeks, CancellationToken cancellationToken)
        {
            if (noOfWeeks < 0)
                throw new Exception("Invalid number of weeks specified");

            var queryString = new Dictionary<string, string>();
            queryString.Add("district_id", districtId);
            var centers = new List<Center>();
            for (int i = 0; i < noOfWeeks; i++)
            {
                if (!queryString.ContainsKey("date"))
                    queryString.Add("date", DateTime.Today.AddDays(i * 7).ToString("d/M/yyyy"));
                else
                    queryString["date"] = DateTime.Today.AddDays(i * 7).ToString("d/M/yyyy");
                var response = await coWINApiClient.FetchSessionsByDistrictIdAndDate(queryString, coWINUtilities.TokenText, cancellationToken);
                if (response.Centers != null)
                    centers.AddRange(response.Centers);
            }
            return centers;
        }

        public async Task<List<Beneficiary>> FetchBeneficiaries(CancellationToken cancellationToken)
        {
            var fetchBeneficiaryResponse =  await coWINApiClient.FetchBeneficiaries(
                coWINUtilities.TokenText, cancellationToken);
            return fetchBeneficiaryResponse.Beneficiaries;
        }
    }
}
