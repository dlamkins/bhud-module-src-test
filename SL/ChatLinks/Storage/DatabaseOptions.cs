using System.IO;
using GuildWars2;

namespace SL.ChatLinks.Storage
{
	public class DatabaseOptions
	{
		public string Directory { get; set; } = System.IO.Directory.GetCurrentDirectory();


		public string DatabaseFileName(Language language)
		{
			if (language != null)
			{
				switch (language.Alpha2Code)
				{
				case "de":
					return "data_de.db";
				case "es":
					return "data_es.db";
				case "fr":
					return "data_fr.db";
				}
			}
			return "data.db";
		}

		public string ConnectionString(Language language)
		{
			return ConnectionString(DatabaseFileName(language));
		}

		public string ConnectionString(string file)
		{
			return "Data Source=" + Path.Combine(Directory, file);
		}
	}
}
