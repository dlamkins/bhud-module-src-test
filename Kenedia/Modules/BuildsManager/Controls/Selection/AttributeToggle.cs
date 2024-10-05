using System;
using Blish_HUD.Content;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Controls.Selection
{
	public class AttributeToggle : ImageToggle
	{
		private AttributeType _attribute;

		public AttributeType Attribute
		{
			get
			{
				return _attribute;
			}
			set
			{
				Common.SetProperty(ref _attribute, value, new Action(OnAttributeChanged));
			}
		}

		public AttributeToggle()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			base.ImageColor = Color.get_Gray() * 0.5f;
			base.ActiveColor = Color.get_White();
			base.TextureRectangle = new Rectangle(4, 4, 24, 24);
		}

		private void OnAttributeChanged()
		{
			base.Texture = _attribute switch
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
