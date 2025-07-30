using System;
using Newtonsoft.Json;

namespace Soeed.WhatRoleAmIPlaying.Models
{
	[Serializable]
	public class ProfessionInfo
	{
		[JsonProperty("name")]
		public string Name { get; set; } = string.Empty;


		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("icon")]
		public string Icon { get; set; } = string.Empty;

	}
}
