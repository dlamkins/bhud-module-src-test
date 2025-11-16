using System.Collections.Generic;

namespace roguishpanda.AB_Bauble_Farm
{
	public class TimerDetailData
	{
		public int ID { get; set; }

		public string Description { get; set; }

		public double Minutes { get; set; }

		public double Seconds { get; set; }

		public List<NotesData> WaypointData { get; set; }

		public List<NotesData> NotesData { get; set; }
	}
}
