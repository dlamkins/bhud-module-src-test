using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class ManageReminderTimesView : BaseView
	{
		private Estreya.BlishHUD.EventTable.Models.Event _ev;

		private List<TimeSpan> _reminderTimes = new List<TimeSpan>();

		public event EventHandler<(Estreya.BlishHUD.EventTable.Models.Event Event, List<TimeSpan> ReminderTimes)> SaveClicked;

		public event EventHandler CancelClicked;

		public ManageReminderTimesView(Estreya.BlishHUD.EventTable.Models.Event ev, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, BitmapFont font = null)
			: base(apiManager, iconService, translationService, font)
		{
			_ev = ev;
			if (_ev.ReminderTimes != null)
			{
				_reminderTimes.AddRange(_ev.ReminderTimes);
			}
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Expected O, but got Unknown
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Location(new Point(20, 20));
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width - 40);
			((Control)val).set_Height(((Container)parent).get_ContentRegion().Height - 60);
			val.set_OuterControlPadding(new Vector2(20f, 20f));
			val.set_ControlPadding(new Vector2(0f, 5f));
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			((Panel)val).set_ShowBorder(true);
			FlowPanel listPanel = val;
			RenderTimes((Panel)(object)listPanel);
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)parent);
			((Control)val2).set_Top(((Control)listPanel).get_Bottom() + 5);
			((Control)val2).set_Left(((Control)listPanel).get_Left());
			((Control)val2).set_Width(((Control)listPanel).get_Width());
			val2.set_FlowDirection((ControlFlowDirection)5);
			FlowPanel buttonGroup = val2;
			RenderButton((Panel)(object)buttonGroup, base.TranslationService.GetTranslation("manageReminderTimesView-btn-cancel", "Cancel"), delegate
			{
				this.CancelClicked?.Invoke(this, EventArgs.Empty);
			});
			RenderButton((Panel)(object)buttonGroup, base.TranslationService.GetTranslation("manageReminderTimesView-btn-save", "Save"), delegate
			{
				this.SaveClicked?.Invoke(this, (_ev, _reminderTimes));
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
			Estreya.BlishHUD.EventTable.Controls.Dropdown dropdown = new Estreya.BlishHUD.EventTable.Controls.Dropdown();
			((Control)dropdown).set_Parent((Container)(object)timeSectionPanel);
			((Control)dropdown).set_Location(new Point(0, 0));
			((Control)dropdown).set_Width(75);
			((Control)dropdown).set_BasicTooltipText("Hours");
			dropdown.PanelHeight = 400;
			Estreya.BlishHUD.EventTable.Controls.Dropdown hours = dropdown;
			foreach (string valueToAdd3 in from h in Enumerable.Range(0, 24)
				select h.ToString())
			{
				hours.Items.Add(valueToAdd3);
			}
			hours.SelectedItem = time.Hours.ToString();
			hours.ValueChanged += delegate(object s, ValueChangedEventArgs e)
			{
				try
				{
					TimeSpan timeSpan3 = new TimeSpan(int.Parse(e.get_CurrentValue()), time.Minutes, time.Seconds) { };
					int index3;
					_reminderTimes[index3] = timeSpan3;
					time = timeSpan3;
				}
				catch (Exception ex3)
				{
					ShowError(ex3.Message);
				}
			};
			Estreya.BlishHUD.EventTable.Controls.Dropdown dropdown2 = new Estreya.BlishHUD.EventTable.Controls.Dropdown();
			((Control)dropdown2).set_Parent((Container)(object)timeSectionPanel);
			((Control)dropdown2).set_Location(new Point(((Control)hours).get_Right() + 5, 0));
			((Control)dropdown2).set_Width(75);
			((Control)dropdown2).set_BasicTooltipText("Minutes");
			dropdown2.PanelHeight = 400;
			Estreya.BlishHUD.EventTable.Controls.Dropdown minutes = dropdown2;
			foreach (string valueToAdd2 in from h in Enumerable.Range(0, 60)
				select h.ToString())
			{
				minutes.Items.Add(valueToAdd2);
			}
			minutes.SelectedItem = time.Minutes.ToString();
			minutes.ValueChanged += delegate(object s, ValueChangedEventArgs e)
			{
				try
				{
					TimeSpan timeSpan2 = new TimeSpan(time.Hours, int.Parse(e.get_CurrentValue()), time.Seconds) { };
					int index2;
					_reminderTimes[index2] = timeSpan2;
					time = timeSpan2;
				}
				catch (Exception ex2)
				{
					ShowError(ex2.Message);
				}
			};
			Estreya.BlishHUD.EventTable.Controls.Dropdown dropdown3 = new Estreya.BlishHUD.EventTable.Controls.Dropdown();
			((Control)dropdown3).set_Parent((Container)(object)timeSectionPanel);
			((Control)dropdown3).set_Location(new Point(((Control)minutes).get_Right() + 5, 0));
			((Control)dropdown3).set_Width(75);
			((Control)dropdown3).set_BasicTooltipText("Seconds");
			dropdown3.PanelHeight = 400;
			Estreya.BlishHUD.EventTable.Controls.Dropdown seconds = dropdown3;
			foreach (string valueToAdd in from h in Enumerable.Range(0, 60)
				select h.ToString())
			{
				seconds.Items.Add(valueToAdd);
			}
			seconds.SelectedItem = time.Seconds.ToString();
			seconds.ValueChanged += delegate(object s, ValueChangedEventArgs e)
			{
				try
				{
					TimeSpan timeSpan = new TimeSpan(time.Hours, time.Minutes, int.Parse(e.get_CurrentValue())) { };
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
