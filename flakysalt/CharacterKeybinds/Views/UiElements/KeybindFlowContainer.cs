using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Data;
using flakysalt.CharacterKeybinds.Resources;

namespace flakysalt.CharacterKeybinds.Views.UiElements
{
	public sealed class KeybindFlowContainer : FlowPanel
	{
		private readonly int _minDropdownWidth = 130;

		private readonly int maxDropdownHeight = 300;

		private string WildcardSpecialization = Loca.wildcardSpecializationName;

		private string InvalidSpecialization = Loca.invalidSpecializationName;

		private Keymap _oldCharacterKeymap;

		private List<LocalizedSpecialization> _localizedSpecializations;

		private StandardButton RemoveButton { get; }

		private StandardButton ApplyButton { get; }

		private Image ProfessionImage { get; }

		public Dropdown<string> CharacterNameDropdown { get; }

		public Dropdown<string> SpecializationDropdown { get; }

		public Dropdown<string> KeymapDropdown { get; }

		private string DefaultCharacterEntry => Loca.defaultCharacterEntry;

		private string DefaultKeybindsEntry => Loca.defaultKeybindsEntry;

		private string DefaultSpecializationEntry => Loca.defaultSpecializationEntry;

		private string CoreSpecialization => Loca.coreSpecializationName;

		public event EventHandler<Keymap> OnApply;

		public event EventHandler<KeymapEventArgs> OnDataChanged;

		public event EventHandler<Keymap> OnRemove;

		public KeybindFlowContainer()
			: this()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Expected O, but got Unknown
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Expected O, but got Unknown
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Expected O, but got Unknown
			((FlowPanel)this).set_OuterControlPadding(new Vector2(10f, 0f));
			((FlowPanel)this).set_ControlPadding(new Vector2(2f, 0f));
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)0);
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Panel)this).set_CanScroll(false);
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(new Point(32, 32));
			ProfessionImage = val;
			Dropdown<string> dropdown = new Dropdown<string>();
			((Control)dropdown).set_Height(30);
			((Control)dropdown).set_Parent((Container)(object)this);
			((Control)dropdown).set_Width(_minDropdownWidth);
			dropdown.PanelHeight = maxDropdownHeight;
			CharacterNameDropdown = dropdown;
			Dropdown<string> dropdown2 = new Dropdown<string>();
			((Control)dropdown2).set_Padding(new Thickness(22f, 0f, 0f, 0f));
			((Control)dropdown2).set_Height(30);
			((Control)dropdown2).set_Parent((Container)(object)this);
			((Control)dropdown2).set_Width(_minDropdownWidth);
			dropdown2.PanelHeight = maxDropdownHeight;
			SpecializationDropdown = dropdown2;
			SpecializationDropdown.Items.Add(WildcardSpecialization);
			SpecializationDropdown.Items.Add(CoreSpecialization);
			Dropdown<string> dropdown3 = new Dropdown<string>();
			((Control)dropdown3).set_Height(30);
			((Control)dropdown3).set_Parent((Container)(object)this);
			((Control)dropdown3).set_Width(_minDropdownWidth);
			dropdown3.PanelHeight = maxDropdownHeight;
			KeymapDropdown = dropdown3;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(Loca.apply);
			((Control)val2).set_Size(new Point(60, 30));
			ApplyButton = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text(Loca.delete);
			((Control)val3).set_Size(new Point(60, 30));
			RemoveButton = val3;
			KeymapDropdown.ValueChanged += OnKeymapChanged;
			SpecializationDropdown.ValueChanged += OnSpecializationChanged;
			CharacterNameDropdown.ValueChanged += OnCharacterChanged;
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

		public void SetDropdownContent(Dropdown<string> dropdown, List<string> values)
		{
			values.ForEach(delegate(string e)
			{
				dropdown.Items.Add(e);
			});
		}

		public void SetSpecializationContent(List<LocalizedSpecialization> values)
		{
			_localizedSpecializations = values;
			values.ForEach(delegate(LocalizedSpecialization e)
			{
				SpecializationDropdown.Items.Add(e.displayName);
			});
		}

		public void SetValues(Keymap keymap)
		{
			_oldCharacterKeymap = keymap;
			CharacterNameDropdown.SelectedItem = (string.IsNullOrEmpty(keymap.CharacterName) ? DefaultCharacterEntry : keymap.CharacterName);
			switch (keymap.SpecialisationId)
			{
			case 0:
				SpecializationDropdown.SelectedItem = DefaultSpecializationEntry;
				break;
			case -1:
				SpecializationDropdown.SelectedItem = CoreSpecialization;
				break;
			case -2:
				SpecializationDropdown.SelectedItem = WildcardSpecialization;
				break;
			case -10:
				SpecializationDropdown.SelectedItem = InvalidSpecialization;
				break;
			default:
				SpecializationDropdown.SelectedItem = _localizedSpecializations.FirstOrDefault((LocalizedSpecialization e) => e.id == keymap.SpecialisationId)?.displayName;
				break;
			}
			KeymapDropdown.SelectedItem = (string.IsNullOrEmpty(keymap.KeymapName) ? DefaultKeybindsEntry : keymap.KeymapName);
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
			if (SpecializationDropdown.Items.Contains(SpecializationDropdown.SelectedItem) || SpecializationDropdown.SelectedItem == InvalidSpecialization)
			{
				specialisationId = SpecializationDropdown.Items.IndexOf(SpecializationDropdown.SelectedItem) switch
				{
					0 => -2, 
					1 => -1, 
					_ => _localizedSpecializations.FirstOrDefault((LocalizedSpecialization e) => e.displayName == SpecializationDropdown.SelectedItem)?.id ?? (-10), 
				};
			}
			return new Keymap
			{
				CharacterName = (CharacterNameDropdown.Items.Contains(CharacterNameDropdown.SelectedItem) ? CharacterNameDropdown.SelectedItem : null),
				SpecialisationId = specialisationId,
				KeymapName = (KeymapDropdown.Items.Contains(KeymapDropdown.SelectedItem) ? KeymapDropdown.SelectedItem : null)
			};
		}

		private void OnKeymapChanged(object sender, ValueChangedEventArgs<string> args)
		{
			((Control)ApplyButton).set_Enabled(KeymapDropdown.SelectedItem != DefaultKeybindsEntry);
			this.OnDataChanged?.Invoke(this, GetKeymapArgs());
			_oldCharacterKeymap = GetKeymap();
		}

		private void OnSpecializationChanged(object sender, ValueChangedEventArgs<string> args)
		{
			this.OnDataChanged?.Invoke(this, GetKeymapArgs());
			_oldCharacterKeymap = GetKeymap();
		}

		private void OnCharacterChanged(object sender, ValueChangedEventArgs<string> args)
		{
			SpecializationDropdown.SelectedItem = DefaultSpecializationEntry;
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
			KeymapDropdown.ValueChanged -= OnKeymapChanged;
			SpecializationDropdown.ValueChanged -= OnSpecializationChanged;
			CharacterNameDropdown.ValueChanged -= OnCharacterChanged;
			((Control)ApplyButton).remove_Click((EventHandler<MouseEventArgs>)OnApplyClick);
			((Control)RemoveButton).remove_Click((EventHandler<MouseEventArgs>)OnRemoveClick);
		}
	}
}
