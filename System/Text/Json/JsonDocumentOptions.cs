namespace System.Text.Json
{
	internal struct JsonDocumentOptions
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
				if ((int)value > 1)
				{
					throw new ArgumentOutOfRangeException("value", _003C040126af_002Deea0_002D4cbb_002Db0ec_002D5483efb85291_003ESR.JsonDocumentDoesNotSupportComments);
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

		internal JsonReaderOptions GetReaderOptions()
		{
			JsonReaderOptions result = default(JsonReaderOptions);
			result.AllowTrailingCommas = AllowTrailingCommas;
			result.CommentHandling = CommentHandling;
			result.MaxDepth = MaxDepth;
			return result;
		}
	}
}
