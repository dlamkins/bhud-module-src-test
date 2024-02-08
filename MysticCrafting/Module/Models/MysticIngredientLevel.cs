using MysticCrafting.Models;
using MysticCrafting.Module.Recipe.TreeView.Nodes;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.API;

namespace MysticCrafting.Module.Models
{
	public class MysticIngredientLevel : MysticIngredient, IIngredient
	{
		private IPlayerItemService _playerItemService = ServiceContainer.PlayerItemService;

		public IIngredient Parent;

		private MysticIngredientLevel _child;

		private string _fullPath;

		public int RequiredQuantity => CalculateQuantity();

		public int RequiredTotalQuantity
		{
			get
			{
				if (Parent == null)
				{
					return Quantity.GetValueOrDefault();
				}
				return Quantity.GetValueOrDefault() * Parent.RequiredQuantity;
			}
		}

		public MysticIngredientLevel Child
		{
			get
			{
				return _child;
			}
			set
			{
				_child = value;
				_child.Parent = this;
			}
		}

		public string UniqueTitle => $"{base.Index}-{base.Item.Id}";

		public virtual string FullPath => _fullPath ?? (_fullPath = GetFullPath());

		public MysticIngredientLevel(MysticIngredient ingredient)
		{
			base.Name = ingredient.Name;
			Quantity = ingredient.Quantity;
			base.Item = ingredient.Item;
			base.GameId = ingredient.GameId;
			base.Index = ingredient.Index;
		}

		private int CalculateQuantity()
		{
			int quantity = (Parent?.RequiredQuantity ?? 0) * (Quantity ?? 1);
			if (Child == null)
			{
				return quantity;
			}
			int playerItemCount = _playerItemService.GetItemCount(base.GameId);
			int amountRequired = quantity - playerItemCount;
			if (amountRequired <= 0)
			{
				return 0;
			}
			return amountRequired;
		}

		public string GetFullPath(string separator = "/")
		{
			if (Parent != null)
			{
				return Parent.FullPath + separator + UniqueTitle;
			}
			return (base.GameId + base.Index).ToString();
		}
	}
}
