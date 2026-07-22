using System;
using System.Runtime.CompilerServices;
using Blish_HUD.Content;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Controls.Selection
{
	public class AttributeToggle : ImageToggle
	{
		public AttributeType Attribute
		{
			[CompilerGenerated]
			get
			{
				return _003CAttribute_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CAttribute_003Ek__BackingField, value, delegate(AttributeType v)
				{
					_003CAttribute_003Ek__BackingField = v;
				}, new Action(OnAttributeChanged));
			}
		}

		public AttributeToggle()
		{
			base.ImageColor = Microsoft.Xna.Framework.Color.Gray * 0.5f;
			base.ActiveColor = Microsoft.Xna.Framework.Color.White;
			base.TextureRectangle = new Microsoft.Xna.Framework.Rectangle(4, 4, 24, 24);
		}

		private void OnAttributeChanged()
		{
			base.Texture = Attribute switch
			{
				AttributeType.Power => AsyncTexture2D.FromAssetId(66722), 
				AttributeType.Toughness => AsyncTexture2D.FromAssetId(156612), 
				AttributeType.Vitality => AsyncTexture2D.FromAssetId(156613), 
				AttributeType.Precision => AsyncTexture2D.FromAssetId(156609), 
				AttributeType.CritDamage => AsyncTexture2D.FromAssetId(156602), 
				AttributeType.ConditionDamage => AsyncTexture2D.FromAssetId(156600), 
				AttributeType.ConditionDuration => AsyncTexture2D.FromAssetId(156601), 
				AttributeType.BoonDuration => AsyncTexture2D.FromAssetId(156599), 
				AttributeType.Healing => AsyncTexture2D.FromAssetId(156606), 
				_ => AsyncTexture2D.FromAssetId(536054), 
			};
		}
	}
}
