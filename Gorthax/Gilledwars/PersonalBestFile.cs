using System.Collections.Generic;

namespace Gorthax.Gilledwars
{
	public class PersonalBestFile
	{
		public int Version { get; set; } = 1;


		public Dictionary<int, PersonalBestRecord> Records { get; set; } = new Dictionary<int, PersonalBestRecord>();

	}
}
