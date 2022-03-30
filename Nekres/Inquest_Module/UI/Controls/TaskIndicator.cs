using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.Inquest_Module.UI.Controls
{
	internal sealed class TaskIndicator : TaskIndicatorBase
	{
		private static BitmapFont _font = GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0);

		private string _text;

		private Color _textColor;

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _text, value, false, "Text");
			}
		}

		public Color TextColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _textColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _textColor, value, false, "TextColor");
			}
		}

		public TaskIndicator(bool attachToCursor = true)
			: base(attachToCursor)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			_textColor = Color.get_White();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			base.Paint(spriteBatch, bounds);
			if (!base.Paused)
			{
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, bounds);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, _font, bounds, _textColor, false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
		}
	}
}
