using System;
using System.Linq;
using System.Reflection;
using Blish_HUD;
using Blish_HUD.Modules;
using Quarry.Interfaces;

namespace Quarry.Services
{
	public class PathingBridge : IPathingBridge
	{
		private const string PathingNamespace = "bh.community.pathing";

		private readonly Logger logger;

		private object cachedModuleInstance;

		private object cachedCategoryStates;

		private MethodInfo getNamespaceInactiveMethod;

		private MethodInfo setInactiveMethod;

		private bool loggedMissing;

		private bool loggedShapeMismatch;

		public bool IsAvailable => ResolveCategoryStates() != null;

		public PathingBridge(Logger logger)
		{
			this.logger = logger;
		}

		public bool TryGetInactive(string categoryNamespace, out bool inactive)
		{
			inactive = false;
			object categoryStates = ResolveCategoryStates();
			if (categoryStates == null)
			{
				return false;
			}
			try
			{
				inactive = (bool)getNamespaceInactiveMethod.Invoke(categoryStates, new object[1] { categoryNamespace });
				return true;
			}
			catch (Exception ex)
			{
				LogShapeMismatchOnce(ex);
				return false;
			}
		}

		public bool TrySetInactive(string categoryNamespace, bool inactive)
		{
			object categoryStates = ResolveCategoryStates();
			if (categoryStates == null)
			{
				return false;
			}
			try
			{
				setInactiveMethod.Invoke(categoryStates, new object[2] { categoryNamespace, inactive });
				return true;
			}
			catch (Exception ex)
			{
				LogShapeMismatchOnce(ex);
				return false;
			}
		}

		private object ResolveCategoryStates()
		{
			object moduleInstance = GetPathingModuleInstance(logger, ref loggedMissing);
			if (moduleInstance == null)
			{
				cachedModuleInstance = null;
				cachedCategoryStates = null;
				return null;
			}
			if (moduleInstance == cachedModuleInstance && cachedCategoryStates != null)
			{
				return cachedCategoryStates;
			}
			cachedModuleInstance = moduleInstance;
			cachedCategoryStates = null;
			getNamespaceInactiveMethod = null;
			setInactiveMethod = null;
			object categoryStates = GetProp(GetProp(GetProp(moduleInstance, "PackInitiator"), "PackState"), "CategoryStates");
			if (categoryStates == null)
			{
				if (!loggedMissing)
				{
					loggedMissing = true;
					logger.Info("Pathing bridge: Pathing loaded but PackInitiator/PackState/CategoryStates isn't ready yet; will retry.");
				}
				return null;
			}
			Type categoryStatesType = categoryStates.GetType();
			getNamespaceInactiveMethod = categoryStatesType.GetMethod("GetNamespaceInactive", BindingFlags.Instance | BindingFlags.Public, null, new Type[1] { typeof(string) }, null);
			setInactiveMethod = categoryStatesType.GetMethod("SetInactive", BindingFlags.Instance | BindingFlags.Public, null, new Type[2]
			{
				typeof(string),
				typeof(bool)
			}, null);
			if ((object)getNamespaceInactiveMethod == null || (object)setInactiveMethod == null)
			{
				LogShapeMismatchOnce(null);
				return null;
			}
			cachedCategoryStates = categoryStates;
			return cachedCategoryStates;
		}

		private static object GetPathingModuleInstance(Logger logger, ref bool loggedMissing)
		{
			try
			{
				ModuleManager pathing = GameService.Module.get_Modules().FirstOrDefault(delegate(ModuleManager m)
				{
					Manifest manifest = m.get_Manifest();
					return string.Equals((manifest != null) ? manifest.get_Namespace() : null, "bh.community.pathing", StringComparison.OrdinalIgnoreCase);
				});
				if (pathing == null || !pathing.get_Enabled() || pathing.get_ModuleInstance() == null)
				{
					if (!loggedMissing)
					{
						loggedMissing = true;
						logger.Info((pathing == null) ? "Pathing bridge: Pathing module not installed; hunt mode inactive." : "Pathing bridge: Pathing module installed but not enabled/loaded yet; will retry.");
					}
					return null;
				}
				return pathing.get_ModuleInstance();
			}
			catch (Exception ex)
			{
				logger.Warn(ex, "Pathing bridge: failed to locate Pathing module.");
				return null;
			}
		}

		private static object GetProp(object target, string name)
		{
			return target?.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public)?.GetValue(target);
		}

		private void LogShapeMismatchOnce(Exception ex)
		{
			if (!loggedShapeMismatch)
			{
				loggedShapeMismatch = true;
				logger.Error(ex, "Pathing bridge: Pathing's CategoryStates shape didn't match what we expect; hunt mode will do nothing until this is fixed.");
			}
		}
	}
}
