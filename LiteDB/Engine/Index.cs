using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal abstract class Index
	{
		public string Name { get; private set; }

		public int Order { get; set; }

		internal Index(string name, int order)
		{
			Name = name;
			Order = order;
		}

		public abstract uint GetCost(CollectionIndex index);

		public abstract IEnumerable<IndexNode> Execute(IndexService indexer, CollectionIndex index);

		public virtual IEnumerable<IndexNode> Run(CollectionPage col, IndexService indexer)
		{
			CollectionIndex index = col.GetCollectionIndex(Name);
			if (index == null)
			{
				throw LiteException.IndexNotFound(Name);
			}
			return Execute(indexer, index).DistinctBy((IndexNode x) => x.DataBlock, null);
		}
	}
}
