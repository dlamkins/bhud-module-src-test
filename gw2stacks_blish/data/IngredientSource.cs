namespace gw2stacks_blish.data
{
	internal class IngredientSource : Source
	{
		public int id;

		public IngredientSource(ulong count_, int id_)
			: base(count_, null)
		{
			id = id_;
		}

		public override string ToString()
		{
			return count + " x " + Magic.get_local_name(id);
		}
	}
}
