using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Data;
using flakysalt.CharacterKeybinds.Presenter;
using flakysalt.CharacterKeybinds.Views.UiElements;

namespace flakysalt.CharacterKeybinds.Views
{
	public class CharacterKeybindsTab : View<CharacterKeybindSettingsPresenter>, IDisposable
	{
		private TabbedWindow2 WindowView;

		private StandardButton addEntryButton;

		private StandardButton applyDefaultKeybindButton;

		private FlowPanel scrollView;

		private FlowPanel mainFlowPanel;

		private FlowPanel keybindScrollView;

		private Label blockerOverlay;

		private Dropdown defaultKeybindDropdown;

		public EventHandler<string> OnApplyDefaultKeymapClicked;

		public EventHandler<string> OnDefaultKeymapChanged;

		public EventHandler OnAddButtonClicked;

		public void Dispose()
		{
			base.Unload();
		}

		private IView SomeViewMethod()
		{
			Logger.GetLogger<CharacterKeybindsTab>().Debug("test");
			return (IView)(object)this;
		}

		public CharacterKeybindsTab(ContentsManager ContentsManager)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Expected O, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Expected O, but got Unknown
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Expected O, but got Unknown
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Expected O, but got Unknown
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Expected O, but got Unknown
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Expected O, but got Unknown
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Expected O, but got Unknown
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Expected O, but got Unknown
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Expected O, but got Unknown
			TabbedWindow2 val = new TabbedWindow2(AsyncTexture2D.FromAssetId(155997), new Rectangle(24, 30, 545, 600), new Rectangle(82, 30, 467, 600));
			((WindowBase2)val).set_Emblem(ContentsManager.GetTexture("images/logo.png"));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Character Keybinds");
			((Control)val).set_Size(new Point(645, 700));
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id("flakysalt_CharacterKeybinds");
			((WindowBase2)val).set_CanClose(true);
			WindowView = val;
			Tab tab1 = new Tab(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("images/logo_small.png")), (Func<IView>)SomeViewMethod, "Character Keybinds", (int?)null);
			WindowView.get_Tabs().Add(tab1);
			WindowView.add_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)WindowView_TabChanged);
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)WindowView);
			((Control)val2).set_ZIndex(10);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val2).set_Size(((Control)WindowView).get_Size());
			((Control)val2).set_Visible(false);
			val2.set_Text("");
			((Control)val2).set_BackgroundColor(Color.get_Black());
			blockerOverlay = val2;
			FlowPanel val3 = new FlowPanel();
			val3.set_ControlPadding(new Vector2(0f, 10f));
			Rectangle contentRegion = ((Container)WindowView).get_ContentRegion();
			((Control)val3).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val3.set_FlowDirection((ControlFlowDirection)3);
			((Control)val3).set_Parent((Container)(object)WindowView);
			mainFlowPanel = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)mainFlowPanel);
			((Control)val4).set_Width(((Control)mainFlowPanel).get_Width());
			((Control)val4).set_BasicTooltipText("Applies these keybindings in case there are no specific ones setup for a character.");
			val4.set_Text("Default Keybinds");
			val4.set_Font(GameService.Content.get_DefaultFont18());
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Height(30);
			((Control)val5).set_Width(((Control)mainFlowPanel).get_Width());
			val5.set_FlowDirection((ControlFlowDirection)2);
			((Control)val5).set_Parent((Container)(object)mainFlowPanel);
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
			((Control)val9).set_Width(((Control)mainFlowPanel).get_Width());
			val9.set_FlowDirection((ControlFlowDirection)3);
			((Control)val9).set_Parent((Container)(object)mainFlowPanel);
			((Container)val9).set_HeightSizingMode((SizingMode)2);
			scrollView = val9;
			FlowPanel val10 = new FlowPanel();
			val10.set_OuterControlPadding(new Vector2(0f, 10f));
			((Control)val10).set_Width(((Control)scrollView).get_Width());
			val10.set_FlowDirection((ControlFlowDirection)3);
			((Control)val10).set_Parent((Container)(object)scrollView);
			((Container)val10).set_HeightSizingMode((SizingMode)1);
			keybindScrollView = val10;
			((Control)new Scrollbar((Container)(object)scrollView)).set_Height(((Control)scrollView).get_Height());
			StandardButton val11 = new StandardButton();
			val11.set_Text("+ Add Binding");
			((Control)val11).set_Parent((Container)(object)scrollView);
			((Control)val11).set_Width(((Control)scrollView).get_Width());
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
		}

		private void WindowView_TabChanged(object sender, ValueChangedEventArgs<Tab> e)
		{
			Logger.GetLogger<CharacterKeybindsTab>().Debug("test");
		}

		public void Show()
		{
			((Control)WindowView).Show();
		}

		public void SetAddButtonState(bool state)
		{
			((Control)addEntryButton).set_Enabled(state);
			addEntryButton.set_Text(state ? "+ Add Binding" : "Add Binding (Loading Characters...)");
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

		public void ToggleWindow()
		{
			((WindowBase2)WindowView).ToggleWindow();
		}

		public void SetBlocker(bool visibility)
		{
			SetAddButtonState(!visibility);
			blockerOverlay.set_Text("API token missing or not available yet.\n\nMake sure you have added an API token to Blish HUD \nand it has the neccessary permissions!\n(Previously setup keybinds will still work!)");
			((Control)blockerOverlay).set_Visible(visibility);
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

		public void SetKeybindOptions(KeybindFlowContainer keybindFlowContainer, List<string> charaters, List<string> specializations, List<string> keymaps)
		{
			keybindFlowContainer.SetDropdownContent(keybindFlowContainer.characterNameDropdown, charaters);
			keybindFlowContainer.SetDropdownContent(keybindFlowContainer.specializationDropdown, specializations);
			keybindFlowContainer.SetDropdownContent(keybindFlowContainer.keymapDropdown, keymaps);
		}

		public void SetKeybindValues(KeybindFlowContainer keybindFlowContainer, CharacterKeybind characterKeybind, int iconId)
		{
			keybindFlowContainer.SetValues(characterKeybind);
			keybindFlowContainer.SetProfessionIcon(iconId);
		}

		public void AttachListeners(KeybindFlowContainer keybindFlowContainer, EventHandler<CharacterKeybind> OnApplyAction, EventHandler<KeymapEventArgs> OnDataChanged, EventHandler<CharacterKeybind> OnDeleteAction)
		{
			keybindFlowContainer.AttachListeners(OnApplyAction, OnDataChanged, OnDeleteAction);
		}

		public void ClearKeybindEntries()
		{
			for (int i = ((Container)keybindScrollView).get_Children().get_Count() - 1; i >= 0; i--)
			{
				((Container)keybindScrollView).RemoveChild(((Container)keybindScrollView).get_Children().get_Item(i));
			}
		}
	}
}
