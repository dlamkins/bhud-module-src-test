using System;
using System.Collections.Generic;

namespace Ideka.CustomCombatText
{
	public class MessageViewSet : DoubleSortedSet<MessageView>
	{
		private class MessageViewValueComparer : IComparer<MessageView>
		{
			public int Compare(MessageView a, MessageView b)
			{
				return MessageViewSet.Compare(a, b, (Func<MessageView, int>)((MessageView x) => x.Value));
			}
		}

		private class MessageViewLastTimeComparer : IComparer<MessageView>
		{
			public int Compare(MessageView a, MessageView b)
			{
				return MessageViewSet.Compare(a, b, (Func<MessageView, TimeSpan>)((MessageView x) => x.LastTime));
			}
		}

		public IEnumerable<MessageView> LowToHigh => SortedA();

		public IEnumerable<MessageView> HighToLow => ReverseA();

		public IEnumerable<MessageView> OldToNew => SortedB();

		public IEnumerable<MessageView> NewToOld => ReverseB();

		private static int Compare<T>(MessageView a, MessageView b, Func<MessageView, T> getter) where T : IComparable<T>
		{
			if (a == b)
			{
				return 0;
			}
			int sign2 = getter(a).CompareTo(getter(b));
			if (sign2 != 0)
			{
				return sign2;
			}
			int sign = a.GetHashCode() - b.GetHashCode();
			if (sign != 0)
			{
				return sign;
			}
			return -1;
		}

		public MessageViewSet()
			: base((IComparer<MessageView>)new MessageViewValueComparer(), (IComparer<MessageView>)new MessageViewLastTimeComparer())
		{
		}
	}
}
