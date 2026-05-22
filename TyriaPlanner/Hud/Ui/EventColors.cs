using Microsoft.Xna.Framework;
using TyriaPlanner.Hud.Settings;

namespace TyriaPlanner.Hud.Ui
{
	public static class EventColors
	{
		public static Color For(string type, ColorThemePreference theme = ColorThemePreference.Default)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(theme switch
			{
				ColorThemePreference.HighContrast => HighContrast(type), 
				ColorThemePreference.Pastel => Pastel(type), 
				ColorThemePreference.Monochrome => Monochrome(type), 
				_ => Default(type), 
			});
		}

		private static Color Default(string type)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(type switch
			{
				"raid" => new Color(218, 165, 32), 
				"fractal" => new Color(70, 180, 90), 
				"strike" => new Color(60, 140, 230), 
				"wvw" => new Color(220, 70, 60), 
				"open_world" => new Color(160, 90, 200), 
				_ => new Color(180, 180, 180), 
			});
		}

		private static Color HighContrast(string type)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(type switch
			{
				"raid" => new Color(255, 210, 60), 
				"fractal" => new Color(80, 230, 110), 
				"strike" => new Color(80, 180, 255), 
				"wvw" => new Color(255, 90, 80), 
				"open_world" => new Color(210, 110, 255), 
				_ => new Color(220, 220, 220), 
			});
		}

		private static Color Pastel(string type)
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(type switch
			{
				"raid" => new Color(230, 200, 130), 
				"fractal" => new Color(150, 210, 165), 
				"strike" => new Color(140, 180, 230), 
				"wvw" => new Color(230, 150, 145), 
				"open_world" => new Color(195, 165, 220), 
				_ => new Color(200, 200, 200), 
			});
		}

		private static Color Monochrome(string type)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Color(218, 165, 32);
		}
	}
}
