using System;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SL.ChatLinks.UI.Tabs.Items;

namespace SL.ChatLinks
{
	internal sealed class ChatLinkOptionsAdapter : IOptionsChangeTokenSource<ChatLinkOptions>
	{
		private CancellationTokenSource _cts = new CancellationTokenSource();

		public string Name => Options.DefaultName;

		public ChatLinkOptionsAdapter(SettingCollection settings)
		{
			SettingEntry<bool> raiseStackSize = default(SettingEntry<bool>);
			if (settings.TryGetSetting<bool>("RaiseStackSize", ref raiseStackSize))
			{
				raiseStackSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
				{
					CancellationTokenSource cancellationTokenSource2 = Interlocked.Exchange(ref _cts, new CancellationTokenSource());
					cancellationTokenSource2.Cancel();
					cancellationTokenSource2.Dispose();
				});
			}
			SettingEntry<bool> bananaMode = default(SettingEntry<bool>);
			if (settings.TryGetSetting<bool>("BananaMode", ref bananaMode))
			{
				bananaMode.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
				{
					CancellationTokenSource cancellationTokenSource = Interlocked.Exchange(ref _cts, new CancellationTokenSource());
					cancellationTokenSource.Cancel();
					cancellationTokenSource.Dispose();
				});
			}
		}

		public IChangeToken GetChangeToken()
		{
			return new CancellationChangeToken(_cts.Token);
		}
	}
}
