using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace Nekres.Music_Mixer.Core.UI.Controls
{
	public class TrackBarWithDraggingStopped : TrackBar
	{
		private float _dragStartValue;

		public event EventHandler<ValueEventArgs<float>> DraggingStopped;

		public TrackBarWithDraggingStopped()
			: this()
		{
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonReleased(e);
			if (!((TrackBar)this).get_Dragging() && Math.Abs(_dragStartValue - ((TrackBar)this).get_Value()) > 0.01f)
			{
				this.DraggingStopped?.Invoke(this, new ValueEventArgs<float>(((TrackBar)this).get_Value()));
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			((TrackBar)this).OnLeftMouseButtonPressed(e);
			if (((TrackBar)this).get_Dragging())
			{
				_dragStartValue = ((TrackBar)this).get_Value();
			}
		}
	}
}
