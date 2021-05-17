using CoWINVaccineFinder.Application;
using CoWINVaccineFinder.BlazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoWINVaccineFinder.BlazorApp.ViewModels
{
    public class IndexViewModel
    {
        private CoWINAuthService coWINAuthService;
        public IndexViewModel(CoWINAuthService coWINAuthService)
        {
            OTPField = true;
            OTPButton = true;
            this.coWINAuthService = coWINAuthService;
            AvailableVaccineCenters = new List<VaccineCenter>();
            AvailableVaccineCenters.Add(new VaccineCenter()
            {
                CenterName = "Apollo",
                MinimumAgeLimit = "45+",
                VaccineType = "Covaxin",
                Availability = "2"
            });
            AvailableVaccineCenters.Add(new VaccineCenter()
            {
                CenterName = "Star",
                MinimumAgeLimit = "45+",
                VaccineType = "Covishield",
                Availability = "4"
            });
        }

        public Action StateHasChangedDelegate { get; set; }

        public string State { get; set; }
        public string District { get; set; }
        public string MobileNumber { get; set; }
        public string OTP { get; set; }
        public string StateSearchParameters { get; set; }
        public string DistrictSearchParameters { get; set; }
        public bool OTPField { get; set; }
        public bool OTPButton { get; set; }

        public List<VaccineCenter> AvailableVaccineCenters { get; set; }

        private List<string> States = new List<string>() { "Telangana", "Andhra Pradesh", "Karnataka", "Kerala", "Rajasthan" };

        private List<string> Districts = new List<string>() { "Ranga Reddy", "Medchal", "Kurnool", "Kadapa", "Lingampally" };

        public async Task<IEnumerable<string>> SearchStates(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return States;
            return States.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<IEnumerable<string>> SearchDistricts(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return Districts;
            return Districts.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        public void StartSearchAsync()
        {
        }

        public async Task GetOTP()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            await coWINAuthService.GenerateMobileOTPAsync(MobileNumber, cancellationToken);
            OTPField = false;
            OTPButton = false;
            StateHasChangedDelegate?.Invoke();
        }

        public async Task ValidateOTP()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            await coWINAuthService.ValidateMobileOTPAsync(OTP, cancellationToken);
        }
    }
}
