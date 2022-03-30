using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Inquest_Module.UI.Controls
{
	internal sealed class ClickIndicator : Control
	{
		private static Texture2D _mouseIcon = GameService.Content.GetTexture("156734");

		private static Texture2D _stopIcon = GameService.Content.GetTexture("common/154982");

		private static Texture2D _mouseIdleTex = InquestModule.ModuleInstance.ContentsManager.GetTexture("mouse-idle.png");

		private static Texture2D _mouseLeftClickTex = InquestModule.ModuleInstance.ContentsManager.GetTexture("mouse-left-click.png");

		private bool _paused;

		private bool _leftClick;

		private DateTime _clickEnd;

		public bool Paused
		{
			get
			{
				return _paused;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _paused, value, false, "Paused");
			}
		}

		public ClickIndicator()
			: this()
		{
			((Control)this).set_ZIndex(1000);
		}

		public void LeftClick()
		{
			_leftClick = true;
			_clickEnd = DateTime.UtcNow.AddMilliseconds(150.0);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)1;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			if (_paused)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _mouseIcon, new Rectangle((bounds.Width - _mouseIcon.get_Width()) / 2, (bounds.Height - _mouseIcon.get_Height()) / 2, _mouseIcon.get_Width(), _mouseIcon.get_Height()), (Rectangle?)_mouseIcon.get_Bounds());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _stopIcon, new Rectangle((bounds.Width - _stopIcon.get_Width()) / 2, (bounds.Height - _stopIcon.get_Height()) / 2, _stopIcon.get_Width(), _stopIcon.get_Height()), (Rectangle?)_stopIcon.get_Bounds(), Color.get_White() * 0.65f);
			}
			else if (_leftClick && DateTime.UtcNow < _clickEnd)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _mouseLeftClickTex, new Rectangle((bounds.Width - _mouseIdleTex.get_Width()) / 2, (bounds.Height - _mouseIdleTex.get_Height()) / 2, _mouseIdleTex.get_Width(), _mouseIdleTex.get_Height()), (Rectangle?)_mouseIdleTex.get_Bounds());
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _mouseIdleTex, new Rectangle((bounds.Width - _mouseIdleTex.get_Width()) / 2, (bounds.Height - _mouseIdleTex.get_Height()) / 2, _mouseIdleTex.get_Width(), _mouseIdleTex.get_Height()), (Rectangle?)_mouseIdleTex.get_Bounds());
			}
		}
	}
}
