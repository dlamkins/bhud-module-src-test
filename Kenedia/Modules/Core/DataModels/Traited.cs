using System.Collections.Generic;

namespace Kenedia.Modules.Core.DataModels
{
	public class Traited : Dictionary<int, int>
	{
		public bool HasValues()
		{
			return base.Count > 0;
		}
	}
}
