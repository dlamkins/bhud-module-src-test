using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using GuildWars2.Items;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SL.ChatLinks.UI.Tabs.Items.Controls;
using SL.Common;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public class ItemsTabView : View<ItemsTabPresenter>, IItemsTabView, IView
	{
		private Container? _root;

		private TextBox? _searchBox;

		private ItemsList? _searchResults;

		private ItemWidget? _selectedItem;

		private readonly ILogger<ItemsTabView> _logger;

		public ItemsTabView(ILogger<ItemsTabView> logger)
		{
			_logger = logger;
			((View<ItemsTabPresenter>)(object)this).AutoWire<ItemsTabPresenter>();
		}

		public void SetSearchLoading(bool loading)
		{
			_searchResults?.SetLoading(loading);
		}

		public void AddOption(Item item)
		{
			_searchResults?.AddOption(item);
		}

		public void SetOptions(IEnumerable<Item> items)
		{
			List<Item> list = items.ToList();
			_searchResults?.SetOptions(list);
		}

		public void ClearOptions()
		{
			_searchResults?.ClearOptions();
		}

		public void Select(Item item)
		{
			ItemWidget? selectedItem = _selectedItem;
			if (selectedItem != null)
			{
				((Control)selectedItem).Dispose();
			}
			ItemWidget itemWidget = new ItemWidget(item, ((Presenter<IItemsTabView, ItemsTabModel>)base.get_Presenter()).get_Model().Upgrades);
			((Control)itemWidget).set_Parent(_root);
			((Control)itemWidget).set_Left(((Control)_searchResults).get_Right());
			_selectedItem = itemWidget;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			_root = buildPanel;
			TextBox val = new TextBox();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(450);
			((TextInputBase)val).set_PlaceholderText("Enter item name or chat link...");
			_searchBox = val;
			ItemsList itemsList = new ItemsList(((Presenter<IItemsTabView, ItemsTabModel>)base.get_Presenter()).get_Model().Upgrades);
			((Control)itemsList).set_Parent(buildPanel);
			((Control)itemsList).set_Size(new Point(450, 500));
			((Control)itemsList).set_Top(((Control)_searchBox).get_Bottom());
			_searchResults = itemsList;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)SearchTextChanged);
			_searchBox!.add_EnterPressed((EventHandler<EventArgs>)SearchInput);
			((TextInputBase)_searchBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate(object sender, ValueEventArgs<bool> args)
			{
				if (args.get_Value())
				{
					((TextInputBase)_searchBox).set_SelectionStart(0);
					((TextInputBase)_searchBox).set_SelectionEnd(((TextInputBase)_searchBox).get_Length());
				}
				else
				{
					((TextInputBase)_searchBox).set_SelectionStart(((TextInputBase)_searchBox).get_SelectionEnd());
				}
			});
			_searchResults!.OptionClicked += delegate(object sender, Item item)
			{
				base.get_Presenter().ViewOptionSelected(item);
			};
		}

		protected override void OnPresenterAssigned(ItemsTabPresenter presenter)
		{
			ItemsTabPresenter presenter2 = presenter;
			MessageBus.Register("items_tab", async delegate(string message)
			{
				try
				{
					if (message == "refresh")
					{
						await presenter2.RefreshUpgrades();
					}
				}
				catch (Exception reason)
				{
					_logger.LogError(reason, "Failed to process message: {Message}", message);
				}
			});
		}

		protected override void Unload()
		{
			MessageBus.Unregister("items_tab");
			if (_searchBox != null)
			{
				((TextInputBase)_searchBox).remove_TextChanged((EventHandler<EventArgs>)SearchInput);
			}
			TextBox? searchBox = _searchBox;
			if (searchBox != null)
			{
				((Control)searchBox).Dispose();
			}
			ItemsList? searchResults = _searchResults;
			if (searchResults != null)
			{
				((Control)searchResults).Dispose();
			}
			base.Unload();
		}

		private async void SearchTextChanged(object sender, EventArgs e)
		{
			try
			{
				if (_searchBox != null && ((TextInputBase)_searchBox).get_Focused())
				{
					await base.get_Presenter().Search(((TextInputBase)_searchBox).get_Text());
				}
			}
			catch (Exception reason)
			{
				_logger.LogError(reason, "Failed to search for items");
				ScreenNotification.ShowNotification("Something went wrong", (NotificationType)6, (Texture2D)null, 4);
			}
		}

		private async void SearchInput(object sender, EventArgs e)
		{
			try
			{
				if (_searchBox != null)
				{
					await base.get_Presenter().Search(((TextInputBase)_searchBox).get_Text());
				}
			}
			catch (Exception reason)
			{
				_logger.LogError(reason, "Failed to search for items");
				ScreenNotification.ShowNotification("Something went wrong", (NotificationType)6, (Texture2D)null, 4);
			}
		}
	}
}
