using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class AquaticWeapon_TemplateItem : TemplateItem
	{
		public API.weaponType WeaponType = API.weaponType.Unkown;

		public List<API.SigilItem> Sigils = new List<API.SigilItem>
		{
			new API.SigilItem(),
			new API.SigilItem()
		};

		public List<Rectangle> SigilsBounds = new List<Rectangle>
		{
			Rectangle.get_Empty(),
			Rectangle.get_Empty()
		};
	}
}
