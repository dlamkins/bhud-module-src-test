using Blish_HUD.Controls;

namespace felix.BlishEmotes.UI.Controls
{
	internal class ReorderableMenuItem : MenuItem
	{
		private bool _canDrag = true;

		private bool _dragging;

		public bool CanDrag
		{
			get
			{
				return _canDrag;
			}
			internal set
			{
				SetProperty(ref _canDrag, value, invalidateLayout: true, "CanDrag");
			}
		}

		public bool Dragging
		{
			get
			{
				return _dragging;
			}
			internal set
			{
				SetProperty(ref _dragging, value, invalidateLayout: true, "Dragging");
			}
		}

		public ReorderableMenuItem()
		{
		}

		public ReorderableMenuItem(string text)
			: base(text)
		{
		}
	}
}
