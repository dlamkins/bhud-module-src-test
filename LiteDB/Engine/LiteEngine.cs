using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LiteDB.Engine
{
	public class LiteEngine : ILiteEngine, IDisposable
	{
		private readonly ConcurrentDictionary<string, long> _sequences = new ConcurrentDictionary<string, long>(StringComparer.OrdinalIgnoreCase);

		private readonly Dictionary<string, SystemCollection> _systemCollections = new Dictionary<string, SystemCollection>(StringComparer.OrdinalIgnoreCase);

		private readonly LockService _locker;

		private readonly DiskService _disk;

		private readonly WalIndexService _walIndex;

		private readonly HeaderPage _header;

		private readonly TransactionMonitor _monitor;

		private readonly SortDisk _sortDisk;

		private readonly EngineSettings _settings;

		private bool _disposed;

		public IEnumerable<string> GetCollectionNames()
		{
			return from x in _header.GetCollections()
				select x.Key;
		}

		public bool DropCollection(string name)
		{
			if (name.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("name");
			}
			if (_locker.IsInTransaction)
			{
				throw LiteException.AlreadyExistsTransaction();
			}
			return AutoTransaction(delegate(TransactionService transaction)
			{
				Snapshot snapshot = transaction.CreateSnapshot(LockMode.Write, name, addIfNotExists: false);
				if (snapshot.CollectionPage == null)
				{
					return false;
				}
				snapshot.DropCollection(transaction.Safepoint);
				_sequences.TryRemove(name, out var _);
				return true;
			});
		}

		public bool RenameCollection(string collection, string newName)
		{
			if (collection.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("collection");
			}
			if (newName.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("newName");
			}
			if (_locker.IsInTransaction)
			{
				throw LiteException.AlreadyExistsTransaction();
			}
			if (collection.Equals(newName, StringComparison.OrdinalIgnoreCase))
			{
				throw LiteException.InvalidCollectionName(newName, "New name must be different from current name");
			}
			CollectionService.CheckName(newName, _header);
			if (_locker.IsInTransaction)
			{
				throw LiteException.AlreadyExistsTransaction();
			}
			return AutoTransaction(delegate(TransactionService transaction)
			{
				Snapshot snapshot = transaction.CreateSnapshot(LockMode.Write, collection, addIfNotExists: false);
				transaction.CreateSnapshot(LockMode.Write, newName, addIfNotExists: false);
				if (snapshot.CollectionPage == null)
				{
					return false;
				}
				if (_header.GetCollectionPageID(newName) != uint.MaxValue)
				{
					throw LiteException.AlreadyExistsCollectionName(newName);
				}
				transaction.Pages.Commit += delegate(HeaderPage h)
				{
					h.RenameCollection(collection, newName);
				};
				return true;
			});
		}

		public int Delete(string collection, IEnumerable<BsonValue> ids)
		{
			if (collection.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("collection");
			}
			if (ids == null)
			{
				throw new ArgumentNullException("ids");
			}
			return AutoTransaction(delegate(TransactionService transaction)
			{
				Snapshot snapshot = transaction.CreateSnapshot(LockMode.Write, collection, addIfNotExists: false);
				CollectionPage collectionPage = snapshot.CollectionPage;
				DataService dataService = new DataService(snapshot);
				IndexService indexService = new IndexService(snapshot, _header.Pragmas.Collation);
				if (collectionPage == null)
				{
					return 0;
				}
				int num = 0;
				CollectionIndex pK = collectionPage.PK;
				foreach (BsonValue current in ids)
				{
					IndexNode indexNode = indexService.Find(pK, current, sibling: false, 1);
					if (indexNode != null)
					{
						dataService.Delete(indexNode.DataBlock);
						indexService.DeleteAll(indexNode.Position);
						transaction.Safepoint();
						num++;
					}
				}
				return num;
			});
		}

		public int DeleteMany(string collection, BsonExpression predicate)
		{
			if (collection.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("collection");
			}
			if (predicate != null && predicate.Type == BsonExpressionType.Equal && predicate.Left.Type == BsonExpressionType.Path && predicate.Left.Source == "$._id" && predicate.Right.IsValue)
			{
				BsonValue id = predicate.Right.Execute(_header.Pragmas.Collation).First();
				return Delete(collection, new BsonValue[1] { id });
			}
			return Delete(collection, getIds());
			IEnumerable<BsonValue> getIds()
			{
				Query query = new Query
				{
					Select = "{ i: _id }",
					ForUpdate = true
				};
				if (predicate != null)
				{
					query.Where.Add(predicate);
				}
				using IBsonDataReader reader = Query(collection, query);
				while (reader.Read())
				{
					BsonValue value = reader.Current["i"];
					if (value != BsonValue.Null)
					{
						yield return value;
					}
				}
			}
		}

		public bool EnsureIndex(string collection, string name, BsonExpression expression, bool unique)
		{
			if (collection.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("collection");
			}
			if (name.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("name");
			}
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			if (!expression.IsIndexable)
			{
				throw new ArgumentException("Index expressions must contains at least one document field. Used methods must be immutable. Parameters are not supported.", "expression");
			}
			if (name.Length > Constants.INDEX_NAME_MAX_LENGTH)
			{
				throw LiteException.InvalidIndexName(name, collection, "MaxLength = " + Constants.INDEX_NAME_MAX_LENGTH);
			}
			if (!name.IsWord())
			{
				throw LiteException.InvalidIndexName(name, collection, "Use only [a-Z$_]");
			}
			if (name.StartsWith("$"))
			{
				throw LiteException.InvalidIndexName(name, collection, "Index name can't start with `$`");
			}
			if (!expression.IsScalar && unique)
			{
				throw new LiteException(0, "Multikey index expression do not support unique option");
			}
			if (expression.Source == "$._id")
			{
				return false;
			}
			return AutoTransaction(delegate(TransactionService transaction)
			{
				Snapshot snapshot = transaction.CreateSnapshot(LockMode.Write, collection, addIfNotExists: true);
				CollectionPage collectionPage = snapshot.CollectionPage;
				IndexService indexService = new IndexService(snapshot, _header.Pragmas.Collation);
				DataService dataService = new DataService(snapshot);
				CollectionIndex collectionIndex = collectionPage.GetCollectionIndex(name);
				if (collectionIndex != null)
				{
					if (collectionIndex.Expression != expression.Source)
					{
						throw LiteException.IndexAlreadyExist(name);
					}
					return false;
				}
				CollectionIndex index = indexService.CreateIndex(name, expression.Source, unique);
				uint num = 0u;
				foreach (IndexNode current in new IndexAll("_id", 1).Run(collectionPage, indexService))
				{
					using (BufferReader bufferReader = new BufferReader(dataService.Read(current.DataBlock)))
					{
						BsonDocument doc = bufferReader.ReadDocument(expression.Fields);
						IndexNode indexNode = null;
						IndexNode indexNode2 = null;
						foreach (BsonValue current2 in expression.GetIndexKeys(doc, _header.Pragmas.Collation))
						{
							IndexNode indexNode3 = indexService.AddNode(index, current2, current.DataBlock, indexNode);
							if (indexNode2 == null)
							{
								indexNode2 = indexNode3;
							}
							indexNode = indexNode3;
							num++;
						}
						if (indexNode2 != null)
						{
							indexNode.SetNextNode(current.NextNode);
							current.SetNextNode(indexNode2.Position);
						}
					}
					transaction.Safepoint();
				}
				return true;
			});
		}

		public bool DropIndex(string collection, string name)
		{
			if (collection.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("collection");
			}
			if (name.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("name");
			}
			if (name == "_id")
			{
				throw LiteException.IndexDropId();
			}
			return AutoTransaction(delegate(TransactionService transaction)
			{
				Snapshot snapshot = transaction.CreateSnapshot(LockMode.Write, collection, addIfNotExists: false);
				CollectionPage collectionPage = snapshot.CollectionPage;
				IndexService indexService = new IndexService(snapshot, _header.Pragmas.Collation);
				if (collectionPage == null)
				{
					return false;
				}
				CollectionIndex collectionIndex = collectionPage.GetCollectionIndex(name);
				if (collectionIndex == null)
				{
					return false;
				}
				indexService.DropIndex(collectionIndex);
				snapshot.CollectionPage.DeleteCollectionIndex(name);
				return true;
			});
		}

		public int Insert(string collection, IEnumerable<BsonDocument> docs, BsonAutoId autoId)
		{
			if (collection.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("collection");
			}
			if (docs == null)
			{
				throw new ArgumentNullException("docs");
			}
			return AutoTransaction(delegate(TransactionService transaction)
			{
				Snapshot snapshot = transaction.CreateSnapshot(LockMode.Write, collection, addIfNotExists: true);
				int num = 0;
				IndexService indexer = new IndexService(snapshot, _header.Pragmas.Collation);
				DataService data = new DataService(snapshot);
				foreach (BsonDocument current in docs)
				{
					transaction.Safepoint();
					InsertDocument(snapshot, current, autoId, indexer, data);
					num++;
				}
				return num;
			});
		}

		private void InsertDocument(Snapshot snapshot, BsonDocument doc, BsonAutoId autoId, IndexService indexer, DataService data)
		{
			if (!doc.TryGetValue("_id", out var id))
			{
				id = (doc["_id"] = autoId switch
				{
					BsonAutoId.Guid => new BsonValue(Guid.NewGuid()), 
					BsonAutoId.ObjectId => new BsonValue(ObjectId.NewObjectId()), 
					_ => GetSequence(snapshot, autoId), 
				});
			}
			else if (id.IsNumber)
			{
				SetSequence(snapshot, id);
			}
			if (id.IsNull || id.IsMinValue || id.IsMaxValue)
			{
				throw LiteException.InvalidDataType("_id", id);
			}
			PageAddress dataBlock = data.Insert(doc);
			IndexNode last = null;
			foreach (CollectionIndex index in snapshot.CollectionPage.GetCollectionIndexes())
			{
				foreach (BsonValue key in index.BsonExpr.GetIndexKeys(doc, _header.Pragmas.Collation))
				{
					last = indexer.AddNode(index, key, dataBlock, last);
				}
			}
		}

		public BsonValue Pragma(string name)
		{
			return _header.Pragmas.Get(name);
		}

		public bool Pragma(string name, BsonValue value)
		{
			if (Pragma(name) == value)
			{
				return false;
			}
			if (_locker.IsInTransaction)
			{
				throw LiteException.AlreadyExistsTransaction();
			}
			return AutoTransaction(delegate(TransactionService transaction)
			{
				transaction.Pages.Commit += delegate(HeaderPage h)
				{
					h.Pragmas.Set(name, value, validate: true);
				};
				return true;
			});
		}

		public IBsonDataReader Query(string collection, Query query)
		{
			if (string.IsNullOrWhiteSpace(collection))
			{
				throw new ArgumentNullException("collection");
			}
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}
			IEnumerable<BsonDocument> source = null;
			if (collection.StartsWith("$"))
			{
				SqlParser.ParseCollection(new Tokenizer(collection), out var name, out var options);
				SystemCollection systemCollection = GetSystemCollection(name);
				source = systemCollection.Input(options);
				collection = systemCollection.Name;
			}
			return new QueryExecutor(this, _monitor, _sortDisk, _header.Pragmas, collection, query, source).ExecuteQuery();
		}

		public long Rebuild(RebuildOptions options)
		{
			bool mustExit = _locker.EnterExclusive();
			PageBuffer savepoint = null;
			try
			{
				_walIndex.Checkpoint();
				long originalLength = _disk.GetVirtualLength(FileOrigin.Data);
				savepoint = _header.Savepoint();
				_disk.Cache.Clear();
				if (_disk.GetVirtualLength(FileOrigin.Log) > 0)
				{
					throw new LiteException(0, "Rebuild operation requires no log file - run Checkpoint before continue");
				}
				FileReaderV8 reader = new FileReaderV8(_header, _disk);
				_header.FreeEmptyPageList = uint.MaxValue;
				_header.LastPageID = 0u;
				_header.GetCollections().ToList().ForEach(delegate(KeyValuePair<string, uint> c)
				{
					_header.DeleteCollection(c.Key);
				});
				if (options?.Collation != null)
				{
					_header.Pragmas.Set("COLLATION", options.Collation.ToString(), validate: false);
				}
				RebuildContent(reader);
				if (options != null)
				{
					_disk.ChangePassword(options.Password, _settings);
				}
				_walIndex.Checkpoint();
				_disk.Write(new PageBuffer[1] { _header.UpdateBuffer() }, FileOrigin.Data);
				_disk.SetLength((_header.LastPageID + 1) * 8192, FileOrigin.Data);
				long newLength = _disk.GetVirtualLength(FileOrigin.Data);
				return originalLength - newLength;
			}
			catch
			{
				if (savepoint != null)
				{
					_header.Restore(savepoint);
				}
				throw;
			}
			finally
			{
				if (mustExit)
				{
					_locker.ExitExclusive();
				}
			}
		}

		internal void RebuildContent(IFileReader reader)
		{
			bool isNew;
			TransactionService transaction = _monitor.GetTransaction(create: true, queryOnly: false, out isNew);
			try
			{
				foreach (string collection in reader.GetCollections())
				{
					foreach (IndexInfo index in reader.GetIndexes(collection))
					{
						EnsureIndex(collection, index.Name, BsonExpression.Create(index.Expression), index.Unique);
					}
					IEnumerable<BsonDocument> documents = reader.GetDocuments(collection);
					Snapshot snapshot = transaction.CreateSnapshot(LockMode.Write, collection, addIfNotExists: true);
					IndexService indexer = new IndexService(snapshot, _header.Pragmas.Collation);
					DataService data = new DataService(snapshot);
					foreach (BsonDocument doc in documents)
					{
						transaction.Safepoint();
						InsertDocument(snapshot, doc, BsonAutoId.ObjectId, indexer, data);
					}
				}
				transaction.Commit();
			}
			catch (Exception)
			{
				_walIndex.Clear();
				throw;
			}
			finally
			{
				_monitor.ReleaseTransaction(transaction);
			}
		}

		private BsonValue GetSequence(Snapshot snapshot, BsonAutoId autoId)
		{
			long next = _sequences.AddOrUpdate(snapshot.CollectionName, delegate
			{
				BsonValue lastId = GetLastId(snapshot);
				if (lastId.IsMinValue)
				{
					return 1L;
				}
				if (!lastId.IsNumber)
				{
					throw new LiteException(0, $"It's not possible use AutoId={autoId} because '{snapshot.CollectionName}' collection contains not only numbers in _id index ({lastId}).");
				}
				return lastId.AsInt64 + 1;
			}, (string s, long value) => value + 1);
			if (autoId != BsonAutoId.Int32)
			{
				return new BsonValue(next);
			}
			return new BsonValue((int)next);
		}

		private void SetSequence(Snapshot snapshot, BsonValue newId)
		{
			_sequences.AddOrUpdate(snapshot.CollectionName, delegate
			{
				BsonValue lastId = GetLastId(snapshot);
				return lastId.IsNumber ? Math.Max(lastId.AsInt64, newId.AsInt64) : newId.AsInt64;
			}, (string s, long value) => Math.Max(value, newId.AsInt64));
		}

		private BsonValue GetLastId(Snapshot snapshot)
		{
			CollectionIndex pk = snapshot.CollectionPage.PK;
			IndexPage tailPage = snapshot.GetPage<IndexPage>(pk.Tail.PageID);
			PageAddress prevNode = tailPage.GetIndexNode(pk.Tail.Index).Prev[0];
			if (prevNode == pk.Head)
			{
				return BsonValue.MinValue;
			}
			return ((prevNode.PageID == tailPage.PageID) ? tailPage : snapshot.GetPage<IndexPage>(prevNode.PageID)).GetIndexNode(prevNode.Index).Key;
		}

		internal SystemCollection GetSystemCollection(string name)
		{
			if (_systemCollections.TryGetValue(name, out var sys))
			{
				return sys;
			}
			throw new LiteException(0, "System collection '" + name + "' are not registered as system collection");
		}

		internal void RegisterSystemCollection(SystemCollection systemCollection)
		{
			if (systemCollection == null)
			{
				throw new ArgumentNullException("systemCollection");
			}
			_systemCollections[systemCollection.Name] = systemCollection;
		}

		internal void RegisterSystemCollection(string collectionName, Func<IEnumerable<BsonDocument>> factory)
		{
			if (collectionName.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("collectionName");
			}
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			_systemCollections[collectionName] = new SystemCollection(collectionName, factory);
		}

		public bool BeginTrans()
		{
			bool isNew;
			TransactionService transaction = _monitor.GetTransaction(create: true, queryOnly: false, out isNew);
			transaction.ExplicitTransaction = true;
			if (transaction.OpenCursors.Count > 0)
			{
				throw new LiteException(0, "This thread contains an open cursors/query. Close cursors before Begin()");
			}
			return isNew;
		}

		public bool Commit()
		{
			bool isNew;
			TransactionService transaction = _monitor.GetTransaction(create: false, queryOnly: false, out isNew);
			if (transaction != null)
			{
				if (transaction.OpenCursors.Count > 0)
				{
					throw new LiteException(0, "Current transaction contains open cursors. Close cursors before run Commit()");
				}
				if (transaction.State == TransactionState.Active)
				{
					CommitAndReleaseTransaction(transaction);
					return true;
				}
			}
			return false;
		}

		public bool Rollback()
		{
			bool isNew;
			TransactionService transaction = _monitor.GetTransaction(create: false, queryOnly: false, out isNew);
			if (transaction != null && transaction.State == TransactionState.Active)
			{
				transaction.Rollback();
				_monitor.ReleaseTransaction(transaction);
				return true;
			}
			return false;
		}

		private T AutoTransaction<T>(Func<TransactionService, T> fn)
		{
			bool isNew;
			TransactionService transaction = _monitor.GetTransaction(create: true, queryOnly: false, out isNew);
			try
			{
				T result = fn(transaction);
				if (isNew)
				{
					CommitAndReleaseTransaction(transaction);
				}
				return result;
			}
			catch (Exception)
			{
				transaction.Rollback();
				_monitor.ReleaseTransaction(transaction);
				throw;
			}
		}

		private void CommitAndReleaseTransaction(TransactionService transaction)
		{
			transaction.Commit();
			_monitor.ReleaseTransaction(transaction);
			if (_header.Pragmas.Checkpoint > 0 && _disk.GetVirtualLength(FileOrigin.Log) > _header.Pragmas.Checkpoint * 8192)
			{
				_walIndex.TryCheckpoint();
			}
		}

		public int Update(string collection, IEnumerable<BsonDocument> docs)
		{
			if (collection.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("collection");
			}
			if (docs == null)
			{
				throw new ArgumentNullException("docs");
			}
			return AutoTransaction(delegate(TransactionService transaction)
			{
				Snapshot snapshot = transaction.CreateSnapshot(LockMode.Write, collection, addIfNotExists: false);
				CollectionPage collectionPage = snapshot.CollectionPage;
				IndexService indexer = new IndexService(snapshot, _header.Pragmas.Collation);
				DataService data = new DataService(snapshot);
				int num = 0;
				if (collectionPage == null)
				{
					return 0;
				}
				foreach (BsonDocument current in docs)
				{
					transaction.Safepoint();
					if (UpdateDocument(snapshot, collectionPage, current, indexer, data))
					{
						num++;
					}
				}
				return num;
			});
		}

		public int UpdateMany(string collection, BsonExpression transform, BsonExpression predicate)
		{
			if (collection.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("collection");
			}
			if (transform == null)
			{
				throw new ArgumentNullException("transform");
			}
			return Update(collection, transformDocs());
			IEnumerable<BsonDocument> transformDocs()
			{
				Query q = new Query
				{
					Select = "$",
					ForUpdate = true
				};
				if (predicate != null)
				{
					q.Where.Add(predicate);
				}
				using IBsonDataReader reader = Query(collection, q);
				while (reader.Read())
				{
					BsonDocument doc = reader.Current.AsDocument;
					BsonValue id = doc["_id"];
					BsonValue value = transform.ExecuteScalar(doc, _header.Pragmas.Collation);
					if (!value.IsDocument)
					{
						throw new ArgumentException("Extend expression must return a document", "transform");
					}
					BsonDocument result = BsonExpressionMethods.EXTEND(doc, value.AsDocument).AsDocument;
					if (result.TryGetValue("_id", out var newId))
					{
						if (newId != id)
						{
							throw LiteException.InvalidUpdateField("_id");
						}
					}
					else
					{
						result["_id"] = id;
					}
					yield return result;
				}
			}
		}

		private bool UpdateDocument(Snapshot snapshot, CollectionPage col, BsonDocument doc, IndexService indexer, DataService data)
		{
			BsonValue id = doc["_id"];
			if (id.IsNull || id.IsMinValue || id.IsMaxValue)
			{
				throw LiteException.InvalidDataType("_id", id);
			}
			IndexNode pkNode = indexer.Find(col.PK, id, sibling: false, 1);
			if (pkNode == null)
			{
				return false;
			}
			data.Update(col, pkNode.DataBlock, doc);
			Tuple<byte, BsonValue, PageAddress>[] oldKeys = (from x in indexer.GetNodeList(pkNode.NextNode)
				select new Tuple<byte, BsonValue, PageAddress>(x.Slot, x.Key, x.Position)).ToArray();
			List<Tuple<byte, BsonValue, string>> newKeys = new List<Tuple<byte, BsonValue, string>>();
			foreach (CollectionIndex index2 in from x in col.GetCollectionIndexes()
				where x.Name != "_id"
				select x)
			{
				foreach (BsonValue key in index2.BsonExpr.GetIndexKeys(doc, _header.Pragmas.Collation))
				{
					newKeys.Add(new Tuple<byte, BsonValue, string>(index2.Slot, key, index2.Name));
				}
			}
			if (oldKeys.Length == 0 && newKeys.Count == 0)
			{
				return true;
			}
			HashSet<PageAddress> toDelete = new HashSet<PageAddress>(from x in oldKeys
				where !newKeys.Any((Tuple<byte, BsonValue, string> n) => n.Item1 == x.Item1 && n.Item2 == x.Item2)
				select x.Item3);
			Tuple<byte, BsonValue, string>[] toInsert = newKeys.Where((Tuple<byte, BsonValue, string> x) => !oldKeys.Any((Tuple<byte, BsonValue, PageAddress> o) => o.Item1 == x.Item1 && o.Item2 == x.Item2)).ToArray();
			if (toDelete.Count == 0 && toInsert.Length == 0)
			{
				return true;
			}
			IndexNode last = indexer.DeleteList(pkNode.Position, toDelete);
			Tuple<byte, BsonValue, string>[] array = toInsert;
			foreach (Tuple<byte, BsonValue, string> elem in array)
			{
				CollectionIndex index = col.GetCollectionIndex(elem.Item3);
				last = indexer.AddNode(index, elem.Item2, pkNode.DataBlock, last);
			}
			return true;
		}

		public static bool Upgrade(string filename, string password = null, Collation collation = null)
		{
			if (filename.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("filename");
			}
			if (!File.Exists(filename))
			{
				return false;
			}
			EngineSettings settings = new EngineSettings
			{
				Filename = filename,
				Password = password,
				Collation = collation
			};
			string backup = FileHelper.GetSufixFile(filename, "-backup");
			settings.Filename = FileHelper.GetSufixFile(filename);
			byte[] buffer = new byte[16384];
			using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				stream.Read(buffer, 0, buffer.Length);
				if ((Encoding.UTF8.GetString(buffer, 32, "** This is a LiteDB file **".Length) == "** This is a LiteDB file **" && buffer[59] == 8) || buffer[0] == 1)
				{
					return false;
				}
				if (!(Encoding.UTF8.GetString(buffer, 25, "** This is a LiteDB file **".Length) == "** This is a LiteDB file **") || buffer[52] != 7)
				{
					throw new LiteException(0, "Invalid data file format to upgrade");
				}
				IFileReader reader = new FileReaderV7(stream, password);
				using LiteEngine engine = new LiteEngine(settings);
				engine.Pragma("CHECKPOINT", 0);
				engine.RebuildContent(reader);
				engine.Checkpoint();
				engine.Pragma("CHECKPOINT", 1000);
				engine.Pragma("USER_VERSION", (reader as FileReaderV7).UserVersion);
			}
			File.Move(filename, backup);
			File.Move(settings.Filename, filename);
			return true;
		}

		public int Upsert(string collection, IEnumerable<BsonDocument> docs, BsonAutoId autoId)
		{
			if (collection.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("collection");
			}
			if (docs == null)
			{
				throw new ArgumentNullException("docs");
			}
			return AutoTransaction(delegate(TransactionService transaction)
			{
				Snapshot snapshot = transaction.CreateSnapshot(LockMode.Write, collection, addIfNotExists: true);
				CollectionPage collectionPage = snapshot.CollectionPage;
				IndexService indexer = new IndexService(snapshot, _header.Pragmas.Collation);
				DataService data = new DataService(snapshot);
				int num = 0;
				foreach (BsonDocument current in docs)
				{
					transaction.Safepoint();
					if (current["_id"] == BsonValue.Null || !UpdateDocument(snapshot, collectionPage, current, indexer, data))
					{
						InsertDocument(snapshot, current, autoId, indexer, data);
						num++;
					}
				}
				return num;
			});
		}

		public LiteEngine()
			: this(new EngineSettings
			{
				DataStream = new MemoryStream()
			})
		{
		}

		public LiteEngine(string filename)
			: this(new EngineSettings
			{
				Filename = filename
			})
		{
		}

		public LiteEngine(EngineSettings settings)
		{
			_settings = settings ?? throw new ArgumentNullException("settings");
			try
			{
				_disk = new DiskService(settings, Constants.MEMORY_SEGMENT_SIZES);
				PageBuffer buffer = _disk.ReadFull(FileOrigin.Data).First();
				if (buffer[0] == 1)
				{
					throw new LiteException(0, "This data file is encrypted and needs a password to open");
				}
				_header = new HeaderPage(buffer);
				if (settings.Collation != null && settings.Collation.ToString() != _header.Pragmas.Collation.ToString())
				{
					throw new LiteException(0, $"Datafile collation '{_header.Pragmas.Collation}' is different from engine settings. Use Rebuild database to change collation.");
				}
				_locker = new LockService(_header.Pragmas);
				_walIndex = new WalIndexService(_disk, _locker);
				if (_disk.GetVirtualLength(FileOrigin.Log) > 0)
				{
					_walIndex.RestoreIndex(ref _header);
				}
				_sortDisk = new SortDisk(settings.CreateTempFactory(), 819200, _header.Pragmas);
				_monitor = new TransactionMonitor(_header, _locker, _disk, _walIndex);
				InitializeSystemCollections();
			}
			catch (Exception)
			{
				Dispose(disposing: true);
				throw;
			}
		}

		public int Checkpoint()
		{
			return _walIndex.Checkpoint();
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~LiteEngine()
		{
			Dispose(disposing: false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}
			if (disposing)
			{
				_monitor?.Dispose();
				HeaderPage header = _header;
				if (header != null && header.Pragmas.Checkpoint > 0)
				{
					_walIndex?.TryCheckpoint();
				}
				_disk?.Dispose();
				_sortDisk?.Dispose();
				_locker?.Dispose();
			}
			_disposed = true;
		}

		private void InitializeSystemCollections()
		{
			RegisterSystemCollection("$database", () => SysDatabase());
			RegisterSystemCollection("$cols", () => SysCols());
			RegisterSystemCollection("$indexes", () => SysIndexes());
			RegisterSystemCollection("$sequences", () => SysSequences());
			RegisterSystemCollection("$transactions", () => SysTransactions());
			RegisterSystemCollection("$snapshots", () => SysSnapshots());
			RegisterSystemCollection("$open_cursors", () => SysOpenCursors());
			RegisterSystemCollection(new SysFile());
			RegisterSystemCollection(new SysDump(_header, _monitor));
			RegisterSystemCollection(new SysPageList(_header, _monitor));
			RegisterSystemCollection(new SysQuery(this));
		}

		private IEnumerable<BsonDocument> SysCols()
		{
			foreach (KeyValuePair<string, uint> col in _header.GetCollections())
			{
				yield return new BsonDocument
				{
					["name"] = col.Key,
					["type"] = "user"
				};
			}
			foreach (KeyValuePair<string, SystemCollection> item in _systemCollections)
			{
				yield return new BsonDocument
				{
					["name"] = item.Key,
					["type"] = "system"
				};
			}
		}

		private IEnumerable<BsonDocument> SysDatabase()
		{
			Version version = typeof(LiteEngine).GetTypeInfo().Assembly.GetName().Version;
			yield return new BsonDocument
			{
				["name"] = _disk.GetName(FileOrigin.Data),
				["encrypted"] = _settings.Password != null,
				["readOnly"] = _settings.ReadOnly,
				["lastPageID"] = (int)_header.LastPageID,
				["freeEmptyPageID"] = (int)_header.FreeEmptyPageList,
				["creationTime"] = _header.CreationTime,
				["dataFileSize"] = (int)_disk.GetVirtualLength(FileOrigin.Data),
				["logFileSize"] = (int)_disk.GetVirtualLength(FileOrigin.Log),
				["asyncQueueLength"] = _disk.Queue.Length,
				["currentReadVersion"] = _walIndex.CurrentReadVersion,
				["lastTransactionID"] = _walIndex.LastTransactionID,
				["engine"] = $"litedb-ce-v{version.Major}.{version.Minor}.{version.Build}",
				["pragmas"] = new BsonDocument(_header.Pragmas.Pragmas.ToDictionary((Pragma x) => x.Name, (Pragma x) => x.Get())),
				["cache"] = new BsonDocument
				{
					["extendSegments"] = _disk.Cache.ExtendSegments,
					["extendPages"] = _disk.Cache.ExtendPages,
					["freePages"] = _disk.Cache.FreePages,
					["readablePages"] = _disk.Cache.GetPages().Count,
					["writablePages"] = _disk.Cache.WritablePages,
					["pagesInUse"] = _disk.Cache.PagesInUse
				},
				["transactions"] = new BsonDocument
				{
					["open"] = _monitor.Transactions.Count,
					["maxOpenTransactions"] = 100,
					["initialTransactionSize"] = _monitor.InitialSize,
					["availableSize"] = _monitor.FreePages
				}
			};
		}

		private IEnumerable<BsonDocument> SysIndexes()
		{
			TransactionService transaction = _monitor.GetThreadTransaction();
			foreach (KeyValuePair<string, uint> collection in _header.GetCollections())
			{
				Snapshot snapshot = transaction.CreateSnapshot(LockMode.Read, collection.Key, addIfNotExists: false);
				foreach (CollectionIndex index in snapshot.CollectionPage.GetCollectionIndexes())
				{
					yield return new BsonDocument
					{
						["collection"] = collection.Key,
						["name"] = index.Name,
						["expression"] = index.Expression,
						["unique"] = index.Unique,
						["maxLevel"] = index.MaxLevel
					};
				}
			}
		}

		private IEnumerable<BsonDocument> SysOpenCursors()
		{
			foreach (TransactionService transaction in _monitor.Transactions)
			{
				foreach (CursorInfo cursor in transaction.OpenCursors)
				{
					yield return new BsonDocument
					{
						["threadID"] = transaction.ThreadID,
						["transactionID"] = (int)transaction.TransactionID,
						["elapsedMS"] = (int)cursor.Elapsed.ElapsedMilliseconds,
						["collection"] = cursor.Collection,
						["mode"] = (cursor.Query.ForUpdate ? "write" : "read"),
						["sql"] = cursor.Query.ToSQL(cursor.Collection).Replace(Environment.NewLine, " "),
						["running"] = cursor.Elapsed.IsRunning,
						["fetched"] = cursor.Fetched
					};
				}
			}
		}

		private IEnumerable<BsonDocument> SysSequences()
		{
			KeyValuePair<string, long>[] values = _sequences.ToArray();
			KeyValuePair<string, long>[] array = values;
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<string, long> value = array[i];
				yield return new BsonDocument
				{
					["collection"] = value.Key,
					["value"] = value.Value
				};
			}
		}

		private IEnumerable<BsonDocument> SysSnapshots()
		{
			foreach (TransactionService transaction in _monitor.Transactions)
			{
				foreach (Snapshot snapshot in transaction.Snapshots)
				{
					yield return new BsonDocument
					{
						["transactionID"] = (int)transaction.TransactionID,
						["collection"] = snapshot.CollectionName,
						["mode"] = snapshot.Mode.ToString(),
						["readVersion"] = snapshot.ReadVersion,
						["pagesInMemory"] = snapshot.LocalPages.Count,
						["collectionDirty"] = snapshot.CollectionPage?.IsDirty ?? false
					};
				}
			}
		}

		private IEnumerable<BsonDocument> SysTransactions()
		{
			foreach (TransactionService transaction in _monitor.Transactions)
			{
				yield return new BsonDocument
				{
					["threadID"] = transaction.ThreadID,
					["transactionID"] = (int)transaction.TransactionID,
					["startTime"] = transaction.StartTime,
					["mode"] = transaction.Mode.ToString(),
					["transactionSize"] = transaction.Pages.TransactionSize,
					["maxTransactionSize"] = transaction.MaxTransactionSize,
					["pagesInLogFile"] = transaction.Pages.DirtyPages.Count,
					["newPages"] = transaction.Pages.NewPages.Count,
					["deletedPages"] = transaction.Pages.DeletedPages,
					["modifiedPages"] = transaction.Snapshots.Select((Snapshot x) => x.GetWritablePages(dirty: true, includeCollectionPage: true).Count()).Sum()
				};
			}
		}
	}
}
