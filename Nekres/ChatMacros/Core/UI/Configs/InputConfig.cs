using System;
using Blish_HUD.Input;
using Newtonsoft.Json;

namespace Nekres.ChatMacros.Core.UI.Configs
{
	internal class InputConfig : ConfigBase
	{
		public static InputConfig Default = new InputConfig
		{
			_inputDevice = Guid.Empty,
			_voiceLang = VoiceLanguage.English,
			_secondaryVoiceLang = VoiceLanguage.English,
			PushToTalk = new KeyBinding()
		};

		private Guid _inputDevice;

		private VoiceLanguage _voiceLang;

		private VoiceLanguage _secondaryVoiceLang;

		private KeyBinding _pushToTalk;

		[JsonProperty("input_device")]
		public Guid InputDevice
		{
			get
			{
				return _inputDevice;
			}
			set
			{
				if (SetProperty(ref _inputDevice, value))
				{
					SaveConfig<InputConfig>(ChatMacros.Instance.InputConfig);
				}
			}
		}

		[JsonProperty("voice_lang")]
		public VoiceLanguage VoiceLanguage
		{
			get
			{
				return _voiceLang;
			}
			set
			{
				if (SetProperty(ref _voiceLang, value))
				{
					SaveConfig<InputConfig>(ChatMacros.Instance.InputConfig);
				}
			}
		}

		[JsonProperty("secondary_voice_lang")]
		public VoiceLanguage SecondaryVoiceLanguage
		{
			get
			{
				return _secondaryVoiceLang;
			}
			set
			{
				if (SetProperty(ref _secondaryVoiceLang, value))
				{
					SaveConfig<InputConfig>(ChatMacros.Instance.InputConfig);
				}
			}
		}

		[JsonProperty("push_to_talk")]
		public KeyBinding PushToTalk
		{
			get
			{
				return _pushToTalk;
			}
			set
			{
				_pushToTalk = ResetDelegates(_pushToTalk, value);
			}
		}

		protected override void BindingChanged()
		{
			SaveConfig<InputConfig>(ChatMacros.Instance.InputConfig);
		}
	}
}
