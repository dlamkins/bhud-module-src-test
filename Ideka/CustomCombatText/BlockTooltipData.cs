using System.Collections.Generic;
using System.Linq;
using HsAPI;

namespace Ideka.CustomCombatText
{
	public class BlockTooltipData
	{
		public string Description { get; }

		public List<FactTooltipData> Facts { get; }

		public BlockTooltipData(FactBlock block, double weaponStrength, TooltipContext context)
		{
			TooltipContext context2 = context;
			Description = TooltipUtils.FormatText(block.Description) ?? "";
			Facts = (from f in block.Facts
				orderby f.Order
				select new FactTooltipData(f, weaponStrength, context2)).ToList();
			base._002Ector();
		}
	}
}
