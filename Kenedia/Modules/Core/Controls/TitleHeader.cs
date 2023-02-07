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

		public BitmapFont Font { get; set; } = Control.get_Content().get_DefaultFont16();


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
				((Control)this).set_BasicTooltipText(value?.Invoke());
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
			((Control)this).RecalculateLayout();
			_titleBounds = new Rectangle(5, 0, ((Control)this).get_Width() - 10, ((Control)this).get_Height());
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedTooltip != null)
			{
				((Control)this).set_BasicTooltipText(SetLocalizedTooltip?.Invoke());
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
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_texturePanelHeader), bounds, (Rectangle?)_texturePanelHeader.get_Bounds());
			if (Title != null)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Title, Font, _titleBounds, Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		public TitleHeader()
			: this()
		{
		}//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)

	}
}
