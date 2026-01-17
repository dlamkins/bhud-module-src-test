namespace NpcFinder.Util
{
	public static class ContinentNames
	{
		public static string Name(int id)
		{
			return id switch
			{
				1 => "Tyria (Central Tyria / Kryta)", 
				2 => "The Mists", 
				3 => "Elona", 
				4 => "Cantha", 
				_ => "Continent " + id, 
			};
		}
	}
}
