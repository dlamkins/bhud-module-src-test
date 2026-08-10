using System;
using Blish_HUD.Controls;
using MonoGame.Extended.BitmapFonts;

namespace Neokain.GW2.AllianceManager.Controls.Shared
{
	public class AutoHeightTextBox : Panel
	{
		private readonly WrappingMultilineTextBox _textBox;

		private BitmapFont _font;

		private int _minHeight;

		public string Text
		{
			get
			{
				return ((TextInputBase)_textBox).get_Text();
			}
			set
			{
				if (!(((TextInputBase)_textBox).get_Text() == value))
				{
					((TextInputBase)_textBox).set_Text(value ?? string.Empty);
					RecalculateHeightFromContent();
				}
			}
		}

		public BitmapFont Font
		{
			get
			{
				return _font;
			}
			set
			{
				if (_font != value)
				{
					_font = value ?? Control.get_Content().get_DefaultFont14();
					((TextInputBase)_textBox).set_Font(_font);
					_minHeight = CalculateMinHeight();
					RecalculateHeightFromContent();
				}
			}
		}

		public string PlaceholderText
		{
			get
			{
				return ((TextInputBase)_textBox).get_PlaceholderText();
			}
			set
			{
				((TextInputBase)_textBox).set_PlaceholderText(value);
			}
		}

		public event EventHandler<EventArgs> TextChanged;

		public event EventHandler<EventArgs> HeightChanged;

		public AutoHeightTextBox(BitmapFont font = null)
			: this()
		{
			_font = font ?? Control.get_Content().get_DefaultFont14();
			_minHeight = CalculateMinHeight();
			WrappingMultilineTextBox wrappingMultilineTextBox = new WrappingMultilineTextBox();
			((Control)wrappingMultilineTextBox).set_Parent((Container)(object)this);
			((Control)wrappingMultilineTextBox).set_Left(0);
			((Control)wrappingMultilineTextBox).set_Top(0);
			((TextInputBase)wrappingMultilineTextBox).set_Font(_font);
			_textBox = wrappingMultilineTextBox;
			((TextInputBase)_textBox).add_TextChanged((EventHandler<EventArgs>)OnInternalTextChanged);
			((Control)this).set_Height(_minHeight);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			if (_textBox != null)
			{
				((Control)_textBox).set_Width(((Control)this).get_Width());
				RecalculateHeightFromContent();
			}
		}

		public override void RecalculateLayout()
		{
			if (_textBox != null)
			{
				((Control)_textBox).set_Width(((Control)this).get_Width());
				((Control)_textBox).set_Height(((Control)this).get_Height());
				RecalculateHeightFromContent();
			}
		}

		private void OnInternalTextChanged(object sender, EventArgs e)
		{
			RecalculateHeightFromContent();
			this.TextChanged?.Invoke(this, EventArgs.Empty);
		}

		private int CalculateMinHeight()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return (int)Math.Ceiling(_font.MeasureString("Wj").Height) + 14;
		}

		private void RecalculateHeightFromContent()
		{
			if (_textBox != null && _font != null)
			{
				int contentHeight = _textBox.GetContentHeight();
				int newHeight = Math.Max(_minHeight, contentHeight);
				if (((Control)this).get_Height() != newHeight)
				{
					((Control)this).set_Height(newHeight);
					((Control)_textBox).set_Height(((Control)this).get_Height());
					this.HeightChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		protected override void DisposeControl()
		{
			if (_textBox != null)
			{
				((TextInputBase)_textBox).remove_TextChanged((EventHandler<EventArgs>)OnInternalTextChanged);
			}
			((Panel)this).DisposeControl();
		}
	}
}
