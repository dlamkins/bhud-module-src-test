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
	public class APIPermissionNotification : BaseNotification
	{
		private Rectangle _textRectangle;

		private DetailedTexture _settingsCog = new DetailedTexture(222246);

		private DetailedTexture _dismiss = new DetailedTexture(156012, 156011)
		{
			TextureRegion = new Rectangle(4, 4, 24, 24)
		};

		public Action ClickAction { get; set; }

		public APIPermissionNotification()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			base.NotificationType = NotificationType.APITimeout;
		}

		public override void RecalculateLayout()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int height = GameService.Content.DefaultFont14.get_LineHeight() + 4;
			_dismiss.Bounds = new Rectangle(0, 0, height, height);
			DetailedTexture settingsCog = _settingsCog;
			Rectangle bounds = _dismiss.Bounds;
			settingsCog.Bounds = new Rectangle(((Rectangle)(ref bounds)).get_Right() + 2, 0, height, height);
			int width2 = base.Width;
			bounds = _settingsCog.Bounds;
			int width = width2 - ((Rectangle)(ref bounds)).get_Right() - 6;
			string wrappedText = TextUtil.WrapText(GameService.Content.DefaultFont14, strings.APIPermissionNotification, width);
			RectangleF rect = GameService.Content.DefaultFont14.GetStringRectangle(wrappedText);
			bounds = _settingsCog.Bounds;
			_textRectangle = new Rectangle(((Rectangle)(ref bounds)).get_Right() + 6, 0, width, (int)rect.Height);
			base.Height = Math.Max(height, _textRectangle.Height);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			string txt = string.Empty;
			_dismiss.Draw(this, spriteBatch, base.RelativeMousePosition);
			_settingsCog.Draw(this, spriteBatch, base.RelativeMousePosition);
			spriteBatch.DrawStringOnCtrl(this, strings.APIPermissionNotification, GameService.Content.DefaultFont14, _textRectangle, Color.get_White(), wrap: true);
			base.BasicTooltipText = txt;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			if (_dismiss.Hovered)
			{
				Container parent = base.Parent;
				Dispose();
				parent?.Invalidate();
			}
		}
	}
}
