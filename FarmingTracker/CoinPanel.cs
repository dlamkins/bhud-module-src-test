using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class CoinPanel : FlowPanel
	{
		private readonly Label _coinLabel;

		private readonly Image _coinImage;

		private readonly Container _parent;

		private const long MAX_COIN_DISPLAY_VALUE = 1000000L;

		public CoinPanel(AsyncTexture2D coinTexture, Color textColor, string tooltip, BitmapFont font, bool widthFixed, Container parent)
			: this()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Expected O, but got Unknown
			_parent = parent;
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)2);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Control)this).set_Parent(parent);
			Label val = new Label();
			val.set_Text("?");
			((Control)val).set_BasicTooltipText(tooltip);
			val.set_Font(font);
			val.set_TextColor(textColor);
			val.set_AutoSizeHeight(true);
			val.set_HorizontalAlignment((HorizontalAlignment)2);
			((Control)val).set_Parent((Container)(object)this);
			_coinLabel = val;
			if (widthFixed)
			{
				((Control)_coinLabel).set_Width(20);
			}
			else
			{
				_coinLabel.set_AutoSizeWidth(true);
			}
			Image val2 = new Image(coinTexture);
			((Control)val2).set_Size(new Point(((Control)_coinLabel).get_Height() * 11 / 10));
			((Control)val2).set_BasicTooltipText(tooltip);
			((Control)val2).set_Parent((Container)(object)this);
			_coinImage = val2;
		}

		public void SetValue(long unsignedCoinValue, bool isZeroValueVisible)
		{
			((Control)this).set_Parent((Container)null);
			if (isZeroValueVisible || unsignedCoinValue != 0L)
			{
				_coinLabel.set_Text((unsignedCoinValue > 1000000) ? $">{1000000L}" : unsignedCoinValue.ToString());
				((Control)_coinLabel).set_Parent((Container)(object)this);
				((Control)_coinImage).set_Parent((Container)(object)this);
				((Control)this).set_Parent(_parent);
			}
		}

		protected override void DisposeControl()
		{
			Label coinLabel = _coinLabel;
			if (coinLabel != null)
			{
				((Control)coinLabel).Dispose();
			}
			Image coinImage = _coinImage;
			if (coinImage != null)
			{
				((Control)coinImage).Dispose();
			}
			((FlowPanel)this).DisposeControl();
		}
	}
}
