using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Taskmaster.Models;
using Taskmaster.Services;
using Taskmaster.Settings;
using Taskmaster.UI.Controls;

namespace Taskmaster.UI
{
	public class TaskmasterWindow : StandardWindow
	{
		private const int WindowWidth = 550;

		private const int WindowHeight = 560;

		private const int MinWindowWidth = 550;

		private const int MinWindowHeight = 360;

		private const int ListTop = 36;

		private const int ActionBarHeight = 40;

		private const int ActionBarPadding = 6;

		private const int ActionControlGap = 4;

		private const int ListActionBarGap = 4;

		private static readonly TimeSpan WindowFadeSettleDelay = TimeSpan.FromMilliseconds(250.0);

		private readonly TaskStore _store;

		private readonly ModuleSettings _settings;

		private readonly TabStrip _tabStrip;

		private readonly TaskListPanel _listPanel;

		private readonly IconButton _hideDoneBtn;

		private readonly IconButton _lockBtn;

		private readonly IconButton _checkAllBtn;

		private readonly Panel _actionBar;

		private readonly Panel _actionBarSeparator;

		private readonly IconButton _addPresetBtn;

		private readonly StandardButton _addTaskBtn;

		private readonly Label _emptyLabel;

		private TextBox _renameBox;

		private Guid? _renamingTabId;

		private Guid? _activeTabId;

		private TaskmasterSizing _sizing;

		private bool _hideDone;

		private bool _locked;

		private DateTime? _applyUnfocusedOpacityAtUtc;

		private bool _isConstructed;

		private TodoTab ActiveTab
		{
			get
			{
				List<TodoTab> visible = (from t in _store.Tabs
					where !ShouldHideTab(t)
					orderby t.Order
					select t).ToList();
				TodoTab match = visible.FirstOrDefault(delegate(TodoTab t)
				{
					Guid id2 = t.Id;
					Guid? activeTabId2 = _activeTabId;
					return id2 == activeTabId2;
				});
				if (match != null)
				{
					return match;
				}
				TodoTab wasActive = _store.Tabs.FirstOrDefault(delegate(TodoTab t)
				{
					Guid id = t.Id;
					Guid? activeTabId = _activeTabId;
					return id == activeTabId;
				});
				if (wasActive != null)
				{
					List<TodoTab> allOrdered = _store.Tabs.OrderBy((TodoTab t) => t.Order).ToList();
					int idx = allOrdered.IndexOf(wasActive);
					for (int j = idx + 1; j < allOrdered.Count; j++)
					{
						if (!ShouldHideTab(allOrdered[j]))
						{
							return allOrdered[j];
						}
					}
					for (int i = idx - 1; i >= 0; i--)
					{
						if (!ShouldHideTab(allOrdered[i]))
						{
							return allOrdered[i];
						}
					}
				}
				return visible.FirstOrDefault();
			}
		}

