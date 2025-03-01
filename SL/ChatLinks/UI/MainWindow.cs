using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using SL.ChatLinks.UI.Tabs.Achievements;
using SL.ChatLinks.UI.Tabs.Items;
using SL.Common;

namespace SL.ChatLinks.UI
{
	public sealed class MainWindow : TabbedWindow2
	{
		private readonly AsyncEmblem _emblem;

		private readonly Tab _itemsTab;

		private readonly Tab _achievementsTab;

		public MainWindowViewModel ViewModel { get; }

		public MainWindow(MainWindowViewModel viewModel)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected O, but got Unknown
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Expected O, but got Unknown
			MainWindowViewModel viewModel2 = viewModel;
			((TabbedWindow2)this)._002Ector(viewModel2?.BackgroundTexture, new Rectangle(0, 26, 953, 691), new Rectangle(60, 35, 890, 650));
			ThrowHelper.ThrowIfNull(viewModel2, "viewModel");
			ViewModel = viewModel2;
			_emblem = AsyncEmblem.Attach((WindowBase2)(object)this, viewModel2.EmblemTexture);
			((Control)this).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			((WindowBase2)this).set_Id(viewModel2.Id);
			((WindowBase2)this).set_Title(viewModel2.Title);
			((Control)this).set_Location(new Point(300, 300));
			((Control)this).set_Width(1000);
			((TabbedWindow2)this).add_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
			_itemsTab = new Tab(AsyncTexture2D.FromAssetId(156699), (Func<IView>)(() => (IView)(object)new ItemsTabView(viewModel2.CreateItemsTabViewModel())), viewModel2.ItemsTabName, (int?)1);
			_achievementsTab = new Tab(AsyncTexture2D.FromAssetId(156710), (Func<IView>)(() => (IView)(object)new AchievementsTabView(viewModel2.CreateAchievementsTabViewModel())), viewModel2.AchievementsTabName, (int?)1);
			((TabbedWindow2)this).get_Tabs().Add(_itemsTab);
			((TabbedWindow2)this).get_Tabs().Add(_achievementsTab);
			((Control)this).add_PropertyChanged((PropertyChangedEventHandler)ViewPropertyChanged);
			viewModel2.PropertyChanged += new PropertyChangedEventHandler(ModelPropertyChanged);
			viewModel2.Initialize();
		}

		private void ViewPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "Visible")
			{
				ViewModel.Visible = ((Control)this).get_Visible();
			}
		}

		private void ModelPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
			case "Title":
				((WindowBase2)this).set_Title(ViewModel.Title);
				break;
			case "ItemsTabName":
				_itemsTab.set_Name(ViewModel.ItemsTabName);
				((WindowBase2)this).set_Subtitle(((TabbedWindow2)this).get_SelectedTab().get_Name());
				break;
			case "AchievementsTabName":
				_achievementsTab.set_Name(ViewModel.AchievementsTabName);
				((WindowBase2)this).set_Subtitle(((TabbedWindow2)this).get_SelectedTab().get_Name());
				break;
			case "Visible":
				if (ViewModel.Visible)
				{
					((Control)this).Show();
				}
				else
				{
					((Control)this).Hide();
				}
				break;
			}
		}

		private void OnTabChanged(object sender, ValueChangedEventArgs<Tab> args)
		{
			((WindowBase2)this).set_Subtitle(args.get_NewValue().get_Name());
		}

		protected override void DisposeControl()
		{
			((TabbedWindow2)this).remove_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
			_emblem.Dispose();
			((WindowBase2)this).DisposeControl();
		}
	}
}
