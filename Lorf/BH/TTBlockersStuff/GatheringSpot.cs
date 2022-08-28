using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TTBlockersStuff.Language;

namespace Lorf.BH.TTBlockersStuff
{
	internal class GatheringSpot
	{
		public static readonly GatheringSpot Amber = new GatheringSpot(Translations.GatheringSpotTitleAmber, 90, new Vector2(670.07f, -606.34f), isWurm: true, 100f);

		public static readonly GatheringSpot Crimson = new GatheringSpot(Translations.GatheringSpotTitleCrimson, 90, new Vector2(198.5f, -438.5f), isWurm: true, 100f);

		public static readonly GatheringSpot Cobalt = new GatheringSpot(Translations.GatheringSpotTitleCobalt, 80, new Vector2(-277.5f, -878.2f), isWurm: true, 150f);

		public static readonly GatheringSpot General = new GatheringSpot(Translations.GatheringspotTitleMain, 90, new Vector2(185f, -83f), isWurm: false, 100f);

		public static readonly IEnumerable<GatheringSpot> All = new List<GatheringSpot> { Amber, Crimson, Cobalt, General };

		public string Name { get; }

		public int HuskTime { get; }

		public Vector2 Position { get; }

		public bool IsWurm { get; }

		public float SpotRadius { get; }

		private GatheringSpot(string name, int huskTime, Vector2 position, bool isWurm, float spotRadius)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			Name = name;
			HuskTime = huskTime;
			Position = position;
			IsWurm = isWurm;
			SpotRadius = spotRadius;
		}

		public static GatheringSpot FromPosition(Vector2 position)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return All.FirstOrDefault((GatheringSpot x) => Vector2.Distance(x.Position, position) < x.SpotRadius);
		}
	}
}
