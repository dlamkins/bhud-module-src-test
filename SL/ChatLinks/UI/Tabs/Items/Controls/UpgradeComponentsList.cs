using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using GuildWars2.Items;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items.Controls
{
	public sealed class UpgradeComponentsList : FlowPanel
	{
		private readonly StandardButton _cancelButton;

		public UpgradeComponentsListViewModel ViewModel { get; }

		public UpgradeComponentsList(IEnumerable<UpgradeComponent> upgrades)
			: this()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			ViewModel = Objects.Create<UpgradeComponentsListViewModel>();
			ViewModel.Options = upgrades.ToList().AsReadOnly();
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Container)this).set_HeightSizingMode((SizingMode)2);
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Cancel");
			_cancelButton = val;
			((Control)_cancelButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)this).set_Parent((Container)null);
			});
			ViewModel.Selected += delegate
			{
				((Control)this).set_Parent((Container)null);
			};
			Initialize();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			((Control)_cancelButton).set_Width(e.get_CurrentSize().X);
			Container parent = ((Control)this).get_Parent();
			if (parent != null)
			{
				((Control)parent).Invalidate();
			}
		}

		private void Initialize()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			foreach (IGrouping<string, UpgradeComponent> group in ViewModel.GetOptions())
			{
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)this);
				((Container)val).set_WidthSizingMode((SizingMode)2);
				((Container)val).set_HeightSizingMode((SizingMode)1);
				val.set_Title(group.Key);
				val.set_CanCollapse(true);
				val.set_Collapsed(true);
				Panel groupPanel = val;
				ItemsList itemsList = new ItemsList(new Dictionary<int, UpgradeComponent>(0));
				((Control)itemsList).set_Parent((Container)(object)groupPanel);
				((Container)itemsList).set_WidthSizingMode((SizingMode)2);
				((Container)itemsList).set_HeightSizingMode((SizingMode)1);
				((Panel)itemsList).set_CanScroll(false);
				ItemsList list = itemsList;
				list.SetOptions(group);
				list.OptionClicked += new EventHandler<Item>(OptionClicked);
				((Control)groupPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
				{
					if (((Control)list).get_Height() >= 300)
					{
						((Container)list).set_HeightSizingMode((SizingMode)0);
						((Control)list).set_Height(300);
						((Panel)list).set_CanScroll(true);
					}
					else
					{
						((Container)list).set_HeightSizingMode((SizingMode)1);
						((Panel)list).set_CanScroll(false);
					}
				});
			}
		}

		private void OptionClicked(object sender, Item e)
		{
			ViewModel.OnSelected((UpgradeComponent)e);
		}
	}
}
