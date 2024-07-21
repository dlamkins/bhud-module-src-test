using System;
using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Vendor;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysticCrafting.Module.Extensions;

namespace MysticCrafting.Module.RecipeTree.TreeView.Controls
{
	public class VendorPriceContainer : FlowPanel
	{
		private CoinsControl _coinsControl;

		private List<Label> _plusLabels = new List<Label>();

		private List<VendorCostControl> _costControls = new List<VendorCostControl>();

		private IList<VendorSellsItemCost> _costs = new List<VendorSellsItemCost>();

		private bool _isInitialized;

		public IList<VendorSellsItemCost> Costs
		{
			get
			{
				return _costs;
			}
			set
			{
				_costs = value;
				if (!_isInitialized)
				{
					Build(_costs);
				}
				else
				{
					Update(_costs);
				}
			}
		}

		public VendorPriceContainer(Container parent)
			: this()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)4);
			((FlowPanel)this).set_ControlPadding(new Vector2(4f, 0f));
		}

		public void Build(IList<VendorSellsItemCost> costs)
		{
			int coinPrice = costs.CoinCount();
			CoinsControl coinsControl = _coinsControl;
			if (coinsControl != null)
			{
				((Control)coinsControl).Dispose();
			}
			_coinsControl = new CoinsControl((Container)(object)this)
			{
				UnitPrice = coinPrice
			};
			BuildVendorCostControls(costs);
			_isInitialized = true;
		}

		public void Update(IList<VendorSellsItemCost> costs)
		{
			int coinPrice = costs.CoinCount();
			_coinsControl.UnitPrice = coinPrice;
			DisposeCurrencyControls();
			BuildVendorCostControls(costs);
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
			(_costControls?.ToList())?.ForEach(delegate(VendorCostControl l)
			{
				((Control)l).Dispose();
			});
			_costControls = new List<VendorCostControl>();
		}

		private void BuildVendorCostControls(IList<VendorSellsItemCost> costs)
		{
			costs = costs.ExcludingCoins().ToList();
			bool showPlusSign = _coinsControl != null && _coinsControl.UnitPrice != 0;
			foreach (VendorSellsItemCost cost in costs)
			{
				if (showPlusSign)
				{
					BuildPlusSignControl();
				}
				showPlusSign = true;
				if (cost.Currency == null)
				{
					_ = cost.Item;
				}
				BuildVendorCostControl(cost);
			}
		}

		private void BuildPlusSignControl()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("+");
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_AutoSizeWidth(true);
		}

		private void BuildVendorCostControl(VendorSellsItemCost cost)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			_costControls.Add(new VendorCostControl((Container)(object)this)
			{
				Cost = cost,
				IconSize = new Point(20, 20)
			});
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
			Color lineColor = Color.get_LightYellow() * 0.5f;
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
