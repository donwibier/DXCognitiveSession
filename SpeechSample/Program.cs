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
		static readonly string luisAppId = "302732a3-ef55-43b6-80d1-27709f7578cb";
		static readonly string luisKey = "ec2cf4cb444945708c09ab8f16805a12";
		static readonly string bingKey = "9f0af4eefd0742c0bd2b9de2641a0a87";
		
		static readonly SpeechConfig config = SpeechConfig.FromSubscription("de61811d40ef4d1eb8cd7f134e9ad56a", "westus");
		
		
		static async Task Main(string[] args)
		{
			var cancelled = false;

			Console.WriteLine("Starting up");
			SpeechResult<string> spkResult = await Speech.Speak(config, "Hi, welcome to DWX Nuremberg conference.");
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
						
						Console.WriteLine(JsonPrint.Prettify(str));
						LuisResponse resp = JsonConvert.DeserializeObject<LuisResponse>(str);
						var intent = (resp.Prediction.TopIntent ?? "").ToLower();
						var score = (resp.Prediction.Intents.Score?.Score ?? 0);

						string departure = resp.Prediction.Entities.Instance.Departure?.FirstOrDefault()?.Text ?? "";
						string destination = resp.Prediction.Entities.Instance.Destination?.FirstOrDefault()?.Text ?? "";

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
