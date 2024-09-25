using System;
using System.Collections.Generic;
using System.Linq;
using Estreya.BlishHUD.Shared.Extensions;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Controls.World
{
	public class WorldCircle : WorldPolygone
	{
		public WorldCircle(Vector3 position, float radius, int tessellation = 50)
			: this(position, radius, Color.get_White(), tessellation)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)


		public WorldCircle(Vector3 position, float radius, Color color, int tessellation = 50)
			: base(position, Enumerable.Range(0, tessellation + 1).SelectWithIndex((Func<int, int, IEnumerable<int>, Vector3>)delegate(int t, int index, IEnumerable<int> sourceList)
			{
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				float num = (float)((double)((float)index / (float)tessellation * 2f) * Math.PI);
				float num2 = (float)Math.Cos(num);
				float num3 = (float)Math.Sin(num);
				float num4 = num2 * radius;
				float num5 = num3 * radius;
				return new Vector3(num4, num5, 0f);
			}).SelectManyWithIndex(delegate(Vector3 t, int index, IEnumerable<Vector3> sourceList, bool first, bool last)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				IEnumerable<Vector3> result = Enumerable.Repeat<Vector3>(t, (first || last) ? 1 : 2);
				first = false;
				return result;
			})
				.ToArray(), color)
		{
		}//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)

	}
}
