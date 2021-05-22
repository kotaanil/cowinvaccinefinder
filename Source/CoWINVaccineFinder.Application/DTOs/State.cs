using Newtonsoft.Json;

namespace CoWINVaccineFinder.Application.DTOs
{
    public class State
    {
        [JsonProperty("state_id")]
        public long StateId { get; set; }
        [JsonProperty("state_name")]
        public string StateName { get; set; }
    }
}