		public TaskmasterWindow(TaskStore store, ModuleSettings settings)
			: this(TaskmasterTheme.CreateWindowBackground(550, 560), new Rectangle(0, 0, 550, 560), new Rectangle(10, 30, 530, 520))
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Expected O, but got Unknown
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Expected O, but got Unknown
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Expected O, but got Unknown
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Expected O, but got Unknown
			_store = store;
			_settings = settings;
			_sizing = new TaskmasterSizing(_settings.InterfaceScale.get_Value(), _settings.TextScale.get_Value());
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("Taskmaster");
			((WindowBase2)this).set_Emblem(Module.Instance.ContentsManager.GetTexture("taskmaster-emblem.png"));
			((WindowBase2)this).set_Id("Taskmaster_MainWindow");
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_SavesSize(true);
			((WindowBase2)this).set_CanResize(true);
			((WindowBase2)this).set_CanCloseWithEscape(false);
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_BackgroundColor(TaskmasterTheme.ActionBarFill);
			_actionBar = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_actionBar);
			((Control)val2).set_Height(1);
			((Control)val2).set_BackgroundColor(TaskmasterTheme.SubtleBorder);
			_actionBarSeparator = val2;
			IconButton iconButton = new IconButton(TaskmasterIcons.Eye, TaskmasterTheme.IconGlyph);
			((Control)iconButton).set_Parent((Container)(object)_actionBar);
			iconButton.Selected = _hideDone;
			((Control)iconButton).set_BasicTooltipText("Hide completed tasks and fully completed tabs");
			_hideDoneBtn = iconButton;
			IconButton iconButton2 = new IconButton(TaskmasterIcons.Lock, TaskmasterTheme.IconGlyph);
			((Control)iconButton2).set_Parent((Container)(object)_actionBar);
			iconButton2.Selected = _locked;
			((Control)iconButton2).set_BasicTooltipText("Lock tasks (checking still works)");
			_lockBtn = iconButton2;
			IconButton iconButton3 = new IconButton(TaskmasterIcons.Check, TaskmasterTheme.IconGlyph);
			((Control)iconButton3).set_Parent((Container)(object)_actionBar);
			((Control)iconButton3).set_BasicTooltipText("Check all tasks in this tab");
			_checkAllBtn = iconButton3;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)_actionBar);
			val3.set_Text("+  Add task");
			_addTaskBtn = val3;
			IconButton iconButton4 = new IconButton(TaskmasterIcons.ChevronDown, TaskmasterTheme.IconGlyph);
			((Control)iconButton4).set_Parent((Container)(object)_actionBar);
			((Control)iconButton4).set_BasicTooltipText("Add preset");
			_addPresetBtn = iconButton4;
			UpdateAddTaskButtonState();
			TabStrip tabStrip = new TabStrip();
			((Control)tabStrip).set_Parent((Container)(object)this);
			tabStrip.Locked = _locked;
			tabStrip.Sizing = _sizing;
			_tabStrip = tabStrip;
			TaskListPanel taskListPanel = new TaskListPanel();
			((Control)taskListPanel).set_Parent((Container)(object)this);
			taskListPanel.HideDone = _hideDone;
			taskListPanel.Locked = _locked;
			taskListPanel.DragReorderingEnabled = _settings.EnableDragReordering.get_Value();
			taskListPanel.Sizing = _sizing;
			_listPanel = taskListPanel;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Create your first tab with the + in the top right corner");
			val4.set_TextColor(TaskmasterTheme.DimText);
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			val4.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val4).set_Visible(false);
			_emptyLabel = val4;
			ApplySizing();
			RelayoutChildren();
			WireEvents();
			SelectInitialTab();
			RefreshAll();
			_isConstructed = true;
		}

		private void RelayoutChildren()
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			int listTop = ((Control)_tabStrip).get_Height() + _sizing.Px(4);
			int actionBarHeight = _sizing.Px(40);
			int listActionBarGap = _sizing.Px(4);
			((Control)_tabStrip).set_Location(Point.get_Zero());
			((Control)_tabStrip).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)_actionBar).set_Location(new Point(0, Math.Max(0, ((Container)this).get_ContentRegion().Height - actionBarHeight)));
			((Control)_actionBar).set_Size(new Point(((Container)this).get_ContentRegion().Width, actionBarHeight));
			((Control)_actionBarSeparator).set_Width(((Control)_actionBar).get_Width());
			RelayoutActionBarControls();
			((Control)_listPanel).set_Location(new Point(0, listTop));
			((Control)_listPanel).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)_listPanel).set_Height(Math.Max(0, ((Control)_actionBar).get_Top() - listTop - listActionBarGap));
			_listPanel.Rebuild();
			((Control)_emptyLabel).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)_emptyLabel).set_Location(new Point(0, listTop + ((Control)_listPanel).get_Height() / 2 - ((Control)_emptyLabel).get_Height() / 2));
			RelayoutTabRenameBox();
		}

		private void RelayoutActionBarControls()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			int padding = _sizing.Px(6);
			int controlY = (((Control)_actionBar).get_Height() - ((Control)_hideDoneBtn).get_Height()) / 2;
			((Control)_hideDoneBtn).set_Location(new Point(padding, controlY));
			((Control)_lockBtn).set_Location(new Point(((Control)_hideDoneBtn).get_Right() + _sizing.Px(4), controlY));
			((Control)_checkAllBtn).set_Location(new Point(((Control)_lockBtn).get_Right() + _sizing.Px(4), controlY));
			((Control)_addTaskBtn).set_Location(new Point(Math.Max(padding, ((Control)_actionBar).get_Width() - ((Control)_addTaskBtn).get_Width() - padding), (((Control)_actionBar).get_Height() - ((Control)_addTaskBtn).get_Height()) / 2));
			((Control)_addPresetBtn).set_Location(new Point(Math.Max(padding, ((Control)_addTaskBtn).get_Left() - _sizing.Px(4) - ((Control)_addPresetBtn).get_Width()), (((Control)_actionBar).get_Height() - ((Control)_addPresetBtn).get_Height()) / 2));
		}

		private void RelayoutTabRenameBox()
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			if (_renameBox != null && _renamingTabId.HasValue)
			{
				if (!_tabStrip.TryGetTabEditBounds(_renamingTabId.Value, out var tabBounds))
				{
					((Control)_renameBox).set_Visible(false);
					return;
				}
				((Control)_renameBox).set_Visible(true);
				((Control)_renameBox).set_Location(new Point(((Control)_tabStrip).get_Left() + tabBounds.X, ((Control)_tabStrip).get_Top() + tabBounds.Y));
				((Control)_renameBox).set_Size(new Point(tabBounds.Width, tabBounds.Height));
			}
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).OnResized(e);
			if (_isConstructed)
			{
				int correctedWidth = Math.Max(_sizing.Px(550), ((Control)this).get_Width());
				int correctedHeight = Math.Max(_sizing.Px(360), ((Control)this).get_Height());
				if (correctedWidth != ((Control)this).get_Width() || correctedHeight != ((Control)this).get_Height())
				{
					((Control)this).set_Size(new Point(correctedWidth, correctedHeight));
				}
				else
				{
					RelayoutChildren();
				}
			}
		}

		private void WireEvents()
		{
			((SettingEntry)_settings.UnfocusedOpacity).add_PropertyChanged((PropertyChangedEventHandler)delegate(object s, PropertyChangedEventArgs e)
			{
				if (e.PropertyName == "Value" && !((Control)this).get_MouseOver())
				{
					((Control)this).set_Opacity(_settings.UnfocusedOpacity.get_Value());
				}
			});
			((SettingEntry)_settings.InterfaceScale).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				ApplySizingAndRelayout();
			});
			((SettingEntry)_settings.TextScale).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				ApplySizingAndRelayout();
			});
			((SettingEntry)_settings.EnableDragReordering).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				_listPanel.DragReorderingEnabled = _settings.EnableDragReordering.get_Value();
			});
			((Control)_hideDoneBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_hideDone = !_hideDone;
				_hideDoneBtn.Selected = _hideDone;
				_listPanel.HideDone = _hideDone;
				RefreshAll();
			});
			((Control)_lockBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_locked = !_locked;
				_lockBtn.Selected = _locked;
				_listPanel.Locked = _locked;
				_tabStrip.Locked = _locked;
				UpdateAddTaskButtonState();
			});
			((Control)_addTaskBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				AddTaskToActiveTab();
			});
			((Control)_addPresetBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowPresetMenu();
			});
			((Control)_checkAllBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ToggleAllTasksInActiveTab();
			});
			_tabStrip.TabClicked += delegate(TodoTab tab)
			{
				_activeTabId = tab.Id;
				RefreshAll();
			};
			_tabStrip.AddClicked += AddTab;
			_tabStrip.TabRightClicked += ShowTabMenu;
			_tabStrip.TabReordered += delegate(TodoTab tab, int newIndex)
			{
				_store.Tabs.Remove(tab);
				_store.Tabs.Insert(Math.Min(newIndex, _store.Tabs.Count), tab);
				for (int i = 0; i < _store.Tabs.Count; i++)
				{
					_store.Tabs[i].Order = i;
				}
				MarkDirtyAndRefresh();
			};
			_listPanel.DataChanged += MarkDirtyAndRefresh;
			_listPanel.TaskContextMenuRequested += ShowTaskMenu;
			_listPanel.CopyToClipboardRequested += delegate(TodoTask task)
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(TaskPresetService.ResolveClipboardContent(task, DateTime.UtcNow));
			};
		}

		private bool ShouldHideTab(TodoTab tab)
		{
			if (_hideDone && tab.TotalCount > 0)
			{
				return tab.DoneCount == tab.TotalCount;
			}
			return false;
		}

		private void SelectInitialTab()
		{
			_activeTabId = _store.Tabs.OrderBy((TodoTab t) => t.Order).FirstOrDefault()?.Id;
		}

		private void MarkDirtyAndRefresh()
		{
			_store.MarkDirty(DateTime.UtcNow);
			RefreshAll();
		}

		private void RefreshAll()
		{
			List<TodoTab> visibleTabs = (from t in _store.Tabs
				where !ShouldHideTab(t)
				orderby t.Order
				select t).ToList();
			_tabStrip.SetTabs(visibleTabs);
			TodoTab active = ActiveTab;
			_activeTabId = active?.Id;
			_tabStrip.ActiveTabId = _activeTabId;
			_listPanel.ShowTab(active);
			((Control)_emptyLabel).set_Visible(visibleTabs.Count == 0);
			_emptyLabel.set_Text((_store.Tabs.Count == 0) ? "Create your first tab with the + in the top right corner" : "All tabs are complete - toggle the eye to show them");
			((Control)_listPanel).set_Visible(active != null);
			UpdateAddTaskButtonState();
			RelayoutTabRenameBox();
		}

		public void OnMinuteTick(DateTime nowUtc)
		{
			if (ResetEngine.ApplyResets(_store.Tabs, nowUtc) > 0)
			{
				_store.MarkDirty(nowUtc);
				RefreshAll();
			}
			else
			{
				_listPanel.RefreshCountdowns(nowUtc);
				((Control)_tabStrip).Invalidate();
			}
		}

		private void AddTab()
		{
			if (!_locked)
			{
				TodoTab tab = new TodoTab
				{
					Name = "New tab",
					Order = _store.Tabs.Count
				};
				_store.Tabs.Add(tab);
				_activeTabId = tab.Id;
				MarkDirtyAndRefresh();
				BeginRenameTab(tab, isNew: true);
			}
		}

		private void UpdateAddTaskButtonState()
		{
			TodoTab activeTab = ActiveTab;
			bool hasActiveTab = activeTab != null;
			bool hasTasks = activeTab != null && activeTab.Tasks.Count > 0;
			bool allTasksComplete = hasTasks && activeTab.Tasks.All((TodoTask task) => task.IsDone);
			((Control)_addTaskBtn).set_Enabled(!_locked && hasActiveTab);
			((Control)_addPresetBtn).set_Enabled(!_locked && hasActiveTab);
			((Control)_checkAllBtn).set_Enabled(hasTasks);
			_checkAllBtn.Selected = allTasksComplete;
			((Control)_addTaskBtn).set_BasicTooltipText(_locked ? "Locked - unlock to add tasks" : (hasActiveTab ? null : "Add a tab first"));
			((Control)_addPresetBtn).set_BasicTooltipText(_locked ? "Locked - unlock to add presets" : (hasActiveTab ? "Add preset" : "Add a tab first"));
			((Control)_checkAllBtn).set_BasicTooltipText((!hasActiveTab) ? "Add a tab first" : ((!hasTasks) ? "Add a task first" : (allTasksComplete ? "Uncheck all tasks in this tab" : "Check all tasks in this tab")));
		}

		private void ToggleAllTasksInActiveTab()
		{
			TodoTab tab = ActiveTab;
			if (tab == null || tab.Tasks.Count == 0)
			{
				return;
			}
			DateTime nowUtc = DateTime.UtcNow;
			bool allTasksComplete = tab.Tasks.All((TodoTask task) => task.IsDone);
			foreach (TodoTask task2 in tab.Tasks)
			{
				if (allTasksComplete)
				{
					task2.UncheckAll();
				}
				else if (!task2.IsDone)
				{
					task2.CompleteAll(nowUtc);
				}
				task2.SyncGroupAnchor(nowUtc);
			}
			MarkDirtyAndRefresh();
		}

		private void AddTaskToActiveTab()
		{
			if (!_locked)
			{
				TodoTab tab = ActiveTab;
				if (tab == null)
				{
					ScreenNotification.ShowNotification("Please add a tab first", (NotificationType)0, (Texture2D)null, 4);
					return;
				}
				TodoTask t = new TodoTask
				{
					Name = "New task",
					Order = tab.Tasks.Count
				};
				tab.Tasks.Add(t);
				_listPanel.AddNewTask(t);
				MarkDirtyAndRefresh();
			}
		}

		private void ShowPresetMenu()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			TodoTab tab = ActiveTab;
			if (!_locked && tab != null)
			{
				ContextMenuStrip val = new ContextMenuStrip();
				bool alreadyAdded = TaskPresetService.ContainsPreset(tab, TaskPresetType.PactSupplyNetworkAgents);
				ContextMenuStripItem obj = val.AddMenuItem(alreadyAdded ? "Pact Supply Network Agents (already added)" : "Pact Supply Network Agents");
				((Control)obj).set_Enabled(!alreadyAdded);
				((Control)obj).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					AddPsnaPreset(tab);
				});
				val.Show(GameService.Input.get_Mouse().get_Position());
			}
		}

		private void AddPsnaPreset(TodoTab tab)
		{
			if (!_locked && tab != null && !TaskPresetService.ContainsPreset(tab, TaskPresetType.PactSupplyNetworkAgents))
			{
				TodoTask preset = TaskPresetService.CreatePsna(tab.Tasks.Count);
				tab.Tasks.Add(preset);
				_activeTabId = tab.Id;
				MarkDirtyAndRefresh();
				_listPanel.ExpandTask(preset);
			}
		}

		private void BeginRenameTab(TodoTab tab, bool isNew = false)
		{
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			TextBox renameBox = _renameBox;
			if (renameBox != null)
			{
				((Control)renameBox).Dispose();
			}
			if (_activeTabId != tab.Id)
			{
				_activeTabId = tab.Id;
				RefreshAll();
			}
			_renamingTabId = tab.Id;
			_tabStrip.EditingTabId = tab.Id;
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Height(_sizing.Px(26));
			((TextInputBase)val).set_Font(_sizing.BodyFont);
			((TextInputBase)val).set_Text(isNew ? "" : tab.Name);
			((TextInputBase)val).set_PlaceholderText(isNew ? tab.Name : null);
			_renameBox = val;
			RelayoutTabRenameBox();
			_renameBox.add_EnterPressed((EventHandler<EventArgs>)delegate
			{
				Commit();
			});
			((TextInputBase)_renameBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate
			{
				if (!((TextInputBase)_renameBox).get_Focused())
				{
					Commit();
				}
			});
			((TextInputBase)_renameBox).set_Focused(true);
			void Commit()
			{
				if (_renameBox != null)
				{
					if (!string.IsNullOrWhiteSpace(((TextInputBase)_renameBox).get_Text()))
					{
						tab.Name = ((TextInputBase)_renameBox).get_Text().Trim();
					}
					((Control)_renameBox).Dispose();
					_renameBox = null;
					_renamingTabId = null;
					_tabStrip.EditingTabId = null;
					UpdateAddTaskButtonState();
					MarkDirtyAndRefresh();
				}
			}
		}

		private void ApplySizingAndRelayout()
		{
			ApplySizing();
			RelayoutChildren();
		}

		private void ApplySizing()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			_sizing = new TaskmasterSizing(_settings.InterfaceScale.get_Value(), _settings.TextScale.get_Value());
			int compactButtonHeight = _sizing.Px(26);
			int iconButtonWidth = _sizing.Px(30);
			int glyphSize = _sizing.Px(16);
			((Control)_hideDoneBtn).set_Size(new Point(iconButtonWidth, compactButtonHeight));
			_hideDoneBtn.GlyphSize = glyphSize;
			((Control)_lockBtn).set_Size(new Point(iconButtonWidth, compactButtonHeight));
			_lockBtn.GlyphSize = glyphSize;
			((Control)_checkAllBtn).set_Size(new Point(iconButtonWidth, compactButtonHeight));
			_checkAllBtn.GlyphSize = glyphSize;
			((Control)_addTaskBtn).set_Height(compactButtonHeight);
			int addTaskTextWidth = (int)Math.Max(GameService.Content.get_DefaultFont14().MeasureString(_addTaskBtn.get_Text()).Width, _sizing.BodyFont.MeasureString(_addTaskBtn.get_Text()).Width);
			((Control)_addTaskBtn).set_Width(addTaskTextWidth + _sizing.Px(24));
			((Control)_addPresetBtn).set_Size(new Point(_sizing.Px(28), compactButtonHeight));
			_addPresetBtn.GlyphSize = _sizing.Px(14);
			_tabStrip.Sizing = _sizing;
			_listPanel.Sizing = _sizing;
			((Control)_emptyLabel).set_Height(_sizing.Px(28));
			_emptyLabel.set_Font(_sizing.HeadingFont);
			if (_renameBox != null)
			{
				((Control)_renameBox).set_Height(compactButtonHeight);
				((TextInputBase)_renameBox).set_Font(_sizing.BodyFont);
			}
			int minWidth = _sizing.Px(550);
			int minHeight = _sizing.Px(360);
			if (((Control)this).get_Width() < minWidth || ((Control)this).get_Height() < minHeight)
			{
				((Control)this).set_Size(new Point(Math.Max(((Control)this).get_Width(), minWidth), Math.Max(((Control)this).get_Height(), minHeight)));
			}
		}

		private void ShowTabMenu(TodoTab tab)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			ContextMenuStrip menu = new ContextMenuStrip();
			((Control)menu.AddMenuItem("Rename")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				BeginRenameTab(tab);
			});
			ContextMenuStripItem changeColor = menu.AddMenuItem("Change color");
			ContextMenuStrip colorMenu = new ContextMenuStrip();
			(string, Color)[] tabAccentPresets = TaskmasterTheme.TabAccentPresets;
			for (int i = 0; i < tabAccentPresets.Length; i++)
			{
				(string, Color) preset = tabAccentPresets[i];
				string hex = TaskmasterTheme.ToHex(preset.Item2);
				ContextMenuStripItem obj = colorMenu.AddMenuItem(preset.Item1);
				obj.set_CanCheck(true);
				obj.set_Checked(tab.AccentColorHex == hex);
				((Control)obj).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					tab.AccentColorHex = hex;
					MarkDirtyAndRefresh();
				});
			}
			ContextMenuStripItem obj2 = colorMenu.AddMenuItem("Default");
			obj2.set_CanCheck(true);
			obj2.set_Checked(string.IsNullOrEmpty(tab.AccentColorHex));
			((Control)obj2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				tab.AccentColorHex = null;
				MarkDirtyAndRefresh();
			});
			changeColor.set_Submenu(colorMenu);
			((Control)menu.AddMenuItem("Export to clipboard")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(TabShare.Export(tab));
				ScreenNotification.ShowNotification("Tab copied to clipboard", (NotificationType)0, (Texture2D)null, 4);
			});
			((Control)menu.AddMenuItem("Import tab from clipboard")).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				TabShareImportResult result = TabShare.TryImport(await ClipboardUtil.get_WindowsClipboardService().GetTextAsync());
				switch (result.Outcome)
				{
				case TabShareImportOutcome.Success:
					result.Tab.Order = _store.Tabs.Count;
					_store.Tabs.Add(result.Tab);
					_activeTabId = result.Tab.Id;
					MarkDirtyAndRefresh();
					ScreenNotification.ShowNotification("Imported tab \"" + result.Tab.Name + "\"", (NotificationType)0, (Texture2D)null, 4);
					break;
				case TabShareImportOutcome.VersionTooNew:
					ScreenNotification.ShowNotification("Tab export is from a newer Taskmaster version", (NotificationType)2, (Texture2D)null, 4);
					break;
				case TabShareImportOutcome.InvalidPresetData:
					ScreenNotification.ShowNotification("Tab export contains invalid or duplicate preset data", (NotificationType)2, (Texture2D)null, 4);
					break;
				default:
					ScreenNotification.ShowNotification("Not a Taskmaster tab export", (NotificationType)2, (Texture2D)null, 4);
					break;
				}
			});
			((Control)menu.AddMenuItem("Delete tab")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				ContextMenuStrip val2 = new ContextMenuStrip();
				((Control)val2.AddMenuItem($"Really delete \"{tab.Name}\" and its {tab.TotalCount} task(s)?")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					if (_activeTabId == tab.Id)
					{
						List<TodoTab> list = _store.Tabs.OrderBy((TodoTab t) => t.Order).ToList();
						int num2 = list.IndexOf(tab);
						TodoTab obj3 = ((num2 + 1 < list.Count) ? list[num2 + 1] : ((num2 > 0) ? list[num2 - 1] : null));
						_activeTabId = obj3?.Id;
					}
					_store.Tabs.Remove(tab);
					MarkDirtyAndRefresh();
				});
				val2.Show(GameService.Input.get_Mouse().get_Position());
			});
			((Control)menu.AddMenuItem("Delete other tabs")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				int num = _store.Tabs.Count - 1;
				if (num > 0)
				{
					ContextMenuStrip val = new ContextMenuStrip();
					((Control)val.AddMenuItem($"Really delete the other {num} tab(s)?")).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						_store.Tabs.RemoveAll((TodoTab t) => t.Id != tab.Id);
						_activeTabId = tab.Id;
						MarkDirtyAndRefresh();
					});
					val.Show(GameService.Input.get_Mouse().get_Position());
				}
			});
			menu.Show(GameService.Input.get_Mouse().get_Position());
		}

		private void ShowTaskMenu(TodoTask task, TodoTask parent)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Expected O, but got Unknown
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			TodoTab tab = ActiveTab;
			if (tab == null)
			{
				return;
			}
			ContextMenuStrip menu = new ContextMenuStrip();
			if (parent != null)
			{
				List<TodoTask> visibleSubtasks = (from candidate in parent.Subtasks
					where !_hideDone || !candidate.IsDone || candidate == task
					orderby candidate.Order
					select candidate).ToList();
				int subtaskIndex = visibleSubtasks.IndexOf(task);
				if (subtaskIndex > 0)
				{
					((Control)menu.AddMenuItem("Move up")).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						ReorderSubtask(parent, task, () => TaskOrdering.MoveByVisible(parent.Subtasks, task, -1, (TodoTask candidate) => !_hideDone || !candidate.IsDone));
					});
					((Control)menu.AddMenuItem("Move to top")).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						ReorderSubtask(parent, task, () => TaskOrdering.MoveToStart(parent.Subtasks, task));
					});
				}
				if (subtaskIndex >= 0 && subtaskIndex < visibleSubtasks.Count - 1)
				{
					((Control)menu.AddMenuItem("Move down")).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						ReorderSubtask(parent, task, () => TaskOrdering.MoveByVisible(parent.Subtasks, task, 1, (TodoTask candidate) => !_hideDone || !candidate.IsDone));
					});
					((Control)menu.AddMenuItem("Move to bottom")).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						ReorderSubtask(parent, task, () => TaskOrdering.MoveToEnd(parent.Subtasks, task));
					});
				}
				((Control)menu.AddMenuItem("Delete subtask")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					_listPanel.DeleteSubtask(parent, task);
				});
				menu.Show(GameService.Input.get_Mouse().get_Position());
				return;
			}
			IReadOnlyList<TodoTask> selectedTasks = _listPanel.GetSelectedTasks();
			List<TodoTask> visibleTasks = (from candidate in tab.Tasks
				where !_hideDone || !candidate.IsDone || candidate == task
				orderby candidate.Order
				select candidate).ToList();
			int taskIndex = visibleTasks.IndexOf(task);
			if (!task.IsManagedPresetParent)
			{
				((Control)menu.AddMenuItem("Edit")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					_listPanel.BeginEdit(task);
				});
				((Control)menu.AddMenuItem("Duplicate")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					TodoTask todoTask = TabShare.TryImport(TabShare.Export(new TodoTab
					{
						Name = "x",
						Tasks = { task }
					})).Tab.Tasks[0];
					todoTask.Name = task.Name + " (copy)";
					todoTask.Order = task.Order + 1;
					tab.Tasks.Insert(Math.Min(tab.Tasks.IndexOf(task) + 1, tab.Tasks.Count), todoTask);
					_listPanel.ClearSelection();
					MarkDirtyAndRefresh();
				});
			}
			if (taskIndex > 0)
			{
				((Control)menu.AddMenuItem("Move up")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					MoveTask(tab, task, -1);
				});
				((Control)menu.AddMenuItem("Move to top")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					MoveTaskToEdge(tab, task, toStart: true);
				});
			}
			if (taskIndex >= 0 && taskIndex < visibleTasks.Count - 1)
			{
				((Control)menu.AddMenuItem("Move down")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					MoveTask(tab, task, 1);
				});
				((Control)menu.AddMenuItem("Move to bottom")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					MoveTaskToEdge(tab, task, toStart: false);
				});
			}
			IReadOnlyList<TodoTask> readOnlyList2;
			if (selectedTasks.Count <= 1)
			{
				IReadOnlyList<TodoTask> readOnlyList = new List<TodoTask> { task };
				readOnlyList2 = readOnlyList;
			}
			else
			{
				readOnlyList2 = selectedTasks;
			}
			IReadOnlyList<TodoTask> tasksToMove = readOnlyList2;
			List<TodoTab> otherTabs = (from candidate in _store.Tabs
				where candidate.Id != tab.Id && TaskPresetService.CanMoveTo(tasksToMove, candidate)
				orderby candidate.Order
				select candidate).ToList();
			if (otherTabs.Count > 0)
			{
				string moveToLabel = ((tasksToMove.Count > 1) ? $"Move selected ({tasksToMove.Count}) to" : "Move to");
				ContextMenuStripItem moveTo = menu.AddMenuItem(moveToLabel);
				ContextMenuStrip moveMenu = new ContextMenuStrip();
				foreach (TodoTab other in otherTabs)
				{
					TodoTab captured = other;
					((Control)moveMenu.AddMenuItem(other.Name)).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						MoveTasksToTab(tab, captured, tasksToMove);
					});
				}
				moveTo.set_Submenu(moveMenu);
			}
			((Control)menu.AddMenuItem(task.IsManagedPresetParent ? "Remove preset" : "Delete")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				tab.Tasks.Remove(task);
				NormalizeTaskOrder(tab);
				_listPanel.ClearSelection();
				MarkDirtyAndRefresh();
			});
			if (selectedTasks.Count > 1)
			{
				((Control)menu.AddMenuItem($"Delete selected ({selectedTasks.Count})")).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					List<Guid> selectedIds = selectedTasks.Select((TodoTask selected) => selected.Id).ToList();
					tab.Tasks.RemoveAll((TodoTask candidate) => selectedIds.Contains(candidate.Id));
					NormalizeTaskOrder(tab);
					_listPanel.ClearSelection();
					MarkDirtyAndRefresh();
				});
			}
			menu.Show(GameService.Input.get_Mouse().get_Position());
		}

		private void MoveTasksToTab(TodoTab source, TodoTab destination, IReadOnlyList<TodoTask> tasks)
		{
			List<TodoTask> orderedTasks = (from candidate in tasks.Where(source.Tasks.Contains)
				orderby candidate.Order
				select candidate).ToList();
			if (orderedTasks.Count == 0)
			{
				return;
			}
			foreach (TodoTask task2 in orderedTasks)
			{
				source.Tasks.Remove(task2);
			}
			NormalizeTaskOrder(source);
			NormalizeTaskOrder(destination);
			int nextOrder = destination.Tasks.Count;
			foreach (TodoTask task in orderedTasks)
			{
				task.Order = nextOrder++;
				destination.Tasks.Add(task);
			}
			_listPanel.ClearSelection();
			MarkDirtyAndRefresh();
		}

		private void MoveTask(TodoTab tab, TodoTask task, int delta)
		{
			_listPanel.PreserveScrollPosition();
			if (TaskOrdering.MoveByVisible(tab.Tasks, task, delta, (TodoTask candidate) => !_hideDone || !candidate.IsDone))
			{
				MarkDirtyAndRefresh();
			}
		}

		private void MoveTaskToEdge(TodoTab tab, TodoTask task, bool toStart)
		{
			_listPanel.PreserveScrollPosition();
			if (toStart ? TaskOrdering.MoveToStart(tab.Tasks, task) : TaskOrdering.MoveToEnd(tab.Tasks, task))
			{
				MarkDirtyAndRefresh();
			}
		}

		private void ReorderSubtask(TodoTask parent, TodoTask subtask, Func<bool> reorder)
		{
			_listPanel.PreserveScrollPosition();
			if (reorder())
			{
				MarkDirtyAndRefresh();
			}
		}

		private static void NormalizeTaskOrder(TodoTab tab)
		{
			TaskOrdering.Normalize(tab.Tasks);
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((Control)this).OnMouseEntered(e);
			((Control)this).set_Opacity(1f);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((WindowBase2)this).OnMouseLeft(e);
			((Control)this).set_Opacity(_settings.UnfocusedOpacity.get_Value());
		}

		protected override void OnShown(EventArgs e)
		{
			((Control)this).OnShown(e);
			_applyUnfocusedOpacityAtUtc = DateTime.UtcNow + WindowFadeSettleDelay;
			OnMinuteTick(DateTime.UtcNow);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).UpdateContainer(gameTime);
			if (_applyUnfocusedOpacityAtUtc.HasValue && !(DateTime.UtcNow < _applyUnfocusedOpacityAtUtc.Value))
			{
				_applyUnfocusedOpacityAtUtc = null;
				Rectangle windowBounds = default(Rectangle);
				((Rectangle)(ref windowBounds))._002Ector(((Control)this).get_Left(), ((Control)this).get_Top(), ((Control)this).get_Width(), ((Control)this).get_Height());
				((Control)this).set_Opacity(((Rectangle)(ref windowBounds)).Contains(GameService.Input.get_Mouse().get_Position()) ? 1f : _settings.UnfocusedOpacity.get_Value());
			}
		}

		public override void Hide()
		{
			_applyUnfocusedOpacityAtUtc = null;
			((WindowBase2)this).Hide();
		}

		protected override void DisposeControl()
		{
			TextBox renameBox = _renameBox;
			if (renameBox != null)
			{
				((Control)renameBox).Dispose();
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
