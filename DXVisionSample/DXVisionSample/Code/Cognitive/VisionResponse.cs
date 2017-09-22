using Newtonsoft.Json;

namespace DXVisionSample.Code.Cognitive
{
	public class VisionResponse
	{
		[JsonProperty("requestId")]
		public string RequestId { get; set; }
		[JsonProperty("code")]
		public string ErrorCode { get; set; }
		[JsonProperty("message")]
		public int ErrorMessage { get; set; }
		
		[JsonProperty("categories")]
		public VisionResponseCategory[] Categories { get; set; }

		[JsonProperty("adult")]
		public VisionResponseAdult Adult { get; set; }

		[JsonProperty("tags")]
		public VisionResponseTag[] Tags { get; set; }

		[JsonProperty("description")]
		public VisionResponseDescription Description { get; set; }

		[JsonProperty("metadata")]
		public VisionResponseMetadata Metadata { get; set; }

		[JsonProperty("faces")]
		public VisionResponseFace[] Faces { get; set; }

		[JsonProperty("color")]
		public VisionResponseColor Color { get; set; }

		[JsonProperty("imageType")]
		public VisionResponseImageType ImageType { get; set; }
	}

	public class VisionResponseBaseName
	{
		[JsonProperty("name")]
		public virtual string Name { get; set; }

	}
	public class VisionResponseBaseNameScore : VisionResponseBaseName
	{
		[JsonProperty("score")]
		public virtual double Score { get; set; }
	}
	public class VisionResponseBaseNameConfidence : VisionResponseBaseNameScore
	{
		
		
		[JsonProperty("confidence")]
		public override double Score { get => base.Score; set => base.Score = value; }
	}

	public class VisionResponseCategory : VisionResponseBaseNameScore
	{
		[JsonProperty("detail")]
		public virtual VisionResponseCategoryDetail Detail { get; set; }

	}

	public class VisionResponseCategoryDetail
	{
		[JsonProperty("celebrities")]
		public VisionResponseCategoryDetailCeleb[] Celebs { get; set; }

		[JsonProperty("landmarks")]
		public VisionResponseCategoryDetailLandmark[] Landmarks { get; set; }

	}

	public class VisionResponseRectangle {
		[JsonProperty("top")]
		public int Top { get; set; }
		[JsonProperty("left")]
		public int Left { get; set; }
		[JsonProperty("width")]
		public int Width { get; set; }
		[JsonProperty("height")]
		public int Height { get; set; }
	}
	public class VisionResponseCategoryDetailCeleb : VisionResponseBaseNameConfidence
	{
		[JsonProperty("faceRectangle")]
		public VisionResponseRectangle Rectangle { get; set; }
	}

	public class VisionResponseCategoryDetailLandmark : VisionResponseBaseNameConfidence
	{

	}

	public class VisionResponseAdult
	{
		[JsonProperty("isAdultContent"), JsonConverter(typeof(BooleanJsonConverter))]
		public bool IsAdultContent { get; set; }
		[JsonProperty("isRacyContent"), JsonConverter(typeof(BooleanJsonConverter))]
		public bool IsRacyContent { get; set; }
		[JsonProperty("adultScore")]
		public double AdultScore { get; set; }
		[JsonProperty("racyScore")]
		public double RacyScore { get; set; }
	}

	public class VisionResponseTag : VisionResponseBaseNameConfidence
	{

	}

	public class VisionResponseDescriptionCaption : VisionResponseBaseNameConfidence
	{
		[JsonProperty("text")]
		public override string Name { get => base.Name; set => base.Name = value; }

	}

	public class VisionResponseDescription
	{
		[JsonProperty("tags")]
		public string[] Tags { get; set; }
		[JsonProperty("captions")]
		public VisionResponseDescriptionCaption[] Captions { get; set; }
	}

	public class VisionResponseMetadata
	{
		[JsonProperty("width")]
		public int Width { get; set; }
		[JsonProperty("height")]
		public int Height { get; set; }
		[JsonProperty("format")]
		public string Format { get; set; }
	}

	public class VisionResponseFace
	{
		[JsonProperty("age")]
		public int Age { get; set; }
		[JsonProperty("gender")]
		public string Gender { get; set; }
		[JsonProperty("faceRectangle")]
		public VisionResponseRectangle Rectangle { get; set; }
	}

	public class VisionResponseColor
	{
		[JsonProperty("dominantColorForeground")]
		public string DominantColorForeground { get; set; }
		[JsonProperty("dominantColorBackground")]
		public string DominantColorBackground { get; set; }
		[JsonProperty("dominantColors")]
		public string[] DominantColors { get; set; }
		[JsonProperty("accentColor")]
		public string AccentColor { get; set; }
		[JsonProperty("isBWImg"), JsonConverter(typeof(BooleanJsonConverter))]
		public bool IsBWImg { get; set; }
	}

	public class VisionResponseImageType
	{
		[JsonProperty("clipArtType")]
		public int ClipArtType { get; set; }
		[JsonProperty("lineDrawingType")]
		public int LineDrawingType { get; set; }
	}
}
