using System;
using Microsoft.Xna.Framework;

namespace Frtal.Wayfinder.Util
{
	public struct MapCalibration
	{
		public bool IsValid;

		public double MetersPerContinent;

		private double _crLeft;

		private double _crTop;

		private double _crRight;

		private double _crBottom;

		private double _mrLeft;

		private double _mrTop;

		private double _mrRight;

		private double _mrBottom;

		public static MapCalibration Create(double crLeft, double crTop, double crRight, double crBottom, double mrLeft, double mrTop, double mrRight, double mrBottom)
		{
			double contW = crRight - crLeft;
			double mapW = mrRight - mrLeft;
			MapCalibration result;
			if (Math.Abs(contW) < 1E-06 || Math.Abs(mapW) < 1E-06)
			{
				result = default(MapCalibration);
				result.IsValid = false;
				return result;
			}
			result = default(MapCalibration);
			result.IsValid = true;
			result._crLeft = crLeft;
			result._crTop = crTop;
			result._crRight = crRight;
			result._crBottom = crBottom;
			result._mrLeft = mrLeft;
			result._mrTop = mrTop;
			result._mrRight = mrRight;
			result._mrBottom = mrBottom;
			result.MetersPerContinent = Math.Abs(mapW) / 39.3701 / Math.Abs(contW);
			return result;
		}

		public Vector2 WorldToContinent(float worldX, float worldY)
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			double mapX = (double)worldX * 39.3701;
			double mapY = (double)(0f - worldY) * 39.3701;
			double num = _crLeft + (mapX - _mrLeft) / (_mrRight - _mrLeft) * (_crRight - _crLeft);
			double cy = _crTop - (mapY - _mrBottom) / (_mrBottom - _mrTop) * (_crBottom - _crTop);
			return new Vector2((float)num, (float)cy);
		}

		public double ToMeters(double continentDistance)
		{
			return continentDistance * MetersPerContinent;
		}
	}
}
