using System;
using System.Collections;
using System.Collections.Generic;
using Kenedia.Modules.BuildsManager.DataModels.Professions;

namespace Kenedia.Modules.BuildsManager.Models.Templates
{
	public class LegendCollection : IEnumerable<Legend>, IEnumerable
	{
		public Legend? TerrestrialActive { get; set; }

		public Legend? TerrestrialInactive { get; set; }

		public Legend? AquaticActive { get; set; }

		public Legend? AquaticInactive { get; set; }

		public Legend this[LegendSlotType slot]
		{
			get
			{
				if (!TryGetValue(slot, out var legend))
				{
					return null;
				}
				return legend;
			}
		}

		public LegendSlotType this[Legend legend]
		{
			get
			{
				if (legend != null)
				{
					if (legend == TerrestrialActive)
					{
						return LegendSlotType.TerrestrialActive;
					}
					if (legend == TerrestrialInactive)
					{
						return LegendSlotType.TerrestrialInactive;
					}
					if (legend == AquaticActive)
					{
						return LegendSlotType.AquaticActive;
					}
					if (legend == AquaticInactive)
					{
						return LegendSlotType.AquaticInactive;
					}
				}
				throw new ArgumentOutOfRangeException("legend", legend, null);
			}
		}

		public byte GetLegendByte(LegendSlotType slot)
		{
			Legend legend;
			return (byte)((TryGetValue(slot, out legend) && legend != null) ? ((uint)legend.Id) : 0u);
		}

		public bool TryGetValue(LegendSlotType slot, out Legend legend)
		{
			legend = slot switch
			{
				LegendSlotType.AquaticActive => AquaticActive, 
				LegendSlotType.AquaticInactive => AquaticInactive, 
				LegendSlotType.TerrestrialActive => TerrestrialActive, 
				LegendSlotType.TerrestrialInactive => TerrestrialInactive, 
				_ => null, 
			};
			return legend != null;
		}

		public void SetLegend(LegendSlotType slot, Legend? legend)
		{
			switch (slot)
			{
			case LegendSlotType.AquaticActive:
				AquaticActive = legend;
				break;
			case LegendSlotType.AquaticInactive:
				AquaticInactive = legend;
				break;
			case LegendSlotType.TerrestrialActive:
				TerrestrialActive = legend;
				break;
			case LegendSlotType.TerrestrialInactive:
				TerrestrialInactive = legend;
				break;
			default:
				throw new ArgumentOutOfRangeException("slot", slot, null);
			}
		}

		public IEnumerator<Legend> GetEnumerator()
		{
			yield return TerrestrialActive;
			yield return TerrestrialInactive;
			yield return AquaticActive;
			yield return AquaticInactive;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Wipe()
		{
			TerrestrialActive = null;
			TerrestrialInactive = null;
			AquaticActive = null;
			AquaticInactive = null;
		}
	}
}
