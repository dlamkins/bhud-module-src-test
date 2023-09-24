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
			return readable switch
			{
				"None" => SquadMarker.None, 
				"Arrow" => SquadMarker.Arrow, 
				"Circle" => SquadMarker.Circle, 
				"Heart" => SquadMarker.Heart, 
				"Square" => SquadMarker.Square, 
				"STAR" => SquadMarker.Star, 
				"Sprial" => SquadMarker.Spiral, 
				"Triangle" => SquadMarker.Triangle, 
				"Cross" => SquadMarker.Cross, 
				"Clear" => SquadMarker.Clear, 
				_ => SquadMarker.None, 
			};
		}
	}
}
