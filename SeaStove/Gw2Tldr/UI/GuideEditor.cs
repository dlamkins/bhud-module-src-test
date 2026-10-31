using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using SeaStove.Gw2Tldr.Data;

namespace SeaStove.Gw2Tldr.UI
{
	public sealed class GuideEditor : IDisposable
	{
		private readonly UtilityWindow _window;

		private readonly Panel _rows;

		private Panel _typeMenu;

		private readonly Label _status;

		private readonly DarkButton _mode;

		private readonly DarkButton _add;

		private readonly DarkButton _resetButton;

		private readonly DarkButton _cancel;

		private readonly DarkButton _saveButton;

		private readonly List<WrappedNoteInput> _inputs = new List<WrappedNoteInput>();

		private readonly List<Mechanic> _normal;

		private readonly List<Mechanic> _challenge;

		private readonly Action<List<Mechanic>, List<Mechanic>> _save;

		private readonly Action _reset;

		private bool _isChallenge;

		private readonly bool _colorTypes;

		private bool _refreshAfterEditing;

		private static readonly string[] Types = new string[7] { "movement", "group", "combat", "danger", "timer", "shield", "info" };

		private static readonly string[] Colors = new string[7] { "blue", "green", "orange", "red", "purple", "teal", "gray" };

		private List<Mechanic> Current
		{
			get
			{
				if (!_isChallenge)
				{
					return _normal;
				}
				return _challenge;
			}
		}

