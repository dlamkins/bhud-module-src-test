using Newtonsoft.Json.Converters;

namespace Estreya.BlishHUD.EventTable.Json
{
	public class DateJsonConverter : IsoDateTimeConverter
	{
		public DateJsonConverter()
			: this()
		{
			((IsoDateTimeConverter)this).set_DateTimeFormat("yyyy-MM-dd");
		}
	}
}
