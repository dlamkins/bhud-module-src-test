using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.ChatMacros.Core.Services;
using Nekres.ChatMacros.Core.UI.Configs;
using Nekres.ChatMacros.Properties;

namespace Nekres.ChatMacros.Core.UI.Settings
{
	internal class VoiceRecognitionSettings : View
	{
		private InputConfig _config;

		public VoiceRecognitionSettings(InputConfig config)
			: this()
		{
			_config = config;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(5f, 5f));
			val.set_ControlPadding(new Vector2(5f, 5f));
			((Panel)val).set_CanCollapse(true);
			((Panel)val).set_Title(Resources.Voice_Recognition);
			FlowPanel voiceRecognitionPanel = val;
			KeyValueDropdown<Guid> keyValueDropdown = new KeyValueDropdown<Guid>();
			((Control)keyValueDropdown).set_Parent((Container)(object)voiceRecognitionPanel);
			keyValueDropdown.PlaceholderText = Resources.Select_an_input_device___;
			keyValueDropdown.SelectedItem = _config.InputDevice;
			((Control)keyValueDropdown).set_BasicTooltipText(Resources.Select_an_input_device___);
			KeyValueDropdown<Guid> inputDevice = keyValueDropdown;
			foreach (var device in ChatMacros.Instance.Speech.InputDevices)
			{
				inputDevice.AddItem(device.ProductNameGuid, device.ProductName);
			}
			KeyValueDropdown<VoiceLanguage> keyValueDropdown2 = new KeyValueDropdown<VoiceLanguage>();
			((Control)keyValueDropdown2).set_Parent((Container)(object)voiceRecognitionPanel);
			keyValueDropdown2.PlaceholderText = Resources.Select_your_primary_command_language___;
			keyValueDropdown2.SelectedItem = _config.VoiceLanguage;
			((Control)keyValueDropdown2).set_BasicTooltipText(Resources.Select_your_primary_command_language___);
			KeyValueDropdown<VoiceLanguage> voiceLanguage = keyValueDropdown2;
			KeyValueDropdown<VoiceLanguage> keyValueDropdown3 = new KeyValueDropdown<VoiceLanguage>();
			((Control)keyValueDropdown3).set_Parent((Container)(object)voiceRecognitionPanel);
			keyValueDropdown3.PlaceholderText = Resources.Select_a_secondary_command_language___;
			keyValueDropdown3.SelectedItem = _config.SecondaryVoiceLanguage;
			((Control)keyValueDropdown3).set_BasicTooltipText(Resources.Select_a_secondary_command_language___);
			KeyValueDropdown<VoiceLanguage> secondaryVoiceLanguage = keyValueDropdown3;
			foreach (VoiceLanguage lang in Enum.GetValues(typeof(VoiceLanguage)).Cast<VoiceLanguage>())
			{
				voiceLanguage.AddItem(lang, lang.ToDisplayString());
				secondaryVoiceLanguage.AddItem(lang, lang.ToDisplayString());
			}
			KeybindingAssigner val2 = new KeybindingAssigner(_config.PushToTalk);
			((Control)val2).set_Parent((Container)(object)voiceRecognitionPanel);
			val2.set_KeyBindingName(Resources.Push_to_Talk);
			((Control)val2).set_BasicTooltipText(Resources.Hold_to_recognize_voice_commands_ + "\n" + Resources.Release_to_trigger_an_action_);
			inputDevice.ValueChanged += OnInputDeviceChanged;
			voiceLanguage.ValueChanged += OnVoiceLanguageChanged;
			secondaryVoiceLanguage.ValueChanged += OnSecondaryVoiceLanguageChanged;
			((View<IPresenter>)this).Build(buildPanel);
		}

		private void OnVoiceLanguageChanged(object sender, ValueChangedEventArgs<VoiceLanguage> e)
		{
			if (IsInstalled(e.get_NewValue()))
			{
				_config.VoiceLanguage = e.get_NewValue();
			}
		}

		private void OnSecondaryVoiceLanguageChanged(object sender, ValueChangedEventArgs<VoiceLanguage> e)
		{
			if (IsInstalled(e.get_NewValue()))
			{
				_config.SecondaryVoiceLanguage = e.get_NewValue();
			}
		}

		private bool IsInstalled(VoiceLanguage lang)
		{
			CultureInfo culture = lang.Culture();
			if (!WindowsSpeech.TestVoiceLanguage(culture))
			{
				GameService.Content.PlaySoundEffectByName("error");
				ScreenNotification.ShowNotification(string.Format(Resources.Speech_recognition_for__0__is_not_installed_, "'" + culture.DisplayName + "'"), (NotificationType)2, (Texture2D)null, 4);
				Process.Start("ms-settings:speech");
				return false;
			}
			return true;
		}

		private void OnInputDeviceChanged(object o, ValueChangedEventArgs<Guid> e)
		{
			_config.InputDevice = e.get_NewValue();
		}
	}
}
