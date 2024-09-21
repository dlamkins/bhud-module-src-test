using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Crafting;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.API;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
{
	public class RecipeSheetNode : ItemIngredientNode
	{
		private bool _recipeUnlocked;

		private Image _recipeLockedIcon;

		private readonly IPlayerUnlocksService _playerUnlocksService = ServiceContainer.PlayerUnlocksService;

		public Recipe Recipe { get; private set; }

		public bool RecipeUnlocked
		{
			get
			{
				return _recipeUnlocked;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _recipeUnlocked, value, false, "RecipeUnlocked");
				if (_recipeLockedIcon != null)
				{
					((Control)_recipeLockedIcon).set_Visible(!_recipeUnlocked);
				}
				UpdateIconStyle(_recipeUnlocked);
			}
		}

		public override int TotalUnitCount { get; set; } = 1;


		public RecipeSheetNode(Recipe recipe, Item item, Container parent, bool loadingChildren = false)
			: base(item, 1, parent, null, loadingChildren)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			Recipe = recipe;
			Image val = new Image(ServiceContainer.TextureRepository.Textures.Lock);
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(new Point(base.IconSize, base.IconSize));
			((Control)val).set_Location(new Point(base.IsTopIngredient ? 14 : 33, 5));
			((Control)val).set_Visible(false);
			_recipeLockedIcon = val;
			UpdateCollectionStatus();
		}

		private void UpdateIconStyle(bool unlocked)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			if (base.Icon != null)
			{
				if (unlocked)
				{
					((Control)base.Icon).set_Opacity(1f);
					base.Icon.set_Tint(Color.get_White());
				}
				else
				{
					((Control)base.Icon).set_Opacity(0.4f);
					base.Icon.set_Tint(Color.get_Gray());
				}
			}
		}

		public override void Build(Container parent)
		{
			base.Build(parent);
			if (Recipe.RecipeSheets?.FirstOrDefault() != null)
			{
				if (_recipeLockedIcon != null)
				{
					((Control)_recipeLockedIcon).set_Visible(true);
				}
				if (base.PlayerCountTooltip != null)
				{
					((Control)_recipeLockedIcon).set_Tooltip(base.PlayerCountTooltip);
				}
				RecipeUnlocked = _playerUnlocksService.RecipeUnlocked(Recipe.Id);
			}
		}

		public override IList<CraftingDisciplineRequirement> GetCraftingRequirements()
		{
			return Recipe.Disciplines.Select((Discipline d) => new CraftingDisciplineRequirement
			{
				DisciplineName = LocalizationHelper.TranslateDiscipline(d),
				RequiredLevel = Recipe.RequiredRating
			}).ToList();
		}

		public override void BuildUnitCountLabels()
		{
		}

		public override void UpdateItemCountControls()
		{
		}

		protected override void DisposeControl()
		{
			Image recipeLockedIcon = _recipeLockedIcon;
			if (recipeLockedIcon != null)
			{
				((Control)recipeLockedIcon).Dispose();
			}
			base.DisposeControl();
		}
	}
}
