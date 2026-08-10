using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.SignalR.Client.Internal
{
	internal sealed class ConnectionLogScope : IReadOnlyList<KeyValuePair<string, object?>>, IReadOnlyCollection<KeyValuePair<string, object?>>, IEnumerable<KeyValuePair<string, object?>>, IEnumerable
	{
		private const string ClientConnectionIdKey = "ClientConnectionId";

		private string _cachedToString;

		private string _connectionId;

		public string? ConnectionId
		{
			get
			{
				return _connectionId;
			}
			set
			{
				_cachedToString = null;
				_connectionId = value;
			}
		}

		public KeyValuePair<string, object?> this[int index]
		{
			get
			{
				if (index == 0)
				{
					return new KeyValuePair<string, object>("ClientConnectionId", ConnectionId);
				}
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public int Count => (!string.IsNullOrEmpty(ConnectionId)) ? 1 : 0;

		public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
		{
			int i = 0;
			while (i < Count)
			{
				yield return this[i];
				int num = i + 1;
				i = num;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override string ToString()
		{
			if (_cachedToString == null && !string.IsNullOrEmpty(ConnectionId))
			{
				_cachedToString = FormattableString.Invariant(FormattableStringFactory.Create("{0}:{1}", "ClientConnectionId", ConnectionId));
			}
			return _cachedToString ?? string.Empty;
		}
	}
}
