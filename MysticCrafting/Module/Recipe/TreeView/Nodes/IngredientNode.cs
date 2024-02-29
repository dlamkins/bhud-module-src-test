using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MysticCrafting.Models;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Recipe.TreeView.Extensions;
using MysticCrafting.Module.Recipe.TreeView.Presenters;
using MysticCrafting.Module.Recipe.TreeView.Tooltips;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	[DebuggerDisplay("Name = {Item.Name}")]
	public class IngredientNode : TradeableItemNode, IIngredient, IIngredientContainer
	{
		private int _nodeIndex;

		private bool _isLinked;

		private bool _enoughCollected;

		private Image _missingTabs;

		public int? IngredientIndex { get; set; }

		public int NodeIndex
		{
			get
			{
				if (_nodeIndex == 0)
				{
					NodeIndex = this.CalculateIndex();
				}
				return _nodeIndex;
			}
			set
			{
				_nodeIndex = value;
			}
		}

		public BitmapFont Font { get; set; } = GameService.Content.DefaultFont16;


		public TreeView TreeView { get; private set; }

		public MysticItem Item { get; set; }

		public MysticIngredient Ingredient { get; }

		public IngredientNode ParentNode { get; set; }

		public Image IconBackground { get; set; }

		public Image Icon { get; set; }

		public Image IconChain { get; set; }

		public Image IconSwap { get; set; }

		public int IconSize { get; set; } = 35;


		protected Label ItemNameLabel { get; set; }

		private LoadingSpinner LoadingSpinner { get; set; }

		private Label ItemCountLabel { get; set; }

		protected Tooltip ItemCountTooltip { get; set; }

		public ItemNodeTooltipView ItemCountTooltipView { get; set; }

		private Label ItemRecipeRequiredCountLabel { get; set; }

		private NumberBox ItemCountNumberBox { get; set; }

		private NumberBoxTooltipView ItemCountNumberTooltipView { get; set; }

		private Label ItemCountNumberLabel { get; set; }

		public IEnumerable<IngredientNode> IngredientNodes => _children.OfType<IngredientNode>();

		public IItemSource SelectedItemSource => base.Children?.OfType<ItemTab>()?.FirstOrDefault((ItemTab t) => t.Active)?.ItemSource;

		public bool IsLinked
		{
			get
			{
				return _isLinked;
			}
			set
			{
				if (_isLinked != value)
				{
					_isLinked = value;
					UpdateLinkedItemControls();
					UpdateItemCountControls();
				}
			}
		}

		public bool IsSharedItem { get; set; }

		public bool IsTopItem { get; private set; }

		public virtual int RecipeRequiredQuantity { get; set; } = 1;


		public virtual int TotalRequiredQuantity { get; set; }

		public override int RequiredQuantity
		{
			get
			{
				if (IsSharedItem && !TreeView.IngredientNodes.IsFirstNode(this))
				{
					return 0;
				}
				return Math.Max(0, TotalRequiredQuantity - TotalPlayerItemCount);
			}
		}

		public int ReservedQuantity { get; set; }

		public int PlayerItemCount { get; set; }

		public int TotalPlayerItemCount => Math.Max(0, PlayerItemCount - ReservedQuantity);

		public override string PathName => $"{IngredientIndex}-{Item.GameId}";

		private CraftingDisciplinesControl RequirementsControl { get; set; }

		public IngredientNode(MysticItem item, int? quantity = null, int? index = null)
		{
			base.EffectBehind = new ScrollingHighlightEffect(this);
			RecipeRequiredQuantity = quantity ?? 1;
			Item = item;
			IngredientIndex = index;
			Ingredient = new MysticIngredient
			{
				GameId = item.GameId,
				Quantity = quantity,
				Index = index,
				Name = item.LocalizedName(),
				Item = item
			};
			UpdateItemDetails();
		}

		public IngredientNode(MysticIngredient ingredient, MysticItem item, Blish_HUD.Controls.Container parent, bool loadingChildren = false)
		{
			base.EffectBehind = new ScrollingHighlightEffect(this);
			LoadingChildren = loadingChildren;
			RecipeRequiredQuantity = ingredient.Quantity ?? 1;
			Item = item;
			IngredientIndex = ingredient.Index;
			Ingredient = ingredient;
			PlayerItemCount = ServiceContainer.PlayerItemService.GetItemCount(item.GameId);
			SetParentProperties(parent);
		}

		private void SetParentProperties(Blish_HUD.Controls.Container parent)
		{
			IngredientNode parentNode = parent as IngredientNode;
			if (parentNode != null)
			{
				ParentNode = parentNode;
				TreeView = parentNode.TreeView;
				return;
			}
			TreeView treeView = parent as TreeView;
			if (treeView != null)
			{
				IsTopItem = true;
				TotalRequiredQuantity = 1;
				TreeView = treeView;
				Font = GameService.Content.DefaultFont18;
				BackgroundOpaqueColor = ColorHelper.FromRarity(Item.Rarity) * 0.2f;
				FrameColor = ColorHelper.FromRarity(Item.Rarity) * 0.8f;
			}
		}

		public virtual void Build(Blish_HUD.Controls.Container parent)
		{
			UpdateTotalRequiredQuantity();
			UpdateItemDetails();
			if (IsTopItem)
			{
				BuildItemCountEditControls();
			}
			else
			{
				BuildItemCountControls();
			}
			BuildItemCountTooltip();
			base.Parent = parent;
			UpdateItemCountControls();
		}

		public void ClearChildIngredientNodes()
		{
			IEnumerable<IngredientNode> childNodes = base.Nodes.OfType<IngredientNode>();
			while (childNodes.Count() > 0)
			{
				childNodes.FirstOrDefault().Parent = null;
			}
		}

		public void UpdateItemDetails()
		{
			ClearChildren();
			if (Icon == null)
			{
				IconBackground = new Image(ServiceContainer.TextureRepository.Textures.ItemBackground)
				{
					Parent = this,
					Size = new Point(IconSize + 8, IconSize + 8),
					Location = new Point(IsTopItem ? 3 : 24, 1)
				};
				if (!string.IsNullOrEmpty(Item.Icon))
				{
					AsyncTexture2D icon = ServiceContainer.TextureRepository.GetTexture(Item.Icon);
					if (icon != null)
					{
						Icon = new Image(icon)
						{
							Parent = this,
							Size = new Point(IconSize, IconSize),
							Location = new Point(IsTopItem ? 7 : 28, 5)
						};
					}
				}
				else
				{
					Icon = new Image(AsyncTexture2D.FromAssetId(1318604))
					{
						Parent = this,
						Size = new Point(IconSize, IconSize),
						Location = new Point(IsTopItem ? 7 : 28, 5)
					};
				}
			}
			int paddingLeft = (ItemCountLabel?.Right ?? ItemRecipeRequiredCountLabel?.Right ?? Icon?.Right ?? (30 + IconSize)) + 5;
			ItemNameLabel = new Label
			{
				Parent = this,
				Text = (Item.LocalizedName() ?? "").Truncate(24),
				Location = new Point(paddingLeft, 12),
				Width = 200,
				Font = Font,
				TextColor = ColorHelper.FromRarity(Item.Rarity),
				StrokeText = true,
				AutoSizeHeight = true
			};
			BuildMenuStrip();
			if (!LoadingChildren)
			{
				OnChildrenLoaded();
				return;
			}
			LoadingSpinner = new LoadingSpinner
			{
				Parent = this,
				Size = new Point(40, 40),
				Location = new Point(PriceLocation.X, 3)
			};
		}

		public void OnChildrenLoaded()
		{
			LoadingSpinner?.Dispose();
			UpdatePriceControls();
		}

		private void UpdateLinkedItemControls()
		{
			if (IconChain == null)
			{
				AsyncTexture2D iconTexture = (IsSharedItem ? ServiceContainer.TextureRepository.Textures.ChainGold : ServiceContainer.TextureRepository.Textures.Chain);
				IconChain = new Image(iconTexture)
				{
					Parent = this,
					Size = new Point(IconSize + 7, IconSize + 7),
					Location = new Point(28, 5),
					Visible = true
				};
				if (ItemCountTooltip != null)
				{
					IconChain.Tooltip = ItemCountTooltip;
				}
			}
			else if (IsLinked)
			{
				IconChain.Show();
			}
			else
			{
				IconChain?.Hide();
			}
			if (IsLinked)
			{
				ItemCountTooltipView?.UpdateLinkedNodes();
			}
		}

		private void BuildMenuStrip()
		{
			ContextMenuPresenter menuStripPresenter = new ContextMenuPresenter(ServiceContainer.WikiLinkRepository);
			MenuStrip = menuStripPresenter.BuildMenuStrip(Item, this);
		}

		public void BuildMissingTabsLabel()
		{
			_missingTabs?.Dispose();
			_missingTabs = new Image(ServiceContainer.TextureRepository.Textures.ExclamationMark)
			{
				Parent = this,
				Location = new Point(base.Width - 40, 5),
				Tint = Color.Orange,
				BasicTooltipText = "The module does not currently support the methods to acquire this item. Right click on this item to easily open the wiki."
			};
		}

		public virtual void BuildItemCountTooltip()
		{
			ItemCountTooltipView = new ItemNodeTooltipView(this);
			ItemCountTooltip = new DisposableTooltip(ItemCountTooltipView);
			if (ItemNameLabel != null)
			{
				ItemNameLabel.Tooltip = ItemCountTooltip;
			}
			if (Icon != null)
			{
				Icon.Tooltip = ItemCountTooltip;
			}
			if (ItemCountLabel != null)
			{
				ItemCountLabel.Tooltip = ItemCountTooltip;
			}
		}

		public void UpdateItemCountTooltip()
		{
			if (ItemCountTooltipView != null)
			{
				ItemCountTooltipView.RequiredQuantity = RequiredQuantity;
			}
		}

		public int GetPlayerItemCount()
		{
			if (!IsLinked || TotalPlayerItemCount <= TotalRequiredQuantity)
			{
				return TotalPlayerItemCount;
			}
			return TotalRequiredQuantity;
		}

		public string GetPlayerCountAsText()
		{
			return $"{GetPlayerItemCount()}/{TotalRequiredQuantity}";
		}

		public void BuildItemCountEditControls()
		{
			ItemCountNumberBox?.Dispose();
			ItemCountNumberLabel?.Dispose();
			int? maxValue = Item.GetMaxCount();
			if (maxValue > 1)
			{
				ItemCountNumberBox = new NumberBox
				{
					Value = TotalRequiredQuantity,
					MinValue = 1,
					MaxValue = (maxValue ?? int.MaxValue),
					Font = GameService.Content.DefaultFont18,
					Parent = this,
					Location = new Point(Icon.Right + 5, 10),
					Size = new Point(30, 27)
				};
				ItemCountNumberTooltipView = new NumberBoxTooltipView
				{
					Text = MysticCrafting.Module.Strings.Recipe.ItemCountBoxTooltipText
				};
				ItemCountNumberBox.Tooltip = new DisposableTooltip(ItemCountNumberTooltipView);
				ItemCountNumberBox.AfterTextChanged += delegate
				{
					UpdateTotalRequiredQuantity();
				};
				if (Item.RarityEnum != MysticItemRarity.Legendary)
				{
					return;
				}
				int unlockedCount = ServiceContainer.PlayerUnlocksService.LegendaryUnlockedCount(Item.GameId);
				NumberBox itemCountNumberBox = ItemCountNumberBox;
				itemCountNumberBox.MaxValueChanged = (EventHandler<PropertyChangedEventArgs>)Delegate.Combine(itemCountNumberBox.MaxValueChanged, (EventHandler<PropertyChangedEventArgs>)delegate(object sender, PropertyChangedEventArgs args)
				{
					if (ItemCountNumberTooltipView != null && sender is NumberBox)
					{
						ItemCountNumberTooltipView.Title = string.Format(MysticCrafting.Module.Strings.Recipe.TooltipUnlockedItem, unlockedCount, maxValue);
					}
				});
				ItemCountNumberBox.MaxValue -= unlockedCount;
			}
			else
			{
				ItemCountNumberLabel = new Label
				{
					Parent = this,
					Text = "1",
					Location = new Point(60, 12),
					Font = Font,
					StrokeText = true,
					AutoSizeWidth = true
				};
				UpdateNameLabelPosition();
			}
		}

		public virtual void BuildItemCountControls()
		{
			UpdateCollectionStatus();
			Color itemCountColor = Color.White;
			if (RequiredQuantity == 0 && TotalRequiredQuantity == 0)
			{
				if (ItemRecipeRequiredCountLabel == null)
				{
					ItemRecipeRequiredCountLabel = new Label
					{
						Parent = this,
						Text = $"{RecipeRequiredQuantity}",
						Location = new Point(70, 12),
						Font = Font,
						StrokeText = true,
						TextColor = itemCountColor,
						AutoSizeWidth = true
					};
				}
				else
				{
					ItemRecipeRequiredCountLabel.Text = $"{RecipeRequiredQuantity}";
				}
				ItemCountLabel?.Dispose();
				return;
			}
			ItemRecipeRequiredCountLabel?.Dispose();
			if (ItemCountLabel == null)
			{
				ItemCountLabel = new Label
				{
					Parent = this,
					Text = GetPlayerCountAsText(),
					Location = new Point(70, 12),
					Font = Font,
					StrokeText = true,
					TextColor = itemCountColor,
					AutoSizeWidth = true
				};
			}
			UpdateNameLabelPosition();
			ResetPrices();
		}

		private void UpdateNameLabelPosition()
		{
			if (ItemNameLabel != null)
			{
				int paddingLeft = (ItemCountLabel?.Right ?? ItemRecipeRequiredCountLabel?.Right ?? ItemCountNumberLabel?.Right ?? ItemCountNumberBox?.Right ?? Icon?.Right ?? 30) + 5;
				ItemNameLabel.Location = new Point(paddingLeft, 12);
			}
		}

		public void UpdateCollectionStatus()
		{
			if (RequiredQuantity == 0 && TotalRequiredQuantity != 0)
			{
				_enoughCollected = true;
			}
			else
			{
				_enoughCollected = false;
			}
		}

		public virtual void UpdateItemCountControls()
		{
			UpdateCollectionStatus();
			if (ItemCountLabel != null)
			{
				ItemCountLabel.Text = GetPlayerCountAsText();
			}
			UpdateNameLabelPosition();
			UpdateItemCountTooltip();
		}

		public void UpdateTotalRequiredQuantity()
		{
			int totalRequiredQuantity = TotalRequiredQuantity;
			if (ParentNode == null)
			{
				TotalRequiredQuantity = ItemCountNumberBox?.Value ?? RecipeRequiredQuantity;
			}
			else
			{
				TotalRequiredQuantity = (IsSharedItem ? 1 : (RecipeRequiredQuantity * ParentNode.RequiredQuantity));
			}
			if (totalRequiredQuantity == TotalRequiredQuantity)
			{
				return;
			}
			foreach (IngredientNode ingredientNode in IngredientNodes)
			{
				ingredientNode.UpdateTotalRequiredQuantity();
			}
			CalculateTotalPrices();
			UpdateItemCountControls();
			this.UpdateRelatedNodes();
		}

		public void UpdateProfessionRequirements()
		{
			IList<CraftingDisciplineRequirement> requirements = GetCraftingRequirements();
			if (requirements.Count != 0)
			{
				RequirementsControl?.Dispose();
				RequirementsControl = new CraftingDisciplinesControl(base.RequirementsPanel)
				{
					Size = new Point(60, 25),
					DisciplineCount = requirements.Count,
					Tooltip = new Tooltip(new CraftingDisciplinesTooltipView(requirements))
				};
			}
		}

		public virtual IList<CraftingDisciplineRequirement> GetCraftingRequirements()
		{
			return (from c in _children.OfType<RecipeSheetNode>().SelectMany((RecipeSheetNode n) => n.GetCraftingRequirements()).Concat(IngredientNodes.SelectMany((IngredientNode n) => n.GetCraftingRequirements()))
				group c by c.DisciplineName into c
				select c.First()).ToList();
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			IngredientNode node = e.ChangedChild as IngredientNode;
			if (node != null)
			{
				if (TreeView.IngredientNodes == null)
				{
					return;
				}
				TreeView.IngredientNodes.Add(node);
				node.UpdateRelatedNodes();
			}
			base.OnChildAdded(e);
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			IngredientNode node = e.ChangedChild as IngredientNode;
			if (node != null)
			{
				TreeView.RemoveNode(node);
			}
			base.OnChildRemoved(e);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (_enoughCollected)
			{
				spriteBatch.DrawFrame(this, new Rectangle(base.PanelRectangle.Left, base.PanelRectangle.Top, 3, base.PanelRectangle.Height), ColorHelper.BrightGreen, 2);
			}
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		protected override void DisposeControl()
		{
			Icon?.Dispose();
			IconChain?.Dispose();
			IconSwap?.Dispose();
			ItemNameLabel?.Dispose();
			LoadingSpinner?.Dispose();
			ItemCountLabel?.Dispose();
			ItemRecipeRequiredCountLabel?.Dispose();
			MenuStrip?.Dispose();
			RequirementsControl?.Dispose();
			ItemCountTooltipView?.DoUnload();
			ItemCountTooltip?.Dispose();
			base.DisposeControl();
		}
	}
}
