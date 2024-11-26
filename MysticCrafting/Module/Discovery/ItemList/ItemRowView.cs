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

		private Image Icon { get; set; }

		private DisposableTooltip ItemTooltip { get; set; }

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
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Expected O, but got Unknown
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_039b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0423: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
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
			AsyncTexture2D iconTexture = ServiceContainer.TextureRepository.GetTexture(Item.IconId);
			if (iconTexture != null)
			{
				try
				{
					Image val = new Image(iconTexture);
					((Control)val).set_Parent(buildPanel);
					((Control)val).set_Location(new Point(2, 3));
					((Control)val).set_Size(new Point(40, 40));
					Icon = val;
					ItemTooltip = new DisposableTooltip((ITooltipView)(object)new ItemTooltipView(Item, 0));
					((Control)Icon).set_Tooltip((Tooltip)(object)ItemTooltip);
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
				val2.set_Text(LocalizationHelper.TranslateWeightClass(Item.ArmorWeight));
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
				goto IL_029c;
			}
			bool skinUnlocked = (Item.IsDye() ? ServiceContainer.PlayerUnlocksService.DyeUnlocked(Item.Id) : ServiceContainer.PlayerUnlocksService.ItemUnlocked(Item.DefaultSkin));
			Image val4 = new Image(skinUnlocked ? ServiceContainer.TextureRepository.Textures.Checkmark : ServiceContainer.TextureRepository.Textures.Lock);
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Location(new Point(((Control)buildPanel).get_Size().X - 210, ((Control)buildPanel).get_Size().Y / 2 - 15));
			((Control)val4).set_Size(new Point(30, 30));
			((Control)val4).set_BasicTooltipText(skinUnlocked ? Common.SkinUnlocked : Common.SkinLocked);
			goto IL_029c;
			IL_029c:
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

		protected override void Unload()
		{
			Item = null;
			Image icon = Icon;
			if (icon != null)
			{
				((Control)icon).Dispose();
			}
			DisposableTooltip itemTooltip = ItemTooltip;
			if (itemTooltip != null)
			{
				((Control)itemTooltip).Dispose();
			}
			((View<IPresenter>)this).Unload();
		}
	}
}
