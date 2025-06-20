using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class GuildEmblemControl : Control, IDisposable
	{
		protected Guild _guild;

		protected AsyncTexture2D _emblemForeground;

		protected Rectangle emblemRect = new Rectangle(0, 0, 128, 128);

		public GuildEmblemControl(Guild guild)
			: this()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			_guild = guild;
			((Control)this).set_Width(400);
			((Control)this).set_Height(130);
			string url = Service.GeoServerWrapper.GetGuildEmblemUrl(_guild.Id.ToString());
			_emblemForeground = Service.Textures.GetURLTexture(url, _guild.Id + ".png");
		}

		protected override void DisposeControl()
		{
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			if (_emblemForeground != null && _emblemForeground.get_HasSwapped())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_emblemForeground), emblemRect, (Rectangle?)emblemRect, Color.get_White());
			}
			Color color = (base._mouseOver ? Color.get_Green() : Color.get_LightGoldenrodYellow());
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _guild.Name, Control.get_Content().get_DefaultFont18(), new Rectangle(135, 25, 270, 40), color, false, (HorizontalAlignment)0, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "[" + _guild.Tag + "]", Control.get_Content().get_DefaultFont14(), new Rectangle(135, 50, 270, 40), Color.get_WhiteSmoke(), false, (HorizontalAlignment)0, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, 400, 3), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 127, 400, 130), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, 3, 130), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(397, 0, 400, 130), color);
		}
	}
}
