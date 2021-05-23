using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoWINVaccineFinder.Application.DTOs
{
    public partial class Center
    {
        [JsonProperty("center_id")]
        public long CenterId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("state_name")]
        public string StateName { get; set; }

        [JsonProperty("district_name")]
        public string DistrictName { get; set; }

        [JsonProperty("block_name")]
        public string BlockName { get; set; }

        [JsonProperty("pincode")]
        public long Pincode { get; set; }

        [JsonProperty("lat")]
        public long Lat { get; set; }

        [JsonProperty("long")]
        public long Long { get; set; }

        [JsonProperty("from")]
        public DateTimeOffset From { get; set; }

        [JsonProperty("to")]
        public DateTimeOffset To { get; set; }

        [JsonProperty("fee_type")]
        public FeeType FeeType { get; set; }

        [JsonProperty("sessions")]
        public List<Session> Sessions { get; set; }

        [JsonProperty("vaccine_fees", NullValueHandling = NullValueHandling.Ignore)]
        public List<VaccineFee> VaccineFees { get; set; }
    }
}
