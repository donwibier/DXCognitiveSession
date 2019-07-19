using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;

namespace SpeechSample
{

	class Program
	{
		static readonly string luisAppId = "416311e4-e365-4cba-9f6a-63eba0e4bf58";
		static readonly string luisKey = "59990945ef334679b426b6a365957443";
		static readonly string bingKey = "c2abbdbf7988481697718ff4956d2ac9";
		
		static readonly SpeechConfig config = SpeechConfig.FromSubscription("2421b8acd9584cec8fa566d4304e7afd", "westus");
		
		
		static async Task Main(string[] args)
		{
			var cancelled = false;

			Console.WriteLine("Starting up");
			SpeechResult<string> spkResult = await Speech.Speak(config, "Hi, how can I help you?");
			if (!spkResult.Success)
			{
				Console.WriteLine("I can't speak");
				Console.WriteLine($"Reason: {spkResult.ErrorCode} / {spkResult.ErrorDetails}");
				return;
			}

			while (!cancelled)
			{
				Console.WriteLine("Listening");
				var result = await Speech.Listen(config);
				if (result.Success)
				{
					var dontknow = true;
					Console.WriteLine($"You said:{result.Result}");
					if (result.Result.ToLower().Contains("quit"))
					{
						cancelled = true;
						break;
					}
					else if (result.Result != "")
					{
						//string str = SampleJSON.Data;
						string str = await Luis.Analyze(luisAppId, luisKey, result.Result);
						LuisResponse resp = JsonConvert.DeserializeObject<LuisResponse>(str);
						Console.WriteLine(str);
						var intent = (resp.TopScoringIntent?.Intent ?? "").ToLower();
						var score = (resp.TopScoringIntent?.Score ?? 0);

						string departure = resp.Entities.FirstOrDefault(x => x.Type == "departure")?.Entity ?? "";
						string destination = resp.Entities.FirstOrDefault(x => x.Type == "destination")?.Entity ?? "";

						if (score > 0.2)
						{
							if (intent == "bookflight")
							{
								dontknow = false;
								spkResult = await Speech.Speak(config, $"Once I know how, I will search flights from {departure} to {destination}");

							}
							else if (intent == "showdestination")
							{
								dontknow = false;
								var imageResults = await Bing.ImageSearchAsync(bingKey, destination);
								if (imageResults != null)
								{
									var firstImageResult = imageResults.Value.First();
									Bing.OpenBrowser(firstImageResult.ContentUrl);
								}
							}
							else if (intent == "gettemperature")
							{
								dontknow = false;
								spkResult = await Speech.Speak(config, $"You want to know the temperature in {destination}");
							}
						}

						if (dontknow && result.Result != "")
						{
							spkResult = await Speech.Speak(config, "Sorry, I don't know what you mean");
							dontknow = false;
						}
					}
				}
				else
				{
					Console.WriteLine($"Error: {result.Result}");
				}
			}			

			Console.WriteLine("Please press a key to continue.");
			Console.ReadLine();
		}
	}
}
