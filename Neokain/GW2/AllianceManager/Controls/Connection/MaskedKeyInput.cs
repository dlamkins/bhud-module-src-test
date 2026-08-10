using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Neokain.GW2.AllianceManager.Controls.Connection
{
	public sealed class MaskedKeyInput : Panel
	{
		private const int TOGGLE_WIDTH = 52;

		private const int SPACING = 6;

		private const int CONTROL_HEIGHT = 30;

		private readonly SettingEntry<string> _setting;

		private readonly TextBox _textBox;

		private readonly Label _maskLabel;

		private readonly StandardButton _toggleButton;

		private bool _revealed;

		public MaskedKeyInput(SettingEntry<string> setting, BitmapFont font = null)
			: this()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Expected O, but got Unknown
			_setting = setting ?? throw new ArgumentNullException("setting");
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Control)this).set_Height(30);
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Left(0);
			((Control)val).set_Top(0);
			((Control)val).set_Height(30);
			((TextInputBase)val).set_PlaceholderText("am_...");
			((TextInputBase)val).set_Text(_setting.get_Value() ?? string.Empty);
			_textBox = val;
			if (font != null)
			{
				((TextInputBase)_textBox).set_Font(font);
			}
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Left(0);
			((Control)val2).set_Top(0);
			((Control)val2).set_Height(30);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val2).set_BackgroundColor(new Color(43, 43, 43, 255));
			val2.set_ShowShadow(false);
			val2.set_Text(string.Empty);
			((Control)val2).set_Visible(false);
			_maskLabel = val2;
			if (font != null)
			{
				_maskLabel.set_Font(font);
			}
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Top(0);
			((Control)val3).set_Width(52);
			((Control)val3).set_Height(30);
			val3.set_Text("Show");
			((Control)val3).set_BasicTooltipText("Show / hide the key");
			_toggleButton = val3;
			_textBox.add_EnterPressed((EventHandler<EventArgs>)OnEnterPressed);
			((TextInputBase)_textBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)OnInputFocusChanged);
			((Control)_toggleButton).add_Click((EventHandler<MouseEventArgs>)OnToggleClicked);
			_setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnSettingChangedExternally);
			UpdateLayout();
			UpdateMaskState();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			if (_textBox != null && _maskLabel != null && _toggleButton != null)
			{
				int available = Math.Max(0, ((Control)this).get_Width() - 52 - 6);
				((Control)_textBox).set_Width(available);
				((Control)_maskLabel).set_Width(available);
				((Control)_toggleButton).set_Left(available + 6);
			}
		}

		private void UpdateMaskLabelText()
		{
			int len = ((TextInputBase)_textBox).get_Text()?.Length ?? 0;
			_maskLabel.set_Text((len == 0) ? string.Empty : new string('•', Math.Min(len, 64)));
		}

		private void UpdateMaskState()
		{
			UpdateMaskLabelText();
			bool showMask = !_revealed && !((TextInputBase)_textBox).get_Focused() && (((TextInputBase)_textBox).get_Text()?.Length ?? 0) > 0;
			((Control)_maskLabel).set_Visible(showMask);
			_toggleButton.set_Text(_revealed ? "Hide" : "Show");
		}

		private void OnToggleClicked(object sender, MouseEventArgs e)
		{
			_revealed = !_revealed;
			UpdateMaskState();
		}

		private void OnEnterPressed(object sender, EventArgs e)
		{
			Commit();
		}

		private void OnInputFocusChanged(object sender, EventArgs e)
		{
			Commit();
			UpdateMaskState();
		}

		private void Commit()
		{
			string newValue = ((TextInputBase)_textBox).get_Text()?.Trim() ?? string.Empty;
			if ((_setting.get_Value() ?? string.Empty) != newValue)
			{
				_setting.set_Value(newValue);
			}
		}

		private void OnSettingChangedExternally(object sender, ValueChangedEventArgs<string> e)
		{
			string incoming = e.get_NewValue() ?? string.Empty;
			if ((((TextInputBase)_textBox).get_Text() ?? string.Empty) != incoming)
			{
				((TextInputBase)_textBox).set_Text(incoming);
				UpdateMaskState();
			}
		}

		protected override void DisposeControl()
		{
			_textBox.remove_EnterPressed((EventHandler<EventArgs>)OnEnterPressed);
			((TextInputBase)_textBox).remove_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)OnInputFocusChanged);
			((Control)_toggleButton).remove_Click((EventHandler<MouseEventArgs>)OnToggleClicked);
			_setting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)OnSettingChangedExternally);
			((Panel)this).DisposeControl();
		}
	}
}
