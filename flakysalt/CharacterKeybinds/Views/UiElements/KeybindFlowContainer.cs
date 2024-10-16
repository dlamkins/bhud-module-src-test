using System;
using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Data;

namespace flakysalt.CharacterKeybinds.Views.UiElements
{
	public class KeybindFlowContainer : FlowPanel
	{
		private string defaultCharacterEntry = "Select Character";

		private string defaultKeybindsEntry = "Keybinds";

		private string defaultSpecializationEntry = "Specialization";

		private string coreSpecialization = "Core";

		private string wildcardSpecialization = "All Specialization";

		private CharacterKeybind _oldCharacterKeybind;

		private StandardButton removeButton { get; }

		private StandardButton applyButton { get; }

		private Image professionImage { get; }

		public Dropdown characterNameDropdown { get; }

		public Dropdown specializationDropdown { get; }

		public Dropdown keymapDropdown { get; }

		public event EventHandler<CharacterKeybind> OnApply;

		public event EventHandler<KeymapEventArgs> OnDataChanged;

		public event EventHandler<CharacterKeybind> OnRemove;

		public KeybindFlowContainer()
			: this()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Expected O, but got Unknown
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected O, but got Unknown
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Expected O, but got Unknown
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Expected O, but got Unknown
			((FlowPanel)this).set_OuterControlPadding(new Vector2(10f, 0f));
			((FlowPanel)this).set_ControlPadding(new Vector2(2f, 0f));
			((Control)this).set_Height(45);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)0);
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(new Point(30, 30));
			professionImage = val;
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Height(30);
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Width(120);
			characterNameDropdown = val2;
			Dropdown val3 = new Dropdown();
			((Control)val3).set_Height(30);
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Width(120);
			specializationDropdown = val3;
			specializationDropdown.get_Items().Add(wildcardSpecialization);
			specializationDropdown.get_Items().Add(coreSpecialization);
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Height(30);
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Width(120);
			keymapDropdown = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Apply");
			((Control)val5).set_Size(new Point(60, 30));
			applyButton = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("Delete");
			((Control)val6).set_Size(new Point(60, 30));
			removeButton = val6;
			keymapDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				((Control)applyButton).set_Enabled(keymapDropdown.get_SelectedItem() != defaultKeybindsEntry);
				this.OnDataChanged?.Invoke(this, GetKeymapArgs());
				_oldCharacterKeybind = GetKeymap();
			});
			specializationDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				this.OnDataChanged?.Invoke(this, GetKeymapArgs());
				_oldCharacterKeybind = GetKeymap();
			});
			characterNameDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				specializationDropdown.set_SelectedItem(defaultSpecializationEntry);
				this.OnDataChanged?.Invoke(this, GetKeymapArgs());
				_oldCharacterKeybind = GetKeymap();
			});
			((Control)applyButton).add_Click((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs eventArgs)
			{
				this.OnApply?.Invoke(o, GetKeymap());
			});
			((Control)removeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.OnRemove?.Invoke(0, GetKeymap());
				((Control)this).Dispose();
			});
		}

		public void SetDropdownContent(Dropdown dropdown, List<string> values)
		{
			values.ForEach(delegate(string e)
			{
				dropdown.get_Items().Add(e);
			});
		}

		public void SetValues(CharacterKeybind characterKeybind)
		{
			_oldCharacterKeybind = characterKeybind;
			characterNameDropdown.set_SelectedItem(string.IsNullOrEmpty(characterKeybind.characterName) ? defaultCharacterEntry : characterKeybind.characterName);
			specializationDropdown.set_SelectedItem(string.IsNullOrEmpty(characterKeybind.spezialisation) ? defaultSpecializationEntry : characterKeybind.spezialisation);
			keymapDropdown.set_SelectedItem(string.IsNullOrEmpty(characterKeybind.keymap) ? defaultKeybindsEntry : characterKeybind.keymap);
		}

		public void SetProfessionIcon(int iconId)
		{
			professionImage.set_Texture(AsyncTexture2D.FromAssetId(iconId));
		}

		public void AttachListeners(EventHandler<CharacterKeybind> OnApplyAction, EventHandler<KeymapEventArgs> OnDataChanged, EventHandler<CharacterKeybind> OnDeleteAction)
		{
			this.OnDataChanged += OnDataChanged;
			OnApply += OnApplyAction;
			OnRemove += OnDeleteAction;
			((Control)removeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.OnDataChanged -= OnDataChanged;
				OnApply -= OnApplyAction;
				OnRemove -= OnDeleteAction;
			});
		}

		private KeymapEventArgs GetKeymapArgs()
		{
			return new KeymapEventArgs
			{
				NewCharacterKeybind = GetKeymap(),
				OldCharacterKeybind = _oldCharacterKeybind
			};
		}

		private CharacterKeybind GetKeymap()
		{
			return new CharacterKeybind
			{
				characterName = ((characterNameDropdown.get_SelectedItem() == defaultCharacterEntry) ? null : characterNameDropdown.get_SelectedItem()),
				spezialisation = ((specializationDropdown.get_SelectedItem() == defaultSpecializationEntry) ? null : specializationDropdown.get_SelectedItem()),
				keymap = ((keymapDropdown.get_SelectedItem() == defaultKeybindsEntry) ? null : keymapDropdown.get_SelectedItem())
			};
		}
	}
}
