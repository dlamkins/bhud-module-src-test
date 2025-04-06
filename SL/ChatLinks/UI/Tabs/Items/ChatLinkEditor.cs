using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;
using SL.ChatLinks.UI.Tabs.Items.Upgrades;
using SL.Common;
using SL.Common.Controls;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed class ChatLinkEditor : FlowPanel
	{
		private readonly Image _itemIcon;

		private readonly Label _itemName;

		private readonly NumberInput _quantity;

		private readonly TrackBar _quantitySlider;

		private readonly TextBox _chatLink;

		private readonly Label _infusionWarning;

		public ChatLinkEditorViewModel ViewModel { get; }

		public ChatLinkEditor(ChatLinkEditorViewModel viewModel)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Expected O, but got Unknown
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Expected O, but got Unknown
			//IL_0126: Expected O, but got Unknown
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Expected O, but got Unknown
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Expected O, but got Unknown
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Expected O, but got Unknown
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Expected O, but got Unknown
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_040a: Expected O, but got Unknown
			//IL_040b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			//IL_0427: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Expected O, but got Unknown
			//IL_0575: Unknown result type (might be due to invalid IL or missing references)
			//IL_057a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0581: Unknown result type (might be due to invalid IL or missing references)
			//IL_058c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0594: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_05da: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0605: Expected O, but got Unknown
			//IL_0634: Unknown result type (might be due to invalid IL or missing references)
			//IL_0639: Unknown result type (might be due to invalid IL or missing references)
			//IL_0641: Unknown result type (might be due to invalid IL or missing references)
			//IL_064c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0659: Expected O, but got Unknown
			//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e0: Expected O, but got Unknown
			//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_071f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0724: Unknown result type (might be due to invalid IL or missing references)
			//IL_072b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0736: Unknown result type (might be due to invalid IL or missing references)
			//IL_073d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0744: Unknown result type (might be due to invalid IL or missing references)
			//IL_0745: Unknown result type (might be due to invalid IL or missing references)
			//IL_074f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0765: Expected O, but got Unknown
			ChatLinkEditorViewModel viewModel2 = viewModel;
			((FlowPanel)this)._002Ector();
			ChatLinkEditor chatLinkEditor = this;
			ThrowHelper.ThrowIfNull(viewModel2, "viewModel");
			ViewModel = viewModel2;
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, 15f));
			((FlowPanel)this).set_OuterControlPadding(new Vector2(20f));
			((Container)this).set_AutoSizePadding(new Point(10));
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Container)this).set_HeightSizingMode((SizingMode)2);
			((Panel)this).set_CanScroll(true);
			((Panel)this).set_ShowBorder(true);
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)2);
			val.set_ControlPadding(new Vector2(5f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(50);
			((Control)val).set_Parent((Container)(object)this);
			FlowPanel header = val;
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)header);
			val2.set_Texture(viewModel2.GetIcon());
			((Control)val2).set_Size(new Point(50));
			((Control)val2).set_Menu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)(() => new _003C_003Ez__ReadOnlyArray<ContextMenuStripItem>((ContextMenuStripItem[])(object)new ContextMenuStripItem[4]
			{
				chatLinkEditor.ViewModel.CopyNameCommand.ToMenuItem(() => chatLinkEditor.ViewModel.CopyNameLabel),
				chatLinkEditor.ViewModel.CopyChatLinkCommand.ToMenuItem(() => viewModel2.CopyChatLinkLabel),
				chatLinkEditor.ViewModel.OpenWikiCommand.ToMenuItem(() => viewModel2.OpenWikiLabel),
				chatLinkEditor.ViewModel.OpenApiCommand.ToMenuItem(() => viewModel2.OpenApiLabel)
			}))));
			_itemIcon = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)header);
			val3.set_TextColor(viewModel2.ItemNameColor);
			((Control)val3).set_Width(400);
			((Control)val3).set_Height(50);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			val3.set_Font(GameService.Content.get_DefaultFont18());
			val3.set_WrapText(true);
			_itemName = val3;
			Binder.Bind(viewModel2, (ChatLinkEditorViewModel vm) => vm.ItemName, _itemName);
			foreach (UpgradeEditorViewModel upgradeEditorViewModel in viewModel2.UpgradeEditorViewModels)
			{
				((Control)new UpgradeEditor(upgradeEditorViewModel)).set_Parent((Container)(object)this);
			}
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_FlowDirection((ControlFlowDirection)0);
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			val4.set_ControlPadding(new Vector2(5f));
			FlowPanel quantityGroup = val4;
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_FlowDirection((ControlFlowDirection)0);
			((Container)val5).set_WidthSizingMode((SizingMode)2);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			val5.set_ControlPadding(new Vector2(5f));
			FlowPanel chatLinkGroup = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)quantityGroup);
			val6.set_AutoSizeWidth(true);
			((Control)val6).set_Height(32);
			Label stackSizeLabel = val6;
			Binder.Bind(viewModel2, (ChatLinkEditorViewModel vm) => vm.StackSizeLabel, stackSizeLabel);
			NumberInput numberInput = new NumberInput();
			((Control)numberInput).set_Parent((Container)(object)quantityGroup);
			((Control)numberInput).set_Width(80);
			numberInput.MinValue = 1;
			_quantity = numberInput;
			Binder.Bind(viewModel2, (ChatLinkEditorViewModel vm) => vm.Quantity, _quantity);
			Binder.Bind<ChatLinkEditorViewModel, NumberInput, int>(viewModel2, (Expression<Func<ChatLinkEditorViewModel, int>>)((ChatLinkEditorViewModel vm) => vm.MaxStackSize), _quantity, (Expression<Func<NumberInput, int>>)((NumberInput ctl) => ctl.MaxValue), BindingMode.ToView);
			Panel val7 = new Panel();
			((Control)val7).set_Parent((Container)(object)quantityGroup);
			((Control)val7).set_Width(80);
			((Control)val7).set_Height(32);
			Panel quantitySliderDiv = val7;
			TrackBar val8 = new TrackBar();
			((Control)val8).set_Parent((Container)(object)quantitySliderDiv);
			((Control)val8).set_Width(80);
			((Control)val8).set_Top(8);
			val8.set_MinValue(1f);
			_quantitySlider = val8;
			Binder.Bind<ChatLinkEditorViewModel, TrackBar, float>(viewModel2, (Expression<Func<ChatLinkEditorViewModel, float>>)((ChatLinkEditorViewModel vm) => vm.Quantity), _quantitySlider, (Expression<Func<TrackBar, float>>)((TrackBar ctl) => ctl.get_Value()), BindingMode.Bidirectional);
			Binder.Bind<ChatLinkEditorViewModel, TrackBar, float>(viewModel2, (Expression<Func<ChatLinkEditorViewModel, float>>)((ChatLinkEditorViewModel vm) => vm.MaxStackSize), _quantitySlider, (Expression<Func<TrackBar, float>>)((TrackBar ctl) => ctl.get_MaxValue()), BindingMode.ToView);
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)quantityGroup);
			val9.set_Text("250");
			((Control)val9).set_Width(50);
			((Control)val9).set_Height(32);
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)MaxQuantityClicked);
			GlowButton val10 = new GlowButton();
			((Control)val10).set_Parent((Container)(object)quantityGroup);
			((Control)val10).set_Width(32);
			((Control)val10).set_Height(32);
			val10.set_Icon(AsyncTexture2D.FromAssetId(157324));
			val10.set_ActiveIcon(AsyncTexture2D.FromAssetId(157325));
			((Control)val10).set_BasicTooltipText(viewModel2.ResetTooltip);
			GlowButton resetQuantity = val10;
			ViewModel.PropertyChanged += delegate(object _, PropertyChangedEventArgs args)
			{
				if (args.PropertyName == "ResetTooltip")
				{
					((Control)resetQuantity).set_BasicTooltipText(chatLinkEditor.ViewModel.ResetTooltip);
				}
			};
			((Control)resetQuantity).add_Click((EventHandler<MouseEventArgs>)ResetQuantityClicked);
			TextBox val11 = new TextBox();
			((Control)val11).set_Parent((Container)(object)chatLinkGroup);
			((Control)val11).set_Width(350);
			((Control)val11).set_Height(32);
			_chatLink = val11;
			Binder.Bind(ViewModel, (ChatLinkEditorViewModel vm) => vm.ChatLink, _chatLink, BindingMode.ToView);
			((Control)_chatLink).add_Click((EventHandler<MouseEventArgs>)ChatLinkClicked);
			((Control)_chatLink).set_Menu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)(() => new _003C_003Ez__ReadOnlyArray<ContextMenuStripItem>((ContextMenuStripItem[])(object)new ContextMenuStripItem[1] { viewModel2.CopyChatLinkCommand.ToMenuItem(() => viewModel2.CopyChatLinkLabel) }))));
			GlowButton val12 = new GlowButton();
			((Control)val12).set_Parent((Container)(object)chatLinkGroup);
			val12.set_Icon(AsyncTexture2D.FromAssetId(2208345));
			val12.set_ActiveIcon(AsyncTexture2D.FromAssetId(2208347));
			((Control)val12).add_Click((EventHandler<MouseEventArgs>)OnCopyClicked);
			Label val13 = new Label();
			((Control)val13).set_Parent((Container)(object)this);
			((Control)val13).set_Width(350);
			val13.set_AutoSizeHeight(true);
			val13.set_WrapText(true);
			val13.set_TextColor(Color.get_Yellow());
			((Control)val13).set_Visible(ViewModel.ShowInfusionWarning);
			_infusionWarning = val13;
			Binder.Bind(viewModel2, (ChatLinkEditorViewModel vm) => vm.InfusionWarning, _infusionWarning);
			Control.get_Input().get_Mouse().add_MouseWheelScrolled((EventHandler<MouseEventArgs>)OnGlobalMouseWheelScrolled);
		}

		private void OnCopyClicked(object sender, MouseEventArgs e)
		{
			Soundboard.Click();
			ViewModel.CopyChatLinkCommand.Execute();
		}

		private void OnGlobalMouseWheelScrolled(object sender, MouseEventArgs e)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)_quantitySlider).get_MouseOver())
			{
				MouseState state = Control.get_Input().get_Mouse().get_State();
				if (((MouseState)(ref state)).get_ScrollWheelValue() > 0)
				{
					TrackBar quantitySlider = _quantitySlider;
					float value = quantitySlider.get_Value();
					quantitySlider.set_Value(value + 1f);
				}
				else
				{
					TrackBar quantitySlider2 = _quantitySlider;
					float value = quantitySlider2.get_Value();
					quantitySlider2.set_Value(value - 1f);
				}
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			//IL_0039: Expected O, but got Unknown
			if (((Control)_itemIcon).get_MouseOver())
			{
				Image itemIcon = _itemIcon;
				if (((Control)itemIcon).get_Tooltip() == null)
				{
					Tooltip val = new Tooltip((ITooltipView)(object)new ItemTooltipView(ViewModel.CreateTooltipViewModel()));
					Tooltip val2 = val;
					((Control)itemIcon).set_Tooltip(val);
				}
			}
			else
			{
				Tooltip tooltip = ((Control)_itemIcon).get_Tooltip();
				if (tooltip != null)
				{
					((Control)tooltip).Dispose();
				}
				((Control)_itemIcon).set_Tooltip((Tooltip)null);
			}
			((Control)_infusionWarning).set_Visible(ViewModel.ShowInfusionWarning);
		}

		protected override void OnMouseWheelScrolled(MouseEventArgs e)
		{
			if (ViewModel.AllowScroll)
			{
				((Control)this).OnMouseWheelScrolled(e);
			}
		}

		private void MaxQuantityClicked(object sender, MouseEventArgs e)
		{
			Soundboard.Click();
			ViewModel.MaxQuantityCommand.Execute();
		}

		private void ResetQuantityClicked(object sender, MouseEventArgs e)
		{
			Soundboard.Click();
			ViewModel.MinQuantityCommand.Execute();
		}

		private void ChatLinkClicked(object sender, MouseEventArgs e)
		{
			((TextInputBase)_chatLink).set_SelectionStart(0);
			((TextInputBase)_chatLink).set_SelectionEnd(((TextInputBase)_chatLink).get_Text().Length);
		}

		protected override void DisposeControl()
		{
			Control.get_Input().get_Mouse().remove_MouseWheelScrolled((EventHandler<MouseEventArgs>)OnGlobalMouseWheelScrolled);
			ViewModel.Dispose();
			((FlowPanel)this).DisposeControl();
		}
	}
}
