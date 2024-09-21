using System;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Discovery.ItemList.Controls;
using MysticCrafting.Module.Discovery.ItemList.Presenters;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Helpers;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Tooltips;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery.ItemList
{
	public class ItemRowView : View
	{
		public EventHandler<MouseEventArgs> OnClick;

		private Item Item { get; set; }

		private bool DisplayDetailsType { get; set; }

		public CommercePrices Prices { get; set; }

		public ItemRowView(Item item, bool displayDetailsType = false)
			: this()
		{
			Item = item;
			DisplayDetailsType = displayDetailsType;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03de: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0417: Unknown result type (might be due to invalid IL or missing references)
			//IL_0425: Unknown result type (might be due to invalid IL or missing references)
			//IL_0434: Unknown result type (might be due to invalid IL or missing references)
			if (Item == null)
			{
				return;
			}
			ItemButton itemButton = new ItemButton();
			((Control)itemButton).set_Parent(buildPanel);
			itemButton.Text = Item.LocalizedName().Truncate(35);
			itemButton.Rarity = Item.Rarity.ToString();
			itemButton.Type = LocalizationHelper.TranslateMenuItem(Item.DetailsType);
			itemButton.Item = Item;
			((Control)itemButton).set_Size(new Point(344, 45));
			ItemButton detailsButton = itemButton;
			AsyncTexture2D iconTexture = ServiceContainer.TextureRepository.GetTexture(Item.Icon);
			if (iconTexture != null)
			{
				try
				{
					Image val = new Image(iconTexture);
					((Control)val).set_Parent(buildPanel);
					((Control)val).set_Location(new Point(2, 3));
					((Control)val).set_Size(new Point(40, 40));
					((Control)val).set_Tooltip((Tooltip)(object)new DisposableTooltip((ITooltipView)(object)new ItemTooltipView(Item, 0)));
				}
				catch (Exception)
				{
				}
			}
			if (Item.ArmorWeight != 0)
			{
				Label val2 = new Label();
				((Control)val2).set_Parent(buildPanel);
				val2.set_AutoSizeWidth(true);
				((Control)val2).set_Location(new Point(((Control)buildPanel).get_Size().X - 300, ((Control)buildPanel).get_Size().Y / 2 - 10));
				val2.set_Text(Item.ArmorWeight.ToString());
			}
			if (DisplayDetailsType)
			{
				Label val3 = new Label();
				((Control)val3).set_Parent(buildPanel);
				val3.set_AutoSizeWidth(true);
				((Control)val3).set_Location(new Point(((Control)buildPanel).get_Size().X - 450, ((Control)buildPanel).get_Size().Y / 2 - 10));
				val3.set_Text(Item.DetailsType);
			}
			if (Item.HasSkin())
			{
				_ = Item.DefaultSkin;
			}
			else if (!Item.IsDye())
			{
				goto IL_0290;
			}
			bool skinUnlocked = (Item.IsDye() ? ServiceContainer.PlayerUnlocksService.DyeUnlocked(Item.Id) : ServiceContainer.PlayerUnlocksService.ItemUnlocked(Item.DefaultSkin));
			Image val4 = new Image(skinUnlocked ? ServiceContainer.TextureRepository.Textures.Checkmark : ServiceContainer.TextureRepository.Textures.Lock);
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Location(new Point(((Control)buildPanel).get_Size().X - 210, ((Control)buildPanel).get_Size().Y / 2 - 15));
			((Control)val4).set_Size(new Point(30, 30));
			((Control)val4).set_BasicTooltipText(skinUnlocked ? Common.SkinUnlocked : Common.SkinLocked);
			goto IL_0290;
			IL_0290:
			if (Item.Rarity == ItemRarity.Legendary)
			{
				int unlockedCount2 = ServiceContainer.PlayerUnlocksService.LegendaryUnlockedCount(Item.Id);
				int? maxCount = Item.GetMaxCount();
				Label val5 = new Label();
				((Control)val5).set_Parent(buildPanel);
				val5.set_AutoSizeWidth(true);
				((Control)val5).set_Location(new Point(((Control)buildPanel).get_Size().X - 120, ((Control)buildPanel).get_Size().Y / 2 - 10));
				val5.set_Font(GameService.Content.get_DefaultFont14());
				val5.set_Text($"{unlockedCount2}/{maxCount}");
				((Control)val5).set_BasicTooltipText(string.Format(Recipe.TooltipUnlockedItem, unlockedCount2, maxCount));
			}
			else if (!Item.IsDecoration())
			{
				int unlockedCount = ServiceContainer.PlayerItemService.GetItemCount(Item.Id);
				Label val6 = new Label();
				((Control)val6).set_Parent(buildPanel);
				val6.set_AutoSizeWidth(true);
				((Control)val6).set_Location(new Point(((Control)buildPanel).get_Size().X - 120, ((Control)buildPanel).get_Size().Y / 2 - 10));
				val6.set_Font(GameService.Content.get_DefaultFont16());
				val6.set_Text($"{unlockedCount}");
				val6.set_TextColor(Color.get_LightYellow());
				((Control)val6).set_BasicTooltipText(string.Format(Recipe.TooltipCollectedItem, unlockedCount));
			}
			((Control)new ButtonPresenter().BuildFavoriteButton(Item.Id, buildPanel)).set_Location(new Point(((Control)buildPanel).get_Size().X - 50, ((Control)buildPanel).get_Size().Y / 2 - 20));
			if (OnClick != null)
			{
				((Control)detailsButton).add_Click(OnClick);
			}
		}
	}
}
