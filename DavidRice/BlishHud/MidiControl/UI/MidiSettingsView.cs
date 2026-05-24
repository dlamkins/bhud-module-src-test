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

		private Action? _onLogUpdate;

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
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Expected O, but got Unknown
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Expected O, but got Unknown
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Expected O, but got Unknown
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Expected O, but got Unknown
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_0397: Expected O, but got Unknown
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Expected O, but got Unknown
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			//IL_0444: Unknown result type (might be due to invalid IL or missing references)
			//IL_0456: Unknown result type (might be due to invalid IL or missing references)
			//IL_0466: Expected O, but got Unknown
			//IL_048e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Expected O, but got Unknown
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_050e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Expected O, but got Unknown
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			//IL_052f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0540: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Unknown result type (might be due to invalid IL or missing references)
			//IL_054d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_055f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0566: Unknown result type (might be due to invalid IL or missing references)
			//IL_0567: Unknown result type (might be due to invalid IL or missing references)
			//IL_0576: Expected O, but got Unknown
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
			y += ((Control)_keymapDropdown).get_Height() + 4;
			Panel val7 = new Panel();
			((Control)val7).set_Parent((Container)(object)buildPanel);
			((Control)val7).set_Location(new Point(x, y));
			((Control)val7).set_Size(new Point(420, 90));
			val7.set_CanScroll(true);
			Panel previewPanel = val7;
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)previewPanel);
			val8.set_Text("");
			((Control)val8).set_Location(new Point(0, 0));
			val8.set_AutoSizeHeight(true);
			((Control)val8).set_Width(400);
			val8.set_WrapText(true);
			val8.set_TextColor(Color.get_LightGray());
			_previewLabel = val8;
			y += ((Control)previewPanel).get_Height() + 10;
			Checkbox val9 = new Checkbox();
			((Control)val9).set_Parent((Container)(object)buildPanel);
			val9.set_Text("Send Notes");
			((Control)val9).set_Location(new Point(x, y));
			val9.set_Checked(_module.SendNotesEnabled);
			Checkbox sendNotesCb = val9;
			sendNotesCb.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_module.SendNotesEnabled = e.get_Checked();
			});
			y += ((Control)sendNotesCb).get_Height() + 4;
			Checkbox val10 = new Checkbox();
			((Control)val10).set_Parent((Container)(object)buildPanel);
			val10.set_Text("Auto Swap Octave");
			((Control)val10).set_Location(new Point(x, y));
			val10.set_Checked(_module.AutoSwapOctaveEnabled);
			Checkbox autoSwapCb = val10;
			autoSwapCb.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_module.AutoSwapOctaveEnabled = e.get_Checked();
			});
			y += ((Control)autoSwapCb).get_Height() + 4;
			Checkbox val11 = new Checkbox();
			((Control)val11).set_Parent((Container)(object)buildPanel);
			val11.set_Text("Focus Guard");
			((Control)val11).set_Location(new Point(x, y));
			val11.set_Checked(_module.FocusGuardEnabled);
			Checkbox focusGuardCb = val11;
			focusGuardCb.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_module.FocusGuardEnabled = e.get_Checked();
			});
			y += ((Control)focusGuardCb).get_Height() + 10;
			Label val12 = new Label();
			((Control)val12).set_Parent((Container)(object)buildPanel);
			val12.set_Text($"Multi-Octave Shift Delay: {_module.MultipleOctaveShiftDelay} ms");
			((Control)val12).set_Location(new Point(x, y));
			val12.set_AutoSizeHeight(true);
			val12.set_AutoSizeWidth(true);
			Label delayLabel = val12;
			y += ((Control)delayLabel).get_Height() + 4;
			TrackBar val13 = new TrackBar();
			((Control)val13).set_Parent((Container)(object)buildPanel);
			((Control)val13).set_Location(new Point(x, y));
			val13.set_MinValue(0f);
			val13.set_MaxValue(500f);
			val13.set_Value((float)_module.MultipleOctaveShiftDelay);
			((Control)val13).set_Width(310);
			TrackBar delaySlider = val13;
			delaySlider.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				_module.MultipleOctaveShiftDelay = (int)delaySlider.get_Value();
				delayLabel.set_Text($"Multi-Octave Shift Delay: {_module.MultipleOctaveShiftDelay} ms");
			});
			y += ((Control)delaySlider).get_Height() + 10;
			Label val14 = new Label();
			((Control)val14).set_Parent((Container)(object)buildPanel);
			val14.set_Text("Recent Sends");
			((Control)val14).set_Location(new Point(x, y));
			val14.set_TextColor(Color.FromNonPremultiplied(194, 181, 145, 255));
			val14.set_AutoSizeHeight(true);
			val14.set_AutoSizeWidth(true);
			Label logHeader = val14;
			y += ((Control)logHeader).get_Height() + 4;
			Panel val15 = new Panel();
			((Control)val15).set_Parent((Container)(object)buildPanel);
			((Control)val15).set_Location(new Point(x, y));
			((Control)val15).set_Size(new Point(420, 90));
			val15.set_CanScroll(true);
			Panel logPanel = val15;
			Label val16 = new Label();
			((Control)val16).set_Parent((Container)(object)logPanel);
			val16.set_Text(_module.LastSendLog);
			((Control)val16).set_Location(new Point(0, 0));
			val16.set_AutoSizeHeight(true);
			((Control)val16).set_Width(400);
			val16.set_WrapText(true);
			val16.set_TextColor(Color.get_LightGray());
			_logLabel = val16;
			_onLogUpdate = delegate
			{
				if (_logLabel != null)
				{
					_logLabel!.set_Text(_module.LastSendLog);
				}
			};
			_module.RecentSendLogUpdated += _onLogUpdate;
			RefreshDevices();
			PopulateKeymapDropdown();
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
