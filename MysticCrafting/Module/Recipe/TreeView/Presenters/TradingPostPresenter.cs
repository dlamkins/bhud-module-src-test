using System;
using System.Linq;
using Blish_HUD.Controls;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Recipe.TreeView.Nodes;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Settings;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Recipe.TreeView.Presenters
{
	public class TradingPostPresenter
	{
		private readonly IChoiceRepository _choiceRepository;

		private TradingPostSource Source;

		public event EventHandler<CheckChangedEvent> SelectChanged;

		public TradingPostPresenter(IChoiceRepository choiceRepository)
		{
			_choiceRepository = choiceRepository;
		}

		public void Build(Container parent, TradingPostSource source)
		{
			Source = source;
			TradingPostNode buyItem = new TradingPostNode(source.BuyPrice.UnitPrice, MysticCrafting.Module.Strings.Recipe.TradingPostBuy, TradingPostOptions.Buy, parent)
			{
				PanelHeight = 40,
				Name = "buy",
				Height = 40,
				Width = parent.Width - 25,
				PanelExtensionHeight = 0
			};
			TradingPostNode sellItem = new TradingPostNode(source.SellPrice.UnitPrice, MysticCrafting.Module.Strings.Recipe.TradingPostSell, TradingPostOptions.Sell, parent)
			{
				Parent = parent,
				Name = "sell",
				PanelHeight = 40,
				Height = 40,
				Width = parent.Width - 25,
				PanelExtensionHeight = 0
			};
			IngredientNode parentNode = parent as IngredientNode;
			if (parentNode != null)
			{
				string choiceValue = _choiceRepository.GetChoice(GetFullPath(parentNode), ChoiceType.TradingPost)?.Value;
				if (choiceValue == null)
				{
					if (buyItem.UnitPrice != 0 && MysticCraftingModule.TradingPostPreference.Value == TradingPostOptions.Buy)
					{
						choiceValue = buyItem.PathName;
					}
					else if (sellItem.UnitPrice != 0)
					{
						choiceValue = sellItem.PathName;
					}
				}
				if (choiceValue.Equals(buyItem.PathName))
				{
					buyItem.ToggleSelect();
					OnOptionSelect(buyItem);
				}
				else if (choiceValue.Equals(sellItem.PathName))
				{
					sellItem.ToggleSelect();
					OnOptionSelect(sellItem);
				}
			}
			buyItem.OnSelected += Option_OnSelected;
			sellItem.OnSelected += Option_OnSelected;
		}

		private string GetFullPath(IngredientNode parentNode, string separator = "/")
		{
			return parentNode.FullPath + separator + parentNode.SelectedItemSource?.UniqueId;
		}

		private void Option_OnSelected(object sender, CheckChangedEvent e)
		{
			this.SelectChanged?.Invoke(this, e);
			TradingPostNode option = sender as TradingPostNode;
			if (option != null && e.Checked)
			{
				OnOptionSelect(option);
				IngredientNode parentNode = option.Parent as IngredientNode;
				if (parentNode != null)
				{
					_choiceRepository.SaveChoice(GetFullPath(parentNode), option.PathName, ChoiceType.TradingPost);
				}
			}
		}

		private void OnOptionSelect(TradingPostNode option)
		{
			IngredientNode parent = option.Parent as IngredientNode;
			if (parent != null)
			{
				parent.TradingPostPrice = option.UnitPrice;
			}
			foreach (TradingPostNode child in from o in option.Parent.Children.OfType<TradingPostNode>()
				where o.Selected
				select o)
			{
				if (!child.Equals(option))
				{
					child.Deselect();
				}
			}
		}
	}
}
