using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using MysticCrafting.Models.Vendor;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Recipe.TreeView.Nodes;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Recipe.TreeView.Presenters
{
	public class VendorPresenter
	{
		private readonly IChoiceRepository _choiceRepository;

		public event EventHandler<CheckChangedEvent> SelectChanged;

		public VendorPresenter(IChoiceRepository choiceRepository)
		{
			_choiceRepository = choiceRepository;
		}

		public void Build(Container parent, VendorSource source)
		{
			List<VendorNode> options = new List<VendorNode>();
			using (IEnumerator<VendorSellsItem> enumerator = source.VendorItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VendorNode option = new VendorNode(enumerator.Current, parent)
					{
						PanelHeight = 40,
						Height = 40,
						Width = parent.Width - 25,
						PanelExtensionHeight = 0
					};
					options.Add(option);
				}
			}
			IngredientNode parentNode = parent as IngredientNode;
			if (parentNode != null)
			{
				string choiceValue = _choiceRepository.GetChoice(GetFullPath(parentNode), ChoiceType.Vendor)?.Value;
				options.FirstOrDefault((VendorNode o) => o.PathName.Equals(choiceValue))?.Select();
			}
			if (!options.Any((VendorNode o) => o.Selected) && options.Any())
			{
				options.FirstOrDefault()?.Select();
			}
			VendorNode selectedOption = options.FirstOrDefault((VendorNode o) => o.Selected);
			if (selectedOption != null)
			{
				OnOptionSelect(selectedOption);
			}
			foreach (VendorNode item in options)
			{
				item.OnSelected += Option_OnSelected;
			}
		}

		private string GetFullPath(IngredientNode node, string separator = "/")
		{
			if (node != null)
			{
				return node.FullPath + separator + node.SelectedItemSource?.UniqueId;
			}
			return node.SelectedItemSource?.UniqueId;
		}

		private void Option_OnSelected(object sender, CheckChangedEvent e)
		{
			this.SelectChanged?.Invoke(this, e);
			VendorNode option = sender as VendorNode;
			if (option != null && option.Selected)
			{
				OnOptionSelect(option);
				IngredientNode node = option.Parent as IngredientNode;
				if (node != null)
				{
					_choiceRepository.SaveChoice(GetFullPath(node), option.PathName, ChoiceType.Vendor);
				}
			}
		}

		private void OnOptionSelect(VendorNode option)
		{
			ITradeableItemNode parent = option.Parent as ITradeableItemNode;
			if (parent != null)
			{
				parent.VendorUnitQuantity = option.VendorItem.ItemQuantity ?? 1;
				parent.VendorUnitPrice = option.VendorItem.MapToCurrencyQuantities().ToList();
			}
			foreach (VendorNode child in from o in option.Parent.Children.OfType<VendorNode>()
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
