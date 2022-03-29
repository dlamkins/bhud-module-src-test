using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Nekres.Screenshot_Manager.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
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
					resourceMan = new ResourceManager("Nekres.Screenshot_Manager.Properties.Resources", typeof(Resources).Assembly);
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

		internal static string A_duplicate_image_name_was_specified_ => ResourceManager.GetString("A duplicate image name was specified!", resourceCulture);

		internal static string Are_you_sure_ => ResourceManager.GetString("Are you sure?", resourceCulture);

		internal static string By_default__screenshots_are_sent_to_the_Recycle_Bin_so_that_they_can_be_recovered_if_needed__nWhen_this_feature_is_disabled__deleted_screenshots_are_removed_from_the_hard_disk_and_their_space_is_marked_as_overwriteable_ => ResourceManager.GetString("By default, screenshots are sent to the Recycle Bin so that they can be recovered if needed.\\nWhen this feature is disabled, deleted screenshots are removed from the hard disk and their space is marked as overwriteable.", resourceCulture);

		internal static string Cancel => ResourceManager.GetString("Cancel", resourceCulture);

		internal static string Click_To_Zoom => ResourceManager.GetString("Click To Zoom", resourceCulture);

		internal static string Delete_Image_ => ResourceManager.GetString("Delete Image?", resourceCulture);

		internal static string Delete_sends_to_Recycle_Bin => ResourceManager.GetString("Delete sends to Recycle Bin", resourceCulture);

		internal static string Disable_Screenshot_Notification => ResourceManager.GetString("Disable Screenshot Notification", resourceCulture);

		internal static string Disables_the_corner_icon_in_the_navigation_menu_ => ResourceManager.GetString("Disables the corner icon in the navigation menu.", resourceCulture);

		internal static string Disables_the_notification_when_a_new_screenshot_has_been_captured_ => ResourceManager.GetString("Disables the notification when a new screenshot has been captured.", resourceCulture);

		internal static string Failed_to_delete_image__0__ => ResourceManager.GetString("Failed to delete image {0}.", resourceCulture);

		internal static string Favourite => ResourceManager.GetString("Favourite", resourceCulture);

		internal static string Hide_Corner_Icon => ResourceManager.GetString("Hide Corner Icon", resourceCulture);

		internal static string Image_name_cannot_be_empty_ => ResourceManager.GetString("Image name cannot be empty.", resourceCulture);

		internal static string Mute_Screenshot_Sound => ResourceManager.GetString("Mute Screenshot Sound", resourceCulture);

		internal static string Mutes_the_sound_alert_when_a_new_screenshot_has_been_captured_ => ResourceManager.GetString("Mutes the sound alert when a new screenshot has been captured.", resourceCulture);

		internal static string Normal => ResourceManager.GetString("Normal", resourceCulture);

		internal static string Please_enter_a_different_image_name_ => ResourceManager.GetString("Please enter a different image name.", resourceCulture);

		internal static string Rename_Image => ResourceManager.GetString("Rename Image", resourceCulture);

		internal static string Screenshot => ResourceManager.GetString("Screenshot", resourceCulture);

		internal static string Screenshot_Created_ => ResourceManager.GetString("Screenshot Created!", resourceCulture);

		internal static string Search___ => ResourceManager.GetString("Search...", resourceCulture);

		internal static string Stereoscopic => ResourceManager.GetString("Stereoscopic", resourceCulture);

		internal static string Take_a_normal_screenshot_ => ResourceManager.GetString("Take a normal screenshot.", resourceCulture);

		internal static string Take_a_stereoscopic_screenshot_ => ResourceManager.GetString("Take a stereoscopic screenshot.", resourceCulture);

		internal static string The_following_characters_are_not_allowed___0_ => ResourceManager.GetString("The following characters are not allowed: {0}", resourceCulture);

		internal static string The_image_file_doesn_t_exist_anymore_ => ResourceManager.GetString("The image file doesn't exist anymore!", resourceCulture);

		internal static string The_image_file_is_in_use_by_another_process_ => ResourceManager.GetString("The image file is in use by another process.", resourceCulture);

		internal static string The_image_name_contains_invalid_characters_ => ResourceManager.GetString("The image name contains invalid characters.", resourceCulture);

		internal static string Unable_to_rename_image__0__ => ResourceManager.GetString("Unable to rename image {0}.", resourceCulture);

		internal static string Unfavourite => ResourceManager.GetString("Unfavourite", resourceCulture);

		internal static string Yes => ResourceManager.GetString("Yes", resourceCulture);

		internal static string You_are_about_to_permanently_destroy__0__ => ResourceManager.GetString("You are about to permanently destroy {0}.", resourceCulture);

		internal Resources()
		{
		}
	}
}
