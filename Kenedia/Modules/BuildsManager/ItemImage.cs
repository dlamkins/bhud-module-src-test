using System;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager
{
	public class ItemImage : Panel
	{
		private Texture2D _Texture;

		private Image Image;

		public int FrameWidth = 3;

		public ItemRarity __Rarity;

		public string BasicTooltipText
		{
			set
			{
				((Control)Image).set_BasicTooltipText(value);
			}
		}

		public Texture2D Texture
		{
			get
			{
				return _Texture;
			}
			set
			{
				_Texture = value;
				Image.set_Texture(AsyncTexture2D.op_Implicit(Texture));
			}
		}

		public ItemRarity _Rarity
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return __Rarity;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				__Rarity = value;
				((Control)this).set_BackgroundColor(FrameColor);
			}
		}

		public Color FrameColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Expected I4, but got Unknown
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
				ItemRarity rarity = _Rarity;
				return (Color)((int)rarity switch
				{
					0 => new Color(0, 0, 0, 225), 
					1 => new Color(170, 170, 170, 225), 
					2 => new Color(240, 240, 240, 225), 
					3 => new Color(98, 164, 218, 225), 
					4 => new Color(26, 147, 6, 225), 
					5 => new Color(252, 208, 11, 225), 
					6 => new Color(255, 164, 5, 225), 
					7 => new Color(251, 62, 141, 225), 
					8 => new Color(76, 19, 157, 225), 
					_ => new Color(0, 0, 0, 0), 
				});
			}
		}

		public ItemImage()
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(FrameWidth, FrameWidth));
			((Control)val).set_Size(new Point(((Control)this).get_Width() - FrameWidth * 2, ((Control)this).get_Height() - FrameWidth * 2));
			Image = val;
			((Control)this).set_BackgroundColor(FrameColor);
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				((Control)Image).set_Location(new Point(FrameWidth, FrameWidth));
				((Control)Image).set_Size(new Point(((Control)this).get_Width() - FrameWidth * 2, ((Control)this).get_Height() - FrameWidth * 2));
			});
		}
	}
}
