using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Nekres.Mumble_Info.Core.UI.Controls
{
	internal class DynamicLabel : Label
	{
		private AsyncTexture2D _icon;

		private Color _textDataColor;

		private Func<string> _textData;

		private bool _strokeTextData;

		public AsyncTexture2D Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				((Control)this).SetProperty<AsyncTexture2D>(ref _icon, value, false, "Icon");
			}
		}

		public Color TextDataColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _textDataColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _textDataColor, value, false, "TextDataColor");
			}
		}

		public Func<string> TextData
		{
			get
			{
				return _textData;
			}
			set
			{
				((Control)this).SetProperty<Func<string>>(ref _textData, value, false, "TextData");
			}
		}

		public bool StrokeTextData
		{
			get
			{
				return _strokeTextData;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _strokeTextData, value, false, "StrokeTextData");
			}
		}

		public DynamicLabel(Func<string> textData)
			: this()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			_textDataColor = ((LabelBase)this)._textColor;
			_textData = textData;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			Size2 size = ((LabelBase)this)._font.MeasureString(string.IsNullOrEmpty(((LabelBase)this)._text) ? "." : ((LabelBase)this)._text);
			int width = ((!string.IsNullOrEmpty(((LabelBase)this)._text)) ? ((int)Math.Round(size.Width)) : 0);
			int iconSize = (int)Math.Round(size.Height);
			AsyncTexture2D icon = _icon;
			if (icon != null && icon.get_HasSwapped() && icon.get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon), new Rectangle(0, (bounds.Height - iconSize) / 2, iconSize, iconSize));
				((Rectangle)(ref bounds))._002Ector(bounds.X + iconSize, bounds.Y, bounds.Width - iconSize, bounds.Height);
			}
			((LabelBase)this).Paint(spriteBatch, bounds);
			string textData = _textData?.Invoke();
			if (string.IsNullOrEmpty(textData))
			{
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, new Rectangle(width + iconSize, (bounds.Height - iconSize) / 2, iconSize, iconSize));
				return;
			}
			((Rectangle)(ref bounds))._002Ector(bounds.X + width + 1, bounds.Y, bounds.Width - width, bounds.Height);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, textData, ((LabelBase)this)._font, bounds, _textDataColor, ((LabelBase)this)._wrapText, _strokeTextData, 1, ((LabelBase)this)._horizontalAlignment, ((LabelBase)this)._verticalAlignment);
		}
	}
}
