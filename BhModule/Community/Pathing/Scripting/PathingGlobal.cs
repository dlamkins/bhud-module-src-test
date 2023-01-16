using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Scripting.Lib;
using BhModule.Community.Pathing.Scripting.Lib.Std;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Neo.IronLua;
using TmfLib;

namespace BhModule.Community.Pathing.Scripting
{
	public class PathingGlobal : LuaTable
	{
		private readonly Dictionary<int, LuaChunk> _loadedChunks = new Dictionary<int, LuaChunk>();

		private readonly LuaGlobal _sandboxedGlobal;

		private bool _packsWarning;

		internal ScriptEngine ScriptEngine { get; set; }

		[LuaMember("World", false)]
		public World World { get; } = new World();


		[LuaMember("Packs", false)]
		public World Packs
		{
			get
			{
				if (!_packsWarning)
				{
					ScriptEngine.PushMessage("`Packs` is deprecated.  Use `World` instead.", -1);
					_packsWarning = true;
				}
				return World;
			}
		}

		[LuaMember("Mumble", false)]
		public Gw2MumbleService Mumble => GameService.Gw2Mumble;

		[LuaMember("Debug", false)]
		public Debug Debug { get; }

		[LuaMember("I", false)]
		public Instance I { get; } = new Instance();


		[LuaMember("Menu", false)]
		public Menu Menu { get; } = new Menu("Scripts", null);


		[LuaMember("Event", false)]
		public Event Event { get; }

		[LuaMember("User", false)]
		public User User { get; }

		[LuaMember("_VERSION", false)]
		public virtual string Version => "NeoLua 5.3";

		[LuaMember("coroutine", false)]
		private static dynamic LuaLibraryCoroutine => LuaType.GetType(typeof(LuaThread));

		[LuaMember("bit32", false)]
		private static dynamic LuaLibraryBit32 => LuaType.GetType(typeof(LuaLibraryBit32));

		[LuaMember("math", false)]
		private static dynamic LuaLibraryMath => LuaType.GetType(typeof(LuaLibraryMath));

		[LuaMember("string", false)]
		private static dynamic LuaLibraryString => LuaType.GetType(typeof(LuaLibraryString));

		[LuaMember("table", false)]
		private static dynamic LuaLibraryTable => LuaType.GetType(typeof(LuaTable));

		[LuaMember("os", false)]
		private static dynamic LuaLibraryOS => LuaType.GetType(typeof(LuaLibraryOS));

		public PathingGlobal(Lua lua)
		{
			_sandboxedGlobal = new LuaGlobal(lua);
			Debug = new Debug(this);
			Event = new Event(this);
			User = new User(this);
		}

		[LuaMember("require", false)]
		public LuaResult Require(string scriptName, IPackResourceManager resourceManager)
		{
			scriptName = scriptName.ToLowerInvariant();
			if (!scriptName.EndsWith(".lua"))
			{
				scriptName += ".lua";
			}
			int lookup = HashCode.Combine<string, IPackResourceManager>(scriptName, resourceManager);
			if (!_loadedChunks.TryGetValue(lookup, out var chunk))
			{
				Task<LuaChunk> loadScript = ScriptEngine.LoadScript(scriptName, resourceManager);
				loadScript.Wait();
				if (loadScript.Result != null)
				{
					chunk = (_loadedChunks[lookup] = loadScript.Result);
				}
			}
			if (chunk != null)
			{
				return new LuaResult(chunk);
			}
			return LuaResult.Empty;
		}

		[LuaMember("getmetatable", false)]
		public static LuaTable LuaGetMetaTable(object obj)
		{
			return LuaGlobal.LuaGetMetaTable(obj);
		}

		[LuaMember("rawmembers", false)]
		public static IEnumerable<KeyValuePair<string, object>> LuaRawMembers(LuaTable t)
		{
			return LuaGlobal.LuaRawMembers(t);
		}

		[LuaMember("rawarray", false)]
		public static IList<object> LuaRawArray(LuaTable t)
		{
			return LuaGlobal.LuaRawArray(t);
		}

		[LuaMember("ipairs", false)]
		public static LuaResult LuaIPairs(LuaTable t)
		{
			return LuaGlobal.LuaIPairs(t);
		}

		[LuaMember("mpairs", false)]
		public static LuaResult LuaMPairs(LuaTable t)
		{
			return LuaGlobal.LuaMPairs(t);
		}

		[LuaMember("pairs", false)]
		public static LuaResult LuaPairs(LuaTable t)
		{
			return LuaGlobal.LuaPairs(t);
		}

		[LuaMember("next", false)]
		private static LuaResult LuaNext(LuaTable t, object next)
		{
			if (t == null)
			{
				return null;
			}
			object i = t.NextKey(next);
			return new LuaResult(i, t[i]);
		}

		[LuaMember("nextKey", false)]
		private static object LuaNextKey(LuaTable t, object next)
		{
			return t?.NextKey(next);
		}

		[LuaMember("print", false)]
		private void LuaPrint(params object[] args)
		{
			if (args != null)
			{
				Debug.Print(string.Join(" ", args.Select((object a) => (a != null) ? a.ToString() : string.Empty)));
			}
		}

		[LuaMember("rawequal", false)]
		public static bool LuaRawEqual(object a, object b)
		{
			return LuaGlobal.LuaRawEqual(a, b);
		}

		[LuaMember("rawget", false)]
		public static object LuaRawGet(LuaTable t, object index)
		{
			return LuaGlobal.LuaRawGet(t, index);
		}

		[LuaMember("rawlen", false)]
		public static int LuaRawLen(object v)
		{
			return LuaGlobal.LuaRawLen(v);
		}

		[LuaMember("rawset", false)]
		public static LuaTable LuaRawSet(LuaTable t, object index, object value)
		{
			return LuaGlobal.LuaRawSet(t, index, value);
		}

		[LuaMember("select", false)]
		public static LuaResult LuaSelect(string index, params object[] values)
		{
			return LuaGlobal.LuaSelect(index, values);
		}

		[LuaMember("setmetatable", false)]
		public static LuaTable LuaSetMetaTable(LuaTable t, LuaTable metaTable)
		{
			return LuaGlobal.LuaSetMetaTable(t, metaTable);
		}

		[LuaMember("tonumber", false)]
		public object LuaToNumber(object v, int? iBase = null)
		{
			return _sandboxedGlobal.LuaToNumber(v, iBase);
		}

		[LuaMember("tostring", false)]
		public static string LuaToString(object v)
		{
			return LuaGlobal.LuaToString(v);
		}

		[LuaMember("type", false)]
		public static string LuaTypeTest(object v, bool clr = false)
		{
			return LuaGlobal.LuaTypeTest(v, clr);
		}

		internal void Update(GameTime gameTime)
		{
			Event.Update(gameTime);
		}
	}
}
