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

		private string _prefix;

		private Func<string> _textData;

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

		public string Prefix
		{
			get
			{
				return _prefix;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _prefix, value, false, "Prefix");
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

		public DynamicLabel(Func<string> textData)
			: this()
		{
			_textData = textData;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			Size2 val = ((LabelBase)this)._font.MeasureString(".");
			int height = (int)Math.Round(val.Height);
			int width = (int)Math.Round(val.Width);
			string prefix = _prefix;
			AsyncTexture2D icon = _icon;
			if (icon != null && icon.get_HasSwapped() && icon.get_HasTexture())
			{
				prefix = "    " + prefix;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon), new Rectangle(0, (bounds.Height - height) / 2, height, height));
			}
			string textData = _textData?.Invoke();
			if (string.IsNullOrEmpty(textData))
			{
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, new Rectangle(width + height, (bounds.Height - height) / 2, height, height));
			}
			((Label)this).set_Text(prefix + textData);
			((LabelBase)this).Paint(spriteBatch, bounds);
		}
	}
}
