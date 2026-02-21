using Newtonsoft.Json;

namespace CinemaModule.Models
{
	public class StreamPresetData : StreamDataBase
	{
		private string _name;

		[JsonProperty("id")]
		public override string Id { get; set; }

		[JsonProperty("name")]
		public string NameValue
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		[JsonIgnore]
		public override string Name => _name;

		[JsonProperty("type")]
		public override string TypeString { get; set; } = "video";


		[JsonProperty("url")]
		public override string Url { get; set; }

		[JsonProperty("avatar")]
		public string Avatar { get; set; }

		[JsonProperty("infoUrl")]
		public override string InfoUrl { get; set; }

		[JsonProperty("staticImage")]
		public override string StaticImage { get; set; }

		[JsonProperty("asylumInfo")]
		public bool AsylumInfo { get; set; }
	}
}
