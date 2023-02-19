namespace Kenedia.Modules.Core.DataModels
{
	public class Chain
	{
		public int? First;

		public int? Second;

		public int? Third;

		public int? Fourth;

		public int? Fifth;

		public int? Stealth;

		public int? StealthDeadeye;

		public int? Ambush;

		public int? Unleashed;

		public void Clear()
		{
			First = null;
			Second = null;
			Third = null;
			Fourth = null;
			Fifth = null;
			Stealth = null;
			StealthDeadeye = null;
			Ambush = null;
			Unleashed = null;
		}
	}
}
