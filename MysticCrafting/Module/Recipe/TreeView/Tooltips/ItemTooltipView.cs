using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Recipe.TreeView.Tooltips
{
	public class ItemTooltipView : View, ITooltipView, IView
	{
		private int _playerItemCount;

		private int _requiredQuantity;

		protected bool Initialized;

		private MysticItem Item { get; set; }

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
					ItemCountLabel.Text = PlayerCountText;
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

		public ItemTooltipView(MysticItem item, int requiredQuantity)
		{
			Item = item;
			PlayerItemCount = ServiceContainer.PlayerItemService.GetItemCount(Item.GameId);
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
			CharacterCount = ServiceContainer.PlayerItemService.GetCharacterInventoryItemCount(Item.GameId);
			MaterialStorageCount = ServiceContainer.PlayerItemService.GetMaterialItemCount(Item.GameId);
			SharedInventoryCount = ServiceContainer.PlayerItemService.GetSharedInventoryCount(Item.GameId);
			BankCount = ServiceContainer.PlayerItemService.GetBankItemCount(Item.GameId);
			new Image(ServiceContainer.TextureRepository.GetTexture(Item.Icon))
			{
				Parent = BuildPanel,
				Size = new Point(30, 30),
				Location = new Point(0, 0)
			};
			new Label
			{
				Parent = BuildPanel,
				Text = Item.LocalizedName(),
				Font = GameService.Content.DefaultFont18,
				Location = new Point(40, 5),
				TextColor = ColorHelper.FromRarity(Item.Rarity),
				StrokeText = true,
				AutoSizeWidth = true
			};
			int yPosition = 35;
			if (!string.IsNullOrEmpty(Item.LocalizedDescription()))
			{
				Label descriptionLabel = new Label
				{
					Parent = BuildPanel,
					Text = Item.LocalizedDescription(),
					Font = GameService.Content.DefaultFont14,
					Location = new Point(0, 40),
					TextColor = Color.White,
					Width = 400,
					AutoSizeHeight = true,
					WrapText = true,
					StrokeText = true
				};
				yPosition += descriptionLabel.Height + 10;
			}
			if (PlayerItemCount != 0 || RequiredQuantity != 0)
			{
				ItemCountLabel = new Label
				{
					Parent = BuildPanel,
					Text = PlayerCountText,
					Location = new Point(0, yPosition),
					Font = GameService.Content.DefaultFont18,
					StrokeText = true,
					TextColor = Color.LightYellow,
					AutoSizeWidth = true
				};
				yPosition = ItemCountLabel.Bottom + 5;
			}
			if (RequiredQuantity > 0)
			{
				MoreRequiredLabel = new Label
				{
					Parent = BuildPanel,
					Text = string.Format(MysticCrafting.Module.Strings.Recipe.MoreRequired, RequiredQuantity.ToString("N0")),
					Location = new Point(0, yPosition),
					Font = GameService.Content.DefaultFont16,
					TextColor = ColorHelper.BrightBlue,
					StrokeText = true,
					AutoSizeWidth = true
				};
				yPosition += 25;
			}
			yPosition += 10;
			if (Item.HasSkin())
			{
				bool skinUnlocked = ServiceContainer.PlayerUnlocksService.ItemUnlocked(Item.DefaultSkin.GetValueOrDefault());
				new Label
				{
					Parent = BuildPanel,
					Text = (skinUnlocked ? Common.SkinUnlocked : Common.SkinLocked),
					TextColor = (skinUnlocked ? ColorHelper.BrightGreen : Color.LightGray),
					Font = GameService.Content.DefaultFont14,
					Location = new Point(0, yPosition),
					AutoSizeHeight = true,
					AutoSizeWidth = true,
					StrokeText = true
				};
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
				MoreRequiredLabel.Text = string.Format(MysticCrafting.Module.Strings.Recipe.MoreRequired, RequiredQuantity.ToString("N0"));
			}
			if (ItemCountLabel != null)
			{
				ItemCountLabel.Text = PlayerCountText;
			}
		}

		public void BuildItemCounters(ref int yPosition)
		{
			if (BankCount != 0 || MaterialStorageCount != 0 || CharacterCount.Any((KeyValuePair<string, int> c) => c.Value != 0))
			{
				AccountTitleLabel = new Label
				{
					Parent = BuildPanel,
					Text = MysticCrafting.Module.Strings.Recipe.Account,
					Location = new Point(0, yPosition),
					Font = GameService.Content.DefaultFont16,
					ShowShadow = true,
					TextColor = Color.White,
					AutoSizeWidth = true
				};
				int totalPlayerItemCount = ServiceContainer.PlayerItemService.GetItemCount(Item.GameId);
				if (AccountTitleLabel != null)
				{
					new Label
					{
						Parent = BuildPanel,
						Text = $"({totalPlayerItemCount})",
						Location = new Point(AccountTitleLabel.Right + 5, AccountTitleLabel.Top),
						Font = GameService.Content.DefaultFont16,
						ShowShadow = true,
						TextColor = Color.LightGray,
						AutoSizeWidth = true
					};
				}
				yPosition += 25;
			}
			if (BankCount != 0)
			{
				new Label
				{
					Parent = BuildPanel,
					Text = string.Format(MysticCrafting.Module.Strings.Recipe.CountInBank, BankCount.ToString("N0")),
					Location = new Point(0, yPosition),
					Font = GameService.Content.DefaultFont16,
					TextColor = Color.LightGray,
					ShowShadow = true,
					AutoSizeWidth = true
				};
				yPosition += 20;
			}
			if (MaterialStorageCount != 0)
			{
				new Label
				{
					Parent = BuildPanel,
					Text = string.Format(MysticCrafting.Module.Strings.Recipe.CountInMaterialStorage, MaterialStorageCount.ToString("N0")),
					Location = new Point(0, yPosition),
					Font = GameService.Content.DefaultFont16,
					TextColor = Color.LightGray,
					ShowShadow = true,
					AutoSizeWidth = true
				};
				yPosition += 20;
			}
			if (SharedInventoryCount != 0)
			{
				new Label
				{
					Parent = BuildPanel,
					Text = string.Format(MysticCrafting.Module.Strings.Recipe.CountInSharedInventory, SharedInventoryCount.ToString("N0")),
					Location = new Point(0, yPosition),
					Font = GameService.Content.DefaultFont16,
					TextColor = Color.LightGray,
					ShowShadow = true,
					AutoSizeWidth = true
				};
				yPosition += 20;
			}
			foreach (KeyValuePair<string, int> characterCount in CharacterCount)
			{
				if (characterCount.Value != 0)
				{
					new Label
					{
						Parent = BuildPanel,
						Text = string.Format(MysticCrafting.Module.Strings.Recipe.CountInInventoryCharacter, characterCount.Value.ToString("N0"), characterCount.Key),
						Location = new Point(0, yPosition),
						Font = GameService.Content.DefaultFont16,
						TextColor = Color.LightGray,
						ShowShadow = true,
						AutoSizeWidth = true
					};
					yPosition += 20;
				}
			}
		}

		protected override void Unload()
		{
			ItemCountLabel?.Dispose();
			MoreRequiredLabel?.Dispose();
			AccountTitleLabel?.Dispose();
			BuildPanel = null;
			base.Unload();
		}
	}
}
