using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MysticCrafting.Models.Commerce;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Recipe.TreeView.Presenters;
using MysticCrafting.Module.Recipe.TreeView.Tooltips;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	public abstract class TradeableItemNode : TreeNodeBase, ITradeableItemNode
	{
		private int _tradingPostPrice;

		private IList<MysticCurrencyQuantity> _vendorUnitPrice = new List<MysticCurrencyQuantity>();

		private IList<MysticCurrencyQuantity> _totalPrice = new List<MysticCurrencyQuantity>();

		private MysticCurrencyQuantity _totalCoinPrice = new MysticCurrencyQuantity();

		private readonly ITradeableNodePresenter _tradeableNodePresenter = new TradeableNodePresenter(ServiceContainer.CurrencyRepository);

		public IEnumerable<TradeableItemNode> TradeableNodes => base.Nodes.OfType<TradeableItemNode>();

		public int TradingPostPrice
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

		public int VendorUnitQuantity { get; set; } = 1;


		public IList<MysticCurrencyQuantity> VendorUnitPrice
		{
			get
			{
				return _vendorUnitPrice;
			}
			set
			{
				_vendorUnitPrice = value;
				CalculateTotalPrices();
				if (!LoadingChildren)
				{
					UpdatePriceControls();
				}
			}
		}

		public IList<MysticCurrencyQuantity> TotalVendorPrice { get; set; } = new List<MysticCurrencyQuantity>();


		public IList<MysticCurrencyQuantity> GrandTotalPrice
		{
			get
			{
				return _totalPrice;
			}
			set
			{
				_totalPrice = value?.CombineQuantities().ToList();
				if (!LoadingChildren)
				{
					UpdatePriceControls();
				}
			}
		}

		public MysticCurrencyQuantity TotalCoinPrice
		{
			get
			{
				return _totalCoinPrice;
			}
			set
			{
				if (value == null)
				{
					_totalCoinPrice = new MysticCurrencyQuantity();
					return;
				}
				_totalCoinPrice = value;
				if (!LoadingChildren)
				{
					UpdatePriceControls();
				}
			}
		}

		public abstract int RequiredQuantity { get; }

		private CurrenciesContainer CurrencyContainer { get; set; }

		private CoinsControl CoinsControl { get; set; }

		private VendorCurrencyControl VendorCurrencyControl { get; set; }

		public FlowPanel RequirementsPanel { get; set; }

		private Tooltip VendorCurrencyTooltip { get; set; }

		public bool HasTradeableChildren => base.Nodes.OfType<TradeableItemNode>().Any();

		public virtual Point PriceLocation { get; set; } = new Point(310, 10);


		public void CalculateTotalPrices()
		{
			_tradeableNodePresenter.CalculateTotalPrice(this);
		}

		public void ResetPrices()
		{
			_tradingPostPrice = 0;
			_vendorUnitPrice = new List<MysticCurrencyQuantity>();
			if (TotalCoinPrice != null)
			{
				TotalCoinPrice.Count = 0;
			}
			TotalVendorPrice = new List<MysticCurrencyQuantity>();
			VendorUnitPrice = new List<MysticCurrencyQuantity>();
			_totalPrice = new List<MysticCurrencyQuantity>();
		}

		public virtual void BuildPriceControls()
		{
			CoinsControl = new CoinsControl(this)
			{
				UnitPrice = TotalCoinPrice.Count,
				Location = PriceLocation
			};
			BuildVendorPriceControls();
		}

		private Point GetRequirementsLocation()
		{
			return new Point((_totalCoinPrice.Count != 0) ? (PriceLocation.X + 150) : (PriceLocation.X - 5), PriceLocation.Y);
		}

		public void UpdateRequirementsLocation()
		{
			if (RequirementsPanel != null)
			{
				RequirementsPanel.Location = GetRequirementsLocation();
			}
		}

		private void BuildVendorPriceControls()
		{
			if (RequirementsPanel == null)
			{
				RequirementsPanel = new FlowPanel
				{
					FlowDirection = ControlFlowDirection.LeftToRight,
					Size = new Point(120, 25),
					Location = GetRequirementsLocation(),
					ControlPadding = new Vector2(10f, 0f),
					Parent = this,
					CanScroll = false
				};
			}
			VendorCurrencyControl = new VendorCurrencyControl(RequirementsPanel)
			{
				Price = TotalVendorPrice,
				Size = new Point(30, 25),
				Visible = false
			};
			VendorCurrencyTooltip = new DisposableTooltip(new CurrenciesTooltipView(TotalVendorPrice));
			VendorCurrencyControl.Tooltip = VendorCurrencyTooltip;
			CurrencyContainer = new CurrenciesContainer(RequirementsPanel)
			{
				Price = TotalVendorPrice,
				Visible = false
			};
			SwitchVendorPriceControls();
		}

		private bool ShowMinifiedVendorCurrencies()
		{
			if (HasTradeableChildren || TotalVendorPrice.Count > 2)
			{
				if (CoinsControl.UnitPrice == 0)
				{
					return TotalVendorPrice.Count > 3;
				}
				return true;
			}
			return false;
		}

		private void SwitchVendorPriceControls()
		{
			if (ShowMinifiedVendorCurrencies())
			{
				VendorCurrencyControl.Show();
				CurrencyContainer.Hide();
			}
			else
			{
				VendorCurrencyControl.Hide();
				CurrencyContainer.Show();
			}
		}

		public virtual void UpdatePriceControls()
		{
			if (CoinsControl == null)
			{
				BuildPriceControls();
			}
			if (TotalCoinPrice.Count >= 0)
			{
				if (CoinsControl.Parent == null)
				{
					CoinsControl.Parent = this;
				}
				CoinsControl.UnitPrice = TotalCoinPrice.Count;
			}
			if (TotalVendorPrice.Any((MysticCurrencyQuantity p) => p.Count > 0))
			{
				if (ShowMinifiedVendorCurrencies())
				{
					VendorCurrencyControl.Price = TotalVendorPrice;
					VendorCurrencyTooltip?.Dispose();
					VendorCurrencyTooltip = new DisposableTooltip(new CurrenciesTooltipView(TotalVendorPrice));
					VendorCurrencyControl.Tooltip = VendorCurrencyTooltip;
				}
				else
				{
					CurrencyContainer.Price = TotalVendorPrice;
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

		protected override void DisposeControl()
		{
			VendorCurrencyTooltip?.Dispose();
			VendorCurrencyControl?.Dispose();
			CoinsControl?.Dispose();
			MenuStrip?.Dispose();
			base.DisposeControl();
		}
	}
}
