using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Taimi.UndaDaSea_BlishHUD
{
	public class SkyLake
	{
		private readonly float _waterSurface;

		private readonly float _waterBottom;

		private readonly List<Vector3> bounds;

		private readonly Vector3 center;

		private readonly float radius;

		private float _distance;

		private readonly string _name;

		private readonly int _map;

		public string Name => _name;

		public int Map => _map;

		public float WaterSurface => _waterSurface;

		public float WaterBottom => _waterBottom;

		[JsonIgnore]
		public float Distance => _distance;

		public List<Vector3> Bounds => bounds;

		public SkyLake(float waterSurface, float waterBottom, List<Vector3> bounds, int map, string name)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			_waterSurface = waterSurface;
			_waterBottom = waterBottom;
			_map = map;
			_name = name;
			this.bounds = bounds;
			center = GetCenter();
			radius = GetRadius();
		}

		public bool IsNearby(Vector3 playerPos)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			_distance = Vector3.Distance(playerPos, center);
			if ((double)_distance > (double)radius * 1.5)
			{
				return false;
			}
			return true;
		}

		public bool IsInWater(Vector3 playerPos)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			if (playerPos.Z < _waterBottom || playerPos.Z > _waterSurface + 10f)
			{
				return false;
			}
			return IsInPolygon(playerPos);
		}

		private bool IsInPolygon(Vector3 playerPos)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			int count = bounds.Count;
			bool isInside = false;
			int i = 0;
			int j = count - 1;
			while (i < count)
			{
				bool num = bounds[i].Y > playerPos.Y;
				bool isYAboveSecondVertex = bounds[j].Y > playerPos.Y;
				if (num != isYAboveSecondVertex)
				{
					float intersectionX = bounds[i].X + (playerPos.Y - bounds[i].Y) / (bounds[j].Y - bounds[i].Y) * (bounds[j].X - bounds[i].X);
					if (playerPos.X < intersectionX)
					{
						isInside = !isInside;
					}
				}
				j = i++;
			}
			return isInside;
		}

		private Vector3 GetCenter()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			Vector3 center = default(Vector3);
			((Vector3)(ref center))._002Ector(0f, 0f, 0f);
			foreach (Vector3 bound in bounds)
			{
				center += bound;
			}
			center /= (float)bounds.Count;
			return center;
		}

		private float GetRadius()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			float radius = 0f;
			foreach (Vector3 bound in bounds)
			{
				float distance = Vector3.Distance(bound, center);
				if (distance > radius)
				{
					radius = distance;
				}
			}
			return radius;
		}
	}
}
