using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Library.Enums
{
	public static class SquadMarkerExtension
	{
		public static Texture2D GetIcon(this SquadMarker marker)
		{
			return (Texture2D)(marker switch
			{
				SquadMarker.Arrow => Service.Textures!._imgArrow, 
				SquadMarker.Circle => Service.Textures!._imgCircle, 
				SquadMarker.Heart => Service.Textures!._imgHeart, 
				SquadMarker.Square => Service.Textures!._imgSquare, 
				SquadMarker.Star => Service.Textures!._imgStar, 
				SquadMarker.Spiral => Service.Textures!._imgSpiral, 
				SquadMarker.Triangle => Service.Textures!._imgTriangle, 
				SquadMarker.Cross => Service.Textures!._imgX, 
				_ => Service.Textures!._blishHeart, 
			});
		}

		public static Texture2D GetFadedIcon(this SquadMarker marker)
		{
			return (Texture2D)(marker switch
			{
				SquadMarker.Arrow => Service.Textures!._imgArrowFade, 
				SquadMarker.Circle => Service.Textures!._imgCircleFade, 
				SquadMarker.Heart => Service.Textures!._imgHeartFade, 
				SquadMarker.Square => Service.Textures!._imgSquareFade, 
				SquadMarker.Star => Service.Textures!._imgStarFade, 
				SquadMarker.Spiral => Service.Textures!._imgSpiralFade, 
				SquadMarker.Triangle => Service.Textures!._imgTriangleFade, 
				SquadMarker.Cross => Service.Textures!._imgXFade, 
				_ => Service.Textures!._blishHeart, 
			});
		}
	}
}
