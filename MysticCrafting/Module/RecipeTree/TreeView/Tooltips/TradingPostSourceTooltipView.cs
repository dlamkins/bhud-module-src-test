using System.Collections.Generic;
using Atzie.MysticCrafting.Models.Items;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.RecipeTree.TreeView.Controls;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.API;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Tooltips
{
	public class TradingPostSourceTooltipView : View, ITooltipView, IView
	{
		private List<Control> _controls = new List<Control>();

		protected bool Initialized;

		private CoinsControl _buyCoinsControl;

		private Label _buyCoinsUnavailableLabel;

		private CoinsControl _sellCoinsControl;

		private Label _sellCoinsUnavailableLabel;

		private TradingPostSource TradingPostSource { get; set; }

		public Container BuildPanel { get; set; }

		public TradingPostSourceTooltipView(TradingPostSource tradingPostSource)
			: this()
		{
			TradingPostSource = tradingPostSource;
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
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected O, but got Unknown
			AsyncTexture2D icon = ServiceContainer.TextureRepository.Textures.TradingPostIcon;
			List<Control> controls = _controls;
			Image val = new Image(icon);
			((Control)val).set_Parent(BuildPanel);
			((Control)val).set_Size(new Point(25, 25));
			((Control)val).set_Location(new Point(0, 0));
			controls.Add((Control)val);
			List<Control> controls2 = _controls;
			Label val2 = new Label();
			((Control)val2).set_Parent(BuildPanel);
			val2.set_Text(Recipe.TradingPost);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val2).set_Location(new Point(30, 3));
			val2.set_StrokeText(true);
			val2.set_AutoSizeWidth(true);
			controls2.Add((Control)val2);
			BuildBuyPriceControls();
			BuildSellPriceControls();
			ServiceContainer.TradingPostService.ItemPriceChanged += TradingPostServiceOnItemPriceChanged;
		}

		private void TradingPostServiceOnItemPriceChanged(object sender, ItemPriceChangedEventArgs e)
		{
			if (e.Item.Id == TradingPostSource.Item.Id)
			{
				UpdateBuyPrice(e.Item);
				UpdateSellPrice(e.Item);
			}
		}

		private void BuildBuyPriceControls()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent(BuildPanel);
			val.set_Text(Recipe.TradingPostBuy + ":");
			val.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val).set_Location(new Point(5, 40));
			val.set_TextColor(Color.get_White());
			val.set_StrokeText(true);
			val.set_AutoSizeWidth(true);
			Label buyLabel = val;
			_controls.Add((Control)(object)buyLabel);
			CoinsControl coinsControl = new CoinsControl(BuildPanel);
			((Control)coinsControl).set_Location(new Point(((Control)buyLabel).get_Right() + 5, 40));
			coinsControl.UnitPrice = TradingPostSource.Item.TradingPostBuy.GetValueOrDefault();
			_buyCoinsControl = coinsControl;
			_controls.Add((Control)(object)_buyCoinsControl);
			Label val2 = new Label();
			((Control)val2).set_Parent(BuildPanel);
			val2.set_Text(Recipe.Unavailable);
			((Control)val2).set_Location(new Point(((Control)buyLabel).get_Right() + 5, 40));
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(Color.get_White());
			val2.set_StrokeText(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Visible(TradingPostSource.Item.TradingPostBuy == 0);
			_buyCoinsUnavailableLabel = val2;
			_controls.Add((Control)(object)_buyCoinsUnavailableLabel);
		}

		private void UpdateBuyPrice(Item item)
		{
			if (item.TradingPostBuy.HasValue && item.TradingPostBuy != 0)
			{
				_buyCoinsControl.UnitPrice = item.TradingPostBuy.GetValueOrDefault();
				((Control)_buyCoinsUnavailableLabel).Hide();
				return;
			}
			Label buyCoinsUnavailableLabel = _buyCoinsUnavailableLabel;
			if (buyCoinsUnavailableLabel != null)
			{
				((Control)buyCoinsUnavailableLabel).Show();
			}
		}

		private void BuildSellPriceControls()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent(BuildPanel);
			val.set_Text(Recipe.TradingPostSell + ":");
			val.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val).set_Location(new Point(5, 65));
			val.set_TextColor(Color.get_White());
			val.set_StrokeText(true);
			val.set_AutoSizeWidth(true);
			Label sellLabel = val;
			_controls.Add((Control)(object)sellLabel);
			CoinsControl coinsControl = new CoinsControl(BuildPanel);
			((Control)coinsControl).set_Location(new Point(((Control)sellLabel).get_Right() + 5, 65));
			coinsControl.UnitPrice = TradingPostSource.Item.TradingPostSell.GetValueOrDefault();
			_sellCoinsControl = coinsControl;
			_controls.Add((Control)(object)_sellCoinsControl);
			Label val2 = new Label();
			((Control)val2).set_Parent(BuildPanel);
			val2.set_Text(Recipe.Unavailable);
			((Control)val2).set_Location(new Point(((Control)sellLabel).get_Right() + 5, 65));
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(Color.get_White());
			val2.set_StrokeText(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Visible(TradingPostSource.Item.TradingPostSell == 0);
			_sellCoinsUnavailableLabel = val2;
			_controls.Add((Control)(object)_sellCoinsUnavailableLabel);
		}

		private void UpdateSellPrice(Item item)
		{
			if (item.TradingPostSell.HasValue && item.TradingPostSell != 0)
			{
				_sellCoinsControl.UnitPrice = item.TradingPostSell.GetValueOrDefault();
				((Control)_sellCoinsUnavailableLabel).Hide();
				return;
			}
			Label sellCoinsUnavailableLabel = _sellCoinsUnavailableLabel;
			if (sellCoinsUnavailableLabel != null)
			{
				((Control)sellCoinsUnavailableLabel).Show();
			}
		}

		protected override void Unload()
		{
			_controls?.SafeDispose();
			_controls?.Clear();
			BuildPanel = null;
			ServiceContainer.TradingPostService.ItemPriceChanged -= TradingPostServiceOnItemPriceChanged;
			((View<IPresenter>)this).Unload();
		}
	}
}
