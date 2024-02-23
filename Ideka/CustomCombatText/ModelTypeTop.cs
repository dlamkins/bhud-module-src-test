using System;

namespace Ideka.CustomCombatText
{
	public class ModelTypeTop : IAreaModelType
	{
		public AreaType Type => AreaType.Top;

		public float MessagePivotX { get; set; }

		public TimeSpan MessageTimeout { get; set; } = TimeSpan.FromSeconds(3.0);


		public bool AnimateOnHit { get; set; } = true;


		public AreaViewType CreateView(AreaModel model)
		{
			return new ViewTypeTop(model);
		}
	}
}
