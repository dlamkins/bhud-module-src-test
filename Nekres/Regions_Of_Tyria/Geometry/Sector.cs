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
			Id = sector.get_Id();
			Name = sector.get_Name();
			_envelope = new Envelope(sector.get_Bounds().Min((Coordinates2 coord) => ((Coordinates2)(ref coord)).get_X()), sector.get_Bounds().Min((Coordinates2 coord) => ((Coordinates2)(ref coord)).get_Y()), sector.get_Bounds().Max((Coordinates2 coord) => ((Coordinates2)(ref coord)).get_X()), sector.get_Bounds().Max((Coordinates2 coord) => ((Coordinates2)(ref coord)).get_Y()));
		}

		public int CompareTo(Sector other)
		{
			if (Envelope.MinX != other.Envelope.MinX)
			{
				return Envelope.MinX.CompareTo(other.Envelope.MinX);
			}
			if (Envelope.MinY != other.Envelope.MinY)
			{
				return Envelope.MinY.CompareTo(other.Envelope.MinY);
			}
			if (Envelope.MaxX != other.Envelope.MaxX)
			{
				return Envelope.MaxX.CompareTo(other.Envelope.MaxX);
			}
			if (Envelope.MaxY != other.Envelope.MaxY)
			{
				return Envelope.MaxY.CompareTo(other.Envelope.MaxY);
			}
			return 0;
		}

		public bool Equals(Sector other)
		{
			if (other != null)
			{
				return _envelope == other._envelope;
			}
			return false;
		}
	}
}
