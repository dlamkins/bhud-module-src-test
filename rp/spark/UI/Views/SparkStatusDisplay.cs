using System;
using Microsoft.Xna.Framework;
using rp.spark.Services;

namespace rp.spark.UI.Views
{
	internal sealed class SparkStatusDisplay
	{
		private static readonly Color ReadyColor = new Color(140, 220, 140);

		private static readonly Color StartingColor = new Color(255, 194, 55);

		public string ReadinessText { get; }

		public string ReadinessTooltip { get; }

		public Color ReadinessColor { get; }

		public string ServerText { get; }

		public string ServerTooltip { get; }

		public Color ServerColor { get; }

		public string CombinedText => ReadinessText + " • " + ServerText;

		public string CombinedTooltip => JoinLines(ReadinessTooltip, ServerTooltip);

		public Color CombinedColor
		{
			get
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				if (!IsReady || !IsServerConnected)
				{
					return StartingColor;
				}
				return ReadyColor;
			}
		}

		private bool IsReady => string.Equals(ReadinessText, "SPARK ready", StringComparison.Ordinal);

		private bool IsServerConnected => string.Equals(ServerText, "Server: Connected", StringComparison.Ordinal);

		private SparkStatusDisplay(string readinessText, string readinessTooltip, Color readinessColor, string serverText, string serverTooltip, Color serverColor)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			ReadinessText = readinessText;
			ReadinessTooltip = readinessTooltip;
			ReadinessColor = readinessColor;
			ServerText = serverText;
			ServerTooltip = serverTooltip;
			ServerColor = serverColor;
		}

		public static SparkStatusDisplay Create(Func<string> getImportantNotice, Func<ServerSyncStatus> getServerStatus)
		{
			return Create(SafeInvoke(getImportantNotice), SafeInvoke(getServerStatus));
		}

		public static SparkStatusDisplay Create(string notice, ServerSyncStatus serverStatus)
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			notice = notice?.Trim() ?? string.Empty;
			bool ready = string.IsNullOrWhiteSpace(notice);
			bool connected = serverStatus != null && serverStatus.State == ServerSyncState.Connected;
			return new SparkStatusDisplay(ready ? "SPARK ready" : "Starting up...", ready ? "SPARK is ready." : notice, ready ? ReadyColor : StartingColor, "Server: " + GetServerText(serverStatus), GetServerTooltip(serverStatus), connected ? ReadyColor : StartingColor);
		}

		private static string GetServerText(ServerSyncStatus status)
		{
			if (status == null)
			{
				return "Disconnected";
			}
			if (!string.IsNullOrWhiteSpace(status.DisplayName))
			{
				return status.DisplayName.Trim();
			}
			if (status.State != 0)
			{
				return "Attention needed";
			}
			return "Getting ready";
		}

		private static string GetServerTooltip(ServerSyncStatus status)
		{
			if (status == null)
			{
				return "SPARK cannot connect.";
			}
			if (string.IsNullOrWhiteSpace(status.DisplayName))
			{
				return status.Message?.Trim() ?? string.Empty;
			}
			if (!string.IsNullOrWhiteSpace(status.Message))
			{
				return status.DisplayName.Trim() + ": " + status.Message.Trim();
			}
			return status.DisplayName.Trim();
		}

		private static string JoinLines(string first, string second)
		{
			first = first?.Trim() ?? string.Empty;
			second = second?.Trim() ?? string.Empty;
			if (string.IsNullOrWhiteSpace(first))
			{
				return second;
			}
			if (string.IsNullOrWhiteSpace(second))
			{
				return first;
			}
			return first + Environment.NewLine + second;
		}

		private static string SafeInvoke(Func<string> getValue)
		{
			try
			{
				return getValue?.Invoke() ?? string.Empty;
			}
			catch
			{
				return string.Empty;
			}
		}

		private static ServerSyncStatus SafeInvoke(Func<ServerSyncStatus> getValue)
		{
			try
			{
				return getValue?.Invoke();
			}
			catch
			{
				return null;
			}
		}
	}
}
