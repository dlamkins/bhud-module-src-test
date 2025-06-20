using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class LeaderboardStatBestScoreControl : Control, IDisposable
	{
		protected BestScoreStat _bestScoreStat;

		protected const int BORDER_SIZE = 3;

		public LeaderboardStatBestScoreControl(BestScoreStat stat)
			: this()
		{
			_bestScoreStat = stat;
			((Control)this).set_Width(805);
			((Control)this).set_Height(300);
		}

		protected override void DisposeControl()
		{
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Height(270);
			Color color = Color.get_LightGoldenrodYellow();
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _bestScoreStat.Title, Control.get_Content().get_DefaultFont18(), new Rectangle(5, 5, 390, 30), color, false, (HorizontalAlignment)0, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Account", Control.get_Content().get_DefaultFont16(), new Rectangle(5, 35, 190, 20), Color.get_WhiteSmoke(), false, (HorizontalAlignment)0, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _bestScoreStat.ValueHeader, Control.get_Content().get_DefaultFont16(), new Rectangle(205, 35, 190, 20), Color.get_WhiteSmoke(), false, (HorizontalAlignment)0, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _bestScoreStat.CountHeader, Control.get_Content().get_DefaultFont16(), new Rectangle(405, 35, 190, 20), Color.get_WhiteSmoke(), false, (HorizontalAlignment)0, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(5, 55, ((Control)this).get_Width() - 10, 1), Color.get_WhiteSmoke());
			int verticalOffset = 60;
			Color colorOdd = Color.get_White();
			Color colorEven = Color.get_Wheat();
			bool isOdd = false;
			foreach (BestScoreEntry entry in _bestScoreStat.Entries)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, entry.Author, Control.get_Content().get_DefaultFont16(), new Rectangle(10, verticalOffset, 180, 20), isOdd ? colorOdd : colorEven, false, (HorizontalAlignment)0, (VerticalAlignment)0);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, entry.Value.ToString(), Control.get_Content().get_DefaultFont16(), new Rectangle(210, verticalOffset, 190, 20), isOdd ? colorOdd : colorEven, false, (HorizontalAlignment)0, (VerticalAlignment)0);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, entry.Count.ToString(), Control.get_Content().get_DefaultFont16(), new Rectangle(410, verticalOffset, 190, 20), isOdd ? colorOdd : colorEven, false, (HorizontalAlignment)0, (VerticalAlignment)0);
				verticalOffset += 20;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(10, verticalOffset, ((Control)this).get_Width() - 20, 1), isOdd ? colorOdd : colorEven);
				isOdd = !isOdd;
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _bestScoreStat.ScoreText, Control.get_Content().get_DefaultFont14(), new Rectangle(5, ((Control)this).get_Height() - 20, ((Control)this).get_Width() - 10, 20), Color.get_LightGoldenrodYellow(), false, (HorizontalAlignment)0, (VerticalAlignment)0);
			DrawBoarder(spriteBatch);
		}

		protected void DrawBoarder(SpriteBatch spriteBatch)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			Color color = Color.get_LightGoldenrodYellow();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, ((Control)this).get_Width(), 3), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, ((Control)this).get_Height() - 3, ((Control)this).get_Width(), ((Control)this).get_Height()), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, 3, ((Control)this).get_Height()), color);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Control)this).get_Width() - 3, 0, ((Control)this).get_Width(), ((Control)this).get_Height()), color);
		}
	}
}
