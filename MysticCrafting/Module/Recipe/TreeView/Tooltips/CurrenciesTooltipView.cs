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
	public class CurrenciesTooltipView : View, ITooltipView, IView
	{
		public IList<MysticCurrencyQuantity> Quantities { get; set; }

		private IList<Control> Controls { get; set; } = new List<Control>();


		public Point IconSize { get; set; } = new Point(30, 30);


		public CurrenciesTooltipView(IList<MysticCurrencyQuantity> quantities)
		{
			BuildControls(quantities);
		}

		public void BuildControls(IList<MysticCurrencyQuantity> quantities)
		{
			Quantities = quantities;
			foreach (Control control in Controls)
			{
				control.Dispose();
			}
			int paddingTop = 5;
			foreach (MysticCurrencyQuantity quantity in quantities)
			{
				BuildControls(quantity, ref paddingTop);
			}
		}

		private void BuildControls(MysticCurrencyQuantity quantity, ref int paddingTop)
		{
			if (quantity.Currency.Name == "Coin")
			{
				return;
			}
			if (quantity.Currency?.Icon != null)
			{
				AsyncTexture2D texture = ServiceContainer.TextureRepository.GetTexture(quantity.Currency.Icon);
				if (texture != null)
				{
					Controls.Add(new Image(texture)
					{
						Size = IconSize,
						Location = new Point(0, paddingTop)
					});
				}
			}
			MysticCurrencyQuantity walletQuantity = ServiceContainer.WalletService.GetQuantity(quantity.Currency.GameId);
			if (walletQuantity == null)
			{
				return;
			}
			paddingTop += 5;
			Label textLabel = new Label
			{
				Text = walletQuantity.Count.ToString("N0"),
				Font = GameService.Content.DefaultFont16,
				Location = new Point(40, paddingTop),
				TextColor = ColorHelper.FromItemCount(walletQuantity.Count, quantity.Count),
				StrokeText = true,
				AutoSizeWidth = true
			};
			Label totalLabel = new Label
			{
				Text = $"/{quantity.Count:N0}",
				Font = GameService.Content.DefaultFont16,
				Location = new Point(textLabel.Right, paddingTop),
				TextColor = Color.White,
				StrokeText = true,
				AutoSizeWidth = true
			};
			Controls.Add(textLabel);
			Controls.Add(totalLabel);
			int paddingLeft = totalLabel.Right + 5;
			Color nameColor = Color.White;
			if (quantity.Currency.IsItem)
			{
				MysticItem currencyItem = ServiceContainer.ItemRepository.GetItem(quantity.Currency.GameId);
				if (currencyItem != null)
				{
					nameColor = ColorHelper.FromRarity(currencyItem.Rarity);
				}
			}
			Label nameLabel = new Label
			{
				Text = quantity.Currency.LocalizedName(),
				Font = GameService.Content.DefaultFont16,
				Location = new Point(paddingLeft, paddingTop),
				TextColor = nameColor,
				StrokeText = true,
				AutoSizeWidth = true
			};
			Controls.Add(nameLabel);
			int moreRequired = quantity.Count - walletQuantity.Count;
			if (moreRequired > 0)
			{
				Controls.Add(new Label
				{
					Text = "(" + string.Format(MysticCrafting.Module.Strings.Recipe.MoreRequired, moreRequired) + ")",
					Font = GameService.Content.DefaultFont14,
					Location = new Point(nameLabel.Right + 10, paddingTop),
					TextColor = Color.LightGray,
					ShowShadow = true,
					AutoSizeWidth = true
				});
			}
			paddingTop += 35;
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
