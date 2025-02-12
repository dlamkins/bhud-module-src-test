using System.Runtime.CompilerServices;
using GuildWars2;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SL.ChatLinks.Storage
{
	public class SqliteDbContextFactory : IDbContextFactory
	{
		[CompilerGenerated]
		private IOptions<DatabaseOptions> _003Coptions_003EP;

		public SqliteDbContextFactory(IOptions<DatabaseOptions> options)
		{
			_003Coptions_003EP = options;
			base._002Ector();
		}

		public ChatLinksContext CreateDbContext(Language language)
		{
			DbContextOptionsBuilder<ChatLinksContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<ChatLinksContext>();
			SqliteConnection connection = new SqliteConnection(_003Coptions_003EP.Value.ConnectionString(language));
			dbContextOptionsBuilder.UseSqlite(connection);
			Levenshtein.RegisterLevenshteinFunction(connection);
			return new ChatLinksContext(dbContextOptionsBuilder.Options);
		}

		public ChatLinksContext CreateDbContext(string file)
		{
			DbContextOptionsBuilder<ChatLinksContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<ChatLinksContext>();
			SqliteConnection connection = new SqliteConnection(_003Coptions_003EP.Value.ConnectionString(file));
			dbContextOptionsBuilder.UseSqlite(connection);
			Levenshtein.RegisterLevenshteinFunction(connection);
			return new ChatLinksContext(dbContextOptionsBuilder.Options);
		}
	}
}
