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
			: this()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			((Control)this).set_ZIndex(int.MaxValue);
			_batchParameters = ((Control)this).get_SpriteBatchParameters();
			((Control)this).set_SpriteBatchParameters(new SpriteBatchParameters((SpriteSortMode)0, BlendState.Opaque, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null));
		}

		protected override CaptureType CapturesInput()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (!CaptureInput)
			{
				return (CaptureType)0;
			}
			return ((Control)this).CapturesInput();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			Rectangle b = default(Rectangle);
			((Rectangle)(ref b))._002Ector(((Control)this).get_Location(), ((Control)this).get_Size());
			spriteBatch.Draw(Textures.get_TransparentPixel(), b, Color.get_Transparent());
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, _batchParameters);
		}
	}
}
