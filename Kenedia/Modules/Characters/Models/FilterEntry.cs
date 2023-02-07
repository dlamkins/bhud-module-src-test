namespace Kenedia.Modules.Characters.Models
{
	public class FilterEntry
	{
		public object Entry { get; set; }

		public int Threshold { get; set; }

		public bool Enabled { get; set; }

		public FilterEntry(object entry)
		{
			Entry = entry;
		}
	}
}
