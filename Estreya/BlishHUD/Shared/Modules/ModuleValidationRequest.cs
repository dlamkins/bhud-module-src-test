using Estreya.BlishHUD.Shared.Json.Converter;
using Newtonsoft.Json;
using SemVer;

namespace Estreya.BlishHUD.Shared.Modules
{
	public struct ModuleValidationRequest
	{
		[JsonProperty("version")]
		[JsonConverter(typeof(SemanticVersionConverter))]
		public Version Version;
	}
}
