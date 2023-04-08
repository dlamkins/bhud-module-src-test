using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using KpRefresher.Domain;
using KpRefresher.Services;
using Microsoft.Xna.Framework;

namespace KpRefresher.UI.Views
{
	public class KpRefresherWindow : StandardWindow
	{
		private readonly ModuleSettings _moduleSettings;

		private readonly BusinessService _businessService;

		private readonly List<StandardButton> _buttons = new List<StandardButton>();

		private static readonly Regex _regex = new Regex("^[0-9]*$");

		private LoadingSpinner _loadingSpinner;

		private Panel _notificationsContainer;

		private Label _notificationLabel;

		private FormattedLabel _notificationFormattedLabel;

		private Checkbox _showAutoRetryNotificationCheckbox;

		private Checkbox _onlyRefreshOnFinalBossKillCheckbox;

		private bool _delayTextChangeFlag;

		public KpRefresherWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, AsyncTexture2D cornerIconTexture, ModuleSettings moduleSettings, BusinessService businessService)
			: this(background, windowRegion, contentRegion)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("KillProof.me Refresher");
			((WindowBase2)this).set_Emblem(AsyncTexture2D.op_Implicit(cornerIconTexture));
			((Control)this).set_Location(new Point(300, 300));
			((WindowBase2)this).set_SavesPosition(true);
			_moduleSettings = moduleSettings;
			_businessService = businessService;
		}

		public void BuildUi()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Expected O, but got Unknown
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_034b: Expected O, but got Unknown
			//IL_0350: Expected O, but got Unknown
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_038c: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Expected O, but got Unknown
			//IL_0394: Expected O, but got Unknown
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d3: Expected O, but got Unknown
			//IL_03d8: Expected O, but got Unknown
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0401: Unknown result type (might be due to invalid IL or missing references)
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0414: Unknown result type (might be due to invalid IL or missing references)
			//IL_0417: Expected O, but got Unknown
			//IL_041c: Expected O, but got Unknown
			//IL_0435: Unknown result type (might be due to invalid IL or missing references)
			//IL_043a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_0450: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Expected O, but got Unknown
			//IL_0460: Expected O, but got Unknown
			//IL_0479: Unknown result type (might be due to invalid IL or missing references)
			//IL_047e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0489: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Expected O, but got Unknown
			//IL_0499: Expected O, but got Unknown
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Expected O, but got Unknown
			//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04de: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f9: Expected O, but got Unknown
			//IL_0511: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_052f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0539: Unknown result type (might be due to invalid IL or missing references)
			//IL_0545: Unknown result type (might be due to invalid IL or missing references)
			//IL_054c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0553: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Unknown result type (might be due to invalid IL or missing references)
			//IL_056f: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_ControlPadding(new Vector2(3f, 3f));
			FlowPanel mainContainer = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)mainContainer);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Panel)val2).set_Title("Configuration");
			((Panel)val2).set_ShowBorder(true);
			((Panel)val2).set_CanCollapse(true);
			val2.set_OuterControlPadding(new Vector2(5f));
			val2.set_ControlPadding(new Vector2(5f));
			FlowPanel configContainer = val2;
			(FlowPanel, Label, Checkbox) checkbox_controls = this.CreateLabeledControl<Checkbox>("Enable auto-retry", "Schedule automatically a new try when KillProof.me was not available for a refresh", configContainer, 2, 50);
			checkbox_controls.Item3.set_Checked(_moduleSettings.EnableAutoRetry.get_Value());
			checkbox_controls.Item3.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_moduleSettings.EnableAutoRetry.set_Value(e.get_Checked());
				((Control)_showAutoRetryNotificationCheckbox).set_Enabled(e.get_Checked());
			});
			checkbox_controls = this.CreateLabeledControl<Checkbox>("Show auto-retry notifications", "Display notification when retry is scheduled", configContainer, 2, 50);
			_showAutoRetryNotificationCheckbox = checkbox_controls.Item3;
			((Control)checkbox_controls.Item3).set_Enabled(_moduleSettings.EnableAutoRetry.get_Value());
			checkbox_controls.Item3.set_Checked(_moduleSettings.ShowScheduleNotification.get_Value());
			checkbox_controls.Item3.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_moduleSettings.ShowScheduleNotification.set_Value(e.get_Checked());
			});
			checkbox_controls = this.CreateLabeledControl<Checkbox>("Condition refresh to clear", "Only allow refresh if a clear was made and is visible by GW2 API", configContainer, 2, 50);
			checkbox_controls.Item3.set_Checked(_moduleSettings.EnableRefreshOnKill.get_Value());
			checkbox_controls.Item3.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_moduleSettings.EnableRefreshOnKill.set_Value(e.get_Checked());
				((Control)_onlyRefreshOnFinalBossKillCheckbox).set_Enabled(e.get_Checked());
			});
			checkbox_controls = this.CreateLabeledControl<Checkbox>("Refresh on final boss kill", "Only refresh if a final raid wing boss was cleared (e.g. Sabetha)", configContainer, 2, 50);
			_onlyRefreshOnFinalBossKillCheckbox = checkbox_controls.Item3;
			((Control)checkbox_controls.Item3).set_Enabled(_moduleSettings.EnableRefreshOnKill.get_Value());
			checkbox_controls.Item3.set_Checked(_moduleSettings.RefreshOnKillOnlyBoss.get_Value());
			checkbox_controls.Item3.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_moduleSettings.RefreshOnKillOnlyBoss.set_Value(e.get_Checked());
			});
			checkbox_controls = this.CreateLabeledControl<Checkbox>("Refresh on map change", "Schedule a refresh when leaving a raid or strike map", configContainer, 2, 50);
			checkbox_controls.Item3.set_Checked(_moduleSettings.RefreshOnMapChange.get_Value());
			checkbox_controls.Item3.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_moduleSettings.RefreshOnMapChange.set_Value(e.get_Checked());
			});
			TextBox control = this.CreateLabeledControl<TextBox>("Delay before refresh", "Time in minutes before refresh is triggered after map change (between 1 and 60)", configContainer, 2, 50).control;
			((TextInputBase)control).set_Text(_moduleSettings.DelayBeforeRefreshOnMapChange.get_Value().ToString());
			((TextInputBase)control).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate(object s, ValueEventArgs<bool> e)
			{
				if (string.IsNullOrWhiteSpace(((TextInputBase)((s is TextBox) ? s : null)).get_Text().Trim()))
				{
					_delayTextChangeFlag = true;
					((TextInputBase)control).set_Text("1");
					_moduleSettings.DelayBeforeRefreshOnMapChange.set_Value(1);
					_delayTextChangeFlag = false;
				}
			});
			((TextInputBase)control).add_TextChanged((EventHandler<EventArgs>)delegate(object s, EventArgs e)
			{
				if (!_delayTextChangeFlag)
				{
					_delayTextChangeFlag = true;
					string text = ((TextInputBase)((s is TextBox) ? s : null)).get_Text().Trim();
					int result;
					if (string.IsNullOrWhiteSpace(text))
					{
						_delayTextChangeFlag = false;
					}
					else if (!_regex.IsMatch(text))
					{
						((TextInputBase)control).set_Text(((ValueChangedEventArgs<string>)(object)e).get_PreviousValue());
						((TextInputBase)control).set_CursorIndex(((TextInputBase)control).get_Text().Length);
						_delayTextChangeFlag = false;
					}
					else if (!int.TryParse(text, out result))
					{
						((TextInputBase)control).set_Text(((ValueChangedEventArgs<string>)(object)e).get_PreviousValue());
						_delayTextChangeFlag = false;
					}
					else
					{
						if (result < 1)
						{
							result = 1;
							((TextInputBase)control).set_Text("1");
							((TextInputBase)control).set_CursorIndex(1);
						}
						else if (result > 60)
						{
							result = 60;
							((TextInputBase)control).set_Text("60");
							((TextInputBase)control).set_CursorIndex(2);
						}
						_moduleSettings.DelayBeforeRefreshOnMapChange.set_Value(result);
						_delayTextChangeFlag = false;
					}
				}
			});
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)mainContainer);
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Panel)val3).set_Title("Actions");
			((Panel)val3).set_ShowBorder(true);
			((Panel)val3).set_CanCollapse(true);
			val3.set_OuterControlPadding(new Vector2(5f));
			val3.set_ControlPadding(new Vector2(5f));
			FlowPanel actionContainer = val3;
			((Container)actionContainer).add_ContentResized((EventHandler<RegionChangedEventArgs>)ActionContainer_ContentResized);
			List<StandardButton> buttons = _buttons;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Refresh KillProof.me");
			((Control)val4).set_BasicTooltipText("Attempts to refresh KillProof.me\nIf auto-retry is enable, a new refresh will be scheduled in case of failure");
			((Control)val4).set_Parent((Container)(object)actionContainer);
			StandardButton button = val4;
			buttons.Add(val4);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await RefreshRaidClears();
			});
			List<StandardButton> buttons2 = _buttons;
			StandardButton val5 = new StandardButton();
			val5.set_Text("Refresh linked accounts");
			((Control)val5).set_BasicTooltipText("Attempts to refresh all linked KillProof.me accounts");
			((Control)val5).set_Parent((Container)(object)actionContainer);
			button = val5;
			buttons2.Add(val5);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				List<string> linkedKpId = _businessService.LinkedKpId;
				if (linkedKpId != null && linkedKpId.Count > 0)
				{
					string res = await _businessService.RefreshLinkedAccounts();
					KpRefresherWindow kpRefresherWindow = this;
					object arg = _businessService.LinkedKpId?.Count;
					List<string> linkedKpId2 = _businessService.LinkedKpId;
					kpRefresherWindow.ShowInsideNotification(string.Format("{0} linked account{1} found !\n{2}", arg, (linkedKpId2 != null && linkedKpId2.Count > 1) ? "s" : string.Empty, res), persistMessage: true);
				}
				else
				{
					ShowInsideNotification("No linked account found !");
				}
			});
			List<StandardButton> buttons3 = _buttons;
			StandardButton val6 = new StandardButton();
			val6.set_Text("Show clears");
			((Control)val6).set_BasicTooltipText("Displays current raid clears according to KillProof.me and GW2\n\nIf the color is green, it means the clear has been registered on KillProof.me\nIf the color is purple, it means that the clear is visible by GW2 API, and can be added to KillProof.me through refresh");
			((Control)val6).set_Parent((Container)(object)actionContainer);
			button = val6;
			buttons3.Add(val6);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await DisplayRaidDifference();
			});
			List<StandardButton> buttons4 = _buttons;
			StandardButton val7 = new StandardButton();
			val7.set_Text("Show current KP");
			((Control)val7).set_BasicTooltipText("Scan your bank, shared slots and characters and displays current KP according GW2 API.\nEvery kp in the list is able to be scanned by KillProof.me, if not already scanned. You can use this feature to check if a newly opened chest is already visible for KillProof.me.");
			((Control)val7).set_Parent((Container)(object)actionContainer);
			button = val7;
			buttons4.Add(val7);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await DisplayCurrentKp();
			});
			List<StandardButton> buttons5 = _buttons;
			StandardButton val8 = new StandardButton();
			val8.set_Text("Clear schedule");
			((Control)val8).set_BasicTooltipText("Resets any scheduled refresh");
			((Control)val8).set_Parent((Container)(object)actionContainer);
			button = val8;
			buttons5.Add(val8);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				StopRetry();
			});
			List<StandardButton> buttons6 = _buttons;
			StandardButton val9 = new StandardButton();
			val9.set_Text("Clear notifications");
			((Control)val9).set_Parent((Container)(object)actionContainer);
			button = val9;
			buttons6.Add(val9);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClearNotifications();
			});
			Panel val10 = new Panel();
			((Control)val10).set_Parent((Container)(object)mainContainer);
			((Container)val10).set_HeightSizingMode((SizingMode)2);
			((Container)val10).set_WidthSizingMode((SizingMode)2);
			_notificationsContainer = val10;
			LoadingSpinner val11 = new LoadingSpinner();
			((Control)val11).set_Parent((Container)(object)_notificationsContainer);
			((Control)val11).set_Size(new Point(29, 29));
			((Control)val11).set_Visible(false);
			_loadingSpinner = val11;
			((Control)_loadingSpinner).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				TimeSpan nextScheduledTimer = _businessService.GetNextScheduledTimer();
				int num = (int)nextScheduledTimer.TotalMinutes;
				if (num >= 1)
				{
					((Control)_loadingSpinner).set_BasicTooltipText(string.Format("Next retry in {0} minute{1}.", num, (num > 1) ? "s" : string.Empty));
				}
				else
				{
					((Control)_loadingSpinner).set_BasicTooltipText(string.Format("Next retry in {0} second{1}.", (int)nextScheduledTimer.TotalSeconds, ((int)nextScheduledTimer.TotalSeconds > 1) ? "s" : string.Empty));
				}
			});
			Label val12 = new Label();
			((Control)val12).set_Location(new Point(((Control)_loadingSpinner).get_Right() + 5, ((Control)_loadingSpinner).get_Top()));
			((Control)val12).set_Parent((Container)(object)_notificationsContainer);
			val12.set_HorizontalAlignment((HorizontalAlignment)0);
			val12.set_VerticalAlignment((VerticalAlignment)0);
			val12.set_Font(GameService.Content.get_DefaultFont18());
			val12.set_WrapText(true);
			_notificationLabel = val12;
		}

		private void ActionContainer_ContentResized(object sender, RegionChangedEventArgs e)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			List<StandardButton> buttons = _buttons;
			if (buttons == null || buttons.Count < 0)
			{
				return;
			}
			int columns = 2;
			StandardButton obj = _buttons.FirstOrDefault();
			Container obj2 = ((obj != null) ? ((Control)obj).get_Parent() : null);
			Container obj3 = ((obj2 is FlowPanel) ? obj2 : null);
			int width = ((((obj3 != null) ? new int?(obj3.get_ContentRegion().Width) : null) - (int)((FlowPanel)obj3).get_OuterControlPadding().X - (int)((FlowPanel)obj3).get_ControlPadding().X * (columns - 1)) / columns) ?? 100;
			foreach (StandardButton button in _buttons)
			{
				((Control)button).set_Width(width);
			}
		}

		public void RefreshLoadingSpinnerState()
		{
			((Control)_loadingSpinner).set_Visible(_businessService.RefreshScheduled);
		}

		private async Task RefreshRaidClears()
		{
			((Control)_loadingSpinner).set_Visible(true);
			await _businessService.RefreshKillproofMe();
			RefreshLoadingSpinnerState();
		}

		private async Task DisplayRaidDifference()
		{
			ShowInsideNotification("Loading ...", persistMessage: true);
			ShowFormattedNotification(await _businessService.GetFullRaidStatus(), persistMessage: true);
		}

		private void StopRetry()
		{
			if (_businessService.RefreshScheduled)
			{
				_businessService.CancelSchedule();
				ShowInsideNotification("Scheduled refresh disabled !");
			}
			else
			{
				ShowInsideNotification("No scheduled refresh");
			}
			((Control)_loadingSpinner).set_Visible(false);
		}

		private void ShowInsideNotification(string message, bool persistMessage = false)
		{
			ClearNotifications();
			if (string.IsNullOrWhiteSpace(message))
			{
				return;
			}
			_notificationLabel.set_Text(message);
			((Control)_notificationLabel).set_Visible(true);
			((Control)_notificationLabel).set_Width(((Control)_notificationsContainer).get_Width());
			((Control)_notificationLabel).set_Height(((Control)_notificationsContainer).get_Height());
			if (!persistMessage)
			{
				Task.Run(async delegate
				{
					await Task.Delay(4000);
					ClearNotifications();
				});
			}
		}

		private void ShowFormattedNotification(List<(string, Color?)> parts, bool persistMessage = false)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			ClearNotifications();
			if (parts == null || parts.Count == 0)
			{
				return;
			}
			FormattedLabelBuilder builder = new FormattedLabelBuilder();
			foreach (var part in parts)
			{
				builder = ((!part.Item2.HasValue) ? builder.CreatePart(part.Item1, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					b.SetFontSize((FontSize)18);
				}) : ((!(part.Item2.Value == Colors.OnlyGw2)) ? builder.CreatePart(part.Item1, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					b.SetFontSize((FontSize)18).SetTextColor(part.Item2.Value);
				}) : builder.CreatePart(part.Item1, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					b.SetFontSize((FontSize)18).SetTextColor(part.Item2.Value).MakeBold();
				})));
			}
			FormattedLabel notificationFormattedLabel = _notificationFormattedLabel;
			if (notificationFormattedLabel != null)
			{
				((Control)notificationFormattedLabel).Dispose();
			}
			_notificationFormattedLabel = builder.SetWidth(((Control)_notificationsContainer).get_Width()).SetHeight(((Control)_notificationsContainer).get_Height()).SetHorizontalAlignment((HorizontalAlignment)0)
				.SetVerticalAlignment((VerticalAlignment)0)
				.Build();
			((Control)_notificationFormattedLabel).set_Location(new Point(((Control)_loadingSpinner).get_Right() + 5, ((Control)_loadingSpinner).get_Top()));
			((Control)_notificationFormattedLabel).set_Parent((Container)(object)_notificationsContainer);
			if (!persistMessage)
			{
				Task.Run(async delegate
				{
					await Task.Delay(4000);
					ClearNotifications();
				});
			}
		}

		private async Task DisplayCurrentKp()
		{
			ShowInsideNotification("Loading ...", persistMessage: true);
			ShowInsideNotification(await _businessService.DisplayCurrentKp(), persistMessage: true);
		}

		private void ClearNotifications()
		{
			_notificationLabel.set_Text(string.Empty);
			((Control)_notificationLabel).set_Visible(false);
			FormattedLabel notificationFormattedLabel = _notificationFormattedLabel;
			if (notificationFormattedLabel != null)
			{
				((Control)notificationFormattedLabel).Dispose();
			}
		}

		private (FlowPanel panel, Label label, T control) CreateLabeledControl<T>(string labelText, string tooltipText, FlowPanel parent, int amount = 2, int ctrlWidth = 50) where T : Control, new()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_FlowDirection((ControlFlowDirection)2);
			val.set_ControlPadding(new Vector2(5f));
			((Control)val).set_BasicTooltipText(tooltipText);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			FlowPanel panel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)panel);
			val2.set_Text(labelText);
			((Control)val2).set_Height(25);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val2).set_BasicTooltipText(tooltipText);
			Label label = val2;
			T val3 = new T();
			((Control)val3).set_Parent((Container)(object)panel);
			((Control)val3).set_BasicTooltipText(tooltipText);
			((Control)val3).set_Height(((Control)label).get_Height());
			((Control)val3).set_Width(ctrlWidth);
			T control = val3;
			((Container)panel).add_ContentResized((EventHandler<RegionChangedEventArgs>)FitToPanel);
			((Container)parent).add_ContentResized((EventHandler<RegionChangedEventArgs>)FitToParent);
			return (panel, label, control);
			void FitToPanel(object sender, RegionChangedEventArgs e)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				((Control)label).set_Width(((Container)panel).get_ContentRegion().Width - ((Control)control).get_Width() - (int)panel.get_ControlPadding().X * amount);
				((Control)panel).Invalidate();
			}
			void FitToParent(object sender, RegionChangedEventArgs e)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				int width = (((Container)parent).get_ContentRegion().Width - (int)(parent.get_ControlPadding().X * (float)(amount - 1))) / amount;
				((Control)panel).set_Width(width);
				((Control)panel).Invalidate();
			}
		}
	}
}
