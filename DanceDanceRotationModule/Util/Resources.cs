using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using DanceDanceRotationModule.Model;
using DanceDanceRotationModule.Storage;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace DanceDanceRotationModule.Util
{
	public class Resources
	{
		private struct AbilityInfo
		{
			public int assetId { get; }

			public string icon { get; }

			public string name { get; }

			public AbilityInfo(int assetId, string icon, string name)
			{
				this.assetId = assetId;
				this.icon = icon;
				this.name = name;
				if (name == null)
				{
					throw new Exception($"AbilityInfo was read in wrong. Missing name! ${this}");
				}
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<Resources>();

		public static Resources Instance;

		private ContentsManager ContentsManager;

		private IDictionary<AbilityId, AbilityInfo> AbilityInfos = new Dictionary<AbilityId, AbilityInfo>();

		private IDictionary<PaletteId, AbilityId> PaletteIdLookup = new Dictionary<PaletteId, AbilityId>();

		private IDictionary<string, AsyncTexture2D> AbilityIconTextureCache = new Dictionary<string, AsyncTexture2D>();

		public Texture2D WindowBackgroundEmptyTexture { get; private set; }

		public Texture2D WindowBackgroundTexture { get; private set; }

		public Texture2D WindowBackground2Texture { get; private set; }

		public Texture2D DdrLogoTexture { get; private set; }

		public Texture2D DdrLogoEmblemTexture { get; private set; }

		public Texture2D DdrNotePurpleTexture { get; private set; }

		public Texture2D DdrNoteRedTexture { get; private set; }

		public Texture2D DdrNoteGreenTexture { get; private set; }

		public Texture2D DdrTargetCircle { get; private set; }

		public Texture2D DdrTargetSpacer { get; private set; }

		public Texture2D DdrTargetTop { get; private set; }

		public Texture2D DdrTargetBottom { get; private set; }

		public Texture2D DdrTargetLeft { get; private set; }

		public Texture2D DdrTargetRight { get; private set; }

		public Texture2D ButtonCopy { get; private set; }

		public Texture2D ButtonDelete { get; private set; }

		public Texture2D ButtonDownload { get; private set; }

		public Texture2D ButtonList { get; private set; }

		public Texture2D ButtonDetails { get; private set; }

		public Texture2D ButtonOpenUrl { get; private set; }

		public Texture2D ButtonPause { get; private set; }

		public Texture2D ButtonPlay { get; private set; }

		public Texture2D ButtonReload { get; private set; }

		public Texture2D ButtonStop { get; private set; }

		public Texture2D NotesControlsBg { get; private set; }

		public Texture2D NotesBg { get; private set; }

		public Texture2D SongInfoBackground { get; private set; }

		public Texture2D UnknownAbilityIcon { get; private set; }

		public void LoadResources(ContentsManager contentsManager)
		{
			ContentsManager = contentsManager;
			LoadAbilityInfoFile("abilityInfoApi.json");
			LoadAbilityInfoFile("abilityInfoCustom.json");
			LoadPaletteIdLookup("paletteSkillLookup.json");
			ButtonCopy = contentsManager.GetTexture("buttons/copyIcon.png");
			ButtonDelete = contentsManager.GetTexture("buttons/deleteIcon.png");
			ButtonDownload = contentsManager.GetTexture("buttons/downloadIcon.png");
			ButtonList = contentsManager.GetTexture("buttons/menuIcon.png");
			ButtonDetails = contentsManager.GetTexture("buttons/menuSettings.png");
			ButtonOpenUrl = contentsManager.GetTexture("buttons/openLinkIcon.png");
			ButtonPause = contentsManager.GetTexture("buttons/pauseIcon.png");
			ButtonPlay = contentsManager.GetTexture("buttons/playIcon.png");
			ButtonReload = contentsManager.GetTexture("buttons/reloadIcon.png");
			ButtonStop = contentsManager.GetTexture("buttons/stopIcon.png");
			DdrLogoTexture = contentsManager.GetTexture("ddr_logo.png");
			DdrLogoEmblemTexture = contentsManager.GetTexture("ddr_logo_emblem.png");
			DdrNoteGreenTexture = contentsManager.GetTexture("notes/ddr_note_green.png");
			DdrNotePurpleTexture = contentsManager.GetTexture("notes/ddr_note_purple.png");
			DdrNoteRedTexture = contentsManager.GetTexture("notes/ddr_note_red.png");
			DdrTargetCircle = contentsManager.GetTexture("notes/ddr_target_circle.png");
			DdrTargetSpacer = contentsManager.GetTexture("notes/ddr_target_spacer.png");
			DdrTargetTop = contentsManager.GetTexture("notes/ddr_target_top.png");
			DdrTargetBottom = contentsManager.GetTexture("notes/ddr_target_bottom.png");
			DdrTargetLeft = contentsManager.GetTexture("notes/ddr_target_left.png");
			DdrTargetRight = contentsManager.GetTexture("notes/ddr_target_right.png");
			NotesControlsBg = contentsManager.GetTexture("notes/notesControlsBg.png");
			NotesBg = contentsManager.GetTexture("notes/notesBg.png");
			SongInfoBackground = contentsManager.GetTexture("windows/songInfoBg.png");
			UnknownAbilityIcon = contentsManager.GetTexture("abilityIcons/custom/unknownAbilityIcon.png");
			WindowBackgroundEmptyTexture = contentsManager.GetTexture("windows/windowBgEmpty.png");
			WindowBackgroundTexture = contentsManager.GetTexture("windows/windowBg.png");
			WindowBackground2Texture = contentsManager.GetTexture("windows/windowBg2.png");
		}

		private void LoadAbilityInfoFile(string fileName)
		{
			foreach (KeyValuePair<string, AbilityInfo> entry in JsonConvert.DeserializeObject<Dictionary<string, AbilityInfo>>(new StreamReader(ContentsManager.GetFileStream(fileName), Encoding.UTF8).ReadToEnd()))
			{
				try
				{
					int raw = int.Parse(entry.Key);
					AbilityId abilityId = new AbilityId(raw);
					AbilityInfo abilityInfo = entry.Value;
					AbilityInfos[abilityId] = abilityInfo;
				}
				catch
				{
					Logger logger = Logger;
					KeyValuePair<string, AbilityInfo> keyValuePair = entry;
					logger.Warn("Failed to decode an entry in " + fileName + ": " + keyValuePair.ToString());
				}
			}
		}

		private void LoadPaletteIdLookup(string fileName)
		{
			Logger.Info("Loading Palette ID lookup tables from: " + fileName);
			foreach (KeyValuePair<string, int> entry in JsonConvert.DeserializeObject<Dictionary<string, int>>(new StreamReader(ContentsManager.GetFileStream(fileName), Encoding.UTF8).ReadToEnd()))
			{
				try
				{
					int raw = int.Parse(entry.Key);
					PaletteId paletteId = new PaletteId(raw);
					AbilityId abilityId = new AbilityId(entry.Value);
					PaletteIdLookup[paletteId] = abilityId;
				}
				catch
				{
					Logger logger = Logger;
					KeyValuePair<string, int> keyValuePair = entry;
					logger.Warn("Failed to decode an entry in " + fileName + ": " + keyValuePair.ToString());
				}
			}
		}

		public AbilityId GetAbilityIdForPaletteId(PaletteId paletteId)
		{
			if (PaletteIdLookup.ContainsKey(paletteId))
			{
				return PaletteIdLookup[paletteId];
			}
			return AbilityId.Unknown;
		}

		public AsyncTexture2D GetAbilityIcon(PaletteId paletteId)
		{
			AbilityId abilityId = GetAbilityIdForPaletteId(paletteId);
			return GetAbilityIcon(abilityId);
		}

		public AsyncTexture2D GetAbilityIcon(AbilityId abilityId)
		{
			if (AbilityInfos.ContainsKey(abilityId))
			{
				AbilityInfo abilityInfo = AbilityInfos[abilityId];
				string assetCacheKey = (string.IsNullOrEmpty(abilityInfo.icon) ? abilityInfo.assetId.ToString() : abilityInfo.icon);
				if (AbilityIconTextureCache.ContainsKey(assetCacheKey))
				{
					return AbilityIconTextureCache[assetCacheKey];
				}
				if (!string.IsNullOrEmpty(abilityInfo.icon))
				{
					string imageFileName = "abilityIcons/" + abilityInfo.icon;
					Texture2D texture2 = ContentsManager.GetTexture(imageFileName);
					if (texture2 == null)
					{
						Logger.Warn("Failed to load icon name from lookup table: " + imageFileName);
						return AsyncTexture2D.op_Implicit(UnknownAbilityIcon);
					}
					AbilityIconTextureCache[assetCacheKey] = AsyncTexture2D.op_Implicit(texture2);
					return AsyncTexture2D.op_Implicit(texture2);
				}
				AsyncTexture2D texture = default(AsyncTexture2D);
				if (GameService.Content.get_DatAssetCache().TryGetTextureFromAssetId(abilityInfo.assetId, ref texture))
				{
					AbilityIconTextureCache[assetCacheKey] = texture;
					return texture;
				}
				Logger.Warn($"Failed to load icon name from lookup table: abilityId=${abilityId.Raw} assertId=${assetCacheKey}");
				return AsyncTexture2D.op_Implicit(UnknownAbilityIcon);
			}
			Logger.Debug("Unknown Ability ID: " + abilityId.Raw);
			return AsyncTexture2D.op_Implicit(UnknownAbilityIcon);
		}

		public string GetAbilityName(AbilityId abilityId)
		{
			if (AbilityInfos.ContainsKey(abilityId))
			{
				return AbilityInfos[abilityId].name;
			}
			return "";
		}

		public void SetUpCurrentSongListener(SongRepo songRepo)
		{
			Song.ID lastSelectedSongId = default(Song.ID);
			songRepo.OnSelectedSongChanged += delegate(object sender, SelectedSongInfo info)
			{
				if (info.Song != null && !info.Song.Id.Equals(lastSelectedSongId))
				{
					lastSelectedSongId = info.Song.Id;
					Logger.Info("Selected Song Changed. Updating ability icon cache to just this song.");
					PurgeAbilityIconCache();
					if (info.Song != null)
					{
						PreloadSongAbilityIcons(info.Song);
					}
				}
			};
		}

		public void PurgeAbilityIconCache()
		{
			AbilityIconTextureCache.Clear();
		}

		public void PreloadSongAbilityIcons(Song song)
		{
			HashSet<AbilityId> uniqueAbilityIds = new HashSet<AbilityId>();
			foreach (Note note in song.Notes)
			{
				uniqueAbilityIds.Add(note.AbilityId);
			}
			foreach (AbilityId abilityId in uniqueAbilityIds)
			{
				GetAbilityIcon(abilityId);
			}
		}

		public void Unload()
		{
			foreach (AsyncTexture2D value in AbilityIconTextureCache.Values)
			{
				((GraphicsResource)AsyncTexture2D.op_Implicit(value)).Dispose();
			}
			AbilityIconTextureCache.Clear();
			AbilityInfos.Clear();
			Texture2D windowBackgroundEmptyTexture = WindowBackgroundEmptyTexture;
			if (windowBackgroundEmptyTexture != null)
			{
				((GraphicsResource)windowBackgroundEmptyTexture).Dispose();
			}
			Texture2D windowBackgroundTexture = WindowBackgroundTexture;
			if (windowBackgroundTexture != null)
			{
				((GraphicsResource)windowBackgroundTexture).Dispose();
			}
			Texture2D windowBackground2Texture = WindowBackground2Texture;
			if (windowBackground2Texture != null)
			{
				((GraphicsResource)windowBackground2Texture).Dispose();
			}
			Texture2D buttonCopy = ButtonCopy;
			if (buttonCopy != null)
			{
				((GraphicsResource)buttonCopy).Dispose();
			}
			Texture2D buttonDelete = ButtonDelete;
			if (buttonDelete != null)
			{
				((GraphicsResource)buttonDelete).Dispose();
			}
			Texture2D buttonDownload = ButtonDownload;
			if (buttonDownload != null)
			{
				((GraphicsResource)buttonDownload).Dispose();
			}
			Texture2D buttonList = ButtonList;
			if (buttonList != null)
			{
				((GraphicsResource)buttonList).Dispose();
			}
			Texture2D buttonDetails = ButtonDetails;
			if (buttonDetails != null)
			{
				((GraphicsResource)buttonDetails).Dispose();
			}
			Texture2D buttonOpenUrl = ButtonOpenUrl;
			if (buttonOpenUrl != null)
			{
				((GraphicsResource)buttonOpenUrl).Dispose();
			}
			Texture2D buttonPause = ButtonPause;
			if (buttonPause != null)
			{
				((GraphicsResource)buttonPause).Dispose();
			}
			Texture2D buttonPlay = ButtonPlay;
			if (buttonPlay != null)
			{
				((GraphicsResource)buttonPlay).Dispose();
			}
			Texture2D buttonReload = ButtonReload;
			if (buttonReload != null)
			{
				((GraphicsResource)buttonReload).Dispose();
			}
			Texture2D buttonStop = ButtonStop;
			if (buttonStop != null)
			{
				((GraphicsResource)buttonStop).Dispose();
			}
			Texture2D ddrLogoTexture = DdrLogoTexture;
			if (ddrLogoTexture != null)
			{
				((GraphicsResource)ddrLogoTexture).Dispose();
			}
			Texture2D ddrLogoEmblemTexture = DdrLogoEmblemTexture;
			if (ddrLogoEmblemTexture != null)
			{
				((GraphicsResource)ddrLogoEmblemTexture).Dispose();
			}
			Texture2D ddrNoteGreenTexture = DdrNoteGreenTexture;
			if (ddrNoteGreenTexture != null)
			{
				((GraphicsResource)ddrNoteGreenTexture).Dispose();
			}
			Texture2D ddrNotePurpleTexture = DdrNotePurpleTexture;
			if (ddrNotePurpleTexture != null)
			{
				((GraphicsResource)ddrNotePurpleTexture).Dispose();
			}
			Texture2D ddrNoteRedTexture = DdrNoteRedTexture;
			if (ddrNoteRedTexture != null)
			{
				((GraphicsResource)ddrNoteRedTexture).Dispose();
			}
			Texture2D ddrTargetCircle = DdrTargetCircle;
			if (ddrTargetCircle != null)
			{
				((GraphicsResource)ddrTargetCircle).Dispose();
			}
			Texture2D ddrTargetSpacer = DdrTargetSpacer;
			if (ddrTargetSpacer != null)
			{
				((GraphicsResource)ddrTargetSpacer).Dispose();
			}
			Texture2D ddrTargetTop = DdrTargetTop;
			if (ddrTargetTop != null)
			{
				((GraphicsResource)ddrTargetTop).Dispose();
			}
			Texture2D ddrTargetBottom = DdrTargetBottom;
			if (ddrTargetBottom != null)
			{
				((GraphicsResource)ddrTargetBottom).Dispose();
			}
			Texture2D ddrTargetLeft = DdrTargetLeft;
			if (ddrTargetLeft != null)
			{
				((GraphicsResource)ddrTargetLeft).Dispose();
			}
			Texture2D ddrTargetRight = DdrTargetRight;
			if (ddrTargetRight != null)
			{
				((GraphicsResource)ddrTargetRight).Dispose();
			}
			Texture2D notesControlsBg = NotesControlsBg;
			if (notesControlsBg != null)
			{
				((GraphicsResource)notesControlsBg).Dispose();
			}
			Texture2D notesBg = NotesBg;
			if (notesBg != null)
			{
				((GraphicsResource)notesBg).Dispose();
			}
			Texture2D songInfoBackground = SongInfoBackground;
			if (songInfoBackground != null)
			{
				((GraphicsResource)songInfoBackground).Dispose();
			}
			Texture2D unknownAbilityIcon = UnknownAbilityIcon;
			if (unknownAbilityIcon != null)
			{
				((GraphicsResource)unknownAbilityIcon).Dispose();
			}
			Logger.Info("Resourced Unloaded");
		}
	}
}
