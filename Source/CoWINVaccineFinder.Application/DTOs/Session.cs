using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoWINVaccineFinder.Application.DTOs
{
    public partial class Session
    {
        [JsonProperty("session_id")]
        public Guid SessionId { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("available_capacity")]
        public long AvailableCapacity { get; set; }

        [JsonProperty("available_capacity_dose1")]
        public long AvailableCapacityDose1 { get; set; }

        [JsonProperty("available_capacity_dose2")]
        public long AvailableCapacityDose2 { get; set; }

        [JsonProperty("min_age_limit")]
        public long MinAgeLimit { get; set; }

        [JsonProperty("vaccine")]
        public string Vaccine { get; set; }

        [JsonProperty("slots")]
        public List<string> Slots { get; set; }
    }
}
