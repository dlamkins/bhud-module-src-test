namespace gw2stacks_blish.data
{
	internal class Source
	{
		public ulong count;

		public string place;

		public Source(ulong count_, string place_)
		{
			count = count_;
			place = place_;
		}

		public override string ToString()
		{
			return count + " @ " + Magic.get_local_storage_name(place);
		}
	}
}
