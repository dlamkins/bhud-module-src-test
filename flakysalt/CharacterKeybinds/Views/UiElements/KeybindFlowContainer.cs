using System;
using System.Collections.Generic;
using System.ComponentModel;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace flakysalt.CharacterKeybinds.Views.UiElements
{
	internal class KeybindFlowContainer : FlowPanel
	{
		public StandardButton removeButton { get; private set; }

		public StandardButton applyButton { get; private set; }

		public Image professionImage { get; private set; }

		public Dropdown characterNameDropdown { get; private set; }

		public Dropdown specializationDropdown { get; private set; }

		public Dropdown keymapDropdown { get; private set; }

		public event EventHandler<string> OnApplyPressed;

		public void SetKeymapOptions(List<string> options)
		{
			SetDropdownOptions(keymapDropdown, options);
		}

		public void SetSpecializationOptions(List<string> options)
		{
			SetDropdownOptions(specializationDropdown, options);
		}

		public void SetNameOptions(List<string> options)
		{
			SetDropdownOptions(characterNameDropdown, options);
		}

		private void SetDropdownOptions(Dropdown dropdown, List<string> options)
		{
			dropdown.get_Items().Clear();
			foreach (string option in options)
			{
				dropdown.get_Items().Add(option);
			}
			((Control)dropdown).set_Enabled(options.Count > 0);
			((Control)removeButton).set_Enabled(options.Count > 0);
		}

		public KeybindFlowContainer(string selectedCharacter = "", string selectedSpezialisations = "", string selectedKeymap = "")
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Expected O, but got Unknown
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Expected O, but got Unknown
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Expected O, but got Unknown
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)0);
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(new Point(30, 30));
			professionImage = val;
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Size(new Point(120, 30));
			((Control)val2).set_Enabled(false);
			characterNameDropdown = val2;
			characterNameDropdown.set_SelectedItem(string.IsNullOrEmpty(selectedCharacter) ? "Select Character" : selectedCharacter);
			Dropdown val3 = new Dropdown();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Size(new Point(120, 30));
			((Control)val3).set_Enabled(false);
			specializationDropdown = val3;
			specializationDropdown.set_SelectedItem(string.IsNullOrEmpty(selectedSpezialisations) ? "Specialization" : selectedSpezialisations);
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Size(new Point(120, 30));
			((Control)val4).set_Enabled(false);
			keymapDropdown = val4;
			keymapDropdown.set_SelectedItem(string.IsNullOrEmpty(selectedKeymap) ? "Keybinds" : selectedKeymap);
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Delete");
			((Control)val5).set_Size(new Point(70, 30));
			((Control)val5).set_Enabled(false);
			removeButton = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("Apply");
			((Control)val6).set_Size(new Point(60, 30));
			((Control)val6).set_Enabled(true);
			applyButton = val6;
			((Control)keymapDropdown).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				((Control)applyButton).set_Enabled(keymapDropdown.get_SelectedItem() != "Keybinds");
			});
			((Control)characterNameDropdown).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				((Control)specializationDropdown).set_Enabled(characterNameDropdown.get_SelectedItem() != "Select Character" && characterNameDropdown.get_Items().Count > 0);
			});
			((Control)applyButton).add_Click((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs eventArgs)
			{
				this.OnApplyPressed(o, keymapDropdown.get_SelectedItem());
			});
			((Control)removeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)this).Dispose();
			});
		}
	}
}
