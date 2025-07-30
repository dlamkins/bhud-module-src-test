using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Soeed.WhatRoleAmIPlaying.Models
{
	[Serializable]
	public class RoleConfig
	{
		[JsonProperty("version")]
		public string Version { get; set; } = "0.1.0";


		[JsonProperty("last_updated")]
		public string LastUpdated { get; set; } = string.Empty;


		[JsonProperty("roles")]
		public List<RoleSuggestion> Roles { get; set; } = new List<RoleSuggestion>();


		[JsonProperty("professions")]
		public List<ProfessionInfo> Professions { get; set; } = new List<ProfessionInfo>();


		[JsonProperty("elite_specs")]
		public List<EliteSpecInfo> EliteSpecs { get; set; } = new List<EliteSpecInfo>();

	}
}
