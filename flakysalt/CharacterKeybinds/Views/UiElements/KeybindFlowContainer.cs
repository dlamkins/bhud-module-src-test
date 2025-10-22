using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Data;

namespace flakysalt.CharacterKeybinds.Views.UiElements
{
	public sealed class KeybindFlowContainer : FlowPanel
	{
		private readonly int _minDropdownWidth = 130;

		private const string DefaultCharacterEntry = "Select Character";

		private const string DefaultKeybindsEntry = "Keybinds";

		private const string DefaultSpecializationEntry = "Specialization";

		private const string CoreSpecialization = "Core";

		private const string WildcardSpecialization = "All Specialization";

		private const string InvalidSpecialization = "Invalid";

		private Keymap _oldCharacterKeymap;

		private List<LocalizedSpecialization> _localizedSpecializations;

		private StandardButton RemoveButton { get; }

		private StandardButton ApplyButton { get; }

		private Image ProfessionImage { get; }

		public Dropdown CharacterNameDropdown { get; }

		public Dropdown SpecializationDropdown { get; }

		public Dropdown KeymapDropdown { get; }

		public event EventHandler<Keymap> OnApply;

		public event EventHandler<KeymapEventArgs> OnDataChanged;

		public event EventHandler<Keymap> OnRemove;

