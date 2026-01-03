using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LoreBridge.Modules.AreaTranslation.Controls
{
	public sealed class ScreenDetectionRegion : Control
	{
		public Color BorderColor { get; set; } = Color.get_Red();


		public int BorderWidth { get; set; } = 2;


		public ScreenDetectionRegion()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_ZIndex(int.MaxValue);
			((Control)this).set_Enabled(false);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			Rectangle rect = default(Rectangle);
			((Rectangle)(ref rect))._002Ector(((Control)this).get_Location(), ((Control)this).get_Size());
			spriteBatch.Draw(Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width + 2 * BorderWidth, BorderWidth), BorderColor);
			spriteBatch.Draw(Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - BorderWidth, rect.Width + 2 * BorderWidth, BorderWidth), BorderColor);
			spriteBatch.Draw(Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), BorderWidth, rect.Height + 2 * BorderWidth), BorderColor);
			spriteBatch.Draw(Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - BorderWidth, ((Rectangle)(ref rect)).get_Top(), BorderWidth, rect.Height + 2 * BorderWidth), BorderColor);
		}
	}
}
