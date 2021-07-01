using System;
using System.Threading;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace SpeechSample
{
	public static class Luis
	{
		public static async Task<string> Analyze(string luisAppId, string endpointKey, string queryText)
		{
			var client = new HttpClient();
			var queryString = HttpUtility.ParseQueryString(string.Empty);

			// The request header contains your subscription key
			//client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", endpointKey);

			// The "q" parameter contains the utterance to send to LUIS
			queryString["q"] = queryText;

			// These optional request parameters are set to their default values
			queryString["timezoneOffset"] = "0";
			queryString["verbose"] = "false";
			queryString["spellCheck"] = "false";
			queryString["staging"] = "false";

			//var endpointUri = "https://dxlanguageunderstanding-demo.cognitiveservices.azure.com/luis/prediction/v3.0/apps/" + luisAppId + "?" + queryString;
			var endpointUri = $"https://dxlanguageunderstanding-demo.cognitiveservices.azure.com/luis/prediction/v3.0/apps/{luisAppId}/slots/staging/predict?subscription-key={endpointKey}&verbose=true&show-all-intents=true&log=true&query={queryString}";


			var response = await client.GetAsync(endpointUri);

			var strResponseContent = await response.Content.ReadAsStringAsync();
			return strResponseContent;
		}
	}
}
