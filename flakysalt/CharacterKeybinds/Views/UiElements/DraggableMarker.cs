using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace flakysalt.CharacterKeybinds.Views.UiElements
{
	internal class DraggableMarker : Container
	{
		private Point startDragMouseOffset;

		private bool Dragging;

		public event EventHandler<Point> OnMarkerReleased;

		public DraggableMarker(int order = 0)
			: this()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D obj = AsyncTexture2D.FromAssetId(1863840);
			((Control)this).set_Width(32);
			((Control)this).set_Height(32);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_ZIndex(1000);
			Image val = new Image(obj);
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(((Control)this).get_Size());
			Label val2 = new Label();
			val2.set_Text((order == 0) ? "" : order.ToString());
			val2.set_TextColor(Color.get_White());
			val2.set_ShadowColor(Color.get_Black());
			val2.set_ShowShadow(true);
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Size(((Control)this).get_Size());
			val2.set_HorizontalAlignment((HorizontalAlignment)2);
			val2.set_VerticalAlignment((VerticalAlignment)0);
			((Control)this).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)Image_LeftMouseButtonPressed);
			((Control)this).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)DragMarker_LeftMouseButtonReleased);
		}

		private void DragMarker_LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Dragging = false;
			this.OnMarkerReleased(this, ((Control)this).get_Location());
		}

		private void Image_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			startDragMouseOffset = ((Control)this).get_Location() - Control.get_Input().get_Mouse().get_Position();
			Dragging = true;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).UpdateContainer(gameTime);
			if (Dragging)
			{
				((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position() + startDragMouseOffset);
			}
		}

		public Point CalculateClickOffset()
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			Point offset = default(Point);
			((Point)(ref offset))._002Ector((int)((float)((Control)this).get_Location().X * GameService.Graphics.get_UIScaleMultiplier()), (int)((float)((Control)this).get_Location().Y * GameService.Graphics.get_UIScaleMultiplier()));
			return ((Control)this).get_Location() - offset;
		}

		public void SimulateClick()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			Mouse.Click((MouseButton)0, ((Control)this).get_Location().X - CalculateClickOffset().X, ((Control)this).get_Location().Y - CalculateClickOffset().Y, false);
		}
	}
}
