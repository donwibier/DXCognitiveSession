using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using DevExpress.Web;
using System.Web.Hosting;
using DXVisionSample.Code;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace DXVisionSample
{
	public partial class Default : System.Web.UI.Page
	{

		const string apiKey = "840c87b723e64cd18c85ebf7ee318c49";
		const string apiEndPoint = "https://westcentralus.api.cognitive.microsoft.com/";
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void ASPxUploadControl1_FilesUploadComplete(object sender, DevExpress.Web.FilesUploadCompleteEventArgs e)
		{
			ASPxUploadControl ctrl = sender as ASPxUploadControl;
			if (ctrl != null) {
				var result = UploadAndAnalyzeImage(ctrl.UploadedFiles[0].FileBytes).Result;
				
				var resultObj = Newtonsoft.Json.JsonConvert.SerializeObject(result);

				var desc = (from d in result.Description.Captions
								select $"{d.Text} ({d.Confidence})").ToArray();

				//e.CallbackData = "<p>" + String.Join("<br />", desc) + "</p><p>Tags: " + String.Join(",", resultObj.Description.Tags) + "</p>";
                e.CallbackData = JsonPrint.Prettify(resultObj);
			}
			
		}

		private async Task<ImageAnalysis> UploadAndAnalyzeImage(byte[] imageBytes)
		{
			using (Stream data = ImageUtils.GetStream(ImageUtils.ScaleImage(ImageUtils.CreateImage(imageBytes), 1024, 768), System.Drawing.Imaging.ImageFormat.Png))
			{
				ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(apiKey))
				{
					Endpoint = apiEndPoint
				};
				List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
				{
					VisualFeatureTypes.Categories,
					VisualFeatureTypes.Description,
					VisualFeatureTypes.Faces,
					VisualFeatureTypes.ImageType,
					VisualFeatureTypes.Tags,
					VisualFeatureTypes.Adult,
					VisualFeatureTypes.Color,
					VisualFeatureTypes.Brands,
					VisualFeatureTypes.Objects
				};

				List<Details?> details = new List<Details?>() { 
					Details.Celebrities,
					Details.Landmarks
				};

				ImageAnalysis results = await client.AnalyzeImageInStreamAsync(data, features, details);
				return results;
			};		
		}
	}
}