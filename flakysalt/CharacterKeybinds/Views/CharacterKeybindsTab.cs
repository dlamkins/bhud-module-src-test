using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Data;
using flakysalt.CharacterKeybinds.Views.UiElements;

namespace flakysalt.CharacterKeybinds.Views
{
	public class CharacterKeybindsTab : View
	{
		private StandardButton addEntryButton;

		private StandardButton applyDefaultKeybindButton;

		private FlowPanel scrollView;

		private FlowPanel mainFlowPanel;

		private FlowPanel keybindScrollView;

		private Dropdown defaultKeybindDropdown;

		private LoadingSpinner _spinner;

		private Label defaultKeybindsLabel;

		private Image errorInfoIcon;

		public EventHandler<string> OnApplyDefaultKeymapClicked;

		public EventHandler<string> OnDefaultKeymapChanged;

		public EventHandler OnAddButtonClicked;

		protected override void Build(Container buildPanel)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Expected O, but got Unknown
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Expected O, but got Unknown
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Expected O, but got Unknown
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Expected O, but got Unknown
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Expected O, but got Unknown
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Expected O, but got Unknown
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Expected O, but got Unknown
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Expected O, but got Unknown
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_ControlPadding(new Vector2(0f, 10f));
			((Container)val).set_HeightSizingMode((SizingMode)2);
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Parent(buildPanel);
			mainFlowPanel = val;
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_BasicTooltipText("Loading Data from API...");
			((Control)val2).set_Location(new Point(((Control)mainFlowPanel).get_Width() / 2 - 32, ((Control)mainFlowPanel).get_Height() / 2 - 32));
			((Control)val2).set_Size(new Point(64, 64));
			((Control)val2).set_ZIndex(100);
			((Control)val2).set_Visible(false);
			_spinner = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)mainFlowPanel);
			((Control)val3).set_Width(((Control)mainFlowPanel).get_Width());
			((Control)val3).set_BasicTooltipText("Applies these keybindings in case there are no specific ones setup for a character.\nCan be disabled in the settings.");
			val3.set_Text("Default Keybinds");
			val3.set_Font(GameService.Content.get_DefaultFont18());
			defaultKeybindsLabel = val3;
			AsyncTexture2D texture = AsyncTexture2D.FromAssetId(155018);
			Image val4 = new Image(texture);
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Size(new Point(48, 48));
			((Control)val4).set_Visible(false);
			((Control)val4).set_Location(new Point(((Control)mainFlowPanel).get_Right() - 64, ((Control)mainFlowPanel).get_Top() + 16));
			errorInfoIcon = val4;
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Height(30);
			val5.set_FlowDirection((ControlFlowDirection)2);
			((Control)val5).set_Parent((Container)(object)mainFlowPanel);
			((Container)val5).set_WidthSizingMode((SizingMode)2);
			FlowPanel defaultKeybindFlowPanel = val5;
			Dropdown val6 = new Dropdown();
			((Control)val6).set_Parent((Container)(object)defaultKeybindFlowPanel);
			((Control)val6).set_Height(30);
			defaultKeybindDropdown = val6;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Width(60);
			((Control)val7).set_Height(30);
			val7.set_Text("Apply");
			((Control)val7).set_Parent((Container)(object)defaultKeybindFlowPanel);
			applyDefaultKeybindButton = val7;
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)mainFlowPanel);
			((Control)val8).set_Width(((Control)mainFlowPanel).get_Width());
			((Control)val8).set_BasicTooltipText("Keybinding to use for a specific character or specializations");
			val8.set_Text("Character Specific Keybinds");
			val8.set_Font(GameService.Content.get_DefaultFont18());
			FlowPanel val9 = new FlowPanel();
			((Panel)val9).set_CanScroll(true);
			((Panel)val9).set_ShowBorder(true);
			val9.set_FlowDirection((ControlFlowDirection)3);
			((Control)val9).set_Parent((Container)(object)mainFlowPanel);
			((Container)val9).set_HeightSizingMode((SizingMode)2);
			((Container)val9).set_WidthSizingMode((SizingMode)2);
			scrollView = val9;
			FlowPanel val10 = new FlowPanel();
			val10.set_OuterControlPadding(new Vector2(0f, 10f));
			val10.set_ControlPadding(new Vector2(0f, 5f));
			val10.set_FlowDirection((ControlFlowDirection)3);
			((Control)val10).set_Parent((Container)(object)scrollView);
			((Container)val10).set_HeightSizingMode((SizingMode)1);
			((Container)val10).set_WidthSizingMode((SizingMode)2);
			keybindScrollView = val10;
			((Control)new Scrollbar((Container)(object)scrollView)).set_Height(((Control)scrollView).get_Height());
			StandardButton val11 = new StandardButton();
			val11.set_Text("+ Add Binding");
			((Control)val11).set_Parent((Container)(object)scrollView);
			contentRegion = buildPanel.get_ContentRegion();
			((Control)val11).set_Width(((Rectangle)(ref contentRegion)).get_Size().X);
			addEntryButton = val11;
			((Control)addEntryButton).add_Click((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs args)
			{
				OnAddButtonClicked?.Invoke(sender, (EventArgs)(object)args);
			});
			((Control)applyDefaultKeybindButton).add_Click((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs args)
			{
				OnApplyDefaultKeymapClicked?.Invoke(sender, defaultKeybindDropdown.get_SelectedItem());
			});
			defaultKeybindDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object sender, ValueChangedEventArgs args)
			{
				OnDefaultKeymapChanged?.Invoke(sender, args.get_CurrentValue());
			});
			((Control)buildPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_0098: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
				Logger logger = Logger.GetLogger<CharacterKeybindsTab>();
				object arg = ((Control)buildPanel).get_Size();
				Rectangle contentRegion2 = buildPanel.get_ContentRegion();
				logger.Debug($"Window{arg}| Content: {((Rectangle)(ref contentRegion2)).get_Size()},");
				FlowPanel obj = mainFlowPanel;
				contentRegion2 = buildPanel.get_ContentRegion();
				((Control)obj).set_Size(((Rectangle)(ref contentRegion2)).get_Size());
				StandardButton obj2 = addEntryButton;
				contentRegion2 = buildPanel.get_ContentRegion();
				((Control)obj2).set_Width(((Rectangle)(ref contentRegion2)).get_Size().X);
				((Control)_spinner).set_Location(new Point(buildPanel.get_ContentBounds().X / 2 - 32, buildPanel.get_ContentBounds().Y / 2 - 32));
				((Control)errorInfoIcon).set_Location(new Point(((Control)mainFlowPanel).get_Right() - 64, ((Control)mainFlowPanel).get_Top() + 16));
			});
			((View<IPresenter>)this).Build(buildPanel);
		}

		public void SetSpinner(bool state)
		{
			((Control)_spinner).set_Visible(state);
		}

		public void SetErrorInfoIcon(bool isValid, bool isDataLoaded, string error)
		{
			((Control)errorInfoIcon).set_BasicTooltipText(error);
			((Control)errorInfoIcon).set_Visible(!isValid);
			SetKeybindContainerEnabled(isValid && isDataLoaded);
		}

		private void SetKeybindContainerEnabled(bool enabled)
		{
			foreach (KeybindFlowContainer item in ((Container)keybindScrollView).GetChildrenOfType<KeybindFlowContainer>())
			{
				item.SetEnabled(enabled);
			}
		}

		public void SetDefaultKeybindOptions(List<string> options, string selectedOption)
		{
			defaultKeybindDropdown.get_Items().Clear();
			options.ForEach(delegate(string e)
			{
				defaultKeybindDropdown.get_Items().Add(e);
			});
			defaultKeybindDropdown.set_SelectedItem(selectedOption);
		}

		public KeybindFlowContainer AddKeybind()
		{
			KeybindFlowContainer keybindFlowContainer = new KeybindFlowContainer();
			((Control)keybindFlowContainer).set_Parent((Container)(object)keybindScrollView);
			((Control)keybindFlowContainer).set_Width(((Control)keybindScrollView).get_Width());
			((Panel)keybindFlowContainer).set_CanScroll(false);
			((FlowPanel)keybindFlowContainer).set_FlowDirection((ControlFlowDirection)0);
			return keybindFlowContainer;
		}

		public void SetKeybindOptions(KeybindFlowContainer keybindFlowContainer, List<string> characterList, List<LocalizedSpecialization> specializations, List<string> keymaps)
		{
			keybindFlowContainer.SetDropdownContent(keybindFlowContainer.CharacterNameDropdown, characterList);
			keybindFlowContainer.SetSpecializationContent(specializations);
			keybindFlowContainer.SetDropdownContent(keybindFlowContainer.KeymapDropdown, keymaps);
		}

		public void SetKeybindValues(KeybindFlowContainer keybindFlowContainer, Keymap characterKeybind, int iconId)
		{
			keybindFlowContainer.SetValues(characterKeybind);
			keybindFlowContainer.SetProfessionIcon(iconId);
		}

		public void AttachListeners(KeybindFlowContainer keybindFlowContainer, EventHandler<Keymap> onApplyAction, EventHandler<KeymapEventArgs> onDataChanged, EventHandler<Keymap> OnDeleteAction)
		{
			keybindFlowContainer.AttachListeners(onApplyAction, onDataChanged, OnDeleteAction);
		}

		public void ClearKeybindEntries()
		{
			for (int i = ((Container)keybindScrollView).get_Children().get_Count() - 1; i >= 0; i--)
			{
				((KeybindFlowContainer)(object)((Container)keybindScrollView).get_Children().get_Item(i)).DisposeEvents();
				((Container)keybindScrollView).RemoveChild(((Container)keybindScrollView).get_Children().get_Item(i));
			}
		}

		public CharacterKeybindsTab()
			: this()
		{
		}
	}
}
