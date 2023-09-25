using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Newtonsoft.Json;

namespace felix.BlishEmotes
{
	internal class EmotesManager
	{
		private static readonly Logger Logger = Logger.GetLogger<EmotesManager>();

		private ContentsManager ContentsManager;

		private ModuleSettings Settings;

		private Dictionary<string, Emote> emotes;

		public EmotesManager(ContentsManager contentsManager, ModuleSettings settings)
		{
			ContentsManager = contentsManager;
			Settings = settings;
			emotes = new Dictionary<string, Emote>();
		}

		public void Load()
		{
			try
			{
				string fileContents;
				using (StreamReader reader = new StreamReader(ContentsManager.GetFileStream("json/emotes.json")))
				{
					fileContents = reader.ReadToEnd();
				}
				List<Emote> loadedEmotes = JsonConvert.DeserializeObject<List<Emote>>(fileContents);
				foreach (Emote emote2 in loadedEmotes)
				{
					emote2.Texture = ContentsManager.GetTexture("textures/emotes/" + emote2.TextureRef, ContentsManager.GetTexture("textures/missing-texture.png"));
				}
				foreach (Emote emote in loadedEmotes)
				{
					emotes.Add(emote.Id, emote);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Failed to load emotes.");
				Logger.Error(ex.Message);
				Logger.Debug(ex.StackTrace);
			}
		}

		public void UpdateAll(List<Emote> newEmotes)
		{
			emotes.Clear();
			foreach (Emote emote in newEmotes)
			{
				emotes.Add(emote.Id, emote);
			}
		}

		public List<Emote> GetAll()
		{
			return new List<Emote>(emotes.Values);
		}

		public List<Emote> GetRadial()
		{
			return emotes.Values.Where((Emote el) => !Settings.EmotesRadialEnabledMap.ContainsKey(el) || Settings.EmotesRadialEnabledMap[el].Value).ToList();
		}
	}
}
