using CoWINVaccineFinder.Application.DTOs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoWINVaccineFinder.Application
{
    public class CoWINAuthService
    {
        private readonly CoWINApiClient coWINApiClient;
        private CoWINUtilities coWinUtilities;
        private string transactionId = string.Empty;

        public CoWINAuthService(CoWINApiClient coWINApiClient, CoWINUtilities utilities)
        {
            this.coWINApiClient = coWINApiClient;
            this.coWinUtilities = utilities;
        }

        public async Task GenerateMobileOTPAsync(string mobileNumber, CancellationToken cancellationToken)
        {
            var generateMobileOTPRequest = new GenerateMobileOTPRequest();
            generateMobileOTPRequest.mobile = mobileNumber;
            generateMobileOTPRequest.secret = coWinUtilities.Secret;
            coWinUtilities.MobileNumber = mobileNumber;

            var generateMobileOTPResponse = await coWINApiClient.GenerateMobileOTPAsync(generateMobileOTPRequest, cancellationToken);
            transactionId = generateMobileOTPResponse.txnId;
        }

        public async Task ValidateMobileOTPAsync(string otp, CancellationToken cancellationToken)
        {
            var validateMobileOTPRequest = new ValidateMobileOTPRequest();
            validateMobileOTPRequest.otp = coWinUtilities.ComputeSha256Hash(otp);
            validateMobileOTPRequest.txnId = transactionId;

            var generateMobileOTPResponse = await coWINApiClient.ValidateMobileOTPAsync(validateMobileOTPRequest, cancellationToken);
            coWinUtilities.TokenText = generateMobileOTPResponse.token;
        }
    }
}
