using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class LeaderboardStatControl : Control, IDisposable
	{
		protected LeaderboardStat _lbStat;

		protected string _statTitle;

		protected const int BORDER_SIZE = 3;

		public LeaderboardStatControl(LeaderboardStat stat)
			: this()
		{
			_lbStat = stat;
			((Control)this).set_Width(400);
			((Control)this).set_Height(270);
		}

		protected override void DisposeControl()
		{
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			Color color = Color.get_LightGoldenrodYellow();
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _lbStat.Title, Control.get_Content().get_DefaultFont18(), new Rectangle(5, 5, 390, 30), color, false, (HorizontalAlignment)0, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Account", Control.get_Content().get_DefaultFont16(), new Rectangle(5, 35, 190, 20), Color.get_WhiteSmoke(), false, (HorizontalAlignment)0, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _lbStat.ValueHeader, Control.get_Content().get_DefaultFont16(), new Rectangle(205, 35, 190, 20), Color.get_WhiteSmoke(), false, (HorizontalAlignment)0, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(5, 55, ((Control)this).get_Width() - 10, 1), Color.get_WhiteSmoke());
			int verticalOffset = 60;
			Color colorOdd = Color.get_White();
			Color colorEven = Color.get_Wheat();
			bool isOdd = false;
			foreach (LeaderboardEntry entry in _lbStat.Entries)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, entry.Author, Control.get_Content().get_DefaultFont16(), new Rectangle(10, verticalOffset, 180, 20), isOdd ? colorOdd : colorEven, false, (HorizontalAlignment)0, (VerticalAlignment)0);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, entry.Value.ToString(), Control.get_Content().get_DefaultFont16(), new Rectangle(210, verticalOffset, 180, 20), isOdd ? colorOdd : colorEven, false, (HorizontalAlignment)0, (VerticalAlignment)0);
				verticalOffset += 20;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(10, verticalOffset, ((Control)this).get_Width() - 20, 1), isOdd ? colorOdd : colorEven);
				isOdd = !isOdd;
			}
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
