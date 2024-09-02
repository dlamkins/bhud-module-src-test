using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class DrfConnectionStatusService
	{
		public const string DRF_CONNECTED_TEXT = "Connected";

		private const string SMILEY_VERTICAL_SPACE = "  ";

		private static readonly Color RED = new Color(255, 120, 120);

		public static Color GetDrfConnectionStatusTextColor(DrfConnectionStatus drfConnectionStatus)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			switch (drfConnectionStatus)
			{
			case DrfConnectionStatus.Connecting:
			case DrfConnectionStatus.TryReconnect:
				return Color.get_Yellow();
			case DrfConnectionStatus.Connected:
				return Color.get_LightGreen();
			case DrfConnectionStatus.Disconnected:
			case DrfConnectionStatus.AuthenticationFailed:
				return RED;
			default:
				Module.Logger.Error(Helper.CreateSwitchCaseNotFoundMessage(drfConnectionStatus, "DrfConnectionStatus", "white"));
				return RED;
			}
		}

		public static string GetSettingTabDrfConnectionStatusText(DrfConnectionStatus drfConnectionStatus, int reconnectTriesCounter, int reconnectDelaySeconds)
		{
			switch (drfConnectionStatus)
			{
			case DrfConnectionStatus.Disconnected:
				return "Disconnected  :-(";
			case DrfConnectionStatus.Connecting:
				return "Connecting...";
			case DrfConnectionStatus.TryReconnect:
				return $"Connect failed {reconnectTriesCounter} time(s). Next reconnect try in {reconnectDelaySeconds} seconds.";
			case DrfConnectionStatus.Connected:
				return "Connected  :-)";
			case DrfConnectionStatus.AuthenticationFailed:
				return "Authentication failed. Add a valid DRF Token!  :-(";
			case DrfConnectionStatus.ModuleError:
				return "Module Error.  :-( Report bug on Discord:\nhttps://discord.com/invite/FYKN3qh";
			default:
				Module.Logger.Error(Helper.CreateSwitchCaseNotFoundMessage(drfConnectionStatus, "DrfConnectionStatus", "unknown status"));
				return "Unknown Status.  :-(";
			}
		}

		public static string GetSummaryTabDrfConnectionStatusText(DrfConnectionStatus drfConnectionStatus)
		{
			return drfConnectionStatus switch
			{
				DrfConnectionStatus.Disconnected => "DRF disconnected  :-(", 
				DrfConnectionStatus.Connecting => "DRF connecting...", 
				DrfConnectionStatus.TryReconnect => "DRF connect failed. Next reconnect try in a few seconds  :-(", 
				DrfConnectionStatus.AuthenticationFailed => "DRF authentication failed. Add a valid DRF Token!  :-(", 
				DrfConnectionStatus.ModuleError => "Module Error.  :-( Report bug on Discord:\nhttps://discord.com/invite/FYKN3qh", 
				_ => "", 
			};
		}
	}
}
