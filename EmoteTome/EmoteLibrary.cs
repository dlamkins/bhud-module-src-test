using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Blish_HUD.Modules.Managers;
using Newtonsoft.Json;

namespace EmoteTome
{
	internal class EmoteLibrary
	{
		private ContentsManager contentsManager;

		private static readonly string CORELIST = "Emotes/Core/Json/emotelist.txt";

		private static readonly string COREJSON = "Emotes/Core/Json/";

		private static readonly string UNLOCKLIST = "Emotes/Unlock/Json/emotelist.txt";

		private static readonly string UNLOCKJSON = "Emotes/Unlock/Json/";

		private static readonly string RANKLIST = "Emotes/Rank/Json/emotelist.txt";

		private static readonly string RANKJSON = "Emotes/Rank/Json/";

		public static readonly string CORECODE = "core";

		public static readonly string UNLOCKCODE = "unlock";

		public static readonly string RANKCODE = "rank";

		public EmoteLibrary(ContentsManager manager)
		{
			contentsManager = manager;
		}

		private List<Emote> loadEmoteFiles(string listpath, string jsonPath)
		{
			List<Emote> emoteList = new List<Emote>();
			try
			{
				Stream fileStream = contentsManager.GetFileStream(listpath);
				int filesize = (int)fileStream.Length;
				byte[] buffer = new byte[filesize];
				fileStream.Position = 0L;
				fileStream.Read(buffer, 0, filesize);
				fileStream.Close();
				string[] array = Encoding.UTF8.GetString(buffer).Split(new string[1] { Environment.NewLine }, StringSplitOptions.None);
				foreach (string name in array)
				{
					try
					{
						Stream fileStream2 = contentsManager.GetFileStream(jsonPath + name + ".json");
						filesize = (int)fileStream2.Length;
						buffer = new byte[filesize];
						fileStream2.Position = 0L;
						fileStream2.Read(buffer, 0, filesize);
						fileStream2.Close();
						emoteList.Add(JsonConvert.DeserializeObject<Emote>(Encoding.UTF8.GetString(buffer)));
					}
					catch (Exception)
					{
					}
				}
				return emoteList;
			}
			catch (Exception)
			{
				return emoteList;
			}
		}

		public List<Emote> loadCoreEmotes()
		{
			return loadEmoteFiles(CORELIST, COREJSON);
		}

		public List<Emote> loadUnlockEmotes()
		{
			return loadEmoteFiles(UNLOCKLIST, UNLOCKJSON);
		}

		public List<Emote> loadRankEmotes()
		{
			return loadEmoteFiles(RANKLIST, RANKJSON);
		}
	}
}
