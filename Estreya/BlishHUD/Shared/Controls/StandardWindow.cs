using Blish_HUD.Content;
using Estreya.BlishHUD.Shared.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls
{
	public class StandardWindow : Window
	{
		public StandardWindow(BaseModuleSettings baseModuleSettings, AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: base(baseModuleSettings)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			ConstructWindow(background, windowRegion, contentRegion);
		}

		public StandardWindow(BaseModuleSettings baseModuleSettings, Texture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(baseModuleSettings, AsyncTexture2D.op_Implicit(background), windowRegion, contentRegion)
		{
		}//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)


		public StandardWindow(BaseModuleSettings baseModuleSettings, AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: base(baseModuleSettings)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			ConstructWindow(background, windowRegion, contentRegion, windowSize);
		}

		public StandardWindow(BaseModuleSettings baseModuleSettings, Texture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: this(baseModuleSettings, AsyncTexture2D.op_Implicit(background), windowRegion, contentRegion, windowSize)
		{
		}//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)

	}
}
