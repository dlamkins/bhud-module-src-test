using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Currencies;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.RecipeTree.TreeView.Presenters;
using MysticCrafting.Module.RecipeTree.TreeView.Tooltips;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
{
	public abstract class TradeableItemNode : TreeNodeBase, ITradeableItemNode
	{
		private bool _active = true;

		private long _tradingPostPrice;

		private IList<CurrencyQuantity> _vendorPurchasePrice = new List<CurrencyQuantity>();

		private IList<CurrencyQuantity> _totalPrice = new List<CurrencyQuantity>();

		private CurrencyQuantity _totalCoinPrice = new CurrencyQuantity();

		private readonly ITradeableNodePresenter _tradeableNodePresenter = new TradeableNodePresenter(ServiceContainer.CurrencyRepository);

		private int _orderUnitCount;

		public virtual bool Active
		{
			get
			{
				return _active;
			}
			set
			{
				if (_active == value)
				{
					return;
				}
				_active = value;
				foreach (ITradeableItemNode item in ((IEnumerable)((Container)this).get_Children()).OfType<ITradeableItemNode>())
				{
					item.Active = value;
				}
			}
		}

		protected bool HidePriceControls { get; set; }

		protected float CoinControlOpacity { get; set; } = 1f;


		public long TradingPostPrice
		{
			get
			{
				return _tradingPostPrice;
			}
			set
			{
				_tradingPostPrice = value;
				CalculateTotalPrices();
			}
		}

		public virtual int VendorPriceUnitCount { get; set; } = 1;


		public IList<CurrencyQuantity> VendorPrice
		{
			get
			{
				return _vendorPurchasePrice;
			}
			set
			{
				_vendorPurchasePrice = value;
				CalculateTotalPrices();
				UpdatePriceControls();
			}
		}

		public IList<CurrencyQuantity> TotalVendorPrice { get; set; } = new List<CurrencyQuantity>();


		public IList<CurrencyQuantity> GrandTotalPrice
		{
			get
			{
				return _totalPrice;
			}
			set
			{
				_totalPrice = value?.CombineQuantities().ToList();
				UpdatePriceControls();
			}
		}

		public CurrencyQuantity TotalCoinPrice
		{
			get
			{
				return _totalCoinPrice;
			}
			set
			{
				if (value == null)
				{
					_totalCoinPrice = new CurrencyQuantity();
					return;
				}
				_totalCoinPrice = value;
				if (!base.LoadingChildren)
				{
					UpdatePriceControls();
				}
			}
		}

		public virtual int OrderUnitCount
		{
			get
			{
				return _orderUnitCount;
			}
			set
			{
				if (_orderUnitCount != value)
				{
					_orderUnitCount = value;
					CalculateTotalPrices();
				}
			}
		}

		protected CurrenciesContainer CurrencyContainer { get; set; }

		protected CoinsControl CoinsControl { get; set; }

		private VendorCurrencyControl VendorCurrencyControl { get; set; }

		public FlowPanel RequirementsPanel { get; set; }

		private Tooltip VendorCurrencyTooltip { get; set; }

		public bool HasTradeableChildren => base.ChildNodes.OfType<ITradeableItemNode>().Any();

		public virtual Point PriceLocation { get; set; } = new Point(380, 10);


		public void CalculateTotalPrices()
		{
			_tradeableNodePresenter.CalculateTotalPrice(this);
		}

		protected TradeableItemNode(Container parent)
			: base(parent)
		{
		}//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)


		public void ResetPrices()
		{
			_tradingPostPrice = 0L;
			_vendorPurchasePrice = new List<CurrencyQuantity>();
			if (TotalCoinPrice != null)
			{
				TotalCoinPrice.Count = 0L;
			}
			TotalVendorPrice = new List<CurrencyQuantity>();
			_vendorPurchasePrice = new List<CurrencyQuantity>();
			_totalPrice = new List<CurrencyQuantity>();
			UpdatePriceControls();
			_tradeableNodePresenter.RecalculateParents(this);
		}

		public virtual void BuildPriceControls()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			CoinsControl obj = new CoinsControl((Container)(object)this)
			{
				UnitPrice = TotalCoinPrice.Count
			};
			((Control)obj).set_Location(PriceLocation);
			((Control)obj).set_Opacity(CoinControlOpacity);
			CoinsControl = obj;
			BuildVendorPriceControls();
		}

		private Point GetRequirementsLocation()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			return new Point((_totalCoinPrice.Count != 0L) ? (PriceLocation.X + 150) : (PriceLocation.X - 5), PriceLocation.Y);
		}

		public void UpdateRequirementsLocation()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (RequirementsPanel != null)
			{
				((Control)RequirementsPanel).set_Location(GetRequirementsLocation());
			}
		}

		private void BuildVendorPriceControls()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			if (RequirementsPanel == null)
			{
				FlowPanel val = new FlowPanel();
				val.set_FlowDirection((ControlFlowDirection)0);
				((Control)val).set_Size(new Point(120, 25));
				((Control)val).set_Location(GetRequirementsLocation());
				val.set_ControlPadding(new Vector2(10f, 0f));
				((Control)val).set_Parent((Container)(object)this);
				((Control)val).set_Opacity(CoinControlOpacity);
				((Panel)val).set_CanScroll(false);
				RequirementsPanel = val;
			}
			VendorCurrencyControl obj = new VendorCurrencyControl((Container)(object)RequirementsPanel)
			{
				Price = TotalVendorPrice
			};
			((Control)obj).set_Size(new Point(30, 25));
			((Control)obj).set_Visible(false);
			VendorCurrencyControl = obj;
			VendorCurrencyTooltip = (Tooltip)(object)new DisposableTooltip((ITooltipView)(object)new CurrenciesTooltipView(TotalVendorPrice));
			((Control)VendorCurrencyControl).set_Tooltip(VendorCurrencyTooltip);
			CurrenciesContainer obj2 = new CurrenciesContainer((Container)(object)RequirementsPanel, this)
			{
				Price = TotalVendorPrice
			};
			((Control)obj2).set_Visible(false);
			CurrencyContainer = obj2;
			SwitchVendorPriceControls();
		}

		public void ExpandAllActiveNodes(bool includeIncomplete = true)
		{
			if (!Active || (OrderUnitCount == 0 && !includeIncomplete))
			{
				return;
			}
			if (!base.Expanded)
			{
				Toggle();
			}
			foreach (TradeableItemNode item in ((IEnumerable)((Container)this).get_Children()).OfType<TradeableItemNode>())
			{
				item.ExpandAllActiveNodes(includeIncomplete);
			}
		}

		public void CollapseAllActiveNodes(bool isComplete = true)
		{
			if (OrderUnitCount == 0 && isComplete)
			{
				return;
			}
			if (base.Expanded)
			{
				Toggle();
			}
			foreach (TradeableItemNode item in ((IEnumerable)((Container)this).get_Children()).OfType<TradeableItemNode>())
			{
				item.ExpandAllActiveNodes(isComplete);
			}
		}

		private bool ShowMinifiedVendorCurrencies()
		{
			if (HasTradeableChildren || TotalVendorPrice.ExcludingCoins().Count() > 2)
			{
				if (CoinsControl.UnitPrice == 0L)
				{
					return TotalVendorPrice.ExcludingCoins().Count() > 2;
				}
				return true;
			}
			return false;
		}

		private void SwitchVendorPriceControls()
		{
			if (ShowMinifiedVendorCurrencies())
			{
				((Control)VendorCurrencyControl).Show();
				((Control)CurrencyContainer).Hide();
			}
			else
			{
				((Control)VendorCurrencyControl).Hide();
				((Control)CurrencyContainer).Show();
			}
		}

		public virtual void UpdatePriceControls()
		{
			if (base.LoadingChildren || HidePriceControls)
			{
				return;
			}
			if (CoinsControl == null)
			{
				BuildPriceControls();
			}
			if (TotalCoinPrice.Count >= 0)
			{
				if (((Control)CoinsControl).get_Parent() == null)
				{
					((Control)CoinsControl).set_Parent((Container)(object)this);
				}
				CoinsControl.UnitPrice = TotalCoinPrice.Count;
			}
			if (TotalVendorPrice.Any((CurrencyQuantity p) => p.Count > 0))
			{
				if (ShowMinifiedVendorCurrencies())
				{
					VendorCurrencyControl.Price = TotalVendorPrice;
					Tooltip vendorCurrencyTooltip = VendorCurrencyTooltip;
					if (vendorCurrencyTooltip != null)
					{
						((Control)vendorCurrencyTooltip).Dispose();
					}
					VendorCurrencyTooltip = (Tooltip)(object)new DisposableTooltip((ITooltipView)(object)new CurrenciesTooltipView(this));
					((Control)VendorCurrencyControl).set_Tooltip(VendorCurrencyTooltip);
				}
				else
				{
					CurrencyContainer.Price = TotalVendorPrice;
					((Control)RequirementsPanel).set_Width(CurrencyContainer.CalculateWidth());
				}
				SwitchVendorPriceControls();
			}
			else
			{
				if (CurrencyContainer != null)
				{
					CurrencyContainer.Price = TotalVendorPrice;
				}
				if (VendorCurrencyControl != null)
				{
					VendorCurrencyControl.Price = TotalVendorPrice;
				}
			}
			UpdateRequirementsLocation();
		}

		public void UpdateReservedQuantityTooltip(int nodeId = 0)
		{
			if (nodeId == 0)
			{
				nodeId = base.NodeIndex;
			}
			if (CurrencyContainer != null)
			{
				CurrencyContainer.UpdateReservedQuantity(nodeId);
			}
			(((Control)this).get_Parent() as TradeableItemNode)?.UpdateReservedQuantityTooltip(nodeId);
		}

		protected override void DisposeControl()
		{
			Tooltip vendorCurrencyTooltip = VendorCurrencyTooltip;
			if (vendorCurrencyTooltip != null)
			{
				((Control)vendorCurrencyTooltip).Dispose();
			}
			VendorCurrencyControl vendorCurrencyControl = VendorCurrencyControl;
			if (vendorCurrencyControl != null)
			{
				((Control)vendorCurrencyControl).Dispose();
			}
			CoinsControl coinsControl = CoinsControl;
			if (coinsControl != null)
			{
				((Control)coinsControl).Dispose();
			}
			base.DisposeControl();
		}
	}
}
