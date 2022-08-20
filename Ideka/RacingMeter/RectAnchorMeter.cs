using System.Collections.Generic;

namespace Ideka.RacingMeter
{
	public abstract class RectAnchorMeter : RectAnchor, IMeter
	{
		private readonly List<Projection> _projections = new List<Projection>();

		public abstract double MinProjected { get; protected set; }

		public abstract double FullPortion { get; protected set; }

		public Projection AddProjection(Projection projection)
		{
			_projections.Add(projection);
			return projection;
		}

		public void UpdateProjections()
		{
			foreach (Projection projection in _projections)
			{
				projection.Update();
			}
		}

		protected override void EarlyDraw(RectTarget target)
		{
			UpdateProjections();
		}
	}
}
