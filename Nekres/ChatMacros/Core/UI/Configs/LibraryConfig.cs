using System.Collections.Generic;
using System.Linq;
using Nekres.ChatMacros.Core.Services.Data;
using Newtonsoft.Json;

namespace Nekres.ChatMacros.Core.UI.Configs
{
	internal class LibraryConfig : ConfigBase
	{
		public static LibraryConfig Default = new LibraryConfig
		{
			ChannelHistory = new List<ChatChannel>()
		};

		private bool _showActivesOnly;

		private List<ChatChannel> _channelHistory = new List<ChatChannel>();

		private bool _advancedEdit;

		[JsonProperty("show_actives_only")]
		public bool ShowActivesOnly
		{
			get
			{
				return _showActivesOnly;
			}
			set
			{
				if (SetProperty(ref _showActivesOnly, value))
				{
					SaveConfig<LibraryConfig>(ChatMacros.Instance.LibraryConfig);
				}
			}
		}

		[JsonProperty("channel_history")]
		public List<ChatChannel> ChannelHistory
		{
			get
			{
				return _channelHistory;
			}
			set
			{
				if (SetProperty(ref _channelHistory, value))
				{
					SaveConfig<LibraryConfig>(ChatMacros.Instance.LibraryConfig);
				}
			}
		}

		[JsonProperty("advanced_edit")]
		public bool AdvancedEdit
		{
			get
			{
				return _advancedEdit;
			}
			set
			{
				if (SetProperty(ref _advancedEdit, value))
				{
					SaveConfig<LibraryConfig>(ChatMacros.Instance.LibraryConfig);
				}
			}
		}

		public void UpdateChannelHistory(ChatChannel usedChannel)
		{
			if (_channelHistory == null)
			{
				_channelHistory = new List<ChatChannel>();
			}
			ChannelHistory.Remove(usedChannel);
			ChannelHistory = ChannelHistory.Prepend(usedChannel).ToList();
		}

		public int IndexChannelHistory(ChatChannel channel)
		{
			int indexInHistory = ChannelHistory.IndexOf(channel);
			if (indexInHistory == -1)
			{
				return int.MaxValue;
			}
			return indexInHistory;
		}
	}
}
