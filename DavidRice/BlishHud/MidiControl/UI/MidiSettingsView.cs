using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using DavidRice.BlishHud.MidiControl.Keymaps;
using Microsoft.Xna.Framework;

namespace DavidRice.BlishHud.MidiControl.UI
{
	public class MidiSettingsView
	{
		private static readonly Logger Logger = Logger.GetLogger<MidiSettingsView>();

		private readonly MidiModule _module;

		private Dropdown? _deviceDropdown;

		private Label? _statusLabel;

		private Dropdown? _keymapDropdown;

		private Label? _logLabel;

		private Label? _previewLabel;

		private Panel? _previewPanel;

		private int _previewPanelBaseY;

		private int _previewPanelTallHeight;

		private int _previewPanelShortHeight;

		private Label? _keymapStatusLabel;

		private Action? _onLogUpdate;

		private Checkbox? _sendNotesCb;

		private bool _updatingSendNotesFromEvent;

		public MidiSettingsView(MidiModule module)
		{
			_module = module ?? throw new ArgumentNullException("module");
		}

		public void Build(Panel buildPanel)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected O, but got Unknown
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Expected O, but got Unknown
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Expected O, but got Unknown
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Expected O, but got Unknown
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Expected O, but got Unknown
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Expected O, but got Unknown
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Expected O, but got Unknown
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_0393: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Expected O, but got Unknown
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0401: Unknown result type (might be due to invalid IL or missing references)
			//IL_040b: Unknown result type (might be due to invalid IL or missing references)
			//IL_041c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Expected O, but got Unknown
			//IL_0448: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_045f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0462: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0477: Unknown result type (might be due to invalid IL or missing references)
			//IL_0484: Expected O, but got Unknown
			//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04da: Expected O, but got Unknown
			//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_0510: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0530: Expected O, but got Unknown
			//IL_0551: Unknown result type (might be due to invalid IL or missing references)
			//IL_0556: Unknown result type (might be due to invalid IL or missing references)
			//IL_055d: Unknown result type (might be due to invalid IL or missing references)
			//IL_057d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0580: Unknown result type (might be due to invalid IL or missing references)
			//IL_058a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_059d: Expected O, but got Unknown
			//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ff: Expected O, but got Unknown
			//IL_0627: Unknown result type (might be due to invalid IL or missing references)
			//IL_062c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0633: Unknown result type (might be due to invalid IL or missing references)
			//IL_063e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0641: Unknown result type (might be due to invalid IL or missing references)
			//IL_064b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0660: Unknown result type (might be due to invalid IL or missing references)
			//IL_066a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0671: Unknown result type (might be due to invalid IL or missing references)
			//IL_067a: Expected O, but got Unknown
			//IL_0686: Unknown result type (might be due to invalid IL or missing references)
			//IL_068b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0692: Unknown result type (might be due to invalid IL or missing references)
			//IL_0695: Unknown result type (might be due to invalid IL or missing references)
			//IL_069f: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ba: Expected O, but got Unknown
			//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0700: Unknown result type (might be due to invalid IL or missing references)
			//IL_070f: Expected O, but got Unknown
			buildPanel.set_ShowTint(true);
			int x = 20;
			int y = 15;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)buildPanel);
			val.set_Text("MIDI Device");
			((Control)val).set_Location(new Point(x, y));
			val.set_TextColor(Color.FromNonPremultiplied(194, 181, 145, 255));
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			Label deviceHeader = val;
			y += ((Control)deviceHeader).get_Height() + 6;
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Parent((Container)(object)buildPanel);
			((Control)val2).set_Location(new Point(x, y));
			((Control)val2).set_Width(220);
			_deviceDropdown = val2;
			_deviceDropdown!.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnDeviceSelected);
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)buildPanel);
			val3.set_Text("Refresh");
			((Control)val3).set_Location(new Point(248, y));
			((Control)val3).set_Width(80);
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RefreshDevices();
			});
			y += ((Control)_deviceDropdown).get_Height() + 6;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)buildPanel);
			val4.set_Text(_module.MidiDeviceStatus);
			((Control)val4).set_Location(new Point(x, y));
			val4.set_AutoSizeHeight(true);
			val4.set_AutoSizeWidth(true);
			val4.set_TextColor(Color.get_Gray());
			_statusLabel = val4;
			_module.StatusLabel = _statusLabel;
			y += ((Control)_statusLabel).get_Height() + 10;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)buildPanel);
			val5.set_Text("Keymap");
			((Control)val5).set_Location(new Point(x, y));
			val5.set_TextColor(Color.FromNonPremultiplied(194, 181, 145, 255));
			val5.set_AutoSizeHeight(true);
			val5.set_AutoSizeWidth(true);
			Label keymapHeader = val5;
			y += ((Control)keymapHeader).get_Height() + 4;
			Dropdown val6 = new Dropdown();
			((Control)val6).set_Parent((Container)(object)buildPanel);
			((Control)val6).set_Location(new Point(x, y));
			((Control)val6).set_Width(220);
			_keymapDropdown = val6;
			_keymapDropdown!.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnKeymapSelected);
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)buildPanel);
			val7.set_Text("Reload Keymaps");
			((Control)val7).set_Location(new Point(248, y));
			((Control)val7).set_Width(110);
			((Control)val7).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RefreshKeymaps();
			});
			y += ((Control)_keymapDropdown).get_Height() + 4;
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)buildPanel);
			val8.set_Text("");
			((Control)val8).set_Location(new Point(x, y));
			((Control)val8).set_Height(20);
			val8.set_AutoSizeHeight(false);
			val8.set_AutoSizeWidth(true);
			val8.set_TextColor(Color.get_Gray());
			_keymapStatusLabel = val8;
			RefreshKeymapStatusLabel();
			y += 24;
			Panel val9 = new Panel();
			((Control)val9).set_Parent((Container)(object)buildPanel);
			((Control)val9).set_Location(new Point(x, y));
			((Control)val9).set_Size(new Point(420, 90));
			val9.set_CanScroll(true);
			_previewPanel = val9;
			_previewPanelBaseY = y;
			_previewPanelTallHeight = 114;
			_previewPanelShortHeight = 90;
			Label val10 = new Label();
			((Control)val10).set_Parent((Container)(object)_previewPanel);
			val10.set_Text("");
			((Control)val10).set_Location(new Point(0, 0));
			val10.set_AutoSizeHeight(true);
			((Control)val10).set_Width(400);
			val10.set_WrapText(true);
			val10.set_TextColor(Color.get_LightGray());
			_previewLabel = val10;
			y += ((Control)_previewPanel).get_Height() + 10;
			Checkbox val11 = new Checkbox();
			((Control)val11).set_Parent((Container)(object)buildPanel);
			val11.set_Text("Send Notes");
			((Control)val11).set_Location(new Point(x, y));
			val11.set_Checked(_module.SendNotesEnabled);
			_sendNotesCb = val11;
			_sendNotesCb!.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				if (!_updatingSendNotesFromEvent)
				{
					_module.SendNotesEnabled = e.get_Checked();
				}
			});
			_module.SendNotesEnabledChanged += new Action<bool>(OnSendNotesEnabledChanged);
			y += ((Control)_sendNotesCb).get_Height() + 4;
			Checkbox val12 = new Checkbox();
			((Control)val12).set_Parent((Container)(object)buildPanel);
			val12.set_Text("Enable Key Hold");
			((Control)val12).set_Location(new Point(x, y));
			val12.set_Checked(_module.EnableKeyHoldEnabled);
			((Control)val12).set_BasicTooltipText("When enabled, MIDI note-on sends a key-down and note-off sends a key-up. Otherwise notes are tapped.");
			Checkbox enableKeyHoldCb = val12;
			enableKeyHoldCb.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_module.EnableKeyHoldEnabled = e.get_Checked();
			});
			y += ((Control)enableKeyHoldCb).get_Height() + 4;
			StandardButton val13 = new StandardButton();
			((Control)val13).set_Parent((Container)(object)buildPanel);
			val13.set_Text("Release All Keys");
			((Control)val13).set_Location(new Point(x, y));
			((Control)val13).set_Width(140);
			((Control)val13).set_BasicTooltipText("Force key-up for all instrument keys. Use if a key gets stuck in Key Hold mode.");
			StandardButton releaseAllKeysBtn = val13;
			((Control)releaseAllKeysBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.ReleaseAllKeys();
			});
			y += ((Control)releaseAllKeysBtn).get_Height() + 4;
			Checkbox val14 = new Checkbox();
			((Control)val14).set_Parent((Container)(object)buildPanel);
			val14.set_Text("Auto Swap Octave");
			((Control)val14).set_Location(new Point(x, y));
			val14.set_Checked(_module.AutoSwapOctaveEnabled);
			Checkbox autoSwapCb = val14;
			autoSwapCb.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_module.AutoSwapOctaveEnabled = e.get_Checked();
			});
			y += ((Control)autoSwapCb).get_Height() + 4;
			Checkbox val15 = new Checkbox();
			((Control)val15).set_Parent((Container)(object)buildPanel);
			val15.set_Text("Focus Guard");
			((Control)val15).set_Location(new Point(x, y));
			val15.set_Checked(_module.FocusGuardEnabled);
			Checkbox focusGuardCb = val15;
			focusGuardCb.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_module.FocusGuardEnabled = e.get_Checked();
			});
			y += ((Control)focusGuardCb).get_Height() + 10;
			Label val16 = new Label();
			((Control)val16).set_Parent((Container)(object)buildPanel);
			val16.set_Text($"Multi-Octave Shift Delay: {_module.MultipleOctaveShiftDelay} ms");
			((Control)val16).set_Location(new Point(x, y));
			val16.set_AutoSizeHeight(true);
			val16.set_AutoSizeWidth(true);
			Label delayLabel = val16;
			y += ((Control)delayLabel).get_Height() + 4;
			TrackBar val17 = new TrackBar();
			((Control)val17).set_Parent((Container)(object)buildPanel);
			((Control)val17).set_Location(new Point(x, y));
			val17.set_MinValue(0f);
			val17.set_MaxValue(500f);
			val17.set_Value((float)_module.MultipleOctaveShiftDelay);
			((Control)val17).set_Width(310);
			TrackBar delaySlider = val17;
			delaySlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				_module.MultipleOctaveShiftDelay = (int)delaySlider.get_Value();
				delayLabel.set_Text($"Multi-Octave Shift Delay: {_module.MultipleOctaveShiftDelay} ms");
			});
			y += ((Control)delaySlider).get_Height() + 10;
			Label val18 = new Label();
			((Control)val18).set_Parent((Container)(object)buildPanel);
			val18.set_Text("Recent Sends");
			((Control)val18).set_Location(new Point(x, y));
			val18.set_TextColor(Color.FromNonPremultiplied(194, 181, 145, 255));
			val18.set_AutoSizeHeight(true);
			val18.set_AutoSizeWidth(true);
			Label logHeader = val18;
			y += ((Control)logHeader).get_Height() + 4;
			Panel val19 = new Panel();
			((Control)val19).set_Parent((Container)(object)buildPanel);
			((Control)val19).set_Location(new Point(x, y));
			((Control)val19).set_Size(new Point(420, 90));
			val19.set_CanScroll(true);
			Panel logPanel = val19;
			Label val20 = new Label();
			((Control)val20).set_Parent((Container)(object)logPanel);
			val20.set_Text(_module.LastSendLog);
			((Control)val20).set_Location(new Point(0, 0));
			val20.set_AutoSizeHeight(true);
			((Control)val20).set_Width(400);
			val20.set_WrapText(true);
			val20.set_TextColor(Color.get_LightGray());
			_logLabel = val20;
			_onLogUpdate = delegate
			{
				if (_logLabel != null)
				{
					_logLabel!.set_Text(_module.LastSendLog);
				}
			};
			_module.RecentSendLogUpdated += _onLogUpdate;
			RefreshDevices();
			RefreshKeymaps();
			_module.SelectedKeymapChanged += new Action<string>(OnSelectedKeymapChanged);
		}

		private void RefreshDevices()
		{
			if (_deviceDropdown == null)
			{
				return;
			}
			_deviceDropdown!.remove_ValueChanged((EventHandler<ValueChangedEventArgs>)OnDeviceSelected);
			_deviceDropdown!.get_Items().Clear();
			try
			{
				IReadOnlyList<string> devices = _module.AvailableMidiDevices;
				if (devices.Count == 0)
				{
					Logger.Info("No MIDI devices detected by NAudio.");
					_deviceDropdown!.get_Items().Add("No MIDI devices found");
					((Control)_deviceDropdown).set_Enabled(false);
				}
				else
				{
					foreach (string device in devices)
					{
						_deviceDropdown!.get_Items().Add(device);
					}
					((Control)_deviceDropdown).set_Enabled(true);
					string saved = _module.SelectedMidiDeviceName;
					if (!string.IsNullOrEmpty(saved) && _deviceDropdown!.get_Items().Contains(saved))
					{
						_deviceDropdown!.set_SelectedItem(saved);
					}
					else if (_deviceDropdown!.get_Items().Count > 0)
					{
						_deviceDropdown!.set_SelectedItem(_deviceDropdown!.get_Items()[0]);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error("RefreshDevices failed.", new object[1] { ex });
				_deviceDropdown!.get_Items().Add("Error: " + ex.Message);
				((Control)_deviceDropdown).set_Enabled(false);
			}
			_deviceDropdown!.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnDeviceSelected);
			if (_deviceDropdown!.get_SelectedItem() != null)
			{
				OnDeviceSelected(null, EventArgs.Empty);
			}
		}

		private void OnDeviceSelected(object? sender, EventArgs e)
		{
			Dropdown? deviceDropdown = _deviceDropdown;
			string selected = ((deviceDropdown != null) ? deviceDropdown!.get_SelectedItem() : null);
			if (!string.IsNullOrEmpty(selected) && !(selected == "No MIDI devices found") && !(_module.SelectedMidiDeviceName == selected))
			{
				_module.OpenMidiDevice(selected);
				if (_statusLabel != null)
				{
					_statusLabel!.set_Text(_module.MidiDeviceStatus);
				}
			}
		}

		private void RefreshKeymaps()
		{
			_module.ReloadKeymaps();
			PopulateKeymapDropdown();
			RefreshKeymapStatusLabel();
		}

		private void PopulateKeymapDropdown()
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
			Keymap current = _module.AvailableKeymaps.FirstOrDefault((Keymap k) => k.Id == _module.SelectedKeymapId);
			if (current != null)
			{
				_keymapDropdown!.set_SelectedItem(current.Name);
			}
			else if (_keymapDropdown!.get_Items().Count > 0)
			{
				_keymapDropdown!.set_SelectedItem(_keymapDropdown!.get_Items()[0]);
			}
			_keymapDropdown!.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnKeymapSelected);
			RefreshPreview();
		}

		private void OnKeymapSelected(object? sender, EventArgs e)
		{
			Dropdown? keymapDropdown = _keymapDropdown;
			string selectedName = ((keymapDropdown != null) ? keymapDropdown!.get_SelectedItem() : null);
			if (!string.IsNullOrEmpty(selectedName))
			{
				Keymap keymap = _module.AvailableKeymaps.FirstOrDefault((Keymap k) => k.Name == selectedName);
				if (keymap != null)
				{
					_module.SelectKeymap(keymap.Id);
					RefreshPreview();
				}
			}
		}

		public void Unload()
		{
			if (_onLogUpdate != null)
			{
				_module.RecentSendLogUpdated -= _onLogUpdate;
			}
			_module.SendNotesEnabledChanged -= new Action<bool>(OnSendNotesEnabledChanged);
			_module.SelectedKeymapChanged -= new Action<string>(OnSelectedKeymapChanged);
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
					RefreshPreview();
				}
			}
		}

		private void OnSendNotesEnabledChanged(bool enabled)
		{
			if (_sendNotesCb != null)
			{
				_updatingSendNotesFromEvent = true;
				_sendNotesCb!.set_Checked(enabled);
				_updatingSendNotesFromEvent = false;
			}
		}

		private void RefreshKeymapStatusLabel()
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			if (_keymapStatusLabel == null || _previewPanel == null)
			{
				return;
			}
			int customCount = _module.CustomKeymapCount;
			IReadOnlyList<string> errors = _module.KeymapLoadErrors;
			int errorCount = errors.Count;
			if (customCount == 0 && errorCount == 0)
			{
				_keymapStatusLabel!.set_Text("");
				((Control)_keymapStatusLabel).set_BasicTooltipText((string)null);
				((Control)_previewPanel).set_Location(new Point(((Control)_previewPanel).get_Location().X, _previewPanelBaseY - (_previewPanelTallHeight - _previewPanelShortHeight)));
				((Control)_previewPanel).set_Height(_previewPanelTallHeight);
				return;
			}
			string text2 = ((customCount != 1) ? $"{customCount} custom keymaps loaded" : "1 custom keymap loaded");
			string text = text2;
			if (errorCount > 0)
			{
				text += ((errorCount == 1) ? ", 1 error" : $", {errorCount} errors");
				_keymapStatusLabel!.set_TextColor(Color.get_Orange());
			}
			else
			{
				_keymapStatusLabel!.set_TextColor(Color.get_Gray());
			}
			_keymapStatusLabel!.set_Text(text);
			if (errorCount > 0)
			{
				List<string> tooltipLines = errors.Take(10).ToList();
				if (errors.Count > 10)
				{
					tooltipLines.Add($"(+{errors.Count - 10} more)");
				}
				((Control)_keymapStatusLabel).set_BasicTooltipText(string.Join("\n", tooltipLines));
			}
			else
			{
				((Control)_keymapStatusLabel).set_BasicTooltipText((string)null);
			}
			((Control)_previewPanel).set_Location(new Point(((Control)_previewPanel).get_Location().X, _previewPanelBaseY));
			((Control)_previewPanel).set_Height(_previewPanelShortHeight);
		}

		private void RefreshPreview()
		{
			if (_previewLabel == null)
			{
				return;
			}
			Dropdown? keymapDropdown = _keymapDropdown;
			string selectedName = ((keymapDropdown != null) ? keymapDropdown!.get_SelectedItem() : null);
			if (string.IsNullOrEmpty(selectedName))
			{
				_previewLabel!.set_Text("");
				return;
			}
			Keymap keymap = _module.AvailableKeymaps.FirstOrDefault((Keymap k) => k.Name == selectedName);
			if (keymap == null)
			{
				_previewLabel!.set_Text("");
				return;
			}
			IReadOnlyList<string> lines = KeymapPreviewFormatter.FormatLines(keymap);
			_previewLabel!.set_Text(string.Join("\n", lines));
		}
	}
}
