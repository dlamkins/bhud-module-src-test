using TargetYourFeet.Settings.Services;

namespace TargetYourFeet
{
	public static class Service
	{
		public static Module ModuleInstance { get; set; }

		public static SettingService Settings { get; set; }
	}
}
