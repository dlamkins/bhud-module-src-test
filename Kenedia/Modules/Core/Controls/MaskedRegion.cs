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
			base.SpriteBatchParameters = new SpriteBatchParameters((SpriteSortMode)0, BlendState.Opaque);
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
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			Rectangle b = default(Rectangle);
			((Rectangle)(ref b))._002Ector(base.Location, base.Size);
			spriteBatch.Draw(ContentService.Textures.TransparentPixel, b, Color.get_Transparent());
			spriteBatch.End();
			spriteBatch.Begin(_batchParameters);
		}
	}
}
