using Newtonsoft.Json;

namespace CoWINVaccineFinder.Application.DTOs
{
    public partial class District
    {
        [JsonProperty("state_id")]
        public long StateId { get; set; }

        [JsonProperty("district_id")]
        public long DistrictId { get; set; }

        [JsonProperty("district_name")]
        public string DistrictName { get; set; }

        [JsonProperty("district_name_l")]
        public string DistrictNameL { get; set; }
    }
}
