using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Gw2Mumble;
using Blish_HUD.Modules;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoMod.RuntimeDetour;

namespace BhModule.PathingMapAlignPlugin
{
	public class PluginService
	{
		private const string _pathingNamespace = "bh.community.pathing";

		private ModuleManager _pathingModuleManager;

		private readonly List<Action> _hookDisposeActions = new List<Action>();

		private bool _error;

		private int _mapWidth_max;

		private Logger Logger => PathingMapAlignPluginModule.Logger;

		private bool DependenciesMet => PathingMapAlignPluginModule.InstanceManager.get_DependenciesMet();

		public void Upadate()
		{
			if (DependenciesMet && !_error && _pathingModuleManager == null)
			{
				try
				{
					GetPathingModuleManager();
					HookFlatMap();
					HookRenderToMiniMap();
				}
				catch (Exception ex)
				{
					_error = true;
					OnPathingUnload(this, EventArgs.Empty);
					LogError(ex);
				}
			}
		}

		public void Unload()
		{
			if (_pathingModuleManager != null)
			{
				_pathingModuleManager.remove_ModuleDisabled((EventHandler<EventArgs>)OnPathingUnload);
				OnPathingUnload(this, EventArgs.Empty);
			}
		}

		private void OnPathingUnload(object sender, EventArgs e)
		{
			foreach (Action hookDisposeAction in _hookDisposeActions)
			{
				hookDisposeAction();
			}
			_hookDisposeActions.Clear();
		}

		private void GetPathingModuleManager()
		{
			_pathingModuleManager = GameService.Module.get_Modules().FirstOrDefault((ModuleManager m) => m.get_Manifest().get_Namespace() == "bh.community.pathing");
			_pathingModuleManager.add_ModuleDisabled((EventHandler<EventArgs>)OnPathingUnload);
		}

		private void HookFlatMap()
		{
			Type type = Assembly.GetAssembly(((object)_pathingModuleManager.get_ModuleInstance()).GetType()).GetType("BhModule.Community.Pathing.Entity.FlatMap");
			_mapWidth_max = (int)type.GetField("MAPWIDTH_MAX", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			MethodInfo method = type.GetMethod("GetOffset", BindingFlags.Instance | BindingFlags.NonPublic);
			Hook hook1 = new Hook(method, new Func<Func<object, float, float, float, float, int>, object, float, float, float, float, int>(GetOffset));
			_hookDisposeActions.Add(delegate
			{
				hook1.Dispose();
			});
			MethodInfo method2 = type.GetMethod("UpdateBounds", BindingFlags.Instance | BindingFlags.NonPublic);
			Hook hook2 = new Hook(method2, new Action<Action<Control>, Control>(UpdateBounds));
			_hookDisposeActions.Add(delegate
			{
				hook2.Dispose();
			});
		}

		private void HookRenderToMiniMap()
		{
			Assembly assembly = Assembly.GetAssembly(((object)_pathingModuleManager.get_ModuleInstance()).GetType());
			MethodInfo method = assembly.GetType("BhModule.Community.Pathing.Entity.StandardMarker").GetMethod("RenderToMiniMap", BindingFlags.Instance | BindingFlags.Public);
			Hook hook1 = new Hook(method, new Func<Func<object, SpriteBatch, Rectangle, double, double, double, float, RectangleF?>, object, SpriteBatch, Rectangle, double, double, double, float, RectangleF?>(RenderToMiniMap));
			_hookDisposeActions.Add(delegate
			{
				hook1.Dispose();
			});
			MethodInfo method2 = assembly.GetType("BhModule.Community.Pathing.Entity.StandardTrail").GetMethod("RenderToMiniMap", BindingFlags.Instance | BindingFlags.Public);
			Hook hook2 = new Hook(method2, new Func<Func<object, SpriteBatch, Rectangle, double, double, double, float, RectangleF?>, object, SpriteBatch, Rectangle, double, double, double, float, RectangleF?>(RenderToMiniMap));
			_hookDisposeActions.Add(delegate
			{
				hook2.Dispose();
			});
		}

		private void LogError(Exception ex)
		{
			Logger.Error(ex.Message + "\n" + ex.StackTrace);
		}

		private int GetOffset(Func<object, float, float, float, float, int> originFunc, object instance, float curr, float max, float min, float val)
		{
			ModuleSettings settings = PathingMapAlignPluginModule.Instance.Settings;
			int num = ((max == (float)_mapWidth_max) ? settings.Width.get_Value() : settings.Height.get_Value());
			return originFunc(instance, curr, max, min, val) + num;
		}

		private void UpdateBounds(Action<Control> originFunc, Control instance)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			originFunc(instance);
			ModuleSettings settings = PathingMapAlignPluginModule.Instance.Settings;
			UI uI = GameService.Gw2Mumble.get_UI();
			Size compassSize = uI.get_CompassSize();
			int width = ((Size)(ref compassSize)).get_Width();
			compassSize = uI.get_CompassSize();
			int height = ((Size)(ref compassSize)).get_Height();
			if (width >= 1 && height >= 1 && !uI.get_IsMapOpen())
			{
				if (uI.get_IsCompassTopRight())
				{
					instance.set_Location(new Point(instance.get_Location().X + settings.X.get_Value(), instance.get_Location().Y));
				}
				else
				{
					instance.set_Location(new Point(instance.get_Location().X + settings.X.get_Value(), instance.get_Location().Y + settings.Y.get_Value()));
				}
			}
		}

		private RectangleF? RenderToMiniMap(Func<object, SpriteBatch, Rectangle, double, double, double, float, RectangleF?> originFunc, object instance, SpriteBatch spriteBatch, Rectangle bounds, double offsetX, double offsetY, double scale, float opacity)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			ModuleSettings settings = PathingMapAlignPluginModule.Instance.Settings;
			return originFunc(instance, spriteBatch, bounds, offsetX, offsetY, scale * (double)settings.Scale.get_Value(), opacity);
		}
	}
}
