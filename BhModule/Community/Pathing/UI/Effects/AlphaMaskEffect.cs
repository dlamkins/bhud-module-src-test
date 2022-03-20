using Blish_HUD.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.UI.Effects
{
	public class AlphaMaskEffect : SharedEffect
	{
		private static readonly AlphaMaskEffect _sharedInstance = new AlphaMaskEffect(PathingModule.Instance.ContentsManager.GetEffect("hlsl\\alphamask.mgfx"));

		private const string PARAMETER_MASK = "Mask";

		private Texture2D _mask;

		public static AlphaMaskEffect SharedInstance => _sharedInstance;

		public Texture2D Mask
		{
			get
			{
				return _mask;
			}
			set
			{
				((SharedEffect)this).SetParameter("Mask", ref _mask, value);
			}
		}

		public AlphaMaskEffect(Effect cloneSource)
			: this(cloneSource)
		{
		}

		public AlphaMaskEffect(GraphicsDevice graphicsDevice, byte[] effectCode)
			: this(graphicsDevice, effectCode)
		{
		}

		public AlphaMaskEffect(GraphicsDevice graphicsDevice, byte[] effectCode, int index, int count)
			: this(graphicsDevice, effectCode, index, count)
		{
		}

		public void SetEffectState(Texture2D mask)
		{
			Mask = mask;
		}

		protected override void Update(GameTime gameTime)
		{
		}
	}
}
