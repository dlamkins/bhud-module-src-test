namespace Ideka.RacingMeter
{
	public interface IMeter
	{
		double MinProjected { get; }

		double FullPortion { get; }

		Projection AddProjection(Projection projection);

		void UpdateProjections();
	}
}
