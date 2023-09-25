using Newtonsoft.Json;

namespace felix.BlishEmotes
{
	internal class JsonVersionWrapper<T>
	{
		[JsonProperty("version", Required = Required.Always)]
		public string Version { get; set; }

		[JsonProperty("data", Required = Required.Always)]
		public T Data { get; set; }

		public JsonVersionWrapper(string version, T data)
		{
			Version = version;
			Data = data;
		}
	}
}
