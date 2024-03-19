using System;
using System.Diagnostics;

namespace LiteDB
{
	internal class Constants
	{
		public const int PAGE_SIZE = 8192;

		public const int PAGE_HEADER_SIZE = 32;

		public const int ENCRYPTION_SALT_SIZE = 16;

		public static int BUFFER_WRITABLE = -1;

		public static int INDEX_NAME_MAX_LENGTH = 32;

		public const int MAX_LEVEL_LENGTH = 32;

		public const int MAX_INDEX_KEY_LENGTH = 1023;

		public const int MAX_INDEX_LENGTH = 1400;

		public const int PAGE_FREE_LIST_SLOTS = 5;

		public const int MAX_DOCUMENT_SIZE = 16683050;

		public const int MAX_OPEN_TRANSACTIONS = 100;

		public const int MAX_TRANSACTION_SIZE = 100000;

		public static int[] MEMORY_SEGMENT_SIZES = new int[5] { 12, 50, 100, 500, 1000 };

		public const int VIRTUAL_INDEX_MAX_CACHE = 2000;

		public const int CONTAINER_SORT_SIZE = 819200;

		[DebuggerHidden]
		[Conditional("DEBUG")]
		public static void LOG(string message, string category)
		{
		}

		[DebuggerHidden]
		[Conditional("DEBUG")]
		public static void LOG(bool conditional, string message, string category)
		{
		}

		[DebuggerHidden]
		public static void ENSURE(bool conditional, string message = null)
		{
			if (!conditional)
			{
				_ = Debugger.IsAttached;
				throw new Exception("LiteDB ENSURE: " + message);
			}
		}

		[DebuggerHidden]
		public static void ENSURE(bool ifTest, bool conditional, string message = null)
		{
			if (ifTest && !conditional)
			{
				_ = Debugger.IsAttached;
				throw new Exception("LiteDB ENSURE: " + message);
			}
		}

		[DebuggerHidden]
		[Conditional("DEBUG")]
		public static void DEBUG(bool conditional, string message = null)
		{
			if (!conditional)
			{
				_ = Debugger.IsAttached;
				throw new Exception("LiteDB DEBUG: " + message);
			}
		}
	}
}
