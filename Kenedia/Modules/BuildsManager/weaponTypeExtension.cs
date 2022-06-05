using Kenedia.Modules.BuildsManager.Strings;

namespace Kenedia.Modules.BuildsManager
{
	internal static class weaponTypeExtension
	{
		public static string getLocalName(this API.weaponType weaponType)
		{
			string text = weaponType.ToString();
			return weaponType switch
			{
				API.weaponType.Axe => common.Axe, 
				API.weaponType.Dagger => common.Dagger, 
				API.weaponType.Mace => common.Mace, 
				API.weaponType.Pistol => common.Pistol, 
				API.weaponType.Scepter => common.Scepter, 
				API.weaponType.Sword => common.Sword, 
				API.weaponType.Focus => common.Focus, 
				API.weaponType.Shield => common.Shield, 
				API.weaponType.Torch => common.Torch, 
				API.weaponType.Warhorn => common.Warhorn, 
				API.weaponType.Greatsword => common.Greatsword, 
				API.weaponType.Hammer => common.Hammer, 
				API.weaponType.Longbow => common.Longbow, 
				API.weaponType.Rifle => common.Rifle, 
				API.weaponType.Shortbow => common.Shortbow, 
				API.weaponType.Staff => common.Staff, 
				API.weaponType.Harpoon => common.Spear, 
				API.weaponType.Speargun => common.Speargun, 
				API.weaponType.Trident => common.Trident, 
				_ => text, 
			};
		}
	}
}
