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
		public Color Color
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				if (_003CColor_003Ek__BackingField == Color.get_Transparent())
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
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_003CColor_003Ek__BackingField = Color.get_Transparent();
			Names = new LocalizedString();
			base._002Ector();
		}

		public void ApplyApiData(Profession profession)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected I4, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			Id = (ProfessionType)(Enum.TryParse<ProfessionType>(profession.get_Id(), out ProfessionType professionType) ? ((int)professionType) : 0);
			ProfessionType id = Id;
			WeightClass = (id - 1) switch
			{
				0 => ArmorWeight.Heavy, 
				1 => ArmorWeight.Heavy, 
				8 => ArmorWeight.Heavy, 
				2 => ArmorWeight.Medium, 
				3 => ArmorWeight.Medium, 
				4 => ArmorWeight.Medium, 
				5 => ArmorWeight.Light, 
				6 => ArmorWeight.Light, 
				7 => ArmorWeight.Light, 
				_ => ArmorWeight.Heavy, 
			};
			IconAssetId = profession.get_Icon().GetAssetIdFromRenderUrl();
			IconBigAssetId = profession.get_IconBig().GetAssetIdFromRenderUrl();
			Name = profession.get_Name();
		}
	}
}
