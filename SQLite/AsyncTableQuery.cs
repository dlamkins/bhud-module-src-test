using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SQLite
{
	public class AsyncTableQuery<T> where T : new()
	{
		private TableQuery<T> _innerQuery;

		public AsyncTableQuery(TableQuery<T> innerQuery)
		{
			_innerQuery = innerQuery;
		}

		private Task<U> ReadAsync<U>(Func<SQLiteConnectionWithLock, U> read)
		{
			return Task.Factory.StartNew(delegate
			{
				SQLiteConnectionWithLock sQLiteConnectionWithLock = (SQLiteConnectionWithLock)_innerQuery.Connection;
				using (sQLiteConnectionWithLock.Lock())
				{
					return read(sQLiteConnectionWithLock);
				}
			}, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		private Task<U> WriteAsync<U>(Func<SQLiteConnectionWithLock, U> write)
		{
			return Task.Factory.StartNew(delegate
			{
				SQLiteConnectionWithLock sQLiteConnectionWithLock = (SQLiteConnectionWithLock)_innerQuery.Connection;
				using (sQLiteConnectionWithLock.Lock())
				{
					return write(sQLiteConnectionWithLock);
				}
			}, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		public AsyncTableQuery<T> Where(Expression<Func<T, bool>> predExpr)
		{
			return new AsyncTableQuery<T>(_innerQuery.Where(predExpr));
		}

		public AsyncTableQuery<T> Skip(int n)
		{
			return new AsyncTableQuery<T>(_innerQuery.Skip(n));
		}

		public AsyncTableQuery<T> Take(int n)
		{
			return new AsyncTableQuery<T>(_innerQuery.Take(n));
		}

		public AsyncTableQuery<T> OrderBy<U>(Expression<Func<T, U>> orderExpr)
		{
			return new AsyncTableQuery<T>(_innerQuery.OrderBy(orderExpr));
		}

		public AsyncTableQuery<T> OrderByDescending<U>(Expression<Func<T, U>> orderExpr)
		{
			return new AsyncTableQuery<T>(_innerQuery.OrderByDescending(orderExpr));
		}

		public AsyncTableQuery<T> ThenBy<U>(Expression<Func<T, U>> orderExpr)
		{
			return new AsyncTableQuery<T>(_innerQuery.ThenBy(orderExpr));
		}

		public AsyncTableQuery<T> ThenByDescending<U>(Expression<Func<T, U>> orderExpr)
		{
			return new AsyncTableQuery<T>(_innerQuery.ThenByDescending(orderExpr));
		}

		public Task<List<T>> ToListAsync()
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => _innerQuery.ToList());
		}

		public Task<T[]> ToArrayAsync()
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => _innerQuery.ToArray());
		}

		public Task<int> CountAsync()
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => _innerQuery.Count());
		}

		public Task<int> CountAsync(Expression<Func<T, bool>> predExpr)
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => _innerQuery.Count(predExpr));
		}

		public Task<T> ElementAtAsync(int index)
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => _innerQuery.ElementAt(index));
		}

		public Task<T> FirstAsync()
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => _innerQuery.First());
		}

		public Task<T> FirstOrDefaultAsync()
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => _innerQuery.FirstOrDefault());
		}

		public Task<T> FirstAsync(Expression<Func<T, bool>> predExpr)
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => _innerQuery.First(predExpr));
		}

		public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predExpr)
		{
			return ReadAsync((SQLiteConnectionWithLock conn) => _innerQuery.FirstOrDefault(predExpr));
		}

		public Task<int> DeleteAsync(Expression<Func<T, bool>> predExpr)
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => _innerQuery.Delete(predExpr));
		}

		public Task<int> DeleteAsync()
		{
			return WriteAsync((SQLiteConnectionWithLock conn) => _innerQuery.Delete());
		}
	}
}
