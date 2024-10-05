using System;
using System.Collections.Generic;
using System.Linq;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Extensions;

namespace Kenedia.Modules.BuildsManager.DataModels.Stats
{
	public class StatAttributes : Dictionary<AttributeType, StatAttribute>
	{
		public string ToString(double attributeAdjustment)
		{
			return string.Join(Environment.NewLine, from e in base.Values
				where e != null
				select $"+ {Math.Round((double)e.Value + e.Multiplier * attributeAdjustment)} {e.Id.GetDisplayName()}");
		}
	}
}
