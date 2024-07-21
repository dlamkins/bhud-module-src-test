using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Vendor;
using Blish_HUD.Controls;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.RecipeTree.TreeView.Presenters
{
	public class VendorPresenter
	{
		private readonly IChoiceRepository _choiceRepository;

		public event EventHandler<CheckChangedEvent> SelectChanged;

		public VendorPresenter(IChoiceRepository choiceRepository)
		{
			_choiceRepository = choiceRepository;
		}

		public List<VendorNode> Build(Container parent, VendorSource source)
		{
			List<VendorNode> options = new List<VendorNode>();
			using (IEnumerator<VendorSellsItemGroup> enumerator = source.VendorGroups.Where((VendorSellsItemGroup g) => g.Vendors.Any()).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VendorNode obj = new VendorNode(enumerator.Current, parent)
					{
						PanelHeight = 40
					};
					((Control)obj).set_Width(((Control)parent).get_Width() - 25);
					obj.PanelExtensionHeight = 0;
					VendorNode option = obj;
					options.Add(option);
				}
			}
			IngredientNode parentNode = parent as IngredientNode;
			if (parentNode != null)
			{
				string choiceValue = _choiceRepository.GetChoice(GetFullPath(parentNode), ChoiceType.Vendor)?.Value;
				options.FirstOrDefault((VendorNode o) => o.AllVendorPathNames.Any((string p) => p.Equals(choiceValue)))?.Select();
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
			if (options.Count == 1)
			{
				selectedOption?.Toggle();
			}
			return options;
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
				IngredientNode node = ((Control)option).get_Parent() as IngredientNode;
				if (node != null)
				{
					_choiceRepository.SaveChoice(GetFullPath(node), option.PathName, ChoiceType.Vendor);
				}
			}
		}

		private void OnOptionSelect(VendorNode option)
		{
			foreach (VendorNode child in from o in ((IEnumerable)((Control)option).get_Parent().get_Children()).OfType<VendorNode>()
				where o.Selected
				select o)
			{
				if (!((object)child).Equals((object)option))
				{
					child.Deselect();
				}
			}
			option.CalculateTotalPrices();
		}
	}
}