		public KeybindFlowContainer()
			: this()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Expected O, but got Unknown
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Expected O, but got Unknown
			((FlowPanel)this).set_OuterControlPadding(new Vector2(10f, 0f));
			((FlowPanel)this).set_ControlPadding(new Vector2(2f, 0f));
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)0);
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(new Point(30, 30));
			ProfessionImage = val;
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Height(30);
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Width(_minDropdownWidth);
			CharacterNameDropdown = val2;
			Dropdown val3 = new Dropdown();
			((Control)val3).set_Height(30);
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Width(_minDropdownWidth);
			SpecializationDropdown = val3;
			SpecializationDropdown.get_Items().Add("All Specialization");
			SpecializationDropdown.get_Items().Add("Core");
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Height(30);
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Width(_minDropdownWidth);
			KeymapDropdown = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Apply");
			((Control)val5).set_Size(new Point(60, 30));
			ApplyButton = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("Delete");
			((Control)val6).set_Size(new Point(60, 30));
			RemoveButton = val6;
			KeymapDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnKeymapChanged);
			SpecializationDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnSpecializationChanged);
			CharacterNameDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnCharacterChanged);
			((Control)ApplyButton).add_Click((EventHandler<MouseEventArgs>)OnApplyClick);
			((Control)RemoveButton).add_Click((EventHandler<MouseEventArgs>)OnRemoveClick);
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)OnResized);
		}

		private void OnResized(object sender, ResizedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			int desiredSize = (int)((float)(e.get_CurrentSize().X - ((Control)ProfessionImage).get_Width() - ((Control)ApplyButton).get_Width() - ((Control)RemoveButton).get_Width()) - ((FlowPanel)this).get_ControlPadding().X * 6f - 40f) / 3;
			int width = MathHelper.Clamp(desiredSize, _minDropdownWidth, desiredSize);
			((Control)CharacterNameDropdown).set_Width(width);
			((Control)SpecializationDropdown).set_Width(width);
			((Control)KeymapDropdown).set_Width(width);
		}

		public void SetEnabled(bool enabled)
		{
			((Control)CharacterNameDropdown).set_Enabled(enabled);
			((Control)SpecializationDropdown).set_Enabled(enabled);
			((Control)KeymapDropdown).set_Enabled(enabled);
			((Control)ApplyButton).set_Enabled(enabled);
			((Control)RemoveButton).set_Enabled(enabled);
		}

		public void SetDropdownContent(Dropdown dropdown, List<string> values)
		{
			values.ForEach(delegate(string e)
			{
				dropdown.get_Items().Add(e);
			});
		}

		public void SetSpecializationContent(List<LocalizedSpecialization> values)
		{
			_localizedSpecializations = values;
			values.ForEach(delegate(LocalizedSpecialization e)
			{
				SpecializationDropdown.get_Items().Add(e.displayName);
			});
		}

		public void SetValues(Keymap keymap)
		{
			_oldCharacterKeymap = keymap;
			CharacterNameDropdown.set_SelectedItem(string.IsNullOrEmpty(keymap.CharacterName) ? "Select Character" : keymap.CharacterName);
			switch (keymap.SpecialisationId)
			{
			case 0:
				SpecializationDropdown.set_SelectedItem("Specialization");
				break;
			case -1:
				SpecializationDropdown.set_SelectedItem("Core");
				break;
			case -2:
				SpecializationDropdown.set_SelectedItem("All Specialization");
				break;
			case -10:
				SpecializationDropdown.set_SelectedItem("Invalid");
				break;
			default:
				SpecializationDropdown.set_SelectedItem(_localizedSpecializations.FirstOrDefault((LocalizedSpecialization e) => e.id == keymap.SpecialisationId)?.displayName);
				break;
			}
			KeymapDropdown.set_SelectedItem(string.IsNullOrEmpty(keymap.KeymapName) ? "Keybinds" : keymap.KeymapName);
		}

		public void SetProfessionIcon(int iconId)
		{
			ProfessionImage.set_Texture(AsyncTexture2D.FromAssetId(iconId));
		}

		public void AttachListeners(EventHandler<Keymap> onApplyAction, EventHandler<KeymapEventArgs> onDataChanged, EventHandler<Keymap> onDeleteAction)
		{
			OnDataChanged += onDataChanged;
			OnApply += onApplyAction;
			OnRemove += onDeleteAction;
			((Control)RemoveButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OnDataChanged -= onDataChanged;
				OnApply -= onApplyAction;
				OnRemove -= onDeleteAction;
			});
		}

		private KeymapEventArgs GetKeymapArgs()
		{
			return new KeymapEventArgs
			{
				NewCharacterKeymap = GetKeymap(),
				OldCharacterKeymap = _oldCharacterKeymap
			};
		}

		private Keymap GetKeymap()
		{
			int specialisationId = 0;
			if (SpecializationDropdown.get_Items().Contains(SpecializationDropdown.get_SelectedItem()) || SpecializationDropdown.get_SelectedItem() == "Invalid")
			{
				specialisationId = SpecializationDropdown.get_Items().IndexOf(SpecializationDropdown.get_SelectedItem()) switch
				{
					0 => -2, 
					1 => -1, 
					_ => _localizedSpecializations.FirstOrDefault((LocalizedSpecialization e) => e.displayName == SpecializationDropdown.get_SelectedItem())?.id ?? (-10), 
				};
			}
			return new Keymap
			{
				CharacterName = (CharacterNameDropdown.get_Items().Contains(CharacterNameDropdown.get_SelectedItem()) ? CharacterNameDropdown.get_SelectedItem() : null),
				SpecialisationId = specialisationId,
				KeymapName = (KeymapDropdown.get_Items().Contains(KeymapDropdown.get_SelectedItem()) ? KeymapDropdown.get_SelectedItem() : null)
			};
		}

		private void OnKeymapChanged(object sender, ValueChangedEventArgs args)
		{
			((Control)ApplyButton).set_Enabled(KeymapDropdown.get_SelectedItem() != "Keybinds");
			this.OnDataChanged?.Invoke(this, GetKeymapArgs());
			_oldCharacterKeymap = GetKeymap();
		}

		private void OnSpecializationChanged(object sender, ValueChangedEventArgs args)
		{
			this.OnDataChanged?.Invoke(this, GetKeymapArgs());
			_oldCharacterKeymap = GetKeymap();
		}

		private void OnCharacterChanged(object sender, ValueChangedEventArgs args)
		{
			SpecializationDropdown.set_SelectedItem("Specialization");
			this.OnDataChanged?.Invoke(this, GetKeymapArgs());
			_oldCharacterKeymap = GetKeymap();
		}

		private void OnApplyClick(object sender, MouseEventArgs args)
		{
			this.OnApply?.Invoke(sender, GetKeymap());
		}

		private void OnRemoveClick(object sender, MouseEventArgs args)
		{
			this.OnRemove?.Invoke(0, GetKeymap());
			DisposeEvents();
			((Control)this).Dispose();
		}

		public void DisposeEvents()
		{
			KeymapDropdown.remove_ValueChanged((EventHandler<ValueChangedEventArgs>)OnKeymapChanged);
			SpecializationDropdown.remove_ValueChanged((EventHandler<ValueChangedEventArgs>)OnSpecializationChanged);
			CharacterNameDropdown.remove_ValueChanged((EventHandler<ValueChangedEventArgs>)OnCharacterChanged);
			((Control)ApplyButton).remove_Click((EventHandler<MouseEventArgs>)OnApplyClick);
			((Control)RemoveButton).remove_Click((EventHandler<MouseEventArgs>)OnRemoveClick);
		}
	}
}
