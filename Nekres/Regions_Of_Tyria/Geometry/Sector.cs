using System;
using System.Linq;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using RBush;

namespace Nekres.Regions_Of_Tyria.Geometry
{
	public class Sector : ISpatialData, IComparable<Sector>, IEquatable<Sector>
	{
		private readonly Envelope _envelope;

		public readonly int Id;

		public readonly string Name;

		public ref readonly Envelope Envelope => ref _envelope;

		public Sector(ContinentFloorRegionMapSector sector)
		{
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			Id = sector.get_Id();
			Name = sector.get_Name();
			_envelope = new Envelope(sector.get_Bounds().Min((Coordinates2 coord) => ((Coordinates2)(ref coord)).get_X()), sector.get_Bounds().Min((Coordinates2 coord) => ((Coordinates2)(ref coord)).get_Y()), sector.get_Bounds().Max((Coordinates2 coord) => ((Coordinates2)(ref coord)).get_X()), sector.get_Bounds().Max((Coordinates2 coord) => ((Coordinates2)(ref coord)).get_Y()));
		}

		public int CompareTo(Sector other)
		{
			if (((Envelope)(ref Envelope)).get_MinX() != ((Envelope)(ref other.Envelope)).get_MinX())
			{
				return ((Envelope)(ref Envelope)).get_MinX().CompareTo(((Envelope)(ref other.Envelope)).get_MinX());
			}
			if (((Envelope)(ref Envelope)).get_MinY() != ((Envelope)(ref other.Envelope)).get_MinY())
			{
				return ((Envelope)(ref Envelope)).get_MinY().CompareTo(((Envelope)(ref other.Envelope)).get_MinY());
			}
			if (((Envelope)(ref Envelope)).get_MaxX() != ((Envelope)(ref other.Envelope)).get_MaxX())
			{
				return ((Envelope)(ref Envelope)).get_MaxX().CompareTo(((Envelope)(ref other.Envelope)).get_MaxX());
			}
			if (((Envelope)(ref Envelope)).get_MaxY() != ((Envelope)(ref other.Envelope)).get_MaxY())
			{
				return ((Envelope)(ref Envelope)).get_MaxY().CompareTo(((Envelope)(ref other.Envelope)).get_MaxY());
			}
			return 0;
		}

		public bool Equals(Sector other)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return _envelope == other._envelope;
		}
	}
}
