namespace Gorthax.Gilledwars
{
	public class PersonalBestRecord
	{
		public double Weight { get; set; }

		public double Length { get; set; }

		public string Signature { get; set; }

		public bool IsCheater { get; set; }

		public bool IsSuperPb { get; set; }

		public SubRecord BestWeight { get; set; }

		public SubRecord BestLength { get; set; }
	}
}
