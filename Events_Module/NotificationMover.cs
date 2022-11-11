using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Events_Module
{
	public class NotificationMover : Control
	{
		private const int HANDLE_SIZE = 40;

		private readonly SpriteBatchParameters _clearDrawParameters;

		private readonly ScreenRegion[] _screenRegions;

		private ScreenRegion _activeScreenRegion;

		private Point _grabPosition = Point.get_Zero();

		private readonly Texture2D _handleTexture;

		public NotificationMover(params ScreenRegion[] screenPositions)
			: this(screenPositions.ToList())
		{
		}

		public NotificationMover(IEnumerable<ScreenRegion> screenPositions)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			((Control)this).set_ZIndex(2147483637);
			_clearDrawParameters = new SpriteBatchParameters((SpriteSortMode)0, BlendState.Opaque, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			_screenRegions = screenPositions.ToArray();
			_handleTexture = EventsModule.ModuleInstance.ContentsManager.GetTexture("textures/handle.png");
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (_activeScreenRegion != null)
			{
				_grabPosition = ((Control)this).get_RelativeMousePosition();
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			if (GameService.Input.get_Keyboard().get_KeysDown().Contains((Keys)27))
			{
				((Control)this).Dispose();
			}
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_grabPosition = Point.get_Zero();
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			if (_grabPosition != Point.get_Zero() && _activeScreenRegion != null)
			{
				Point lastPos = _grabPosition;
				_grabPosition = ((Control)this).get_RelativeMousePosition();
				ScreenRegion activeScreenRegion = _activeScreenRegion;
				activeScreenRegion.Location += _grabPosition - lastPos;
				return;
			}
			ScreenRegion[] screenRegions = _screenRegions;
			foreach (ScreenRegion region in screenRegions)
			{
				Rectangle bounds = region.Bounds;
				if (((Rectangle)(ref bounds)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					_activeScreenRegion = region;
					return;
				}
			}
			_activeScreenRegion = null;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * 0.8f);
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, _clearDrawParameters);
			ScreenRegion[] screenRegions = _screenRegions;
			foreach (ScreenRegion region in screenRegions)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_TransparentPixel(), region.Bounds, Color.get_Transparent());
			}
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, ((Control)this).get_SpriteBatchParameters());
			screenRegions = _screenRegions;
			foreach (ScreenRegion region2 in screenRegions)
			{
				if (region2 == _activeScreenRegion)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(region2.Location, region2.Size), Color.get_White() * 0.5f);
				}
				Texture2D handleTexture = _handleTexture;
				Rectangle bounds2 = region2.Bounds;
				int left = ((Rectangle)(ref bounds2)).get_Left();
				bounds2 = region2.Bounds;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, handleTexture, new Rectangle(left, ((Rectangle)(ref bounds2)).get_Top(), 40, 40), (Rectangle?)_handleTexture.get_Bounds(), Color.get_White() * 0.6f);
				Texture2D handleTexture2 = _handleTexture;
				bounds2 = region2.Bounds;
				int num = ((Rectangle)(ref bounds2)).get_Right() - 20;
				bounds2 = region2.Bounds;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, handleTexture2, new Rectangle(num, ((Rectangle)(ref bounds2)).get_Top() + 20, 40, 40), (Rectangle?)_handleTexture.get_Bounds(), Color.get_White() * 0.6f, (float)Math.PI / 2f, new Vector2(20f, 20f), (SpriteEffects)0);
				Texture2D handleTexture3 = _handleTexture;
				bounds2 = region2.Bounds;
				int num2 = ((Rectangle)(ref bounds2)).get_Left() + 20;
				bounds2 = region2.Bounds;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, handleTexture3, new Rectangle(num2, ((Rectangle)(ref bounds2)).get_Bottom() - 20, 40, 40), (Rectangle?)_handleTexture.get_Bounds(), Color.get_White() * 0.6f, 4.712389f, new Vector2(20f, 20f), (SpriteEffects)0);
				Texture2D handleTexture4 = _handleTexture;
				bounds2 = region2.Bounds;
				int num3 = ((Rectangle)(ref bounds2)).get_Right() - 20;
				bounds2 = region2.Bounds;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, handleTexture4, new Rectangle(num3, ((Rectangle)(ref bounds2)).get_Bottom() - 20, 40, 40), (Rectangle?)_handleTexture.get_Bounds(), Color.get_White() * 0.6f, (float)Math.PI, new Vector2(20f, 20f), (SpriteEffects)0);
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Press ESC to close.", GameService.Content.get_DefaultFont32(), bounds, Color.get_White(), false, (HorizontalAlignment)1, (VerticalAlignment)1);
		}
	}
}
