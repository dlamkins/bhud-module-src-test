using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class NumberBox : Panel, ILocalizable
	{
		private readonly AsyncTexture2D _addButton = AsyncTexture2D.FromAssetId(155927);

		private readonly AsyncTexture2D _addButtonHovered = AsyncTexture2D.FromAssetId(155928);

		private readonly AsyncTexture2D _minusButton = AsyncTexture2D.FromAssetId(155925);

		private readonly AsyncTexture2D _minusButtonHovered = AsyncTexture2D.FromAssetId(155926);

		private readonly TextBox _inputField = new TextBox();

		private Rectangle _addRectangle;

		private Rectangle _minusRectangle;

		private string _lastText = $"{0}";

		private int _value;

		public Action<int> ValueChangedAction { get; set; }

		public new Func<string> SetLocalizedTooltip
		{
			[CompilerGenerated]
			get
			{
				return _003CSetLocalizedTooltip_003Ek__BackingField;
			}
			set
			{
				_003CSetLocalizedTooltip_003Ek__BackingField = value;
				BasicTooltipText = value?.Invoke();
			}
		}

		public new string BasicTooltipText
		{
			get
			{
				return _inputField.BasicTooltipText;
			}
			set
			{
				_inputField.BasicTooltipText = value;
			}
		}

		public int Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				_inputField.Text = $"{value}";
			}
		}

		public bool ShowButtons
		{
			[CompilerGenerated]
			get
			{
				return _003CShowButtons_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CShowButtons_003Ek__BackingField, value, delegate(bool v)
				{
					_003CShowButtons_003Ek__BackingField = v;
				}, new Action(RecalculateLayout));
			}
		}

		public int Step { get; set; }

		public int MaxValue { get; set; }

		public int MinValue { get; set; }

		public event EventHandler<int> ValueChanged;

		public NumberBox()
		{
			_003CShowButtons_003Ek__BackingField = true;
			Step = 1;
			MaxValue = int.MaxValue;
			MinValue = int.MinValue;
			base._002Ector();
			_inputField.Parent = this;
			_inputField.TextChanged += InputField_TextChanged;
			_inputField.InputFocusChanged += InputField_TextChanged;
			_inputField.HorizontalAlignment = HorizontalAlignment.Center;
			base.Width = 100;
			base.Height = 20;
			LocalizingService.LocaleChanged += new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			UserLocale_SettingChanged(null, null);
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int size = Math.Min(base.Height, base.Width);
			_addRectangle = (ShowButtons ? new Rectangle(base.Width - size, (base.Height - size) / 2, size, size) : Rectangle.Empty);
			_minusRectangle = (ShowButtons ? new Rectangle(base.Width - size * 2, (base.Height - size) / 2, size, size) : Rectangle.Empty);
			int spacing = (ShowButtons ? 2 : 0);
			_inputField.Width = Math.Max(0, base.Width - (_addRectangle.Width + _minusRectangle.Width) - spacing);
			_inputField.Height = base.Height;
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintAfterChildren(spriteBatch, bounds);
			if (ShowButtons)
			{
				if (_addRectangle.Width > 0 && _addRectangle.Height > 0)
				{
					spriteBatch.DrawOnCtrl(this, _addRectangle.Contains(base.RelativeMousePosition) ? _addButtonHovered : _addButton, _addRectangle, new Rectangle(6, 6, 20, 20), Color.White, 0f, default(Vector2));
				}
				if (_minusRectangle.Width > 0 && _minusRectangle.Height > 0)
				{
					spriteBatch.DrawOnCtrl(this, _minusRectangle.Contains(base.RelativeMousePosition) ? _minusButtonHovered : _minusButton, _minusRectangle, new Rectangle(6, 6, 20, 20), Color.White, 0f, default(Vector2));
				}
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			if (_addRectangle.Contains(base.RelativeMousePosition))
			{
				Value += Step;
			}
			if (_minusRectangle.Contains(base.RelativeMousePosition))
			{
				Value -= Step;
			}
		}

		private void InputField_TextChanged(object sender, EventArgs e)
		{
			int value;
			bool isNumeric = int.TryParse(_inputField.Text, out value);
			if (!isNumeric)
			{
				_inputField.Text = _lastText;
				_inputField.CursorIndex = _lastText.Length;
				return;
			}
			_lastText = _inputField.Text;
			if (_inputField.Focused)
			{
				return;
			}
			if (isNumeric)
			{
				if (value > MaxValue || value < MinValue)
				{
					Value = Math.Max(Math.Min(value, MaxValue), MinValue);
					return;
				}
				_value = value;
				ValueChangedAction?.Invoke(Value);
				this.ValueChanged?.Invoke(this, Value);
			}
			else
			{
				_inputField.Text = $"{Value}";
			}
		}

		public new void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedTooltip != null)
			{
				BasicTooltipText = SetLocalizedTooltip?.Invoke();
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			GameService.Overlay.UserLocale.SettingChanged -= UserLocale_SettingChanged;
		}
	}
}
