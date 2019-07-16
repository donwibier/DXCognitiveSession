using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SpeechSample
{
	public class LuisResponse
	{
		[JsonProperty("query")]
		public string Query { get; set; }
		[JsonProperty("topScoringIntent")]
		public LuisIntent TopScoringIntent { get; set; }
		[JsonProperty("entities")]
		public LuisEntity[] Entities { get; set;}
	}

	public class LuisIntent
	{
		[JsonProperty("intent")]
		public string Intent { get; set; }
		[JsonProperty("score")]
		public double Score { get; set; }
	}

	public class LuisEntity
	{
		[JsonProperty("entity")]
		public string Entity { get; set; }
		[JsonProperty("type")]
		public string Type { get; set; }
		[JsonProperty("startIndex")]
		public int StartIndex { get; set; }
		[JsonProperty("endIndex")]
		public int EndIndex { get; set; }
		[JsonProperty("score")]
		public double Score { get; set; }
		[JsonProperty("resolution")]
		public LuisEntityResolution Resolution { get; set; }
	}

	public class LuisEntityResolution
	{
		[JsonProperty("values")]
		public IEnumerable<IDictionary<string, string>> Values { get; set; }
	}

	public static class SampleJSON
	{
		public const string Data = @"{
  ""query"": ""I want to book a flight from Amsterdam to London next week"",
  ""topScoringIntent"": {
    ""intent"": ""BookFlight"",
    ""score"": 0.969691
  },
  ""entities"": [
    {
      ""entity"": ""next week"",
      ""type"": ""builtin.datetimeV2.daterange"",
      ""startIndex"": 49,
      ""endIndex"": 57,
      ""resolution"": {
        ""values"": [
          {
            ""timex"": ""2019-W30"",
            ""type"": ""daterange"",
            ""start"": ""2019-07-22"",
            ""end"": ""2019-07-29""

		  }
        ]
      }
    },
    {
      ""entity"": ""amsterdam"",
      ""type"": ""departure"",
      ""startIndex"": 29,
      ""endIndex"": 37,
      ""score"": 0.967211
    },
    {
      ""entity"": ""london"",
      ""type"": ""destination"",
      ""startIndex"": 42,
      ""endIndex"": 47,
      ""score"": 0.9096222
    }
  ]
}";
	}
}
