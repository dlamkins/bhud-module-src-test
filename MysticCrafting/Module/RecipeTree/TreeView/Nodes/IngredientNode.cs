using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Presenters;
using MysticCrafting.Module.RecipeTree.TreeView.Tooltips;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
{
	[DebuggerDisplay("Name = {NameLabel?.Text}")]
	public abstract class IngredientNode : TradeableItemNode, IIngredient, IIngredientContainer
	{
		private bool _showIcon = true;

		private bool _isLinked;

		private int _unitCount = 1;

		private int _vendorPriceUnitCount = 1;

		private int _reservedUnitCount;

		private int _playerUnitCount;

		private bool _enoughCollected;

		private readonly IRequirementsPresenter _requirementsPresenter = new RequirementsPresenter();

		private Image _missingTabs;

		public int Id { get; }

		public int? IngredientIndex { get; set; }

		public override bool Active
		{
			get
			{
				return base.Active;
			}
			set
			{
				if (base.Active != value)
				{
					base.Active = value;
					this.UpdateRelatedNodes();
				}
			}
		}

		public TreeNodeBase ParentNode { get; set; }

		public IngredientNode ParentIngredientNode { get; set; }

		public VendorNode ParentVendorNode { get; set; }

		public BitmapFont Font { get; set; } = GameService.Content.get_DefaultFont16();


		protected Image IconBackground { get; set; }

		public Image Icon { get; protected set; }

		public AsyncTexture2D IconTexture
		{
			get
			{
				Image icon = Icon;
				if (icon == null)
				{
					return null;
				}
				return icon.get_Texture();
			}
			set
			{
				if (Icon != null)
				{
					Icon.set_Texture(value);
				}
			}
		}

		protected Image IconChain { get; set; }

		protected Image IconSwap { get; set; }

		protected int IconSize { get; set; } = 30;


		protected Label UnitCountLabel { get; set; }

		protected CoinsControl UnitCountCoinsControl { get; set; }

		protected Tooltip PlayerCountTooltip { get; set; }

		public ICountTooltipView PlayerCountTooltipView { get; set; }

		protected Label NameLabel { get; set; }

		public Color NameLabelColor
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				Label nameLabel = NameLabel;
				if (nameLabel == null)
				{
					return Color.get_White();
				}
				return nameLabel.get_TextColor();
			}
			set
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				if (NameLabel != null)
				{
					NameLabel.set_TextColor(value);
				}
			}
		}

		public string Name
		{
			get
			{
				Label nameLabel = NameLabel;
				return ((nameLabel != null) ? nameLabel.get_Text() : null) ?? string.Empty;
			}
			set
			{
				if (NameLabel != null)
				{
					NameLabel.set_Text(value);
				}
			}
		}

		private LoadingSpinner LoadingSpinner { get; set; }

		protected NumberBox UnitCountNumberBox { get; set; }

		protected NumberBoxTooltipView UnitCountNumberTooltipView { get; set; }

		private Label UnitCountNumberLabel { get; set; }

		public int? UnitCountNumber
		{
			get
			{
				return UnitCountNumberBox?.Value;
			}
			set
			{
				if (UnitCountNumberBox != null)
				{
					UnitCountNumberBox.Value = value.GetValueOrDefault();
				}
			}
		}

		public int MaxEditValue
		{
			get
			{
				return UnitCountNumberBox?.MaxValue ?? 0;
			}
			set
			{
				if (UnitCountNumberBox != null)
				{
					UnitCountNumberBox.MaxValue = value;
					ToggleUnitNumberBoxControls();
				}
			}
		}

		public IEnumerable<IngredientNode> ChildIngredientNodes => ((IEnumerable)((Container)this)._children).OfType<IngredientNode>();

		public IItemSource SelectedItemSource => ((IEnumerable)((Container)this).get_Children())?.OfType<ItemTab>()?.FirstOrDefault((ItemTab t) => t.Active)?.ItemSource;

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

		public bool IsTopIngredient { get; private set; }

		public virtual int UnitCount
		{
			get
			{
				return _unitCount;
			}
			set
			{
				_unitCount = value;
				CalculateTotalUnitCount();
			}
		}

		public virtual int TotalUnitCount { get; set; }

		public override int VendorPriceUnitCount
		{
			get
			{
				return _vendorPriceUnitCount;
			}
			set
			{
				if (_vendorPriceUnitCount != value)
				{
					_vendorPriceUnitCount = value;
					CalculateTotalUnitCount();
				}
			}
		}

		public int ReservedUnitCount
		{
			get
			{
				return _reservedUnitCount;
			}
			set
			{
				_reservedUnitCount = value;
				CalculateTotalPlayerCount();
			}
		}

		public int? ReservedGroup
		{
			get
			{
				VendorNode vendorNode = ((Control)this).get_Parent() as VendorNode;
				if (vendorNode != null)
				{
					TreeNodeBase node = ((Control)vendorNode).get_Parent() as TreeNodeBase;
					if (node != null)
					{
						return node.NodeIndex;
					}
				}
				return (((Control)this).get_Parent() as IngredientNode)?.ReservedGroup;
			}
		}

		public int PlayerUnitCount
		{
			get
			{
				return _playerUnitCount;
			}
			set
			{
				if (_playerUnitCount != value)
				{
					_playerUnitCount = value;
					CalculateTotalPlayerCount();
				}
			}
		}

		public int TotalPlayerUnitCount { get; set; }

		public override string PathName => $"{IngredientIndex}-{Id}";

		private CraftingDisciplinesControl RequirementsControl { get; set; }

		protected IngredientNode(int id, int quantity, Container parent, int? index = null, bool showUnitCount = true, bool loadingChildren = false)
			: base(parent)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			SetParentProperties(parent);
			((Control)this).set_EffectBehind((ControlEffect)new ScrollingHighlightEffect((Control)(object)this));
			base.LoadingChildren = loadingChildren;
			Id = id;
			UnitCount = quantity;
			IngredientIndex = index;
			BuildIcon();
			BuildNameLabel();
			if (showUnitCount)
			{
				if (IsTopIngredient)
				{
					BuildUnitNumberBox();
				}
				else
				{
					BuildUnitCountLabels();
				}
			}
		}

		public void CalculateTotalUnitCount()
		{
			_requirementsPresenter.CalculateTotalUnitCount(this);
		}

		public void CalculateTotalPlayerCount()
		{
			_requirementsPresenter.CalculateTotalPlayerCount(this);
		}

		public void CalculateOrderUnitCount(bool updateChildren = false)
		{
			_requirementsPresenter.CalculateOrderUnitCount(this, updateChildren);
		}

		private void SetParentProperties(Container parent)
		{
			TreeNodeBase node = parent as TreeNodeBase;
			if (node != null)
			{
				ParentNode = node;
			}
			IngredientNode parentNode = parent as IngredientNode;
			if (parentNode != null)
			{
				ParentIngredientNode = parentNode;
				base.TreeView = parentNode.TreeView;
				return;
			}
			VendorNode vendorNode = parent as VendorNode;
			if (vendorNode != null)
			{
				ParentVendorNode = vendorNode;
				base.TreeView = vendorNode.TreeView;
				VendorPriceUnitCount = vendorNode.VendorPriceUnitCount;
				return;
			}
			TreeView treeView = parent as TreeView;
			if (treeView != null)
			{
				IsTopIngredient = true;
				_showIcon = false;
				TotalUnitCount = 1;
				base.TreeView = treeView;
				Font = GameService.Content.get_DefaultFont18();
			}
		}

		public virtual void Build(Container parent)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			BuildMenuStrip();
			if (!base.LoadingChildren)
			{
				OnChildrenLoaded();
			}
			else
			{
				LoadingSpinner val = new LoadingSpinner();
				((Control)val).set_Parent((Container)(object)this);
				((Control)val).set_Size(new Point(40, 40));
				((Control)val).set_Location(new Point(PriceLocation.X, 3));
				LoadingSpinner = val;
			}
			((Control)this).set_Parent(parent);
			UpdateItemCountControls();
		}

		public void BuildIcon()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Expected O, but got Unknown
			Image icon = Icon;
			if (icon != null)
			{
				((Control)icon).Dispose();
			}
			Image val = new Image(ServiceContainer.TextureRepository.Textures.ItemBackground);
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(new Point(IconSize + 8, IconSize + 8));
			((Control)val).set_Visible(_showIcon);
			((Control)val).set_Location(new Point(IsTopIngredient ? 5 : 28, 1));
			IconBackground = val;
			Image val2 = new Image(ServiceContainer.TextureRepository.GetRefTexture("102804_trimmed.png"));
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Size(new Point(IconSize, IconSize));
			((Control)val2).set_Visible(_showIcon);
			((Control)val2).set_Location(new Point(IsTopIngredient ? 9 : 32, 5));
			Icon = val2;
		}

		private void BuildNameLabel()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			Label nameLabel = NameLabel;
			if (nameLabel != null)
			{
				((Control)nameLabel).Dispose();
			}
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(Name);
			((Control)val).set_Location(new Point(0, 10));
			((Control)val).set_Width(280);
			val.set_Font(Font);
			val.set_TextColor(NameLabelColor);
			val.set_StrokeText(true);
			val.set_AutoSizeHeight(true);
			NameLabel = val;
			UpdateNameLabelPosition();
		}

		public void ToggleUnitNumberBoxControls()
		{
			if (MaxEditValue <= 1)
			{
				((Control)UnitCountNumberLabel).set_Visible(true);
				((Control)UnitCountNumberBox).set_Visible(false);
			}
			else
			{
				((Control)UnitCountNumberLabel).set_Visible(false);
				((Control)UnitCountNumberBox).set_Visible(true);
			}
		}

		public void BuildUnitNumberBox()
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Expected O, but got Unknown
			NumberBox unitCountNumberBox = UnitCountNumberBox;
			if (unitCountNumberBox != null)
			{
				((Control)unitCountNumberBox).Dispose();
			}
			Label unitCountNumberLabel = UnitCountNumberLabel;
			if (unitCountNumberLabel != null)
			{
				((Control)unitCountNumberLabel).Dispose();
			}
			Image icon = Icon;
			int xLocation = ((icon != null && ((Control)icon).get_Visible()) ? (((Control)Icon).get_Right() + 5) : 10);
			NumberBox obj = new NumberBox
			{
				Value = TotalUnitCount,
				MinValue = 1
			};
			((TextInputBase)obj).set_Font(GameService.Content.get_DefaultFont18());
			((Control)obj).set_Parent((Container)(object)this);
			((Control)obj).set_Location(new Point(xLocation, 7));
			((Control)obj).set_Size(new Point(50, 27));
			UnitCountNumberBox = obj;
			UnitCountNumberBox.AfterTextChanged += delegate(object sender, EventArgs args)
			{
				NumberBox numberBox = sender as NumberBox;
				if (numberBox != null)
				{
					TotalUnitCount = numberBox.Value;
				}
				CalculateOrderUnitCount(updateChildren: true);
			};
			UnitCountNumberTooltipView = new NumberBoxTooltipView
			{
				Text = Recipe.ItemCountBoxTooltipText
			};
			((Control)UnitCountNumberBox).set_Tooltip((Tooltip)(object)new DisposableTooltip((ITooltipView)(object)UnitCountNumberTooltipView));
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("1");
			((Control)val).set_Location(new Point(xLocation + 15, 10));
			val.set_Font(Font);
			val.set_StrokeText(true);
			val.set_AutoSizeWidth(true);
			((Control)val).set_Visible(false);
			UnitCountNumberLabel = val;
			UpdateNameLabelPosition();
		}

		public string GetUnitCountText()
		{
			if (TotalUnitCount == 0)
			{
				return $"{UnitCount}";
			}
			return $"{GetPlayerUnitCount()}/{TotalUnitCount}";
		}

		public virtual void BuildUnitCountLabels()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			UpdateCollectionStatus();
			Color itemCountColor = Color.get_White();
			Image icon = Icon;
			int xLocation = ((icon != null && ((Control)icon).get_Visible()) ? (((Control)Icon).get_Right() + 5) : 10);
			Label unitCountLabel = UnitCountLabel;
			if (unitCountLabel != null)
			{
				((Control)unitCountLabel).Dispose();
			}
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(GetUnitCountText());
			((Control)val).set_Location(new Point(xLocation, 10));
			val.set_Font(Font);
			val.set_StrokeText(true);
			val.set_TextColor(itemCountColor);
			val.set_AutoSizeWidth(true);
			UnitCountLabel = val;
			UpdateNameLabelPosition();
			ResetPrices();
		}

		public void ClearChildIngredientNodes()
		{
			IEnumerable<IngredientNode> childNodes = base.ChildNodes.OfType<IngredientNode>();
			while (childNodes.Count() > 0)
			{
				((Control)childNodes.FirstOrDefault()).set_Parent((Container)null);
			}
		}

		public void OnChildrenLoaded()
		{
			LoadingSpinner loadingSpinner = LoadingSpinner;
			if (loadingSpinner != null)
			{
				((Control)loadingSpinner).Dispose();
			}
			UpdatePriceControls();
		}

		private void UpdateLinkedItemControls()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			if (IconChain == null)
			{
				AsyncTexture2D iconTexture = (IsSharedItem ? ServiceContainer.TextureRepository.Textures.ChainGold : ServiceContainer.TextureRepository.Textures.Chain);
				Image val = new Image(iconTexture);
				((Control)val).set_Parent((Container)(object)this);
				((Control)val).set_Size(new Point(IconSize + 7, IconSize + 7));
				((Control)val).set_Location(new Point(32, 5));
				((Control)val).set_Visible(true);
				IconChain = val;
				if (PlayerCountTooltip != null)
				{
					((Control)IconChain).set_Tooltip(PlayerCountTooltip);
				}
			}
			else if (IsLinked)
			{
				((Control)IconChain).Show();
			}
			else
			{
				Image iconChain = IconChain;
				if (iconChain != null)
				{
					((Control)iconChain).Hide();
				}
			}
			if (IsLinked)
			{
				PlayerCountTooltipView?.UpdateLinkedNodes();
			}
		}

		protected abstract void BuildMenuStrip();

		public void BuildMissingTabsLabel()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			Image missingTabs = _missingTabs;
			if (missingTabs != null)
			{
				((Control)missingTabs).Dispose();
			}
			Image val = new Image(ServiceContainer.TextureRepository.Textures.IgnoreIcon);
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(((Control)this).get_Width() - 35, 5));
			((Control)val).set_Size(new Point(30, 30));
			val.set_Tint(Color.get_LightYellow());
			((Control)val).set_BasicTooltipText(Recipe.ItemSourceWarning);
			_missingTabs = val;
		}

		public void UpdateItemCountTooltip()
		{
			if (PlayerCountTooltipView != null)
			{
				PlayerCountTooltipView.RequiredQuantity = OrderUnitCount;
			}
		}

		public int GetPlayerUnitCount()
		{
			if (!IsLinked || TotalPlayerUnitCount <= TotalUnitCount)
			{
				return TotalPlayerUnitCount;
			}
			return TotalUnitCount;
		}

		public virtual void UpdateItemCountControls()
		{
			UpdateCollectionStatus();
			if (UnitCountLabel != null)
			{
				UnitCountLabel.set_Text(GetUnitCountText());
			}
			UpdateNameLabelPosition();
			UpdateItemCountTooltip();
		}

		private void UpdateNameLabelPosition()
		{
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			if (NameLabel == null)
			{
				return;
			}
			int paddingLeft = 0;
			Label unitCountLabel = UnitCountLabel;
			if (unitCountLabel != null && ((Control)unitCountLabel).get_Visible())
			{
				paddingLeft = ((Control)UnitCountLabel).get_Right();
			}
			else
			{
				Label unitCountNumberLabel = UnitCountNumberLabel;
				if (unitCountNumberLabel != null && ((Control)unitCountNumberLabel).get_Visible())
				{
					paddingLeft = ((Control)UnitCountNumberLabel).get_Right();
				}
				else
				{
					CoinsControl unitCountCoinsControl = UnitCountCoinsControl;
					if (unitCountCoinsControl != null && ((Control)unitCountCoinsControl).get_Visible())
					{
						paddingLeft = ((Control)UnitCountCoinsControl).get_Right();
					}
					else
					{
						NumberBox unitCountNumberBox = UnitCountNumberBox;
						if (unitCountNumberBox != null && ((Control)unitCountNumberBox).get_Visible())
						{
							paddingLeft = ((Control)UnitCountNumberBox).get_Right();
						}
						else
						{
							Image icon = Icon;
							paddingLeft = ((icon == null || !((Control)icon).get_Visible()) ? 30 : (((Control)Icon).get_Right() + 5));
						}
					}
				}
			}
			((Control)NameLabel).set_Location(new Point(paddingLeft + 5, 10));
		}

		public void UpdateCollectionStatus()
		{
			if (OrderUnitCount == 0 && TotalUnitCount != 0)
			{
				_enoughCollected = true;
			}
			else
			{
				_enoughCollected = false;
			}
		}

		public abstract bool UpdatePlayerUnitCount();

		public void UpdateProfessionRequirements()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			IList<CraftingDisciplineRequirement> requirements = GetCraftingRequirements();
			if (requirements.Count != 0)
			{
				CraftingDisciplinesControl requirementsControl = RequirementsControl;
				if (requirementsControl != null)
				{
					((Control)requirementsControl).Dispose();
				}
				CraftingDisciplinesControl craftingDisciplinesControl = new CraftingDisciplinesControl((Container)(object)base.RequirementsPanel);
				((Control)craftingDisciplinesControl).set_Size(new Point(60, 25));
				craftingDisciplinesControl.DisciplineCount = requirements.Count;
				((Control)craftingDisciplinesControl).set_Tooltip(new Tooltip((ITooltipView)(object)new CraftingDisciplinesTooltipView(requirements)));
				RequirementsControl = craftingDisciplinesControl;
			}
		}

		public virtual IList<CraftingDisciplineRequirement> GetCraftingRequirements()
		{
			return (from c in ((IEnumerable)((Container)this)._children).OfType<RecipeSheetNode>().SelectMany((RecipeSheetNode n) => n.GetCraftingRequirements()).Concat(ChildIngredientNodes.SelectMany((IngredientNode n) => n.GetCraftingRequirements()))
				group c by c.DisciplineName into c
				select c.First()).ToList();
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			base.OnChildAdded(e);
			IngredientNode node = e.get_ChangedChild() as IngredientNode;
			if (node != null && base.TreeView.IngredientNodes != null && node.Id != 1)
			{
				base.TreeView.IngredientNodes.Add(node);
				node.UpdateRelatedNodes();
			}
		}

		protected override void OnChildRemoved(ChildChangedEventArgs e)
		{
			if (base.TreeView != null)
			{
				IngredientNode node = e.get_ChangedChild() as IngredientNode;
				if (node != null)
				{
					base.TreeView.RemoveNode(node);
				}
			}
			if (base.TreeView != null)
			{
				VendorNode vendorNode = e.get_ChangedChild() as VendorNode;
				if (vendorNode != null)
				{
					foreach (IngredientNode child in vendorNode.ChildNodes.OfType<IngredientNode>())
					{
						base.TreeView.RemoveNode(child);
					}
				}
			}
			((Container)this).OnChildRemoved(e);
			e.get_ChangedChild().Dispose();
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			if (_enoughCollected)
			{
				Rectangle panelRectangle = base.PanelRectangle;
				int left = ((Rectangle)(ref panelRectangle)).get_Left();
				panelRectangle = base.PanelRectangle;
				spriteBatch.DrawFrame((Control)(object)this, new Rectangle(left, ((Rectangle)(ref panelRectangle)).get_Top(), 3, base.PanelRectangle.Height), ColorHelper.BrightGreen, 2);
			}
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		protected override void DisposeControl()
		{
			Image icon = Icon;
			if (icon != null)
			{
				((Control)icon).Dispose();
			}
			Image iconBackground = IconBackground;
			if (iconBackground != null)
			{
				((Control)iconBackground).Dispose();
			}
			Image iconChain = IconChain;
			if (iconChain != null)
			{
				((Control)iconChain).Dispose();
			}
			Image iconSwap = IconSwap;
			if (iconSwap != null)
			{
				((Control)iconSwap).Dispose();
			}
			Label nameLabel = NameLabel;
			if (nameLabel != null)
			{
				((Control)nameLabel).Dispose();
			}
			LoadingSpinner loadingSpinner = LoadingSpinner;
			if (loadingSpinner != null)
			{
				((Control)loadingSpinner).Dispose();
			}
			NumberBox unitCountNumberBox = UnitCountNumberBox;
			if (unitCountNumberBox != null)
			{
				((Control)unitCountNumberBox).Dispose();
			}
			Label unitCountLabel = UnitCountLabel;
			if (unitCountLabel != null)
			{
				((Control)unitCountLabel).Dispose();
			}
			UnitCountNumberTooltipView = null;
			CraftingDisciplinesControl requirementsControl = RequirementsControl;
			if (requirementsControl != null)
			{
				((Control)requirementsControl).Dispose();
			}
			ICountTooltipView playerCountTooltipView = PlayerCountTooltipView;
			if (playerCountTooltipView != null)
			{
				((IView)playerCountTooltipView).DoUnload();
			}
			PlayerCountTooltipView = null;
			Tooltip playerCountTooltip = PlayerCountTooltip;
			if (playerCountTooltip != null)
			{
				((Control)playerCountTooltip).Dispose();
			}
			((Control)this).set_EffectBehind((ControlEffect)null);
			base.DisposeControl();
		}
	}
}
