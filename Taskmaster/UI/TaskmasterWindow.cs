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

		private readonly TaskStore _store;

		private readonly ModuleSettings _settings;

		private readonly TabStrip _tabStrip;

		private readonly TaskListPanel _listPanel;

		private readonly IconButton _hideDoneBtn;

		private readonly IconButton _lockBtn;

		private readonly Panel _actionBar;

		private readonly Panel _actionBarSeparator;

		private readonly StandardButton _addTaskBtn;

		private readonly Label _emptyLabel;

		private TextBox _renameBox;

		private Guid? _renamingTabId;

		private Guid? _activeTabId;

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
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Expected O, but got Unknown
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Expected O, but got Unknown
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Expected O, but got Unknown
			_store = store;
			_settings = settings;
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
			((Control)iconButton).set_Width(30);
			((Control)iconButton).set_Height(26);
			iconButton.Selected = _settings.HideDone.get_Value();
			((Control)iconButton).set_BasicTooltipText("Hide completed tasks and fully completed tabs");
			_hideDoneBtn = iconButton;
			IconButton iconButton2 = new IconButton(TaskmasterIcons.Lock, TaskmasterTheme.IconGlyph);
			((Control)iconButton2).set_Parent((Container)(object)_actionBar);
			((Control)iconButton2).set_Width(30);
			((Control)iconButton2).set_Height(26);
			iconButton2.Selected = _settings.LockTasks.get_Value();
			((Control)iconButton2).set_BasicTooltipText("Lock tasks (checking still works)");
			_lockBtn = iconButton2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)_actionBar);
			((Control)val3).set_Height(26);
			val3.set_Text("+  Add task");
			_addTaskBtn = val3;
			((Control)_addTaskBtn).set_Width((int)GameService.Content.get_DefaultFont14().MeasureString(_addTaskBtn.get_Text()).Width + 24);
			UpdateAddTaskButtonState();
			TabStrip tabStrip = new TabStrip();
			((Control)tabStrip).set_Parent((Container)(object)this);
			tabStrip.Locked = _settings.LockTasks.get_Value();
			_tabStrip = tabStrip;
			TaskListPanel taskListPanel = new TaskListPanel();
			((Control)taskListPanel).set_Parent((Container)(object)this);
			taskListPanel.HideDone = _settings.HideDone.get_Value();
			taskListPanel.Locked = _settings.LockTasks.get_Value();
			_listPanel = taskListPanel;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Height(28);
			val4.set_Font(GameService.Content.get_DefaultFont16());
			val4.set_Text("Create your first tab with the + in the top right corner");
			val4.set_TextColor(TaskmasterTheme.DimText);
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			val4.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val4).set_Visible(false);
			_emptyLabel = val4;
			RelayoutChildren();
			WireEvents();
			SelectInitialTab();
			RefreshAll();
			_isConstructed = true;
		}

		private void RelayoutChildren()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			((Control)_tabStrip).set_Location(Point.get_Zero());
			((Control)_tabStrip).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)_actionBar).set_Location(new Point(0, Math.Max(0, ((Container)this).get_ContentRegion().Height - 40)));
			((Control)_actionBar).set_Size(new Point(((Container)this).get_ContentRegion().Width, 40));
			((Control)_actionBarSeparator).set_Width(((Control)_actionBar).get_Width());
			RelayoutActionBarControls();
			((Control)_listPanel).set_Location(new Point(0, 36));
			((Control)_listPanel).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)_listPanel).set_Height(Math.Max(0, ((Control)_actionBar).get_Top() - 36 - 4));
			_listPanel.Rebuild();
			((Control)_emptyLabel).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)_emptyLabel).set_Location(new Point(0, 36 + ((Control)_listPanel).get_Height() / 2 - 14));
			RelayoutTabRenameBox();
		}

		private void RelayoutActionBarControls()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			int controlY = (40 - ((Control)_hideDoneBtn).get_Height()) / 2;
			((Control)_hideDoneBtn).set_Location(new Point(6, controlY));
			((Control)_lockBtn).set_Location(new Point(((Control)_hideDoneBtn).get_Right() + 4, controlY));
			((Control)_addTaskBtn).set_Location(new Point(Math.Max(6, ((Control)_actionBar).get_Width() - ((Control)_addTaskBtn).get_Width() - 6), (40 - ((Control)_addTaskBtn).get_Height()) / 2));
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
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).OnResized(e);
			if (_isConstructed)
			{
				int correctedWidth = Math.Max(550, ((Control)this).get_Width());
				int correctedHeight = Math.Max(360, ((Control)this).get_Height());
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
			((Control)_hideDoneBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_settings.HideDone.set_Value(!_settings.HideDone.get_Value());
				_hideDoneBtn.Selected = _settings.HideDone.get_Value();
				_listPanel.HideDone = _settings.HideDone.get_Value();
				RefreshAll();
			});
			((Control)_lockBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_settings.LockTasks.set_Value(!_settings.LockTasks.get_Value());
				_lockBtn.Selected = _settings.LockTasks.get_Value();
				_listPanel.Locked = _settings.LockTasks.get_Value();
				_tabStrip.Locked = _settings.LockTasks.get_Value();
				UpdateAddTaskButtonState();
			});
			((Control)_addTaskBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				AddTaskToActiveTab();
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
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(task.ClipboardContent);
			};
		}

		private bool ShouldHideTab(TodoTab tab)
		{
			if (_settings.HideDone.get_Value() && tab.TotalCount > 0)
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
			if (!_settings.LockTasks.get_Value())
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
			bool locked = _settings.LockTasks.get_Value();
			bool hasActiveTab = ActiveTab != null;
			((Control)_addTaskBtn).set_Enabled(!locked && hasActiveTab);
			((Control)_addTaskBtn).set_BasicTooltipText(locked ? "Locked - unlock to add tasks" : (hasActiveTab ? null : "Add a tab first"));
		}

		private void AddTaskToActiveTab()
		{
			if (!_settings.LockTasks.get_Value())
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

		private void BeginRenameTab(TodoTab tab, bool isNew = false)
		{
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
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
			((Control)val).set_Height(26);
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

		private void ShowTaskMenu(TodoTask task)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Expected O, but got Unknown
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			TodoTab tab = ActiveTab;
			if (tab == null)
			{
				return;
			}
			IReadOnlyList<TodoTask> selectedTasks = _listPanel.GetSelectedTasks();
			ContextMenuStrip menu = new ContextMenuStrip();
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
			((Control)menu.AddMenuItem("Move up")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MoveTask(tab, task, -1);
			});
			((Control)menu.AddMenuItem("Move down")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MoveTask(tab, task, 1);
			});
			List<TodoTab> otherTabs = (from t in _store.Tabs
				where t.Id != tab.Id
				orderby t.Order
				select t).ToList();
			if (otherTabs.Count > 0)
			{
				ContextMenuStripItem moveTo = menu.AddMenuItem("Move to");
				ContextMenuStrip moveMenu = new ContextMenuStrip();
				foreach (TodoTab other in otherTabs)
				{
					TodoTab captured = other;
					((Control)moveMenu.AddMenuItem(other.Name)).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						tab.Tasks.Remove(task);
						NormalizeTaskOrder(tab);
						task.Order = captured.Tasks.Count;
						captured.Tasks.Add(task);
						_listPanel.ClearSelection();
						MarkDirtyAndRefresh();
					});
				}
				moveTo.set_Submenu(moveMenu);
			}
			((Control)menu.AddMenuItem("Delete")).add_Click((EventHandler<MouseEventArgs>)delegate
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

		private void MoveTask(TodoTab tab, TodoTask task, int delta)
		{
			List<TodoTask> ordered = tab.Tasks.OrderBy((TodoTask t) => t.Order).ToList();
			int i = ordered.IndexOf(task);
			int j = i + delta;
			if (i >= 0 && j >= 0 && j < ordered.Count)
			{
				TodoTask tmp = ordered[i];
				ordered[i] = ordered[j];
				ordered[j] = tmp;
				for (int k = 0; k < ordered.Count; k++)
				{
					ordered[k].Order = k;
				}
				_listPanel.ClearSelection();
				MarkDirtyAndRefresh();
			}
		}

		private static void NormalizeTaskOrder(TodoTab tab)
		{
			List<TodoTask> ordered = tab.Tasks.OrderBy((TodoTask task) => task.Order).ToList();
			for (int i = 0; i < ordered.Count; i++)
			{
				ordered[i].Order = i;
			}
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
			OnMinuteTick(DateTime.UtcNow);
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
