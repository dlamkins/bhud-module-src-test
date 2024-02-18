using System;
using System.Reflection;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexedHero.Blish_HUD.MarkerPackAssistant.Utils
{
	public class Reflection
	{
		public static void InjectNewBackground(WindowBase2 Window, Texture2D backgroundTexture, Rectangle Bounds)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				((object)Window).GetType().BaseType.GetProperty("WindowBackground", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Window, AsyncTexture2D.op_Implicit(backgroundTexture));
				InjectNewBackgroundBounds(Window, Bounds);
			}
			catch (Exception Exception)
			{
				MarkerPackAssistant.Instance.Logger.Error("Could not inject new background! Exception: " + Exception.Message);
			}
		}

		public static void InjectNewBackgroundBounds(WindowBase2 Window, Rectangle Bounds)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				((object)Window).GetType().BaseType.GetProperty("BackgroundDestinationBounds", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Window, Bounds);
			}
			catch (Exception Exception)
			{
				MarkerPackAssistant.Instance.Logger.Error("Could not inject new background bounds! Exception: " + Exception.Message);
			}
		}

		public static void ReloadPathingMarkers(ModuleManager moduleManager)
		{
			try
			{
				Module pathingModule = moduleManager.get_ModuleInstance();
				object packInitiator = ((object)pathingModule).GetType().GetProperty("PackInitiator").GetValue(pathingModule);
				packInitiator.GetType().GetMethod("ReloadPacks").Invoke(packInitiator, null);
			}
			catch (Exception Exception)
			{
				MarkerPackAssistant.Instance.Logger.Error("Could not reload pathing markers! Exception: " + Exception.Message);
			}
		}
	}
}
