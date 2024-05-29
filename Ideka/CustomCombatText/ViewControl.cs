using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Ideka.CustomCombatText
{
	public class ViewControl : Control
	{
		private readonly AreaViewBase _container;

		public ViewControl(AreaViewBase container)
			: this()
		{
			_container = container;
			((Control)this).set_ClipsBounds(false);
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)22;
		}

		public void ReceiveMessage(Message message)
		{
			foreach (AreaView areaViewChild in _container.GetAreaViewChildren())
			{
				areaViewChild.ReceiveMessage(message);
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			_container.Update(gameTime);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.Gw2Mumble.get_UI().get_IsMapOpen() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				_container.Draw(spriteBatch, (Control)(object)this, _container.Target(RectangleF.op_Implicit(((Control)Control.get_Graphics().get_SpriteScreen()).get_AbsoluteBounds())));
			}
		}
	}
}
