using System;
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

		public Settings(SettingCollection settings)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			_settings = settings;
			_internal_settings = _settings.AddSubCollection("Internal", false);
			HotbarPosition = _internal_settings.DefineSetting<Point>("HotbarPosition", new Point(0, 32), (Func<string>)null, (Func<string>)null);
			HotbarExpandDirection = _settings.DefineSetting<ExpandType>("HotbarExpandDirection", ExpandType.LeftToRight, (Func<string>)null, (Func<string>)null);
			HotbarButtonSorting = _settings.DefineSetting<SortType>("HotbarButtonSorting", SortType.ActivesFirst, (Func<string>)null, (Func<string>)null);
			KeyboardLayout = _settings.DefineSetting<KeyboardLayoutType>("KeyboardLayout", KeyboardLayoutType.QWERTZ, (Func<string>)null, (Func<string>)null);
		}
	}
}
