using System;
using System.Collections.Generic;
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

namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	[DebuggerDisplay("Name = {Item.Name}")]
	public class IngredientNode : TradeableItemNode, IIngredient, IIngredientContainer
	{
		private int _nodeIndex;

		private bool _isLinked;

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

		public TreeView TreeView { get; }

		public MysticItem Item { get; set; }

		public MysticIngredient Ingredient { get; }

		public IngredientNode ParentNode { get; set; }

		public Image Icon { get; set; }

		public Image IconChain { get; set; }

		public Image IconSwap { get; set; }

		private Tooltip ItemCountTooltip { get; set; }

		public int IconSize { get; set; } = 35;


		public BitmapFont Font { get; set; } = GameService.Content.DefaultFont16;


		private Label ItemNameLabel { get; set; }

		private LoadingSpinner LoadingSpinner { get; set; }

		private Label ItemCountLabel { get; set; }

		private Label ItemRecipeRequiredCountLabel { get; set; }

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

		public bool IsTopItem { get; }

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

		public override string PathName => $"{IngredientIndex}-{Item.Id}";

		public ItemNodeTooltipView TooltipView { get; set; }

		public IngredientNode(MysticItem item, int? quantity = null, int? index = null)
		{
			base.EffectBehind = new ScrollingHighlightEffect(this);
			RecipeRequiredQuantity = quantity ?? 1;
			Item = item;
			IngredientIndex = index;
			Ingredient = new MysticIngredient
			{
				GameId = item.Id,
				Quantity = quantity,
				Index = index,
				Name = item.Name,
				Item = item
			};
			UpdateItemDetails();
		}

		public IngredientNode(MysticIngredient ingredient, MysticItem item, Container parent, bool loadingChildren = false)
		{
			base.EffectBehind = new ScrollingHighlightEffect(this);
			LoadingChildren = loadingChildren;
			RecipeRequiredQuantity = ingredient.Quantity ?? 1;
			Item = item;
			IngredientIndex = ingredient.Index;
			Ingredient = ingredient;
			PlayerItemCount = ServiceContainer.PlayerItemService.GetItemCount(item.Id);
			IngredientNode parentNode = parent as IngredientNode;
			if (parentNode != null)
			{
				ParentNode = parentNode;
				TreeView = parentNode.TreeView;
			}
			else
			{
				TreeView treeview = parent as TreeView;
				if (treeview != null)
				{
					IsTopItem = true;
					TreeView = treeview;
					Font = GameService.Content.DefaultFont18;
					BackgroundOpaqueColor = ColorHelper.FromRarity(item.Rarity) * 0.2f;
					FrameColor = ColorHelper.FromRarity(item.Rarity) * 0.8f;
				}
			}
			UpdateTotalRequiredQuantity();
			UpdateItemDetails();
			if (!IsTopItem)
			{
				BuildItemCountControls();
			}
			UpdateItemTooltip();
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
				Icon = new Image(ServiceContainer.TextureRepository.GetTexture(Item.Icon))
				{
					Parent = this,
					Size = new Point(IconSize, IconSize),
					Location = new Point(IsTopItem ? 12 : 30, 5)
				};
			}
			int paddingLeft = (ItemCountLabel?.Right ?? ItemRecipeRequiredCountLabel?.Right ?? Icon?.Right ?? 30) + 5;
			ItemNameLabel = new Label
			{
				Parent = this,
				Text = (Item.Name ?? "").Truncate(27),
				Location = new Point(paddingLeft, 12),
				Width = 200,
				Font = Font,
				TextColor = ColorHelper.FromRarity(Item.Rarity),
				StrokeText = true,
				AutoSizeHeight = true
			};
			UpdateMenuStrip();
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
					Size = new Point(IconSize + 20, IconSize + 20),
					Location = new Point(30, 5),
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
				TooltipView?.UpdateLinkedNodes();
			}
		}

		private void UpdateMenuStrip()
		{
			ContextMenuPresenter menuStripPresenter = new ContextMenuPresenter(ServiceContainer.WikiLinkRepository);
			MenuStrip = menuStripPresenter.BuildMenuStrip(Item);
		}

		public void UpdateItemTooltip()
		{
			TooltipView = new ItemNodeTooltipView(this);
			ItemCountTooltip = new DisposableTooltip(TooltipView);
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

		public virtual void BuildItemCountControls()
		{
			UpdateBackgroundColor();
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
			if (ItemNameLabel != null)
			{
				int paddingLeft = (ItemCountLabel?.Right ?? ItemRecipeRequiredCountLabel?.Right ?? Icon?.Right ?? 30) + 5;
				ItemNameLabel.Location = new Point(paddingLeft, 12);
			}
			ResetPrices();
		}

		public void UpdateBackgroundColor()
		{
			if (RequiredQuantity == 0 && TotalRequiredQuantity != 0)
			{
				FrameColor = Color.DarkGreen;
				BackgroundOpaqueColor = Color.DarkGreen;
			}
			else
			{
				FrameColor = Color.Black;
				BackgroundOpaqueColor = Color.Black;
			}
		}

		public virtual void UpdateItemCountControls()
		{
			UpdateBackgroundColor();
			UpdateTotalRequiredQuantity();
			if (ItemCountLabel != null)
			{
				ItemCountLabel.Text = GetPlayerCountAsText();
			}
			if (ItemNameLabel != null)
			{
				int paddingLeft = (ItemCountLabel?.Right ?? ItemRecipeRequiredCountLabel?.Right ?? Icon?.Right ?? 30) + 5;
				ItemNameLabel.Location = new Point(paddingLeft, 12);
			}
		}

		public void UpdateTotalRequiredQuantity()
		{
			if (ParentNode == null)
			{
				TotalRequiredQuantity = RecipeRequiredQuantity;
			}
			else
			{
				TotalRequiredQuantity = RecipeRequiredQuantity * ParentNode.RequiredQuantity;
			}
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			IngredientNode node = e.ChangedChild as IngredientNode;
			if (node != null)
			{
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

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (Icon != null)
			{
				Color lineColor = Color.DarkGray * 0.7f;
				spriteBatch.DrawFrame(this, Icon.LocalBounds, lineColor);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
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
			TooltipView?.DoUnload();
			ItemCountTooltip?.Dispose();
			base.DisposeControl();
		}
	}
}
