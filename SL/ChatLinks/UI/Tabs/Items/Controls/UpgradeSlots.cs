using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using GuildWars2.Items;
using Microsoft.Xna.Framework;
using SL.Common;
using SL.Common.Controls.Items.Upgrades;

namespace SL.ChatLinks.UI.Tabs.Items.Controls
{
	public sealed class UpgradeSlots : FlowPanel
	{
		private readonly List<(UpgradeSlot Slot, UpgradeComponentsList Options)> _slots = new List<(UpgradeSlot, UpgradeComponentsList)>();

		public UpgradeSlotsViewModel ViewModel { get; }

		public UpgradeSlots(Item item, IReadOnlyDictionary<int, UpgradeComponent> upgrades)
			: this()
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			ViewModel = Objects.Create<UpgradeSlotsViewModel>();
			ViewModel.Item = item;
			ViewModel.UpgradeComponents = upgrades;
			ViewModel.Initialize();
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)this).set_ControlPadding(new Vector2(20f));
			foreach (UpgradeSlotModel slotModel in ViewModel.Slots())
			{
				UpgradeSlot upgradeSlot = new UpgradeSlot(slotModel);
				((Control)upgradeSlot).set_Parent((Container)(object)this);
				((Container)upgradeSlot).set_WidthSizingMode((SizingMode)2);
				((Container)upgradeSlot).set_HeightSizingMode((SizingMode)1);
				UpgradeSlot slot = upgradeSlot;
				IReadOnlyList<UpgradeComponent> options = (IReadOnlyList<UpgradeComponent>)((slotModel.Type switch
				{
					UpgradeSlotType.Default => ViewModel.UpgradeOptions, 
					UpgradeSlotType.Infusion => ViewModel.InfusionOptions, 
					UpgradeSlotType.Enrichment => ViewModel.EnrichmentOptions, 
					_ => Array.Empty<UpgradeComponent>(), 
				}) ?? Array.Empty<UpgradeComponent>());
				UpgradeComponentsList upgradeComponentsList = new UpgradeComponentsList(options);
				((Container)upgradeComponentsList).set_WidthSizingMode((SizingMode)2);
				((Container)upgradeComponentsList).set_HeightSizingMode((SizingMode)1);
				UpgradeComponentsList list = upgradeComponentsList;
				_slots.Add((slot, list));
				slot.ViewModel.Customized += delegate
				{
					if (((Control)list).get_Parent() == null)
					{
						ShowOptions(list);
					}
					else
					{
						((Control)list).set_Parent((Container)null);
					}
				};
				slot.ViewModel.Cleared += delegate
				{
					slotModel.SelectedUpgradeComponent = null;
					ViewModel.OnUpgradeChanged();
				};
				list.ViewModel.Selected += delegate(UpgradeComponent component)
				{
					slotModel.SelectedUpgradeComponent = ((slotModel.SelectedUpgradeComponent == component) ? null : component);
					slot.ViewModel.SelectedUpgradeComponent = slotModel.SelectedUpgradeComponent;
					ViewModel.OnUpgradeChanged();
				};
			}
		}

		private void ShowOptions(UpgradeComponentsList options)
		{
			foreach (var (slot, list) in _slots)
			{
				((Control)slot).set_Parent((Container)null);
				((Control)slot).set_Parent((Container)(object)this);
				((Control)list).set_Parent((Container)null);
				if (list == options)
				{
					((Control)list).set_Parent((Container)(object)this);
				}
			}
		}
	}
}
