using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysticCrafting.Models.Commerce;
using MysticCrafting.Models.Items;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Recipe.TreeView.Tooltips;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Recipe.TreeView.Controls
{
	public class CurrenciesContainer : FlowPanel
	{
		public bool MinifyPricing;

		private CoinsControl _coinsControl;

		private List<Label> _plusLabels = new List<Label>();

		private List<Control> _controls = new List<Control>();

		private List<CurrencyControl> _currencyControls = new List<CurrencyControl>();

		private IList<MysticCurrencyQuantity> _price = new List<MysticCurrencyQuantity>();

		private bool _isInitialized;

		public IList<MysticCurrencyQuantity> Price
		{
			get
			{
				return _price;
			}
			set
			{
				_price = value;
				if (!_isInitialized)
				{
					Build(_price);
				}
				else
				{
					Update(_price);
				}
			}
		}

		public CurrenciesContainer(Container parent)
		{
			base.Parent = parent;
			base.FlowDirection = ControlFlowDirection.LeftToRight;
			base.ControlPadding = new Vector2(4f, 0f);
		}

		public void Build(IList<MysticCurrencyQuantity> prices)
		{
			int coinPrice = prices.CoinCount();
			_coinsControl?.Dispose();
			_coinsControl = new CoinsControl(this)
			{
				UnitPrice = coinPrice
			};
			if (prices.ExcludingCoins().Count() < 2)
			{
				MinifyPricing = false;
			}
			if (MinifyPricing)
			{
				BuildMinifiedCurrencies(prices);
			}
			else
			{
				BuildCurrencies(prices);
			}
			_isInitialized = true;
		}

		private void BuildMinifiedCurrencies(IList<MysticCurrencyQuantity> prices)
		{
			prices = prices.ExcludingCoins().ToList();
			if (prices.Count != 0)
			{
				Label numberLabel = new Label
				{
					Parent = this,
					Text = prices.Count.ToString(),
					Font = GameService.Content.DefaultFont14,
					AutoSizeWidth = true
				};
				_plusLabels.Add(numberLabel);
				Image image = new Image
				{
					Parent = this,
					Texture = AsyncTexture2D.FromAssetId(156754),
					Size = new Point(20, 20)
				};
				_controls.Add(image);
			}
		}

		private void DisposeCurrencyControls()
		{
			if (_plusLabels != null)
			{
				_plusLabels.ForEach(delegate(Label l)
				{
					l.Dispose();
				});
			}
			_plusLabels = new List<Label>();
			(_currencyControls?.ToList())?.ForEach(delegate(CurrencyControl l)
			{
				l.Dispose();
			});
			_currencyControls = new List<CurrencyControl>();
			_controls.ToList().ForEach(delegate(Control l)
			{
				l.Dispose();
			});
			_controls = new List<Control>();
		}

		private void BuildCurrencies(IList<MysticCurrencyQuantity> prices)
		{
			bool showPlusSign = _coinsControl != null && _coinsControl.UnitPrice != 0;
			foreach (MysticCurrencyQuantity price in prices.Where((MysticCurrencyQuantity p) => p.Currency != null && p.Currency.GameId != 1))
			{
				if (price.Currency != null)
				{
					if (showPlusSign)
					{
						_plusLabels.Add(new Label
						{
							Parent = this,
							Text = "+",
							Font = GameService.Content.DefaultFont16,
							AutoSizeWidth = true
						});
					}
					showPlusSign = true;
					CurrencyControl currencyControl = new CurrencyControl(this)
					{
						Quantity = price,
						IconSize = new Point(20, 20)
					};
					BuildCurrencyTooltip(currencyControl, price);
					_currencyControls.Add(currencyControl);
				}
			}
		}

		private void BuildCurrencyTooltip(CurrencyControl currencyControl, MysticCurrencyQuantity price)
		{
			if (price.Currency.IsItem)
			{
				MysticItem item = ServiceContainer.ItemRepository.GetItem(price.Currency.GameId);
				if (item != null)
				{
					currencyControl.Tooltip = new DisposableTooltip(new ItemTooltipView(item, price.Count));
				}
			}
			if (currencyControl.Tooltip == null)
			{
				currencyControl.Tooltip = new DisposableTooltip(new CurrencyTooltipView(price));
			}
		}

		public void Update(IList<MysticCurrencyQuantity> prices)
		{
			int coinPrice = prices.CoinCount();
			_coinsControl.UnitPrice = coinPrice;
			DisposeCurrencyControls();
			if (MinifyPricing)
			{
				BuildMinifiedCurrencies(prices);
			}
			else
			{
				BuildCurrencies(prices);
			}
		}

		protected override void OnChildAdded(ChildChangedEventArgs e)
		{
			e.ChangedChild.Resized += delegate
			{
				int num = CalculateWidth();
				if (base.Width != num)
				{
					base.Width = num;
				}
			};
		}

		public int CalculateWidth()
		{
			float padding = (float)(base.Children.Count + 1) * base.ControlPadding.X;
			return base.Children.Sum((Control c) => c.Width) + (int)padding;
		}

		private void DrawOutline(SpriteBatch spriteBatch)
		{
			Color lineColor = Color.Yellow * 0.5f;
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(0, 0, base.Width, 1), lineColor);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(0, base.Height - 2, base.Width, 1), lineColor);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(0, 0, 1, base.Height - 1), lineColor);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(base.Width - 1, 0, 1, base.Height - 1), lineColor);
		}

		protected override void DisposeControl()
		{
			DisposeCurrencyControls();
			base.DisposeControl();
		}
	}
}
