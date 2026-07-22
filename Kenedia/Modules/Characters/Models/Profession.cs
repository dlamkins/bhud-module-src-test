using System;
using Blish_HUD.Content;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Kenedia.Modules.Characters.Models
{
	public class Profession
	{
		public ProfessionType Id { get; set; }

		[JsonIgnore]
		public Microsoft.Xna.Framework.Color Color
		{
			get
			{
				if (_003CColor_003Ek__BackingField == Microsoft.Xna.Framework.Color.Transparent)
				{
					_003CColor_003Ek__BackingField = Id.GetProfessionColor();
				}
				return _003CColor_003Ek__BackingField;
			}
		}

		public ArmorWeight WeightClass { get; set; }

		public int IconAssetId { get; set; }

		public int IconBigAssetId { get; set; }

		[JsonIgnore]
		public AsyncTexture2D Icon
		{
			get
			{
				if (_003CIcon_003Ek__BackingField == null && IconAssetId != 0)
				{
					_003CIcon_003Ek__BackingField = AsyncTexture2D.FromAssetId(IconAssetId);
				}
				return _003CIcon_003Ek__BackingField;
			}
		}

		[JsonIgnore]
		public AsyncTexture2D IconBig
		{
			get
			{
				if (_003CIconBig_003Ek__BackingField == null && IconAssetId != 0)
				{
					_003CIconBig_003Ek__BackingField = AsyncTexture2D.FromAssetId(IconBigAssetId);
				}
				return _003CIconBig_003Ek__BackingField;
			}
		}

		public LocalizedString Names { get; set; }

		[JsonIgnore]
		public string Name
		{
			get
			{
				return Names.Text;
			}
			set
			{
				Names.Text = value;
			}
		}

		public Profession()
		{
			_003CColor_003Ek__BackingField = Microsoft.Xna.Framework.Color.Transparent;
			Names = new LocalizedString();
			base._002Ector();
		}

		public void ApplyApiData(Gw2Sharp.WebApi.V2.Models.Profession profession)
		{
			Id = (Enum.TryParse<ProfessionType>(profession.Id, out var professionType) ? professionType : ((ProfessionType)0));
			WeightClass = Id switch
			{
				ProfessionType.Guardian => ArmorWeight.Heavy, 
				ProfessionType.Warrior => ArmorWeight.Heavy, 
				ProfessionType.Revenant => ArmorWeight.Heavy, 
				ProfessionType.Engineer => ArmorWeight.Medium, 
				ProfessionType.Ranger => ArmorWeight.Medium, 
				ProfessionType.Thief => ArmorWeight.Medium, 
				ProfessionType.Elementalist => ArmorWeight.Light, 
				ProfessionType.Mesmer => ArmorWeight.Light, 
				ProfessionType.Necromancer => ArmorWeight.Light, 
				_ => ArmorWeight.Heavy, 
			};
			IconAssetId = profession.Icon.GetAssetIdFromRenderUrl();
			IconBigAssetId = profession.IconBig.GetAssetIdFromRenderUrl();
			Name = profession.Name;
		}
	}
}
