using Blish_HUD.Input;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace Nekres.ChatMacros.Core.UI.Configs
{
	internal class ControlsConfig : ConfigBase
	{
		public static ControlsConfig Default;

		private KeyBinding _openQuickAccess;

		private KeyBinding _chatMessage;

		private KeyBinding _squadBroadcastMessage;

		[JsonProperty("open_quick_access")]
		public KeyBinding OpenQuickAccess
		{
			get
			{
				return _openQuickAccess;
			}
			set
			{
				_openQuickAccess = ResetDelegates(_openQuickAccess, value);
			}
		}

		[JsonProperty("chat_message")]
		public KeyBinding ChatMessage
		{
			get
			{
				return _chatMessage;
			}
			set
			{
				_chatMessage = ResetDelegates(_chatMessage, value);
			}
		}

		[JsonProperty("squad_broadcast_message")]
		public KeyBinding SquadBroadcastMessage
		{
			get
			{
				return _squadBroadcastMessage;
			}
			set
			{
				_squadBroadcastMessage = ResetDelegates(_squadBroadcastMessage, value);
			}
		}

		protected override void BindingChanged()
		{
			SaveConfig<ControlsConfig>(ChatMacros.Instance.ControlsConfig);
		}

		static ControlsConfig()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			ControlsConfig controlsConfig = new ControlsConfig();
			KeyBinding val = new KeyBinding((Keys)13);
			val.set_Enabled(false);
			val.set_IgnoreWhenInTextField(true);
			controlsConfig.ChatMessage = val;
			KeyBinding val2 = new KeyBinding((ModifierKeys)4, (Keys)13);
			val2.set_Enabled(false);
			val2.set_IgnoreWhenInTextField(true);
			controlsConfig.SquadBroadcastMessage = val2;
			controlsConfig.OpenQuickAccess = new KeyBinding();
			Default = controlsConfig;
		}
	}
}
