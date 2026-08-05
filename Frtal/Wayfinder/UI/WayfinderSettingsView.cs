using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace Frtal.Wayfinder.UI
{
	public class WayfinderSettingsView : View
	{
		private const int RowHeight = 24;

		private const int LabelWidth = 190;

		private const int ValueWidth = 62;

		private readonly ModuleSettings _s;

		public WayfinderSettingsView(ModuleSettings settings)
			: this()
		{
			_s = settings;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Expected O, but got Unknown
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Expected O, but got Unknown
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Expected O, but got Unknown
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Expected O, but got Unknown
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_0425: Expected O, but got Unknown
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d2: Expected O, but got Unknown
			//IL_0544: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 8f));
			val.set_OuterControlPadding(new Vector2(4f, 6f));
			((Panel)val).set_CanScroll(true);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			int w = buildPanel.get_ContentRegion().Width - 28;
			FlowPanel p = Section((Container)val, "Hotkeys", w);
			Keybind((Container)(object)p, "Show / hide compass", _s.KeyToggleCompass, w);
			Keybind((Container)(object)p, "Switch bar / radial", _s.KeyToggleMode, w);
			Keybind((Container)(object)p, "Peek at compass", _s.KeyPeek, w);
			Keybind((Container)(object)p, "Discovery window", _s.KeyDiscovery, w);
			Slider((Container)(object)p, "Peek duration", _s.PeekSeconds, 1, 15, "s", w, "How long the compass stays visible after pressing the peek hotkey.");
			FlowPanel p2 = Section((Container)val, "What to show", w);
			Check((Container)(object)p2, "Waypoints", _s.ShowWaypoints, w);
			Check((Container)(object)p2, "POIs / Landmarks", _s.ShowPois, w);
			Check((Container)(object)p2, "Vistas", _s.ShowVistas, w);
			Check((Container)(object)p2, "Hearts (renown)", _s.ShowHearts, w);
			Check((Container)(object)p2, "Skill / Hero points", _s.ShowSkillPoints, w);
			Slider((Container)(object)p2, "Max distance", _s.MaxDistanceMeters, 50, 10000, "m", w, "Objectives further away than this are hidden.");
			Slider((Container)(object)p2, "Max per category", _s.MaxPerCategory, 1, 50, "", w, "Shows only the nearest N icons of each category, to keep the compass readable.");
			Check((Container)(object)p2, "Show only undiscovered", _s.OnlyUndiscovered, w, "Uses local tracking: an objective counts as discovered once you have been near it.");
			Slider((Container)(object)p2, "Discovery radius", _s.DiscoveryThresholdMeters, 5, 200, "m", w, "How close you must get for an objective to count as discovered.");
			Button((Container)(object)p2, "Manage discovered...", w, delegate
			{
				_s.OpenDiscovery?.Invoke();
			});
			FlowPanel p3 = Section((Container)val, "Look", w);
			Slider((Container)(object)p3, "Bar width", _s.WidthPercent, 20, 100, "%", w);
			Slider((Container)(object)p3, "Scale", _s.ScalePercent, 50, 200, "%", w);
			Slider((Container)(object)p3, "Opacity", _s.OpacityPercent, 10, 100, "%", w);
			Slider((Container)(object)p3, "Background opacity", _s.BackgroundOpacity, 0, 100, "%", w, "0 % is fully transparent, 100 % is solid black.");
			Slider((Container)(object)p3, "Top offset", _s.TopOffset, 0, 300, "px", w, "Ignored once you move the compass yourself in drag mode.");
			Check((Container)(object)p3, "Show distance", _s.ShowDistance, w);
			Check((Container)(object)p3, "Show names under icons", _s.ShowNames, w);
			Check((Container)(object)p3, "Show cardinal directions", _s.ShowCardinals, w, "N / E / S / W markers.");
			Check((Container)(object)p3, "Monochrome icons", _s.MonochromeIcons, w);
			FlowPanel p4 = Section((Container)val, "Distance scaling", w);
			Check((Container)(object)p4, "Nearer icons appear larger", _s.ScaleWithDistance, w);
			Slider((Container)(object)p4, "Size when close", _s.IconScaleNear, 100, 400, "%", w);
			Slider((Container)(object)p4, "Size when far", _s.IconScaleFar, 10, 100, "%", w);
			Slider((Container)(object)p4, "Smallest at", _s.IconScaleDistance, 20, 2000, "m", w, "Lower this for a much more pronounced size difference.");
			FlowPanel p5 = Section((Container)val, "Position", w);
			Check((Container)(object)p5, "Drag mode", _s.DragMode, w, "While enabled the compass can be dragged with the mouse and will capture clicks.");
			Button((Container)(object)p5, "Reset compass position", w, delegate
			{
				_s.PositionX.set_Value(-1);
				_s.PositionY.set_Value(-1);
			});
			FlowPanel p6 = Section((Container)val, "Radial mode (experimental)", w);
			Check((Container)(object)p6, "Ring at the character's feet", _s.RadialMode, w, "Replaces the bar with a ring drawn on the ground around your character.");
			Slider((Container)(object)p6, "Ring radius", _s.RadialRadius, 2, 60, "m", w);
			Slider((Container)(object)p6, "Vertical offset", _s.RadialVerticalOffset, -3, 5, "m", w, "Raise the ring if it sinks into sloped terrain.");
			Slider((Container)(object)p6, "Icon size", _s.RadialIconSize, 12, 64, "px", w);
			Slider((Container)(object)p6, "Icon opacity", _s.RadialIconOpacity, 10, 100, "%", w);
			FlowPanel p7 = Section((Container)val, "Advanced", w);
			Check((Container)(object)p7, "Match game field of view", _s.MatchGameFov, w, "Keeps markers aligned with what you see. Turn off to set the span manually.");
			Slider((Container)(object)p7, "Manual field of view", _s.FovDegrees, 60, 360, "°", w, "Only used when 'Match game field of view' is off.");
			Slider((Container)(object)p7, "Motion smoothing", _s.Smoothing, 0, 95, "%", w, "Softens stutter caused by the overlay running at its own framerate.");
			FlowPanel p8 = Section((Container)val, "Developer", w);
			Check((Container)(object)p8, "Debug overlay", _s.Debug, w, "Prints coordinates onto the bar.");
			Check((Container)(object)p8, "Project POIs onto the map", _s.MapOverlayDebug, w, "Draws dots on the open world map.");
		}

		private static FlowPanel Section(Container parent, string title, int w)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(parent);
			((Panel)val).set_Title(title);
			((Control)val).set_Width(w);
			((Panel)val).set_ShowBorder(true);
			((Panel)val).set_CanCollapse(true);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 3f));
			val.set_OuterControlPadding(new Vector2(6f, 4f));
			((Container)val).set_WidthSizingMode((SizingMode)0);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			return val;
		}

		private static void Check(Container p, string text, SettingEntry<bool> setting, int w, string tooltip = null)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			Checkbox val = new Checkbox();
			((Control)val).set_Parent(p);
			val.set_Text(text);
			val.set_Checked(setting.get_Value());
			((Control)val).set_Width(w - 24);
			((Control)val).set_Height(24);
			((Control)val).set_BasicTooltipText(tooltip);
			val.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object o, CheckChangedEvent e)
			{
				setting.set_Value(e.get_Checked());
			});
		}

		private static void Slider(Container p, string text, SettingEntry<int> setting, int min, int max, string unit, int w, string tooltip = null)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			int rowW = w - 24;
			Panel val = new Panel();
			((Control)val).set_Parent(p);
			((Control)val).set_Width(rowW);
			((Control)val).set_Height(24);
			((Control)val).set_BasicTooltipText(tooltip);
			Panel row = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)row);
			val2.set_Text(text);
			((Control)val2).set_Location(new Point(0, 3));
			((Control)val2).set_Width(190);
			((Control)val2).set_Height(18);
			((Control)val2).set_BasicTooltipText(tooltip);
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)row);
			((Control)val3).set_Location(new Point(rowW - 62, 3));
			((Control)val3).set_Width(62);
			((Control)val3).set_Height(18);
			val3.set_Text(Format(setting.get_Value(), unit));
			val3.set_HorizontalAlignment((HorizontalAlignment)2);
			val3.set_TextColor(new Color(255, 220, 140));
			Label value = val3;
			TrackBar val4 = new TrackBar();
			((Control)val4).set_Parent((Container)(object)row);
			((Control)val4).set_Location(new Point(196, 4));
			((Control)val4).set_Width(Math.Max(60, rowW - 190 - 62 - 14));
			val4.set_MinValue((float)min);
			val4.set_MaxValue((float)max);
			val4.set_Value((float)setting.get_Value());
			val4.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object o, ValueEventArgs<float> e)
			{
				int value2 = (int)Math.Round(e.get_Value());
				setting.set_Value(value2);
				value.set_Text(Format(value2, unit));
			});
		}

		private static void Keybind(Container p, string text, SettingEntry<KeyBinding> setting, int w)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			int rowW = w - 24;
			Panel val = new Panel();
			((Control)val).set_Parent(p);
			((Control)val).set_Width(rowW);
			((Control)val).set_Height(28);
			Panel row = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)row);
			val2.set_Text(text);
			((Control)val2).set_Location(new Point(0, 5));
			((Control)val2).set_Width(190);
			((Control)val2).set_Height(18);
			KeybindingAssigner val3 = new KeybindingAssigner(setting.get_Value());
			((Control)val3).set_Parent((Container)(object)row);
			((Control)val3).set_Location(new Point(196, 2));
			((Control)val3).set_Width(Math.Max(100, rowW - 190 - 10));
		}

		private static void Button(Container p, string text, int w, Action onClick)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			int rowW = w - 24;
			Panel val = new Panel();
			((Control)val).set_Parent(p);
			((Control)val).set_Width(rowW);
			((Control)val).set_Height(34);
			Panel row = val;
			int btnW = Math.Min(rowW, Math.Max(180, text.Length * 9 + 30));
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)row);
			val2.set_Text(text);
			((Control)val2).set_Width(btnW);
			((Control)val2).set_Height(28);
			((Control)val2).set_Location(new Point(0, 3));
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				onClick();
			});
		}

		private static string Format(int value, string unit)
		{
			if (!string.IsNullOrEmpty(unit))
			{
				return $"{value} {unit}";
			}
			return value.ToString();
		}
	}
}
