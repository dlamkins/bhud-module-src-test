using System;
using System.Collections.Generic;
using System.Linq;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.Utility;
using Neo.IronLua;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.Scripting.Lib
{
	public class World
	{
		private readonly Dictionary<string, Guid> _guidCache = new Dictionary<string, Guid>(StringComparer.InvariantCultureIgnoreCase);

		private readonly PathingGlobal _global;

		public PathingCategory RootCategory => _global.ScriptEngine.Module.PackInitiator.PackState.RootCategory;

		internal World(PathingGlobal global)
		{
			_global = global;
		}

		public PathingCategory CategoryByType(string id)
		{
			if (!_global.ScriptEngine.Module.PackInitiator.PackState.RootCategory.TryGetCategoryFromNamespace(id, out var category))
			{
				return null;
			}
			return category;
		}

		public IPathingEntity PathableByGuid(string guid)
		{
			if (!_guidCache.TryGetValue(guid, out var g))
			{
				_guidCache.Add(guid, g = AttributeParsingUtil.InternalGetValueAsGuid(guid));
			}
			foreach (IPathingEntity pathable in _global.ScriptEngine.Module.PackInitiator.PackState.Entities)
			{
				if (pathable.Guid == g)
				{
					return pathable;
				}
			}
			return null;
		}

		public LuaTable PathablesByGuid(string guid)
		{
			LuaTable nTable = new LuaTable();
			if (!_guidCache.TryGetValue(guid, out var g))
			{
				_guidCache.Add(guid, g = AttributeParsingUtil.InternalGetValueAsGuid(guid));
			}
			foreach (IPathingEntity pathable in _global.ScriptEngine.Module.PackInitiator.PackState.Entities)
			{
				if (pathable.Guid == g)
				{
					nTable.Add(pathable);
				}
			}
			return nTable;
		}

		public StandardMarker MarkerByGuid(string guid)
		{
			StandardMarker marker = PathableByGuid(guid) as StandardMarker;
			if (marker == null)
			{
				return null;
			}
			return marker;
		}

		public StandardMarker GetClosestMarker()
		{
			return (from marker in _global.ScriptEngine.Module.PackInitiator.PackState.Entities.ToArray().OfType<StandardMarker>()
				orderby marker.DistanceToPlayer
				select marker).FirstOrDefault();
		}

		public LuaTable GetClosestMarkers(int quantity)
		{
			LuaTable nTable = new LuaTable();
			foreach (StandardMarker marker2 in (from marker in _global.ScriptEngine.Module.PackInitiator.PackState.Entities.ToArray().OfType<StandardMarker>()
				orderby marker.DistanceToPlayer
				select marker).Take(quantity))
			{
				nTable.Add(marker2);
			}
			return nTable;
		}

		public StandardTrail TrailByGuid(string guid)
		{
			StandardTrail trail = PathableByGuid(guid) as StandardTrail;
			if (trail == null)
			{
				return null;
			}
			return trail;
		}
	}
}
