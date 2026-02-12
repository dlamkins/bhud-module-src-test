namespace Maestro.Models
{
	public class SeekData
	{
		public long[] CumulativeTimeMs { get; set; }

		public int[] OctaveAtCommand { get; set; }

		public long TotalDurationMs { get; set; }
	}
}
