using BhModule.Community.Pathing.Entity;
using Neo.IronLua;
using TmfLib;

namespace BhModule.Community.Pathing.Scripting
{
	public class PackContext
	{
		private readonly ScriptEngine _scriptEngine;

		public IPackResourceManager ResourceManager { get; }

		public PackContext(ScriptEngine scriptEngine, IPackResourceManager resourceManager)
		{
			_scriptEngine = scriptEngine;
			ResourceManager = resourceManager;
		}

		public LuaResult Require(string path)
		{
			return _scriptEngine.Global.Require(path, ResourceManager);
		}

		public StandardMarker CreateMarker(LuaTable attributes = null)
		{
			return _scriptEngine.Global.I.Marker(ResourceManager, attributes);
		}
	}
}
