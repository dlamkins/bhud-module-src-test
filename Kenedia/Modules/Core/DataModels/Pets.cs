using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Kenedia.Modules.Core.DataModels
{
	public class Pets : List<int?>
	{
		[JsonIgnore]
		public List<int?> Values => this;

		public bool HasValues()
		{
			return this.Any((int? e) => e.HasValue);
		}
	}
}
