using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.Mounts.Controls
{
	internal class DebugControl : Control
	{
		public IEnumerable<string> StringsToDisplay { get; set; }

		public DebugControl()
			: this()
		{
			((Control)this).set_Visible(true);
			StringsToDisplay = new List<string>();
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			int i = 0;
			foreach (string item in StringsToDisplay)
			{
				DrawDbg(spriteBatch, i, item);
				i += 30;
			}
		}

		private void DrawDbg(SpriteBatch spriteBatch, int position, string s)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, s, GameService.Content.get_DefaultFont32(), new Rectangle(new Point(0, position), new Point(400, 400)), Color.get_Red(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
