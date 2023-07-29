using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.ProofLogix.Core.UI.KpProfile
{
	public class RefreshButton : Control
	{
		private DateTime _nextRefresh;

		private AsyncTexture2D _tex;

		private AsyncTexture2D _hoverTex;

		private AsyncTexture2D _blockedTex;

		private bool _isHovering;

		public DateTime NextRefresh
		{
			get
			{
				return _nextRefresh;
			}
			set
			{
				((Control)this).SetProperty<DateTime>(ref _nextRefresh, value, false, "NextRefresh");
			}
		}

		public RefreshButton()
			: this()
		{
			_nextRefresh = DateTime.UtcNow;
			_tex = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(784346);
			_hoverTex = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156330);
			_blockedTex = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(851256);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			_isHovering = false;
			((Control)this).OnMouseMoved(e);
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			_isHovering = true;
			((Control)this).set_BasicTooltipText("Refresh");
			((Control)this).OnMouseMoved(e);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			TimeSpan remainingTime = NextRefresh.Subtract(DateTime.UtcNow);
			if (remainingTime.Ticks > 0)
			{
				if (_isHovering)
				{
					string minutes = ((remainingTime.TotalMinutes > 1.0) ? "minutes" : "minute");
					string seconds = ((remainingTime.TotalSeconds > 1.0) ? "seconds" : "second");
					string timeSuffix = ((remainingTime.TotalMinutes > 0.0) ? minutes : seconds);
					((Control)this).set_BasicTooltipText($"Refresh\nNext refresh available in {remainingTime:m\\:ss} {timeSuffix}.");
				}
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_blockedTex), bounds);
			}
			else
			{
				((Control)this).set_BasicTooltipText("Refresh");
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_isHovering ? _hoverTex : _tex), bounds, (remainingTime.Ticks > 0) ? (Color.get_White() * 0.7f) : Color.get_White());
		}
	}
}
