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
			: this()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Expected O, but got Unknown
			//IL_00d7: Expected O, but got Unknown
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Expected O, but got Unknown
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Expected O, but got Unknown
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Expected O, but got Unknown
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Expected O, but got Unknown
			//IL_0412: Unknown result type (might be due to invalid IL or missing references)
			//IL_0417: Unknown result type (might be due to invalid IL or missing references)
			//IL_041e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0463: Expected O, but got Unknown
			ViewModel = viewModel;
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
			val2.set_Texture(viewModel.GetIcon());
			((Control)val2).set_Size(new Point(50));
			((Control)val2).set_Menu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)(() => new _003C_003Ez__ReadOnlyArray<ContextMenuStripItem>((ContextMenuStripItem[])(object)new ContextMenuStripItem[4]
			{
				ViewModel.CopyNameCommand.ToMenuItem(() => "Copy Name"),
				ViewModel.CopyChatLinkCommand.ToMenuItem(() => "Copy Chat Link"),
				ViewModel.OpenWikiCommand.ToMenuItem(() => "Open Wiki"),
				ViewModel.OpenApiCommand.ToMenuItem(() => "Open API")
			}))));
			_itemIcon = val2;
			((Control)_itemIcon).add_MouseEntered((EventHandler<MouseEventArgs>)IconMouseEntered);
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)header);
			val3.set_TextColor(viewModel.ItemNameColor);
			((Control)val3).set_Width(300);
			((Control)val3).set_Height(50);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			val3.set_Font(GameService.Content.get_DefaultFont18());
			val3.set_WrapText(true);
			_itemName = val3;
			Binder.Bind(viewModel, (ChatLinkEditorViewModel vm) => vm.ItemName, _itemName);
			foreach (UpgradeEditorViewModel upgradeEditorViewModel in viewModel.UpgradeEditorViewModels)
			{
				((Control)new UpgradeEditor(upgradeEditorViewModel)).set_Parent((Container)(object)this);
				upgradeEditorViewModel.PropertyChanged += delegate(object _, PropertyChangedEventArgs args)
				{
					if (args.PropertyName == "EffectiveUpgradeComponent")
					{
						((Control)_itemIcon).set_Tooltip((Tooltip)null);
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
			val5.set_Text("Stack Size:");
			val5.set_AutoSizeWidth(true);
			((Control)val5).set_Height(32);
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
					_quantity.MaxValue = ViewModel.MaxStackSize;
				}
			};
			Binder.Bind(viewModel, (ChatLinkEditorViewModel vm) => vm.Quantity, _quantity);
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
			((Control)val7).set_BasicTooltipText("Reset");
			((Control)val7).add_Click((EventHandler<MouseEventArgs>)ResetQuantityClicked);
			TextBox val8 = new TextBox();
			((Control)val8).set_Parent((Container)(object)this);
			((Control)val8).set_Width(350);
			_chatLink = val8;
			Binder.Bind(ViewModel, (ChatLinkEditorViewModel vm) => vm.ChatLink, _chatLink);
			((Control)_chatLink).add_Click((EventHandler<MouseEventArgs>)ChatLinkClicked);
			((Control)_chatLink).set_Menu(new ContextMenuStrip());
			((Control)_chatLink).get_Menu().AddMenuItem(viewModel.CopyChatLinkCommand.ToMenuItem(() => "Copy"));
			Label val9 = new Label();
			((Control)val9).set_Parent((Container)(object)this);
			((Control)val9).set_Width(350);
			val9.set_AutoSizeHeight(true);
			val9.set_WrapText(true);
			val9.set_TextColor(Color.get_Yellow());
			val9.set_Text("Due to technical restrictions, the game only\r\nshows the item's default infusion(s) instead of\r\nthe selected infusion(s).");
			((Control)val9).set_Visible(ViewModel.ShowInfusionWarning);
			_infusionWarning = val9;
			viewModel.PropertyChanged += new PropertyChangedEventHandler(PropertyChanged);
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
	}
}
