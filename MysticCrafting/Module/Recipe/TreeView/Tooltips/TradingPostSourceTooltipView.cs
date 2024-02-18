using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Models;
using MysticCrafting.Module.Recipe.TreeView.Controls;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Recipe.TreeView.Tooltips
{
	public class TradingPostSourceTooltipView : View, ITooltipView, IView
	{
		private List<Control> _controls = new List<Control>();

		protected bool Initialized;

		private TradingPostSource TradingPostSource { get; set; }

		public Container BuildPanel { get; set; }

		public TradingPostSourceTooltipView(TradingPostSource tradingPostSource)
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
			AsyncTexture2D icon = ServiceContainer.TextureRepository.Textures.TradingPostIcon;
			_controls.Add(new Image(icon)
			{
				Parent = BuildPanel,
				Size = new Point(25, 25),
				Location = new Point(0, 0)
			});
			_controls.Add(new Label
			{
				Parent = BuildPanel,
				Text = MysticCrafting.Module.Strings.Recipe.TradingPost,
				Font = GameService.Content.DefaultFont16,
				Location = new Point(30, 3),
				StrokeText = true,
				AutoSizeWidth = true
			});
			Label buyLabel = new Label
			{
				Parent = BuildPanel,
				Text = MysticCrafting.Module.Strings.Recipe.TradingPostBuy + ":",
				Font = GameService.Content.DefaultFont14,
				Location = new Point(5, 40),
				TextColor = Color.White,
				StrokeText = true,
				AutoSizeWidth = true
			};
			_controls.Add(buyLabel);
			_controls.Add(new CoinsControl(BuildPanel)
			{
				Location = new Point(buyLabel.Right + 5, 40),
				UnitPrice = TradingPostSource.BuyPrice.UnitPrice
			});
			Label sellLabel = new Label
			{
				Parent = BuildPanel,
				Text = MysticCrafting.Module.Strings.Recipe.TradingPostSell + ":",
				Font = GameService.Content.DefaultFont14,
				Location = new Point(5, 65),
				TextColor = Color.White,
				StrokeText = true,
				AutoSizeWidth = true
			};
			_controls.Add(sellLabel);
			_controls.Add(new CoinsControl(BuildPanel)
			{
				Location = new Point(sellLabel.Right + 5, 65),
				UnitPrice = TradingPostSource.SellPrice.UnitPrice
			});
		}

		protected override void Unload()
		{
			_controls?.SafeDispose();
			_controls?.Clear();
			BuildPanel = null;
			base.Unload();
		}
	}
}
