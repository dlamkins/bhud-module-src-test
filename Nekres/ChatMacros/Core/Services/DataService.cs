using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Input;
using LiteDB;
using Nekres.ChatMacros.Core.Services.Data;
using Newtonsoft.Json;

namespace Nekres.ChatMacros.Core.Services
{
	internal class DataService : IDisposable
	{
		private ConnectionString _connectionString;

		private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

		private ManualResetEvent _lockReleased = new ManualResetEvent(initialState: false);

		private bool _lockAcquired;

		public const string TBL_CHATLINES = "chat_lines";

		private const string TBL_CHATMACROS = "chat_macros";

		private const string LITEDB_FILENAME = "macros.db";

		public event EventHandler<ValueEventArgs<BaseMacro>> LinkFileChange;

		public DataService()
		{
			_connectionString = new ConnectionString
			{
				Filename = Path.Combine(ChatMacros.Instance.ModuleDirectory, "macros.db"),
				Connection = ConnectionType.Shared
			};
			BsonMapper.Global.RegisterType((KeyBinding binding) => JsonConvert.SerializeObject((object)binding, IncludePropertyResolver.Settings("PrimaryKey", "ModifierKeys")), delegate(BsonValue bson)
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				object obj = ((object)JsonConvert.DeserializeObject<KeyBinding>((string)bson)) ?? ((object)new KeyBinding());
				((KeyBinding)obj).set_Enabled(false);
				return (KeyBinding)obj;
			});
		}

		private bool Upsert<T>(T model, string table)
		{
			LockUtil.Acquire(_rwLock, _lockReleased, ref _lockAcquired);
			try
			{
				using LiteDatabase db = new LiteDatabase(_connectionString);
				db.GetCollection<T>(table).Upsert(model);
				return true;
			}
			catch (Exception e)
			{
				ChatMacros.Logger.Warn(e, e.Message);
				return false;
			}
			finally
			{
				LockUtil.Release(_rwLock, _lockReleased, ref _lockAcquired);
			}
		}

		public bool Upsert(ChatMacro model)
		{
			return Upsert(model, "chat_macros");
		}

		public void LinkFileChanged(ChatMacro macro)
		{
			Upsert(macro);
			if (!macro.LinkFile.IsNullOrWhiteSpace() && !macro.LinkFile.IsWebLink())
			{
				this.LinkFileChange?.Invoke(this, new ValueEventArgs<BaseMacro>((BaseMacro)macro));
			}
		}

		private bool InsertMany<T>(List<T> model, string table)
		{
			LockUtil.Acquire(_rwLock, _lockReleased, ref _lockAcquired);
			try
			{
				using LiteDatabase db = new LiteDatabase(_connectionString);
				return db.GetCollection<T>(table).InsertBulk(model) > 0;
			}
			catch (Exception e)
			{
				ChatMacros.Logger.Warn(e, e.Message);
				return false;
			}
			finally
			{
				LockUtil.Release(_rwLock, _lockReleased, ref _lockAcquired);
			}
		}

		public bool Upsert(ChatLine model)
		{
			return Upsert(model, "chat_lines");
		}

		public bool Insert(params ChatLine[] chatLines)
		{
			return InsertMany(chatLines.ToList(), "chat_lines");
		}

		public List<ChatMacro> GetActiveMacros()
		{
			LockUtil.Acquire(_rwLock, _lockReleased, ref _lockAcquired);
			try
			{
				using LiteDatabase db = new LiteDatabase(_connectionString);
				IEnumerable<ChatMacro> source = db.GetCollection<ChatMacro>("chat_macros").Include((ChatMacro x) => x.Lines).Find((ChatMacro x) => x.Lines != null && x.Lines.Any());
				GameMode mode = MapUtil.GetCurrentGameMode();
				return source.Where((ChatMacro x) => ((x.MapIds == null || !x.MapIds.Any()) && (x.GameModes == GameMode.None || (x.GameModes & mode) == mode)) || (x.MapIds != null && x.MapIds.Any() && x.MapIds.Contains(GameService.Gw2Mumble.get_CurrentMap().get_Id()))).ToList();
			}
			catch (Exception e)
			{
				ChatMacros.Logger.Warn(e, e.Message);
			}
			finally
			{
				LockUtil.Release(_rwLock, _lockReleased, ref _lockAcquired);
			}
			return Enumerable.Empty<ChatMacro>().ToList();
		}

		public List<ChatMacro> GetAllMacros()
		{
			LockUtil.Acquire(_rwLock, _lockReleased, ref _lockAcquired);
			try
			{
				using LiteDatabase db = new LiteDatabase(_connectionString);
				return db.GetCollection<ChatMacro>("chat_macros").Include((ChatMacro x) => x.Lines).FindAll()
					.ToList();
			}
			catch (Exception e)
			{
				ChatMacros.Logger.Warn(e, e.Message);
			}
			finally
			{
				LockUtil.Release(_rwLock, _lockReleased, ref _lockAcquired);
			}
			return Enumerable.Empty<ChatMacro>().ToList();
		}

		public ChatMacro GetChatMacro(BsonValue id)
		{
			LockUtil.Acquire(_rwLock, _lockReleased, ref _lockAcquired);
			try
			{
				using LiteDatabase db = new LiteDatabase(_connectionString);
				return db.GetCollection<ChatMacro>("chat_macros").Include((ChatMacro macro) => macro.Lines).FindById(id);
			}
			catch (Exception e)
			{
				ChatMacros.Logger.Warn(e, e.Message);
			}
			finally
			{
				LockUtil.Release(_rwLock, _lockReleased, ref _lockAcquired);
			}
			return null;
		}

		private bool Delete<T>(string table, params BsonValue[] ids)
		{
			LockUtil.Acquire(_rwLock, _lockReleased, ref _lockAcquired);
			try
			{
				using LiteDatabase db = new LiteDatabase(_connectionString);
				return db.GetCollection<T>(table).DeleteMany(Query.In("_id", ids)) == ids.Length;
			}
			catch (Exception e)
			{
				ChatMacros.Logger.Warn(e, e.Message);
				return false;
			}
			finally
			{
				LockUtil.Release(_rwLock, _lockReleased, ref _lockAcquired);
			}
		}

		public bool Delete(ChatMacro macro)
		{
			ChatMacros.Instance.Macro.Observer.Remove(macro.Id);
			return Delete<ChatMacro>("chat_macros", new BsonValue[1] { macro.Id });
		}

		public bool Delete(ChatLine line)
		{
			return Delete<ChatLine>("chat_lines", new BsonValue[1] { line.Id });
		}

		public bool DeleteMany(IEnumerable<ChatLine> lines)
		{
			return Delete<ChatLine>("chat_lines", lines.Select((ChatLine line) => new BsonValue(line.Id)).ToArray());
		}

		public void Dispose()
		{
			if (_lockAcquired)
			{
				_lockReleased.WaitOne(500);
			}
			_lockReleased.Dispose();
			try
			{
				_rwLock.Dispose();
			}
			catch (Exception ex)
			{
				ChatMacros.Logger.Debug(ex, ex.Message);
			}
		}
	}
}
