using System.Collections.Generic;

namespace roguishpanda.AB_Bauble_Farm
{
	public class PackageData
	{
		public string PackageName { get; set; }

		public List<StaticDetailData> StaticDetailData { get; set; }

		public List<TimerDetailData> TimerDetailData { get; set; }
	}
}
