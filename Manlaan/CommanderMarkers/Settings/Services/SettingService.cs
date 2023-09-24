using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.Settings.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.CommanderMarkers.Settings.Services
{
	public class SettingService : IDisposable
	{
		public SettingEntry<bool> _settingGroundMarkersEnabled { get; private set; }

		public SettingEntry<bool> _settingTargetMarkersEnabled { get; private set; }

		public SettingEntry<KeyBinding> _settingArrowGndBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingCircleGndBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingHeartGndBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingSpiralGndBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingSquareGndBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingStarGndBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingTriangleGndBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingXGndBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingClearGndBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingArrowObjBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingCircleObjBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingHeartObjBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingSpiralObjBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingSquareObjBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingStarObjBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingTriangleObjBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingXObjBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingClearObjBinding { get; private set; }

		public SettingEntry<KeyBinding> _settingInteractKeyBinding { get; private set; }

		public SettingEntry<Point> _settingLoc { get; private set; }

		public SettingEntry<Layout> _settingOrientation { get; private set; }

		public SettingEntry<int> _settingImgWidth { get; private set; }

		public SettingEntry<float> _settingOpacity { get; private set; }

		public SettingEntry<bool> _settingDrag { get; private set; }

		public SettingEntry<bool> _settingShowMarkersPanel { get; private set; }

		public SettingEntry<bool> _settingOnlyWhenCommander { get; private set; }

		public SettingEntry<int> AutoMarker_PlacementDelay { get; private set; }

		public SettingEntry<bool> AutoMarker_OnlyWhenCommander { get; private set; }

		public SettingEntry<bool> AutoMarker_FeatureEnabled { get; private set; }

		public SettingEntry<bool> AutoMarker_LibraryFilterToCurrent { get; private set; }

		public SettingEntry<bool> AutoMarker_ShowPreview { get; private set; }

		public SettingEntry<bool> AutoMarker_ShowTrigger { get; private set; }

		public SettingEntry<bool> CornerIconEnabled { get; private set; }

		public SettingEntry<CornerIconActions> CornerIconLeftClickAction { get; private set; }

		public SettingService(SettingCollection settings)
		{
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Expected O, but got Unknown
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Expected O, but got Unknown
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Expected O, but got Unknown
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Expected O, but got Unknown
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Expected O, but got Unknown
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Expected O, but got Unknown
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Expected O, but got Unknown
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Expected O, but got Unknown
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Expected O, but got Unknown
			//IL_0416: Unknown result type (might be due to invalid IL or missing references)
			//IL_045e: Expected O, but got Unknown
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b5: Expected O, but got Unknown
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Expected O, but got Unknown
			//IL_051b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Expected O, but got Unknown
			//IL_0572: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ba: Expected O, but got Unknown
			//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0611: Expected O, but got Unknown
			//IL_0620: Unknown result type (might be due to invalid IL or missing references)
			//IL_0668: Expected O, but got Unknown
			//IL_0677: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bf: Expected O, but got Unknown
			//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0716: Expected O, but got Unknown
			//IL_0724: Unknown result type (might be due to invalid IL or missing references)
			//IL_076c: Expected O, but got Unknown
			//IL_077c: Unknown result type (might be due to invalid IL or missing references)
			_settingGroundMarkersEnabled = settings.DefineSetting<bool>("CmdMrkGnnEnabled", true, (Func<string>)(() => "Show icons for placing Ground Markers"), (Func<string>)(() => ""));
			_settingTargetMarkersEnabled = settings.DefineSetting<bool>("CmdMrkTgtEnabled", true, (Func<string>)(() => "Show icons for placing Target/Object Markers"), (Func<string>)(() => ""));
			_settingArrowGndBinding = settings.DefineSetting<KeyBinding>("CmdMrkArrowGndBinding", new KeyBinding((ModifierKeys)2, (Keys)49), (Func<string>)(() => "Arrow Ground Binding"), (Func<string>)(() => ""));
			_settingArrowGndBinding = settings.DefineSetting<KeyBinding>("CmdMrkArrowGndBinding", new KeyBinding((ModifierKeys)2, (Keys)49), (Func<string>)(() => "Arrow Ground Binding"), (Func<string>)(() => ""));
			_settingCircleGndBinding = settings.DefineSetting<KeyBinding>("CmdMrkCircleGndBinding", new KeyBinding((ModifierKeys)2, (Keys)50), (Func<string>)(() => "Circle Ground Binding"), (Func<string>)(() => ""));
			_settingHeartGndBinding = settings.DefineSetting<KeyBinding>("CmdMrkHeartGndBinding", new KeyBinding((ModifierKeys)2, (Keys)51), (Func<string>)(() => "Heart Ground Binding"), (Func<string>)(() => ""));
			_settingSquareGndBinding = settings.DefineSetting<KeyBinding>("CmdMrkSquareGndBinding", new KeyBinding((ModifierKeys)2, (Keys)52), (Func<string>)(() => "Square Ground Binding"), (Func<string>)(() => ""));
			_settingStarGndBinding = settings.DefineSetting<KeyBinding>("CmdMrkStarGndBinding", new KeyBinding((ModifierKeys)2, (Keys)53), (Func<string>)(() => "Star Ground Binding"), (Func<string>)(() => ""));
			_settingSpiralGndBinding = settings.DefineSetting<KeyBinding>("CmdMrkSpiralGndBinding", new KeyBinding((ModifierKeys)2, (Keys)54), (Func<string>)(() => "Spiral Ground Binding"), (Func<string>)(() => ""));
			_settingTriangleGndBinding = settings.DefineSetting<KeyBinding>("CmdMrkTriangleGndBinding", new KeyBinding((ModifierKeys)2, (Keys)55), (Func<string>)(() => "Triangle Ground Binding"), (Func<string>)(() => ""));
			_settingXGndBinding = settings.DefineSetting<KeyBinding>("CmdMrkXGndBinding", new KeyBinding((ModifierKeys)2, (Keys)56), (Func<string>)(() => "X Ground Binding"), (Func<string>)(() => ""));
			_settingClearGndBinding = settings.DefineSetting<KeyBinding>("CmdMrkClearGndBinding", new KeyBinding((ModifierKeys)2, (Keys)57), (Func<string>)(() => "Clear Ground Binding"), (Func<string>)(() => ""));
			_settingArrowObjBinding = settings.DefineSetting<KeyBinding>("CmdMrkArrowObjBinding", new KeyBinding((ModifierKeys)6, (Keys)49), (Func<string>)(() => "Arrow Object Binding"), (Func<string>)(() => ""));
			_settingCircleObjBinding = settings.DefineSetting<KeyBinding>("CmdMrkCircleObjBinding", new KeyBinding((ModifierKeys)6, (Keys)50), (Func<string>)(() => "Circle Object Binding"), (Func<string>)(() => ""));
			_settingHeartObjBinding = settings.DefineSetting<KeyBinding>("CmdMrkHeartObjBinding", new KeyBinding((ModifierKeys)6, (Keys)51), (Func<string>)(() => "Heart Object Binding"), (Func<string>)(() => ""));
			_settingSquareObjBinding = settings.DefineSetting<KeyBinding>("CmdMrkSquareObjBinding", new KeyBinding((ModifierKeys)6, (Keys)52), (Func<string>)(() => "Square Object Binding"), (Func<string>)(() => ""));
			_settingStarObjBinding = settings.DefineSetting<KeyBinding>("CmdMrkStarObjBinding", new KeyBinding((ModifierKeys)6, (Keys)53), (Func<string>)(() => "Star Object Binding"), (Func<string>)(() => ""));
			_settingSpiralObjBinding = settings.DefineSetting<KeyBinding>("CmdMrkSpiralObjBinding", new KeyBinding((ModifierKeys)6, (Keys)54), (Func<string>)(() => "Spiral Object Binding"), (Func<string>)(() => ""));
			_settingTriangleObjBinding = settings.DefineSetting<KeyBinding>("CmdMrkTriangleObjBinding", new KeyBinding((ModifierKeys)6, (Keys)55), (Func<string>)(() => "Triangle Object Binding"), (Func<string>)(() => ""));
			_settingXObjBinding = settings.DefineSetting<KeyBinding>("CmdMrkXObjBinding", new KeyBinding((ModifierKeys)6, (Keys)56), (Func<string>)(() => "X Object Binding"), (Func<string>)(() => ""));
			_settingClearObjBinding = settings.DefineSetting<KeyBinding>("CmdMrkClearObjBinding", new KeyBinding((ModifierKeys)6, (Keys)57), (Func<string>)(() => "Clear Object Binding"), (Func<string>)(() => ""));
			_settingInteractKeyBinding = settings.DefineSetting<KeyBinding>("CmdMrkInteractBinding", new KeyBinding((Keys)70), (Func<string>)(() => "Interact Key"), (Func<string>)(() => ""));
			_settingLoc = settings.DefineSetting<Point>("CmdMrkLoc", new Point(100, 100), (Func<string>)(() => "Location"), (Func<string>)(() => ""));
			_settingOrientation = settings.DefineSetting<Layout>("CmdMrkOrientation2", Layout.Horizontal, (Func<string>)(() => "Orientation"), (Func<string>)(() => ""));
			_settingImgWidth = settings.DefineSetting<int>("CmdMrkImgWidth", 30, (Func<string>)(() => "Icon Size"), (Func<string>)(() => "Set the size of the on screen marker icons"));
			_settingOpacity = settings.DefineSetting<float>("CmdMrkOpacity", 1f, (Func<string>)(() => "Opacity"), (Func<string>)(() => "Set the panel's transparency\nHidden<---->Visible"));
			_settingDrag = settings.DefineSetting<bool>("CmdMrkDrag", false, (Func<string>)(() => "Enable Dragging"), (Func<string>)(() => "Allow the clickable markers to be repositioned"));
			_settingShowMarkersPanel = settings.DefineSetting<bool>("CmdMrkShowMarkerPanelr", true, (Func<string>)(() => "Show clickable markers on screen"), (Func<string>)(() => "Hide/show the clickable markers panel"));
			_settingOnlyWhenCommander = settings.DefineSetting<bool>("CmdMrkOnlyCommander", false, (Func<string>)(() => "Only show when I am the Commander"), (Func<string>)(() => "Hides the clickable markers when you are not the Commander"));
			AutoMarker_PlacementDelay = settings.DefineSetting<int>("CmdMrkPlacementDelay", 100, (Func<string>)(() => "Placement Delay"), (Func<string>)(() => "Delay in milliseconds to wait between marker placement\nFaster <-----> Slower"));
			SettingComplianceExtensions.SetRange(AutoMarker_PlacementDelay, 50, 300);
			SettingComplianceExtensions.SetRange(_settingImgWidth, 16, 200);
			SettingComplianceExtensions.SetRange(_settingOpacity, 0.1f, 1f);
			AutoMarker_OnlyWhenCommander = settings.DefineSetting<bool>("CmdMrkAMOnlyCommander", true, (Func<string>)(() => "Only show when I am the Commander"), (Func<string>)(() => "Only show the AutoMarker activation zones on the map when you are the Commander"));
			AutoMarker_FeatureEnabled = settings.DefineSetting<bool>("CmdMrkAMEnabled", true, (Func<string>)(() => "Enable"), (Func<string>)(() => ""));
			AutoMarker_LibraryFilterToCurrent = settings.DefineSetting<bool>("CmdMrkAMLibraryFilter", false, (Func<string>)(() => "Filter to current map"), (Func<string>)(() => "Filter the library list to only show marker sets for your current map"));
			AutoMarker_ShowPreview = settings.DefineSetting<bool>("CmdMrkAMShowPreview", true, (Func<string>)(() => "Show preview of markers"), (Func<string>)(() => "Allows for drawing a preview of the markers on the map"));
			AutoMarker_ShowTrigger = settings.DefineSetting<bool>("CmdMrkAMShowTrigger", true, (Func<string>)(() => "Show map marker for AutoMarker set locations"), (Func<string>)(() => "Display the Blish holding markers map icon in locations where AutoMarker sets may be activated from the map"));
			CornerIconEnabled = settings.DefineSetting<bool>("CmdMrkCornerIconEnabled", true, (Func<string>)(() => "Show an icon in the top-left manu bar"), (Func<string>)(() => "Adds a shortcut icon in the top-left menu bar"));
			CornerIconLeftClickAction = settings.DefineSetting<CornerIconActions>("CmdMrkAMCornerIconAction", CornerIconActions.SHOW_ICON_MENU, (Func<string>)(() => "Icon left-click action"), (Func<string>)(() => "Select an action for menu bar icon left-click\nRight click will always open a small menu"));
		}

		public void Dispose()
		{
		}
	}
}
