namespace gw2stacks_blish.data
{
	internal class Source
	{
		public ulong count;

		public string place;

		public ulong stacks;

		public Source(ulong count_, string place_)
		{
			count = count_;
			place = place_;
			stacks = 1uL;
		}

		public override string ToString()
		{
			return count + " @ " + Magic.get_local_storage_name(place);
		}
	}
}
