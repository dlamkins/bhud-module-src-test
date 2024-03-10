using System.Linq;
using System.Text.Json;

namespace Estreya.BlishHUD.Shared.Json.Converter.GW2Sharp
{
	public class SnakeCaseNamingPolicy : JsonNamingPolicy
	{
		public static SnakeCaseNamingPolicy SnakeCase => new SnakeCaseNamingPolicy();

		public override string ConvertName(string name)
		{
			return string.Concat(name.Select((char x, int i) => (i <= 0 || !char.IsUpper(x)) ? x.ToString() : $"_{x}")).ToLowerInvariant();
		}
	}
}
