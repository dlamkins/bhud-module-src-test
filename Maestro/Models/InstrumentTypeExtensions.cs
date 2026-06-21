namespace Maestro.Models
{
	public static class InstrumentTypeExtensions
	{
		public static string DisplayName(this InstrumentType type)
		{
			return InstrumentCatalog.Get(type).DisplayName;
		}
	}
}
