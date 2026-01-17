using Newtonsoft.Json;

namespace BhModule.WebPeeper
{
	public class Bookmark
	{
		[JsonProperty(/*Could not decode attribute arguments.*/)]
		public string Name;

		[JsonProperty(/*Could not decode attribute arguments.*/)]
		public string URL;
	}
}
