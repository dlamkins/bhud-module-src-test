using System;

namespace NpcFinder.Models
{
	public class Gw2MapInfo
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int ContinentId { get; set; }

		public int DefaultFloor { get; set; }

		public int[] Floors { get; set; } = Array.Empty<int>();


		public Rect2D MapRect { get; set; }

		public Rect2D ContinentRect { get; set; }
	}
}
