using System;
using System.Collections.Generic;
using System.Linq;
using Ideka.NetCommon;

namespace Ideka.RacingMeter
{
	public class ProjectionAnchors
	{
		private readonly Func<double> _valueGet;

		private readonly double _viewSlack;

		private readonly double _levelScrollBase;

		private readonly List<IProjectionAnchor> _anchors = new List<IProjectionAnchor>();

		public ProjectionAnchors(Func<double> valueGet, double viewSlack = 500.0, double levelScrollBase = 1000.0)
		{
			_valueGet = valueGet;
			_viewSlack = viewSlack;
			_levelScrollBase = levelScrollBase;
		}

		public double GetLevel()
		{
			IProjectionAnchor anchor = GetFurthestAnchor();
			if (anchor != null)
			{
				return Math.Max(0.0, Math.Abs(anchor.Value - _valueGet()) / _levelScrollBase);
			}
			return 0.0;
		}

		public T AddAnchor<T>(T anchor) where T : IProjectionAnchor
		{
			_anchors.Add(anchor);
			return anchor;
		}

		public IProjectionAnchor GetFurthestAnchor()
		{
			double value = _valueGet();
			IEnumerable<IProjectionAnchor> visible = _anchors.Where((IProjectionAnchor a) => a.Visible);
			if (!visible.Any())
			{
				return null;
			}
			return visible.MaxBy((IProjectionAnchor a) => Math.Abs(a.Value - value));
		}

		public void Update(Projection projection)
		{
			double currentHeight = _valueGet();
			IProjectionAnchor anchor = GetFurthestAnchor();
			if (anchor != null)
			{
				double padding = Math.Max(Math.Abs(anchor.Value - currentHeight) / 2.0, _viewSlack);
				projection.MaxValue = Math.Max(anchor.Value, currentHeight) + padding;
				projection.MinValue = Math.Min(anchor.Value, currentHeight) - padding;
			}
			else
			{
				projection.MaxValue = currentHeight + _viewSlack;
				projection.MinValue = currentHeight - _viewSlack;
			}
		}
	}
}
