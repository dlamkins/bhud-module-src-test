using Blish_HUD.Settings;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.QoL.Services
{
	public class Settings : BaseSettingsModel
	{
		private readonly SettingCollection _settings;

		private readonly SettingCollection _internal_settings;

		public SettingEntry<ExpandType> HotbarExpandDirection { get; }

		public SettingEntry<SortType> HotbarButtonSorting { get; }

		public SettingEntry<KeyboardLayoutType> KeyboardLayout { get; }

		public SettingEntry<Point> HotbarPosition { get; }

		public Settings(SettingCollection settingCollection)
			: base(settingCollection)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			_settings = settingCollection;
			_internal_settings = _settings.AddSubCollection("Internal");
			HotbarPosition = _internal_settings.DefineSetting<Point>("HotbarPosition", new Point(0, 32));
			HotbarExpandDirection = _settings.DefineSetting("HotbarExpandDirection", ExpandType.LeftToRight);
			HotbarButtonSorting = _settings.DefineSetting("HotbarButtonSorting", SortType.ActivesFirst);
			KeyboardLayout = _settings.DefineSetting("KeyboardLayout", KeyboardLayoutType.QWERTZ);
		}
	}
}
