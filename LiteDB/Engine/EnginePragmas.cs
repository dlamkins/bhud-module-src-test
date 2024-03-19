using System;
using System.Collections.Generic;
using System.Globalization;

namespace LiteDB.Engine
{
	internal class EnginePragmas
	{
		private const int P_USER_VERSION = 76;

		private const int P_COLLATION_LCID = 80;

		private const int P_COLLATION_SORT = 84;

		private const int P_TIMEOUT = 88;

		private const int P_UTC_DATE = 96;

		private const int P_CHECKPOINT = 97;

		private const int P_LIMIT_SIZE = 101;

		private readonly Dictionary<string, Pragma> _pragmas;

		private bool _isDirty;

		private readonly HeaderPage _headerPage;

		public int UserVersion { get; private set; }

		public Collation Collation { get; private set; } = Collation.Default;


		public TimeSpan Timeout { get; private set; } = TimeSpan.FromMinutes(1.0);


		public long LimitSize { get; private set; } = long.MaxValue;


		public bool UtcDate { get; private set; }

		public int Checkpoint { get; private set; } = 1000;


		public IEnumerable<Pragma> Pragmas => _pragmas.Values;

		public EnginePragmas(HeaderPage headerPage)
		{
			_headerPage = headerPage;
			_pragmas = new Dictionary<string, Pragma>(StringComparer.OrdinalIgnoreCase)
			{
				["USER_VERSION"] = new Pragma
				{
					Name = "USER_VERSION",
					Get = () => UserVersion,
					Set = delegate(BsonValue v)
					{
						UserVersion = v.AsInt32;
					},
					Read = delegate(BufferSlice b)
					{
						UserVersion = b.ReadInt32(76);
					},
					Validate = delegate
					{
					},
					Write = delegate(BufferSlice b)
					{
						b.Write(UserVersion, 76);
					}
				},
				["COLLATION"] = new Pragma
				{
					Name = "COLLATION",
					Get = () => Collation.ToString(),
					Set = delegate(BsonValue v)
					{
						Collation = new Collation(v.AsString);
					},
					Read = delegate(BufferSlice b)
					{
						Collation = new Collation(b.ReadInt32(80), (CompareOptions)b.ReadInt32(84));
					},
					Validate = delegate
					{
						throw new LiteException(0, "Pragma COLLATION is read only. Use Rebuild options.");
					},
					Write = delegate(BufferSlice b)
					{
						b.Write(Collation.LCID, 80);
						b.Write((int)Collation.SortOptions, 84);
					}
				},
				["TIMEOUT"] = new Pragma
				{
					Name = "TIMEOUT",
					Get = () => (int)Timeout.TotalSeconds,
					Set = delegate(BsonValue v)
					{
						Timeout = TimeSpan.FromSeconds(v.AsInt32);
					},
					Read = delegate(BufferSlice b)
					{
						Timeout = TimeSpan.FromSeconds(b.ReadInt32(88));
					},
					Validate = delegate(BsonValue v, HeaderPage h)
					{
						if (v <= 0)
						{
							throw new LiteException(0, "Pragma TIMEOUT must be greater than zero");
						}
					},
					Write = delegate(BufferSlice b)
					{
						b.Write((int)Timeout.TotalSeconds, 88);
					}
				},
				["LIMIT_SIZE"] = new Pragma
				{
					Name = "LIMIT_SIZE",
					Get = () => LimitSize,
					Set = delegate(BsonValue v)
					{
						LimitSize = v.AsInt64;
					},
					Read = delegate(BufferSlice b)
					{
						long num = b.ReadInt64(101);
						LimitSize = ((num == 0L) ? long.MaxValue : num);
					},
					Validate = delegate(BsonValue v, HeaderPage h)
					{
						if (v < 32768)
						{
							throw new LiteException(0, "Pragma LIMIT_SIZE must be at least 4 pages (32768 bytes)");
						}
						if (h != null && v.AsInt64 < (h.LastPageID + 1) * 8192)
						{
							throw new LiteException(0, "Pragma LIMIT_SIZE must be greater or equal to the current file size");
						}
					},
					Write = delegate(BufferSlice b)
					{
						b.Write(LimitSize, 101);
					}
				},
				["UTC_DATE"] = new Pragma
				{
					Name = "UTC_DATE",
					Get = () => UtcDate,
					Set = delegate(BsonValue v)
					{
						UtcDate = v.AsBoolean;
					},
					Read = delegate(BufferSlice b)
					{
						UtcDate = b.ReadBool(96);
					},
					Validate = delegate
					{
					},
					Write = delegate(BufferSlice b)
					{
						b.Write(UtcDate, 96);
					}
				},
				["CHECKPOINT"] = new Pragma
				{
					Name = "CHECKPOINT",
					Get = () => Checkpoint,
					Set = delegate(BsonValue v)
					{
						Checkpoint = v.AsInt32;
					},
					Read = delegate(BufferSlice b)
					{
						Checkpoint = b.ReadInt32(97);
					},
					Validate = delegate(BsonValue v, HeaderPage h)
					{
						if (v < 0)
						{
							throw new LiteException(0, "Pragma CHECKPOINT must be greater or equal to zero");
						}
					},
					Write = delegate(BufferSlice b)
					{
						b.Write(Checkpoint, 97);
					}
				}
			};
			_isDirty = true;
		}

		public EnginePragmas(BufferSlice buffer, HeaderPage headerPage)
			: this(headerPage)
		{
			foreach (Pragma value in _pragmas.Values)
			{
				value.Read(buffer);
			}
			_isDirty = false;
		}

		public void UpdateBuffer(BufferSlice buffer)
		{
			if (!_isDirty)
			{
				return;
			}
			foreach (KeyValuePair<string, Pragma> pragma in _pragmas)
			{
				pragma.Value.Write(buffer);
			}
			_isDirty = false;
		}

		public BsonValue Get(string name)
		{
			if (_pragmas.TryGetValue(name, out var pragma))
			{
				return pragma.Get();
			}
			throw new LiteException(0, "Pragma `" + name + "` not exist");
		}

		public void Set(string name, BsonValue value, bool validate)
		{
			if (_pragmas.TryGetValue(name, out var pragma))
			{
				if (validate)
				{
					pragma.Validate(value, _headerPage);
				}
				pragma.Set(value);
				_isDirty = true;
				return;
			}
			throw new LiteException(0, "Pragma `" + name + "` not exist");
		}
	}
}
