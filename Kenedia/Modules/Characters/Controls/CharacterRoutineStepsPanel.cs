using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Controls
{
	public class CharacterRoutineStepsPanel : Kenedia.Modules.Core.Controls.FlowPanel
	{
		private const int ScrollbarReservedWidth = 12;

		private const int InsertionLineLeftPadding = 5;

		private const int InsertionLineVerticalOffset = 1;

		private const int StepRowSpacing = 3;

		private readonly CharacterRoutineService _service;

		private readonly Settings _settings;

		private readonly ObservableCollection<Character_Model> _characterModels;

		private readonly Kenedia.Modules.Core.Controls.Panel _insertionLine;

		private readonly Dictionary<CharacterRoutineStep, CharacterRoutineStepRow> _stepRows = new Dictionary<CharacterRoutineStep, CharacterRoutineStepRow>();

		private CharacterRoutineModel _boundRoutine;

		private Kenedia.Modules.Core.Controls.Panel _headerPanel;

		private Kenedia.Modules.Core.Controls.Panel _stepsContainer;

		private Kenedia.Modules.Core.Controls.Checkbox _allStepsCheckbox;

		private Kenedia.Modules.Core.Controls.Label _stepsLabel;

		private Button _nextButton;

		private Kenedia.Modules.Core.Controls.Label _statusLabel;

		private Button _hideButton;

		private Separator _separator;

		private Kenedia.Modules.Core.Controls.FlowPanel _newStepRow;

		private AutoSuggestComboBox<Character_Model> _characterSuggestionBox;

		private CharacterRoutineStepRow _draggedStepRow;

		private bool _isDragging;

		private bool _dragActivated;

		private bool _overrideDisplayBehaviorForDrag;

		private bool _overrideHideCompletedSteps;

		private int _pendingTargetIndex = -1;

		private bool _syncingHeaderCheckbox;

		private Blish_HUD.Controls.TextBox _stepDescriptionBox;

		private ImageButton _addStepButton;

		public CharacterRoutineStepsPanel(CharacterRoutineService service, Settings settings, ObservableCollection<Character_Model> characterModels)
		{
			_service = service;
			_settings = settings;
			_characterModels = characterModels;
			WidthSizingMode = SizingMode.Fill;
			HeightSizingMode = SizingMode.Fill;
			base.FlowDirection = ControlFlowDirection.SingleTopToBottom;
			base.ControlPadding = new Vector2(0f, 5f);
			_settings.CompletedRoutineStepsBehavior.SettingChanged += CompletedRoutineStepsBehavior_SettingChanged;
			_service.State.SelectedRoutine.Changed += new EventHandler<StateVarChangedEventArgs<CharacterRoutineModel>>(SelectedRoutine_Changed);
			_service.State.TrackedStep.Changed += new EventHandler<StateVarChangedEventArgs<CharacterRoutineStep>>(TrackedStep_Changed);
			_service.State.StepSwitchStatus.Changed += new EventHandler<StateVarChangedEventArgs<CharacterRoutineStepSwitchStatus>>(StepSwitchStatus_Changed);
			_insertionLine = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = GameService.Graphics.SpriteScreen,
				BackgroundColor = ContentService.Colors.ColonialWhite,
				Height = 3,
				Visible = false,
				ZIndex = 2147483645,
				CaptureInput = false
			};
			BuildHeader();
			BuildNewStepRow();
			_stepsContainer = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = this,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.Fill,
				CanScroll = true
			};
			_stepsContainer.Resized += delegate
			{
				RefreshStepRowsLayout();
			};
			BindSelectedRoutine(_service.SelectedRoutine);
		}

		public void BindSelectedRoutine(CharacterRoutineModel selectedRoutine)
		{
			CancelDrag();
			if (_boundRoutine != null)
			{
				_boundRoutine.PropertyChanged -= new PropertyChangedEventHandler(BoundRoutine_PropertyChanged);
			}
			_boundRoutine = selectedRoutine;
			_overrideHideCompletedSteps = false;
			if (_boundRoutine != null)
			{
				_boundRoutine.PropertyChanged += new PropertyChangedEventHandler(BoundRoutine_PropertyChanged);
			}
			SyncRowsWithBoundRoutine();
			RefreshHeader();
			RefreshStepRowsLayout(preserveScroll: false);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			base.OnResized(e);
			UpdateLayout();
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			UpdateLayout();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (_isDragging && _stepsContainer != null && _draggedStepRow != null)
			{
				_dragActivated = true;
				UpdateInsertionIndicator(GameService.Input.Mouse.Position, _stepsContainer.AbsoluteBounds);
			}
		}

		protected override void DisposeControl()
		{
			CancelDrag();
			if (_boundRoutine != null)
			{
				_boundRoutine.PropertyChanged -= new PropertyChangedEventHandler(BoundRoutine_PropertyChanged);
			}
			_service.State.SelectedRoutine.Changed -= new EventHandler<StateVarChangedEventArgs<CharacterRoutineModel>>(SelectedRoutine_Changed);
			_service.State.TrackedStep.Changed -= new EventHandler<StateVarChangedEventArgs<CharacterRoutineStep>>(TrackedStep_Changed);
			_service.State.StepSwitchStatus.Changed -= new EventHandler<StateVarChangedEventArgs<CharacterRoutineStepSwitchStatus>>(StepSwitchStatus_Changed);
			_settings.CompletedRoutineStepsBehavior.SettingChanged -= CompletedRoutineStepsBehavior_SettingChanged;
			_headerPanel.Resized -= HeaderPanel_Resized;
			foreach (CharacterRoutineStepRow item in _stepRows.Values.ToList())
			{
				item.Dispose();
			}
			_stepRows.Clear();
			_insertionLine?.Dispose();
			base.DisposeControl();
		}

		private void BuildHeader()
		{
			_headerPanel = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = this,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				Width = base.Width
			};
			_headerPanel.Resized += HeaderPanel_Resized;
			_allStepsCheckbox = new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = _headerPanel,
				Location = new Point(10, 0),
				Width = 20,
				Height = 28,
				CheckedChangedAction = delegate(bool isChecked)
				{
					if (!_syncingHeaderCheckbox)
					{
						_service.SetAllRoutineStepsCompletion(isChecked);
					}
				}
			};
			_nextButton = new Button
			{
				Parent = _headerPanel,
				Text = strings.Next,
				Width = 90,
				Height = 28,
				Location = new Point(500, 0),
				ClickAction = delegate
				{
					_service.SwitchToNextIncompleteRoutineStep();
				}
			};
			_stepsLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = _headerPanel,
				Text = "Steps",
				Font = Control.Content.DefaultFont16,
				AutoSizeWidth = false,
				Height = 28,
				Width = _headerPanel.ContentRegion.Right - _allStepsCheckbox.Right - 10 - _nextButton.Width,
				Location = new Point(50, 0),
				VerticalAlignment = VerticalAlignment.Middle
			};
			_statusLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = _headerPanel,
				Width = 380,
				Height = 28,
				VerticalAlignment = VerticalAlignment.Middle
			};
			_hideButton = new Button
			{
				Parent = _headerPanel,
				Width = 165,
				Height = 28,
				BasicTooltipText = strings.CompletedRoutineStepsHiddenTooltip,
				ClickAction = new Action(ToggleOverrideHideCompletedSteps)
			};
			_separator = new Separator
			{
				Parent = this,
				Height = 1,
				Color = Color.LightGray * 0.5f
			};
		}

		private void BuildNewStepRow()
		{
			_newStepRow = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleLeftToRight,
				ControlPadding = new Vector2(5f, 0f)
			};
			_addStepButton = new ImageButton
			{
				Parent = _newStepRow,
				Texture = AsyncTexture2D.FromAssetId(155902),
				DisabledTexture = AsyncTexture2D.FromAssetId(155903),
				HoveredTexture = AsyncTexture2D.FromAssetId(155904),
				Size = new Point(30, 30),
				BasicTooltipText = strings.AddRoutineStep,
				Enabled = true,
				ClickAction = delegate
				{
					_service.AddRoutineStep(_characterSuggestionBox.Selected?.Name, _stepDescriptionBox.Text);
				}
			};
			_characterSuggestionBox = new AutoSuggestComboBox<Character_Model>
			{
				Parent = _newStepRow,
				PlaceholderText = strings.SearchCharacterName,
				Width = 160,
				Height = 30,
				MaxSuggestionHeight = 300,
				SelectableFactory = (Character_Model character) => new CharacterSelectable(_characterSuggestionBox, character),
				Items = _characterModels,
				AllowBlankSelection = true,
				SetSelectedText = false,
				BlankSelectionText = strings.Unassigned
			};
			_characterSuggestionBox.SelectedItemChanged += new ValueChangedEventHandler<Character_Model>(CharacterSuggestionBox_SelectedItemChanged);
			_stepDescriptionBox = new Blish_HUD.Controls.TextBox
			{
				Parent = _newStepRow,
				PlaceholderText = strings.RoutineStepDescriptionPlaceholder,
				Width = 515,
				Height = 30
			};
		}

		private void UpdateLayout()
		{
			_separator?.SetSize(base.Width, _separator.Height);
			_stepDescriptionBox?.SetSize(_stepDescriptionBox?.Parent?.ContentRegion.Right - (_characterSuggestionBox?.Right ?? 0) - 15);
			AutoSuggestComboBox<Character_Model> characterSuggestionBox = _characterSuggestionBox;
			if (characterSuggestionBox != null)
			{
				characterSuggestionBox.MaxSuggestionHeight = base.Height - 20;
			}
			_nextButton?.SetLocation(new Point((_headerPanel?.Width - _nextButton.Width).GetValueOrDefault(), _nextButton.Location.Y));
			_stepsLabel?.SetLocation(new Point(_characterSuggestionBox?.Left ?? 0, _stepsLabel.Location.Y));
			_statusLabel?.SetLocation(new Point(_stepDescriptionBox?.Left ?? 0, _statusLabel.Location.Y));
			Kenedia.Modules.Core.Controls.Label statusLabel = _statusLabel;
			if (statusLabel != null)
			{
				Blish_HUD.Controls.TextBox stepDescriptionBox = _stepDescriptionBox;
				statusLabel.SetSize(((stepDescriptionBox != null) ? new int?(stepDescriptionBox.Width - 5) : null) - (_nextButton?.Width ?? 0), _stepsLabel.Height);
			}
		}

		private void CompletedRoutineStepsBehavior_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<Settings.CompletedRoutineStepsDisplayBehavior> e)
		{
			if (!_isDragging)
			{
				RefreshHeader();
				RefreshStepRowsLayout();
			}
		}

		private void ToggleOverrideHideCompletedSteps()
		{
			_overrideHideCompletedSteps = !_overrideHideCompletedSteps;
			RefreshHeader();
			RefreshStepRowsLayout();
		}

		private void UpdateInsertionIndicator(Point mousePos, Rectangle containerBounds)
		{
			List<CharacterRoutineStepRow> visibleRows = _stepRows.Values.Where((CharacterRoutineStepRow row) => row.Visible).ToList();
			int dragIndex = _boundRoutine?.RoutineSteps.IndexOf(_draggedStepRow.Step) ?? (-1);
			int insertionVisualIndex = visibleRows.Count;
			int insertionY = ((visibleRows.Count > 0) ? visibleRows.Last().AbsoluteBounds.Bottom : containerBounds.Top);
			for (int i = 0; i < visibleRows.Count; i++)
			{
				Rectangle rowBounds = visibleRows[i].AbsoluteBounds;
				int rowMiddle = rowBounds.Top + rowBounds.Height / 2;
				if (mousePos.Y < rowMiddle)
				{
					insertionVisualIndex = i;
					insertionY = rowBounds.Top;
					break;
				}
			}
			int targetIndex = (_pendingTargetIndex = ((insertionVisualIndex <= dragIndex) ? insertionVisualIndex : (insertionVisualIndex - 1)));
			bool withinBounds = insertionY >= containerBounds.Top && insertionY <= containerBounds.Bottom;
			if (targetIndex != dragIndex && withinBounds)
			{
				_insertionLine.Location = new Point(containerBounds.Left + 5, insertionY - 1);
				_insertionLine.Width = Math.Max(0, containerBounds.Width - 10 - 2);
				_insertionLine.Visible = true;
			}
			else
			{
				_insertionLine.Visible = false;
			}
		}

		private void OnDragStart(CharacterRoutineStepRow row)
		{
			if (!_isDragging && _boundRoutine != null)
			{
				_overrideDisplayBehaviorForDrag = true;
				RefreshHeader();
				RefreshStepRowsLayout();
				_stepRows.TryGetValue(row.Step, out _draggedStepRow);
				if (_draggedStepRow == null)
				{
					_overrideDisplayBehaviorForDrag = false;
					RefreshHeader();
					RefreshStepRowsLayout();
				}
				else
				{
					_isDragging = true;
					_dragActivated = false;
					_draggedStepRow.IsDragging = true;
					_pendingTargetIndex = _boundRoutine.RoutineSteps.IndexOf(row.Step);
					GameService.Input.Mouse.LeftMouseButtonReleased += OnGlobalMouseReleased;
				}
			}
		}

		private void OnGlobalMouseReleased(object sender, MouseEventArgs e)
		{
			CompleteDrop();
		}

		private void CompleteDrop()
		{
			if (_isDragging)
			{
				GameService.Input.Mouse.LeftMouseButtonReleased -= OnGlobalMouseReleased;
				_insertionLine.Visible = false;
				CharacterRoutineStep step = _draggedStepRow?.Step;
				int targetIndex = _pendingTargetIndex;
				bool num = _dragActivated && step != null && targetIndex >= 0 && targetIndex != _boundRoutine?.RoutineSteps.IndexOf(step);
				if (_draggedStepRow != null)
				{
					_draggedStepRow.IsDragging = false;
				}
				_draggedStepRow = null;
				_isDragging = false;
				_dragActivated = false;
				_overrideDisplayBehaviorForDrag = false;
				_pendingTargetIndex = -1;
				if (num)
				{
					_service.ReorderRoutineStep(step, targetIndex);
				}
				RefreshHeader();
				RefreshStepRowsLayout();
			}
		}

		private void CancelDrag()
		{
			GameService.Input.Mouse.LeftMouseButtonReleased -= OnGlobalMouseReleased;
			if (_draggedStepRow != null)
			{
				_draggedStepRow.IsDragging = false;
			}
			bool num = _isDragging || _overrideDisplayBehaviorForDrag;
			_draggedStepRow = null;
			_isDragging = false;
			_dragActivated = false;
			_overrideDisplayBehaviorForDrag = false;
			_pendingTargetIndex = -1;
			_insertionLine.Visible = false;
			if (num)
			{
				RefreshHeader();
				RefreshStepRowsLayout();
			}
		}

		private void HeaderPanel_Resized(object sender, ResizedEventArgs e)
		{
			UpdateLayout();
		}

		private Settings.CompletedRoutineStepsDisplayBehavior GetEffectiveBehavior()
		{
			if (!_overrideDisplayBehaviorForDrag)
			{
				return _settings.CompletedRoutineStepsBehavior.Value;
			}
			return Settings.CompletedRoutineStepsDisplayBehavior.Nothing;
		}

		private IEnumerable<CharacterRoutineStep> GetDisplayedRoutineSteps(CharacterRoutineModel characterRoutine)
		{
			if (characterRoutine == null)
			{
				return Array.Empty<CharacterRoutineStep>();
			}
			IEnumerable<CharacterRoutineStep> orderedRoutineSteps = characterRoutine.RoutineSteps.AsEnumerable();
			return GetEffectiveBehavior() switch
			{
				Settings.CompletedRoutineStepsDisplayBehavior.HideCompletedRoutineSteps => _overrideHideCompletedSteps ? orderedRoutineSteps : orderedRoutineSteps.Where((CharacterRoutineStep step) => !step.IsCompleted), 
				Settings.CompletedRoutineStepsDisplayBehavior.MoveCompletedRoutineStepsToBottomOfDisplay => orderedRoutineSteps.Where((CharacterRoutineStep step) => !step.IsCompleted).Concat(orderedRoutineSteps.Where((CharacterRoutineStep step) => step.IsCompleted)), 
				_ => orderedRoutineSteps, 
			};
		}

		private void CharacterSuggestionBox_SelectedItemChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Character_Model> e)
		{
			if (_characterSuggestionBox.Selected != null)
			{
				_service.AddRoutineStep(_characterSuggestionBox.Selected!.Name, _stepDescriptionBox.Text);
			}
			_characterSuggestionBox.Selected = null;
		}

		private int GetStepRowWidth()
		{
			if (_stepsContainer != null)
			{
				return Math.Max(0, _stepsContainer.Width - (_stepsContainer.HasVisibleVerticalScrollbar() ? 22 : 0));
			}
			return 0;
		}

		private void RefreshHeader()
		{
			bool hasRoutine = _boundRoutine != null;
			_headerPanel.Visible = hasRoutine;
			_stepsContainer.Visible = hasRoutine;
			if (hasRoutine)
			{
				int completedCount = _boundRoutine.RoutineSteps.Count((CharacterRoutineStep step) => step.IsCompleted);
				int totalCount = _boundRoutine.RoutineSteps.Count;
				_stepsLabel.Text = $"Steps ({completedCount}/{totalCount})";
				bool allChecked = totalCount > 0 && completedCount == totalCount;
				_syncingHeaderCheckbox = true;
				_allStepsCheckbox.Checked = allChecked;
				_allStepsCheckbox.BasicTooltipText = (allChecked ? strings.UncheckAll : strings.CheckAll);
				_syncingHeaderCheckbox = false;
				CharacterRoutineStep pendingCompletionStep = _service.GetTrackedStepForSelectedRoutine();
				int incompleteCount = _boundRoutine.RoutineSteps.Count((CharacterRoutineStep step) => step.Enabled && !step.IsCompleted);
				bool hasIncompleteSteps = incompleteCount > 0;
				UpdateNextButton(pendingCompletionStep, incompleteCount, hasIncompleteSteps);
				UpdateStatusLabel(pendingCompletionStep);
				bool canUnhideCompletedSteps = GetEffectiveBehavior() == Settings.CompletedRoutineStepsDisplayBehavior.HideCompletedRoutineSteps && _boundRoutine.RoutineSteps.Any((CharacterRoutineStep step) => step.IsCompleted);
				_hideButton.Text = (_overrideHideCompletedSteps ? strings.HideComplete : strings.UnhideComplete);
				_hideButton.Visible = canUnhideCompletedSteps;
				_hideButton.Enabled = canUnhideCompletedSteps;
			}
		}

		private void UpdateNextButton(CharacterRoutineStep pendingCompletionStep, int incompleteCount, bool hasIncompleteSteps)
		{
			if (!hasIncompleteSteps)
			{
				_nextButton.Text = strings.Next;
				_nextButton.Enabled = false;
				_nextButton.BasicTooltipText = strings.AllRoutineStepsComplete;
				return;
			}
			CharacterRoutineStepSwitchStatus switchStatus = _service.State.StepSwitchStatus.Value;
			bool isReadyToComplete = pendingCompletionStep != null && switchStatus == CharacterRoutineStepSwitchStatus.ReadyToComplete;
			bool isReadyToFinish = incompleteCount == 1 && isReadyToComplete;
			_nextButton.Text = ((switchStatus == CharacterRoutineStepSwitchStatus.Failed) ? strings.Retry : (isReadyToFinish ? strings.Finish : strings.Next));
			switch (switchStatus)
			{
			case CharacterRoutineStepSwitchStatus.Switching:
				_nextButton.Enabled = false;
				_nextButton.BasicTooltipText = string.Format(strings.CharacterSwap_SwitchTo, pendingCompletionStep?.CharacterName);
				break;
			case CharacterRoutineStepSwitchStatus.Failed:
				_nextButton.Enabled = true;
				_nextButton.BasicTooltipText = string.Format(strings.RoutineStepSwitchFailed, pendingCompletionStep?.CharacterName);
				break;
			case CharacterRoutineStepSwitchStatus.CharacterNotFound:
				_nextButton.Enabled = false;
				_nextButton.BasicTooltipText = string.Format(strings.RoutineStepCharacterNotFound, pendingCompletionStep?.CharacterName);
				break;
			case CharacterRoutineStepSwitchStatus.CharacterNotAssigned:
				_nextButton.Enabled = false;
				_nextButton.BasicTooltipText = strings.RoutineStepCharacterNotAssigned;
				break;
			case CharacterRoutineStepSwitchStatus.ReadyToComplete:
				_nextButton.Enabled = true;
				_nextButton.BasicTooltipText = (isReadyToFinish ? strings.CompleteCharacterRoutine : string.Format(strings.NextClickMarksComplete, pendingCompletionStep?.CharacterName));
				break;
			default:
				_nextButton.Enabled = true;
				_nextButton.BasicTooltipText = strings.SwitchToFirstIncomplete;
				break;
			}
		}

		private void RefreshStepRowsLayout(bool preserveScroll = true)
		{
			if (_stepsContainer == null || _boundRoutine == null)
			{
				return;
			}
			int previousScrollOffset = (preserveScroll ? _stepsContainer.VerticalScrollOffset : 0);
			int rowWidth = GetStepRowWidth();
			List<CharacterRoutineStep> list = GetDisplayedRoutineSteps(_boundRoutine).ToList();
			HashSet<CharacterRoutineStep> displayedStepSet = new HashSet<CharacterRoutineStep>(list);
			int y = 0;
			foreach (CharacterRoutineStep step in list)
			{
				if (_stepRows.TryGetValue(step, out var row2))
				{
					row2.Visible = true;
					row2.Width = rowWidth;
					row2.Location = new Point(0, y);
					y += row2.Height + 3;
				}
			}
			foreach (KeyValuePair<CharacterRoutineStep, CharacterRoutineStepRow> row in _stepRows)
			{
				if (!displayedStepSet.Contains(row.Key))
				{
					row.Value.Visible = false;
				}
			}
			if (preserveScroll)
			{
				int maxOffset = Math.Max(0, y - _stepsContainer.ContentRegion.Height);
				_stepsContainer.VerticalScrollOffset = Math.Max(0, Math.Min(previousScrollOffset, maxOffset));
			}
		}

		private void SyncRowsWithBoundRoutine()
		{
			if (_boundRoutine == null)
			{
				foreach (CharacterRoutineStep step3 in _stepRows.Keys.ToList())
				{
					RemoveRow(step3);
				}
				return;
			}
			HashSet<CharacterRoutineStep> validSteps = new HashSet<CharacterRoutineStep>(_boundRoutine.RoutineSteps);
			foreach (CharacterRoutineStep step2 in _stepRows.Keys.ToList())
			{
				if (!validSteps.Contains(step2))
				{
					RemoveRow(step2);
				}
			}
			foreach (CharacterRoutineStep step in _boundRoutine.RoutineSteps)
			{
				if (!_stepRows.ContainsKey(step))
				{
					AddRow(step);
				}
			}
		}

		private void AddRow(CharacterRoutineStep step)
		{
			if (step != null && _stepsContainer != null && !_stepRows.ContainsKey(step))
			{
				_stepRows[step] = new CharacterRoutineStepRow(_service, _characterModels, step, new Action<CharacterRoutineStepRow>(OnDragStart))
				{
					Parent = _stepsContainer,
					WidthSizingMode = SizingMode.Standard,
					Width = GetStepRowWidth()
				};
			}
		}

		private void RemoveRow(CharacterRoutineStep step)
		{
			if (step != null && _stepRows.TryGetValue(step, out var row))
			{
				if (_draggedStepRow == row)
				{
					CancelDrag();
				}
				row.Dispose();
				_stepRows.Remove(step);
			}
		}

		private bool IsBoundRoutine(CharacterRoutineModel routine)
		{
			if (_boundRoutine != null)
			{
				return _boundRoutine == routine;
			}
			return false;
		}

		private void SelectedRoutine_Changed(object sender, StateVarChangedEventArgs<CharacterRoutineModel> e)
		{
			if (e.OldValue != e.NewValue)
			{
				BindSelectedRoutine(e.NewValue);
			}
			else if (IsBoundRoutine(e.NewValue))
			{
				SyncRowsWithBoundRoutine();
				RefreshHeader();
				RefreshStepRowsLayout();
			}
		}

		private void TrackedStep_Changed(object sender, StateVarChangedEventArgs<CharacterRoutineStep> e)
		{
			if (_boundRoutine != null && _boundRoutine == _service.SelectedRoutine)
			{
				RefreshHeader();
			}
		}

		private void StepSwitchStatus_Changed(object sender, StateVarChangedEventArgs<CharacterRoutineStepSwitchStatus> e)
		{
			if (_boundRoutine != null && _boundRoutine == _service.SelectedRoutine)
			{
				RefreshHeader();
			}
		}

		private void BoundRoutine_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_boundRoutine != null && sender == _boundRoutine)
			{
				SyncRowsWithBoundRoutine();
				RefreshHeader();
				RefreshStepRowsLayout();
			}
		}

		private void UpdateStatusLabel(CharacterRoutineStep pendingCompletionStep)
		{
			if (pendingCompletionStep == null)
			{
				_statusLabel.Text = string.Empty;
				_statusLabel.TextColor = Color.LightGray;
				return;
			}
			string characterName = pendingCompletionStep.CharacterName;
			switch (_service.State.StepSwitchStatus.Value)
			{
			case CharacterRoutineStepSwitchStatus.Switching:
				_statusLabel.Text = string.Format(strings.CharacterSwap_SwitchTo, characterName);
				_statusLabel.TextColor = Color.LightYellow;
				break;
			case CharacterRoutineStepSwitchStatus.ReadyToComplete:
				_statusLabel.Text = string.Format(strings.NextClickMarksComplete, characterName);
				_statusLabel.TextColor = Color.LightGreen;
				break;
			case CharacterRoutineStepSwitchStatus.Failed:
				_statusLabel.Text = string.Format(strings.RoutineStepSwitchFailed, characterName);
				_statusLabel.TextColor = Color.OrangeRed;
				break;
			case CharacterRoutineStepSwitchStatus.CharacterNotFound:
				_statusLabel.Text = string.Format(strings.RoutineStepCharacterNotFound, characterName);
				_statusLabel.TextColor = Color.OrangeRed;
				break;
			case CharacterRoutineStepSwitchStatus.CharacterNotAssigned:
				_statusLabel.Text = strings.RoutineStepCharacterNotAssigned;
				_statusLabel.TextColor = Color.OrangeRed;
				break;
			default:
				_statusLabel.Text = string.Empty;
				_statusLabel.TextColor = Color.LightGray;
				break;
			}
		}
	}
}
