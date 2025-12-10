using System.Collections.Generic;
using Kenedia.Modules.Core.Converter;
using Newtonsoft.Json;
using SemVer;

namespace Kenedia.Modules.Core.Models
{
	public class DataDictionaryDto<TKey, TValue>
	{
		[JsonConverter(typeof(SemverVersionConverter))]
		public Version Version { get; set; } = new Version("0.0.0", false);


		public Dictionary<TKey, TValue> Data { get; set; }
	}
}
