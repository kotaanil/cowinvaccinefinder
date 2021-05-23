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

        public async Task<List<State>> GetStates(CancellationToken cancellationToken)
        {
            var fetchStatesResponse =  await this.coWINApiClient.FetchStates(coWINUtilities.TokenText, cancellationToken);
            return fetchStatesResponse.States;
        }

        public async Task<List<District>> GetDistricts(string stateId, CancellationToken cancellationToken)
        {
            var fetchDistrictsResponse = await this.coWINApiClient.FetchDistricts(stateId, coWINUtilities.TokenText, cancellationToken);
            return fetchDistrictsResponse.Districts;
        }

        public async Task<List<IdType>> GetBeneficiaryIdTypes(CancellationToken cancellationToken)
        {
            var fetchIdTypesResponse = await  this.coWINApiClient.FetchIdTypes(
                coWINUtilities.TokenText, cancellationToken);
            return fetchIdTypesResponse.Types;
        }

        public async Task<List<Gender>> GetBeneficiaryGenders(CancellationToken cancellationToken)
        {
            var fetchGendersResponse =  await this.coWINApiClient.FetchGenders(
                coWINUtilities.TokenText, cancellationToken);
            return fetchGendersResponse.Genders;
        }
    }
}
