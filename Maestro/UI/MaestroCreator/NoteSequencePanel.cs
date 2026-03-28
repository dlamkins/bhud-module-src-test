using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Services.Playback;
using Maestro.UI.Controls;
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

		private readonly List<BaseChip> _chips = new List<BaseChip>();

		private readonly HashSet<int> _selectedIndices = new HashSet<int>();

		private int _lastClickedIndex = -1;

		private bool _isInsertMode;

		private bool _isReplaceMode;

		private int _replaceTargetIndex = -1;

		private readonly List<string> _clipboard = new List<string>();

		private readonly Stack<List<string>> _undoStack = new Stack<List<string>>();

		private int _pendingScrollToIndex = -1;

		private int _scrollApplyFrames;

		private Scrollbar _scrollbarRef;

		private static readonly FieldInfo PanelScrollbarField = typeof(Panel).GetField("_panelScrollbar", BindingFlags.Instance | BindingFlags.NonPublic);

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

		private readonly StandardButton _insertButton;

		private readonly StandardButton _previewAllButton;

		private readonly StandardButton _previewSelectedButton;

		private readonly StandardButton _pauseButton;

		private readonly StandardButton _stopButton;

		private readonly Label _playbackStatusLabel;

		private bool _suppressSectionScroll;

		private readonly StandardButton _undoButton;

		private readonly StandardButton _clearButton;

		private readonly Panel _headerPanel;

		private readonly Panel _footerPanel;

		private readonly FlowPanel _chipsContainer;

		private Panel _bottomSpacer;

		private readonly StandardButton _sectionButton;

		private readonly ContextMenuStrip _sectionMenu;

		private readonly CustomDropdown _sectionJumpDropdown;

		private TextBox _customSectionInput;

		private int _playbackHighlightIndex = -1;

		private HashSet<int> _savedSelection;

		private bool _isPlaybackActive;

		private int[] _playbackMapping;

		private int[] _playbackNoteIndices;

		private SongPlayer _activeSongPlayer;

		private const int MaxUndoDepth = 100;

		public IReadOnlyList<string> Notes => _notes.AsReadOnly();

		public int NoteCount => _notes.Count((string n) => !IsSectionMarker(n));

		public bool HasSelection => _selectedIndices.Count > 0;

		public bool IsInsertMode => _isInsertMode;

		public bool IsReplaceMode => _isReplaceMode;

		public int ReplaceTargetIndex => _replaceTargetIndex;

		public event EventHandler SequenceChanged;

		public event EventHandler UndoClicked;

		public event EventHandler ClearClicked;

		public event EventHandler SelectionChanged;

		public event EventHandler PreviewAllRequested;

		public event EventHandler PreviewSelectionRequested;

		public event EventHandler PauseRequested;

		public event EventHandler StopRequested;

		public event EventHandler InsertModeChanged;

		public event EventHandler ReplaceModeChanged;

		private Scrollbar GetScrollbar()
		{
			if (_scrollbarRef != null && ((Control)_scrollbarRef).get_Parent() != null)
			{
				return _scrollbarRef;
			}
			ref Scrollbar scrollbarRef = ref _scrollbarRef;
			object obj = PanelScrollbarField?.GetValue(_chipsContainer);
			scrollbarRef = (Scrollbar)((obj is Scrollbar) ? obj : null);
			return _scrollbarRef;
		}

		private void RequestScrollTo(int chipIndex)
		{
			_pendingScrollToIndex = chipIndex;
			_scrollApplyFrames = 5;
		}

		public static bool IsSectionMarker(string s)
		{
			if (s != null && s.StartsWith("["))
			{
				return s.EndsWith("]");
			}
			return false;
		}

		public static string GetSectionName(string s)
		{
			return s.Substring(1, s.Length - 2);
		}

		public NoteSequencePanel(int width, int height)
			: this()
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Expected O, but got Unknown
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Expected O, but got Unknown
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Expected O, but got Unknown
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Expected O, but got Unknown
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Expected O, but got Unknown
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Expected O, but got Unknown
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0426: Unknown result type (might be due to invalid IL or missing references)
			//IL_042a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0434: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_0449: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_045e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0469: Unknown result type (might be due to invalid IL or missing references)
			//IL_0473: Unknown result type (might be due to invalid IL or missing references)
			//IL_047a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_0482: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Expected O, but got Unknown
			//IL_0492: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Expected O, but got Unknown
			//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0605: Unknown result type (might be due to invalid IL or missing references)
			//IL_0609: Unknown result type (might be due to invalid IL or missing references)
			//IL_0613: Unknown result type (might be due to invalid IL or missing references)
			//IL_0614: Unknown result type (might be due to invalid IL or missing references)
			//IL_0623: Expected O, but got Unknown
			//IL_0624: Unknown result type (might be due to invalid IL or missing references)
			//IL_0629: Unknown result type (might be due to invalid IL or missing references)
			//IL_0635: Unknown result type (might be due to invalid IL or missing references)
			//IL_0640: Unknown result type (might be due to invalid IL or missing references)
			//IL_0643: Unknown result type (might be due to invalid IL or missing references)
			//IL_064d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0652: Unknown result type (might be due to invalid IL or missing references)
			//IL_065c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0668: Expected O, but got Unknown
			//IL_0680: Unknown result type (might be due to invalid IL or missing references)
			//IL_0685: Unknown result type (might be due to invalid IL or missing references)
			//IL_0691: Unknown result type (might be due to invalid IL or missing references)
			//IL_069c: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_06af: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c5: Expected O, but got Unknown
			//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0707: Unknown result type (might be due to invalid IL or missing references)
			//IL_070f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0719: Unknown result type (might be due to invalid IL or missing references)
			//IL_0729: Unknown result type (might be due to invalid IL or missing references)
			//IL_072a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0734: Unknown result type (might be due to invalid IL or missing references)
			//IL_0740: Expected O, but got Unknown
			//IL_0741: Unknown result type (might be due to invalid IL or missing references)
			//IL_0746: Unknown result type (might be due to invalid IL or missing references)
			//IL_0752: Unknown result type (might be due to invalid IL or missing references)
			//IL_075d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0763: Unknown result type (might be due to invalid IL or missing references)
			//IL_076d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0772: Unknown result type (might be due to invalid IL or missing references)
			//IL_077c: Unknown result type (might be due to invalid IL or missing references)
			//IL_078c: Expected O, but got Unknown
			//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f9: Expected O, but got Unknown
			((Control)this).set_Size(new Point(width, height));
			((Control)this).set_BackgroundColor(Color.get_Transparent());
			int headerHeight = 84;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(width, headerHeight));
			((Control)val).set_BackgroundColor(MaestroTheme.SlateGray);
			val.set_ShowBorder(true);
			_headerPanel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_headerPanel);
			val2.set_Text("Notes: 0");
			((Control)val2).set_Location(new Point(5, 4));
			((Control)val2).set_Size(new Point(140, 22));
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(MaestroTheme.CreamWhite);
			_headerLabel = val2;
			int clearX = width - 10 - 55;
			int undoX = clearX - 5 - 55;
			int insertX = undoX - 5 - 55;
			int sectionX = insertX - 5 - 60;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)_headerPanel);
			val3.set_Text("Clear");
			((Control)val3).set_Location(new Point(clearX, 6));
			((Control)val3).set_Size(new Point(55, 26));
			_clearButton = val3;
			((Control)_clearButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.ClearClicked?.Invoke(this, EventArgs.Empty);
				Clear();
			});
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)_headerPanel);
			val4.set_Text("Undo");
			((Control)val4).set_Location(new Point(undoX, 6));
			((Control)val4).set_Size(new Point(55, 26));
			_undoButton = val4;
			((Control)_undoButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.UndoClicked?.Invoke(this, EventArgs.Empty);
				Undo();
			});
			_sectionMenu = new ContextMenuStrip();
			string[] array = new string[5] { "Intro", "Verse", "Chorus", "Bridge", "Outro" };
			foreach (string name in array)
			{
				ContextMenuStripItem obj = _sectionMenu.AddMenuItem(name);
				string captured = name;
				((Control)obj).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					AddSectionMarker(captured);
				});
			}
			((Control)_sectionMenu.AddMenuItem("Custom...")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ShowCustomSectionInput();
			});
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)_headerPanel);
			val5.set_Text("Section");
			((Control)val5).set_Location(new Point(sectionX, 6));
			((Control)val5).set_Size(new Point(60, 26));
			((Control)val5).set_BasicTooltipText("Add a section marker to organize notes");
			_sectionButton = val5;
			((Control)_sectionButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_sectionMenu.Show((Control)(object)_sectionButton);
			});
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)_headerPanel);
			val6.set_Text("Insert");
			((Control)val6).set_Location(new Point(insertX, 6));
			((Control)val6).set_Size(new Point(55, 26));
			((Control)val6).set_BasicTooltipText("When active, new notes insert after the selected note instead of appending to the end");
			_insertButton = val6;
			((Control)_insertButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_isInsertMode = !_isInsertMode;
				UpdateInsertVisual();
				UpdateModeStatus();
				this.InsertModeChanged?.Invoke(this, EventArgs.Empty);
			});
			CustomDropdown customDropdown = new CustomDropdown();
			((Control)customDropdown).set_Parent((Container)(object)this);
			((Control)customDropdown).set_Location(new Point(5, 44));
			((Control)customDropdown).set_Size(new Point(150, 27));
			((Control)customDropdown).set_Visible(false);
			((Control)customDropdown).set_BasicTooltipText("Jump to section");
			_sectionJumpDropdown = customDropdown;
			_sectionJumpDropdown.ValueChanged += OnSectionJumpChanged;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)_headerPanel);
			val7.set_Text("");
			((Control)val7).set_Location(new Point(0, 45));
			((Control)val7).set_Size(new Point(width - 10, 18));
			val7.set_Font(GameService.Content.get_DefaultFont14());
			val7.set_TextColor(MaestroTheme.AmberGold);
			val7.set_HorizontalAlignment((HorizontalAlignment)2);
			val7.set_VerticalAlignment((VerticalAlignment)2);
			_modeStatusLabel = val7;
			int chipsY = headerHeight + 4;
			int chipsHeight = height - chipsY - 46 - 4;
			FlowPanel val8 = new FlowPanel();
			((Control)val8).set_Parent((Container)(object)this);
			((Control)val8).set_Location(new Point(0, chipsY));
			((Control)val8).set_Size(new Point(width, chipsHeight));
			val8.set_FlowDirection((ControlFlowDirection)0);
			val8.set_ControlPadding(new Vector2(4f, 4f));
			val8.set_OuterControlPadding(new Vector2(5f, 5f));
			((Panel)val8).set_CanScroll(true);
			((Panel)val8).set_ShowBorder(false);
			((Control)val8).set_BackgroundColor(Color.get_Transparent());
			_chipsContainer = val8;
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
			Panel val9 = new Panel();
			((Control)val9).set_Parent((Container)(object)this);
			((Control)val9).set_Location(new Point(0, height - 46));
			((Control)val9).set_Size(new Point(width, 46));
			((Control)val9).set_BackgroundColor(Color.get_Transparent());
			_footerPanel = val9;
			StandardButton val10 = new StandardButton();
			((Control)val10).set_Parent((Container)(object)_footerPanel);
			val10.set_Text("||");
			((Control)val10).set_Location(new Point(5, 6));
			((Control)val10).set_Size(new Point(30, 26));
			((Control)val10).set_Enabled(false);
			_pauseButton = val10;
			((Control)_pauseButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.PauseRequested?.Invoke(this, EventArgs.Empty);
			});
			StandardButton val11 = new StandardButton();
			((Control)val11).set_Parent((Container)(object)_footerPanel);
			val11.set_Text("X");
			((Control)val11).set_Location(new Point(40, 6));
			((Control)val11).set_Size(new Point(30, 26));
			((Control)val11).set_Enabled(false);
			_stopButton = val11;
			((Control)_stopButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.StopRequested?.Invoke(this, EventArgs.Empty);
			});
			Label val12 = new Label();
			((Control)val12).set_Parent((Container)(object)_footerPanel);
			val12.set_Text("No song playing");
			((Control)val12).set_Location(new Point(77, 6));
			((Control)val12).set_Size(new Point(200, 26));
			val12.set_Font(GameService.Content.get_DefaultFont14());
			val12.set_TextColor(MaestroTheme.LightGray);
			val12.set_VerticalAlignment((VerticalAlignment)1);
			_playbackStatusLabel = val12;
			StandardButton val13 = new StandardButton();
			((Control)val13).set_Parent((Container)(object)_footerPanel);
			val13.set_Text("Preview All");
			((Control)val13).set_Location(new Point(width - 95, 6));
			((Control)val13).set_Size(new Point(85, 26));
			((Control)val13).set_BasicTooltipText("Preview all notes");
			_previewAllButton = val13;
			((Control)_previewAllButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.PreviewAllRequested?.Invoke(this, EventArgs.Empty);
			});
			StandardButton val14 = new StandardButton();
			((Control)val14).set_Parent((Container)(object)_footerPanel);
			val14.set_Text("Preview Selected");
			((Control)val14).set_Location(new Point(width - 220, 6));
			((Control)val14).set_Size(new Point(120, 26));
			((Control)val14).set_Enabled(false);
			((Control)val14).set_BasicTooltipText("Preview selected notes");
			_previewSelectedButton = val14;
			((Control)_previewSelectedButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.PreviewSelectionRequested?.Invoke(this, EventArgs.Empty);
			});
			UpdateButtonStates();
		}

		public IReadOnlyList<string> GetSelectedNotes()
		{
			return (from i in _selectedIndices
				orderby i
				select _notes[i]).ToList();
		}

		public IReadOnlyList<int> GetSelectedIndices()
		{
			return _selectedIndices.OrderBy((int i) => i).ToList();
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
				if (!(_chips[i] is SectionMarkerChip))
				{
					_selectedIndices.Add(i);
					_chips[i].IsSelected = true;
				}
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
				BaseChip baseChip = _chips[index];
				baseChip.ChipClicked -= OnChipClicked;
				baseChip.RemoveClicked -= OnChipRemoveClicked;
				((Control)baseChip).Dispose();
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
			UpdateSectionDropdown();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
			this.SelectionChanged?.Invoke(this, EventArgs.Empty);
		}

		public void AddNote(string noteString)
		{
			PushUndo();
			_notes.Add(noteString);
			BaseChip chip = CreateChip(noteString, _notes.Count - 1);
			((Control)chip).set_Parent((Container)(object)_chipsContainer);
			chip.RemoveClicked += OnChipRemoveClicked;
			chip.ChipClicked += OnChipClicked;
			_chips.Add(chip);
			EnsureBottomSpacer();
			RequestScrollTo(_notes.Count - 1);
			UpdateHeader();
			UpdateButtonStates();
			UpdateContextMenuStates();
			UpdateSectionDropdown();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
		}

		public void InsertAt(int index, string noteString)
		{
			PushUndo();
			index = Math.Max(0, Math.Min(index, _notes.Count));
			_notes.Insert(index, noteString);
			BaseChip chip = CreateChip(noteString, index);
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
			RequestScrollTo(index);
			UpdateHeader();
			UpdateButtonStates();
			UpdateContextMenuStates();
			UpdateSectionDropdown();
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
				BaseChip chip = CreateChip(noteStrings[i], insertIdx);
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
			RequestScrollTo(index);
			UpdateHeader();
			UpdateButtonStates();
			UpdateContextMenuStates();
			UpdateSectionDropdown();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
		}

		public void ReplaceAt(int index, string noteString)
		{
			if (index >= 0 && index < _notes.Count)
			{
				PushUndo();
				_notes[index] = noteString;
				BaseChip baseChip = _chips[index];
				baseChip.ChipClicked -= OnChipClicked;
				baseChip.RemoveClicked -= OnChipRemoveClicked;
				((Control)baseChip).set_Parent((Container)null);
				((Control)baseChip).Dispose();
				BaseChip newChip = CreateChip(noteString, index);
				newChip.RemoveClicked += OnChipRemoveClicked;
				newChip.ChipClicked += OnChipClicked;
				_chips[index] = newChip;
				ReorderChipsFrom(index);
				newChip.IsSelected = _selectedIndices.Contains(index);
				RequestScrollTo(index);
				UpdateHeader();
				UpdateSectionDropdown();
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
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(width, height));
			int headerHeight = 84;
			((Control)_headerPanel).set_Size(new Point(width, headerHeight));
			int clearX = width - 10 - 55;
			int undoX = clearX - 5 - 55;
			int insertX = undoX - 5 - 55;
			int sectionX = insertX - 5 - 60;
			((Control)_clearButton).set_Location(new Point(clearX, 6));
			((Control)_undoButton).set_Location(new Point(undoX, 6));
			((Control)_insertButton).set_Location(new Point(insertX, 6));
			((Control)_sectionButton).set_Location(new Point(sectionX, 6));
			((Control)_modeStatusLabel).set_Size(new Point(width - 10, 18));
			int chipsY = headerHeight + 4;
			int chipsHeight = height - chipsY - 46 - 4;
			((Control)_chipsContainer).set_Location(new Point(0, chipsY));
			((Control)_chipsContainer).set_Size(new Point(width, chipsHeight));
			((Control)_footerPanel).set_Location(new Point(0, height - 46));
			((Control)_footerPanel).set_Size(new Point(width, 46));
			((Control)_previewAllButton).set_Location(new Point(width - 95, ((Control)_previewAllButton).get_Location().Y));
			((Control)_previewSelectedButton).set_Location(new Point(width - 220, ((Control)_previewSelectedButton).get_Location().Y));
			foreach (BaseChip chip in _chips)
			{
				if (chip is SectionMarkerChip)
				{
					((Control)chip).set_Size(new Point(width - 26, 26));
				}
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
			if (_pendingScrollToIndex >= 0 && _scrollApplyFrames > 0)
			{
				_scrollApplyFrames--;
				ScrollToChip(_pendingScrollToIndex);
				if (_scrollApplyFrames <= 0)
				{
					_pendingScrollToIndex = -1;
				}
			}
			if (_isPlaybackActive)
			{
				UpdatePlaybackHighlight();
			}
		}

		private void ScrollToChip(int chipIndex)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			if (chipIndex < 0 || chipIndex >= _chips.Count)
			{
				return;
			}
			Scrollbar scrollbar = GetScrollbar();
			if (scrollbar == null)
			{
				return;
			}
			BaseChip chip = _chips[chipIndex];
			int chipBottom = ((Control)chip).get_Location().Y + ((Control)chip).get_Height();
			int viewportHeight = ((Container)_chipsContainer).get_ContentRegion().Height;
			int contentHeight = 0;
			foreach (Control child in ((Container)_chipsContainer).get_Children())
			{
				if (child.get_Visible() && child.get_Bottom() > contentHeight)
				{
					contentHeight = child.get_Bottom();
				}
			}
			contentHeight = Math.Max(contentHeight, viewportHeight);
			int scrollableRange = contentHeight - viewportHeight;
			if (scrollableRange > 0)
			{
				int targetOffset = chipBottom - viewportHeight + 30;
				targetOffset = Math.Max(0, Math.Min(targetOffset, scrollableRange));
				float scrollDistance = (float)targetOffset / (float)scrollableRange;
				scrollbar.set_ScrollDistance(Math.Max(0f, Math.Min(1f, scrollDistance)));
			}
		}

		private void ReorderChipsFrom(int startIndex)
		{
			Scrollbar scrollbar = GetScrollbar();
			float savedDistance = ((scrollbar != null) ? scrollbar.get_ScrollDistance() : 0f);
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
			if (scrollbar != null)
			{
				scrollbar.set_ScrollDistance(savedDistance);
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
				UpdateSectionDropdown();
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
			foreach (BaseChip chip2 in _chips)
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
				BaseChip chip = CreateChip(note, _notes.Count - 1);
				((Control)chip).set_Parent((Container)(object)_chipsContainer);
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
			BaseChip baseChip = _chips[index];
			baseChip.ChipClicked -= OnChipClicked;
			baseChip.RemoveClicked -= OnChipRemoveClicked;
			((Control)baseChip).Dispose();
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
			UpdateSectionDropdown();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
		}

		public void Clear()
		{
			PushUndo();
			_notes.Clear();
			_selectedIndices.Clear();
			_lastClickedIndex = -1;
			foreach (BaseChip chip in _chips)
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
			UpdateSectionDropdown();
			this.SequenceChanged?.Invoke(this, EventArgs.Empty);
		}

		private void OnChipClicked(object sender, MouseEventArgs e)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			BaseChip chip = sender as BaseChip;
			if (chip == null || chip is SectionMarkerChip)
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
					if (!(_chips[i] is SectionMarkerChip))
					{
						_selectedIndices.Add(i);
						_chips[i].IsSelected = true;
					}
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
				bool wasOnlySelected = _selectedIndices.Count == 1 && _selectedIndices.Contains(index);
				foreach (int si2 in _selectedIndices.Where((int si) => si < _chips.Count))
				{
					_chips[si2].IsSelected = false;
				}
				_selectedIndices.Clear();
				if (!wasOnlySelected)
				{
					_selectedIndices.Add(index);
					chip.IsSelected = true;
					_lastClickedIndex = index;
				}
				else
				{
					_lastClickedIndex = -1;
				}
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
			BaseChip chip = sender as BaseChip;
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
			int noteCount = NoteCount;
			_headerLabel.set_Text((_selectedIndices.Count > 0) ? $"Notes: {noteCount} ({_selectedIndices.Count} selected)" : $"Notes: {noteCount}");
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
			((Control)_previewSelectedButton).set_Enabled(hasSelection);
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

		public void SetControlsEnabled(bool enabled)
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			((Control)_previewAllButton).set_Enabled(enabled);
			((Control)_previewSelectedButton).set_Enabled(enabled && _selectedIndices.Count > 0);
			((Control)_pauseButton).set_Enabled(!enabled);
			((Control)_stopButton).set_Enabled(!enabled);
			if (!enabled)
			{
				_playbackStatusLabel.set_Text("Playing...");
				_playbackStatusLabel.set_TextColor(MaestroTheme.Playing);
				_pauseButton.set_Text("||");
			}
			else
			{
				_playbackStatusLabel.set_Text("No song playing");
				_playbackStatusLabel.set_TextColor(MaestroTheme.LightGray);
				_pauseButton.set_Text("||");
			}
			((Control)_sectionButton).set_Enabled(enabled);
			((Control)_insertButton).set_Enabled(enabled);
			((Control)_undoButton).set_Enabled(enabled);
			((Control)_clearButton).set_Enabled(enabled);
			((Control)_previewSelectedItem).set_Enabled(enabled && _selectedIndices.Count > 0);
			((Control)_deleteSelectedItem).set_Enabled(enabled && _selectedIndices.Count > 0);
			((Control)_replaceItem).set_Enabled(enabled && _selectedIndices.Count == 1);
			((Control)_copyItem).set_Enabled(enabled && _selectedIndices.Count > 0);
			((Control)_pasteItem).set_Enabled(enabled && _clipboard.Count > 0);
			((Control)_selectAllItem).set_Enabled(enabled);
			((Control)_clearSelectionItem).set_Enabled(enabled && _selectedIndices.Count > 0);
		}

		public void SetPlaybackPaused(bool paused)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			_pauseButton.set_Text(paused ? ">" : "||");
			_playbackStatusLabel.set_Text(paused ? "Paused" : "Playing...");
			_playbackStatusLabel.set_TextColor(paused ? MaestroTheme.Paused : MaestroTheme.Playing);
		}

		public void StartPlaybackHighlight(SongPlayer player, int[] mapping, int[] noteIndices)
		{
			_savedSelection = new HashSet<int>(_selectedIndices);
			foreach (int si in _selectedIndices)
			{
				if (si < _chips.Count)
				{
					_chips[si].IsSelected = false;
				}
			}
			_selectedIndices.Clear();
			_playbackHighlightIndex = -1;
			_playbackMapping = mapping;
			_playbackNoteIndices = noteIndices;
			_activeSongPlayer = player;
			_isPlaybackActive = true;
		}

		public void StopPlaybackHighlight()
		{
			_isPlaybackActive = false;
			_playbackMapping = null;
			_playbackNoteIndices = null;
			_activeSongPlayer = null;
			if (_playbackHighlightIndex >= 0 && _playbackHighlightIndex < _chips.Count)
			{
				_chips[_playbackHighlightIndex].IsSelected = false;
			}
			_playbackHighlightIndex = -1;
			if (_savedSelection != null)
			{
				foreach (int si in _savedSelection)
				{
					if (si < _chips.Count)
					{
						_selectedIndices.Add(si);
						_chips[si].IsSelected = true;
					}
				}
				_savedSelection = null;
			}
			UpdateHeader();
			UpdateContextMenuStates();
		}

		private void UpdatePlaybackHighlight()
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			if (_activeSongPlayer == null || !_activeSongPlayer.IsPlaying)
			{
				return;
			}
			SongPlayer player = _activeSongPlayer;
			if (player.IsAdjustingOctave)
			{
				if (_playbackStatusLabel.get_Text() != "Adjusting...")
				{
					_playbackStatusLabel.set_Text("Adjusting...");
					_playbackStatusLabel.set_TextColor(MaestroTheme.AmberGold);
				}
			}
			else
			{
				if (player.IsPaused)
				{
					return;
				}
				if (_playbackStatusLabel.get_Text() != "Playing...")
				{
					_playbackStatusLabel.set_Text("Playing...");
					_playbackStatusLabel.set_TextColor(MaestroTheme.Playing);
				}
				int cmdIndex = player.CurrentCommandIndex;
				if (_playbackMapping == null || cmdIndex >= _playbackMapping.Length)
				{
					return;
				}
				int noteLineIndex = _playbackMapping[cmdIndex];
				if (_playbackNoteIndices != null && noteLineIndex < _playbackNoteIndices.Length)
				{
					noteLineIndex = _playbackNoteIndices[noteLineIndex];
				}
				if (noteLineIndex != _playbackHighlightIndex)
				{
					if (_playbackHighlightIndex >= 0 && _playbackHighlightIndex < _chips.Count)
					{
						_chips[_playbackHighlightIndex].IsSelected = false;
					}
					_playbackHighlightIndex = noteLineIndex;
					if (noteLineIndex >= 0 && noteLineIndex < _chips.Count && !(_chips[noteLineIndex] is SectionMarkerChip))
					{
						_chips[noteLineIndex].IsSelected = true;
						RequestScrollTo(noteLineIndex);
					}
				}
			}
		}

		private BaseChip CreateChip(string noteString, int index)
		{
			if (IsSectionMarker(noteString))
			{
				return new SectionMarkerChip(GetSectionName(noteString), index, ((Control)_chipsContainer).get_Width());
			}
			return new NoteChip(noteString, index);
		}

		private void AddSectionMarker(string name)
		{
			string marker = "[" + name + "]";
			if (_isInsertMode && _selectedIndices.Count > 0)
			{
				int insertIndex = _selectedIndices.Max() + 1;
				InsertAt(insertIndex, marker);
				SelectSingle(insertIndex);
			}
			else
			{
				AddNote(marker);
			}
			_suppressSectionScroll = true;
			for (int i = 0; i < _sectionJumpDropdown.ItemCount; i++)
			{
				if (_sectionJumpDropdown.ItemAt(i)?.DisplayText == name)
				{
					_sectionJumpDropdown.SelectedIndex = i;
					break;
				}
			}
			_suppressSectionScroll = false;
		}

		private void ShowCustomSectionInput()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			if (_customSectionInput != null)
			{
				return;
			}
			int inputX = (((Control)_sectionJumpDropdown).get_Visible() ? (((Control)_sectionJumpDropdown).get_Location().X + ((Control)_sectionJumpDropdown).get_Width() + 5) : 5);
			int inputY = (((Control)_sectionJumpDropdown).get_Visible() ? ((Control)_sectionJumpDropdown).get_Location().Y : ((Control)_modeStatusLabel).get_Location().Y);
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(inputX, inputY));
			((Control)val).set_Width(140);
			((TextInputBase)val).set_PlaceholderText("Section name...");
			((Control)val).set_ZIndex(50);
			_customSectionInput = val;
			_customSectionInput.add_EnterPressed((EventHandler<EventArgs>)delegate(object s, EventArgs e)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				string text = ((TextInputBase)(TextBox)s).get_Text().Trim();
				RemoveCustomSectionInput();
				if (!string.IsNullOrEmpty(text))
				{
					AddSectionMarker(text);
				}
			});
			((Control)_modeStatusLabel).set_Visible(false);
		}

		private void RemoveCustomSectionInput()
		{
			if (_customSectionInput != null)
			{
				((Control)_customSectionInput).Dispose();
				_customSectionInput = null;
				((Control)_modeStatusLabel).set_Visible(true);
			}
		}

		private void OnSectionJumpChanged(object sender, ValueChangedEventArgs e)
		{
			if (!_suppressSectionScroll)
			{
				ScrollToSection(e.get_CurrentValue());
			}
		}

		private void ScrollToSection(string sectionName)
		{
			if (string.IsNullOrEmpty(sectionName))
			{
				return;
			}
			for (int i = 0; i < _notes.Count; i++)
			{
				if (IsSectionMarker(_notes[i]) && GetSectionName(_notes[i]) == sectionName)
				{
					RequestScrollTo(i);
					break;
				}
			}
		}

		private void UpdateSectionDropdown()
		{
			string previousSelection = _sectionJumpDropdown.SelectedValue;
			List<string> sections = new List<string>();
			foreach (string note in _notes)
			{
				if (IsSectionMarker(note))
				{
					sections.Add(GetSectionName(note));
				}
			}
			_suppressSectionScroll = true;
			_sectionJumpDropdown.ClearItems();
			foreach (string section in sections)
			{
				_sectionJumpDropdown.AddItem(section);
			}
			((Control)_sectionJumpDropdown).set_Visible(sections.Count > 0);
			if (previousSelection != null)
			{
				for (int i = 0; i < _sectionJumpDropdown.ItemCount; i++)
				{
					if (_sectionJumpDropdown.ItemAt(i)?.DisplayText == previousSelection)
					{
						_sectionJumpDropdown.SelectedIndex = i;
						break;
					}
				}
			}
			_suppressSectionScroll = false;
		}

		protected override void DisposeControl()
		{
			Panel headerPanel = _headerPanel;
			if (headerPanel != null)
			{
				((Control)headerPanel).Dispose();
			}
			Panel footerPanel = _footerPanel;
			if (footerPanel != null)
			{
				((Control)footerPanel).Dispose();
			}
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
			StandardButton previewAllButton = _previewAllButton;
			if (previewAllButton != null)
			{
				((Control)previewAllButton).Dispose();
			}
			StandardButton previewSelectedButton = _previewSelectedButton;
			if (previewSelectedButton != null)
			{
				((Control)previewSelectedButton).Dispose();
			}
			StandardButton pauseButton = _pauseButton;
			if (pauseButton != null)
			{
				((Control)pauseButton).Dispose();
			}
			StandardButton stopButton = _stopButton;
			if (stopButton != null)
			{
				((Control)stopButton).Dispose();
			}
			Label playbackStatusLabel = _playbackStatusLabel;
			if (playbackStatusLabel != null)
			{
				((Control)playbackStatusLabel).Dispose();
			}
			StandardButton sectionButton = _sectionButton;
			if (sectionButton != null)
			{
				((Control)sectionButton).Dispose();
			}
			ContextMenuStrip sectionMenu = _sectionMenu;
			if (sectionMenu != null)
			{
				((Control)sectionMenu).Dispose();
			}
			CustomDropdown sectionJumpDropdown = _sectionJumpDropdown;
			if (sectionJumpDropdown != null)
			{
				((Control)sectionJumpDropdown).Dispose();
			}
			TextBox customSectionInput = _customSectionInput;
			if (customSectionInput != null)
			{
				((Control)customSectionInput).Dispose();
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
			foreach (BaseChip chip in _chips)
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
