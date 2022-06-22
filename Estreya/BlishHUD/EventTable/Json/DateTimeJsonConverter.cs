using Newtonsoft.Json.Converters;

namespace Estreya.BlishHUD.EventTable.Json
{
	public class DateTimeJsonConverter : IsoDateTimeConverter
	{
		public DateTimeJsonConverter()
			: this()
		{
			((IsoDateTimeConverter)this).set_DateTimeFormat("yyyy-MM-ddTHH:mm:ss");
		}
	}
}
