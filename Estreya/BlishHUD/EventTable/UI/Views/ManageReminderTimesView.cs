using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class ManageReminderTimesView : BaseView
	{
		private Event _ev;

		private readonly bool _showKeepCustomizedQuestion;

		private readonly List<TimeSpan> _reminderTimes = new List<TimeSpan>();

		public event EventHandler<(Event Event, List<TimeSpan> ReminderTimes, bool KeepCustomized)> SaveClicked;

		public event EventHandler CancelClicked;

		public ManageReminderTimesView(Event ev, bool showKeepCustomizedQuestion, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService)
			: base(apiManager, iconService, translationService)
		{
			_ev = ev;
			_showKeepCustomizedQuestion = showKeepCustomizedQuestion;
			if (_ev.ReminderTimes != null)
			{
				_reminderTimes.AddRange(_ev.ReminderTimes);
			}
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Location(new Point(20, 20));
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width - 40);
			((Control)val).set_Height(((Container)parent).get_ContentRegion().Height - 80);
			val.set_OuterControlPadding(new Vector2(20f, 20f));
			val.set_ControlPadding(new Vector2(0f, 5f));
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			((Panel)val).set_ShowBorder(true);
			FlowPanel listPanel = val;
			RenderTimes((Panel)(object)listPanel);
			Panel keepCustomizedGroup = null;
			Checkbox keepCustomizedCheckBox = null;
			if (_showKeepCustomizedQuestion)
			{
				Panel val2 = new Panel();
				((Control)val2).set_Parent((Container)(object)parent);
				((Control)val2).set_Top(((Control)listPanel).get_Bottom() + 5);
				((Control)val2).set_Left(((Control)listPanel).get_Left());
				((Control)val2).set_Width(((Control)listPanel).get_Width());
				keepCustomizedGroup = val2;
				Label keepCustomizedLabel = RenderLabel(keepCustomizedGroup, "Keep Customized:").TitleLabel;
				keepCustomizedLabel.set_AutoSizeWidth(true);
				keepCustomizedCheckBox = RenderCheckbox(keepCustomizedGroup, new Point(((Control)keepCustomizedLabel).get_Right() + 5, ((Control)keepCustomizedLabel).get_Top()), value: false);
			}
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)parent);
			((Control)val3).set_Top(((Control)(((object)keepCustomizedGroup) ?? ((object)listPanel))).get_Bottom() + 5);
			((Control)val3).set_Left(((Control)listPanel).get_Left());
			((Control)val3).set_Width(((Control)listPanel).get_Width());
			val3.set_FlowDirection((ControlFlowDirection)5);
			FlowPanel buttonGroup = val3;
			RenderButton((Panel)(object)buttonGroup, base.TranslationService.GetTranslation("manageReminderTimesView-btn-cancel", "Cancel"), delegate
			{
				this.CancelClicked?.Invoke(this, EventArgs.Empty);
			});
			RenderButton((Panel)(object)buttonGroup, base.TranslationService.GetTranslation("manageReminderTimesView-btn-save", "Save"), delegate
			{
				EventHandler<(Event Event, List<TimeSpan> ReminderTimes, bool KeepCustomized)> saveClicked = this.SaveClicked;
				if (saveClicked != null)
				{
					ManageReminderTimesView sender = this;
					Event ev = _ev;
					List<TimeSpan> reminderTimes = _reminderTimes;
					Checkbox obj = keepCustomizedCheckBox;
					saveClicked(sender, (ev, reminderTimes, obj != null && obj.get_Checked()));
				}
			});
		}

		private void RenderTimes(Panel parent)
		{
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Expected O, but got Unknown
			((Container)parent).ClearChildren();
			Panel lastTimeSection = null;
			foreach (TimeSpan reminderTime in _reminderTimes)
			{
				lastTimeSection = AddTimeSection(parent, reminderTime, _reminderTimes.Count == 1);
			}
			int? obj;
			if (lastTimeSection == null)
			{
				obj = null;
			}
			else
			{
				Control obj2 = ((IEnumerable<Control>)((Container)lastTimeSection).get_Children()).LastOrDefault();
				obj = ((obj2 != null) ? new int?(obj2.get_Left()) : null);
			}
			int? num = obj;
			int x = num.GetValueOrDefault();
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Width(x + 120);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			Panel addButtonPanel = val;
			Button button = RenderButton(addButtonPanel, base.TranslationService.GetTranslation("manageReminderTimesView-btn-add", "Add"), delegate
			{
				_reminderTimes.Add(TimeSpan.Zero);
				RenderTimes(parent);
			});
			((Control)button).set_Left(x);
			((Control)button).set_Width(120);
			button.Icon = base.IconService.GetIcon("1444520.png");
			button.ResizeIcon = false;
		}

		private Panel AddTimeSection(Panel parent, TimeSpan time, bool disableRemove)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			Panel timeSectionPanel = val;
			Dropdown<string> dropdown = new Dropdown<string>();
			((Control)dropdown).set_Parent((Container)(object)timeSectionPanel);
			((Control)dropdown).set_Location(new Point(0, 0));
			((Control)dropdown).set_Width(75);
			((Control)dropdown).set_BasicTooltipText("Hours");
			dropdown.PanelHeight = 400;
			Dropdown<string> hours = dropdown;
			foreach (string valueToAdd3 in from h in Enumerable.Range(0, 24)
				select h.ToString())
			{
				hours.Items.Add(valueToAdd3);
			}
			hours.SelectedItem = time.Hours.ToString();
			hours.ValueChanged += delegate(object s, ValueChangedEventArgs<string> e)
			{
				try
				{
					TimeSpan timeSpan3 = new TimeSpan(int.Parse(e.get_NewValue()), time.Minutes, time.Seconds) { };
					int index3;
					_reminderTimes[index3] = timeSpan3;
					time = timeSpan3;
				}
				catch (Exception ex3)
				{
					ShowError(ex3.Message);
				}
			};
			Dropdown<string> dropdown2 = new Dropdown<string>();
			((Control)dropdown2).set_Parent((Container)(object)timeSectionPanel);
			((Control)dropdown2).set_Location(new Point(((Control)hours).get_Right() + 5, 0));
			((Control)dropdown2).set_Width(75);
			((Control)dropdown2).set_BasicTooltipText("Minutes");
			dropdown2.PanelHeight = 400;
			Dropdown<string> minutes = dropdown2;
			foreach (string valueToAdd2 in from h in Enumerable.Range(0, 60)
				select h.ToString())
			{
				minutes.Items.Add(valueToAdd2);
			}
			minutes.SelectedItem = time.Minutes.ToString();
			minutes.ValueChanged += delegate(object s, ValueChangedEventArgs<string> e)
			{
				try
				{
					TimeSpan timeSpan2 = new TimeSpan(time.Hours, int.Parse(e.get_NewValue()), time.Seconds) { };
					int index2;
					_reminderTimes[index2] = timeSpan2;
					time = timeSpan2;
				}
				catch (Exception ex2)
				{
					ShowError(ex2.Message);
				}
			};
			Dropdown<string> dropdown3 = new Dropdown<string>();
			((Control)dropdown3).set_Parent((Container)(object)timeSectionPanel);
			((Control)dropdown3).set_Location(new Point(((Control)minutes).get_Right() + 5, 0));
			((Control)dropdown3).set_Width(75);
			((Control)dropdown3).set_BasicTooltipText("Seconds");
			dropdown3.PanelHeight = 400;
			Dropdown<string> seconds = dropdown3;
			foreach (string valueToAdd in from h in Enumerable.Range(0, 60)
				select h.ToString())
			{
				seconds.Items.Add(valueToAdd);
			}
			seconds.SelectedItem = time.Seconds.ToString();
			seconds.ValueChanged += delegate(object s, ValueChangedEventArgs<string> e)
			{
				try
				{
					TimeSpan timeSpan = new TimeSpan(time.Hours, time.Minutes, int.Parse(e.get_NewValue())) { };
					int index;
					_reminderTimes[index] = timeSpan;
					time = timeSpan;
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			};
			Button button = RenderButton(timeSectionPanel, base.TranslationService.GetTranslation("manageReminderTimesView-btn-remove", "Remove"), delegate
			{
				_reminderTimes.Remove(time);
				RenderTimes(parent);
			}, () => disableRemove);
			((Control)button).set_Left(((Control)seconds).get_Right() + 10);
			((Control)button).set_Width(120);
			button.Icon = base.IconService.GetIcon("1444524.png");
			button.ResizeIcon = false;
			return timeSectionPanel;
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}

		protected override void Unload()
		{
			base.Unload();
			_ev = null;
		}
	}
}
