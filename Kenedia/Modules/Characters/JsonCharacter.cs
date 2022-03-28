using System;
using System.Collections.Generic;
using Gw2Sharp.Models;

namespace Kenedia.Modules.Characters
{
	public class JsonCharacter
	{
		public DateTime lastLogin;

		public DateTime LastModified;

		public DateTimeOffset Created;

		public List<CharacterCrafting> Crafting;

		public int map;

		public string Tags;

		public string Name { get; set; }

		public RaceType Race { get; set; }

		public int Profession { get; set; }

		public int apiIndex { get; set; }

		public int Specialization { get; set; }

		public int Level { get; set; }
	}
}
