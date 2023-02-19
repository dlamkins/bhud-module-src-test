namespace Kenedia.Modules.Core.DataModels
{
	public class Stealth
	{
		public int? Default;

		public int? Deadeye;

		public int?[] Values => new int?[2] { Default, Deadeye };

		public void Clear()
		{
			Default = null;
			Deadeye = null;
		}
	}
}
