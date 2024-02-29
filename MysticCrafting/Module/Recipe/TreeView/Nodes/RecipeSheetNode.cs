using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Models;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Recipe.TreeView.Extensions;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.API;

namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	public class RecipeSheetNode : IngredientNode
	{
		private bool _recipeUnlocked;

		private Image _recipeLockedIcon;

		private readonly IPlayerUnlocksService _playerUnlocksService = ServiceContainer.PlayerUnlocksService;

		public MysticRecipe Recipe { get; private set; }

		public bool RecipeUnlocked
		{
			get
			{
				return _recipeUnlocked;
			}
			set
			{
				SetProperty(ref _recipeUnlocked, value, invalidateLayout: false, "RecipeUnlocked");
				if (_recipeLockedIcon != null)
				{
					_recipeLockedIcon.Visible = !_recipeUnlocked;
				}
				UpdateIconStyle(_recipeUnlocked);
			}
		}

		public override int TotalRequiredQuantity { get; set; } = 1;


		public override int RequiredQuantity
		{
			get
			{
				if (base.IsSharedItem && !base.TreeView.IngredientNodes.GetByItemId(base.Item.GameId).ToList().IsFirstNode(this))
				{
					return 0;
				}
				if (RecipeUnlocked)
				{
					return 0;
				}
				return 1;
			}
		}

		public RecipeSheetNode(MysticItem item, int? quantity = null, int? index = null)
			: base(item, quantity, index)
		{
		}

		public RecipeSheetNode(MysticRecipe recipe, MysticIngredient ingredient, MysticItem item, Container parent, bool loadingChildren = false)
			: base(ingredient, item, parent, loadingChildren)
		{
			Recipe = recipe;
			_recipeLockedIcon = new Image(ServiceContainer.TextureRepository.Textures.Lock)
			{
				Parent = this,
				ZIndex = 1,
				Size = new Point(base.IconSize, base.IconSize),
				Location = new Point(base.IsTopItem ? 12 : 27, 5)
			};
			UpdateCollectionStatus();
		}

		private void UpdateIconStyle(bool unlocked)
		{
			if (unlocked)
			{
				base.Icon.Opacity = 1f;
				base.Icon.Tint = Color.White;
			}
			else
			{
				base.Icon.Opacity = 0.4f;
				base.Icon.Tint = Color.Gray;
			}
		}

		public override void Build(Container parent)
		{
			base.Build(parent);
			if ((Recipe.RecipeSheetIds?.FirstOrDefault()).HasValue)
			{
				_recipeLockedIcon = new Image(ServiceContainer.TextureRepository.Textures.Lock)
				{
					Parent = this,
					Size = new Point(base.IconSize, base.IconSize),
					Location = new Point(base.IsTopItem ? 12 : 29, 5)
				};
				if (base.ItemCountTooltip != null)
				{
					_recipeLockedIcon.Tooltip = base.ItemCountTooltip;
				}
				RecipeUnlocked = _playerUnlocksService.RecipeUnlocked(Recipe.GameId ?? 9999);
			}
		}

		public override IList<CraftingDisciplineRequirement> GetCraftingRequirements()
		{
			return Recipe.Disciplines.Select((string d) => new CraftingDisciplineRequirement
			{
				DisciplineName = d,
				RequiredLevel = Recipe.RequiredRating.GetValueOrDefault()
			}).ToList();
		}

		public override void BuildItemCountControls()
		{
		}

		public override void UpdateItemCountControls()
		{
		}

		public override void BuildItemCountTooltip()
		{
			base.BuildItemCountTooltip();
			if (_recipeLockedIcon != null)
			{
				_recipeLockedIcon.Tooltip = base.ItemCountTooltip;
			}
		}

		protected override void DisposeControl()
		{
			_recipeLockedIcon?.Dispose();
			base.DisposeControl();
		}
	}
}
