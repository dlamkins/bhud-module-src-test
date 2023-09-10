using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Services;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.UI.Views
{
	public abstract class BaseSettingsView : BaseView
	{
		private static readonly Logger Logger = Logger.GetLogger<BaseSettingsView>();

		private readonly SettingEventService _settingEventService;

		private Point CONTROL_LOCATION;

		protected int CONTROL_WIDTH;

		protected BaseSettingsView(Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, SettingEventService settingEventService)
			: base(apiManager, iconService, translationService)
		{
			base.LABEL_WIDTH = 250;
			CONTROL_WIDTH = 250;
			UpdateControlLocation();
			_settingEventService = settingEventService;
		}

		protected void UpdateControlLocation()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			CONTROL_LOCATION = new Point(base.LABEL_WIDTH + 20, 0);
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
			//IL_0060: Expected O, but got Unknown
			Rectangle bounds = ((Container)parent).get_ContentRegion();
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(((Rectangle)(ref bounds)).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(5f, 2f));
			val.set_OuterControlPadding(new Vector2(20f, 15f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Control)val).set_Parent((Container)(object)parent);
			FlowPanel parentPanel = val;
			BuildView(parentPanel);
		}

		protected abstract void BuildView(FlowPanel parent);

		protected (Panel Panel, Label label, ColorBox colorBox) RenderColorSetting(Panel parent, SettingEntry<Color> settingEntry)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			(Label, Label) label = RenderLabel(panel, ((SettingEntry)settingEntry).get_DisplayName());
			ColorBox colorBox = RenderColorBox(panel, CONTROL_LOCATION, settingEntry.get_Value(), delegate(Color color)
			{
				try
				{
					settingEntry.set_Value(color);
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}, base.MainPanel);
			((Control)colorBox).set_BasicTooltipText(((SettingEntry)settingEntry).get_Description());
			SetControlEnabledState((Control)(object)colorBox, (SettingEntry)(object)settingEntry);
			AddControlForDisabledCheck((Control)(object)colorBox, (SettingEntry)(object)settingEntry);
			return (panel, label.Item1, colorBox);
		}

		protected (Panel Panel, Label label, TextBox textBox) RenderTextSetting(Panel parent, SettingEntry<string> settingEntry, Func<string, string, Task<bool>> onBeforeChangeAction = null)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			(Label, Label) label = RenderLabel(panel, ((SettingEntry)settingEntry).get_DisplayName());
			TextBox textBox = RenderTextbox(panel, CONTROL_LOCATION, CONTROL_WIDTH, settingEntry.get_Value(), ((SettingEntry)settingEntry).get_Description(), delegate(string newValue)
			{
				try
				{
					settingEntry.set_Value(newValue);
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}, null, clearOnEnter: false, onBeforeChangeAction);
			((Control)textBox).set_BasicTooltipText(((SettingEntry)settingEntry).get_Description());
			SetControlEnabledState((Control)(object)textBox, (SettingEntry)(object)settingEntry);
			AddControlForDisabledCheck((Control)(object)textBox, (SettingEntry)(object)settingEntry);
			return (panel, label.Item1, textBox);
		}

		protected (Panel Panel, Label label, TrackBar trackBar) RenderIntSetting(Panel parent, SettingEntry<int> settingEntry)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			(Label, Label) label = RenderLabel(panel, ((SettingEntry)settingEntry).get_DisplayName());
			(float, float)? range = settingEntry.GetRange<int>();
			TrackBar trackbar = RenderTrackBar(panel, CONTROL_LOCATION, CONTROL_WIDTH, settingEntry.get_Value(), new(int, int)?(((int)(range?.Item1 ?? 0f), (int)(range?.Item2 ?? 100f))), delegate(int newValue)
			{
				try
				{
					settingEntry.set_Value(newValue);
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			});
			((Control)trackbar).set_BasicTooltipText(((SettingEntry)settingEntry).get_Description());
			_settingEventService.AddForRangeCheck((SettingEntry)(object)settingEntry);
			_settingEventService.RangeUpdated += delegate(object s, ComplianceUpdated e)
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
				_settingEventService.RemoveFromRangeCheck((SettingEntry)(object)settingEntry);
			});
			SetControlEnabledState((Control)(object)trackbar, (SettingEntry)(object)settingEntry);
			AddControlForDisabledCheck((Control)(object)trackbar, (SettingEntry)(object)settingEntry);
			return (panel, label.Item1, trackbar);
		}

		protected (Panel Panel, Label label, TrackBar trackBar) RenderFloatSetting(Panel parent, SettingEntry<float> settingEntry)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			(Label, Label) label = RenderLabel(panel, ((SettingEntry)settingEntry).get_DisplayName());
			(float, float)? range = settingEntry.GetRange<float>();
			TrackBar trackbar = RenderTrackBar(panel, CONTROL_LOCATION, CONTROL_WIDTH, settingEntry.get_Value(), range, delegate(float newValue)
			{
				try
				{
					settingEntry.set_Value(newValue);
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			});
			((Control)trackbar).set_BasicTooltipText(((SettingEntry)settingEntry).get_Description());
			_settingEventService.AddForRangeCheck((SettingEntry)(object)settingEntry);
			_settingEventService.RangeUpdated += delegate(object s, ComplianceUpdated e)
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
				_settingEventService.RemoveFromRangeCheck((SettingEntry)(object)settingEntry);
			});
			SetControlEnabledState((Control)(object)trackbar, (SettingEntry)(object)settingEntry);
			AddControlForDisabledCheck((Control)(object)trackbar, (SettingEntry)(object)settingEntry);
			return (panel, label.Item1, trackbar);
		}

		protected (Panel Panel, Label label, Checkbox checkbox) RenderBoolSetting(Panel parent, SettingEntry<bool> settingEntry, Func<bool, bool, Task<bool>> onBeforeChangeAction = null)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			(Label, Label) label = RenderLabel(panel, ((SettingEntry)settingEntry).get_DisplayName());
			Checkbox checkbox = RenderCheckbox(panel, CONTROL_LOCATION, settingEntry.get_Value(), delegate(bool newValue)
			{
				try
				{
					settingEntry.set_Value(newValue);
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}, onBeforeChangeAction);
			((Control)checkbox).set_BasicTooltipText(((SettingEntry)settingEntry).get_Description());
			SetControlEnabledState((Control)(object)checkbox, (SettingEntry)(object)settingEntry);
			AddControlForDisabledCheck((Control)(object)checkbox, (SettingEntry)(object)settingEntry);
			return (panel, label.Item1, checkbox);
		}

		protected (Panel Panel, Label label, KeybindingAssigner keybindingAssigner) RenderKeybindingSetting(Panel parent, SettingEntry<KeyBinding> settingEntry)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			(Label, Label) label = RenderLabel(panel, ((SettingEntry)settingEntry).get_DisplayName());
			KeybindingAssigner keybindingAssigner = RenderKeybinding(panel, CONTROL_LOCATION, CONTROL_WIDTH, settingEntry.get_Value(), delegate(KeyBinding newValue)
			{
				try
				{
					settingEntry.set_Value(newValue);
					GameService.Settings.Save(false);
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			});
			((Control)keybindingAssigner).set_BasicTooltipText(((SettingEntry)settingEntry).get_Description());
			SetControlEnabledState((Control)(object)keybindingAssigner, (SettingEntry)(object)settingEntry);
			AddControlForDisabledCheck((Control)(object)keybindingAssigner, (SettingEntry)(object)settingEntry);
			return (panel, label.Item1, keybindingAssigner);
		}

		protected (Panel Panel, Label label, Dropdown<string> dropdown) RenderEnumSetting<T>(Panel parent, SettingEntry<T> settingEntry) where T : struct, Enum
		{
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			(Label, Label) label = RenderLabel(panel, ((SettingEntry)settingEntry).get_DisplayName());
			List<T> values = new List<T>();
			IEnumerable<EnumInclusionComplianceRequisite<T>> requisite = from cr in SettingComplianceExtensions.GetComplianceRequisite((SettingEntry)(object)settingEntry)
				where cr is EnumInclusionComplianceRequisite<T>
				select (EnumInclusionComplianceRequisite<T>)(object)cr;
			if (requisite.Any())
			{
				values.AddRange(requisite.First().get_IncludedValues());
			}
			else
			{
				values.AddRange((T[])Enum.GetValues(((SettingEntry)settingEntry).get_SettingType()));
			}
			Dropdown<string> dropdown = RenderDropdown(panel, CONTROL_LOCATION, CONTROL_WIDTH, settingEntry.get_Value(), values.ToArray(), delegate(T newValue)
			{
				try
				{
					settingEntry.set_Value(newValue);
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			});
			((Control)dropdown).set_BasicTooltipText(((SettingEntry)settingEntry).get_Description());
			SetControlEnabledState((Control)(object)dropdown, (SettingEntry)(object)settingEntry);
			AddControlForDisabledCheck((Control)(object)dropdown, (SettingEntry)(object)settingEntry);
			return (panel, label.Item1, dropdown);
		}

		private void AddControlForDisabledCheck(Control control, SettingEntry settingEntry)
		{
			_settingEventService.AddForDisabledCheck(settingEntry);
			_settingEventService.DisabledUpdated += delegate(object s, ComplianceUpdated e)
			{
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				if (e.SettingEntry.get_EntryKey() == settingEntry.get_EntryKey())
				{
					SettingDisabledComplianceRequisite val = (SettingDisabledComplianceRequisite)(object)e.NewCompliance;
					control.set_Enabled(!((SettingDisabledComplianceRequisite)(ref val)).get_Disabled());
				}
			};
			control.add_Disposed((EventHandler<EventArgs>)delegate
			{
				_settingEventService.RemoveFromDisabledCheck(settingEntry);
			});
		}

		private void SetControlEnabledState(Control control, SettingEntry settingEntry)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			IEnumerable<SettingDisabledComplianceRequisite> compliances = (from c in SettingComplianceExtensions.GetComplianceRequisite(settingEntry)
				where c is SettingDisabledComplianceRequisite
				select c).Select((Func<IComplianceRequisite, SettingDisabledComplianceRequisite>)((IComplianceRequisite c) => (SettingDisabledComplianceRequisite)(object)c));
			if (compliances.Any())
			{
				SettingDisabledComplianceRequisite val = compliances.First();
				control.set_Enabled(!((SettingDisabledComplianceRequisite)(ref val)).get_Disabled());
			}
		}

		protected override void Unload()
		{
			base.Unload();
		}
	}
}
