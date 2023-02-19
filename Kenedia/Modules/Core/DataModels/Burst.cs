namespace Kenedia.Modules.Core.DataModels
{
	public class Burst
	{
		public int? Spellbreaker;

		public int? Berserker;

		public int? Stage0;

		public int? Stage1;

		public int? Stage2;

		public int? Stage3;

		public int?[] Values => new int?[6] { Spellbreaker, Berserker, Stage0, Stage1, Stage2, Stage3 };

		public void Clear()
		{
			Spellbreaker = null;
			Berserker = null;
			Stage0 = null;
			Stage1 = null;
			Stage2 = null;
			Stage3 = null;
		}
	}
}
