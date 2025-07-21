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

		private static readonly Color IngamePink = new Color(224, 85, 224);

		private static readonly Color IngameRed = new Color(241, 45, 45);

		private static readonly Color IngameWhite = new Color(255, 255, 255);

		private static readonly Color IngameGreen = new Color(45, 197, 14);

		private static readonly Color IngameBlue = new Color(105, 229, 255);

		private static readonly Color IngameOrange = new Color(243, 132, 0);

		private static readonly Color IngameTeal = new Color(83, 166, 152);

		private static readonly Color CustomGray = new Color(170, 170, 170);

		public Dictionary<EventResult, ResultFormat> ResultFormats = new Dictionary<EventResult, ResultFormat>
		{
			[EventResult.Strike] = new ResultFormat
			{
				Color = IngameOrange
			},
			[EventResult.Crit] = new ResultFormat
			{
				Color = IngameRed
			},
			[EventResult.Glance] = new ResultFormat
			{
				Color = CustomGray
			},
			[EventResult.Block] = new ResultFormat
			{
				Color = CustomGray,
				Text = "block"
			},
			[EventResult.Evade] = new ResultFormat
			{
				Color = CustomGray,
				Text = "evade"
			},
			[EventResult.Invuln] = new ResultFormat
			{
				Color = CustomGray,
				Text = "invuln"
			},
			[EventResult.Miss] = new ResultFormat
			{
				Color = CustomGray,
				Text = "miss"
			},
			[EventResult.Bleeding] = new ResultFormat
			{
				Color = IngamePink
			},
			[EventResult.Burning] = new ResultFormat
			{
				Color = IngamePink
			},
			[EventResult.Poison] = new ResultFormat
			{
				Color = IngamePink
			},
			[EventResult.Confusion] = new ResultFormat
			{
				Color = IngamePink
			},
			[EventResult.Torment] = new ResultFormat
			{
				Color = IngamePink
			},
			[EventResult.DamageTick] = new ResultFormat
			{
				Color = IngamePink
			},
			[EventResult.Heal] = new ResultFormat
			{
				Color = IngameGreen
			},
			[EventResult.HealTick] = new ResultFormat
			{
				Color = IngameGreen
			},
			[EventResult.Barrier] = new ResultFormat
			{
				Color = IngameBlue
			},
			[EventResult.Interrupt] = new ResultFormat
			{
				Color = IngameTeal,
				Text = "interrupt"
			},
			[EventResult.Breakbar] = new ResultFormat
			{
				Color = IngameTeal,
				Text = "break"
			}
		};

		public Color? BarrierColor { get; set; } = new Color(255, 238, 153);

	}
}
