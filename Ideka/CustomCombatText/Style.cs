using System.Collections.Generic;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;

namespace Ideka.CustomCombatText
{
	public class Style
	{
		public class ResultFormat
		{
			public string? Text { get; set; }

			public Color Color { get; set; } = Color.get_White();

		}

		public Color BaseColor = new Color(255, 255, 255);

		public Color DefaultEntityColor = new Color(238, 85, 85);

		public Color? PetColor = new Color(34, 153, 238);

		public Dictionary<ProfessionType, Color> ProfessionColors = new Dictionary<ProfessionType, Color>
		{
			[(ProfessionType)6] = new Color(221, 68, 68),
			[(ProfessionType)3] = new Color(221, 153, 68),
			[(ProfessionType)1] = new Color(119, 204, 238),
			[(ProfessionType)7] = new Color(170, 102, 221),
			[(ProfessionType)8] = new Color(136, 204, 153),
			[(ProfessionType)4] = new Color(187, 221, 85),
			[(ProfessionType)9] = new Color(204, 85, 119),
			[(ProfessionType)5] = new Color(136, 153, 204),
			[(ProfessionType)2] = new Color(238, 170, 34)
		};

		public Dictionary<EventResult, ResultFormat> ResultFormats = new Dictionary<EventResult, ResultFormat>
		{
			[EventResult.Strike] = new ResultFormat
			{
				Color = new Color(238, 136, 0)
			},
			[EventResult.Crit] = new ResultFormat
			{
				Color = new Color(238, 51, 51)
			},
			[EventResult.Glance] = new ResultFormat
			{
				Color = new Color(170, 170, 170)
			},
			[EventResult.Block] = new ResultFormat
			{
				Color = new Color(170, 170, 170),
				Text = "block"
			},
			[EventResult.Evade] = new ResultFormat
			{
				Color = new Color(170, 170, 170),
				Text = "evade"
			},
			[EventResult.Invuln] = new ResultFormat
			{
				Color = new Color(170, 170, 170),
				Text = "invuln"
			},
			[EventResult.Miss] = new ResultFormat
			{
				Color = new Color(170, 170, 170),
				Text = "miss"
			},
			[EventResult.Bleeding] = new ResultFormat
			{
				Color = new Color(221, 85, 221)
			},
			[EventResult.Burning] = new ResultFormat
			{
				Color = new Color(221, 85, 221)
			},
			[EventResult.Poison] = new ResultFormat
			{
				Color = new Color(221, 85, 221)
			},
			[EventResult.Confusion] = new ResultFormat
			{
				Color = new Color(221, 85, 221)
			},
			[EventResult.Torment] = new ResultFormat
			{
				Color = new Color(221, 85, 221)
			},
			[EventResult.DamageTick] = new ResultFormat
			{
				Color = new Color(221, 85, 221)
			},
			[EventResult.Heal] = new ResultFormat
			{
				Color = new Color(51, 204, 17)
			},
			[EventResult.HealTick] = new ResultFormat
			{
				Color = new Color(51, 204, 17)
			},
			[EventResult.Barrier] = new ResultFormat
			{
				Color = new Color(102, 221, 255)
			},
			[EventResult.Interrupt] = new ResultFormat
			{
				Color = new Color(68, 136, 136),
				Text = "interrupt"
			},
			[EventResult.Breakbar] = new ResultFormat
			{
				Color = new Color(68, 136, 136),
				Text = "break"
			}
		};

		public Color? BarrierColor { get; set; } = new Color(255, 238, 153);

	}
}
