using System;

namespace CinemaModule.Models.WatchParty
{
	public sealed class WatchPartyStateArgs : EventArgs
	{
		public WatchPartyLocalState State { get; }

		public WatchPartyLocalState PreviousState { get; }

		public WatchPartyStateChangeType ChangeType { get; }

		public bool VideoChanged { get; }

		public bool PlayStateChanged { get; }

		public bool WaitingForReadyChanged { get; }

		public bool IsPlaybackRelated
		{
			get
			{
				if (ChangeType != WatchPartyStateChangeType.PlaybackUpdated && ChangeType != WatchPartyStateChangeType.PlayStateChanged)
				{
					return ChangeType == WatchPartyStateChangeType.VideoChanged;
				}
				return true;
			}
		}

		public WatchPartyStateArgs(WatchPartyLocalState state, WatchPartyLocalState previousState, WatchPartyStateChangeType changeType)
		{
			State = state;
			PreviousState = previousState;
			ChangeType = changeType;
			VideoChanged = state?.CurrentVideoId != previousState?.CurrentVideoId;
			PlayStateChanged = state?.IsPlaying != previousState?.IsPlaying;
			WaitingForReadyChanged = state?.IsWaitingForReady != previousState?.IsWaitingForReady;
		}
	}
}
