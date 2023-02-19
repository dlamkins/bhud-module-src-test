namespace Kenedia.Modules.Core.DataModels
{
	public class DualSkill
	{
		public int? Axe;

		public int? Dagger;

		public int? Mace;

		public int? Pistol;

		public int? Scepter;

		public int? Sword;

		public int? Focus;

		public int? Shield;

		public int? Torch;

		public int? Warhorn;

		public int?[] Values => new int?[10] { Axe, Dagger, Mace, Pistol, Scepter, Sword, Focus, Shield, Torch, Warhorn };

		public void Clear()
		{
			Axe = null;
			Dagger = null;
			Mace = null;
			Pistol = null;
			Scepter = null;
			Sword = null;
			Focus = null;
			Shield = null;
			Torch = null;
			Warhorn = null;
		}
	}
}
