using Newtonsoft.Json;

namespace CoWINVaccineFinder.Application.DTOs
{
    public partial class VaccineFee
    {
        [JsonProperty("vaccine")]
        public string Vaccine { get; set; }

        [JsonProperty("fee")]
        public string Fee { get; set; }
    }
}
