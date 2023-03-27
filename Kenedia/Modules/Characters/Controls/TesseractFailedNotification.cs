using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Kenedia.Modules.Characters.Controls
{
	public class TesseractFailedNotification : BaseNotification
	{
		private Rectangle _textRectangle;

		private DetailedTexture _settingsCog = new DetailedTexture(155052, 157110);

		private DetailedTexture _dismiss = new DetailedTexture(156012, 156011)
		{
			TextureRegion = new Rectangle(4, 4, 24, 24)
		};

		public Action ClickAction { get; set; }

		public string PathToEngine { get; internal set; }

		public TesseractFailedNotification()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			base.NotificationType = NotificationType.Tesseract;
		}

		public override void RecalculateLayout()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RecalculateLayout();
			int height = GameService.Content.get_DefaultFont14().get_LineHeight() + 4;
			_dismiss.Bounds = new Rectangle(0, 0, height, height);
			DetailedTexture settingsCog = _settingsCog;
			Rectangle bounds = _dismiss.Bounds;
			settingsCog.Bounds = new Rectangle(((Rectangle)(ref bounds)).get_Right() + 2, 0, height, height);
			int width2 = ((Control)this).get_Width();
			bounds = _settingsCog.Bounds;
			int width = width2 - ((Rectangle)(ref bounds)).get_Right() - 6;
			string wrappedText = TextUtil.WrapText(GameService.Content.get_DefaultFont14(), string.Format(strings.TesseractFailedNotification, PathToEngine), width);
			RectangleF rect = GameService.Content.get_DefaultFont14().GetStringRectangle(wrappedText);
			bounds = _settingsCog.Bounds;
			_textRectangle = new Rectangle(((Rectangle)(ref bounds)).get_Right() + 6, 0, width, (int)rect.Height);
			((Control)this).set_Height(Math.Max(height, _textRectangle.Height));
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			string txt = string.Empty;
			_dismiss.Draw((Control)(object)this, spriteBatch, ((Control)this).get_RelativeMousePosition());
			_settingsCog.Draw((Control)(object)this, spriteBatch, ((Control)this).get_RelativeMousePosition());
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, string.Format(strings.TesseractFailedNotification, PathToEngine), GameService.Content.get_DefaultFont14(), _textRectangle, Color.get_White(), true, (HorizontalAlignment)0, (VerticalAlignment)0);
			((Control)this).set_BasicTooltipText(txt);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
			if (_settingsCog.Hovered)
			{
				ClickAction?.Invoke();
			}
			if (_dismiss.Hovered)
			{
				Container parent = ((Control)this).get_Parent();
				((Control)this).Dispose();
				if (parent != null)
				{
					((Control)parent).Invalidate();
				}
			}
		}
	}
}
