using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace DXVisionSample.Code.Cognitive
{
	//public enum VisionLocation
	//{
	//	None,
	//	WestUS,
	//	EastUS2,
	//	WestCentralUS,
	//	WestEurope,
	//	SouthEastAsia
	//}

	//public enum VisionFeatures
	//{
	//	Categories, //categorizes image content according to a taxonomy defined in documentation.
	//	Tags, //tags the image with a detailed list of words related to the image content.
	//	Description, //describes the image content with a complete English sentence.
	//	Faces, //detects if faces are present. If present, generate coordinates, gender and age.
	//	ImageType, //detects if image is clipart or a line drawing.
	//	Color, //determines the accent color, dominant color, and whether an image is black&white.
	//	Adult //detects if the image is pornographic in nature (depicts nudity or a sex act). Sexually suggestive content is also detected.
	//}

	//public enum VisionLanguage
	//{
	//	en,//English
	//	zh //Simplified Chinese
	//}

	//public enum VisionDetails
	//{
	//	Celebrities,
	//	Landmarks
	//}

	//public class VisionClient
	//{
	//	private VisionLocation _Location = VisionLocation.None;
	//	private string _Key = String.Empty;		

	//	private Uri getVisionUrl(VisionFeatures[] features, VisionDetails[] details, VisionLanguage language = VisionLanguage.en)
	//	{
	//		string result = $"https://{_Location}.api.cognitive.microsoft.com/vision/v1.0/analyze";
	//		List<string> options = new List<string>();
	//		if ((features != null) && (features.Length > 0))
	//			options.Add(String.Format("visualFeatures={0}", String.Join(",", features)));
	//		if ((details != null) && (details.Length > 0))
	//			options.Add(String.Format("details={0}", String.Join(",", details)));
	//		if (language != VisionLanguage.en)
	//			options.Add(String.Format("language={0}", language));

	//		result = String.Format("{0}{1}{2}", result.ToLower(), options.Count > 0 ? "?" : "", options.Count > 0 ? String.Join("&", options) : "");
	//		return new Uri(result);
	//	}
	//	public VisionClient(VisionLocation location, string key)
	//	{
	//		if (location == VisionLocation.None)
	//			throw new ArgumentException("Location unspecified");
	//		if (String.IsNullOrEmpty(key))
	//			throw new ArgumentNullException("key");

	//		_Location = location;
	//		_Key = key;


	//	}

	//	public VisionLocation Location { get { return _Location; } }
	//	public string Key { get { return _Key; } }

	//	public string Analyze(Uri imageUri, VisionFeatures[] features, VisionDetails[] details, VisionLanguage language = VisionLanguage.en)
	//	{
	//		var client = new HttpClient();
	//		client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Key);
	//		using (StringContent content = new StringContent(String.Format("{{\"url\" : \"{0}\"}}", imageUri)))
	//		{				
	//			// Execute the REST API call.
	//			var url = getVisionUrl(features, details, language);
	//			var response = client.PostAsync(url, content);
	//			Task.WaitAll(response);
	//			// Get the JSON response.
	//			var responseContent = response.Result.Content.ReadAsStringAsync();
	//			Task.WaitAll(responseContent);
	//			return responseContent.Result;
	//		}
	//	}

	//	public string Analyze(byte[] imageData, VisionFeatures[] features, VisionDetails[] details, VisionLanguage language = VisionLanguage.en)
	//	{			
	//		HttpClient client = new HttpClient();
	//		client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Key);
	//		using (ByteArrayContent content = new ByteArrayContent(imageData))
	//		{
	//			content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
	//			// Execute the REST API call.
	//			var url = getVisionUrl(features, details, language);
	//			var response = client.PostAsync(url, content);
	//			Task.WaitAll(response);
	//			// Get the JSON response.
	//			var responseContent = response.Result.Content.ReadAsStringAsync();
	//			Task.WaitAll(responseContent);
	//			return responseContent.Result;
	//		}
	//	}

	//}
	#region boolean JsonConverter
	public class BooleanJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(bool);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			switch (reader.Value.ToString().ToLower().Trim())
			{
				case "true":
				case "yes":
				case "y":
				case "1":
					return true;
				case "false":
				case "no":
				case "n":
				case "0":
					return false;
			}

			// If we reach here, we're pretty much going to throw an error so let's let Json.NET throw it's pretty-fied error message.
			return new JsonSerializer().Deserialize(reader, objectType);
		}

		public override bool CanWrite { get { return false; } }

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}
	}
	#endregion

}