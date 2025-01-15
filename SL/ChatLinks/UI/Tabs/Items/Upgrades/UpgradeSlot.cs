using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using GuildWars2.Items;
using Microsoft.Xna.Framework;
using SL.ChatLinks.UI.Tabs.Items.Tooltips;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items.Upgrades
{
	public sealed class UpgradeSlot : Container
	{
		private FormattedLabel _label;

		public UpgradeSlotViewModel ViewModel { get; }

		public UpgradeSlot(UpgradeSlotViewModel viewModel)
			: this()
		{
			ViewModel = viewModel;
			ViewModel.PropertyChanged += new PropertyChangedEventHandler(PropertyChanged);
			_label = FormatSlot();
			((Control)_label).set_Parent((Container)(object)this);
		}

		private void PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
			case "SelectedUpgradeComponent":
			case "DefaultUpgradeComponent":
			case "Type":
				((Control)_label).Dispose();
				_label = FormatSlot();
				((Control)_label).set_Parent((Container)(object)this);
				break;
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			//IL_0052: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Expected O, but got Unknown
			//IL_0097: Expected O, but got Unknown
			if (!((Control)this).get_MouseOver())
			{
				return;
			}
			if ((object)ViewModel.SelectedUpgradeComponent != null)
			{
				FormattedLabel label = _label;
				if (((Control)label).get_Tooltip() == null)
				{
					FormattedLabel obj = label;
					Tooltip val = new Tooltip((ITooltipView)(object)new ItemTooltipView(ViewModel.CreateTooltipViewModel(ViewModel.SelectedUpgradeComponent)));
					Tooltip val2 = val;
					((Control)obj).set_Tooltip(val);
				}
			}
			else if ((object)ViewModel.DefaultUpgradeComponent != null)
			{
				FormattedLabel label = _label;
				if (((Control)label).get_Tooltip() == null)
				{
					FormattedLabel obj2 = label;
					Tooltip val3 = new Tooltip((ITooltipView)(object)new ItemTooltipView(ViewModel.CreateTooltipViewModel(ViewModel.DefaultUpgradeComponent)));
					Tooltip val2 = val3;
					((Control)obj2).set_Tooltip(val3);
				}
			}
			else
			{
				FormattedLabel label = _label;
				if (((Control)label).get_BasicTooltipText() == null)
				{
					string text;
					((Control)label).set_BasicTooltipText(text = "Click to customize\r\nRight-click for options");
				}
			}
		}

		private FormattedLabel FormatSlot()
		{
			UpgradeSlotViewModel viewModel = ViewModel;
			if (viewModel != null)
			{
				if ((object)viewModel.SelectedUpgradeComponent != null)
				{
					return UsedSlot(ViewModel.SelectedUpgradeComponent);
				}
				if ((object)viewModel.DefaultUpgradeComponent != null)
				{
					return UsedSlot(ViewModel.DefaultUpgradeComponent);
				}
			}
			return UnusedSlot();
		}

		private FormattedLabel UsedSlot(UpgradeComponent upgradeComponent)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			UpgradeComponent upgradeComponent2 = upgradeComponent;
			return new FormattedLabelBuilder().AutoSizeWidth().AutoSizeHeight().CreatePart(" " + upgradeComponent2.Name, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				part.SetPrefixImage(ViewModel.GetIcon(upgradeComponent2));
				part.SetPrefixImageSize(new Point(16));
				part.SetHoverColor(Color.get_BurlyWood());
				part.SetFontSize((FontSize)16);
			})
				.Build();
		}

		private FormattedLabel UnusedSlot()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			FormattedLabelBuilder builder = new FormattedLabelBuilder().AutoSizeWidth().AutoSizeHeight();
			switch (ViewModel.Type)
			{
			case UpgradeSlotType.Default:
				builder.CreatePart(" Unused Upgrade Slot", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
				{
					//IL_0019: Unknown result type (might be due to invalid IL or missing references)
					part.SetPrefixImage(AsyncTexture2D.op_Implicit(Resources.Texture("unused_upgrade_slot.png")));
					part.SetPrefixImageSize(new Point(16));
					part.SetFontSize((FontSize)16);
				});
				break;
			case UpgradeSlotType.Infusion:
				builder.CreatePart(" Unused Infusion Slot", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
				{
					//IL_0019: Unknown result type (might be due to invalid IL or missing references)
					part.SetPrefixImage(AsyncTexture2D.op_Implicit(Resources.Texture("unused_infusion_slot.png")));
					part.SetPrefixImageSize(new Point(16));
					part.SetFontSize((FontSize)16);
				});
				break;
			case UpgradeSlotType.Enrichment:
				builder.CreatePart(" Unused Enrichment Slot", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder part)
				{
					//IL_0019: Unknown result type (might be due to invalid IL or missing references)
					part.SetPrefixImage(AsyncTexture2D.op_Implicit(Resources.Texture("unused_enrichment_slot.png")));
					part.SetPrefixImageSize(new Point(16));
					part.SetFontSize((FontSize)16);
				});
				break;
			}
			return builder.Build();
		}
	}
}
