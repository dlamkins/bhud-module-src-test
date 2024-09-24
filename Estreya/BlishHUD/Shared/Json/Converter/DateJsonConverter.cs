using System.Globalization;
using Newtonsoft.Json.Converters;

namespace Estreya.BlishHUD.Shared.Json.Converter
{
	public class DateJsonConverter : IsoDateTimeConverter
	{
		public DateJsonConverter()
		{
			base.DateTimeFormat = "yyyy-MM-dd";
			base.Culture = CultureInfo.InvariantCulture;
		}
	}
}
