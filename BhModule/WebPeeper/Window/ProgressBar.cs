using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace BhModule.WebPeeper.Window
{
	internal class ProgressBar : Control
	{
		private string _text = "";

		private string _wrapText = "";

		private float _percentage = getProgress();

		private readonly Func<float> _getProgress;

		private readonly BitmapFont _textSize;

		private Rectangle _textRect;

		private Rectangle _barRect;

		private Rectangle _barBorderRect;

		private Point _barSize;

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
				((Control)this).RecalculateLayout();
			}
		}

		public Point BarSize
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _barSize;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				_barSize = value;
				((Control)this).RecalculateLayout();
			}
		}

		public event EventHandler<ValueChangedEventArgs<float>> ProgressUpdated;

		public ProgressBar(Func<float> getProgress)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			_getProgress = getProgress;
			_textSize = Control.get_Content().get_DefaultFont16();
			_textRect = Rectangle.get_Empty();
			_barRect = Rectangle.get_Empty();
			_barBorderRect = Rectangle.get_Empty();
			_barSize = Point.get_Zero();
			((Control)this)._002Ector();
		}

		protected override void DisposeControl()
		{
			this.ProgressUpdated = null;
			((Control)this).DisposeControl();
		}

		public override void DoUpdate(GameTime gameTime)
		{
			_barRect.Width = GetBarWidth();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrWhiteSpace(_text))
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _wrapText, _textSize, _textRect, Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _barBorderRect, Color.get_Black());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _barRect, Color.get_White());
		}

		public override void RecalculateLayout()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			((Rectangle)(ref _barBorderRect)).set_Size((_barSize == Point.get_Zero()) ? ((Control)this).get_Size() : _barSize);
			_barBorderRect.X = (((Control)this).get_Width() - _barBorderRect.Width) / 2;
			_barBorderRect.Y = (((Control)this).get_Height() - _barBorderRect.Height) / 2;
			if (!string.IsNullOrWhiteSpace(_text))
			{
				_wrapText = DrawUtil.WrapText(_textSize, _text, (float)((Control)this).get_Width());
				Size2 val = _textSize.MeasureString(_wrapText);
				_textRect.X = (int)(((float)((Control)this).get_Width() - val.Width) / 2f);
				_textRect.Y = (int)((float)(((Control)this).get_Height() / 2) - val.Height);
				_textRect.Width = (int)val.Width;
				_textRect.Height = (int)val.Height;
				_barBorderRect.Y = ((Control)this).get_Height() / 2 + 10;
			}
			_barRect.X = _barBorderRect.X + 1;
			_barRect.Y = _barBorderRect.Y + 1;
			_barRect.Height = _barBorderRect.Height - 2;
			_barRect.Width = GetBarWidth();
		}

		private int GetBarWidth()
		{
			float num = _getProgress();
			if (num != _percentage)
			{
				this.ProgressUpdated?.Invoke(this, new ValueChangedEventArgs<float>(_percentage, num));
			}
			_percentage = num;
			return (int)(_percentage * (float)(_barBorderRect.Width - 2));
		}
	}
}
