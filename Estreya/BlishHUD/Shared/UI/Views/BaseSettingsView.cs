using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.State;
using Gw2Sharp.WebApi.V2.Models;
using Humanizer;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.Shared.UI.Views
{
	public abstract class BaseSettingsView : BaseView
	{
		private readonly int CONTROL_WIDTH;

		private readonly Point CONTROL_LOCATION;

		private static readonly Logger Logger = Logger.GetLogger<BaseSettingsView>();

		private readonly SettingEventState _settingEventState;

		protected BaseSettingsView(Gw2ApiManager apiManager, IconState iconState, TranslationState translationState, SettingEventState settingEventState, BitmapFont font = null)
			: base(apiManager, iconState, translationState, font)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			base.LABEL_WIDTH = 250;
			CONTROL_WIDTH = 250;
			CONTROL_LOCATION = new Point(base.LABEL_WIDTH + 20, 0);
			_settingEventState = settingEventState;
		}

		protected sealed override void InternalBuild(Panel parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			Rectangle bounds = ((Container)parent).get_ContentRegion();
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(((Rectangle)(ref bounds)).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(5f, 2f));
			val.set_OuterControlPadding(new Vector2(20f, 15f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Container)val).set_AutoSizePadding(new Point(0, 15));
			((Control)val).set_Parent((Container)(object)parent);
			FlowPanel parentPanel = val;
			BuildView(parentPanel);
		}

		protected abstract void BuildView(FlowPanel parent);

		protected (Panel Panel, Label label, ColorBox colorBox) RenderColorSetting(Panel parent, SettingEntry<Color> settingEntry)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			(Label, Label) label = RenderLabel(panel, ((SettingEntry)settingEntry).get_DisplayName());
			ColorBox colorBox = RenderColorBox(panel, CONTROL_LOCATION, settingEntry.get_Value(), delegate(Color color)
			{
				settingEntry.set_Value(color);
			}, base.MainPanel);
			((Control)colorBox).set_BasicTooltipText(((SettingEntry)settingEntry).get_Description());
			return (panel, label.Item1, colorBox);
		}

		protected (Panel Panel, Label label, TextBox textBox) RenderTextSetting(Panel parent, SettingEntry<string> settingEntry)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			(Label, Label) label = RenderLabel(panel, ((SettingEntry)settingEntry).get_DisplayName());
			TextBox textBox = RenderTextbox(panel, CONTROL_LOCATION, CONTROL_WIDTH, settingEntry.get_Value(), ((SettingEntry)settingEntry).get_Description(), delegate(string newValue)
			{
				settingEntry.set_Value(newValue);
			});
			((Control)textBox).set_BasicTooltipText(((SettingEntry)settingEntry).get_Description());
			return (panel, label.Item1, textBox);
		}

		protected (Panel Panel, Label label, TrackBar trackBar) RenderIntSetting(Panel parent, SettingEntry<int> settingEntry)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			(Label, Label) label = RenderLabel(panel, ((SettingEntry)settingEntry).get_DisplayName());
			(float, float)? range = settingEntry.GetRange<int>();
			TrackBar trackbar = RenderTrackBar(panel, CONTROL_LOCATION, CONTROL_WIDTH, settingEntry.get_Value(), new(int, int)?(((int)(range?.Item1 ?? 0f), (int)(range?.Item2 ?? 100f))), delegate(int newValue)
			{
				settingEntry.set_Value(newValue);
			});
			((Control)trackbar).set_BasicTooltipText(((SettingEntry)settingEntry).get_Description());
			_settingEventState.AddForRangeCheck((SettingEntry)(object)settingEntry);
			_settingEventState.RangeUpdated += delegate(object s, ComplianceUpdated e)
			{
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				if (e.SettingEntry.get_EntryKey() == ((SettingEntry)settingEntry).get_EntryKey())
				{
					IntRangeRangeComplianceRequisite val = (IntRangeRangeComplianceRequisite)(object)e.NewCompliance;
					trackbar.set_MinValue((float)((IntRangeRangeComplianceRequisite)(ref val)).get_MinValue());
					trackbar.set_MaxValue((float)((IntRangeRangeComplianceRequisite)(ref val)).get_MaxValue());
				}
			};
			((Control)trackbar).add_Disposed((EventHandler<EventArgs>)delegate
			{
				_settingEventState.RemoveFromRangeCheck((SettingEntry)(object)settingEntry);
			});
			return (panel, label.Item1, trackbar);
		}

		protected (Panel Panel, Label label, TrackBar trackBar) RenderFloatSetting(Panel parent, SettingEntry<float> settingEntry)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			(Label, Label) label = RenderLabel(panel, ((SettingEntry)settingEntry).get_DisplayName());
			(float, float)? range = settingEntry.GetRange<float>();
			TrackBar trackbar = RenderTrackBar(panel, CONTROL_LOCATION, CONTROL_WIDTH, settingEntry.get_Value(), range, delegate(float newValue)
			{
				settingEntry.set_Value(newValue);
			});
			((Control)trackbar).set_BasicTooltipText(((SettingEntry)settingEntry).get_Description());
			_settingEventState.AddForRangeCheck((SettingEntry)(object)settingEntry);
			_settingEventState.RangeUpdated += delegate(object s, ComplianceUpdated e)
			{
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				if (e.SettingEntry.get_EntryKey() == ((SettingEntry)settingEntry).get_EntryKey())
				{
					FloatRangeRangeComplianceRequisite val = (FloatRangeRangeComplianceRequisite)(object)e.NewCompliance;
					trackbar.set_MinValue(((FloatRangeRangeComplianceRequisite)(ref val)).get_MinValue());
					trackbar.set_MaxValue(((FloatRangeRangeComplianceRequisite)(ref val)).get_MaxValue());
				}
			};
			((Control)trackbar).add_Disposed((EventHandler<EventArgs>)delegate
			{
				_settingEventState.RemoveFromRangeCheck((SettingEntry)(object)settingEntry);
			});
			return (panel, label.Item1, trackbar);
		}

		protected (Panel Panel, Label label, Checkbox checkbox) RenderBoolSetting(Panel parent, SettingEntry<bool> settingEntry)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			(Label, Label) label = RenderLabel(panel, ((SettingEntry)settingEntry).get_DisplayName());
			Checkbox checkbox = RenderCheckbox(panel, CONTROL_LOCATION, settingEntry.get_Value(), delegate(bool newValue)
			{
				settingEntry.set_Value(newValue);
			});
			((Control)checkbox).set_BasicTooltipText(((SettingEntry)settingEntry).get_Description());
			return (panel, label.Item1, checkbox);
		}

		protected (Panel Panel, Label label, KeybindingAssigner keybindingAssigner) RenderKeybindingSetting(Panel parent, SettingEntry<KeyBinding> settingEntry)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			(Label, Label) label = RenderLabel(panel, ((SettingEntry)settingEntry).get_DisplayName());
			KeybindingAssigner keybindingAssigner = RenderKeybinding(panel, CONTROL_LOCATION, CONTROL_WIDTH, settingEntry.get_Value(), delegate(KeyBinding newValue)
			{
				settingEntry.set_Value(newValue);
				GameService.Settings.Save(false);
			});
			((Control)keybindingAssigner).set_BasicTooltipText(((SettingEntry)settingEntry).get_Description());
			return (panel, label.Item1, keybindingAssigner);
		}

		protected (Panel Panel, Label label, Dropdown dropdown) RenderEnumSetting<T>(Panel parent, SettingEntry<T> settingEntry) where T : Enum
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			(Label, Label) label = RenderLabel(panel, ((SettingEntry)settingEntry).get_DisplayName());
			LetterCasing casing = LetterCasing.Title;
			List<T> values = ((T[])Enum.GetValues(((SettingEntry)settingEntry).get_SettingType())).ToList();
			string[] formattedValues = values.Select((T value) => value.Humanize(casing)).ToArray();
			Dropdown dropdown = RenderDropdown(panel, CONTROL_LOCATION, CONTROL_WIDTH, formattedValues, settingEntry.get_Value().Humanize(casing), delegate(string newValue)
			{
				settingEntry.set_Value(values[formattedValues.ToList().IndexOf(newValue)]);
			});
			((Control)dropdown).set_BasicTooltipText(((SettingEntry)settingEntry).get_Description());
			return (panel, label.Item1, dropdown);
		}

		protected override void Unload()
		{
			base.Unload();
			((Container)base.MainPanel).get_Children()?.ToList().ForEach(delegate(Control c)
			{
				if (c != null)
				{
					c.Dispose();
				}
			});
			((Container)base.MainPanel).get_Children()?.Clear();
		}
	}
}
