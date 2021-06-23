
using DXVision.BlazorApp.Server.Utils;
using DXVision.BlazorApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DXVision.BlazorApp.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImageAnalyzeController : ControllerBase
	{
		readonly IConfiguration config;
		readonly IMemoryCache cache;
		public ImageAnalyzeController(IConfiguration config, IMemoryCache cache)
		{
			this.cache = cache;
			this.config = config;
		}

		private async Task<ImageAnalysis> AnalyzeImage(Image image)
		{
			using (Stream data = ImageUtils.GetStream(image, ImageFormat.Jpeg))
			{
				ComputerVisionClient client = new ComputerVisionClient(
					new ApiKeyServiceClientCredentials(config.GetValue<string>("VisionApiKey"))
				)
				{
					Endpoint = config.GetValue<string>("VisionEndPoint")
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
				data.Seek(0, SeekOrigin.Begin);
				
				ImageAnalysis results = await client.AnalyzeImageInStreamAsync(data, features, details);
				return results;
			};
		}

		[HttpGet("[action]")]
		public async Task<AnalyzeResponse> Analyze([FromQuery]string fileid)
		{
			try
			{
				if (cache.TryGetValue<string>($"Upload_{fileid}", out string uploadedFile))
				{
					using (Stream fileStream = new FileStream(uploadedFile, FileMode.Open, FileAccess.Read))
					{
						var croppedImage = ImageUtils.ScaleImage(fileStream, 1024, 768);
						var result = await AnalyzeImage(croppedImage);
						
						return new AnalyzeResponse
						{
							ImageUrl = $"/uploads/{Path.GetFileName(uploadedFile)}",
							Result = result
						};
					}
				}
				else
				{
					throw new Exception("File was not found");
				}
			}
			catch
			{
				Response.StatusCode = 400;
				throw;
			}			
		}

	}

}
