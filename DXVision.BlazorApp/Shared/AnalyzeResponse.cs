using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;


namespace DXVision.BlazorApp.Shared
{
	public class AnalyzeResponse
	{
		public string ImageUrl { get; set; }
		public ImageAnalysis Result { get; set; }

	}

}
