using System;

namespace BhModule.Community.Pathing.Scripting.Lib
{
	public class Storage
	{
		private const string GLOBAL_NAMESPACE = "000";

		private const string KEY_PATTERN = "{0}\\{1}";

		private readonly PathingGlobal _global;

		internal Storage(PathingGlobal global)
		{
			_global = global;
		}

		private string ValidateKey(string ns, string name)
		{
			if (ns == null)
			{
				ns = "000";
			}
			if (ns.Length > 64 || string.IsNullOrWhiteSpace(ns))
			{
				throw new ArgumentException("Key namespace must be nil or between 1 and 64 characters");
			}
			if (name.Length > 64 || string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Key name must be between 1 and 64 characters");
			}
			return $"{ns}\\{name}";
		}

		public string UpsertValue(string ns, string name, string value)
		{
			string key = ValidateKey(ns, name);
			return _global.ScriptEngine.Module.PackInitiator.PackState.KvStates.UpsertValue(key, value);
		}

		public string UpsertValue(string name, string value)
		{
			return UpsertValue(null, name, value);
		}

		public string ReadValue(string ns, string name)
		{
			string key = ValidateKey(ns, name);
			return _global.ScriptEngine.Module.PackInitiator.PackState.KvStates.ReadValue(key);
		}

		public string ReadValue(string name)
		{
			return ReadValue(null, name);
		}

		public void DeleteValue(string ns, string name)
		{
			string key = ValidateKey(ns, name);
			_global.ScriptEngine.Module.PackInitiator.PackState.KvStates.DeleteValue(key);
		}

		public void DeleteValue(string name)
		{
			DeleteValue(null, name);
		}
	}
}
