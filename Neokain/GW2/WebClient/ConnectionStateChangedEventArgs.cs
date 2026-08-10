using System;

namespace Neokain.GW2.WebClient
{
	public sealed class ConnectionStateChangedEventArgs : EventArgs
	{
		public ConnectionState NewState { get; }

		public ConnectionState State => NewState;

		public AuthenticationSource AuthenticationSource { get; }

		public ConnectionStateChangedEventArgs(ConnectionState state, AuthenticationSource authSource)
		{
			NewState = state;
			AuthenticationSource = authSource;
		}
	}
}
