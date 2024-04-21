using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Regions_Of_Tyria.UI.Controls
{
	internal sealed class NotificationIndicator : Container
	{
		private string _header;

		private string _text;

		public NotificationIndicator(string header, string text)
			: this()
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			_header = MapNotification.FilterDisplayName(header);
			_text = MapNotification.FilterDisplayName(text);
			if (string.IsNullOrEmpty(_text))
			{
				_text = _header;
				_header = string.Empty;
			}
			((Control)this).set_Size(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Height()));
			((Control)this).set_ZIndex(30);
			((Control)this).set_ClipsBounds(true);
			((Control)GameService.Graphics.get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)UpdateLocation);
		}

		private void UpdateLocation(object o, ResizedEventArgs e)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Height()));
			((Control)this).set_Location(new Point(0, 0));
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)1;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (RegionsOfTyria.Instance != null)
			{
				MapNotification.PaintText((Control)(object)this, spriteBatch, bounds, RegionsOfTyria.Instance.TitlingFont, RegionsOfTyria.Instance.TitlingFontSmall, _header, _text, RegionsOfTyria.Instance.OverlapHeader.get_Value(), RegionsOfTyria.Instance.UnderlineHeader.get_Value());
			}
		}

		protected override void DisposeControl()
		{
			((Control)GameService.Graphics.get_SpriteScreen()).remove_Resized((EventHandler<ResizedEventArgs>)UpdateLocation);
			((Container)this).DisposeControl();
		}
	}
}