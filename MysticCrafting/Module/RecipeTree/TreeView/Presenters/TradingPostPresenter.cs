using System;
using System.Collections;
using System.Linq;
using Blish_HUD.Controls;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Settings;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Presenters
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
			TradingPostNode obj = new TradingPostNode(source.Item.TradingPostBuy.GetValueOrDefault(), Recipe.TradingPostBuy, TradingPostOptions.Buy, parent)
			{
				PanelHeight = 40,
				Name = "buy"
			};
			((Control)obj).set_Height(40);
			((Control)obj).set_Width(((Control)parent).get_Width() - 25);
			obj.PanelExtensionHeight = 0;
			TradingPostNode buyItem = obj;
			TradingPostNode tradingPostNode = new TradingPostNode(source.Item.TradingPostSell.GetValueOrDefault(), Recipe.TradingPostSell, TradingPostOptions.Sell, parent);
			((Control)tradingPostNode).set_Parent(parent);
			tradingPostNode.Name = "sell";
			tradingPostNode.PanelHeight = 40;
			((Control)tradingPostNode).set_Height(40);
			((Control)tradingPostNode).set_Width(((Control)parent).get_Width() - 25);
			tradingPostNode.PanelExtensionHeight = 0;
			TradingPostNode sellItem = tradingPostNode;
			IngredientNode parentNode = parent as IngredientNode;
			if (parentNode != null)
			{
				string choiceValue = _choiceRepository.GetChoice(GetFullPath(parentNode), ChoiceType.TradingPost)?.Value;
				if (choiceValue == null)
				{
					choiceValue = ((MysticCraftingModule.Settings.TradingPostPreference.get_Value() != 0) ? sellItem.PathName : buyItem.PathName);
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
			if (option != null && e.get_Checked())
			{
				OnOptionSelect(option);
				IngredientNode parentNode = ((Control)option).get_Parent() as IngredientNode;
				if (parentNode != null)
				{
					_choiceRepository.SaveChoice(GetFullPath(parentNode), option.PathName, ChoiceType.TradingPost);
				}
			}
		}

		private void OnOptionSelect(TradingPostNode option)
		{
			IngredientNode parent = ((Control)option).get_Parent() as IngredientNode;
			if (parent != null)
			{
				parent.TradingPostPrice = option.UnitPrice;
			}
			foreach (TradingPostNode child in from o in ((IEnumerable)((Control)option).get_Parent().get_Children()).OfType<TradingPostNode>()
				where o.Selected
				select o)
			{
				if (!((object)child).Equals((object)option))
				{
					child.Deselect();
				}
			}
		}
	}
}
