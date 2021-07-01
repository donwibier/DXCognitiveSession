using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SpeechSample
{
    public class LuisScore
    {
        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public class LuisIntents
    {
        [JsonProperty("None")]
        public LuisScore Score { get; set; }
    }

    public class LuisResolution
    {
        [JsonProperty("start")]
        public string Start { get; set; }

        [JsonProperty("end")]
        public string End { get; set; }
    }

    public class LuisValue
    {
        [JsonProperty("timex")]
        public string Timex { get; set; }

        [JsonProperty("resolution")]
        public List<LuisResolution> Resolution { get; set; }
    }

    public class LuisDatetimeV2
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("values")]
        public List<LuisValue> Values { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("modelTypeId")]
        public int ModelTypeId { get; set; }

        [JsonProperty("modelType")]
        public string ModelType { get; set; }

        [JsonProperty("recognitionSources")]
        public List<string> RecognitionSources { get; set; }
    }

    public class LuisDeparture
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("modelTypeId")]
        public int ModelTypeId { get; set; }

        [JsonProperty("modelType")]
        public string ModelType { get; set; }

        [JsonProperty("recognitionSources")]
        public List<string> RecognitionSources { get; set; }
    }

    public class LuisDestination
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("modelTypeId")]
        public int ModelTypeId { get; set; }

        [JsonProperty("modelType")]
        public string ModelType { get; set; }

        [JsonProperty("recognitionSources")]
        public List<string> RecognitionSources { get; set; }
    }

    public class LuisInstance
    {
        [JsonProperty("departure")]
        public List<LuisDeparture> Departure { get; set; }

        [JsonProperty("destination")]
        public List<LuisDestination> Destination { get; set; }

        [JsonProperty("datetimeV2")]
        public List<LuisDatetimeV2> DatetimeV2 { get; set; }
    }

    public class LuisEntities
    {
        [JsonProperty("departure")]
        public List<string> Departure { get; set; }

        [JsonProperty("destination")]
        public List<string> Destination { get; set; }

        [JsonProperty("datetimeV2")]
        public List<LuisDatetimeV2> DatetimeV2 { get; set; }

        [JsonProperty("$instance")]
        public LuisInstance Instance { get; set; }
    }

    public class LuisPrediction
    {
        [JsonProperty("topIntent")]
        public string TopIntent { get; set; }

        [JsonProperty("intents")]
        public LuisIntents Intents { get; set; }

        [JsonProperty("entities")]
        public LuisEntities Entities { get; set; }
    }

    public class LuisResponse
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("prediction")]
        public LuisPrediction Prediction { get; set; }
    }
}
