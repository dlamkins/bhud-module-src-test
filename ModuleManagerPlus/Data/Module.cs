using System.Collections.Generic;
using Newtonsoft.Json;

namespace ModuleManagerPlus.Data
{
	internal class Module
	{
		private Author _author;

		[JsonProperty("Namespace")]
		public string Namespace { get; set; }

		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("Description")]
		public string Description { get; set; }

		[JsonProperty("HeroUrl")]
		public string HeroUrl { get; set; }

		[JsonProperty("HasMoreInfo")]
		public bool HasMoreInfo { get; set; }

		[JsonProperty("TotalDownloads")]
		public int TotalDownloads { get; set; }

		[JsonProperty("Releases")]
		public List<Release> Releases { get; set; }

		[JsonProperty("AuthorId")]
		public string AuthorId { get; set; }

		public Author Author(PkgRoot root)
		{
			return _author ?? (_author = root.Authors[AuthorId]);
		}
	}
}
