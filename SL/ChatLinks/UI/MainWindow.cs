using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using SL.ChatLinks.UI.Tabs.Items;

namespace SL.ChatLinks.UI
{
	public sealed class MainWindow : TabbedWindow2
	{
		private readonly AsyncEmblem _emblem;

		private readonly Tab _itemsTab;

		public MainWindowViewModel ViewModel { get; }

		public MainWindow(MainWindowViewModel viewModel)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Expected O, but got Unknown
			MainWindowViewModel viewModel2 = viewModel;
			((TabbedWindow2)this)._002Ector(viewModel2.BackgroundTexture, new Rectangle(0, 26, 953, 691), new Rectangle(70, 35, 880, 650));
			ViewModel = viewModel2;
			_emblem = AsyncEmblem.Attach((WindowBase2)(object)this, viewModel2.EmblemTexture);
			((Control)this).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			((WindowBase2)this).set_Id(viewModel2.Id);
			((WindowBase2)this).set_Title(viewModel2.Title);
			((Control)this).set_Location(new Point(300, 300));
			((TabbedWindow2)this).add_TabChanged((EventHandler<ValueChangedEventArgs<Tab>>)OnTabChanged);
			_itemsTab = new Tab(AsyncTexture2D.FromAssetId(156699), (Func<IView>)(() => (IView)(object)new ItemsTabView(viewModel2.CreateItemsTabViewModel())), viewModel2.ItemsTabName, (int?)1);
			((TabbedWindow2)this).get_Tabs().Add(_itemsTab);
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
				((WindowBase2)this).set_Subtitle(ViewModel.ItemsTabName);
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
