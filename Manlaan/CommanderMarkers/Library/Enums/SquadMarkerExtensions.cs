namespace Manlaan.CommanderMarkers.Library.Enums
{
	public static class SquadMarkerExtensions
	{
		public static string EnumValue(this SquadMarker e)
		{
			return e switch
			{
				SquadMarker.None => "None", 
				SquadMarker.Arrow => "Arrow", 
				SquadMarker.Circle => "Circle", 
				SquadMarker.Heart => "Heart", 
				SquadMarker.Square => "Square", 
				SquadMarker.Star => "STAR", 
				SquadMarker.Spiral => "Sprial", 
				SquadMarker.Triangle => "Triangle", 
				SquadMarker.Cross => "Cross", 
				SquadMarker.Clear => "Clear", 
				_ => "None", 
			};
		}

		public static SquadMarker EnumValue(this SquadMarker e, string readable)
		{
			if (readable != null)
			{
				switch (readable.Length)
				{
				case 4:
					switch (readable[0])
					{
					case 'N':
						if (!(readable == "None"))
						{
							break;
						}
						return SquadMarker.None;
					case 'S':
						if (!(readable == "STAR"))
						{
							break;
						}
						return SquadMarker.Star;
					}
					break;
				case 5:
					switch (readable[2])
					{
					case 'r':
						if (!(readable == "Arrow"))
						{
							break;
						}
						return SquadMarker.Arrow;
					case 'a':
						if (!(readable == "Heart"))
						{
							break;
						}
						return SquadMarker.Heart;
					case 'o':
						if (!(readable == "Cross"))
						{
							break;
						}
						return SquadMarker.Cross;
					case 'e':
						if (!(readable == "Clear"))
						{
							break;
						}
						return SquadMarker.Clear;
					}
					break;
				case 6:
					switch (readable[1])
					{
					case 'i':
						if (!(readable == "Circle"))
						{
							break;
						}
						return SquadMarker.Circle;
					case 'q':
						if (!(readable == "Square"))
						{
							break;
						}
						return SquadMarker.Square;
					case 'p':
						if (!(readable == "Sprial"))
						{
							break;
						}
						return SquadMarker.Spiral;
					}
					break;
				case 8:
					if (!(readable == "Triangle"))
					{
						break;
					}
					return SquadMarker.Triangle;
				}
			}
			return SquadMarker.None;
		}
	}
}
