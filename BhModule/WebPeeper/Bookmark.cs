using Newtonsoft.Json;

namespace BhModule.WebPeeper
{
	internal class Bookmark
	{
		[JsonProperty(/*Could not decode attribute arguments.*/)]
		public string Name;

		[JsonProperty(/*Could not decode attribute arguments.*/)]
		public string URL;
	}
}
