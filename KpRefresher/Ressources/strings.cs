using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace KpRefresher.Ressources
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class strings
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					resourceMan = new ResourceManager("KpRefresher.Ressources.strings", typeof(strings).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static string BusinessService_Error => ResourceManager.GetString("BusinessService_Error", resourceCulture);

		internal static string BusinessService_Refreshed => ResourceManager.GetString("BusinessService_Refreshed", resourceCulture);

		internal static string BusinessService_RefreshNotAvailable => ResourceManager.GetString("BusinessService_RefreshNotAvailable", resourceCulture);

		internal static string BusinessService_Wing => ResourceManager.GetString("BusinessService_Wing", resourceCulture);

		internal static string CornerIcon_CancelNotify => ResourceManager.GetString("CornerIcon_CancelNotify", resourceCulture);

		internal static string CornerIcon_Copy => ResourceManager.GetString("CornerIcon_Copy", resourceCulture);

		internal static string CornerIcon_Notify => ResourceManager.GetString("CornerIcon_Notify", resourceCulture);

		internal static string CornerIcon_OpenWebsite => ResourceManager.GetString("CornerIcon_OpenWebsite", resourceCulture);

		internal static string CornerIcon_Refresh => ResourceManager.GetString("CornerIcon_Refresh", resourceCulture);

		internal static string GW2APIService_Bank => ResourceManager.GetString("GW2APIService_Bank", resourceCulture);

		internal static string GW2APIService_SharedSlots => ResourceManager.GetString("GW2APIService_SharedSlots", resourceCulture);

		internal static string LoadingSpinner_Fetch => ResourceManager.GetString("LoadingSpinner_Fetch", resourceCulture);

		internal static string MainWindow_Actions_Title => ResourceManager.GetString("MainWindow_Actions_Title", resourceCulture);

		internal static string MainWindow_Button_ClearNotif_Label => ResourceManager.GetString("MainWindow_Button_ClearNotif_Label", resourceCulture);

		internal static string MainWindow_Button_ClearSchedule_Label => ResourceManager.GetString("MainWindow_Button_ClearSchedule_Label", resourceCulture);

		internal static string MainWindow_Button_ClearSchedule_Tooltip => ResourceManager.GetString("MainWindow_Button_ClearSchedule_Tooltip", resourceCulture);

		internal static string MainWindow_Button_Refresh_Label => ResourceManager.GetString("MainWindow_Button_Refresh_Label", resourceCulture);

		internal static string MainWindow_Button_Refresh_Tooltip => ResourceManager.GetString("MainWindow_Button_Refresh_Tooltip", resourceCulture);

		internal static string MainWindow_Button_RefreshLinkedAccounts_Label => ResourceManager.GetString("MainWindow_Button_RefreshLinkedAccounts_Label", resourceCulture);

		internal static string MainWindow_Button_RefreshLinkedAccounts_Tooltip => ResourceManager.GetString("MainWindow_Button_RefreshLinkedAccounts_Tooltip", resourceCulture);

		internal static string MainWindow_Button_ShowClears_Label => ResourceManager.GetString("MainWindow_Button_ShowClears_Label", resourceCulture);

		internal static string MainWindow_Button_ShowClears_Tooltip => ResourceManager.GetString("MainWindow_Button_ShowClears_Tooltip", resourceCulture);

		internal static string MainWindow_Button_ShowKP_Label => ResourceManager.GetString("MainWindow_Button_ShowKP_Label", resourceCulture);

		internal static string MainWindow_Button_ShowKP_Tooltip => ResourceManager.GetString("MainWindow_Button_ShowKP_Tooltip", resourceCulture);

		internal static string MainWindow_Configuration_Title => ResourceManager.GetString("MainWindow_Configuration_Title", resourceCulture);

		internal static string MainWindow_DelayBeforeRefreshOnMapChange_Label => ResourceManager.GetString("MainWindow_DelayBeforeRefreshOnMapChange_Label", resourceCulture);

		internal static string MainWindow_DelayBeforeRefreshOnMapChange_Tooltip => ResourceManager.GetString("MainWindow_DelayBeforeRefreshOnMapChange_Tooltip", resourceCulture);

		internal static string MainWindow_EnableAutoRetry_Label => ResourceManager.GetString("MainWindow_EnableAutoRetry_Label", resourceCulture);

		internal static string MainWindow_EnableAutoRetry_Tooltip => ResourceManager.GetString("MainWindow_EnableAutoRetry_Tooltip", resourceCulture);

		internal static string MainWindow_EnableRefreshOnKill_Label => ResourceManager.GetString("MainWindow_EnableRefreshOnKill_Label", resourceCulture);

		internal static string MainWindow_EnableRefreshOnKill_Tooltip => ResourceManager.GetString("MainWindow_EnableRefreshOnKill_Tooltip", resourceCulture);

		internal static string MainWindow_Notif_LinkedAccounts => ResourceManager.GetString("MainWindow_Notif_LinkedAccounts", resourceCulture);

		internal static string MainWindow_Notif_Loading => ResourceManager.GetString("MainWindow_Notif_Loading", resourceCulture);

		internal static string MainWindow_Notif_NoLinkedAccount => ResourceManager.GetString("MainWindow_Notif_NoLinkedAccount", resourceCulture);

		internal static string MainWindow_Notif_NoSchedule => ResourceManager.GetString("MainWindow_Notif_NoSchedule", resourceCulture);

		internal static string MainWindow_Notif_ScheduleDisabled => ResourceManager.GetString("MainWindow_Notif_ScheduleDisabled", resourceCulture);

		internal static string MainWindow_RefreshOnKillOnlyBoss_Label => ResourceManager.GetString("MainWindow_RefreshOnKillOnlyBoss_Label", resourceCulture);

		internal static string MainWindow_RefreshOnKillOnlyBoss_Tooltip => ResourceManager.GetString("MainWindow_RefreshOnKillOnlyBoss_Tooltip", resourceCulture);

		internal static string MainWindow_RefreshOnMapChange_Label => ResourceManager.GetString("MainWindow_RefreshOnMapChange_Label", resourceCulture);

		internal static string MainWindow_RefreshOnMapChange_Tooltip => ResourceManager.GetString("MainWindow_RefreshOnMapChange_Tooltip", resourceCulture);

		internal static string MainWindow_ShowScheduleNotification_Label => ResourceManager.GetString("MainWindow_ShowScheduleNotification_Label", resourceCulture);

		internal static string MainWindow_ShowScheduleNotification_Tooltip => ResourceManager.GetString("MainWindow_ShowScheduleNotification_Tooltip", resourceCulture);

		internal static string MainWindow_Spinner_Minutes => ResourceManager.GetString("MainWindow_Spinner_Minutes", resourceCulture);

		internal static string MainWindow_Spinner_Seconds => ResourceManager.GetString("MainWindow_Spinner_Seconds", resourceCulture);

		internal static string Notification_AccountNameFetchError => ResourceManager.GetString("Notification_AccountNameFetchError", resourceCulture);

		internal static string Notification_CopiedToClipboard => ResourceManager.GetString("Notification_CopiedToClipboard", resourceCulture);

		internal static string Notification_DataNotAvailable => ResourceManager.GetString("Notification_DataNotAvailable", resourceCulture);

		internal static string Notification_InstanceExitDetected => ResourceManager.GetString("Notification_InstanceExitDetected", resourceCulture);

		internal static string Notification_KpAccountAnonymous => ResourceManager.GetString("Notification_KpAccountAnonymous", resourceCulture);

		internal static string Notification_KpAccountUnknown => ResourceManager.GetString("Notification_KpAccountUnknown", resourceCulture);

		internal static string Notification_KPProfileFetchError => ResourceManager.GetString("Notification_KPProfileFetchError", resourceCulture);

		internal static string Notification_NextRefreshAvailableIn => ResourceManager.GetString("Notification_NextRefreshAvailableIn", resourceCulture);

		internal static string Notification_NoClearRefreshAborted => ResourceManager.GetString("Notification_NoClearRefreshAborted", resourceCulture);

		internal static string Notification_NotifyScheduled => ResourceManager.GetString("Notification_NotifyScheduled", resourceCulture);

		internal static string Notification_RefreshAvailable => ResourceManager.GetString("Notification_RefreshAvailable", resourceCulture);

		internal static string Notification_RefreshNotAvailable => ResourceManager.GetString("Notification_RefreshNotAvailable", resourceCulture);

		internal static string Notification_RefreshNotAvailableRetry => ResourceManager.GetString("Notification_RefreshNotAvailableRetry", resourceCulture);

		internal static string Notification_RefreshOk => ResourceManager.GetString("Notification_RefreshOk", resourceCulture);

		internal static string Notification_TryScheduled => ResourceManager.GetString("Notification_TryScheduled", resourceCulture);

		internal strings()
		{
		}
	}
}