		public GuideEditor(GuideEntry guide, bool challenge, Action<List<Mechanic>, List<Mechanic>> save, Action reset, bool colorTypes)
		{
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Expected O, but got Unknown
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Expected O, but got Unknown
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			_colorTypes = colorTypes;
			_save = save;
			_reset = reset;
			_isChallenge = challenge;
			_normal = Clone(guide.GetMechanics(challengeMode: false));
			_challenge = Clone(guide.GetMechanics(challengeMode: true));
			_window = new UtilityWindow("Edit " + guide.Name, guide.Group, new Point(300, 180), resizable: true);
			DarkButton darkButton = new DarkButton();
			((Control)darkButton).set_Parent((Container)(object)_window.Body);
			((Control)darkButton).set_Location(new Point(10, 10));
			((Control)darkButton).set_Width(170);
			_mode = darkButton;
			((Control)_mode).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ReadInputs();
				_isChallenge = !_isChallenge;
				DrawRows();
			});
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_window.Body);
			((Control)val).set_Location(new Point(190, 10));
			((Control)val).set_Size(new Point(320, 25));
			val.set_Text("Changes are local until saved");
			_status = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_window.Body);
			((Control)val2).set_Location(new Point(10, 47));
			((Control)val2).set_Size(new Point(500, 440));
			val2.set_CanScroll(true);
			_rows = val2;
			DarkButton darkButton2 = new DarkButton();
			((Control)darkButton2).set_Parent((Container)(object)_window.Body);
			((Control)darkButton2).set_Location(new Point(10, 495));
			((Control)darkButton2).set_Width(120);
			((Label)darkButton2).set_Text("+ Add note");
			_add = darkButton2;
			((Control)_add).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ReadInputs();
				Current.Add(new Mechanic
				{
					Text = "",
					Type = "info",
					Color = "gray"
				});
				DrawRows();
			});
			DarkButton darkButton3 = new DarkButton();
			((Control)darkButton3).set_Parent((Container)(object)_window.Body);
			((Control)darkButton3).set_Location(new Point(135, 495));
			((Control)darkButton3).set_Width(120);
			((Label)darkButton3).set_Text("Reset");
			_resetButton = darkButton3;
			((Control)_resetButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_reset();
				((Control)_window).Hide();
			});
			DarkButton darkButton4 = new DarkButton();
			((Control)darkButton4).set_Parent((Container)(object)_window.Body);
			((Control)darkButton4).set_Location(new Point(260, 495));
			((Control)darkButton4).set_Width(120);
			((Label)darkButton4).set_Text("Cancel");
			_cancel = darkButton4;
			((Control)_cancel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_window).Hide();
			});
			DarkButton darkButton5 = new DarkButton();
			((Control)darkButton5).set_Parent((Container)(object)_window.Body);
			((Control)darkButton5).set_Location(new Point(385, 495));
			((Control)darkButton5).set_Width(120);
			((Label)darkButton5).set_Text("Save");
			_saveButton = darkButton5;
			((Control)_saveButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ReadInputs();
				if (_normal.Count == 0 || _normal.Concat(_challenge).Any((Mechanic note) => string.IsNullOrWhiteSpace(note.Text)))
				{
					_status.set_Text("Add a normal note and fill empty notes.");
				}
				else
				{
					_save(Clone(_normal), Clone(_challenge));
					((Control)_window).Hide();
				}
			});
			_window.WindowResized += OnResize;
			DrawRows();
			((Control)_window).Show();
		}

		private static List<Mechanic> Clone(IEnumerable<Mechanic> source)
		{
			return source.Select((Mechanic item) => new Mechanic
			{
				Text = item.Text,
				Type = item.Type,
				Color = item.Color
			}).ToList();
		}

		private void ReadInputs()
		{
			for (int index = 0; index < _inputs.Count; index++)
			{
				Current[index].Text = ((TextInputBase)_inputs[index]).get_Text();
			}
		}

		private void DrawRows()
		{
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Expected O, but got Unknown
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			Panel typeMenu = _typeMenu;
			if (typeMenu != null)
			{
				((Control)typeMenu).Dispose();
			}
			_typeMenu = null;
			Control[] array = ((Container)_rows).get_Children().ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Dispose();
			}
			_inputs.Clear();
			((Label)_mode).set_Text(_isChallenge ? "Editing CM" : "Editing normal");
			int top = 5;
			for (int index = 0; index < Current.Count; index++)
			{
				int row = index;
				SearchResultPanel searchResultPanel = new SearchResultPanel();
				((Control)searchResultPanel).set_Parent((Container)(object)_rows);
				((Control)searchResultPanel).set_Location(new Point(5, top));
				((Control)searchResultPanel).set_Size(new Point(Math.Max(360, ((Control)_rows).get_Width() - 40), 69));
				SearchResultPanel card = searchResultPanel;
				int cardWidth = ((Control)card).get_Width();
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)card);
				((Control)val).set_Location(new Point(8, 4));
				((Control)val).set_Width(Math.Max(220, cardWidth - 20));
				val.set_WrapText(true);
				val.set_AutoSizeHeight(true);
				val.set_Text(string.IsNullOrEmpty(Current[row].Text) ? "Click to edit note" : Current[row].Text);
				((Control)val).set_BasicTooltipText("Click to edit mechanic text");
				Label preview = val;
				int actionTop = Math.Max(82, ((Control)preview).get_Height() + 9);
				((Control)card).set_Height(actionTop + 34);
				WrappedNoteInput wrappedNoteInput = new WrappedNoteInput();
				((Control)wrappedNoteInput).set_Parent((Container)(object)card);
				((Control)wrappedNoteInput).set_Location(new Point(8, 4));
				((Control)wrappedNoteInput).set_Size(new Point(Math.Max(220, cardWidth - 20), actionTop - 8));
				((TextInputBase)wrappedNoteInput).set_MaxLength(4000);
				((TextInputBase)wrappedNoteInput).set_Text(Current[row].Text);
				((Control)wrappedNoteInput).set_Visible(false);
				((Control)wrappedNoteInput).set_BasicTooltipText("Edit mechanic text");
				WrappedNoteInput input = wrappedNoteInput;
				_inputs.Add(input);
				((Control)preview).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					((Control)preview).set_Visible(false);
					((Control)input).set_Visible(true);
					((TextInputBase)input).set_Focused(true);
				});
				((TextInputBase)input).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate
				{
					if (!((TextInputBase)input).get_Focused())
					{
						_refreshAfterEditing = true;
					}
				});
				Color typeColor = (_colorTypes ? CompactGuideWindow.IconColor(CompactGuideWindow.TypeColor(Current[row].Type), OverlayTheme.Light) : (OverlayTheme.Light ? Color.get_DarkSlateGray() : Color.get_LightGray()));
				MechanicTypeButton mechanicTypeButton = new MechanicTypeButton(Current[row].Type, typeColor);
				((Control)mechanicTypeButton).set_Parent((Container)(object)card);
				((Control)mechanicTypeButton).set_Location(new Point(7, actionTop));
				((Control)mechanicTypeButton).set_Size(new Point(140, 26));
				((Control)mechanicTypeButton).set_BasicTooltipText("Choose mechanic type");
				((Control)mechanicTypeButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					ShowTypeMenu(row);
				});
				ToolbarIcon toolbarIcon = new ToolbarIcon("up");
				((Control)toolbarIcon).set_Parent((Container)(object)card);
				((Control)toolbarIcon).set_Location(new Point(cardWidth - 100, actionTop));
				((Control)toolbarIcon).set_Size(new Point(24, 24));
				((Control)toolbarIcon).set_Enabled(row > 0);
				((Control)toolbarIcon).set_BasicTooltipText("Move up");
				((Control)toolbarIcon).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					ReadInputs();
					Mechanic item2 = Current[row];
					Current.RemoveAt(row);
					Current.Insert(row - 1, item2);
					DrawRows();
				});
				ToolbarIcon toolbarIcon2 = new ToolbarIcon("down");
				((Control)toolbarIcon2).set_Parent((Container)(object)card);
				((Control)toolbarIcon2).set_Location(new Point(cardWidth - 68, actionTop));
				((Control)toolbarIcon2).set_Size(new Point(24, 24));
				((Control)toolbarIcon2).set_Enabled(row < Current.Count - 1);
				((Control)toolbarIcon2).set_BasicTooltipText("Move down");
				((Control)toolbarIcon2).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					ReadInputs();
					Mechanic item = Current[row];
					Current.RemoveAt(row);
					Current.Insert(row + 1, item);
					DrawRows();
				});
				ToolbarIcon toolbarIcon3 = new ToolbarIcon("close");
				((Control)toolbarIcon3).set_Parent((Container)(object)card);
				((Control)toolbarIcon3).set_Location(new Point(cardWidth - 36, actionTop));
				((Control)toolbarIcon3).set_Size(new Point(24, 24));
				((Control)toolbarIcon3).set_BasicTooltipText("Delete note");
				((Control)toolbarIcon3).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					ReadInputs();
					Current.RemoveAt(row);
					DrawRows();
				});
				top += ((Control)card).get_Height() + 7;
			}
			_window.RefreshTheme();
		}

		private void OnResize()
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			ReadInputs();
			((Control)_rows).set_Size(new Point(((Control)_window.Body).get_Width() - 20, Math.Max(240, ((Control)_window.Body).get_Height() - 120)));
			int footer = ((Control)_rows).get_Bottom() + 8;
			((Control)_add).set_Top(footer);
			((Control)_resetButton).set_Top(footer);
			((Control)_cancel).set_Top(footer);
			((Control)_saveButton).set_Top(footer);
			((Control)_status).set_Width(((Control)_window.Body).get_Width() - 200);
			DrawRows();
		}

		private static string Capitalize(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				return char.ToUpperInvariant(value[0]) + value.Substring(1);
			}
			return value;
		}

		private void ShowTypeMenu(int row)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			ReadInputs();
			if (_typeMenu != null)
			{
				((Control)_typeMenu).Dispose();
				_typeMenu = null;
				return;
			}
			SearchResultPanel searchResultPanel = new SearchResultPanel();
			((Control)searchResultPanel).set_Parent((Container)(object)_window.Body);
			((Control)searchResultPanel).set_Location(new Point(255, 80));
			((Control)searchResultPanel).set_Size(new Point(190, Types.Length * 28));
			((Control)searchResultPanel).set_ZIndex(100);
			_typeMenu = (Panel)(object)searchResultPanel;
			for (int i = 0; i < Types.Length; i++)
			{
				int typeIndex = i;
				MechanicIcon mechanicIcon = new MechanicIcon(Types[i], Color.get_LightGray());
				((Control)mechanicIcon).set_Parent((Container)(object)_typeMenu);
				((Control)mechanicIcon).set_Location(new Point(8, 5 + i * 28));
				((Control)mechanicIcon).set_Size(new Point(18, 18));
				MechanicIcon icon = mechanicIcon;
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)_typeMenu);
				((Control)val).set_Location(new Point(33, i * 28));
				((Control)val).set_Size(new Point(150, 27));
				val.set_Text(Capitalize(Types[i]));
				val.set_TextColor(Color.get_White());
				EventHandler<MouseEventArgs> choose = delegate
				{
					Current[row].Type = Types[typeIndex];
					Current[row].Color = Colors[typeIndex];
					DrawRows();
				};
				((Control)icon).add_Click(choose);
				((Control)val).add_Click(choose);
			}
			_window.RefreshTheme();
		}

		public void Update()
		{
			_window.UpdateDrag();
			if (_refreshAfterEditing)
			{
				_refreshAfterEditing = false;
				ReadInputs();
				DrawRows();
			}
		}

		public void Dispose()
		{
			_window.WindowResized -= OnResize;
			Panel typeMenu = _typeMenu;
			if (typeMenu != null)
			{
				((Control)typeMenu).Dispose();
			}
			_typeMenu = null;
			((Control)_window).Dispose();
		}
	}
}
