using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Inquest_Module.UI.Controls
{
	internal sealed class ClickIndicator : TaskIndicatorBase
	{
		private static readonly Texture2D MouseIdleTex = InquestModule.ModuleInstance.ContentsManager.GetTexture("mouse-idle.png");

		private static readonly Texture2D MouseLeftClickTex = InquestModule.ModuleInstance.ContentsManager.GetTexture("mouse-left-click.png");

		private DateTime _clickEnd;

		public ClickIndicator(bool attachToCursor = true)
			: base(attachToCursor)
		{
		}

		public void LeftClick(int durationMs = 150)
		{
			_clickEnd = DateTime.UtcNow.AddMilliseconds(durationMs);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			base.Paint(spriteBatch, bounds);
			if (!base.Paused)
			{
				if (DateTime.UtcNow < _clickEnd)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, MouseLeftClickTex, new Rectangle((bounds.Width - MouseLeftClickTex.get_Width()) / 2, (bounds.Height - MouseLeftClickTex.get_Height()) / 2, MouseLeftClickTex.get_Width(), MouseLeftClickTex.get_Height()), (Rectangle?)MouseLeftClickTex.get_Bounds());
				}
				else
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, MouseIdleTex, new Rectangle((bounds.Width - MouseIdleTex.get_Width()) / 2, (bounds.Height - MouseIdleTex.get_Height()) / 2, MouseIdleTex.get_Width(), MouseIdleTex.get_Height()), (Rectangle?)MouseIdleTex.get_Bounds());
				}
			}
		}
	}
}
