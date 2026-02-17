using System;

namespace BhModule.WebPeeper
{
	internal class BookmarkDraggedEventArgs : EventArgs
	{
		public readonly Bookmark Bookmark;

		public readonly int Position;

		public BookmarkDraggedEventArgs(Bookmark bookmark, int position)
		{
			Bookmark = bookmark;
			Position = position;
			base._002Ector();
		}
	}
}
