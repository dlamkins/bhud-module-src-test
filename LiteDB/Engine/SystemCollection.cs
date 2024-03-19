using System;
using System.Collections.Generic;

namespace LiteDB.Engine
{
	internal class SystemCollection
	{
		private readonly string _name;

		private readonly Func<IEnumerable<BsonDocument>> _input;

		public string Name => _name;

		public SystemCollection(string name)
		{
			if (!name.StartsWith("$"))
			{
				throw new ArgumentException("System collection name must starts with $");
			}
			_name = name;
		}

		public SystemCollection(string name, Func<IEnumerable<BsonDocument>> input)
			: this(name)
		{
			_input = input;
		}

		public virtual IEnumerable<BsonDocument> Input(BsonValue options)
		{
			return _input();
		}

		public virtual int Output(IEnumerable<BsonDocument> source, BsonValue options)
		{
			throw new LiteException(0, _name + " do not support as output collection");
		}

		protected static BsonValue GetOption(BsonValue options, string key)
		{
			return GetOption(options, key, null);
		}

		protected static BsonValue GetOption(BsonValue options, string key, BsonValue defaultValue)
		{
			if (options != null && options.IsDocument)
			{
				if (options.AsDocument.TryGetValue(key, out var value))
				{
					if (defaultValue == null || value.Type == defaultValue.Type)
					{
						return value;
					}
					throw new LiteException(0, $"Parameter `{key}` expect {defaultValue.Type} value type");
				}
				return defaultValue;
			}
			if (!(defaultValue == null))
			{
				return defaultValue;
			}
			return options;
		}
	}
}
