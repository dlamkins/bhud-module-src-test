using System;
using System.Collections.Generic;
using System.ComponentModel;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using SL.Common;
using SL.Common.Controls;

namespace SL.ChatLinks.UI.Tabs.Items.Upgrades
{
	public sealed class UpgradeEditor : FlowPanel
	{
		private readonly UpgradeSlot _upgradeSlot;

		private StandardButton? _cancelButton;

		private UpgradeSelector? _options;

		public UpgradeEditorViewModel ViewModel { get; }

		public UpgradeEditor(UpgradeEditorViewModel viewModel)
			: this()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Control)this).set_Width(350);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)this).set_ControlPadding(new Vector2(10f));
			ViewModel = viewModel;
			viewModel.PropertyChanged += new PropertyChangedEventHandler(PropertyChanged);
			_upgradeSlot = CreateUpgradeSlot();
			((Control)_upgradeSlot).add_Click((EventHandler<MouseEventArgs>)UpgradeSlotClicked);
			((Control)_upgradeSlot).set_Menu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)(() => new _003C_003Ez__ReadOnlyArray<ContextMenuStripItem>((ContextMenuStripItem[])(object)new ContextMenuStripItem[6]
			{
				ViewModel.CustomizeCommand.ToMenuItem(() => "Customize"),
				ViewModel.RemoveCommand.ToMenuItem(() => ViewModel.RemoveItemText),
				ViewModel.CopyNameCommand.ToMenuItem(() => "Copy Name"),
				ViewModel.CopyChatLinkCommand.ToMenuItem(() => "Copy Chat Link"),
				ViewModel.OpenWikiCommand.ToMenuItem(() => "Open Wiki"),
				ViewModel.OpenApiCommand.ToMenuItem(() => "Open API")
			}))));
		}

		private void UpgradeSlotClicked(object sender, MouseEventArgs e)
		{
			Soundboard.Click.Play();
			ViewModel.CustomizeCommand.Execute();
		}

		public void ShowOptions()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Width(350);
			val.set_Text("Cancel");
			val.set_Icon(AsyncTexture2D.FromAssetId(155149));
			_cancelButton = val;
			UpgradeSelector upgradeSelector = new UpgradeSelector(ViewModel.CreateUpgradeComponentListViewModel());
			((Control)upgradeSelector).set_Parent((Container)(object)this);
			_options = upgradeSelector;
			((Control)_cancelButton).add_Click((EventHandler<MouseEventArgs>)CancelClicked);
		}

		public void HideOptions()
		{
			StandardButton? cancelButton = _cancelButton;
			if (cancelButton != null)
			{
				((Control)cancelButton).Dispose();
			}
			UpgradeSelector? options = _options;
			if (options != null)
			{
				((Control)options).Dispose();
			}
			_cancelButton = null;
			_options = null;
		}

		private void CancelClicked(object sender, MouseEventArgs e)
		{
			ViewModel.HideCommand.Execute();
		}

		private UpgradeSlot CreateUpgradeSlot()
		{
			UpgradeSlot upgradeSlot = new UpgradeSlot(ViewModel.UpgradeSlotViewModel);
			((Control)upgradeSlot).set_Parent((Container)(object)this);
			((Container)upgradeSlot).set_WidthSizingMode((SizingMode)2);
			return upgradeSlot;
		}

		private void PropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "Customizing")
			{
				if (ViewModel.Customizing)
				{
					ShowOptions();
				}
				else
				{
					HideOptions();
				}
			}
		}
	}
}
