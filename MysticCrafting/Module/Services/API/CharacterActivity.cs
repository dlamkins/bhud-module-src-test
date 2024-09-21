using System;

namespace MysticCrafting.Module.Services.API
{
	public class CharacterActivity
	{
		public string CharacterName { get; set; }

		public DateTime LastActive { get; set; }

		public DateTime LastDataUpdate { get; set; }
	}
}
