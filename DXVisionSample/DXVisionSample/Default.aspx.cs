using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.IO;
using System.Threading.Tasks;
using DevExpress.Web;
using System.Web.Hosting;
using Microsoft.ProjectOxford.Emotion;

namespace DXVisionSample
{
	public partial class Default : System.Web.UI.Page
	{

		const string apiKey = "ae2a477837f34d1bb0314b20705b36ab";
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void ASPxUploadControl1_FilesUploadComplete(object sender, DevExpress.Web.FilesUploadCompleteEventArgs e)
		{
			ASPxUploadControl ctrl = sender as ASPxUploadControl;
			if (ctrl != null) {
				AnalysisResult result = UploadAndAnalyzeImage(ctrl.UploadedFiles[0].FileContent);

				var desc = (from d in result.Description.Captions
							select $"{d.Text} ({d.Confidence})").ToArray();

				e.CallbackData = "<p>" + String.Join("<br />", desc) + "</p><p>Tags: " + String.Join(",", result.Description.Tags) + "</p>";
			}
			
		}

		private AnalysisResult UploadAndAnalyzeImage(Stream imageContent)
		{
			VisionServiceClient VisionServiceClient = new VisionServiceClient(apiKey);


			//var analysisResult = VisionServiceClient.DescribeAsync(imageContent);
			var analysisResult = VisionServiceClient.AnalyzeImageAsync(imageContent, new VisualFeature[] {
				VisualFeature.Faces,
				VisualFeature.Adult,
				VisualFeature.Categories,
				VisualFeature.Color,
				VisualFeature.Description,
				VisualFeature.Tags
			});

			Task.WaitAll(analysisResult);

			return analysisResult.Result;			
		}

		private Microsoft.ProjectOxford.Emotion.Contract.Emotion[] AnalyzeEmotions(Stream imageContent)
		{
			EmotionServiceClient client = new EmotionServiceClient(apiKey);
			var result = client.RecognizeAsync(imageContent);
			Task.WaitAll(result);

			return result.Result;
		}
	}
}