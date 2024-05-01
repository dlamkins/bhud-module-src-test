using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HsAPI
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum WeaponType
	{
		Standard,
		BundleLarge,
		BundleSmall,
		Sword,
		Hammer,
		BowLong,
		BowShort,
		Axe,
		Dagger,
		Greatsword,
		Mace,
		Pistol,
		Polearm,
		Rifle,
		Scepter,
		Staff,
		Focus,
		Torch,
		Warhorn,
		Shield,
		Spear,
		Speargun,
		Trident,
		Toy,
		ToyTwoHanded,
		NoWeapon
	}
}
