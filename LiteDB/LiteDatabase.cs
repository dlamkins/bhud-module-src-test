using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB.Engine;

namespace LiteDB
{
	public class LiteDatabase : ILiteDatabase, IDisposable
	{
		private readonly ILiteEngine _engine;

		private readonly BsonMapper _mapper;

		private readonly bool _disposeOnClose;

		private ILiteStorage<string> _fs;

		public BsonMapper Mapper => _mapper;

		public ILiteStorage<string> FileStorage => _fs ?? (_fs = GetStorage<string>());

		public int UserVersion
		{
			get
			{
				return _engine.Pragma("USER_VERSION");
			}
			set
			{
				_engine.Pragma("USER_VERSION", value);
			}
		}

		public TimeSpan Timeout
		{
			get
			{
				return TimeSpan.FromSeconds(_engine.Pragma("TIMEOUT").AsInt32);
			}
			set
			{
				_engine.Pragma("TIMEOUT", (int)value.TotalSeconds);
			}
		}

		public bool UtcDate
		{
			get
			{
				return _engine.Pragma("UTC_DATE");
			}
			set
			{
				_engine.Pragma("UTC_DATE", value);
			}
		}

		public long LimitSize
		{
			get
			{
				return _engine.Pragma("LIMIT_SIZE");
			}
			set
			{
				_engine.Pragma("LIMIT_SIZE", value);
			}
		}

		public int CheckpointSize
		{
			get
			{
				return _engine.Pragma("CHECKPOINT");
			}
			set
			{
				_engine.Pragma("CHECKPOINT", value);
			}
		}

		public Collation Collation => new Collation(_engine.Pragma("COLLATION").AsString);

		public LiteDatabase(string connectionString, BsonMapper mapper = null)
			: this(new ConnectionString(connectionString), mapper)
		{
		}

		public LiteDatabase(ConnectionString connectionString, BsonMapper mapper = null)
		{
			if (connectionString == null)
			{
				throw new ArgumentNullException("connectionString");
			}
			if (connectionString.Upgrade)
			{
				LiteEngine.Upgrade(connectionString.Filename, connectionString.Password, connectionString.Collation);
			}
			_engine = connectionString.CreateEngine();
			_mapper = mapper ?? BsonMapper.Global;
			_disposeOnClose = true;
		}

		public LiteDatabase(Stream stream, BsonMapper mapper = null, Stream logStream = null)
		{
			EngineSettings settings = new EngineSettings
			{
				DataStream = (stream ?? throw new ArgumentNullException("stream")),
				LogStream = logStream
			};
			_engine = new LiteEngine(settings);
			_mapper = mapper ?? BsonMapper.Global;
			_disposeOnClose = true;
		}

		public LiteDatabase(ILiteEngine engine, BsonMapper mapper = null, bool disposeOnClose = true)
		{
			_engine = engine ?? throw new ArgumentNullException("engine");
			_mapper = mapper ?? BsonMapper.Global;
			_disposeOnClose = disposeOnClose;
		}

		public ILiteCollection<T> GetCollection<T>(string name, BsonAutoId autoId = BsonAutoId.ObjectId)
		{
			return new LiteCollection<T>(name, autoId, _engine, _mapper);
		}

		public ILiteCollection<T> GetCollection<T>()
		{
			return GetCollection<T>(null);
		}

		public ILiteCollection<T> GetCollection<T>(BsonAutoId autoId)
		{
			return GetCollection<T>(null, autoId);
		}

		public ILiteCollection<BsonDocument> GetCollection(string name, BsonAutoId autoId = BsonAutoId.ObjectId)
		{
			if (name.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("name");
			}
			return new LiteCollection<BsonDocument>(name, autoId, _engine, _mapper);
		}

		public bool BeginTrans()
		{
			return _engine.BeginTrans();
		}

		public bool Commit()
		{
			return _engine.Commit();
		}

		public bool Rollback()
		{
			return _engine.Rollback();
		}

		public ILiteStorage<TFileId> GetStorage<TFileId>(string filesCollection = "_files", string chunksCollection = "_chunks")
		{
			return new LiteStorage<TFileId>(this, filesCollection, chunksCollection);
		}

		public IEnumerable<string> GetCollectionNames()
		{
			return (from x in GetCollection("$cols").Query().Where("type = 'user'").ToDocuments()
				select x["name"].AsString).ToArray();
		}

		public bool CollectionExists(string name)
		{
			if (name.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("name");
			}
			return GetCollectionNames().Contains(name, StringComparer.OrdinalIgnoreCase);
		}

		public bool DropCollection(string name)
		{
			if (name.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("name");
			}
			return _engine.DropCollection(name);
		}

		public bool RenameCollection(string oldName, string newName)
		{
			if (oldName.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("oldName");
			}
			if (newName.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("newName");
			}
			return _engine.RenameCollection(oldName, newName);
		}

		public IBsonDataReader Execute(TextReader commandReader, BsonDocument parameters = null)
		{
			if (commandReader == null)
			{
				throw new ArgumentNullException("commandReader");
			}
			Tokenizer tokenizer = new Tokenizer(commandReader);
			return new SqlParser(_engine, tokenizer, parameters).Execute();
		}

		public IBsonDataReader Execute(string command, BsonDocument parameters = null)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			Tokenizer tokenizer = new Tokenizer(command);
			return new SqlParser(_engine, tokenizer, parameters).Execute();
		}

		public IBsonDataReader Execute(string command, params BsonValue[] args)
		{
			BsonDocument p = new BsonDocument();
			int index = 0;
			foreach (BsonValue arg in args)
			{
				p[index.ToString()] = arg;
				index++;
			}
			return Execute(command, p);
		}

		public void Checkpoint()
		{
			_engine.Checkpoint();
		}

		public long Rebuild(RebuildOptions options = null)
		{
			return _engine.Rebuild(options);
		}

		public BsonValue Pragma(string name)
		{
			return _engine.Pragma(name);
		}

		public BsonValue Pragma(string name, BsonValue value)
		{
			return _engine.Pragma(name, value);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~LiteDatabase()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && _disposeOnClose)
			{
				_engine.Dispose();
			}
		}
	}
}
