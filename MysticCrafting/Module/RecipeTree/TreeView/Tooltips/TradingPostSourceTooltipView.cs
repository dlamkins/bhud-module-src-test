using System.Collections.Generic;
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
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Tooltips
{
	public class TradingPostSourceTooltipView : View, ITooltipView, IView
	{
		private List<Control> _controls = new List<Control>();

		protected bool Initialized;

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
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Expected O, but got Unknown
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Expected O, but got Unknown
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
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
			Label val3 = new Label();
			((Control)val3).set_Parent(BuildPanel);
			val3.set_Text(Recipe.TradingPostBuy + ":");
			val3.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val3).set_Location(new Point(5, 40));
			val3.set_TextColor(Color.get_White());
			val3.set_StrokeText(true);
			val3.set_AutoSizeWidth(true);
			Label buyLabel = val3;
			_controls.Add((Control)(object)buyLabel);
			List<Control> controls3 = _controls;
			CoinsControl coinsControl = new CoinsControl(BuildPanel);
			((Control)coinsControl).set_Location(new Point(((Control)buyLabel).get_Right() + 5, 40));
			coinsControl.UnitPrice = TradingPostSource.BuyPrice.UnitPrice;
			controls3.Add((Control)(object)coinsControl);
			Label val4 = new Label();
			((Control)val4).set_Parent(BuildPanel);
			val4.set_Text(Recipe.TradingPostSell + ":");
			val4.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val4).set_Location(new Point(5, 65));
			val4.set_TextColor(Color.get_White());
			val4.set_StrokeText(true);
			val4.set_AutoSizeWidth(true);
			Label sellLabel = val4;
			_controls.Add((Control)(object)sellLabel);
			List<Control> controls4 = _controls;
			CoinsControl coinsControl2 = new CoinsControl(BuildPanel);
			((Control)coinsControl2).set_Location(new Point(((Control)sellLabel).get_Right() + 5, 65));
			coinsControl2.UnitPrice = TradingPostSource.SellPrice.UnitPrice;
			controls4.Add((Control)(object)coinsControl2);
		}

		protected override void Unload()
		{
			_controls?.SafeDispose();
			_controls?.Clear();
			BuildPanel = null;
			((View<IPresenter>)this).Unload();
		}
	}
}
