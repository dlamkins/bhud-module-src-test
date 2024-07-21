using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Tooltips
{
	public class ItemTooltipView : View, ITooltipView, IView
	{
		private int _playerItemCount;

		private int _requiredQuantity;

		protected bool Initialized;

		private Item Item { get; set; }

		public Container BuildPanel { get; set; }

		protected int Bottom { get; set; }

		private IDictionary<string, int> CharacterCount { get; set; }

		private int MaterialStorageCount { get; set; }

		private int SharedInventoryCount { get; set; }

		private int BankCount { get; set; }

		private Label ItemCountLabel { get; set; }

		private Label MoreRequiredLabel { get; set; }

		protected Label AccountTitleLabel { get; set; }

		public int PlayerItemCount
		{
			get
			{
				return _playerItemCount;
			}
			set
			{
				_playerItemCount = value;
				if (ItemCountLabel != null)
				{
					ItemCountLabel.set_Text(PlayerCountText);
				}
				SetRequiredQuantityLabel();
			}
		}

		public virtual int RequiredQuantity
		{
			get
			{
				return _requiredQuantity;
			}
			set
			{
				_requiredQuantity = value;
				SetRequiredQuantityLabel();
			}
		}

		public virtual string PlayerCountText => $"{PlayerItemCount:N0}/{RequiredQuantity:N0}";

		public ItemTooltipView(Item item, int requiredQuantity)
			: this()
		{
			Item = item;
			PlayerItemCount = ServiceContainer.PlayerItemService.GetItemCount(Item.Id);
			RequiredQuantity = requiredQuantity;
		}

		protected override void Build(Container buildPanel)
		{
			BuildPanel = buildPanel;
			if (!Initialized)
			{
				Initialize();
			}
		}

		public virtual void Initialize()
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Expected O, but got Unknown
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Expected O, but got Unknown
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Expected O, but got Unknown
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			CharacterCount = ServiceContainer.PlayerItemService.GetCharacterInventoryItemCount(Item.Id);
			MaterialStorageCount = ServiceContainer.PlayerItemService.GetMaterialItemCount(Item.Id);
			SharedInventoryCount = ServiceContainer.PlayerItemService.GetSharedInventoryCount(Item.Id);
			BankCount = ServiceContainer.PlayerItemService.GetBankItemCount(Item.Id);
			Image val = new Image(ServiceContainer.TextureRepository.GetTexture(Item.Icon));
			((Control)val).set_Parent(BuildPanel);
			((Control)val).set_Size(new Point(30, 30));
			((Control)val).set_Location(new Point(0, 0));
			Label val2 = new Label();
			((Control)val2).set_Parent(BuildPanel);
			val2.set_Text(Item.LocalizedName());
			val2.set_Font(GameService.Content.get_DefaultFont18());
			((Control)val2).set_Location(new Point(40, 5));
			val2.set_TextColor(ColorHelper.FromRarity(Item.Rarity.ToString()));
			val2.set_StrokeText(true);
			val2.set_AutoSizeWidth(true);
			int yPosition = 35;
			if (!string.IsNullOrEmpty(Item.LocalizedDescription()))
			{
				Label val3 = new Label();
				((Control)val3).set_Parent(BuildPanel);
				val3.set_Text(Item.LocalizedDescription());
				val3.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val3).set_Location(new Point(0, 40));
				val3.set_TextColor(Color.get_White());
				((Control)val3).set_Width(400);
				val3.set_AutoSizeHeight(true);
				val3.set_WrapText(true);
				val3.set_StrokeText(true);
				Label descriptionLabel = val3;
				yPosition += ((Control)descriptionLabel).get_Height() + 10;
			}
			if (PlayerItemCount != 0 || RequiredQuantity != 0)
			{
				Label val4 = new Label();
				((Control)val4).set_Parent(BuildPanel);
				val4.set_Text(PlayerCountText);
				((Control)val4).set_Location(new Point(0, yPosition));
				val4.set_Font(GameService.Content.get_DefaultFont18());
				val4.set_StrokeText(true);
				val4.set_TextColor(Color.get_LightYellow());
				val4.set_AutoSizeWidth(true);
				ItemCountLabel = val4;
				yPosition = ((Control)ItemCountLabel).get_Bottom() + 5;
			}
			if (RequiredQuantity > 0)
			{
				Label val5 = new Label();
				((Control)val5).set_Parent(BuildPanel);
				val5.set_Text(string.Format(Recipe.MoreRequired, RequiredQuantity.ToString("N0")));
				((Control)val5).set_Location(new Point(0, yPosition));
				val5.set_Font(GameService.Content.get_DefaultFont16());
				val5.set_TextColor(ColorHelper.BrightBlue);
				val5.set_StrokeText(true);
				val5.set_AutoSizeWidth(true);
				MoreRequiredLabel = val5;
				yPosition += 25;
			}
			yPosition += 10;
			if (Item.HasSkin())
			{
				bool skinUnlocked = ServiceContainer.PlayerUnlocksService.ItemUnlocked(Item.DefaultSkin);
				Label val6 = new Label();
				((Control)val6).set_Parent(BuildPanel);
				val6.set_Text(skinUnlocked ? Common.SkinUnlocked : Common.SkinLocked);
				val6.set_TextColor(skinUnlocked ? ColorHelper.BrightGreen : Color.get_LightGray());
				val6.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val6).set_Location(new Point(0, yPosition));
				val6.set_AutoSizeHeight(true);
				val6.set_AutoSizeWidth(true);
				val6.set_StrokeText(true);
				yPosition += 25;
			}
			BuildItemCounters(ref yPosition);
			Initialized = true;
			Bottom = yPosition;
		}

		public void SetRequiredQuantityLabel()
		{
			if (MoreRequiredLabel != null)
			{
				MoreRequiredLabel.set_Text(string.Format(Recipe.MoreRequired, RequiredQuantity.ToString("N0")));
			}
			if (ItemCountLabel != null)
			{
				ItemCountLabel.set_Text(PlayerCountText);
			}
		}

		public void BuildItemCounters(ref int yPosition)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			if (BankCount != 0 || MaterialStorageCount != 0 || CharacterCount.Any((KeyValuePair<string, int> c) => c.Value != 0))
			{
				Label val = new Label();
				((Control)val).set_Parent(BuildPanel);
				val.set_Text(Recipe.Account);
				((Control)val).set_Location(new Point(0, yPosition));
				val.set_Font(GameService.Content.get_DefaultFont16());
				val.set_ShowShadow(true);
				val.set_TextColor(Color.get_White());
				val.set_AutoSizeWidth(true);
				AccountTitleLabel = val;
				int totalPlayerItemCount = ServiceContainer.PlayerItemService.GetItemCount(Item.Id);
				if (AccountTitleLabel != null)
				{
					Label val2 = new Label();
					((Control)val2).set_Parent(BuildPanel);
					val2.set_Text($"({totalPlayerItemCount})");
					((Control)val2).set_Location(new Point(((Control)AccountTitleLabel).get_Right() + 5, ((Control)AccountTitleLabel).get_Top()));
					val2.set_Font(GameService.Content.get_DefaultFont16());
					val2.set_ShowShadow(true);
					val2.set_TextColor(Color.get_LightGray());
					val2.set_AutoSizeWidth(true);
				}
				yPosition += 25;
			}
			if (BankCount != 0)
			{
				Label val3 = new Label();
				((Control)val3).set_Parent(BuildPanel);
				val3.set_Text(string.Format(Recipe.CountInBank, BankCount.ToString("N0")));
				((Control)val3).set_Location(new Point(0, yPosition));
				val3.set_Font(GameService.Content.get_DefaultFont16());
				val3.set_TextColor(Color.get_LightGray());
				val3.set_ShowShadow(true);
				val3.set_AutoSizeWidth(true);
				yPosition += 20;
			}
			if (MaterialStorageCount != 0)
			{
				Label val4 = new Label();
				((Control)val4).set_Parent(BuildPanel);
				val4.set_Text(string.Format(Recipe.CountInMaterialStorage, MaterialStorageCount.ToString("N0")));
				((Control)val4).set_Location(new Point(0, yPosition));
				val4.set_Font(GameService.Content.get_DefaultFont16());
				val4.set_TextColor(Color.get_LightGray());
				val4.set_ShowShadow(true);
				val4.set_AutoSizeWidth(true);
				yPosition += 20;
			}
			if (SharedInventoryCount != 0)
			{
				Label val5 = new Label();
				((Control)val5).set_Parent(BuildPanel);
				val5.set_Text(string.Format(Recipe.CountInSharedInventory, SharedInventoryCount.ToString("N0")));
				((Control)val5).set_Location(new Point(0, yPosition));
				val5.set_Font(GameService.Content.get_DefaultFont16());
				val5.set_TextColor(Color.get_LightGray());
				val5.set_ShowShadow(true);
				val5.set_AutoSizeWidth(true);
				yPosition += 20;
			}
			foreach (KeyValuePair<string, int> characterCount in CharacterCount)
			{
				if (characterCount.Value != 0)
				{
					Label val6 = new Label();
					((Control)val6).set_Parent(BuildPanel);
					val6.set_Text(string.Format(Recipe.CountInInventoryCharacter, characterCount.Value.ToString("N0"), characterCount.Key));
					((Control)val6).set_Location(new Point(0, yPosition));
					val6.set_Font(GameService.Content.get_DefaultFont16());
					val6.set_TextColor(Color.get_LightGray());
					val6.set_ShowShadow(true);
					val6.set_AutoSizeWidth(true);
					yPosition += 20;
				}
			}
		}

		protected override void Unload()
		{
			Label itemCountLabel = ItemCountLabel;
			if (itemCountLabel != null)
			{
				((Control)itemCountLabel).Dispose();
			}
			Label moreRequiredLabel = MoreRequiredLabel;
			if (moreRequiredLabel != null)
			{
				((Control)moreRequiredLabel).Dispose();
			}
			Label accountTitleLabel = AccountTitleLabel;
			if (accountTitleLabel != null)
			{
				((Control)accountTitleLabel).Dispose();
			}
			BuildPanel = null;
			((View<IPresenter>)this).Unload();
		}
	}
}
