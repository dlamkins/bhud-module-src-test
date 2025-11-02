namespace gw2stacks_blish.data
{
	internal class InventoryBagSlot
	{
		public int id;

		public int size;

		public InventoryBagSlot(int id_, int size_ = 0)
		{
			id = id_;
			size = size_;
		}

		public int get_size()
		{
			return size;
		}

		public int get_id()
		{
			return id;
		}
	}
}
