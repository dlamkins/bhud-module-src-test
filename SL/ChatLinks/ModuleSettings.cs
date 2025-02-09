using System;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SL.ChatLinks.UI.Tabs.Items;

namespace SL.ChatLinks
{
	public class ModuleSettings : IConfigureOptions<ChatLinkOptions>, IOptionsChangeTokenSource<ChatLinkOptions>
	{
		private class ChangeTokenSource
		{
			private CancellationTokenSource _cts = new CancellationTokenSource();

			public IChangeToken Token => new CancellationChangeToken(_cts.Token);

			public void OnChange()
			{
				CancellationTokenSource cancellationTokenSource = Interlocked.Exchange(ref _cts, new CancellationTokenSource());
				cancellationTokenSource.Cancel();
				cancellationTokenSource.Dispose();
			}
		}

		private readonly SettingEntry<bool> _bananaMode;

		private readonly SettingEntry<bool> _raiseStackSize;

		private readonly SettingEntry<int> _maxResultCount;

		private readonly ChangeTokenSource _changeTokenSource = new ChangeTokenSource();

		public bool BananaMode
		{
			get
			{
				return _bananaMode.get_Value();
			}
			set
			{
				_bananaMode.set_Value(value);
			}
		}

		public bool RaiseStackSize
		{
			get
			{
				return _raiseStackSize.get_Value();
			}
			set
			{
				_raiseStackSize.set_Value(value);
			}
		}

		public int MaxResultCount
		{
			get
			{
				return _maxResultCount.get_Value();
			}
			set
			{
				_maxResultCount.set_Value(value);
			}
		}

		public string Name => Options.DefaultName;

		public ModuleSettings(SettingCollection settings)
		{
			_bananaMode = settings.DefineSetting<bool>("BananaMode", false, (Func<string>)(() => "Banana of Imagination-mode"), (Func<string>)(() => "When enabled, you can add an upgrade component to any item."));
			_bananaMode.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				_changeTokenSource.OnChange();
			});
			_raiseStackSize = settings.DefineSetting<bool>("RaiseStackSize", false, (Func<string>)(() => "Raise the maximum item stack size from 250 to 255"), (Func<string>)(() => "When enabled, you can generate chat links with stacks of 255 items."));
			_raiseStackSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				_changeTokenSource.OnChange();
			});
			_maxResultCount = settings.DefineSetting<int>("MaxResultCount", 50, (Func<string>)(() => "Maximum Result Count"), (Func<string>)(() => "The maximum number of search results to display. WARNING! High numbers can slow down or even freeze Blish HUD."));
			SettingComplianceExtensions.SetRange(_maxResultCount, 50, 1000);
			_maxResultCount.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate
			{
				_changeTokenSource.OnChange();
			});
		}

		public void Configure(ChatLinkOptions options)
		{
			options.RaiseStackSize = RaiseStackSize;
			options.BananaMode = BananaMode;
			options.MaxResultCount = MaxResultCount;
		}

		public IChangeToken GetChangeToken()
		{
			return _changeTokenSource.Token;
		}
	}
}
