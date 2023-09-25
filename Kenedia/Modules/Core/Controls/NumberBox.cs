using System;
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

		private Func<string> _setLocalizedTooltip;

		private bool _showButtons = true;

		public Action<int> ValueChangedAction { get; set; }

		public new Func<string> SetLocalizedTooltip
		{
			get
			{
				return _setLocalizedTooltip;
			}
			set
			{
				_setLocalizedTooltip = value;
				BasicTooltipText = value?.Invoke();
			}
		}

		public string BasicTooltipText
		{
			get
			{
				return ((Control)_inputField).get_BasicTooltipText();
			}
			set
			{
				((Control)_inputField).set_BasicTooltipText(value);
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
				((TextInputBase)_inputField).set_Text($"{value}");
			}
		}

		public bool ShowButtons
		{
			get
			{
				return _showButtons;
			}
			set
			{
				Common.SetProperty(ref _showButtons, value, ((Control)this).RecalculateLayout);
			}
		}

		public int Step { get; set; } = 1;


		public int MaxValue { get; set; } = int.MaxValue;


		public int MinValue { get; set; } = int.MinValue;


		public event EventHandler<int> ValueChanged;

		public NumberBox()
		{
			((Control)_inputField).set_Parent((Container)(object)this);
			((TextInputBase)_inputField).add_TextChanged((EventHandler<EventArgs>)InputField_TextChanged);
			((TextInputBase)_inputField).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)InputField_TextChanged);
			((TextBox)_inputField).set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)this).set_Width(100);
			((Control)this).set_Height(20);
			LocalizingService.LocaleChanged += UserLocale_SettingChanged;
			UserLocale_SettingChanged(null, null);
		}

		public override void RecalculateLayout()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int size = Math.Min(((Control)this).get_Height(), ((Control)this).get_Width());
			_addRectangle = (Rectangle)(ShowButtons ? new Rectangle(((Control)this).get_Width() - size, (((Control)this).get_Height() - size) / 2, size, size) : Rectangle.get_Empty());
			_minusRectangle = (Rectangle)(ShowButtons ? new Rectangle(((Control)this).get_Width() - size * 2, (((Control)this).get_Height() - size) / 2, size, size) : Rectangle.get_Empty());
			int spacing = (ShowButtons ? 2 : 0);
			((Control)_inputField).set_Width(Math.Max(0, ((Control)this).get_Width() - (_addRectangle.Width + _minusRectangle.Width) - spacing));
			((Control)_inputField).set_Height(((Control)this).get_Height());
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintAfterChildren(spriteBatch, bounds);
			if (ShowButtons)
			{
				if (_addRectangle.Width > 0 && _addRectangle.Height > 0)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(((Rectangle)(ref _addRectangle)).Contains(((Control)this).get_RelativeMousePosition()) ? _addButtonHovered : _addButton), _addRectangle, (Rectangle?)new Rectangle(6, 6, 20, 20), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
				if (_minusRectangle.Width > 0 && _minusRectangle.Height > 0)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(((Rectangle)(ref _minusRectangle)).Contains(((Control)this).get_RelativeMousePosition()) ? _minusButtonHovered : _minusButton), _minusRectangle, (Rectangle?)new Rectangle(6, 6, 20, 20), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			base.OnClick(e);
			if (((Rectangle)(ref _addRectangle)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				Value += Step;
			}
			if (((Rectangle)(ref _minusRectangle)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				Value -= Step;
			}
		}

		private void InputField_TextChanged(object sender, EventArgs e)
		{
			int value;
			bool isNumeric = int.TryParse(((TextInputBase)_inputField).get_Text(), out value);
			if (!isNumeric)
			{
				((TextInputBase)_inputField).set_Text(_lastText);
				((TextInputBase)_inputField).set_CursorIndex(_lastText.Length);
				return;
			}
			_lastText = ((TextInputBase)_inputField).get_Text();
			if (((TextInputBase)_inputField).get_Focused())
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
				((TextInputBase)_inputField).set_Text($"{Value}");
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
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
		}
	}
}
