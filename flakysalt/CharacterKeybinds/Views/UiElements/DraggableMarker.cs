using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace flakysalt.CharacterKeybinds.Views.UiElements
{
	internal sealed class DraggableMarker : Container
	{
		private Point _startDragMouseOffset;

		private bool _dragging;

		private const int MarkerTextureId = 1863840;

		public event EventHandler<Point> OnMarkerReleased;

		public DraggableMarker(int order = 0)
			: this()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
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

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).UpdateContainer(gameTime);
			if (_dragging)
			{
				((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position() + _startDragMouseOffset);
			}
		}

		public void SimulateClick()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			Point offset = CalculateClickOffset();
			Mouse.Click((MouseButton)0, ((Control)this).get_Location().X - offset.X, ((Control)this).get_Location().Y - offset.Y, false);
		}

		private void DragMarker_LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			_dragging = false;
			this.OnMarkerReleased?.Invoke(this, ((Control)this).get_Location());
		}

		private void Image_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			_startDragMouseOffset = ((Control)this).get_Location() - Control.get_Input().get_Mouse().get_Position();
			_dragging = true;
		}

		private Point CalculateClickOffset()
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
	}
}
