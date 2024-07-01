using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Ideka.NetCommon;

namespace Ideka.RacingMeter
{
	public class ProjectionAnchors
	{
		[CompilerGenerated]
		private Func<double> _003CvalueGet_003EP;

		[CompilerGenerated]
		private double _003CviewSlack_003EP;

		[CompilerGenerated]
		private double _003ClevelScrollBase_003EP;

		public const double DefaultViewSlack = 500.0;

		public const double DefaultScrollBase = 1000.0;

		private readonly List<IProjectionAnchor> _anchors;

		public ProjectionAnchors(Func<double> valueGet, double viewSlack = 500.0, double levelScrollBase = 1000.0)
		{
			_003CvalueGet_003EP = valueGet;
			_003CviewSlack_003EP = viewSlack;
			_003ClevelScrollBase_003EP = levelScrollBase;
			_anchors = new List<IProjectionAnchor>();
			base._002Ector();
		}

		public double GetLevel()
		{
			IProjectionAnchor anchor = GetFurthestAnchor();
			if (anchor != null)
			{
				return Math.Max(0.0, Math.Abs(anchor.Value - _003CvalueGet_003EP()) / _003ClevelScrollBase_003EP);
			}
			return 0.0;
		}

		public T AddAnchor<T>(T anchor) where T : IProjectionAnchor
		{
			_anchors.Add(anchor);
			return anchor;
		}

		public IProjectionAnchor? GetFurthestAnchor()
		{
			double value = _003CvalueGet_003EP();
			IEnumerable<IProjectionAnchor> visible = _anchors.Where((IProjectionAnchor a) => a.Visible);
			if (!visible.Any())
			{
				return null;
			}
			return visible.MaxBy((IProjectionAnchor a) => Math.Abs(a.Value - value));
		}

		public void Update(Projection projection)
		{
			double currentHeight = _003CvalueGet_003EP();
			IProjectionAnchor anchor = GetFurthestAnchor();
			if (anchor != null)
			{
				double padding = Math.Max(Math.Abs(anchor.Value - currentHeight) / 2.0, _003CviewSlack_003EP);
				projection.MaxValue = Math.Max(anchor.Value, currentHeight) + padding;
				projection.MinValue = Math.Min(anchor.Value, currentHeight) - padding;
			}
			else
			{
				projection.MaxValue = currentHeight + _003CviewSlack_003EP;
				projection.MinValue = currentHeight - _003CviewSlack_003EP;
			}
		}
	}
}
