using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;

namespace Frtal.Wayfinder
{
	public class ModuleSettings
	{
		public SettingEntry<bool> ShowWaypoints { get; }

		public SettingEntry<bool> ShowPois { get; }

		public SettingEntry<bool> ShowVistas { get; }

		public SettingEntry<bool> ShowHearts { get; }

		public SettingEntry<bool> ShowSkillPoints { get; }

		public SettingEntry<KeyBinding> KeyToggleCompass { get; }

		public SettingEntry<KeyBinding> KeyToggleMode { get; }

		public SettingEntry<KeyBinding> KeyPeek { get; }

		public SettingEntry<KeyBinding> KeyDiscovery { get; }

		public SettingEntry<int> PeekSeconds { get; }

		public Action OpenDiscovery { get; set; }

		public SettingEntry<int> WidthPercent { get; }

		public SettingEntry<int> ScalePercent { get; }

		public SettingEntry<int> OpacityPercent { get; }

		public SettingEntry<int> BackgroundOpacity { get; }

		public SettingEntry<int> TopOffset { get; }

		public SettingEntry<bool> ShowDistance { get; }

		public SettingEntry<bool> ShowNames { get; }

		public SettingEntry<bool> ShowCardinals { get; }

		public SettingEntry<bool> MonochromeIcons { get; }

		public SettingEntry<bool> GreyUndiscovered { get; }

		public SettingEntry<int> EdgeFadePercent { get; }

		public SettingEntry<bool> ScaleWithDistance { get; }

		public SettingEntry<int> IconScaleNear { get; }

		public SettingEntry<int> IconScaleFar { get; }

		public SettingEntry<int> IconScaleDistance { get; }

		public SettingEntry<bool> DragMode { get; }

		public SettingEntry<bool> RadialMode { get; }

		public SettingEntry<int> RadialRadius { get; }

		public SettingEntry<int> RadialVerticalOffset { get; }

		public SettingEntry<int> RadialIconSize { get; }

		public SettingEntry<int> RadialIconOpacity { get; }

		public SettingEntry<bool> MatchGameFov { get; }

		public SettingEntry<int> FovDegrees { get; }

		public SettingEntry<int> MaxDistanceMeters { get; }

		public SettingEntry<int> MaxPerCategory { get; }

		public SettingEntry<bool> ContentGuideLite { get; }

		public SettingEntry<bool> ClickToComplete { get; }

		public SettingEntry<int> Smoothing { get; }

		public SettingEntry<int> DiscoveryThresholdMeters { get; }

		public SettingEntry<bool> OnlyUndiscovered { get; }

		public SettingEntry<bool> Debug { get; }

		public SettingEntry<bool> MapOverlayDebug { get; }

		public SettingEntry<int> PositionX { get; }

		public SettingEntry<int> PositionY { get; }

