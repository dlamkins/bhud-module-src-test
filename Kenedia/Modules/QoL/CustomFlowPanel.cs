using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.QoL
{
	public class CustomFlowPanel : FlowPanel
	{
		private bool Dragging;

		private Point DraggingStart;

		public Texture2D Background;

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			Dragging = (int)Control.get_Input().get_Keyboard().get_ActiveModifiers() == 2;
			DraggingStart = (Dragging ? ((Control)this).get_RelativeMousePosition() : Point.get_Zero());
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonPressed(e);
			Dragging = false;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).UpdateContainer(gameTime);
			if (Dragging)
			{
				((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position()
					.Add(new Point(-DraggingStart.X, -DraggingStart.Y)));
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		protected override void DisposeControl()
		{
			((Panel)this).DisposeControl();
			Background = null;
		}

		public CustomFlowPanel()
			: this()
		{
		}
	}
}
