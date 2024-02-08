using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Commerce;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Recipe.TreeView.Tooltips
{
	public class CurrencyTooltipView : View, ITooltipView, IView
	{
		private MysticCurrencyQuantity Quantity { get; set; }

		private IList<Control> Controls { get; set; } = new List<Control>();


		public Point IconSize { get; set; } = new Point(30, 30);


		public CurrencyTooltipView(MysticCurrencyQuantity quantity)
		{
			Quantity = quantity;
			if (Quantity.Currency?.Icon != null)
			{
				AsyncTexture2D texture = ServiceContainer.TextureRepository.GetTexture(Quantity.Currency.Icon);
				if (texture != null)
				{
					Controls.Add(new Image(texture)
					{
						Size = IconSize,
						Location = new Point(0, 0)
					});
				}
			}
			Color nameColor = Color.White;
			if (quantity.Currency.IsItem)
			{
				MysticItem currencyItem = ServiceContainer.ItemRepository.GetItem(quantity.Currency.GameId);
				if (currencyItem != null)
				{
					nameColor = ColorHelper.FromRarity(currencyItem.Rarity);
				}
			}
			Controls.Add(new Label
			{
				Text = Quantity.Currency.LocalizedName(),
				Font = GameService.Content.DefaultFont18,
				Location = new Point(40, 5),
				TextColor = nameColor,
				StrokeText = true,
				AutoSizeWidth = true
			});
			Label descriptionLabel = new Label
			{
				Text = Quantity.Currency.LocalizedDescription(),
				Font = GameService.Content.DefaultFont14,
				Location = new Point(0, 35),
				TextColor = Color.White,
				Width = 400,
				AutoSizeHeight = true,
				WrapText = true,
				StrokeText = true
			};
			Controls.Add(descriptionLabel);
			if (Quantity.Currency.Name == "Coin")
			{
				return;
			}
			MysticCurrencyQuantity walletQuantity = ServiceContainer.WalletService.GetQuantity(Quantity.Currency.GameId);
			if (walletQuantity != null)
			{
				int yPosition = 40 + descriptionLabel.Height;
				Label textLabel = new Label
				{
					Text = walletQuantity.Count.ToString("N0"),
					Font = GameService.Content.DefaultFont18,
					Location = new Point(0, yPosition),
					TextColor = ColorHelper.FromItemCount(walletQuantity.Count, Quantity.Count),
					StrokeText = true,
					AutoSizeWidth = true
				};
				Label totalLabel = new Label
				{
					Text = $"/ {Quantity.Count:N0}",
					Font = GameService.Content.DefaultFont18,
					Location = new Point(textLabel.Width, yPosition),
					TextColor = Color.White,
					StrokeText = true,
					AutoSizeWidth = true
				};
				yPosition += textLabel.Height + 5;
				Controls.Add(textLabel);
				Controls.Add(totalLabel);
				int moreRequired = Quantity.Count - walletQuantity.Count;
				if (moreRequired > 0)
				{
					Controls.Add(new Label
					{
						Text = string.Format(MysticCrafting.Module.Strings.Recipe.MoreRequired, moreRequired),
						Font = GameService.Content.DefaultFont16,
						Location = new Point(0, yPosition),
						TextColor = Color.LightGray,
						ShowShadow = true,
						AutoSizeWidth = true
					});
					yPosition += 25;
				}
				yPosition += 5;
				if (!Quantity.Currency.IsItem)
				{
					Controls.Add(new Label
					{
						Text = MysticCrafting.Module.Strings.Recipe.WalletLabel,
						Font = GameService.Content.DefaultFont16,
						Location = new Point(0, yPosition),
						TextColor = Color.LightGray,
						ShowShadow = true,
						AutoSizeWidth = true
					});
				}
			}
		}

		protected override void Build(Container buildPanel)
		{
			foreach (Control control in Controls)
			{
				control.Parent = buildPanel;
			}
		}
	}
}
