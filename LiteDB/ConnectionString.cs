using System;
using System.Collections.Generic;
using LiteDB.Engine;

namespace LiteDB
{
	public class ConnectionString
	{
		private readonly Dictionary<string, string> _values;

		public ConnectionType Connection { get; set; }

		public string Filename { get; set; } = "";


		public string Password { get; set; }

		public long InitialSize { get; set; }

		public bool ReadOnly { get; set; }

		public bool Upgrade { get; set; }

		public Collation Collation { get; set; }

		public string this[string key] => _values.GetOrDefault(key);

		public ConnectionString()
		{
			_values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}

		public ConnectionString(string connectionString)
			: this()
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new ArgumentNullException("connectionString");
			}
			if (connectionString.Contains("="))
			{
				_values.ParseKeyValue(connectionString);
			}
			else
			{
				_values["filename"] = connectionString;
			}
			Connection = _values.GetValue("connection", Connection);
			Filename = _values.GetValue("filename", Filename).Trim();
			Password = _values.GetValue("password", Password);
			if (Password == string.Empty)
			{
				Password = null;
			}
			InitialSize = _values.GetFileSize("initial size", InitialSize);
			ReadOnly = _values.GetValue("readonly", ReadOnly);
			Collation = (_values.ContainsKey("collation") ? new Collation(_values.GetValue<string>("collation")) : Collation);
			Upgrade = _values.GetValue("upgrade", Upgrade);
		}

		internal ILiteEngine CreateEngine()
		{
			EngineSettings settings = new EngineSettings
			{
				Filename = Filename,
				Password = Password,
				InitialSize = InitialSize,
				ReadOnly = ReadOnly,
				Collation = Collation
			};
			if (Connection == ConnectionType.Direct)
			{
				return new LiteEngine(settings);
			}
			if (Connection == ConnectionType.Shared)
			{
				return new SharedEngine(settings);
			}
			throw new NotImplementedException();
		}
	}
}
