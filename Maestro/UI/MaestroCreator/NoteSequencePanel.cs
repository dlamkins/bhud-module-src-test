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

		private readonly ContextMenuStrip _contextMenu;

		private readonly ContextMenuStripItem _previewSelectedItem;

		private readonly ContextMenuStripItem _deleteSelectedItem;

		private readonly ContextMenuStripItem _selectAllItem;

		private readonly ContextMenuStripItem _clearSelectionItem;

		private readonly Label _headerLabel;

		private readonly StandardButton _undoButton;

		private readonly StandardButton _clearButton;

		private readonly FlowPanel _chipsContainer;

		private Panel _bottomSpacer;

		public IReadOnlyList<string> Notes => _notes.AsReadOnly();

		public int NoteCount => _notes.Count;

		public bool HasSelection => _selectedIndices.Count > 0;

		public event EventHandler SequenceChanged;

		public event EventHandler UndoClicked;

		public event EventHandler ClearClicked;

		public event EventHandler SelectionChanged;

		public event EventHandler PreviewSelectionRequested;

		public NoteSequencePanel(int width, int height)
			: this()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Expected O, but got Unknown
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Expected O, but got Unknown
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Expected O, but got Unknown
			((Control)this).set_Size(new Point(width, height));
			((Control)this).set_BackgroundColor(MaestroTheme.DarkCharcoal);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Notes: 0");
			((Control)val).set_Location(new Point(5, 4));
			((Control)val).set_Size(new Point(200, 22));
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
				RemoveLast();
			});
			int chipsY = 38;
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Location(new Point(0, chipsY));
			((Control)val4).set_Size(new Point(width, height - chipsY));
			val4.set_FlowDirection((ControlFlowDirection)0);
			val4.set_ControlPadding(new Vector2(4f, 4f));
			val4.set_OuterControlPadding(new Vector2(5f, 5f));
			((Panel)val4).set_CanScroll(true);
			((Panel)val4).set_ShowBorder(false);
			((Control)val4).set_BackgroundColor(Color.get_Transparent());
			_chipsContainer = val4;
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
			UpdateIndices();
			UpdateHeader();
			UpdateButtonStates();
			UpdateContextMenuStates();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
			this.SelectionChanged?.Invoke(this, EventArgs.Empty);
		}

		public void AddNote(string noteString)
		{
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

		public void RemoveLast()
		{
			if (_notes.Count != 0)
			{
				int lastIndex = _notes.Count - 1;
				_selectedIndices.Remove(lastIndex);
				if (_lastClickedIndex == lastIndex)
				{
					_lastClickedIndex = -1;
				}
				_notes.RemoveAt(lastIndex);
				NoteChip noteChip = _chips[lastIndex];
				noteChip.ChipClicked -= OnChipClicked;
				noteChip.RemoveClicked -= OnChipRemoveClicked;
				((Control)noteChip).Dispose();
				_chips.RemoveAt(lastIndex);
				UpdateIndices();
				UpdateHeader();
				UpdateButtonStates();
				UpdateContextMenuStates();
				this.SequenceChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void RemoveAt(int index)
		{
			if (index < 0 || index >= _notes.Count)
			{
				return;
			}
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
			UpdateIndices();
			UpdateHeader();
			UpdateButtonStates();
			UpdateContextMenuStates();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
		}

		public void Clear()
		{
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
		}

		private void UpdateButtonStates()
		{
			bool hasNotes = _notes.Count > 0;
			((Control)_undoButton).set_Enabled(hasNotes);
			((Control)_clearButton).set_Enabled(hasNotes);
		}

		private void UpdateContextMenuStates()
		{
			bool hasSelection = _selectedIndices.Count > 0;
			bool hasChips = _chips.Count > 0;
			((Control)_previewSelectedItem).set_Enabled(hasSelection);
			((Control)_deleteSelectedItem).set_Enabled(hasSelection);
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
