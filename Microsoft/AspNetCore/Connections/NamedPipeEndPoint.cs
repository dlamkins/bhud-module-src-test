using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Microsoft.AspNetCore.Connections
{
	internal sealed class NamedPipeEndPoint : EndPoint
	{
		internal const string LocalComputerServerName = ".";

		public string ServerName { get; }

		public string PipeName { get; }

		public NamedPipeEndPoint(string pipeName)
			: this(pipeName, ".")
		{
		}

		public NamedPipeEndPoint(string pipeName, string serverName)
		{
			ServerName = serverName;
			PipeName = pipeName;
		}

		public override string ToString()
		{
			return "\\\\" + ServerName + "\\pipe\\" + PipeName;
		}

		public override bool Equals([_003Cd2ad1736_002D1597_002D4257_002D9dd6_002D96df07157ca6_003ENotNullWhen(true)] object? obj)
		{
			NamedPipeEndPoint namedPipeEndPoint = obj as NamedPipeEndPoint;
			if (namedPipeEndPoint != null && namedPipeEndPoint.ServerName == ServerName)
			{
				return namedPipeEndPoint.PipeName == PipeName;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ServerName.GetHashCode() ^ PipeName.GetHashCode();
		}
	}
}
