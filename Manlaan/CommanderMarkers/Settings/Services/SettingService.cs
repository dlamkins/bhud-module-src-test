using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.CommanderMarkers.Settings.Services
{
	public class SettingService : IDisposable
	{
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

		public SettingEntry<string> _settingOrientation { get; private set; }

		public SettingEntry<Point> _settingLoc { get; private set; }

		public SettingEntry<int> _settingImgWidth { get; private set; }

		public SettingEntry<float> _settingOpacity { get; private set; }

		public SettingEntry<bool> _settingDrag { get; private set; }

		public SettingEntry<bool> _settingShowMarkersPanel { get; private set; }

		public SettingEntry<bool> _settingOnlyWhenCommander { get; private set; }

		public SettingEntry<int> _settingMarkerPlaceDelay { get; private set; }

		public SettingService(SettingCollection settings)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Expected O, but got Unknown
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Expected O, but got Unknown
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Expected O, but got Unknown
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Expected O, but got Unknown
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Expected O, but got Unknown
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Expected O, but got Unknown
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Expected O, but got Unknown
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Expected O, but got Unknown
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_03be: Expected O, but got Unknown
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Expected O, but got Unknown
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Expected O, but got Unknown
			//IL_047b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c3: Expected O, but got Unknown
			//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_051a: Expected O, but got Unknown
			//IL_0529: Unknown result type (might be due to invalid IL or missing references)
			//IL_0571: Expected O, but got Unknown
			//IL_0580: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c8: Expected O, but got Unknown
			//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_061f: Expected O, but got Unknown
			//IL_062d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0675: Expected O, but got Unknown
			//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
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
			_settingOrientation = settings.DefineSetting<string>("CmdMrkOrientation2", "Horizontal", (Func<string>)(() => "Orientation"), (Func<string>)(() => ""));
			_settingLoc = settings.DefineSetting<Point>("CmdMrkLoc", new Point(100, 100), (Func<string>)(() => "Location"), (Func<string>)(() => ""));
			_settingImgWidth = settings.DefineSetting<int>("CmdMrkImgWidth", 30, (Func<string>)(() => "Width"), (Func<string>)(() => ""));
			_settingOpacity = settings.DefineSetting<float>("CmdMrkOpacity", 1f, (Func<string>)(() => "Opacity"), (Func<string>)(() => ""));
			_settingDrag = settings.DefineSetting<bool>("CmdMrkDrag", false, (Func<string>)(() => "Enable Dragging"), (Func<string>)(() => "Allow the markers to be repositioned"));
			_settingShowMarkersPanel = settings.DefineSetting<bool>("CmdMrkShowMarkerPanelr", true, (Func<string>)(() => "Show marker panel"), (Func<string>)(() => "Hide/show the mouse click markers panel"));
			_settingOnlyWhenCommander = settings.DefineSetting<bool>("CmdMrkOnlyCommander", true, (Func<string>)(() => "Only show when I am the Commander"), (Func<string>)(() => "Hides the markers when you are not a Commander"));
			_settingMarkerPlaceDelay = settings.DefineSetting<int>("CmdMrkPlacementDelay", 100, (Func<string>)(() => "Delay between automarker placement (ms)"), (Func<string>)(() => "Time in milliseconds to wait betweeen keypresses when placing markers"));
			SettingComplianceExtensions.SetRange(_settingMarkerPlaceDelay, 50, 300);
			SettingComplianceExtensions.SetRange(_settingImgWidth, 16, 200);
			SettingComplianceExtensions.SetRange(_settingOpacity, 0.1f, 1f);
		}

		public void Dispose()
		{
		}
	}
}
