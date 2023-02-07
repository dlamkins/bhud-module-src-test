using Newtonsoft.Json.Converters;

namespace Estreya.BlishHUD.Shared.Json.Converter
{
	public class DateTimeJsonConverter : IsoDateTimeConverter
	{
		public DateTimeJsonConverter()
		{
			base.DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
		}
	}
}
