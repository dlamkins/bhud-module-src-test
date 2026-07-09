using System.Collections.Generic;
using rp.spark.Models;

namespace rp.spark.Services
{
	public class ProfileImportGroup
	{
		public string CharacterName { get; set; } = string.Empty;


		public List<CharacterProfile> Profiles { get; set; } = new List<CharacterProfile>();

	}
}
