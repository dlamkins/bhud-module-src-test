using System.Collections.Generic;

namespace GW2app
{
	internal class IncomingMessage
	{
		public MessageKind Kind;

		public StateMessage State;

		public EntryMessage Entry;

		public List<string> SyncedListIds;

		public HoverImageMessage HoverImage;
	}
}
