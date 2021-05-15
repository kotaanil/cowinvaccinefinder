using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CoWINVaccineFinder.Application.DTOs
{
    public partial class ResponseDto
    {
        [JsonProperty("centers")]
        public List<Center> Centers { get; set; }
    }

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

    public partial class Session
    {
        [JsonProperty("session_id")]
        public Guid SessionId { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("available_capacity")]
        public long AvailableCapacity { get; set; }

        [JsonProperty("min_age_limit")]
        public long MinAgeLimit { get; set; }

        [JsonProperty("vaccine")]
        public Vaccine Vaccine { get; set; }

        [JsonProperty("slots")]
        public List<string> Slots { get; set; }
    }

    public partial class VaccineFee
    {
        [JsonProperty("vaccine")]
        public Vaccine Vaccine { get; set; }

        [JsonProperty("fee")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Fee { get; set; }
    }

    public enum FeeType { Free, Paid };

    public enum Vaccine { Covaxin, Covishield };

    public partial class ResponseDto
    {
        public static ResponseDto FromJson(string json) => JsonConvert.DeserializeObject<ResponseDto>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ResponseDto self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                FeeTypeConverter.Singleton,
                VaccineConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class FeeTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(FeeType) || t == typeof(FeeType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Free":
                    return FeeType.Free;
                case "Paid":
                    return FeeType.Paid;
            }
            throw new Exception("Cannot unmarshal type FeeType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (FeeType)untypedValue;
            switch (value)
            {
                case FeeType.Free:
                    serializer.Serialize(writer, "Free");
                    return;
                case FeeType.Paid:
                    serializer.Serialize(writer, "Paid");
                    return;
            }
            throw new Exception("Cannot marshal type FeeType");
        }

        public static readonly FeeTypeConverter Singleton = new FeeTypeConverter();
    }

    internal class VaccineConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Vaccine) || t == typeof(Vaccine?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "COVAXIN":
                    return Vaccine.Covaxin;
                case "COVISHIELD":
                    return Vaccine.Covishield;
            }
            throw new Exception("Cannot unmarshal type Vaccine");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Vaccine)untypedValue;
            switch (value)
            {
                case Vaccine.Covaxin:
                    serializer.Serialize(writer, "COVAXIN");
                    return;
                case Vaccine.Covishield:
                    serializer.Serialize(writer, "COVISHIELD");
                    return;
            }
            throw new Exception("Cannot marshal type Vaccine");
        }

        public static readonly VaccineConverter Singleton = new VaccineConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
