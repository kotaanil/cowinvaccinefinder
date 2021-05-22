using CoWINVaccineFinder.Application;
using CoWINVaccineFinder.Application.DTOs;
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
        private CoWINMetadataService coWINMetadataService;
        private CoWINAppointmentService coWINAppointmentService;
        public IndexViewModel(CoWINAuthService coWINAuthService, 
            CoWINMetadataService coWINMetadataService, 
            CoWINAppointmentService coWINAppointmentService)
        {
            DisableOTPField = true;
            DisableOTPButton = true;
            HideLogoutDetails = true;
            HideLoginDetails = false;
            HideSearchCriteria = true;
            HideSearchResults = true;
            this.coWINAuthService = coWINAuthService;
            this.coWINMetadataService = coWINMetadataService;
            this.coWINAppointmentService = coWINAppointmentService;
            AvailableVaccineCenters = new List<VaccineCenter>();
        }

        public Action StateHasChangedDelegate { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string MobileNumber { get; set; }
        public string OTP { get; set; }
        public bool DisableOTPField { get; set; }
        public bool DisableOTPButton { get; set; }
        public bool HideLoginDetails { get; set; }
        public bool HideLogoutDetails { get; set; }
        public bool HideSearchCriteria { get; set; }
        public bool HideSearchResults { get; set; }

        public List<VaccineCenter> AvailableVaccineCenters { get; set; }

        public List<string> States { get; set; }

        private List<string> Districts { get; set; }

        public List<State> StatesDTO { get; set; }
        public List<District> DistrictsDTO { get; set; }

        public async Task<IEnumerable<string>> SearchStates(string value)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            StatesDTO = await coWINMetadataService.GetStates(cancellationToken);
            States = StatesDTO.Select(x => x.StateName).ToList();
            
            if (string.IsNullOrEmpty(value))
                return States;
            return States.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<IEnumerable<string>> SearchDistricts(string value)
        {
            var cancellationToken = new CancellationTokenSource().Token;
            DistrictsDTO = await coWINMetadataService.
                GetDistricts(StatesDTO.Where(x => x.StateName == State).FirstOrDefault().StateId.ToString(),cancellationToken);
            Districts = DistrictsDTO.Select(x => x.DistrictName).ToList();

            if (string.IsNullOrEmpty(value))
                return Districts;
            return Districts.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task StartSearchAsync()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            var sessionsResponse = await coWINAppointmentService.FetchSessionsByDistrictIdAndDate(
                DistrictsDTO.Where(x => x.DistrictName == District).FirstOrDefault().DistrictId.ToString(), date, cancellationToken);

            foreach (var center in sessionsResponse.Centers)
            {
                if (center != null)
                {
                    foreach (var session in center.Sessions)
                    {
                        if(session != null)
                        {
                            var vaccineCenter = new VaccineCenter();
                            vaccineCenter.CenterName = center.Name;
                            vaccineCenter.Availability = session.AvailableCapacity;
                            vaccineCenter.MinimumAgeLimit = session.MinAgeLimit;
                            vaccineCenter.VaccineType = session.Vaccine;
                            AvailableVaccineCenters.Add(vaccineCenter);
                        }
                        
                    }
                }
            }
            
            HideSearchResults = false;
            StateHasChangedDelegate?.Invoke();
        }

        public async Task GetOTP()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            await coWINAuthService.GenerateMobileOTPAsync(MobileNumber, cancellationToken);
            DisableOTPField = false;
            DisableOTPButton = false;
            StateHasChangedDelegate?.Invoke();
        }

        public async Task ValidateOTP()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            await coWINAuthService.ValidateMobileOTPAsync(OTP, cancellationToken);
            HideLoginDetails = true;
            HideLogoutDetails = false;
            HideSearchCriteria = false;
            StateHasChangedDelegate?.Invoke();
        }
    }
}
