namespace LiteDB
{
	internal struct LastItem<T>
	{
		public T Item { get; set; }

		public bool IsLast { get; set; }
	}
}
