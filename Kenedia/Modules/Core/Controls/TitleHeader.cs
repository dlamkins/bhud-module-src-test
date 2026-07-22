using System;
using System.Runtime.CompilerServices;
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

		private Rectangle _titleBounds = Rectangle.Empty;

		public BitmapFont Font { get; set; } = Control.Content.DefaultFont16;


		public string Title { get; set; }

		public Func<string> SetLocalizedTooltip
		{
			[CompilerGenerated]
			get
			{
				return _003CSetLocalizedTooltip_003Ek__BackingField;
			}
			set
			{
				_003CSetLocalizedTooltip_003Ek__BackingField = value;
				base.BasicTooltipText = value?.Invoke();
			}
		}

		public Func<string> SetLocalizedTitle
		{
			[CompilerGenerated]
			get
			{
				return _003CSetLocalizedTitle_003Ek__BackingField;
			}
			set
			{
				_003CSetLocalizedTitle_003Ek__BackingField = value;
				Title = value?.Invoke();
			}
		}

		public override void RecalculateLayout()
		{
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
			spriteBatch.DrawOnCtrl(this, _texturePanelHeader, bounds, _texturePanelHeader.Bounds);
			if (Title != null)
			{
				spriteBatch.DrawStringOnCtrl(this, Title, Font, _titleBounds, Color.White);
			}
		}
	}
}
