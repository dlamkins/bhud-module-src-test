using System;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using DavidRice.BlishHud.MidiControl.Keymaps.Visualization;
using Microsoft.Xna.Framework;

namespace DavidRice.BlishHud.MidiControl.UI
{
	public class KeymapLayoutTabView : IView
	{
		private static readonly Logger Logger = Logger.GetLogger<KeymapLayoutTabView>();

		private readonly MidiModule _module;

		private KeybedControl? _keybedControl;

		private Panel? _keybedPanel;

		private TrackBar? _scrollTrackBar;

		private Dropdown? _keymapDropdown;

		private Label? _keymapInfoLabel;

		public bool WithPresenter => false;

		public event EventHandler<EventArgs>? Built;

		public event EventHandler<EventArgs>? Loaded;

		public event EventHandler<EventArgs>? Unloaded;

		public KeymapLayoutTabView(MidiModule module)
		{
			_module = module ?? throw new ArgumentNullException("module");
		}

		public Task<bool> DoLoad(IProgress<string> progress)
		{
			progress.Report("Keymap Layout loaded.");
			return Task.FromResult(result: true);
		}

		public void DoBuild(Container buildPanel)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Expected O, but got Unknown
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Expected O, but got Unknown
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Expected O, but got Unknown
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Expected O, but got Unknown
			int x = 20;
			int y = 15;
			Label val = new Label();
			((Control)val).set_Parent(buildPanel);
			val.set_Text("Keymap");
			((Control)val).set_Location(new Point(x, y));
			val.set_TextColor(Color.FromNonPremultiplied(194, 181, 145, 255));
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			Label selectLabel = val;
			y += ((Control)selectLabel).get_Height() + 6;
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Location(new Point(x, y));
			((Control)val2).set_Width(260);
			_keymapDropdown = val2;
			_keymapDropdown!.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnKeymapSelected);
			y += ((Control)_keymapDropdown).get_Height() + 8;
			Label val3 = new Label();
			((Control)val3).set_Parent(buildPanel);
			val3.set_Text("");
			((Control)val3).set_Location(new Point(x, y));
			((Control)val3).set_Height(20);
			val3.set_AutoSizeHeight(false);
			val3.set_AutoSizeWidth(true);
			val3.set_TextColor(Color.get_LightGray());
			_keymapInfoLabel = val3;
			y += 28;
			Panel val4 = new Panel();
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Location(new Point(x, y));
			((Control)val4).set_Size(new Point(420, 118));
			_keybedPanel = val4;
			KeybedControl keybedControl = new KeybedControl();
			((Control)keybedControl).set_Parent((Container)(object)_keybedPanel);
			((Control)keybedControl).set_Location(new Point(0, 0));
			keybedControl.Layout = KeybedLayout.Empty;
			_keybedControl = keybedControl;
			TrackBar val5 = new TrackBar();
			((Control)val5).set_Parent(buildPanel);
			((Control)val5).set_Location(new Point(x, y + ((Control)_keybedPanel).get_Height() + 4));
			((Control)val5).set_Size(new Point(420, 20));
			val5.set_MinValue(0f);
			val5.set_MaxValue(0f);
			val5.set_Value(0f);
			_scrollTrackBar = val5;
			_scrollTrackBar!.add_ValueChanged((EventHandler<ValueEventArgs<float>>)OnScrollValueChanged);
			PopulateDropdown();
			_module.SelectedKeymapChanged += new Action<string>(OnSelectedKeymapChanged);
		}

		public void DoUnload()
		{
			_module.SelectedKeymapChanged -= new Action<string>(OnSelectedKeymapChanged);
		}

		private void PopulateDropdown()
		{
			if (_keymapDropdown == null)
			{
				return;
			}
			_keymapDropdown!.remove_ValueChanged((EventHandler<ValueChangedEventArgs>)OnKeymapSelected);
			_keymapDropdown!.get_Items().Clear();
			foreach (Keymap keymap in _module.AvailableKeymaps)
			{
				_keymapDropdown!.get_Items().Add(keymap.Name);
			}
			string activeId = _module.SelectedKeymapId;
			Keymap active = _module.AvailableKeymaps.FirstOrDefault((Keymap k) => k.Id == activeId);
			if (active != null)
			{
				_keymapDropdown!.set_SelectedItem(active.Name);
			}
			else if (_keymapDropdown!.get_Items().Count > 0)
			{
				_keymapDropdown!.set_SelectedItem(_keymapDropdown!.get_Items()[0]);
			}
			_keymapDropdown!.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnKeymapSelected);
			if (_keymapDropdown!.get_SelectedItem() != null)
			{
				Keymap initialKeymap = _module.AvailableKeymaps.FirstOrDefault((Keymap k) => k.Name == _keymapDropdown!.get_SelectedItem());
				UpdatePreview(initialKeymap);
			}
		}

		private void OnScrollValueChanged(object? sender, EventArgs e)
		{
			if (_keybedPanel != null && _scrollTrackBar != null)
			{
				((Container)_keybedPanel).set_HorizontalScrollOffset((int)_scrollTrackBar!.get_Value());
			}
		}

		private void OnSelectedKeymapChanged(string keymapId)
		{
			string keymapId2 = keymapId;
			Dropdown? keymapDropdown = _keymapDropdown;
			string currentName = ((keymapDropdown != null) ? keymapDropdown!.get_SelectedItem() : null);
			if (!(_module.AvailableKeymaps.FirstOrDefault((Keymap k) => k.Name == currentName)?.Id == keymapId2))
			{
				Keymap keymap = _module.AvailableKeymaps.FirstOrDefault((Keymap k) => k.Id == keymapId2);
				if (keymap != null)
				{
					_keymapDropdown!.remove_ValueChanged((EventHandler<ValueChangedEventArgs>)OnKeymapSelected);
					_keymapDropdown!.set_SelectedItem(keymap.Name);
					_keymapDropdown!.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnKeymapSelected);
					UpdatePreview(keymap);
				}
			}
		}

		private void OnKeymapSelected(object? sender, EventArgs e)
		{
			Dropdown? keymapDropdown = _keymapDropdown;
			string selectedName = ((keymapDropdown != null) ? keymapDropdown!.get_SelectedItem() : null);
			if (string.IsNullOrEmpty(selectedName))
			{
				UpdatePreview(null);
				return;
			}
			Keymap keymap = _module.AvailableKeymaps.FirstOrDefault((Keymap k) => k.Name == selectedName);
			if (keymap == null)
			{
				UpdatePreview(null);
				return;
			}
			_module.SelectKeymap(keymap.Id);
			UpdatePreview(keymap);
		}

		private void UpdatePreview(Keymap? keymap)
		{
			if (keymap == null)
			{
				if (_keybedControl != null)
				{
					_keybedControl!.Layout = KeybedLayout.Empty;
				}
				if (_keymapInfoLabel != null)
				{
					_keymapInfoLabel!.set_Text("");
				}
				UpdateScrollBar();
				return;
			}
			KeybedLayout layout = KeybedLayoutCalculator.Calculate(keymap);
			if (_keybedControl != null)
			{
				_keybedControl!.Layout = layout;
			}
			if (_keymapInfoLabel != null)
			{
				int mappedCount = layout.Keys.Count((KeybedKey k) => k.IsMapped);
				string info = ((mappedCount > 0) ? $"Octaves {layout.StartOctave}–{layout.EndOctave}  |  {mappedCount} mapped notes" : $"Octaves {layout.StartOctave}–{layout.EndOctave}  |  empty");
				_keymapInfoLabel!.set_Text(info);
			}
			UpdateScrollBar();
		}

		private void UpdateScrollBar()
		{
			if (_keybedPanel != null && _scrollTrackBar != null && _keybedControl != null)
			{
				int maxScroll = Math.Max(0, ((Control)_keybedControl).get_Width() - ((Control)_keybedPanel).get_Width());
				_scrollTrackBar!.remove_ValueChanged((EventHandler<ValueEventArgs<float>>)OnScrollValueChanged);
				_scrollTrackBar!.set_MaxValue((float)maxScroll);
				_scrollTrackBar!.set_Value(0f);
				_scrollTrackBar!.add_ValueChanged((EventHandler<ValueEventArgs<float>>)OnScrollValueChanged);
				((Container)_keybedPanel).set_HorizontalScrollOffset(0);
			}
		}
	}
}
