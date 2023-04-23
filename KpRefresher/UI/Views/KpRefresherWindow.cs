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
using KpRefresher.Ressources;
using KpRefresher.Services;
using KpRefresher.UI.Controls;
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
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_045c: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0737: Unknown result type (might be due to invalid IL or missing references)
			//IL_073c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0743: Unknown result type (might be due to invalid IL or missing references)
			//IL_074a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0751: Unknown result type (might be due to invalid IL or missing references)
			//IL_075d: Expected O, but got Unknown
			//IL_075e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0763: Unknown result type (might be due to invalid IL or missing references)
			//IL_076f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0774: Unknown result type (might be due to invalid IL or missing references)
			//IL_077e: Unknown result type (might be due to invalid IL or missing references)
			//IL_078a: Expected O, but got Unknown
			//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0807: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_ControlPadding(new Vector2(3f, 3f));
			FlowPanel mainContainer = val;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)mainContainer);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			flowPanel.SetLocalizedTitle = () => strings.MainWindow_Configuration_Title;
			((Panel)flowPanel).set_ShowBorder(true);
			((Panel)flowPanel).set_CanCollapse(true);
			((FlowPanel)flowPanel).set_OuterControlPadding(new Vector2(5f));
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(5f));
			FlowPanel configContainer = flowPanel;
			(FlowPanel, Label, Checkbox) checkbox_controls = this.CreateLabeledControl<Checkbox>((Func<string>)(() => strings.MainWindow_EnableAutoRetry_Label), (Func<string>)(() => strings.MainWindow_EnableAutoRetry_Tooltip), (FlowPanel)(object)configContainer, 2, 50);
			checkbox_controls.Item3.set_Checked(_moduleSettings.EnableAutoRetry.get_Value());
			checkbox_controls.Item3.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_moduleSettings.EnableAutoRetry.set_Value(e.get_Checked());
				((Control)_showAutoRetryNotificationCheckbox).set_Enabled(e.get_Checked());
			});
			checkbox_controls = this.CreateLabeledControl<Checkbox>((Func<string>)(() => strings.MainWindow_ShowScheduleNotification_Label), (Func<string>)(() => strings.MainWindow_ShowScheduleNotification_Tooltip), (FlowPanel)(object)configContainer, 2, 50);
			_showAutoRetryNotificationCheckbox = checkbox_controls.Item3;
			((Control)checkbox_controls.Item3).set_Enabled(_moduleSettings.EnableAutoRetry.get_Value());
			checkbox_controls.Item3.set_Checked(_moduleSettings.ShowScheduleNotification.get_Value());
			checkbox_controls.Item3.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_moduleSettings.ShowScheduleNotification.set_Value(e.get_Checked());
			});
			checkbox_controls = this.CreateLabeledControl<Checkbox>((Func<string>)(() => strings.MainWindow_EnableRefreshOnKill_Label), (Func<string>)(() => strings.MainWindow_EnableRefreshOnKill_Tooltip), (FlowPanel)(object)configContainer, 2, 50);
			checkbox_controls.Item3.set_Checked(_moduleSettings.EnableRefreshOnKill.get_Value());
			checkbox_controls.Item3.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_moduleSettings.EnableRefreshOnKill.set_Value(e.get_Checked());
				((Control)_onlyRefreshOnFinalBossKillCheckbox).set_Enabled(e.get_Checked());
			});
			checkbox_controls = this.CreateLabeledControl<Checkbox>((Func<string>)(() => strings.MainWindow_RefreshOnKillOnlyBoss_Label), (Func<string>)(() => strings.MainWindow_RefreshOnKillOnlyBoss_Tooltip), (FlowPanel)(object)configContainer, 2, 50);
			_onlyRefreshOnFinalBossKillCheckbox = checkbox_controls.Item3;
			((Control)checkbox_controls.Item3).set_Enabled(_moduleSettings.EnableRefreshOnKill.get_Value());
			checkbox_controls.Item3.set_Checked(_moduleSettings.RefreshOnKillOnlyBoss.get_Value());
			checkbox_controls.Item3.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_moduleSettings.RefreshOnKillOnlyBoss.set_Value(e.get_Checked());
			});
			checkbox_controls = this.CreateLabeledControl<Checkbox>((Func<string>)(() => strings.MainWindow_RefreshOnMapChange_Label), (Func<string>)(() => strings.MainWindow_RefreshOnMapChange_Tooltip), (FlowPanel)(object)configContainer, 2, 50);
			checkbox_controls.Item3.set_Checked(_moduleSettings.RefreshOnMapChange.get_Value());
			checkbox_controls.Item3.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_moduleSettings.RefreshOnMapChange.set_Value(e.get_Checked());
			});
			TextBox control = this.CreateLabeledControl<TextBox>((Func<string>)(() => strings.MainWindow_DelayBeforeRefreshOnMapChange_Label), (Func<string>)(() => strings.MainWindow_DelayBeforeRefreshOnMapChange_Tooltip), (FlowPanel)(object)configContainer, 2, 50).control;
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
			FlowPanel flowPanel2 = new FlowPanel();
			((Control)flowPanel2).set_Parent((Container)(object)mainContainer);
			((Container)flowPanel2).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel2).set_HeightSizingMode((SizingMode)1);
			flowPanel2.SetLocalizedTitle = () => strings.MainWindow_Actions_Title;
			((Panel)flowPanel2).set_ShowBorder(true);
			((Panel)flowPanel2).set_CanCollapse(true);
			((FlowPanel)flowPanel2).set_OuterControlPadding(new Vector2(5f));
			((FlowPanel)flowPanel2).set_ControlPadding(new Vector2(5f));
			FlowPanel actionContainer = flowPanel2;
			((Container)actionContainer).add_ContentResized((EventHandler<RegionChangedEventArgs>)ActionContainer_ContentResized);
			List<StandardButton> buttons = _buttons;
			StandardButton obj = new StandardButton
			{
				SetLocalizedText = () => strings.MainWindow_Button_Refresh_Label,
				SetLocalizedTooltip = () => strings.MainWindow_Button_Refresh_Tooltip
			};
			((Control)obj).set_Parent((Container)(object)actionContainer);
			StandardButton button = (StandardButton)(object)obj;
			buttons.Add((StandardButton)(object)obj);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await RefreshRaidClears();
			});
			List<StandardButton> buttons2 = _buttons;
			StandardButton obj2 = new StandardButton
			{
				SetLocalizedText = () => strings.MainWindow_Button_RefreshLinkedAccounts_Label,
				SetLocalizedTooltip = () => strings.MainWindow_Button_RefreshLinkedAccounts_Tooltip
			};
			((Control)obj2).set_Parent((Container)(object)actionContainer);
			button = (StandardButton)(object)obj2;
			buttons2.Add((StandardButton)(object)obj2);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				List<string> linkedKpId = _businessService.LinkedKpId;
				if (linkedKpId != null && linkedKpId.Count > 0)
				{
					string res = await _businessService.RefreshLinkedAccounts();
					KpRefresherWindow kpRefresherWindow = this;
					string mainWindow_Notif_LinkedAccounts = strings.MainWindow_Notif_LinkedAccounts;
					object arg = _businessService.LinkedKpId?.Count;
					List<string> linkedKpId2 = _businessService.LinkedKpId;
					kpRefresherWindow.ShowInsideNotification(string.Format(mainWindow_Notif_LinkedAccounts, arg, (linkedKpId2 != null && linkedKpId2.Count > 1) ? "s" : string.Empty, res), persistMessage: true);
				}
				else
				{
					ShowInsideNotification(strings.MainWindow_Notif_NoLinkedAccount);
				}
			});
			List<StandardButton> buttons3 = _buttons;
			StandardButton obj3 = new StandardButton
			{
				SetLocalizedText = () => strings.MainWindow_Button_ShowClears_Label,
				SetLocalizedTooltip = () => strings.MainWindow_Button_ShowClears_Tooltip
			};
			((Control)obj3).set_Parent((Container)(object)actionContainer);
			button = (StandardButton)(object)obj3;
			buttons3.Add((StandardButton)(object)obj3);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await DisplayRaidDifference();
			});
			List<StandardButton> buttons4 = _buttons;
			StandardButton obj4 = new StandardButton
			{
				SetLocalizedText = () => strings.MainWindow_Button_ShowKP_Label,
				SetLocalizedTooltip = () => strings.MainWindow_Button_ShowKP_Tooltip
			};
			((Control)obj4).set_Parent((Container)(object)actionContainer);
			button = (StandardButton)(object)obj4;
			buttons4.Add((StandardButton)(object)obj4);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await DisplayCurrentKp();
			});
			List<StandardButton> buttons5 = _buttons;
			StandardButton obj5 = new StandardButton
			{
				SetLocalizedText = () => strings.MainWindow_Button_ClearSchedule_Label,
				SetLocalizedTooltip = () => strings.MainWindow_Button_ClearSchedule_Tooltip
			};
			((Control)obj5).set_Parent((Container)(object)actionContainer);
			button = (StandardButton)(object)obj5;
			buttons5.Add((StandardButton)(object)obj5);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				StopRetry();
			});
			List<StandardButton> buttons6 = _buttons;
			StandardButton obj6 = new StandardButton
			{
				SetLocalizedText = () => strings.MainWindow_Button_ClearNotif_Label
			};
			((Control)obj6).set_Parent((Container)(object)actionContainer);
			button = (StandardButton)(object)obj6;
			buttons6.Add((StandardButton)(object)obj6);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClearNotifications();
			});
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)mainContainer);
			((Container)val2).set_HeightSizingMode((SizingMode)2);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			val2.set_CanScroll(true);
			_notificationsContainer = val2;
			LoadingSpinner val3 = new LoadingSpinner();
			((Control)val3).set_Parent((Container)(object)_notificationsContainer);
			((Control)val3).set_Size(new Point(29, 29));
			((Control)val3).set_Visible(false);
			_loadingSpinner = val3;
			((Control)_loadingSpinner).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				TimeSpan nextScheduledTimer = _businessService.GetNextScheduledTimer();
				int num = (int)nextScheduledTimer.TotalMinutes;
				if (num >= 1)
				{
					((Control)_loadingSpinner).set_BasicTooltipText(string.Format(strings.MainWindow_Spinner_Minutes, num, (num > 1) ? "s" : string.Empty));
				}
				else
				{
					((Control)_loadingSpinner).set_BasicTooltipText(string.Format(strings.MainWindow_Spinner_Seconds, (int)nextScheduledTimer.TotalSeconds, ((int)nextScheduledTimer.TotalSeconds > 1) ? "s" : string.Empty));
				}
			});
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(((Control)_loadingSpinner).get_Right() + 5, ((Control)_loadingSpinner).get_Top()));
			((Control)val4).set_Parent((Container)(object)_notificationsContainer);
			val4.set_HorizontalAlignment((HorizontalAlignment)0);
			val4.set_VerticalAlignment((VerticalAlignment)0);
			val4.set_Font(GameService.Content.get_DefaultFont18());
			val4.set_WrapText(true);
			val4.set_AutoSizeHeight(true);
			_notificationLabel = val4;
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
			ShowInsideNotification(strings.MainWindow_Notif_Loading, persistMessage: true);
			ShowFormattedNotification(await _businessService.GetFullRaidStatus(), persistMessage: true);
		}

		private void StopRetry()
		{
			if (_businessService.RefreshScheduled)
			{
				_businessService.CancelSchedule();
				ShowInsideNotification(strings.MainWindow_Notif_ScheduleDisabled);
			}
			else
			{
				ShowInsideNotification(strings.MainWindow_Notif_NoSchedule);
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
			ShowInsideNotification(strings.MainWindow_Notif_Loading, persistMessage: true);
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

		private (FlowPanel panel, Label label, T control) CreateLabeledControl<T>(Func<string> labelText, Func<string> tooltipText, FlowPanel parent, int amount = 2, int ctrlWidth = 50) where T : Control, new()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)parent);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)2);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(5f));
			flowPanel.SetLocalizedTooltip = tooltipText;
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			FlowPanel panel = flowPanel;
			Label label2 = new Label();
			((Control)label2).set_Parent((Container)(object)panel);
			label2.SetLocalizedText = labelText;
			((Control)label2).set_Height(25);
			((Label)label2).set_VerticalAlignment((VerticalAlignment)1);
			label2.SetLocalizedTooltip = tooltipText;
			Label label = label2;
			T val = new T();
			((Control)val).set_Parent((Container)(object)panel);
			((Control)val).set_Height(((Control)label).get_Height());
			((Control)val).set_Width(ctrlWidth);
			T control = val;
			((Container)panel).add_ContentResized((EventHandler<RegionChangedEventArgs>)FitToPanel);
			((Container)parent).add_ContentResized((EventHandler<RegionChangedEventArgs>)FitToParent);
			return ((FlowPanel)(object)panel, (Label)(object)label, control);
			void FitToPanel(object sender, RegionChangedEventArgs e)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				((Control)label).set_Width(((Container)panel).get_ContentRegion().Width - ((Control)control).get_Width() - (int)((FlowPanel)panel).get_ControlPadding().X * amount);
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
