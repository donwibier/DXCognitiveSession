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
using DXVisionSample.Code.Cognitive;
using DXVisionSample.Code;

namespace DXVisionSample
{
	public partial class Default : System.Web.UI.Page
	{

		const string apiKey = "8701c7f68f9f4cd8be0bd99fe3ae12b0";
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void ASPxUploadControl1_FilesUploadComplete(object sender, DevExpress.Web.FilesUploadCompleteEventArgs e)
		{
			ASPxUploadControl ctrl = sender as ASPxUploadControl;
			if (ctrl != null) {
				var result = UploadAndAnalyzeImage(ctrl.UploadedFiles[0].FileBytes);
				
				if (String.IsNullOrEmpty(result))
				{
					e.CallbackData = "<p>Error analyzing image</p>";
				}
				else
				{
					var resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<VisionResponse>(result);

					var desc = (from d in resultObj.Description.Captions
								select $"{d.Name} ({d.Score})").ToArray();

					//e.CallbackData = "<p>" + String.Join("<br />", desc) + "</p><p>Tags: " + String.Join(",", resultObj.Description.Tags) + "</p>";
                    e.CallbackData = JsonPrint.Prettify(result);
                    
				}
			}
			
		}

		private string UploadAndAnalyzeImage(byte[] imageBytes)
		{
			VisionClient clnt = new VisionClient(VisionLocation.WestCentralUS, apiKey);

			string analysisResult = clnt.Analyze(imageBytes, new VisionFeatures[] {
				VisionFeatures.Faces,
				VisionFeatures.Adult,
				VisionFeatures.Categories,
				VisionFeatures.Color,
				VisionFeatures.Description,
				VisionFeatures.Tags
			}, 
			new VisionDetails[] {
				VisionDetails.Celebrities
			});
			
			return analysisResult;			
		}
	}
}