using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;

namespace SpeechSample
{

	class Program
	{
		static readonly string luisAppId = "0879e4fd-8d0d-4608-87db-00e68c371cee";
		static readonly string luisKey = "59990945ef334679b426b6a365957443";
		static readonly string bingKey = "c2abbdbf7988481697718ff4956d2ac9";
		
		static readonly SpeechConfig config = SpeechConfig.FromSubscription("2421b8acd9584cec8fa566d4304e7afd", "westus");
		
		
		static async Task Main(string[] args)
		{

			/*
			SpeechResult<string> spkResult = await Speech.Speak(config, "Hi, how can I help you?");
			if (!spkResult.Success)
			{
				Console.WriteLine("I can't speak");
				Console.WriteLine($"Reason: {spkResult.ErrorCode} / {spkResult.ErrorDetails}");
			}
			else
			{
				//var result = await Speech.Listen(config);
				var result = new SpeechResult<string>(true, "I want to book a flight from Amsterdam to London next week");
				if (result.Success)
				{
					Console.WriteLine($"You said:{result.Result}");
					spkResult = await Speech.Speak(config, $"You said:{result.Result}");
				}
				else
				{
					Console.WriteLine($"Error: {result.Result}");
				}
			}
			*/
			//string str = await Luis.Analyze(luisAppId, luisKey, "Show me some pictures of Kansas City");
			//string str = await Luis.Analyze(luisAppId, luisKey, "I want to book a flight from Amsterdam to London next week");
			//string str = SampleJSON.Data; 
			//LuisResponse resp = JsonConvert.DeserializeObject<LuisResponse>(str);
			//Console.WriteLine(str);
			
			var imageResults = await Bing.ImageSearchAsync(bingKey, "Kansas City Conference Centre");
			if (imageResults != null)
			{
				
				var firstImageResult = imageResults.Value.First();
				Bing.OpenBrowser(firstImageResult.ContentUrl);
				//Console.WriteLine($"\nTotal number of returned images: {imageResults.Value.Count}\n");
				//Console.WriteLine($"Copy the following URLs to view these images on your browser.\n");
				//Console.WriteLine($"URL to the first image:\n\n {firstImageResult.ContentUrl}\n");
				//Console.WriteLine($"Thumbnail URL for the first image:\n\n {firstImageResult.ThumbnailUrl}");
				//Console.ReadKey();
			}


			
			Console.WriteLine("Please press a key to continue.");
			Console.ReadLine();
		}
	}
}
