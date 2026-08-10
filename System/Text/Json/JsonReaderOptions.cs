namespace System.Text.Json
{
	internal struct JsonReaderOptions
	{
		internal const int DefaultMaxDepth = 64;

		private int _maxDepth;

		private JsonCommentHandling _commentHandling;

		public JsonCommentHandling CommentHandling
		{
			readonly get
			{
				return _commentHandling;
			}
			set
			{
				if ((int)value > 2)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException_CommentEnumMustBeInRange("value");
				}
				_commentHandling = value;
			}
		}

		public int MaxDepth
		{
			readonly get
			{
				return _maxDepth;
			}
			set
			{
				if (value < 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException_MaxDepthMustBePositive("value");
				}
				_maxDepth = value;
			}
		}

		public bool AllowTrailingCommas { get; set; }
	}
}
