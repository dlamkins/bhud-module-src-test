using System;
using Newtonsoft.Json;

namespace Soeed.WhatRoleAmIPlaying.Models
{
	[Serializable]
	public class EliteSpecInfo
	{
		[JsonProperty("name")]
		public string Name { get; set; } = string.Empty;


		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("profession")]
		public string Profession { get; set; } = string.Empty;


		[JsonProperty("icon")]
		public string Icon { get; set; } = string.Empty;


		[JsonProperty("background")]
		public string Background { get; set; } = string.Empty;

	}
}
