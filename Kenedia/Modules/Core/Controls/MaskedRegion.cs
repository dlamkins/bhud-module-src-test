using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class MaskedRegion : Control
	{
		private readonly SpriteBatchParameters _batchParameters;

		public bool CaptureInput { get; set; }

		public MaskedRegion()
		{
			ZIndex = int.MaxValue;
			_batchParameters = base.SpriteBatchParameters;
			base.SpriteBatchParameters = new SpriteBatchParameters(SpriteSortMode.Deferred, BlendState.Opaque);
		}

		protected override CaptureType CapturesInput()
		{
			if (!CaptureInput)
			{
				return CaptureType.None;
			}
			return base.CapturesInput();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			spriteBatch.Draw(destinationRectangle: new Rectangle(base.Location, base.Size), texture: ContentService.Textures.TransparentPixel, color: Color.Transparent);
			spriteBatch.End();
			spriteBatch.Begin(_batchParameters);
		}
	}
}
