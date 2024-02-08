using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Models;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Recipe.TreeView.Tooltips
{
	public class RecipeSourceTooltipView : View, ITooltipView, IView
	{
		private List<Control> _controls = new List<Control>();

		protected bool Initialized;

		private RecipeSource RecipeSource { get; set; }

		public Container BuildPanel { get; set; }

		public RecipeSourceTooltipView(RecipeSource recipeSource)
		{
			RecipeSource = recipeSource;
		}

		protected override void Build(Container buildPanel)
		{
			BuildPanel = buildPanel;
			if (!Initialized)
			{
				Initialize();
			}
		}

		public virtual void InitializeMysticForge()
		{
			AsyncTexture2D icon = ServiceContainer.TextureRepository.Textures.MysticForgeIcon;
			_controls.Add(new Image(icon)
			{
				Parent = BuildPanel,
				Size = new Point(35, 35),
				Location = new Point(0, 0)
			});
			_controls.Add(new Label
			{
				Parent = BuildPanel,
				Text = MysticCrafting.Module.Strings.Recipe.MysticForge,
				Font = GameService.Content.DefaultFont16,
				Location = new Point(35, 7),
				StrokeText = true,
				AutoSizeWidth = true
			});
		}

		public virtual void InitializeCrafting()
		{
			int yPos = 0;
			int xPos = 0;
			foreach (string item in RecipeSource.Recipe.Disciplines ?? new List<string>())
			{
				AsyncTexture2D icon = IconHelper.GetIconColor(item);
				_controls.Add(new Image(icon)
				{
					Parent = BuildPanel,
					Size = new Point(30, 30),
					Location = new Point(xPos, yPos)
				});
				xPos += 25;
			}
			if (RecipeSource.Recipe.DisciplineCount == 1)
			{
				string disciplineLabel = LocalizationHelper.TranslateProfession(RecipeSource.Recipe.Disciplines.FirstOrDefault());
				_controls.Add(new Label
				{
					Parent = BuildPanel,
					Text = disciplineLabel,
					Font = GameService.Content.DefaultFont16,
					Location = new Point(xPos + 5, yPos + 5),
					StrokeText = true,
					AutoSizeWidth = true
				});
			}
			if (RecipeSource.Recipe.DisciplineCount > 0)
			{
				yPos += 30;
			}
			_controls.Add(new Label
			{
				Parent = BuildPanel,
				Text = MysticCrafting.Module.Strings.Recipe.RecipeSource + ": " + LocalizationHelper.TranslateRecipeSource(RecipeSource.DisplayName),
				Font = GameService.Content.DefaultFont16,
				Location = new Point(5, yPos),
				StrokeText = true,
				AutoSizeWidth = true
			});
			yPos += 25;
			if (RecipeSource.Recipe.RequiredRating != 0)
			{
				Label ratingLabel = new Label
				{
					Parent = BuildPanel,
					Text = $"{MysticCrafting.Module.Strings.Recipe.RecipeRequiredLevel}: {RecipeSource.Recipe.RequiredRating}",
					Font = GameService.Content.DefaultFont16,
					Location = new Point(5, yPos),
					StrokeText = true,
					AutoSizeWidth = true
				};
				_controls.Add(ratingLabel);
			}
		}

		public virtual void Initialize()
		{
			if (RecipeSource.Recipe.MysticForgeId != 0)
			{
				InitializeMysticForge();
			}
			else
			{
				InitializeCrafting();
			}
			int yPosition = (_controls.Any() ? (_controls.Max((Control c) => c.Bottom) + 15) : 15);
			_controls.Add(new Label
			{
				Parent = BuildPanel,
				Text = MysticCrafting.Module.Strings.Recipe.Ingredients,
				Font = GameService.Content.DefaultFont14,
				Location = new Point(5, yPosition),
				TextColor = Color.White,
				StrokeText = true,
				AutoSizeWidth = true
			});
			yPosition += 25;
			foreach (MysticIngredient ingredient in RecipeSource.Recipe.Ingredients)
			{
				MysticItem item = ServiceContainer.ItemRepository.GetItem(ingredient.GameId);
				if (item != null)
				{
					_controls.Add(new Image(ServiceContainer.TextureRepository.GetTexture(item.Icon))
					{
						Parent = BuildPanel,
						Size = new Point(20, 20),
						Location = new Point(5, yPosition)
					});
				}
				Label countLabel = new Label
				{
					Parent = BuildPanel,
					Text = ingredient.Quantity.ToString(),
					Font = GameService.Content.DefaultFont16,
					Location = new Point(30, yPosition),
					StrokeText = true,
					AutoSizeWidth = true
				};
				_controls.Add(countLabel);
				Label itemNameLabel = new Label
				{
					Parent = BuildPanel,
					Text = ((item != null) ? item.Name : ingredient.Name),
					Font = GameService.Content.DefaultFont16,
					Location = new Point(countLabel.Right + 5, yPosition),
					StrokeText = true,
					AutoSizeWidth = true
				};
				if (item != null)
				{
					itemNameLabel.TextColor = ColorHelper.FromRarity(item.Rarity);
				}
				_controls.Add(itemNameLabel);
				yPosition += 25;
			}
		}
	}
}
