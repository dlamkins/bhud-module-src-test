using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.GameServices.ArcDps.V2;
using Blish_HUD.GameServices.ArcDps.V2.Models.UnofficialExtras;
using Blish_HUD.Input;
using FontStashSharp;
using LoreBridge.Controls;
using LoreBridge.Models;
using LoreBridge.Modules.Chat.Controls;
using LoreBridge.Modules.Chat.Models;
using LoreBridge.Modules.Chat.Services;
using LoreBridge.Resources;
using Microsoft.Xna.Framework;

namespace LoreBridge.Modules.Chat
{
	public class Chat : Module
	{
		[CompilerGenerated]
		private CornerIcon _003CcornerIcon_003EP;

		private SpriteFontBase _font;

		private Messages _messages;

		private IArcDpsMessageListener<NpcMessageInfo> _npcMessageListener;

		private Settings _settings;

		private TranslationWindow _translationWindow;

		private TranslationQueue _translationQueue;

		public Chat(CornerIcon cornerIcon)
		{
			_003CcornerIcon_003EP = cornerIcon;
			base._002Ector();
		}

		public override void Load(Settings settings)
		{
			_settings = settings;
			_messages = new Messages(_settings);
			_font = Fonts.FontSystem.GetFont(_settings.WindowFontSize.get_Value());
			_translationWindow = new TranslationWindow(_settings, _messages, _font);
			_translationQueue = new TranslationQueue(_messages);
			((Control)_003CcornerIcon_003EP).add_Click((EventHandler<MouseEventArgs>)OnCornerIconClick);
			_settings.WindowFontSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnFontSizeChanged);
			try
			{
				if (((GameService)GameService.ArcDpsV2).get_Loaded())
				{
					_npcMessageListener = (IArcDpsMessageListener<NpcMessageInfo>)(object)new ArcDpsMessageListener<NpcMessageInfo>((MessageType)6, (Func<NpcMessageInfo, CancellationToken, Task>)OnNpcChatMessage);
					GameService.ArcDpsV2.RegisterMessageType<NpcMessageInfo>(_npcMessageListener);
				}
			}
			catch (Exception)
			{
			}
		}

		public override void Update(GameTime gameTime)
		{
		}

		public override void Unload()
		{
			((Control)_translationWindow).Dispose();
			((IDisposable)_npcMessageListener).Dispose();
			((Control)_003CcornerIcon_003EP).remove_Click((EventHandler<MouseEventArgs>)OnCornerIconClick);
			_settings.WindowFontSize.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnFontSizeChanged);
		}

		private void OnCornerIconClick(object sender, EventArgs e)
		{
			_translationWindow.ToggleWindow();
		}

		private void OnFontSizeChanged(object sender, ValueChangedEventArgs<int> e)
		{
			_font = Fonts.FontSystem.GetFont(e.get_NewValue());
			_translationWindow.UpdateFont(_font);
		}

		private Task OnNpcChatMessage(NpcMessageInfo chatMessage, CancellationToken cancellationToken)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)((NpcMessageInfo)(ref chatMessage)).get_TimeStamp() / 1000000000.0);
			DateTime localDateTime = TimeZoneInfo.ConvertTime(DateTime.Today.Add(timeSpan + DateTimeOffset.Now.Offset), TimeZoneInfo.Local);
			_translationQueue.Add(new Message
			{
				Text = ((NpcMessageInfo)(ref chatMessage)).get_Message(),
				TimeStamp = ((NpcMessageInfo)(ref chatMessage)).get_TimeStamp(),
				Name = ((NpcMessageInfo)(ref chatMessage)).get_CharacterName(),
				Time = localDateTime.ToShortTimeString()
			});
			return Task.CompletedTask;
		}
	}
}
