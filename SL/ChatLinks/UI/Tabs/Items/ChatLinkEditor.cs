using System;
using System.Collections.Generic;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
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

		private readonly TextBox _chatLink;

		private readonly Label _infusionWarning;

		public ChatLinkEditorViewModel ViewModel { get; }

		public ChatLinkEditor(ChatLinkEditorViewModel viewModel)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Expected O, but got Unknown
			//IL_0111: Expected O, but got Unknown
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Expected O, but got Unknown
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Expected O, but got Unknown
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Expected O, but got Unknown
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0425: Expected O, but got Unknown
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_0459: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Expected O, but got Unknown
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Expected O, but got Unknown
			//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0504: Unknown result type (might be due to invalid IL or missing references)
			//IL_050f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_051e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_053e: Expected O, but got Unknown
			ChatLinkEditorViewModel viewModel2 = viewModel;
			((FlowPanel)this)._002Ector();
			ChatLinkEditor chatLinkEditor = this;
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
			((Control)_itemIcon).add_MouseEntered((EventHandler<MouseEventArgs>)IconMouseEntered);
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)header);
			val3.set_TextColor(viewModel2.ItemNameColor);
			((Control)val3).set_Width(300);
			((Control)val3).set_Height(50);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			val3.set_Font(GameService.Content.get_DefaultFont18());
			val3.set_WrapText(true);
			_itemName = val3;
			Binder.Bind(viewModel2, (ChatLinkEditorViewModel vm) => vm.ItemName, _itemName);
			foreach (UpgradeEditorViewModel upgradeEditorViewModel in viewModel2.UpgradeEditorViewModels)
			{
				((Control)new UpgradeEditor(upgradeEditorViewModel)).set_Parent((Container)(object)this);
				upgradeEditorViewModel.PropertyChanged += delegate(object _, PropertyChangedEventArgs args)
				{
					if (args.PropertyName == "EffectiveUpgradeComponent")
					{
						((Control)chatLinkEditor._itemIcon).set_Tooltip((Tooltip)null);
					}
				};
			}
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_FlowDirection((ControlFlowDirection)0);
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			val4.set_ControlPadding(new Vector2(5f));
			FlowPanel quantityGroup = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)quantityGroup);
			val5.set_AutoSizeWidth(true);
			((Control)val5).set_Height(32);
			Label stackSizeLabel = val5;
			Binder.Bind(viewModel2, (ChatLinkEditorViewModel vm) => vm.StackSizeLabel, stackSizeLabel);
			NumberInput numberInput = new NumberInput();
			((Control)numberInput).set_Parent((Container)(object)quantityGroup);
			((Control)numberInput).set_Width(80);
			numberInput.Value = 1;
			numberInput.MinValue = 1;
			numberInput.MaxValue = ViewModel.MaxStackSize;
			_quantity = numberInput;
			ViewModel.PropertyChanged += delegate(object _, PropertyChangedEventArgs args)
			{
				if (args.PropertyName == "MaxStackSize")
				{
					chatLinkEditor._quantity.MaxValue = chatLinkEditor.ViewModel.MaxStackSize;
				}
			};
			Binder.Bind(viewModel2, (ChatLinkEditorViewModel vm) => vm.Quantity, _quantity);
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)quantityGroup);
			val6.set_Text("250");
			((Control)val6).set_Width(50);
			((Control)val6).set_Height(32);
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)MaxQuantityClicked);
			GlowButton val7 = new GlowButton();
			((Control)val7).set_Parent((Container)(object)quantityGroup);
			((Control)val7).set_Width(32);
			((Control)val7).set_Height(32);
			val7.set_Icon(AsyncTexture2D.FromAssetId(157324));
			val7.set_ActiveIcon(AsyncTexture2D.FromAssetId(157325));
			((Control)val7).set_BasicTooltipText(viewModel2.ResetTooltip);
			GlowButton resetQuantity = val7;
			ViewModel.PropertyChanged += delegate(object _, PropertyChangedEventArgs args)
			{
				if (args.PropertyName == "ResetTooltip")
				{
					((Control)resetQuantity).set_BasicTooltipText(chatLinkEditor.ViewModel.ResetTooltip);
				}
			};
			((Control)resetQuantity).add_Click((EventHandler<MouseEventArgs>)ResetQuantityClicked);
			TextBox val8 = new TextBox();
			((Control)val8).set_Parent((Container)(object)this);
			((Control)val8).set_Width(350);
			_chatLink = val8;
			Binder.Bind(ViewModel, (ChatLinkEditorViewModel vm) => vm.ChatLink, _chatLink, BindingMode.ToView);
			((Control)_chatLink).add_Click((EventHandler<MouseEventArgs>)ChatLinkClicked);
			((Control)_chatLink).set_Menu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)(() => new _003C_003Ez__ReadOnlyArray<ContextMenuStripItem>((ContextMenuStripItem[])(object)new ContextMenuStripItem[1] { viewModel2.CopyChatLinkCommand.ToMenuItem(() => viewModel2.CopyChatLinkLabel) }))));
			Label val9 = new Label();
			((Control)val9).set_Parent((Container)(object)this);
			((Control)val9).set_Width(350);
			val9.set_AutoSizeHeight(true);
			val9.set_WrapText(true);
			val9.set_TextColor(Color.get_Yellow());
			((Control)val9).set_Visible(ViewModel.ShowInfusionWarning);
			_infusionWarning = val9;
			Binder.Bind(viewModel2, (ChatLinkEditorViewModel vm) => vm.InfusionWarning, _infusionWarning);
			viewModel2.PropertyChanged += new PropertyChangedEventHandler(PropertyChanged);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Control)_infusionWarning).set_Visible(ViewModel.ShowInfusionWarning);
		}

		private void PropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "Quantity")
			{
				((Control)_itemIcon).set_Tooltip((Tooltip)null);
			}
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
			Soundboard.Click.Play();
			ViewModel.MaxQuantityCommand.Execute();
		}

		private void ResetQuantityClicked(object sender, MouseEventArgs e)
		{
			Soundboard.Click.Play();
			ViewModel.MinQuantityCommand.Execute();
		}

		private void IconMouseEntered(object sender, MouseEventArgs e)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_002c: Expected O, but got Unknown
			Image itemIcon = _itemIcon;
			if (((Control)itemIcon).get_Tooltip() == null)
			{
				Tooltip val = new Tooltip((ITooltipView)(object)new ItemTooltipView(ViewModel.CreateTooltipViewModel()));
				Tooltip val2 = val;
				((Control)itemIcon).set_Tooltip(val);
			}
		}

		private void ChatLinkClicked(object sender, MouseEventArgs e)
		{
			((TextInputBase)_chatLink).set_SelectionStart(0);
			((TextInputBase)_chatLink).set_SelectionEnd(((TextInputBase)_chatLink).get_Text().Length);
		}

		protected override void DisposeControl()
		{
			((FlowPanel)this).DisposeControl();
			ViewModel.Dispose();
		}
	}
}
