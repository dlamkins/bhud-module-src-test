using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Newtonsoft.Json;
using felix.BlishEmotes.Strings;

namespace felix.BlishEmotes
{
	internal class EmotesManager
	{
		private static readonly Logger Logger = Logger.GetLogger<EmotesManager>();

		private ContentsManager ContentsManager;

		private ModuleSettings Settings;

		private ResourceManager EmotesResourceManager;

		private Dictionary<string, Emote> emotes;

		public bool IsEmoteSynchronized { get; set; }

		public bool IsEmoteTargeted { get; set; }

		public EmotesManager(ContentsManager contentsManager, ModuleSettings settings)
		{
			ContentsManager = contentsManager;
			Settings = settings;
			EmotesResourceManager = new ResourceManager("felix.BlishEmotes.Strings.Emotes", typeof(Common).Assembly);
			emotes = new Dictionary<string, Emote>();
			GameService.Overlay.UserLocaleChanged += OnLocaleChanged;
		}

		private void OnLocaleChanged(object sender, ValueEventArgs<CultureInfo> eventArgs)
		{
			UpdateEmoteLabels();
			Logger.Debug("Updated emote labels");
		}

		private void UpdateEmoteLabels()
		{
			foreach (KeyValuePair<string, Emote> emote in emotes)
			{
				emote.Value.UpdateLabel(EmotesResourceManager);
			}
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
					emote2.UpdateLabel(EmotesResourceManager);
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

		public void Unload()
		{
			GameService.Overlay.UserLocaleChanged -= OnLocaleChanged;
			foreach (Emote value in emotes.Values)
			{
				value.Texture?.Dispose();
			}
		}

		public void SendEmoteCommand(Emote emote)
		{
			if (emote.Locked)
			{
				Logger.Debug("SendEmoteCommand: Emote locked.");
			}
			else if (GameService.GameIntegration.Gw2Instance.IsInGame && !GameService.Gw2Mumble.UI.IsMapOpen)
			{
				string command = emote.Command;
				if (IsEmoteSynchronized)
				{
					command += " *";
				}
				if (IsEmoteTargeted)
				{
					command += " @";
				}
				Logger.Debug(command);
				GameService.GameIntegration.Chat.Send(command);
			}
		}
	}
}
