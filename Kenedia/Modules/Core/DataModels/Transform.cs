namespace Kenedia.Modules.Core.DataModels
{
	public class Transform
	{
		public int? Enter;

		public int? Exit;

		public int? Weapon1;

		public int? Weapon2;

		public int? Weapon3;

		public int? Weapon4;

		public int? Weapon5;

		public int?[] Values => new int?[7] { Enter, Exit, Weapon1, Weapon2, Weapon3, Weapon4, Weapon5 };

		public void Clear()
		{
			Enter = null;
			Exit = null;
			Weapon1 = null;
			Weapon2 = null;
			Weapon3 = null;
			Weapon4 = null;
			Weapon5 = null;
		}
	}
}
