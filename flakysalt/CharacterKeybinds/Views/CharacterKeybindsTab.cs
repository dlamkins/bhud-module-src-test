using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Data;
using flakysalt.CharacterKeybinds.Resources;
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

		private Dropdown<string> defaultKeybindDropdown;

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
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Expected O, but got Unknown
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Expected O, but got Unknown
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Expected O, but got Unknown
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Expected O, but got Unknown
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Expected O, but got Unknown
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Expected O, but got Unknown
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Expected O, but got Unknown
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
			((Control)val2).set_BasicTooltipText(Loca.apiLoadingHint);
			contentRegion = buildPanel.get_ContentRegion();
			int num = ((Rectangle)(ref contentRegion)).get_Size().X / 2 - 32;
			contentRegion = buildPanel.get_ContentRegion();
			((Control)val2).set_Location(new Point(num, ((Rectangle)(ref contentRegion)).get_Size().Y / 2 - 32));
			((Control)val2).set_Size(new Point(64, 64));
			((Control)val2).set_ZIndex(100);
			((Control)val2).set_Visible(false);
			_spinner = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)mainFlowPanel);
			((Control)val3).set_Width(((Control)mainFlowPanel).get_Width());
			((Control)val3).set_BasicTooltipText(Loca.defaultKeybindHint);
			val3.set_Text(Loca.defaultKeybind);
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
			Dropdown<string> dropdown = new Dropdown<string>();
			((Control)dropdown).set_Parent((Container)(object)defaultKeybindFlowPanel);
			((Control)dropdown).set_Height(30);
			dropdown.PanelHeight = 300;
			defaultKeybindDropdown = dropdown;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Width(60);
			((Control)val6).set_Height(30);
			val6.set_Text(Loca.apply);
			((Control)val6).set_Parent((Container)(object)defaultKeybindFlowPanel);
			applyDefaultKeybindButton = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)mainFlowPanel);
			((Control)val7).set_Width(((Control)mainFlowPanel).get_Width());
			((Control)val7).set_BasicTooltipText(Loca.characterSpecificKeybindsHint);
			val7.set_Text(Loca.characterSpecificKeybinds);
			val7.set_Font(GameService.Content.get_DefaultFont18());
			FlowPanel val8 = new FlowPanel();
			((Panel)val8).set_CanScroll(true);
			((Panel)val8).set_ShowBorder(true);
			val8.set_FlowDirection((ControlFlowDirection)3);
			((Control)val8).set_Parent((Container)(object)mainFlowPanel);
			((Container)val8).set_HeightSizingMode((SizingMode)2);
			((Container)val8).set_WidthSizingMode((SizingMode)2);
			scrollView = val8;
			FlowPanel val9 = new FlowPanel();
			val9.set_OuterControlPadding(new Vector2(0f, 10f));
			val9.set_ControlPadding(new Vector2(0f, 5f));
			val9.set_FlowDirection((ControlFlowDirection)3);
			((Control)val9).set_Parent((Container)(object)scrollView);
			((Container)val9).set_HeightSizingMode((SizingMode)1);
			((Container)val9).set_WidthSizingMode((SizingMode)2);
			keybindScrollView = val9;
			((Control)new Scrollbar((Container)(object)scrollView)).set_Height(((Control)scrollView).get_Height());
			StandardButton val10 = new StandardButton();
			val10.set_Text(Loca.addNewBindingButtonText);
			((Control)val10).set_Parent((Container)(object)scrollView);
			contentRegion = buildPanel.get_ContentRegion();
			((Control)val10).set_Width(((Rectangle)(ref contentRegion)).get_Size().X);
			addEntryButton = val10;
			((Control)addEntryButton).add_Click((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs args)
			{
				OnAddButtonClicked?.Invoke(sender, (EventArgs)(object)args);
			});
			((Control)applyDefaultKeybindButton).add_Click((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs args)
			{
				OnApplyDefaultKeymapClicked?.Invoke(sender, defaultKeybindDropdown.SelectedItem);
			});
			defaultKeybindDropdown.ValueChanged += delegate(object sender, ValueChangedEventArgs<string> args)
			{
				OnDefaultKeymapChanged?.Invoke(sender, args.get_NewValue());
			};
			((Control)buildPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0081: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
				FlowPanel obj = mainFlowPanel;
				Rectangle contentRegion2 = buildPanel.get_ContentRegion();
				((Control)obj).set_Size(((Rectangle)(ref contentRegion2)).get_Size());
				StandardButton obj2 = addEntryButton;
				contentRegion2 = buildPanel.get_ContentRegion();
				((Control)obj2).set_Width(((Rectangle)(ref contentRegion2)).get_Size().X);
				LoadingSpinner spinner = _spinner;
				contentRegion2 = buildPanel.get_ContentRegion();
				int num2 = ((Rectangle)(ref contentRegion2)).get_Size().X / 2 - 32;
				contentRegion2 = buildPanel.get_ContentRegion();
				((Control)spinner).set_Location(new Point(num2, ((Rectangle)(ref contentRegion2)).get_Size().Y / 2 - 32));
				((Control)errorInfoIcon).set_Location(new Point(((Control)mainFlowPanel).get_Right() - 64, ((Control)mainFlowPanel).get_Top() + 16));
			});
			((View<IPresenter>)this).Build(buildPanel);
		}

		public void SetSpinner(bool state)
		{
			((Control)_spinner).set_Visible(state);
		}

		public async Task SetErrorInfoIcon(Task<List<string>> errorTask, bool isDataLoaded)
		{
			string errorMessage2 = string.Empty;
			List<string> errors = await errorTask;
			bool containsErrors = errors.Count > 0;
			if (containsErrors)
			{
				errorMessage2 = $"{errors.Count} {Loca.errorMessageIssueCounter}:\n\n";
				errorMessage2 += string.Join(Environment.NewLine, errors);
			}
			((Control)errorInfoIcon).set_BasicTooltipText(errorMessage2);
			((Control)errorInfoIcon).set_Visible(containsErrors);
			SetKeybindContainerEnabled(!containsErrors && isDataLoaded);
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
			defaultKeybindDropdown.Items.Clear();
			options.ForEach(delegate(string e)
			{
				defaultKeybindDropdown.Items.Add(e);
			});
			defaultKeybindDropdown.SelectedItem = selectedOption;
		}

		public KeybindFlowContainer AddKeybind()
		{
			KeybindFlowContainer keybindFlowContainer = new KeybindFlowContainer();
			((Control)keybindFlowContainer).set_Parent((Container)(object)keybindScrollView);
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

		public void AttachListeners(KeybindFlowContainer keybindFlowContainer, EventHandler<Keymap> onApplyAction, EventHandler<KeymapEventArgs> onDataChanged, EventHandler<Keymap> onDeleteAction)
		{
			keybindFlowContainer.AttachListeners(onApplyAction, onDataChanged, onDeleteAction);
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
