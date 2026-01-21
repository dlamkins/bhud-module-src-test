using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

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

			public const int ButtonY = 6;
		}

		private readonly List<string> _notes = new List<string>();

		private readonly List<NoteChip> _chips = new List<NoteChip>();

		private readonly Label _headerLabel;

		private readonly StandardButton _undoButton;

		private readonly StandardButton _clearButton;

		private readonly FlowPanel _chipsContainer;

		public IReadOnlyList<string> Notes => _notes.AsReadOnly();

		public int NoteCount => _notes.Count;

		public event EventHandler SequenceChanged;

		public event EventHandler UndoClicked;

		public event EventHandler ClearClicked;

		public NoteSequencePanel(int width, int height)
			: this()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Expected O, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Expected O, but got Unknown
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Expected O, but got Unknown
			((Control)this).set_Size(new Point(width, height));
			((Control)this).set_BackgroundColor(MaestroTheme.DarkCharcoal);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Notes: 0");
			((Control)val).set_Location(new Point(5, 4));
			((Control)val).set_Size(new Point(100, 22));
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
			UpdateButtonStates();
		}

		public void AddNote(string noteString)
		{
			_notes.Add(noteString);
			NoteChip noteChip = new NoteChip(noteString, _notes.Count - 1);
			((Control)noteChip).set_Parent((Container)(object)_chipsContainer);
			NoteChip chip = noteChip;
			chip.RemoveClicked += OnChipRemoveClicked;
			_chips.Add(chip);
			UpdateHeader();
			UpdateButtonStates();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
		}

		public void RemoveLast()
		{
			if (_notes.Count != 0)
			{
				_notes.RemoveAt(_notes.Count - 1);
				NoteChip noteChip = _chips[_chips.Count - 1];
				noteChip.RemoveClicked -= OnChipRemoveClicked;
				((Control)noteChip).Dispose();
				_chips.RemoveAt(_chips.Count - 1);
				UpdateIndices();
				UpdateHeader();
				UpdateButtonStates();
				this.SequenceChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void RemoveAt(int index)
		{
			if (index >= 0 && index < _notes.Count)
			{
				_notes.RemoveAt(index);
				NoteChip noteChip = _chips[index];
				noteChip.RemoveClicked -= OnChipRemoveClicked;
				((Control)noteChip).Dispose();
				_chips.RemoveAt(index);
				UpdateIndices();
				UpdateHeader();
				UpdateButtonStates();
				this.SequenceChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public void Clear()
		{
			_notes.Clear();
			foreach (NoteChip chip in _chips)
			{
				chip.RemoveClicked -= OnChipRemoveClicked;
				((Control)chip).Dispose();
			}
			_chips.Clear();
			UpdateHeader();
			UpdateButtonStates();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
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
			_headerLabel.set_Text($"Notes: {_notes.Count}");
		}

		private void UpdateButtonStates()
		{
			bool hasNotes = _notes.Count > 0;
			((Control)_undoButton).set_Enabled(hasNotes);
			((Control)_clearButton).set_Enabled(hasNotes);
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
			foreach (NoteChip chip in _chips)
			{
				chip.RemoveClicked -= OnChipRemoveClicked;
				((Control)chip).Dispose();
			}
			_chips.Clear();
			FlowPanel chipsContainer = _chipsContainer;
			if (chipsContainer != null)
			{
				((Control)chipsContainer).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
