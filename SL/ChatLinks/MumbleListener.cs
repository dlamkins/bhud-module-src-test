using System;
using System.Net;
using System.Runtime.CompilerServices;
using GuildWars2;
using GuildWars2.Mumble;
using SL.Common;

namespace SL.ChatLinks
{
	internal sealed class MumbleListener : IObserver<GameTick>, IDisposable
	{
		[CompilerGenerated]
		private GameLink _003Clink_003EP;

		[CompilerGenerated]
		private IEventAggregator _003CeventAggregator_003EP;

		private IDisposable? _subscription;

		private IPEndPoint? _currentServer;

		public MumbleListener(GameLink link, IEventAggregator eventAggregator)
		{
			_003Clink_003EP = link;
			_003CeventAggregator_003EP = eventAggregator;
			base._002Ector();
		}

		public void Start()
		{
			_subscription?.Dispose();
			_subscription = _003Clink_003EP.Subscribe(this);
		}

		public void OnNext(GameTick value)
		{
			IPEndPoint? currentServer = _currentServer;
			if (currentServer == null || !currentServer!.Equals(value.Context.ServerAddress))
			{
				_currentServer = value.Context.ServerAddress;
				_003CeventAggregator_003EP.Publish(new MapChanged((int)value.Context.MapId));
			}
		}

		public void OnError(Exception error)
		{
		}

		public void OnCompleted()
		{
		}

		public void Dispose()
		{
			_subscription?.Dispose();
		}
	}
}
