using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Blish_HUD.Controls;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Controls
{
	public class CharacterRoutineSidebar : Kenedia.Modules.Core.Controls.Panel
	{
		private sealed class SidebarEntry
		{
			public CharacterRoutineModel CharacterRoutine { get; init; }

			public Kenedia.Modules.Core.Controls.Panel EntryPanel { get; init; }

			public Kenedia.Modules.Core.Controls.Label NameLabel { get; init; }
		}

		private const int SidebarWidth = 200;

		private const int HeaderOuterPadding = 5;

		private const int HeaderControlWidth = 180;

		private const int HeaderControlSpacing = 5;

		private const int HeaderToListOffset = 5;

		private const int ListBottomPadding = 15;

		private const int ScrollbarReservedWidth = 12;

		private const int ListEntryHeight = 32;

		private const int ListEntryLabelX = 5;

		private const int ListEntryLeftPadding = 8;

		private static readonly Color SelectedBackground = new Color(60, 60, 60, 200);

		private static readonly Color CompletedBackground = new Color(40, 80, 40, 200);

		private static readonly Color CompletedTextColor = new Color(120, 200, 120);

		private readonly CharacterRoutineService _service;

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _headerPanel;

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _listPanel;

		private readonly Kenedia.Modules.Core.Controls.TextBox _searchBox;

		private readonly Dictionary<Guid, SidebarEntry> _listRoutineEntries = new Dictionary<Guid, SidebarEntry>();

		public CharacterRoutineSidebar(CharacterRoutineService service)
		{
			_service = service;
			base.Width = 200;
			HeightSizingMode = SizingMode.Fill;
			base.ShowBorder = true;
			_headerPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(0f, 5f),
				OuterControlPadding = new Vector2(5f)
			};
			new Button
			{
				Parent = _headerPanel,
				Text = strings.NewCharacterRoutine,
				Width = 180,
				Height = 30,
				ClickAction = delegate
				{
					_service.CreateNewRoutine();
				}
			};
			_searchBox = new Kenedia.Modules.Core.Controls.TextBox
			{
				Parent = _headerPanel,
				PlaceholderText = strings.SearchCharacterRoutines,
				Width = 180,
				Height = 28,
				TextChangedAction = delegate
				{
					UpdateFilterVisibility();
				}
			};
			_listPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				Location = Point.Zero,
				WidthSizingMode = SizingMode.Fill,
				Height = 0,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(0f, 2f),
				OuterControlPadding = new Vector2(8f, 0f),
				CanScroll = true
			};
			base.Resized += delegate
			{
				UpdateLayout();
			};
			_headerPanel.Resized += delegate
			{
				UpdateLayout();
			};
			_listPanel.Resized += delegate
			{
				UpdateListEntryWidths();
			};
			_service.State.CharacterRoutines.Changed += new EventHandler<StateVarChangedEventArgs<ObservableCollection<CharacterRoutineModel>>>(CharacterRoutines_Changed);
			_service.State.SelectedRoutine.Changed += new EventHandler<StateVarChangedEventArgs<CharacterRoutineModel>>(SelectedRoutine_Changed);
			foreach (CharacterRoutineModel characterRoutine in _service.CharacterRoutines)
			{
				EnsureListEntry(characterRoutine);
			}
			UpdateLayout();
			UpdateFilterVisibility();
			UpdateAllEntryVisuals();
			CharacterRoutineModel firstCharacterRoutine = _service.CharacterRoutines.FirstOrDefault();
			if (firstCharacterRoutine != null)
			{
				_service.SelectRoutine(firstCharacterRoutine);
			}
		}

		private void EnsureListEntry(CharacterRoutineModel characterRoutine)
		{
			CharacterRoutineModel characterRoutine2 = characterRoutine;
			if (characterRoutine2 != null && !_listRoutineEntries.ContainsKey(characterRoutine2.Id))
			{
				Kenedia.Modules.Core.Controls.Panel entryPanel = new Kenedia.Modules.Core.Controls.Panel
				{
					Parent = _listPanel,
					Width = GetListEntryWidth(),
					Height = 32
				};
				Kenedia.Modules.Core.Controls.Label nameLabel = new Kenedia.Modules.Core.Controls.Label
				{
					Parent = entryPanel,
					Text = characterRoutine2.Name,
					Location = new Point(5, 0),
					AutoSizeWidth = true,
					Height = 32,
					VerticalAlignment = VerticalAlignment.Middle
				};
				entryPanel.Click += delegate
				{
					_service.SelectRoutine(characterRoutine2);
				};
				_listRoutineEntries[characterRoutine2.Id] = new SidebarEntry
				{
					CharacterRoutine = characterRoutine2,
					EntryPanel = entryPanel,
					NameLabel = nameLabel
				};
				UpdateListEntryVisual(characterRoutine2);
			}
		}

		private void RemoveListEntry(CharacterRoutineModel characterRoutine)
		{
			if (characterRoutine != null && _listRoutineEntries.TryGetValue(characterRoutine.Id, out var entry))
			{
				entry.EntryPanel.Dispose();
				_listRoutineEntries.Remove(characterRoutine.Id);
			}
		}

		private void UpdateFilterVisibility()
		{
			foreach (SidebarEntry entry in _listRoutineEntries.Values)
			{
				entry.EntryPanel.Visible = MatchesFilter(entry.CharacterRoutine);
			}
			UpdateListEntryWidths();
		}

		private bool MatchesFilter(CharacterRoutineModel characterRoutine)
		{
			string filter = _searchBox?.Text?.Trim() ?? string.Empty;
			if (filter.Length == 0)
			{
				return true;
			}
			if (!string.IsNullOrEmpty(characterRoutine?.Name))
			{
				return characterRoutine.Name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;
			}
			return false;
		}

		private void UpdateListEntryVisual(CharacterRoutineModel characterRoutine)
		{
			if (characterRoutine != null && _listRoutineEntries.TryGetValue(characterRoutine.Id, out var entry))
			{
				bool isSelected = _service.SelectedRoutine?.Id == characterRoutine.Id;
				bool isCompleted = characterRoutine.RoutineSteps.Count > 0 && characterRoutine.RoutineSteps.All((CharacterRoutineStep step) => step.IsCompleted);
				entry.EntryPanel.BackgroundColor = (isSelected ? SelectedBackground : (isCompleted ? CompletedBackground : Color.Transparent));
				entry.NameLabel.Text = characterRoutine.Name;
				entry.NameLabel.TextColor = (isCompleted ? CompletedTextColor : Color.White);
				entry.EntryPanel.Visible = MatchesFilter(characterRoutine);
			}
		}

		private void UpdateAllEntryVisuals()
		{
			foreach (CharacterRoutineModel characterRoutine in _service.CharacterRoutines)
			{
				UpdateListEntryVisual(characterRoutine);
			}
		}

		private void UpdateLayout()
		{
			int listTop = _headerPanel.Bottom + 5;
			_listPanel.Location = new Point(0, listTop);
			_listPanel.Height = Math.Max(0, base.Height - listTop - 15);
			UpdateListEntryWidths();
		}

		private int GetListEntryWidth()
		{
			return Math.Max(0, _listPanel.Width - 8 - 12);
		}

		private void UpdateListEntryWidths()
		{
			int entryWidth = GetListEntryWidth();
			foreach (SidebarEntry value in _listRoutineEntries.Values)
			{
				value.EntryPanel.Width = entryWidth;
			}
		}

		protected override void DisposeControl()
		{
			_service.State.CharacterRoutines.Changed -= new EventHandler<StateVarChangedEventArgs<ObservableCollection<CharacterRoutineModel>>>(CharacterRoutines_Changed);
			_service.State.SelectedRoutine.Changed -= new EventHandler<StateVarChangedEventArgs<CharacterRoutineModel>>(SelectedRoutine_Changed);
			base.DisposeControl();
		}

		private void CharacterRoutines_Changed(object sender, StateVarChangedEventArgs<ObservableCollection<CharacterRoutineModel>> e)
		{
			ObservableCollection<CharacterRoutineModel> currentLists = _service.CharacterRoutines;
			HashSet<Guid> currentIds = new HashSet<Guid>(currentLists.Select((CharacterRoutineModel list) => list.Id));
			foreach (Guid existingId in _listRoutineEntries.Keys.ToList())
			{
				if (!currentIds.Contains(existingId) && _listRoutineEntries.TryGetValue(existingId, out var existing))
				{
					existing.EntryPanel.Dispose();
					_listRoutineEntries.Remove(existingId);
				}
			}
			foreach (CharacterRoutineModel characterRoutine in currentLists)
			{
				EnsureListEntry(characterRoutine);
			}
			UpdateFilterVisibility();
			UpdateAllEntryVisuals();
		}

		private void SelectedRoutine_Changed(object sender, StateVarChangedEventArgs<CharacterRoutineModel> e)
		{
			UpdateListEntryVisual(e.OldValue);
			UpdateListEntryVisual(e.NewValue);
		}
	}
}
