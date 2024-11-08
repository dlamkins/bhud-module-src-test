using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class CoinsPanel : FlowPanel
	{
		private readonly CoinSignLabel _signLabel;

		private readonly CoinPanel _goldPanel;

		private readonly CoinPanel _silverPanel;

		private readonly CoinPanel _copperPanel;

		public CoinsPanel(Tooltip? tooltip, BitmapFont font, TextureService textureService, Container parent, int height = 0)
			: this()
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)2);
			((Control)this).set_Tooltip(tooltip);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			if (height > 0)
			{
				((Control)this).set_Height(height);
			}
			else
			{
				((Container)this).set_HeightSizingMode((SizingMode)1);
			}
			((Control)this).set_Parent(parent);
			_signLabel = new CoinSignLabel(tooltip, font, (Container)(object)this);
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)2);
			val.set_ControlPadding(new Vector2(5f, 0f));
			((Control)val).set_Tooltip(tooltip);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)this);
			FlowPanel coinsFlowPanel = val;
			_goldPanel = new CoinPanel(textureService.SmallGoldCoinTexture, Color.get_Gold(), tooltip, font, widthFixed: false, (Container)(object)coinsFlowPanel);
			_silverPanel = new CoinPanel(textureService.SmallSilverCoinTexture, Color.get_LightGray(), tooltip, font, widthFixed: true, (Container)(object)coinsFlowPanel);
			_copperPanel = new CoinPanel(textureService.SmallCopperCoinTexture, Color.get_SandyBrown(), tooltip, font, widthFixed: true, (Container)(object)coinsFlowPanel);
		}

		public void SetCoins(long signed_coinsInCopper)
		{
			Coin coin = new Coin(signed_coinsInCopper);
			_signLabel.SetSign(coin.Sign);
			_goldPanel.SetValue(coin.Unsigned_Gold, isZeroValueVisible: false);
			_silverPanel.SetValue(coin.Unsigned_Silver, coin.Unsigned_Gold > 0);
			_copperPanel.SetValue(coin.Unsigned_Copper, isZeroValueVisible: true);
		}

		protected override void DisposeControl()
		{
			CoinPanel goldPanel = _goldPanel;
			if (goldPanel != null)
			{
				((Control)goldPanel).Dispose();
			}
			CoinPanel silverPanel = _silverPanel;
			if (silverPanel != null)
			{
				((Control)silverPanel).Dispose();
			}
			CoinPanel copperPanel = _copperPanel;
			if (copperPanel != null)
			{
				((Control)copperPanel).Dispose();
			}
			((FlowPanel)this).DisposeControl();
		}
	}
}
