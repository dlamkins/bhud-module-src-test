using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Maestro.UI.MaestroCreator
{
	public class NoteSequencePanel : Panel
	{
		public static class Layout
		{
			public const int HeaderHeight = 26;

			public const int HeaderBottomSpacing = 12;

			public const int ChipSpacing = 4;

			public const int Padding = 5;

			public const int PaddingBottom = 30;

			public const int ButtonY = 6;
		}

		private readonly List<string> _notes = new List<string>();

		private readonly List<NoteChip> _chips = new List<NoteChip>();

		private readonly HashSet<int> _selectedIndices = new HashSet<int>();

		private int _lastClickedIndex = -1;

		private bool _isInsertMode;

		private bool _isReplaceMode;

		private int _replaceTargetIndex = -1;

		private readonly List<string> _clipboard = new List<string>();

		private readonly Stack<List<string>> _undoStack = new Stack<List<string>>();

		private readonly ContextMenuStrip _contextMenu;

		private readonly ContextMenuStripItem _previewSelectedItem;

		private readonly ContextMenuStripItem _deleteSelectedItem;

		private readonly ContextMenuStripItem _replaceItem;

		private readonly ContextMenuStripItem _copyItem;

		private readonly ContextMenuStripItem _pasteItem;

		private readonly ContextMenuStripItem _selectAllItem;

		private readonly ContextMenuStripItem _clearSelectionItem;

		private readonly Label _headerLabel;

		private readonly Label _modeStatusLabel;

		private readonly StandardButton _expandButton;

		private readonly StandardButton _insertButton;

		private readonly StandardButton _undoButton;

		private readonly StandardButton _clearButton;

		private readonly FlowPanel _chipsContainer;

		private Panel _bottomSpacer;

		private const int MaxUndoDepth = 100;

		public IReadOnlyList<string> Notes => _notes.AsReadOnly();

		public int NoteCount => _notes.Count;

		public bool HasSelection => _selectedIndices.Count > 0;

		public bool IsInsertMode => _isInsertMode;

		public bool IsReplaceMode => _isReplaceMode;

		public int ReplaceTargetIndex => _replaceTargetIndex;

		public event EventHandler SequenceChanged;

		public event EventHandler UndoClicked;

		public event EventHandler ClearClicked;

		public event EventHandler SelectionChanged;

		public event EventHandler PreviewSelectionRequested;

		public event EventHandler InsertModeChanged;

		public event EventHandler ReplaceModeChanged;

		public event EventHandler ExpandRequested;

		public NoteSequencePanel(int width, int height)
			: this()
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Expected O, but got Unknown
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Expected O, but got Unknown
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Expected O, but got Unknown
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Expected O, but got Unknown
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Expected O, but got Unknown
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Expected O, but got Unknown
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Expected O, but got Unknown
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Expected O, but got Unknown
			((Control)this).set_Size(new Point(width, height));
			((Control)this).set_BackgroundColor(MaestroTheme.DarkCharcoal);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Notes: 0");
			((Control)val).set_Location(new Point(5, 4));
			((Control)val).set_Size(new Point(120, 22));
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.CreamWhite);
			_headerLabel = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("Clear");
			((Control)val2).set_Location(new Point(width - 65, 6));
			((Control)val2).set_Size(new Point(55, 26));
			_clearButton = val2;
			((Control)_clearButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.ClearClicked?.Invoke(this, EventArgs.Empty);
				Clear();
			});
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("Undo");
			((Control)val3).set_Location(new Point(width - 125, 6));
			((Control)val3).set_Size(new Point(55, 26));
			_undoButton = val3;
			((Control)_undoButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.UndoClicked?.Invoke(this, EventArgs.Empty);
				Undo();
			});
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Insert");
			((Control)val4).set_Location(new Point(width - 185, 6));
			((Control)val4).set_Size(new Point(55, 26));
			((Control)val4).set_BasicTooltipText("When active, new notes insert after the selected note instead of appending to the end");
			_insertButton = val4;
			((Control)_insertButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_isInsertMode = !_isInsertMode;
				UpdateInsertVisual();
				UpdateModeStatus();
				this.InsertModeChanged?.Invoke(this, EventArgs.Empty);
			});
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Expand");
			((Control)val5).set_Location(new Point(width - 255, 6));
			((Control)val5).set_Size(new Point(65, 26));
			((Control)val5).set_BasicTooltipText("Open notes in a resizable window");
			_expandButton = val5;
			((Control)_expandButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.ExpandRequested?.Invoke(this, EventArgs.Empty);
			});
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("");
			((Control)val6).set_Location(new Point(5, 28));
			((Control)val6).set_Size(new Point(width - 10, 16));
			val6.set_Font(GameService.Content.get_DefaultFont12());
			val6.set_TextColor(MaestroTheme.AmberGold);
			_modeStatusLabel = val6;
			FlowPanel val7 = new FlowPanel();
			((Control)val7).set_Parent((Container)(object)this);
			((Control)val7).set_Location(new Point(0, 48));
			((Control)val7).set_Size(new Point(width, height - 48));
			val7.set_FlowDirection((ControlFlowDirection)0);
			val7.set_ControlPadding(new Vector2(4f, 4f));
			val7.set_OuterControlPadding(new Vector2(5f, 5f));
			((Panel)val7).set_CanScroll(true);
			((Panel)val7).set_ShowBorder(false);
			((Control)val7).set_BackgroundColor(Color.get_Transparent());
			_chipsContainer = val7;
			_contextMenu = new ContextMenuStrip();
			_previewSelectedItem = _contextMenu.AddMenuItem("Preview Selected");
			((Control)_previewSelectedItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.PreviewSelectionRequested?.Invoke(this, EventArgs.Empty);
			});
			_deleteSelectedItem = _contextMenu.AddMenuItem("Delete Selected");
			((Control)_deleteSelectedItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RemoveSelected();
			});
			_replaceItem = _contextMenu.AddMenuItem("Replace");
			((Control)_replaceItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				EnterReplaceMode();
			});
			_copyItem = _contextMenu.AddMenuItem("Copy");
			((Control)_copyItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CopySelected();
			});
			_pasteItem = _contextMenu.AddMenuItem("Paste");
			((Control)_pasteItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				PasteClipboard();
			});
			_selectAllItem = _contextMenu.AddMenuItem("Select All");
			((Control)_selectAllItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SelectAll();
			});
			_clearSelectionItem = _contextMenu.AddMenuItem("Clear Selection");
			((Control)_clearSelectionItem).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ClearSelection();
			});
			((Control)_chipsContainer).set_Menu(_contextMenu);
			UpdateButtonStates();
		}

		public IReadOnlyList<string> GetSelectedNotes()
		{
			return (from i in _selectedIndices
				orderby i
				select _notes[i]).ToList();
		}

		public void ClearSelection()
		{
			foreach (int index in _selectedIndices)
			{
				if (index < _chips.Count)
				{
					_chips[index].IsSelected = false;
				}
			}
			_selectedIndices.Clear();
			_lastClickedIndex = -1;
			UpdateHeader();
			UpdateContextMenuStates();
			this.SelectionChanged?.Invoke(this, EventArgs.Empty);
		}

		public void SelectAll()
		{
			for (int i = 0; i < _chips.Count; i++)
			{
				_selectedIndices.Add(i);
				_chips[i].IsSelected = true;
			}
			UpdateHeader();
			UpdateContextMenuStates();
			this.SelectionChanged?.Invoke(this, EventArgs.Empty);
		}

		public void RemoveSelected()
		{
			if (_selectedIndices.Count == 0)
			{
				return;
			}
			PushUndo();
			foreach (int index in _selectedIndices.OrderByDescending((int i) => i))
			{
				_notes.RemoveAt(index);
				NoteChip noteChip = _chips[index];
				noteChip.ChipClicked -= OnChipClicked;
				noteChip.RemoveClicked -= OnChipRemoveClicked;
				((Control)noteChip).Dispose();
				_chips.RemoveAt(index);
			}
			_selectedIndices.Clear();
			_lastClickedIndex = -1;
			if (_isInsertMode)
			{
				ResetModes();
			}
			UpdateIndices();
			UpdateHeader();
			UpdateButtonStates();
			UpdateContextMenuStates();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
			this.SelectionChanged?.Invoke(this, EventArgs.Empty);
		}

		public void AddNote(string noteString)
		{
			PushUndo();
			_notes.Add(noteString);
			NoteChip noteChip = new NoteChip(noteString, _notes.Count - 1);
			((Control)noteChip).set_Parent((Container)(object)_chipsContainer);
			NoteChip chip = noteChip;
			chip.RemoveClicked += OnChipRemoveClicked;
			chip.ChipClicked += OnChipClicked;
			_chips.Add(chip);
			EnsureBottomSpacer();
			UpdateHeader();
			UpdateButtonStates();
			UpdateContextMenuStates();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
		}

		public void InsertAt(int index, string noteString)
		{
			PushUndo();
			index = Math.Max(0, Math.Min(index, _notes.Count));
			_notes.Insert(index, noteString);
			NoteChip chip = new NoteChip(noteString, index);
			chip.RemoveClicked += OnChipRemoveClicked;
			chip.ChipClicked += OnChipClicked;
			_chips.Insert(index, chip);
			HashSet<int> shifted = new HashSet<int>();
			foreach (int si2 in _selectedIndices)
			{
				shifted.Add((si2 >= index) ? (si2 + 1) : si2);
			}
			_selectedIndices.Clear();
			foreach (int si in shifted)
			{
				_selectedIndices.Add(si);
			}
			if (_lastClickedIndex >= index)
			{
				_lastClickedIndex++;
			}
			ReorderChipsFrom(index);
			UpdateIndices();
			EnsureBottomSpacer();
			UpdateHeader();
			UpdateButtonStates();
			UpdateContextMenuStates();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
		}

		public void InsertRange(int index, IList<string> noteStrings)
		{
			if (noteStrings.Count == 0)
			{
				return;
			}
			PushUndo();
			index = Math.Max(0, Math.Min(index, _notes.Count));
			for (int i = 0; i < noteStrings.Count; i++)
			{
				int insertIdx = index + i;
				_notes.Insert(insertIdx, noteStrings[i]);
				NoteChip chip = new NoteChip(noteStrings[i], insertIdx);
				chip.RemoveClicked += OnChipRemoveClicked;
				chip.ChipClicked += OnChipClicked;
				_chips.Insert(insertIdx, chip);
			}
			HashSet<int> shifted = new HashSet<int>();
			foreach (int si2 in _selectedIndices)
			{
				shifted.Add((si2 >= index) ? (si2 + noteStrings.Count) : si2);
			}
			_selectedIndices.Clear();
			foreach (int si in shifted)
			{
				_selectedIndices.Add(si);
			}
			if (_lastClickedIndex >= index)
			{
				_lastClickedIndex += noteStrings.Count;
			}
			ReorderChipsFrom(index);
			UpdateIndices();
			EnsureBottomSpacer();
			UpdateHeader();
			UpdateButtonStates();
			UpdateContextMenuStates();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
		}

		public void ReplaceAt(int index, string noteString)
		{
			if (index >= 0 && index < _notes.Count)
			{
				PushUndo();
				_notes[index] = noteString;
				NoteChip noteChip = _chips[index];
				noteChip.ChipClicked -= OnChipClicked;
				noteChip.RemoveClicked -= OnChipRemoveClicked;
				((Control)noteChip).set_Parent((Container)null);
				((Control)noteChip).Dispose();
				NoteChip newChip = new NoteChip(noteString, index);
				newChip.RemoveClicked += OnChipRemoveClicked;
				newChip.ChipClicked += OnChipClicked;
				_chips[index] = newChip;
				ReorderChipsFrom(index);
				newChip.IsSelected = _selectedIndices.Contains(index);
				UpdateHeader();
				this.SequenceChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void SelectSingle(int index)
		{
			if (index < 0 || index >= _chips.Count)
			{
				return;
			}
			foreach (int si2 in _selectedIndices.Where((int si) => si < _chips.Count))
			{
				_chips[si2].IsSelected = false;
			}
			_selectedIndices.Clear();
			_selectedIndices.Add(index);
			_chips[index].IsSelected = true;
			_lastClickedIndex = index;
			UpdateHeader();
			UpdateContextMenuStates();
			this.SelectionChanged?.Invoke(this, EventArgs.Empty);
		}

		public void SelectRange(int from, int to)
		{
			if (from < 0)
			{
				from = 0;
			}
			foreach (int si2 in _selectedIndices.Where((int si) => si < _chips.Count))
			{
				_chips[si2].IsSelected = false;
			}
			_selectedIndices.Clear();
			for (int i = from; i <= to && i < _chips.Count; i++)
			{
				_selectedIndices.Add(i);
				_chips[i].IsSelected = true;
			}
			_lastClickedIndex = ((to < _chips.Count) ? to : (-1));
			UpdateHeader();
			UpdateContextMenuStates();
			this.SelectionChanged?.Invoke(this, EventArgs.Empty);
		}

		public int GetLastSelectedIndex()
		{
			if (_selectedIndices.Count <= 0)
			{
				return -1;
			}
			return _selectedIndices.Max();
		}

		public void EnterReplaceMode()
		{
			if (_selectedIndices.Count == 1)
			{
				_isReplaceMode = true;
				_replaceTargetIndex = _selectedIndices.First();
				UpdateModeStatus();
				this.ReplaceModeChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void ExitReplaceMode()
		{
			if (_isReplaceMode)
			{
				_isReplaceMode = false;
				_replaceTargetIndex = -1;
				UpdateModeStatus();
				this.ReplaceModeChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void ResetModes()
		{
			if (_isInsertMode)
			{
				_isInsertMode = false;
				UpdateInsertVisual();
				this.InsertModeChanged?.Invoke(this, EventArgs.Empty);
			}
			ExitReplaceMode();
			UpdateModeStatus();
		}

		private void UpdateInsertVisual()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			((Control)_insertButton).set_BackgroundColor(_isInsertMode ? MaestroTheme.AmberGold : Color.get_Transparent());
		}

		public void CopySelected()
		{
			if (_selectedIndices.Count != 0)
			{
				_clipboard.Clear();
				_clipboard.AddRange(from i in _selectedIndices
					orderby i
					select _notes[i]);
				UpdateContextMenuStates();
			}
		}

		public void PasteClipboard()
		{
			if (_clipboard.Count != 0)
			{
				int insertIndex = ((_selectedIndices.Count > 0) ? (_selectedIndices.Max() + 1) : _notes.Count);
				InsertRange(insertIndex, _clipboard);
				SelectRange(insertIndex, insertIndex + _clipboard.Count - 1);
			}
		}

		public void ResizeTo(int width, int height)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(width, height));
			((Control)_expandButton).set_Location(new Point(width - 255, 6));
			((Control)_insertButton).set_Location(new Point(width - 185, 6));
			((Control)_undoButton).set_Location(new Point(width - 125, 6));
			((Control)_clearButton).set_Location(new Point(width - 65, 6));
			((Control)_modeStatusLabel).set_Size(new Point(width - 10, 16));
			int chipsY = 48;
			((Control)_chipsContainer).set_Location(new Point(0, chipsY));
			((Control)_chipsContainer).set_Size(new Point(width, height - chipsY));
		}

		public void SetExpanded(bool expanded)
		{
			_expandButton.set_Text(expanded ? "Collapse" : "Expand");
			((Control)_expandButton).set_BasicTooltipText(expanded ? "Return notes to the creator window" : "Open notes in a resizable window");
		}

		private void ReorderChipsFrom(int startIndex)
		{
			if (_bottomSpacer != null)
			{
				((Control)_bottomSpacer).set_Parent((Container)null);
			}
			for (int j = startIndex; j < _chips.Count; j++)
			{
				((Control)_chips[j]).set_Parent((Container)null);
			}
			for (int i = startIndex; i < _chips.Count; i++)
			{
				((Control)_chips[i]).set_Parent((Container)(object)_chipsContainer);
			}
			if (_bottomSpacer != null)
			{
				((Control)_bottomSpacer).set_Parent((Container)(object)_chipsContainer);
			}
		}

		private void PushUndo()
		{
			if (_undoStack.Count >= 100)
			{
				List<string>[] items = _undoStack.ToArray();
				_undoStack.Clear();
				for (int i = items.Length - 2; i >= 0; i--)
				{
					_undoStack.Push(items[i]);
				}
			}
			_undoStack.Push(new List<string>(_notes));
		}

		public void Undo()
		{
			if (_undoStack.Count != 0)
			{
				List<string> previousState = _undoStack.Pop();
				RestoreFromNotes(previousState);
				this.SequenceChanged?.Invoke(this, EventArgs.Empty);
				this.SelectionChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void ClearUndoStack()
		{
			_undoStack.Clear();
			UpdateButtonStates();
		}

		private void RestoreFromNotes(List<string> notes)
		{
			foreach (NoteChip chip2 in _chips)
			{
				chip2.ChipClicked -= OnChipClicked;
				chip2.RemoveClicked -= OnChipRemoveClicked;
				((Control)chip2).Dispose();
			}
			_chips.Clear();
			_notes.Clear();
			_selectedIndices.Clear();
			_lastClickedIndex = -1;
			Panel bottomSpacer = _bottomSpacer;
			if (bottomSpacer != null)
			{
				((Control)bottomSpacer).Dispose();
			}
			_bottomSpacer = null;
			foreach (string note in notes)
			{
				_notes.Add(note);
				NoteChip noteChip = new NoteChip(note, _notes.Count - 1);
				((Control)noteChip).set_Parent((Container)(object)_chipsContainer);
				NoteChip chip = noteChip;
				chip.RemoveClicked += OnChipRemoveClicked;
				chip.ChipClicked += OnChipClicked;
				_chips.Add(chip);
			}
			if (_notes.Count > 0)
			{
				EnsureBottomSpacer();
			}
			UpdateIndices();
			UpdateHeader();
			UpdateButtonStates();
			UpdateContextMenuStates();
		}

		public void RemoveAt(int index)
		{
			if (index < 0 || index >= _notes.Count)
			{
				return;
			}
			PushUndo();
			_notes.RemoveAt(index);
			NoteChip noteChip = _chips[index];
			noteChip.ChipClicked -= OnChipClicked;
			noteChip.RemoveClicked -= OnChipRemoveClicked;
			((Control)noteChip).Dispose();
			_chips.RemoveAt(index);
			_selectedIndices.Remove(index);
			HashSet<int> shifted = new HashSet<int>();
			foreach (int si2 in _selectedIndices)
			{
				shifted.Add((si2 > index) ? (si2 - 1) : si2);
			}
			_selectedIndices.Clear();
			foreach (int si in shifted)
			{
				_selectedIndices.Add(si);
			}
			if (_lastClickedIndex == index)
			{
				_lastClickedIndex = -1;
			}
			else if (_lastClickedIndex > index)
			{
				_lastClickedIndex--;
			}
			if (_isInsertMode && _selectedIndices.Count == 0)
			{
				ResetModes();
			}
			UpdateIndices();
			UpdateHeader();
			UpdateButtonStates();
			UpdateContextMenuStates();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
		}

		public void Clear()
		{
			PushUndo();
			_notes.Clear();
			_selectedIndices.Clear();
			_lastClickedIndex = -1;
			foreach (NoteChip chip in _chips)
			{
				chip.ChipClicked -= OnChipClicked;
				chip.RemoveClicked -= OnChipRemoveClicked;
				((Control)chip).Dispose();
			}
			_chips.Clear();
			Panel bottomSpacer = _bottomSpacer;
			if (bottomSpacer != null)
			{
				((Control)bottomSpacer).Dispose();
			}
			_bottomSpacer = null;
			UpdateHeader();
			UpdateButtonStates();
			UpdateContextMenuStates();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
		}

		private void OnChipClicked(object sender, MouseEventArgs e)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			NoteChip chip = sender as NoteChip;
			if (chip == null)
			{
				return;
			}
			int index = chip.Index;
			ModifierKeys modifiers = GameService.Input.get_Keyboard().get_ActiveModifiers();
			bool num = ((Enum)modifiers).HasFlag((Enum)(object)(ModifierKeys)4);
			bool isCtrl = ((Enum)modifiers).HasFlag((Enum)(object)(ModifierKeys)1);
			if (num && _lastClickedIndex >= 0)
			{
				int from = Math.Min(_lastClickedIndex, index);
				int to = Math.Max(_lastClickedIndex, index);
				if (!isCtrl)
				{
					foreach (int si3 in _selectedIndices)
					{
						if (si3 < _chips.Count)
						{
							_chips[si3].IsSelected = false;
						}
					}
					_selectedIndices.Clear();
				}
				for (int i = from; i <= to; i++)
				{
					_selectedIndices.Add(i);
					_chips[i].IsSelected = true;
				}
			}
			else if (isCtrl)
			{
				if (!_selectedIndices.Add(index))
				{
					_selectedIndices.Remove(index);
					chip.IsSelected = false;
				}
				else
				{
					chip.IsSelected = true;
				}
				_lastClickedIndex = index;
			}
			else
			{
				foreach (int si2 in _selectedIndices.Where((int si) => si < _chips.Count))
				{
					_chips[si2].IsSelected = false;
				}
				_selectedIndices.Clear();
				_selectedIndices.Add(index);
				chip.IsSelected = true;
				_lastClickedIndex = index;
			}
			if (_isReplaceMode && (_selectedIndices.Count != 1 || !_selectedIndices.Contains(_replaceTargetIndex)))
			{
				ExitReplaceMode();
			}
			UpdateHeader();
			UpdateContextMenuStates();
			this.SelectionChanged?.Invoke(this, EventArgs.Empty);
		}

		private void OnChipRemoveClicked(object sender, EventArgs e)
		{
			NoteChip chip = sender as NoteChip;
			if (chip != null)
			{
				RemoveAt(chip.Index);
			}
		}

		private void UpdateIndices()
		{
			for (int i = 0; i < _chips.Count; i++)
			{
				_chips[i].Index = i;
			}
		}

		private void UpdateHeader()
		{
			_headerLabel.set_Text((_selectedIndices.Count > 0) ? $"Notes: {_notes.Count} ({_selectedIndices.Count} selected)" : $"Notes: {_notes.Count}");
			UpdateModeStatus();
		}

		private void UpdateModeStatus()
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			if (_isReplaceMode)
			{
				_modeStatusLabel.set_Text($"Click a key to replace note #{_replaceTargetIndex + 1}");
				_modeStatusLabel.set_TextColor(MaestroTheme.Error);
			}
			else if (_isInsertMode && _selectedIndices.Count > 0)
			{
				_modeStatusLabel.set_Text($"Inserting after note #{_selectedIndices.Max() + 1}");
				_modeStatusLabel.set_TextColor(MaestroTheme.AmberGold);
			}
			else if (_isInsertMode)
			{
				_modeStatusLabel.set_Text("Select a note to insert after");
				_modeStatusLabel.set_TextColor(MaestroTheme.AmberGold);
			}
			else if (_chips.Count > 0)
			{
				_modeStatusLabel.set_Text("Right-click notes for more options");
				_modeStatusLabel.set_TextColor(MaestroTheme.LightGray);
			}
			else
			{
				_modeStatusLabel.set_Text("");
			}
		}

		private void UpdateButtonStates()
		{
			((Control)_undoButton).set_Enabled(_undoStack.Count > 0);
			((Control)_clearButton).set_Enabled(_notes.Count > 0);
		}

		private void UpdateContextMenuStates()
		{
			bool hasSelection = _selectedIndices.Count > 0;
			bool hasChips = _chips.Count > 0;
			((Control)_previewSelectedItem).set_Enabled(hasSelection);
			((Control)_deleteSelectedItem).set_Enabled(hasSelection);
			((Control)_replaceItem).set_Enabled(_selectedIndices.Count == 1);
			((Control)_copyItem).set_Enabled(hasSelection);
			((Control)_pasteItem).set_Enabled(_clipboard.Count > 0);
			((Control)_selectAllItem).set_Enabled(hasChips);
			((Control)_clearSelectionItem).set_Enabled(hasSelection);
		}

		private void EnsureBottomSpacer()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			Panel bottomSpacer = _bottomSpacer;
			if (bottomSpacer != null)
			{
				((Control)bottomSpacer).Dispose();
			}
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_chipsContainer);
			((Control)val).set_Size(new Point(((Control)_chipsContainer).get_Width(), 30));
			((Control)val).set_BackgroundColor(Color.get_Transparent());
			_bottomSpacer = val;
		}

		protected override void DisposeControl()
		{
			Label headerLabel = _headerLabel;
			if (headerLabel != null)
			{
				((Control)headerLabel).Dispose();
			}
			Label modeStatusLabel = _modeStatusLabel;
			if (modeStatusLabel != null)
			{
				((Control)modeStatusLabel).Dispose();
			}
			StandardButton expandButton = _expandButton;
			if (expandButton != null)
			{
				((Control)expandButton).Dispose();
			}
			StandardButton insertButton = _insertButton;
			if (insertButton != null)
			{
				((Control)insertButton).Dispose();
			}
			StandardButton undoButton = _undoButton;
			if (undoButton != null)
			{
				((Control)undoButton).Dispose();
			}
			StandardButton clearButton = _clearButton;
			if (clearButton != null)
			{
				((Control)clearButton).Dispose();
			}
			Panel bottomSpacer = _bottomSpacer;
			if (bottomSpacer != null)
			{
				((Control)bottomSpacer).Dispose();
			}
			foreach (NoteChip chip in _chips)
			{
				chip.ChipClicked -= OnChipClicked;
				chip.RemoveClicked -= OnChipRemoveClicked;
				((Control)chip).Dispose();
			}
			_chips.Clear();
			ContextMenuStrip contextMenu = _contextMenu;
			if (contextMenu != null)
			{
				((Control)contextMenu).Dispose();
			}
			FlowPanel chipsContainer = _chipsContainer;
			if (chipsContainer != null)
			{
				((Control)chipsContainer).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
