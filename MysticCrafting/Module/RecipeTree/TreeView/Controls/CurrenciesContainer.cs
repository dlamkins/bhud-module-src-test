using System;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Currencies;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.RecipeTree.TreeView.Nodes;
using MysticCrafting.Module.RecipeTree.TreeView.Tooltips;

namespace MysticCrafting.Module.RecipeTree.TreeView.Controls
{
	public class CurrenciesContainer : FlowPanel
	{
		public bool MinifyPricing;

		private CoinsControl _coinsControl;

		private List<Label> _plusLabels = new List<Label>();

		private List<Control> _controls = new List<Control>();

		private List<CurrencyControl> _currencyControls = new List<CurrencyControl>();

		private IList<CurrencyQuantity> _price = new List<CurrencyQuantity>();

		private bool _isInitialized;

		private TradeableItemNode Node { get; set; }

		public IList<CurrencyQuantity> Price
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

		public CurrenciesContainer(Container parent, TradeableItemNode node)
			: this()
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
			Node = node;
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)0);
			((FlowPanel)this).set_ControlPadding(new Vector2(4f, 0f));
		}

		public void Build(IList<CurrencyQuantity> prices)
		{
			int coinPrice = prices.CoinCount();
			CoinsControl coinsControl = _coinsControl;
			if (coinsControl != null)
			{
				((Control)coinsControl).Dispose();
			}
			_coinsControl = new CoinsControl((Container)(object)this)
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

		private void BuildMinifiedCurrencies(IList<CurrencyQuantity> prices)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			prices = prices.ExcludingCoins().ToList();
			if (prices.Count != 0)
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)this);
				val.set_Text(prices.Count.ToString());
				val.set_Font(GameService.Content.get_DefaultFont14());
				val.set_AutoSizeWidth(true);
				Label numberLabel = val;
				_plusLabels.Add(numberLabel);
				Image val2 = new Image();
				((Control)val2).set_Parent((Container)(object)this);
				val2.set_Texture(AsyncTexture2D.FromAssetId(156754));
				((Control)val2).set_Size(new Point(20, 20));
				Image image = val2;
				_controls.Add((Control)(object)image);
			}
		}

		private void DisposeCurrencyControls()
		{
			if (_plusLabels != null)
			{
				_plusLabels.ForEach(delegate(Label l)
				{
					((Control)l).Dispose();
				});
			}
			_plusLabels = new List<Label>();
			(_currencyControls?.ToList())?.ForEach(delegate(CurrencyControl l)
			{
				((Control)l).Dispose();
			});
			_currencyControls = new List<CurrencyControl>();
			_controls.ToList().ForEach(delegate(Control l)
			{
				l.Dispose();
			});
			_controls = new List<Control>();
		}

		private void BuildCurrencies(IList<CurrencyQuantity> prices)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			bool showPlusSign = _coinsControl != null && _coinsControl.UnitPrice != 0;
			foreach (CurrencyQuantity price in prices.Where((CurrencyQuantity p) => p.Currency != null && p.Currency.Id != 1))
			{
				if (showPlusSign)
				{
					List<Label> plusLabels = _plusLabels;
					Label val = new Label();
					((Control)val).set_Parent((Container)(object)this);
					val.set_Text("+");
					val.set_Font(GameService.Content.get_DefaultFont16());
					val.set_AutoSizeWidth(true);
					plusLabels.Add(val);
				}
				showPlusSign = true;
				CurrencyControl currencyControl = new CurrencyControl((Container)(object)this)
				{
					Quantity = price,
					IconSize = new Point(20, 20)
				};
				BuildCurrencyTooltip(currencyControl, price);
				_currencyControls.Add(currencyControl);
			}
		}

		private void BuildCurrencyTooltip(CurrencyControl currencyControl, CurrencyQuantity price)
		{
			if (((Control)currencyControl).get_Tooltip() == null)
			{
				((Control)currencyControl).set_Tooltip((Tooltip)(object)new DisposableTooltip((ITooltipView)(object)new CurrencyTooltipView(price, Node)));
			}
		}

		public void UpdateReservedQuantity(int nodeIndex)
		{
			foreach (CurrencyControl currencyControl in _currencyControls)
			{
				(((Control)currencyControl).get_Tooltip().get_CurrentView() as CurrencyTooltipView)?.UpdateReservedQuantity(nodeIndex);
			}
		}

		public void Update(IList<CurrencyQuantity> prices)
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
			e.get_ChangedChild().add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				int num = CalculateWidth();
				if (((Control)this).get_Width() != num)
				{
					((Control)this).set_Width(num);
				}
			});
		}

		public int CalculateWidth()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			float padding = (float)(((Container)this).get_Children().get_Count() + 1) * ((FlowPanel)this).get_ControlPadding().X;
			return ((IEnumerable<Control>)((Container)this).get_Children()).Sum((Control c) => c.get_Width()) + (int)padding + 5;
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintAfterChildren(spriteBatch, bounds);
		}

		private void DrawOutline(SpriteBatch spriteBatch)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			Color lineColor = Color.get_Yellow() * 0.5f;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, ((Control)this).get_Width(), 1), lineColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, ((Control)this).get_Height() - 2, ((Control)this).get_Width(), 1), lineColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, 1, ((Control)this).get_Height() - 1), lineColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Control)this).get_Width() - 1, 0, 1, ((Control)this).get_Height() - 1), lineColor);
		}

		protected override void DisposeControl()
		{
			DisposeCurrencyControls();
			((FlowPanel)this).DisposeControl();
		}
	}
}
