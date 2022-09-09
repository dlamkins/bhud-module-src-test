using Newtonsoft.Json;

namespace Nekres.Mumble_Info.Core.Services.Models
{
	internal class Identity
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("profession")]
		public uint Profession { get; set; }

		[JsonProperty("spec")]
		public uint Spec { get; set; }

		[JsonProperty("race")]
		public uint Race { get; set; }

		[JsonProperty("map_id")]
		public uint MapId { get; set; }

		[JsonProperty("world_id")]
		public uint WorldId { get; set; }

		[JsonProperty("team_color_id")]
		public uint TeamColorId { get; set; }

		[JsonProperty("commander")]
		public bool Commander { get; set; }

		[JsonProperty("fov")]
		public float Fov { get; set; }

		[JsonProperty("uisz")]
		public uint Uisz { get; set; }

		public Identity()
		{
			Name = "Dummy";
			Profession = 4u;
			Spec = 55u;
			Race = 4u;
			MapId = 50u;
			WorldId = 268435505u;
			TeamColorId = 0u;
			Commander = false;
			Fov = 0.873f;
			Uisz = 1u;
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject((object)this);
		}
	}
}