		public ModuleSettings(SettingCollection settings)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Expected O, but got Unknown
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Expected O, but got Unknown
			SettingCollection hotkeys = settings.AddSubCollection("hotkeys", true, (Func<string>)(() => "Hotkeys"));
			KeyToggleCompass = hotkeys.DefineSetting<KeyBinding>("keytoggle", new KeyBinding((Keys)0), (Func<string>)(() => "Show / hide the compass"), (Func<string>)(() => "Toggles the compass on and off."));
			KeyToggleMode = hotkeys.DefineSetting<KeyBinding>("keymode", new KeyBinding((Keys)0), (Func<string>)(() => "Switch bar / radial"), (Func<string>)(() => "Switches between the bar compass and the radial one."));
			KeyPeek = hotkeys.DefineSetting<KeyBinding>("keypeek", new KeyBinding((Keys)0), (Func<string>)(() => "Peek at the compass"), (Func<string>)(() => "Briefly fades the compass in and back out again."));
			KeyDiscovery = hotkeys.DefineSetting<KeyBinding>("keydiscovery", new KeyBinding((Keys)0), (Func<string>)(() => "Open discovery window"), (Func<string>)(() => "Opens the list of discovered objectives for this map."));
			PeekSeconds = hotkeys.DefineSetting<int>("peekseconds", 4, (Func<string>)(() => "Peek duration"), (Func<string>)null);
			SettingComplianceExtensions.SetRange(PeekSeconds, 1, 15);
			SettingCollection categories = settings.AddSubCollection("categories", true, (Func<string>)(() => "Categories"));
			ShowWaypoints = categories.DefineSetting<bool>("waypoints", true, (Func<string>)(() => "Waypoints"), (Func<string>)null);
			ShowPois = categories.DefineSetting<bool>("pois", true, (Func<string>)(() => "POIs / Landmarks"), (Func<string>)null);
			ShowVistas = categories.DefineSetting<bool>("vistas", true, (Func<string>)(() => "Vistas"), (Func<string>)null);
			ShowHearts = categories.DefineSetting<bool>("hearts", true, (Func<string>)(() => "Hearts (renown)"), (Func<string>)null);
			ShowSkillPoints = categories.DefineSetting<bool>("skillpoints", true, (Func<string>)(() => "Skill / Hero points"), (Func<string>)null);
			SettingCollection appearance = settings.AddSubCollection("appearance", true, (Func<string>)(() => "Appearance"));
			WidthPercent = appearance.DefineSetting<int>("width", 50, (Func<string>)(() => "Bar width"), (Func<string>)null);
			ScalePercent = appearance.DefineSetting<int>("scale", 100, (Func<string>)(() => "Scale"), (Func<string>)null);
			OpacityPercent = appearance.DefineSetting<int>("opacity", 85, (Func<string>)(() => "Opacity"), (Func<string>)null);
			BackgroundOpacity = appearance.DefineSetting<int>("bgopacity", 45, (Func<string>)(() => "Background opacity"), (Func<string>)null);
			TopOffset = appearance.DefineSetting<int>("top", 8, (Func<string>)(() => "Top offset"), (Func<string>)null);
			ShowDistance = appearance.DefineSetting<bool>("distance", true, (Func<string>)(() => "Show distance"), (Func<string>)null);
			ShowNames = appearance.DefineSetting<bool>("names", false, (Func<string>)(() => "Show names under icons"), (Func<string>)null);
			ShowCardinals = appearance.DefineSetting<bool>("cardinals", true, (Func<string>)(() => "Show cardinal directions (N/E/S/W)"), (Func<string>)null);
			MonochromeIcons = appearance.DefineSetting<bool>("monoicons", false, (Func<string>)(() => "Monochrome icons"), (Func<string>)null);
			GreyUndiscovered = appearance.DefineSetting<bool>("greyundisc", false, (Func<string>)(() => "Grey out undiscovered objectives"), (Func<string>)null);
			EdgeFadePercent = appearance.DefineSetting<int>("edgefade", 14, (Func<string>)(() => "Edge fade"), (Func<string>)null);
			ScaleWithDistance = appearance.DefineSetting<bool>("scaledist", true, (Func<string>)(() => "Nearer icons appear larger"), (Func<string>)null);
			IconScaleNear = appearance.DefineSetting<int>("scalenear", 200, (Func<string>)(() => "Icon size when close"), (Func<string>)null);
			IconScaleFar = appearance.DefineSetting<int>("scalefar", 45, (Func<string>)(() => "Icon size when far"), (Func<string>)null);
			IconScaleDistance = appearance.DefineSetting<int>("scaledistm", 250, (Func<string>)(() => "Distance at which icons are smallest"), (Func<string>)null);
			DragMode = appearance.DefineSetting<bool>("dragmode", false, (Func<string>)(() => "Drag mode (move the compass with the mouse)"), (Func<string>)null);
			SettingComplianceExtensions.SetRange(WidthPercent, 20, 100);
			SettingComplianceExtensions.SetRange(ScalePercent, 50, 200);
			SettingComplianceExtensions.SetRange(OpacityPercent, 10, 100);
			SettingComplianceExtensions.SetRange(BackgroundOpacity, 0, 100);
			SettingComplianceExtensions.SetRange(TopOffset, 0, 300);
			SettingComplianceExtensions.SetRange(IconScaleNear, 100, 400);
			SettingComplianceExtensions.SetRange(IconScaleFar, 10, 100);
			SettingComplianceExtensions.SetRange(IconScaleDistance, 20, 2000);
			SettingComplianceExtensions.SetRange(EdgeFadePercent, 0, 40);
			SettingCollection radial = settings.AddSubCollection("radial", true, (Func<string>)(() => "Radial mode (experimental)"));
			RadialMode = radial.DefineSetting<bool>("radialmode", false, (Func<string>)(() => "Ring at the character's feet instead of a bar"), (Func<string>)null);
			RadialRadius = radial.DefineSetting<int>("radialradius", 10, (Func<string>)(() => "Ring radius"), (Func<string>)null);
			RadialVerticalOffset = radial.DefineSetting<int>("radialvoffset", 0, (Func<string>)(() => "Vertical offset"), (Func<string>)null);
			RadialIconSize = radial.DefineSetting<int>("radialiconsize", 28, (Func<string>)(() => "Icon size"), (Func<string>)null);
			RadialIconOpacity = radial.DefineSetting<int>("radialiconopacity", 100, (Func<string>)(() => "Icon opacity"), (Func<string>)null);
			SettingComplianceExtensions.SetRange(RadialRadius, 2, 60);
			SettingComplianceExtensions.SetRange(RadialVerticalOffset, -3, 5);
			SettingComplianceExtensions.SetRange(RadialIconSize, 12, 64);
			SettingComplianceExtensions.SetRange(RadialIconOpacity, 10, 100);
			SettingCollection behavior = settings.AddSubCollection("behavior", true, (Func<string>)(() => "Behavior"));
			MatchGameFov = behavior.DefineSetting<bool>("matchfov", true, (Func<string>)(() => "Match game field of view"), (Func<string>)null);
			FovDegrees = behavior.DefineSetting<int>("fov", 180, (Func<string>)(() => "Manual field of view"), (Func<string>)null);
			MaxDistanceMeters = behavior.DefineSetting<int>("maxdist", 750, (Func<string>)(() => "Max distance"), (Func<string>)null);
			MaxPerCategory = behavior.DefineSetting<int>("maxpercat", 50, (Func<string>)(() => "Max icons per category"), (Func<string>)null);
			ContentGuideLite = behavior.DefineSetting<bool>("cglite", false, (Func<string>)(() => "Content guide - lite (nearest objective only)"), (Func<string>)null);
			ClickToComplete = behavior.DefineSetting<bool>("clickdone", false, (Func<string>)(() => "Click an icon to mark it found"), (Func<string>)null);
			Smoothing = behavior.DefineSetting<int>("smoothing", 55, (Func<string>)(() => "Motion smoothing"), (Func<string>)null);
			DiscoveryThresholdMeters = behavior.DefineSetting<int>("discthreshold", 25, (Func<string>)(() => "Discovery radius"), (Func<string>)null);
			OnlyUndiscovered = behavior.DefineSetting<bool>("onlyundiscovered", false, (Func<string>)(() => "Show only undiscovered (local tracking)"), (Func<string>)null);
			SettingComplianceExtensions.SetRange(FovDegrees, 60, 360);
			SettingComplianceExtensions.SetRange(MaxDistanceMeters, 50, 10000);
			SettingComplianceExtensions.SetRange(MaxPerCategory, 1, 50);
			SettingComplianceExtensions.SetRange(Smoothing, 0, 95);
			SettingComplianceExtensions.SetRange(DiscoveryThresholdMeters, 5, 200);
			SettingCollection dev = settings.AddSubCollection("developer", true, (Func<string>)(() => "Developer"));
			Debug = dev.DefineSetting<bool>("debug", false, (Func<string>)(() => "Debug (print coordinates on the bar)"), (Func<string>)null);
			MapOverlayDebug = dev.DefineSetting<bool>("mapoverlay", false, (Func<string>)(() => "Debug: project POIs onto the open map"), (Func<string>)null);
			SettingCollection internals = settings.AddSubCollection("internal", false);
			PositionX = internals.DefineSetting<int>("posx", -1, (Func<string>)null, (Func<string>)null);
			PositionY = internals.DefineSetting<int>("posy", -1, (Func<string>)null, (Func<string>)null);
		}
	}
}
