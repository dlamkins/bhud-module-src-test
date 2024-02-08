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

		private ITradeableNodePresenter _tradeableNodePresenter = new TradeableNodePresenter(ServiceContainer.CurrencyRepository);

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

		private CurrenciesContainer _currencyContainer { get; set; }

		private CoinsControl _coinsControl { get; set; }

		private VendorCurrencyControl _vendorCurrencyControl { get; set; }

		private Blish_HUD.Controls.Tooltip VendorCurrencyTooltip { get; set; }

		public bool HasTradableChildren => base.Nodes.OfType<TradeableItemNode>().Any();

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
			_coinsControl = new CoinsControl(this)
			{
				UnitPrice = TotalCoinPrice.Count,
				Location = PriceLocation
			};
			BuildVendorPriceControls();
		}

		private void BuildVendorPriceControls()
		{
			Point location = new Point(PriceLocation.X + 100, 10);
			_vendorCurrencyControl = new VendorCurrencyControl(this)
			{
				Price = TotalVendorPrice,
				Location = location,
				Size = new Point(100, 20)
			};
			VendorCurrencyTooltip = new Blish_HUD.Controls.Tooltip(new CurrenciesTooltipView(TotalVendorPrice));
			_vendorCurrencyControl.Tooltip = VendorCurrencyTooltip;
			_currencyContainer = new CurrenciesContainer(this)
			{
				Location = location,
				Price = TotalVendorPrice
			};
			_currencyContainer.Build(TotalVendorPrice);
			SwitchVendorPriceControls();
		}

		private void SwitchVendorPriceControls()
		{
			if (HasTradableChildren || TotalVendorPrice.Count > 2)
			{
				_vendorCurrencyControl.Show();
				_currencyContainer.Hide();
			}
			else
			{
				_vendorCurrencyControl.Hide();
				_currencyContainer.Show();
			}
		}

		public virtual void UpdatePriceControls()
		{
			if (_coinsControl == null)
			{
				BuildPriceControls();
			}
			if (TotalCoinPrice.Count >= 0)
			{
				_coinsControl.UnitPrice = TotalCoinPrice.Count;
			}
			if (TotalVendorPrice.Any((MysticCurrencyQuantity p) => p.Count > 0))
			{
				if (HasTradableChildren || TotalVendorPrice.Count > 2)
				{
					_vendorCurrencyControl.Location = new Point(PriceLocation.X + 150, PriceLocation.Y);
					_vendorCurrencyControl.Price = TotalVendorPrice;
					VendorCurrencyTooltip?.Dispose();
					VendorCurrencyTooltip = new Blish_HUD.Controls.Tooltip(new CurrenciesTooltipView(TotalVendorPrice));
					_vendorCurrencyControl.Tooltip = VendorCurrencyTooltip;
					_vendorCurrencyControl.Show();
				}
				else
				{
					_currencyContainer.Location = new Point(PriceLocation.X + 150, PriceLocation.Y);
					_currencyContainer.Price = TotalVendorPrice;
				}
				SwitchVendorPriceControls();
			}
			else
			{
				if (_currencyContainer != null)
				{
					_currencyContainer.Price = TotalVendorPrice;
				}
				if (_vendorCurrencyControl != null)
				{
					_vendorCurrencyControl.Price = TotalVendorPrice;
				}
			}
		}
	}
}
