using System.Collections.Generic;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.Core.Converter;
using Newtonsoft.Json;
using SemVer;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class StaticStats
	{
		public List<Stat.StatTextureMapInfo> TextureMapInfo { get; set; }

		public string ImageUrl { get; set; }

		[JsonConverter(typeof(SemverVersionConverter))]
		public Version Version { get; set; } = new Version(0, 0, 0, (string)null, (string)null);

	}
}
