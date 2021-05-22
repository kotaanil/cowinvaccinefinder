using CoWINVaccineFinder.Application.DTOs;
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
    public class CoWINMetadataService
    {
        private readonly CoWINApiClient coWINApiClient; 
        private CoWINUtilities coWINUtilities;

        public CoWINMetadataService(CoWINApiClient coWINApiClient, CoWINUtilities utilities)
        {
            this.coWINApiClient = coWINApiClient;
            this.coWINUtilities = utilities;
        }

        public async Task<FetchStatesResponse> GetStates(CancellationToken cancellationToken)
        {
            return await this.coWINApiClient.FetchStates(cancellationToken);
        }

        public async Task<FetchDistrictsResponse> GetDistricts(int stateID, CancellationToken cancellationToken)
        {
            var queryString = new Dictionary<string, string>();
            queryString.Add("state_id" , stateID.ToString());
            return await this.coWINApiClient.FetchDistricts(queryString, coWINUtilities.TokenText,cancellationToken);
        }

        public async Task<FetchIdTypesResponse> GetBeneficiaryIdTypes( CancellationToken cancellationToken)
        {
            return await this.coWINApiClient.FetchIdTypes(coWINUtilities.TokenText, cancellationToken);
        }

        public async Task<FetchGendersResponse> GetBeneficiaryGenders( CancellationToken cancellationToken)
        {
            return await this.coWINApiClient.FetchGenders(coWINUtilities.TokenText, cancellationToken);
        }
    }
}
