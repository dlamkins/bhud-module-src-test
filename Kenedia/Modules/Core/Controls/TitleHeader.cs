using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Core.Controls
{
	public class TitleHeader : Control, ILocalizable
	{
		private readonly AsyncTexture2D _texturePanelHeader = AsyncTexture2D.FromAssetId(1032325);

		private Rectangle _titleBounds = Rectangle.get_Empty();

		private Func<string> _setLocalizedTitle;

		private Func<string> _setLocalizedTooltip;

		public BitmapFont Font { get; set; } = Control.Content.DefaultFont16;


		public string Title { get; set; }

		public Func<string> SetLocalizedTooltip
		{
			get
			{
				return _setLocalizedTooltip;
			}
			set
			{
				_setLocalizedTooltip = value;
				base.BasicTooltipText = value?.Invoke();
			}
		}

		public Func<string> SetLocalizedTitle
		{
			get
			{
				return _setLocalizedTitle;
			}
			set
			{
				_setLocalizedTitle = value;
				Title = value?.Invoke();
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_titleBounds = new Rectangle(5, 0, base.Width - 10, base.Height);
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedTooltip != null)
			{
				base.BasicTooltipText = SetLocalizedTooltip?.Invoke();
			}
			if (SetLocalizedTitle != null)
			{
				Title = SetLocalizedTitle?.Invoke();
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl((Control)this, (Texture2D)_texturePanelHeader, bounds, (Rectangle?)_texturePanelHeader.Bounds);
			if (Title != null)
			{
				spriteBatch.DrawStringOnCtrl(this, Title, Font, _titleBounds, Color.get_White());
			}
		}
	}
}
