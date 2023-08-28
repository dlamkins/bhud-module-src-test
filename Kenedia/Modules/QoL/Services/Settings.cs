using System;
using Blish_HUD.Settings;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.QoL.Res;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.QoL.Services
{
	public class Settings : BaseSettingsModel
	{
		private readonly SettingCollection _settings;

		private readonly SettingCollection _internal_settings;

		public SettingEntry<ExpandType> HotbarExpandDirection { get; }

		public SettingEntry<Point> HotbarPosition { get; }

		public Settings(SettingCollection settings)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			_settings = settings;
			_internal_settings = _settings.AddSubCollection("Internal", false);
			HotbarPosition = _internal_settings.DefineSetting<Point>("HotbarPosition", new Point(0, 32), (Func<string>)null, (Func<string>)null);
			HotbarExpandDirection = _settings.DefineSetting<ExpandType>("HotbarExpandDirection", ExpandType.LeftToRight, (Func<string>)(() => strings.HotbarExpandDirection_Name), (Func<string>)(() => strings.HotbarExpandDirection_Tooltip));
		}
	}
}
