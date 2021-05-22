using Newtonsoft.Json;

namespace CoWINVaccineFinder.Application.DTOs
{
    public partial class IdType
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("type_l")]
        public string TypeL { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }
}
