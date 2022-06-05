using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Gw2Mumble;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager
{
	[Export(typeof(Module))]
	public class BuildsManager : Module
	{
		internal static BuildsManager ModuleInstance;

		public static readonly Logger Logger = Logger.GetLogger<BuildsManager>();

		public const uint WM_COMMAND = 273u;

		public const uint WM_PASTE = 770u;

		public TextureManager TextureManager;

		public iPaths Paths;

		public iData Data;

		public iTicks Ticks = new iTicks();

		public List<int> ArmoryItems = new List<int>();

		public SettingEntry<bool> PasteOnCopy;

		public SettingEntry<bool> ShowCornerIcon;

		public SettingEntry<bool> IncludeDefaultBuilds;

		public SettingEntry<bool> ShowCurrentProfession;

		public SettingEntry<KeyBinding> ReloadKey;

		public SettingEntry<KeyBinding> ToggleWindow;

		public SettingEntry<int> GameVersion;

		public SettingEntry<string> ModuleVersion;

		public string CultureString;

		public List<Template> Templates = new List<Template>();

		public List<Template> DefaultTemplates = new List<Template>();

		private Template _Selected_Template;

		public Window_MainWindow MainWindow;

		public Texture2D LoadingTexture;

		public LoadingSpinner loadingSpinner;

		public ProgressBar downloadBar;

		private CornerIcon cornerIcon;

		private bool _DataLoaded;

		public bool FetchingAPI;

		public API.Profession CurrentProfession;

		public API.Specialization CurrentSpecialization;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public Template Selected_Template
		{
			get
			{
				return _Selected_Template;
			}
			set
			{
				if (_Selected_Template != null)
				{
					_Selected_Template.Edit -= OnSelected_Template_Edit;
				}
				_Selected_Template = value;
				if (value != null)
				{
					OnSelected_Template_Changed();
					_Selected_Template.Edit += OnSelected_Template_Edit;
				}
			}
		}

		public bool DataLoaded
		{
			get
			{
				return _DataLoaded;
			}
			set
			{
				_DataLoaded = value;
				if (value)
				{
					ModuleInstance.OnDataLoaded();
				}
			}
		}

		public event EventHandler Selected_Template_Redraw;

		public event EventHandler LanguageChanged;

		public event EventHandler Selected_Template_Edit;

		public event EventHandler Selected_Template_Changed;

		public event EventHandler Template_Deleted;

		public event EventHandler Templates_Loaded;

		public event EventHandler DataLoaded_Event;

		[ImportingConstructor]
		public BuildsManager([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		[DllImport("user32")]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32")]
		public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		public void OnSelected_Template_Redraw(object sender, EventArgs e)
		{
			this.Selected_Template_Redraw?.Invoke(this, EventArgs.Empty);
		}

		public void OnLanguageChanged(object sender, EventArgs e)
		{
			this.LanguageChanged?.Invoke(this, EventArgs.Empty);
		}

		public void OnSelected_Template_Edit(object sender, EventArgs e)
		{
			MainWindow._TemplateSelection.RefreshList();
			this.Selected_Template_Edit?.Invoke(this, EventArgs.Empty);
		}

		public void OnSelected_Template_Changed()
		{
			this.Selected_Template_Changed?.Invoke(this, EventArgs.Empty);
		}

		public void OnTemplate_Deleted()
		{
			Selected_Template = new Template();
			this.Template_Deleted?.Invoke(this, EventArgs.Empty);
		}

		public void OnTemplates_Loaded()
		{
			LoadTemplates();
			this.Templates_Loaded?.Invoke(this, EventArgs.Empty);
		}

		private void OnDataLoaded()
		{
			this.DataLoaded_Event?.Invoke(this, EventArgs.Empty);
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Expected O, but got Unknown
			ToggleWindow = settings.DefineSetting<KeyBinding>("ToggleWindow", new KeyBinding((ModifierKeys)1, (Keys)66), (Func<string>)(() => "Toggle Window"), (Func<string>)(() => "Show / Hide the UI"));
			PasteOnCopy = settings.DefineSetting<bool>("PasteOnCopy", false, (Func<string>)(() => "Paste Stat/Upgrade Name"), (Func<string>)(() => "Paste Stat/Upgrade Name after copying it."));
			ShowCornerIcon = settings.DefineSetting<bool>("ShowCornerIcon", true, (Func<string>)(() => "Show Corner Icon"), (Func<string>)(() => "Show / Hide the Corner Icon of this module."));
			IncludeDefaultBuilds = settings.DefineSetting<bool>("IncludeDefaultBuilds", true, (Func<string>)(() => "Incl. Default Builds"), (Func<string>)(() => "Load the default builds from within the module."));
			ShowCurrentProfession = settings.DefineSetting<bool>("ShowCurrentProfession", true, (Func<string>)(() => "Filter Current Profession"), (Func<string>)(() => "Always set the current Profession as an active filter."));
			SettingCollection internal_settings = settings.AddSubCollection("Internal Settings", false);
			GameVersion = internal_settings.DefineSetting<int>("GameVersion", 0, (Func<string>)null, (Func<string>)null);
			ModuleVersion = internal_settings.DefineSetting<string>("ModuleVersion", "0.0.0", (Func<string>)null, (Func<string>)null);
			ReloadKey = internal_settings.DefineSetting<KeyBinding>("ReloadKey", new KeyBinding((Keys)0), (Func<string>)(() => "Reload Button"), (Func<string>)(() => ""));
		}

		protected override void Initialize()
		{
			CultureString = getCultureString();
			Logger.Info("Starting Builds Manager v." + (object)((Module)this).get_Version().BaseVersion());
			Paths = new iPaths(DirectoriesManager.GetFullDirectoryPath("builds-manager"));
			ArmoryItems.AddRange(new int[45]
			{
				80248, 80131, 80190, 80111, 80356, 80399, 80296, 80145, 80578, 80161,
				80252, 80281, 80384, 80435, 80254, 80205, 80277, 80557, 91505, 91536,
				81908, 91048, 91234, 93105, 95380, 74155, 30698, 30699, 30686, 30696,
				30697, 30695, 30684, 30702, 30687, 30690, 30685, 30701, 30691, 30688,
				30692, 30694, 30693, 30700, 30689
			});
			ReloadKey.get_Value().set_Enabled(true);
			ReloadKey.get_Value().add_Activated((EventHandler<EventArgs>)ReloadKey_Activated);
			ToggleWindow.get_Value().set_Enabled(true);
			ToggleWindow.get_Value().add_Activated((EventHandler<EventArgs>)ToggleWindow_Activated);
			IncludeDefaultBuilds.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)IncludeDefaultBuilds_SettingChanged);
			DataLoaded = false;
			GameService.Gw2Mumble.get_PlayerCharacter().add_SpecializationChanged((EventHandler<ValueEventArgs<int>>)PlayerCharacter_SpecializationChanged);
		}

		private void IncludeDefaultBuilds_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (!e.get_NewValue())
			{
				foreach (Template t2 in DefaultTemplates)
				{
					Templates.Remove(t2);
				}
			}
			else
			{
				foreach (Template t in DefaultTemplates)
				{
					Templates.Add(t);
				}
			}
			MainWindow._TemplateSelection.Refresh();
		}

		private void PlayerCharacter_SpecializationChanged(object sender, ValueEventArgs<int> eventArgs)
		{
			PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
			if (player != null && (int)player.get_Profession() > 0 && Data != null && MainWindow != null)
			{
				CurrentProfession = Data.Professions.Find((API.Profession e) => e.Id == player.get_Profession().ToString());
				CurrentSpecialization = CurrentProfession?.Specializations.Find((API.Specialization e) => e.Id == player.get_Specialization());
				MainWindow.PlayerCharacter_NameChanged(null, null);
				Templates = (from a in Templates
					orderby a.Build?.Profession != ModuleInstance.CurrentProfession, a.Profession.Id
					select a).ThenBy((Template b) => b.Specialization?.Id).ThenBy((Template b) => b.Name).ToList();
				MainWindow._TemplateSelection.RefreshList();
			}
		}

		private void ToggleWindow_Activated(object sender, EventArgs e)
		{
			Window_MainWindow mainWindow = MainWindow;
			if (mainWindow != null)
			{
				((WindowBase2)mainWindow).ToggleWindow();
			}
		}

		private void ReloadKey_Activated(object sender, EventArgs e)
		{
			ScreenNotification.ShowNotification("Rebuilding the UI", (NotificationType)1, (Texture2D)null, 4);
			Window_MainWindow mainWindow = MainWindow;
			if (mainWindow != null)
			{
				((Control)mainWindow).Dispose();
			}
			CreateUI();
			((WindowBase2)MainWindow).ToggleWindow();
		}

		protected override async Task LoadAsync()
		{
		}

		public void ImportTemplates()
		{
			Template saveTemplate = null;
			if (System.IO.File.Exists(Paths.builds + "config.ini"))
			{
				string text = System.IO.File.ReadAllText(Paths.builds + "config.ini");
				bool started = false;
				string[] array = text.Split('\n');
				foreach (string s in array)
				{
					if (!(s.Trim() != ""))
					{
						continue;
					}
					if (started && s.StartsWith("["))
					{
						break;
					}
					if (started)
					{
						string[] buildpadBuild = s.Trim().Split('|');
						Template template2 = new Template();
						template2.Template_json = new Template_json
						{
							Name = buildpadBuild[5],
							BuildCode = buildpadBuild[1]
						};
						if (Templates.Find((Template e) => e.Template_json.Name == template2.Template_json.Name && e.Template_json.BuildCode == template2.Template_json.BuildCode) == null)
						{
							template2.Build = new BuildTemplate(buildpadBuild[1]);
							template2.Name = buildpadBuild[5];
							template2.Profession = template2.Build.Profession;
							template2.Specialization = template2.Build.SpecLines.Find((SpecLine e) => e.Specialization?.Elite ?? false)?.Specialization;
							Logger.Debug("Adding new template: '{0}'", new object[1] { template2.Template_json.Name });
							Templates.Add(template2);
							saveTemplate = template2;
						}
					}
					if (!started && s.StartsWith("[Builds]"))
					{
						started = true;
					}
				}
				System.IO.File.Delete(Paths.builds + "config.ini");
			}
			List<string> files = Directory.GetFiles(Paths.builds, "*.json", SearchOption.TopDirectoryOnly).ToList();
			if (files.Contains(Paths.builds + "Builds.json"))
			{
				files.Remove(Paths.builds + "Builds.json");
			}
			foreach (string item in files)
			{
				Template template = new Template(item);
				System.IO.File.Delete(item);
				Templates.Add(template);
				saveTemplate = template;
				if (item == files[files.Count - 1])
				{
					template.Save();
				}
			}
			Logger.Debug("Saving {0} Templates.", new object[1] { Templates.Count });
			saveTemplate?.Save();
		}

		public void LoadTemplates()
		{
			string currentTemplate = _Selected_Template?.Name;
			ImportTemplates();
			Templates = new List<Template>();
			foreach (string path in new List<string> { Paths.builds + "Builds.json" })
			{
				string content = "";
				try
				{
					if (System.IO.File.Exists(path))
					{
						content = System.IO.File.ReadAllText(path);
					}
					if (content == null || !(content != ""))
					{
						continue;
					}
					foreach (Template_json jsonTemplate2 in JsonConvert.DeserializeObject<List<Template_json>>(content))
					{
						Template template2 = new Template(jsonTemplate2.Name, jsonTemplate2.BuildCode, jsonTemplate2.GearCode);
						template2.Path = path;
						Templates.Add(template2);
						if (template2.Name == currentTemplate)
						{
							_Selected_Template = template2;
						}
					}
				}
				catch
				{
				}
			}
			List<Template_json> defaultTemps = JsonConvert.DeserializeObject<List<Template_json>>(new StreamReader(ContentsManager.GetFileStream("data\\builds.json")).ReadToEnd());
			if (defaultTemps != null)
			{
				foreach (Template_json jsonTemplate in defaultTemps)
				{
					Template template = new Template(jsonTemplate.Name, jsonTemplate.BuildCode, jsonTemplate.GearCode);
					template.Path = null;
					DefaultTemplates.Add(template);
					if (IncludeDefaultBuilds.get_Value())
					{
						Templates.Add(template);
					}
					if (template.Name == currentTemplate)
					{
						_Selected_Template = template;
					}
				}
			}
			_Selected_Template?.SetChanged();
			OnSelected_Template_Changed();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Expected O, but got Unknown
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			TextureManager = new TextureManager();
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(TextureManager.getIcon(_Icons.Template)));
			((Control)val).set_BasicTooltipText(((Module)this).get_Name() ?? "");
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Visible(ShowCornerIcon.get_Value());
			cornerIcon = val;
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Location(new Point(((Control)cornerIcon).get_Location().X - ((Control)cornerIcon).get_Width(), ((Control)cornerIcon).get_Location().Y + ((Control)cornerIcon).get_Height() + 5));
			((Control)val2).set_Size(((Control)cornerIcon).get_Size());
			((Control)val2).set_Visible(false);
			((Control)val2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			loadingSpinner = val2;
			ProgressBar progressBar = new ProgressBar();
			((Control)progressBar).set_Location(new Point(((Control)cornerIcon).get_Location().X, ((Control)cornerIcon).get_Location().Y + ((Control)cornerIcon).get_Height() + 5 + 3));
			((Control)progressBar).set_Size(new Point(150, ((Control)cornerIcon).get_Height() - 6));
			((Control)progressBar).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			progressBar.Progress = 0.66;
			((Control)progressBar).set_Visible(false);
			downloadBar = progressBar;
			((Control)cornerIcon).add_MouseEntered((EventHandler<MouseEventArgs>)CornerIcon_MouseEntered);
			((Control)cornerIcon).add_MouseLeft((EventHandler<MouseEventArgs>)CornerIcon_MouseLeft);
			((Control)cornerIcon).add_Click((EventHandler<MouseEventArgs>)CornerIcon_Click);
			((Control)cornerIcon).add_Moved((EventHandler<MovedEventArgs>)CornerIcon_Moved);
			ShowCornerIcon.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowCornerIcon_SettingChanged);
			DataLoaded_Event += BuildsManager_DataLoaded_Event;
			((Module)this).OnModuleLoaded(e);
			LoadData();
		}

		private void CornerIcon_Moved(object sender, MovedEventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			((Control)loadingSpinner).set_Location(new Point(((Control)cornerIcon).get_Location().X - ((Control)cornerIcon).get_Width(), ((Control)cornerIcon).get_Location().Y + ((Control)cornerIcon).get_Height() + 5));
			((Control)downloadBar).set_Location(new Point(((Control)cornerIcon).get_Location().X, ((Control)cornerIcon).get_Location().Y + ((Control)cornerIcon).get_Height() + 5 + 3));
		}

		private void BuildsManager_DataLoaded_Event(object sender, EventArgs e)
		{
			CreateUI();
		}

		private void CornerIcon_Click(object sender, MouseEventArgs e)
		{
			if (MainWindow != null)
			{
				((WindowBase2)MainWindow).ToggleWindow();
			}
		}

		private void CornerIcon_MouseLeft(object sender, MouseEventArgs e)
		{
			cornerIcon.set_Icon(AsyncTexture2D.op_Implicit(TextureManager.getIcon(_Icons.Template)));
		}

		private void CornerIcon_MouseEntered(object sender, MouseEventArgs e)
		{
			cornerIcon.set_Icon(AsyncTexture2D.op_Implicit(TextureManager.getIcon(_Icons.Template_White)));
		}

		private void ShowCornerIcon_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (cornerIcon != null)
			{
				((Control)cornerIcon).set_Visible(e.get_NewValue());
			}
		}

		protected override void Update(GameTime gameTime)
		{
			Ticks.global += gameTime.get_ElapsedGameTime().TotalMilliseconds;
			if (Ticks.global > 1250.0)
			{
				Ticks.global -= 1250.0;
				if (CurrentProfession == null)
				{
					PlayerCharacter_SpecializationChanged(null, null);
				}
				Window_MainWindow mainWindow = MainWindow;
				if (mainWindow != null && ((Control)mainWindow).get_Visible())
				{
					((Control)MainWindow.Import_Button).set_Visible(System.IO.File.Exists(Paths.builds + "config.ini"));
				}
			}
		}

		protected override void Unload()
		{
			Window_MainWindow mainWindow = MainWindow;
			if (mainWindow != null)
			{
				((Control)mainWindow).Dispose();
			}
			Templates?.DisposeAll();
			Templates?.Clear();
			DefaultTemplates?.DisposeAll();
			DefaultTemplates?.Clear();
			TextureManager?.Dispose();
			TextureManager = null;
			_Selected_Template.Edit -= OnSelected_Template_Edit;
			_Selected_Template.Edit -= null;
			Selected_Template = null;
			CurrentProfession = null;
			CurrentSpecialization = null;
			Data?.Dispose();
			Data = null;
			TextureManager = null;
			CornerIcon obj = cornerIcon;
			if (obj != null)
			{
				((Control)obj).Dispose();
			}
			ToggleWindow.get_Value().set_Enabled(false);
			ToggleWindow.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleWindow_Activated);
			ReloadKey.get_Value().set_Enabled(false);
			ReloadKey.get_Value().remove_Activated((EventHandler<EventArgs>)ReloadKey_Activated);
			((Control)cornerIcon).remove_MouseEntered((EventHandler<MouseEventArgs>)CornerIcon_MouseEntered);
			((Control)cornerIcon).remove_MouseLeft((EventHandler<MouseEventArgs>)CornerIcon_MouseLeft);
			((Control)cornerIcon).remove_Click((EventHandler<MouseEventArgs>)CornerIcon_Click);
			((Control)cornerIcon).remove_Moved((EventHandler<MovedEventArgs>)CornerIcon_Moved);
			DataLoaded_Event -= BuildsManager_DataLoaded_Event;
			ShowCornerIcon.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowCornerIcon_SettingChanged);
			IncludeDefaultBuilds.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)IncludeDefaultBuilds_SettingChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_SpecializationChanged((EventHandler<ValueEventArgs<int>>)PlayerCharacter_SpecializationChanged);
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
			ProgressBar progressBar = downloadBar;
			if (progressBar != null)
			{
				((Control)progressBar).Dispose();
			}
			downloadBar = null;
			DataLoaded = false;
			ModuleInstance = null;
		}

		public static string getCultureString()
		{
			string culture = "en-EN";
			return GameService.Overlay.get_UserLocale().get_Value() switch
			{
				Locale.French => "fr-FR", 
				Locale.Spanish => "es-ES", 
				Locale.German => "de-DE", 
				_ => "en-EN", 
			};
		}

		public async Task Fetch_APIData(bool force = false)
		{
			if (GameVersion.get_Value() == GameService.Gw2Mumble.get_Info().get_Version() && !(ModuleVersion.get_Value() != ((object)((Module)this).get_Version().BaseVersion()).ToString()) && !force)
			{
				return;
			}
			FetchingAPI = true;
			List<APIDownload_Image> downloadList = new List<APIDownload_Image>();
			string culture = getCultureString();
			double total2 = 0.0;
			double completed2 = 0.0;
			double progress = 0.0;
			List<int> _runes = JsonConvert.DeserializeObject<List<int>>(new StreamReader(ContentsManager.GetFileStream("data\\runes.json")).ReadToEnd());
			List<int> _sigils = JsonConvert.DeserializeObject<List<int>>(new StreamReader(ContentsManager.GetFileStream("data\\sigils.json")).ReadToEnd());
			JsonSerializerSettings val = new JsonSerializerSettings();
			val.set_NullValueHandling((NullValueHandling)1);
			val.set_Formatting((Formatting)1);
			JsonSerializerSettings settings = val;
			int totalFetches = 9;
			Logger.Debug("Fetching all required Data from the API!");
			((Control)loadingSpinner).set_Visible(true);
			((Control)downloadBar).set_Visible(true);
			downloadBar.Progress = progress;
			downloadBar.Text = $"{completed2} / {totalFetches}";
			IReadOnlyList<Item> sigils = await Gw2ApiManager.get_Gw2ApiClient().V2.Items.ManyAsync(_sigils);
			completed2 += 1.0;
			downloadBar.Progress = completed2 / (double)totalFetches;
			downloadBar.Text = $"{completed2} / {totalFetches}";
			Logger.Debug(string.Format("Fetched {0}", "Sigils"));
			IReadOnlyList<Item> runes = await Gw2ApiManager.get_Gw2ApiClient().V2.Items.ManyAsync(_runes);
			completed2 += 1.0;
			downloadBar.Progress = completed2 / (double)totalFetches;
			downloadBar.Text = $"{completed2} / {totalFetches}";
			Logger.Debug(string.Format("Fetched {0}", "Runes"));
			IReadOnlyList<Item> armory_items = await Gw2ApiManager.get_Gw2ApiClient().V2.Items.ManyAsync(ArmoryItems);
			completed2 += 1.0;
			downloadBar.Progress = completed2 / (double)totalFetches;
			downloadBar.Text = $"{completed2} / {totalFetches}";
			Logger.Debug(string.Format("Fetched {0}", "Armory"));
			IApiV2ObjectList<Profession> professions = await Gw2ApiManager.get_Gw2ApiClient().V2.Professions.AllAsync();
			completed2 += 1.0;
			downloadBar.Progress = completed2 / (double)totalFetches;
			downloadBar.Text = $"{completed2} / {totalFetches}";
			Logger.Debug(string.Format("Fetched {0}", "Professions"));
			IApiV2ObjectList<Specialization> specs = await Gw2ApiManager.get_Gw2ApiClient().V2.Specializations.AllAsync();
			completed2 += 1.0;
			downloadBar.Progress = completed2 / (double)totalFetches;
			downloadBar.Text = $"{completed2} / {totalFetches}";
			Logger.Debug(string.Format("Fetched {0}", "Specs"));
			IApiV2ObjectList<Trait> traits = await Gw2ApiManager.get_Gw2ApiClient().V2.Traits.AllAsync();
			completed2 += 1.0;
			downloadBar.Progress = completed2 / (double)totalFetches;
			downloadBar.Text = $"{completed2} / {totalFetches}";
			Logger.Debug(string.Format("Fetched {0}", "Traits"));
			List<int> Skill_Ids = new List<int>();
			List<iData._Legend> legends = JsonConvert.DeserializeObject<List<iData._Legend>>(new StreamReader(ContentsManager.GetFileStream("data\\legends.json")).ReadToEnd());
			foreach (iData._Legend legend2 in legends)
			{
				if (!Skill_Ids.Contains(legend2.Skill))
				{
					Skill_Ids.Add(legend2.Skill);
				}
				if (!Skill_Ids.Contains(legend2.Swap))
				{
					Skill_Ids.Add(legend2.Swap);
				}
				if (!Skill_Ids.Contains(legend2.Heal))
				{
					Skill_Ids.Add(legend2.Heal);
				}
				if (!Skill_Ids.Contains(legend2.Elite))
				{
					Skill_Ids.Add(legend2.Elite);
				}
				foreach (int id5 in legend2.Utilities)
				{
					if (!Skill_Ids.Contains(id5))
					{
						Skill_Ids.Add(id5);
					}
				}
			}
			foreach (Profession profession2 in professions)
			{
				Logger.Debug($"Checking {profession2.Name} Skills");
				foreach (ProfessionSkill skill6 in profession2.Skills)
				{
					if (!Skill_Ids.Contains(skill6.Id))
					{
						Skill_Ids.Add(skill6.Id);
					}
				}
			}
			Logger.Debug($"Fetching a total of {Skill_Ids.Count} Skills");
			IApiV2ObjectList<Skill> skills = await Gw2ApiManager.get_Gw2ApiClient().V2.Skills.AllAsync();
			completed2 += 1.0;
			downloadBar.Progress = completed2 / (double)totalFetches;
			downloadBar.Text = $"{completed2} / {totalFetches}";
			Logger.Debug(string.Format("Fetched {0}", "Skills"));
			IApiV2ObjectList<Itemstat> stats = await Gw2ApiManager.get_Gw2ApiClient().V2.Itemstats.AllAsync();
			completed2 += 1.0;
			downloadBar.Progress = completed2 / (double)totalFetches;
			downloadBar.Text = $"{completed2} / {totalFetches}";
			Logger.Debug(string.Format("Fetched {0}", "Itemstats"));
			List<API.RuneItem> Runes = new List<API.RuneItem>();
			foreach (ItemUpgradeComponent rune in runes)
			{
				if (rune == null)
				{
					continue;
				}
				_ = rune.Icon;
				if (rune.Icon.Url != null)
				{
					API.RuneItem temp4 = new API.RuneItem
					{
						Name = rune.Name,
						Id = rune.Id,
						ChatLink = rune.ChatLink,
						Icon = new API.Icon
						{
							Url = rune.Icon.Url.ToString(),
							Path = Paths.rune_icons.Replace(Paths.BasePath, "") + Regex.Match((string)rune.Icon, "[0-9]*.png")
						},
						Bonuses = rune.Details.Bonuses.ToList()
					};
					Runes.Add(temp4);
					if (!System.IO.File.Exists(Paths.BasePath + temp4.Icon.Path))
					{
						downloadList.Add(new APIDownload_Image
						{
							display_text = $"Downloading Item Icon '{rune.Name}'",
							url = temp4.Icon.Url,
							path = Paths.BasePath + temp4.Icon.Path
						});
					}
					total2 += 1.0;
				}
			}
			System.IO.File.WriteAllText(Paths.runes + "runes [" + culture + "].json", JsonConvert.SerializeObject((object)Runes.ToArray(), settings));
			List<API.SigilItem> Sigils = new List<API.SigilItem>();
			foreach (ItemUpgradeComponent sigil in sigils)
			{
				if (sigil == null)
				{
					continue;
				}
				_ = sigil.Icon;
				if (sigil.Icon.Url != null)
				{
					API.SigilItem temp5 = new API.SigilItem
					{
						Name = sigil.Name,
						Id = sigil.Id,
						ChatLink = sigil.ChatLink,
						Icon = new API.Icon
						{
							Url = sigil.Icon.Url.ToString(),
							Path = Paths.sigil_icons.Replace(Paths.BasePath, "") + Regex.Match((string)sigil.Icon, "[0-9]*.png")
						},
						Description = sigil.Details.InfixUpgrade!.Buff.Description
					};
					Sigils.Add(temp5);
					if (!System.IO.File.Exists(Paths.BasePath + temp5.Icon.Path))
					{
						downloadList.Add(new APIDownload_Image
						{
							display_text = $"Downloading Item Icon '{sigil.Name}'",
							url = temp5.Icon.Url,
							path = Paths.BasePath + temp5.Icon.Path
						});
					}
					sigil.HttpResponseInfo = null;
					total2 += 1.0;
				}
			}
			System.IO.File.WriteAllText(Paths.sigils + "sigils [" + culture + "].json", JsonConvert.SerializeObject((object)Sigils.ToArray(), settings));
			List<API.Stat> Stats = new List<API.Stat>();
			foreach (Itemstat stat in stats)
			{
				if (stat == null || Enum.GetName(typeof(_EquipmentStats), stat.Id) == null)
				{
					continue;
				}
				API.Stat temp6 = new API.Stat
				{
					Name = stat.Name,
					Id = stat.Id,
					Icon = new API.Icon
					{
						Path = "textures\\stat icons\\" + stat.Id + ".png"
					}
				};
				foreach (ItemstatAttribute attribute in stat.Attributes)
				{
					temp6.Attributes.Add(new API.StatAttribute
					{
						Id = (int)attribute.Attribute.Value,
						Name = API.UniformAttributeName(attribute.Attribute.RawValue),
						Multiplier = attribute.Multiplier,
						Value = attribute.Value,
						Icon = new API.Icon
						{
							Path = "textures\\stats\\" + (int)attribute.Attribute.Value + ".png"
						}
					});
				}
				Stats.Add(temp6);
			}
			System.IO.File.WriteAllText(Paths.stats + "stats [" + culture + "].json", JsonConvert.SerializeObject((object)Stats.ToArray(), settings));
			List<API.ArmorItem> Armors = new List<API.ArmorItem>();
			List<API.WeaponItem> Weapons = new List<API.WeaponItem>();
			List<API.TrinketItem> Trinkets = new List<API.TrinketItem>();
			foreach (Item i in armory_items)
			{
				if (i == null)
				{
					continue;
				}
				_ = i.Icon;
				if (!(i.Icon.Url != null))
				{
					continue;
				}
				if (i.Type.RawValue == "Armor")
				{
					ItemArmor item2 = (ItemArmor)i;
					if (item2 != null)
					{
						API.ArmorItem temp8 = new API.ArmorItem
						{
							Name = item2.Name,
							Id = item2.Id,
							ChatLink = item2.ChatLink,
							Icon = new API.Icon
							{
								Url = item2.Icon.Url.ToString(),
								Path = Paths.armory_icons.Replace(Paths.BasePath, "") + Regex.Match((string)item2.Icon, "[0-9]*.png")
							},
							AttributeAdjustment = item2.Details.AttributeAdjustment
						};
						Enum.TryParse<API.armorSlot>(item2.Details.Type.RawValue, out temp8.Slot);
						Enum.TryParse<API.armorWeight>(item2.Details.WeightClass.RawValue, out temp8.ArmorWeight);
						Armors.Add(temp8);
					}
				}
				if (i.Type.RawValue == "Weapon")
				{
					ItemWeapon item3 = (ItemWeapon)i;
					if (item3 != null)
					{
						API.WeaponItem temp9 = new API.WeaponItem
						{
							Name = item3.Name,
							Id = item3.Id,
							ChatLink = item3.ChatLink,
							Icon = new API.Icon
							{
								Url = item3.Icon.Url.ToString(),
								Path = Paths.armory_icons.Replace(Paths.BasePath, "") + Regex.Match((string)item3.Icon, "[0-9]*.png")
							},
							AttributeAdjustment = item3.Details.AttributeAdjustment
						};
						Enum.TryParse<API.weaponSlot>(item3.Details.Type.RawValue, out temp9.Slot);
						Enum.TryParse<API.weaponType>(item3.Details.Type.RawValue, out temp9.WeaponType);
						Weapons.Add(temp9);
					}
				}
				if (i.Type.RawValue == "Trinket")
				{
					ItemTrinket item4 = (ItemTrinket)i;
					if (item4 != null)
					{
						API.TrinketItem temp7 = new API.TrinketItem
						{
							Name = item4.Name,
							Id = item4.Id,
							ChatLink = item4.ChatLink,
							Icon = new API.Icon
							{
								Url = item4.Icon.Url.ToString(),
								Path = Paths.armory_icons.Replace(Paths.BasePath, "") + Regex.Match((string)item4.Icon, "[0-9]*.png")
							},
							AttributeAdjustment = item4.Details.AttributeAdjustment
						};
						Enum.TryParse<API.trinketType>(item4.Details.Type.RawValue, out temp7.TrinketType);
						Trinkets.Add(temp7);
					}
				}
				if (i.Type.RawValue == "Back")
				{
					ItemBack item = (ItemBack)i;
					if (item != null)
					{
						Trinkets.Add(new API.TrinketItem
						{
							Name = item.Name,
							Id = item.Id,
							ChatLink = item.ChatLink,
							Icon = new API.Icon
							{
								Url = item.Icon.Url.ToString(),
								Path = Paths.armory_icons.Replace(Paths.BasePath, "") + Regex.Match((string)item.Icon, "[0-9]*.png")
							},
							TrinketType = API.trinketType.Back,
							AttributeAdjustment = item.Details.AttributeAdjustment
						});
					}
				}
				if (!System.IO.File.Exists(Paths.armory_icons + Regex.Match((string)i.Icon, "[0-9]*.png")))
				{
					downloadList.Add(new APIDownload_Image
					{
						display_text = $"Downloading Item Icon '{i.Name}'",
						url = (string)i.Icon,
						path = Paths.armory_icons + Regex.Match((string)i.Icon, "[0-9]*.png")
					});
				}
			}
			System.IO.File.WriteAllText(Paths.armory + "armors [" + culture + "].json", JsonConvert.SerializeObject((object)Armors.ToArray(), settings));
			System.IO.File.WriteAllText(Paths.armory + "weapons [" + culture + "].json", JsonConvert.SerializeObject((object)Weapons.ToArray(), settings));
			System.IO.File.WriteAllText(Paths.armory + "trinkets [" + culture + "].json", JsonConvert.SerializeObject((object)Trinkets.ToArray(), settings));
			Logger.Debug("Preparing Traits ....");
			List<API.Trait> Traits = new List<API.Trait>();
			foreach (Trait trait3 in traits)
			{
				if (trait3 != null)
				{
					_ = trait3.Icon;
					if (trait3.Icon.Url != null)
					{
						Traits.Add(new API.Trait
						{
							Name = trait3.Name,
							Description = trait3.Description,
							Id = trait3.Id,
							Icon = new API.Icon
							{
								Url = trait3.Icon.Url.ToString(),
								Path = Paths.traits_icons.Replace(Paths.BasePath, "") + Regex.Match((string)trait3.Icon, "[0-9]*.png")
							},
							Specialization = trait3.Specialization,
							Tier = trait3.Tier,
							Order = trait3.Order,
							Type = (API.traitType)Enum.Parse(typeof(API.traitType), trait3.Slot!.RawValue, ignoreCase: true)
						});
					}
				}
			}
			Logger.Debug("Preparing Specializations ....");
			List<API.Specialization> Specializations = new List<API.Specialization>();
			foreach (Specialization spec in specs)
			{
				if (spec == null)
				{
					continue;
				}
				_ = spec.Icon;
				if (!(spec.Icon.Url != null))
				{
					continue;
				}
				API.Specialization obj = new API.Specialization
				{
					Name = spec.Name,
					Id = spec.Id,
					Icon = new API.Icon
					{
						Url = spec.Icon.Url.ToString(),
						Path = Paths.spec_icons.Replace(Paths.BasePath, "") + Regex.Match((string)spec.Icon, "[0-9]*.png")
					},
					Background = new API.Icon
					{
						Url = spec.Background.Url.ToString(),
						Path = Paths.spec_backgrounds.Replace(Paths.BasePath, "") + Regex.Match((string)spec.Background, "[0-9]*.png")
					}
				};
				object obj2;
				if (!spec.ProfessionIconBig.HasValue)
				{
					obj2 = null;
				}
				else
				{
					obj2 = new API.Icon
					{
						Url = spec.ProfessionIconBig.ToString()
					};
					object obj3 = obj2;
					string text = Paths.spec_icons.Replace(Paths.BasePath, "");
					RenderUrl? professionIconBig = spec.ProfessionIconBig;
					((API.Icon)obj3).Path = text + Regex.Match(professionIconBig.HasValue ? ((string)professionIconBig.GetValueOrDefault()) : null, "[0-9]*.png");
				}
				obj.ProfessionIconBig = (API.Icon)obj2;
				object obj4;
				if (!spec.ProfessionIcon.HasValue)
				{
					obj4 = null;
				}
				else
				{
					obj4 = new API.Icon
					{
						Url = spec.ProfessionIcon.ToString()
					};
					object obj5 = obj4;
					string text2 = Paths.spec_icons.Replace(Paths.BasePath, "");
					RenderUrl? professionIconBig = spec.ProfessionIcon;
					((API.Icon)obj5).Path = text2 + Regex.Match(professionIconBig.HasValue ? ((string)professionIconBig.GetValueOrDefault()) : null, "[0-9]*.png");
				}
				obj.ProfessionIcon = (API.Icon)obj4;
				obj.Profession = spec.Profession;
				obj.Elite = spec.Elite;
				API.Specialization temp3 = obj;
				temp3.WeaponTrait = Traits.Find((API.Trait e) => e.Id == spec.WeaponTrait);
				if (temp3.WeaponTrait != null && !System.IO.File.Exists(Paths.BasePath + temp3.WeaponTrait.Icon.Path))
				{
					downloadList.Add(new APIDownload_Image
					{
						display_text = $"Downloading Trait Icon '{temp3.WeaponTrait.Name}'",
						url = temp3.WeaponTrait.Icon.Url,
						path = Paths.BasePath + temp3.WeaponTrait.Icon.Path
					});
				}
				foreach (int id4 in spec.MinorTraits)
				{
					API.Trait trait2 = Traits.Find((API.Trait e) => e.Id == id4);
					if (trait2 != null)
					{
						temp3.MinorTraits.Add(trait2);
						if (!System.IO.File.Exists(Paths.BasePath + trait2.Icon.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading Trait Icon '{trait2.Name}'",
								url = trait2.Icon.Url,
								path = Paths.BasePath + trait2.Icon.Path
							});
						}
					}
				}
				foreach (int id3 in spec.MajorTraits)
				{
					API.Trait trait = Traits.Find((API.Trait e) => e.Id == id3);
					if (trait != null)
					{
						temp3.MajorTraits.Add(trait);
						if (!System.IO.File.Exists(Paths.BasePath + trait.Icon.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading Trait Icon '{trait.Name}'",
								url = trait.Icon.Url,
								path = Paths.BasePath + trait.Icon.Path
							});
						}
					}
				}
				Specializations.Add(temp3);
			}
			Logger.Debug("Preparing Skills ....");
			List<API.Skill> Skills = new List<API.Skill>();
			foreach (Skill skill5 in skills)
			{
				if (skill5 == null)
				{
					continue;
				}
				_ = skill5.Icon;
				if (skill5.Professions.Count != 1)
				{
					continue;
				}
				API.Skill temp2 = new API.Skill
				{
					Name = skill5.Name,
					Id = skill5.Id,
					Icon = new API.Icon
					{
						Url = skill5.Icon.ToString(),
						Path = Paths.skill_icons.Replace(Paths.BasePath, "") + Regex.Match((string)skill5.Icon, "[0-9]*.png")
					},
					ChatLink = skill5.ChatLink,
					Description = skill5.Description,
					Specialization = (skill5.Specialization.HasValue ? skill5.Specialization.Value : 0),
					Flags = (from e in skill5.Flags.ToList()
						select e.RawValue).ToList(),
					Categories = new List<string>()
				};
				if (skill5.Categories != null)
				{
					foreach (string category in skill5.Categories!)
					{
						temp2.Categories.Add(category);
					}
				}
				Enum.TryParse<API.skillSlot>(skill5.Slot!.RawValue, out temp2.Slot);
				Skills.Add(temp2);
			}
			Logger.Debug("Preparing Professions ....");
			List<API.Profession> Professions = new List<API.Profession>();
			foreach (Profession profession in professions)
			{
				if (profession == null)
				{
					continue;
				}
				_ = profession.Icon;
				if (!(profession.Icon.Url != null))
				{
					continue;
				}
				API.Profession temp = new API.Profession
				{
					Name = profession.Name,
					Id = profession.Id,
					Icon = new API.Icon
					{
						Url = profession.Icon.Url.ToString(),
						Path = Paths.profession_icons.Replace(Paths.BasePath, "") + Regex.Match((string)profession.Icon, "[0-9]*.png")
					},
					IconBig = new API.Icon
					{
						Url = profession.IconBig.Url.ToString(),
						Path = Paths.profession_icons.Replace(Paths.BasePath, "") + Regex.Match((string)profession.IconBig, "[0-9]*.png")
					}
				};
				Logger.Debug("Adding Specs ....");
				foreach (int id2 in profession.Specializations)
				{
					API.Specialization spec2 = Specializations.Find((API.Specialization e) => e.Id == id2);
					if (spec2 != null)
					{
						temp.Specializations.Add(spec2);
						if (!System.IO.File.Exists(Paths.BasePath + spec2.Icon.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading Specialization Icon '{spec2.Name}'",
								url = spec2.Icon.Url,
								path = Paths.BasePath + spec2.Icon.Path
							});
						}
						if (!System.IO.File.Exists(Paths.BasePath + spec2.Background.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading Background '{spec2.Name}'",
								url = spec2.Background.Url,
								path = Paths.BasePath + spec2.Background.Path
							});
						}
						if (spec2.ProfessionIcon != null && !System.IO.File.Exists(Paths.BasePath + spec2.ProfessionIcon.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading ProfessionIcon '{spec2.Name}'",
								url = spec2.ProfessionIcon.Url,
								path = Paths.BasePath + spec2.ProfessionIcon.Path
							});
						}
						if (spec2.ProfessionIconBig != null && !System.IO.File.Exists(Paths.BasePath + spec2.ProfessionIconBig.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading ProfessionIconBig '{spec2.Name}'",
								url = spec2.ProfessionIconBig.Url,
								path = Paths.BasePath + spec2.ProfessionIconBig.Path
							});
						}
					}
				}
				Logger.Debug("Adding Weapons ....");
				foreach (KeyValuePair<string, ProfessionWeapon> weapon in profession.Weapons)
				{
					Enum.TryParse<API.weaponType>(weapon.Key, out var weaponType);
					temp.Weapons.Add(new API.ProfessionWeapon
					{
						Weapon = weaponType,
						Specialization = weapon.Value.Specialization,
						Wielded = weapon.Value.Flags.Select((ApiEnum<ProfessionWeaponFlag> e) => (API.weaponHand)Enum.Parse(typeof(API.weaponHand), e.RawValue)).ToList()
					});
				}
				Logger.Debug("Adding Skills ....");
				List<iData.SkillID_Pair> SkillID_Pairs = JsonConvert.DeserializeObject<List<iData.SkillID_Pair>>(new StreamReader(ContentsManager.GetFileStream("data\\skillpalettes.json")).ReadToEnd());
				if (profession.Id == "Revenant")
				{
					foreach (iData._Legend legend in legends)
					{
						API.Legend tempLegend = new API.Legend();
						tempLegend.Name = Skills.Find((API.Skill e) => e.Id == legend.Skill).Name;
						tempLegend.Skill = Skills.Find((API.Skill e) => e.Id == legend.Skill);
						tempLegend.Id = legend.Id;
						tempLegend.Specialization = legend.Specialization;
						tempLegend.Swap = Skills.Find((API.Skill e) => e.Id == legend.Swap);
						tempLegend.Heal = Skills.Find((API.Skill e) => e.Id == legend.Heal);
						tempLegend.Elite = Skills.Find((API.Skill e) => e.Id == legend.Elite);
						if (!System.IO.File.Exists(Paths.BasePath + tempLegend.Skill.Icon.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading Skill Icon '{tempLegend.Skill.Name}'",
								url = tempLegend.Skill.Icon.Url,
								path = Paths.BasePath + tempLegend.Skill.Icon.Path
							});
						}
						if (!System.IO.File.Exists(Paths.BasePath + tempLegend.Swap.Icon.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading Skill Icon '{tempLegend.Swap.Name}'",
								url = tempLegend.Swap.Icon.Url,
								path = Paths.BasePath + tempLegend.Swap.Icon.Path
							});
						}
						if (!System.IO.File.Exists(Paths.BasePath + tempLegend.Heal.Icon.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading Skill Icon '{tempLegend.Heal.Name}'",
								url = tempLegend.Heal.Icon.Url,
								path = Paths.BasePath + tempLegend.Heal.Icon.Path
							});
						}
						if (!System.IO.File.Exists(Paths.BasePath + tempLegend.Heal.Icon.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								display_text = $"Downloading Skill Icon '{tempLegend.Elite.Name}'",
								url = tempLegend.Elite.Icon.Url,
								path = Paths.BasePath + tempLegend.Elite.Icon.Path
							});
						}
						tempLegend.Utilities = new List<API.Skill>();
						foreach (int id in legend.Utilities)
						{
							API.Skill skill4 = Skills.Find((API.Skill e) => e.Id == id);
							tempLegend.Utilities.Add(skill4);
							if (!System.IO.File.Exists(Paths.BasePath + skill4.Icon.Path))
							{
								downloadList.Add(new APIDownload_Image
								{
									display_text = $"Downloading Skill Icon '{skill4.Name}'",
									url = skill4.Icon.Url,
									path = Paths.BasePath + skill4.Icon.Path
								});
							}
						}
						temp.Legends.Add(tempLegend);
					}
					foreach (ProfessionSkill iSkill in profession.Skills)
					{
						API.Skill skill3 = Skills.Find((API.Skill e) => e.Id == iSkill.Id);
						iData.SkillID_Pair paletteID = SkillID_Pairs.Find((iData.SkillID_Pair e) => e.ID == iSkill.Id);
						if (skill3 != null && paletteID != null)
						{
							skill3.PaletteId = paletteID.PaletteID;
							temp.Skills.Add(skill3);
							if (!System.IO.File.Exists(Paths.BasePath + skill3.Icon.Path))
							{
								downloadList.Add(new APIDownload_Image
								{
									display_text = $"Downloading Skill Icon '{skill3.Name}'",
									url = skill3.Icon.Url,
									path = Paths.BasePath + skill3.Icon.Path
								});
							}
						}
					}
					foreach (KeyValuePair<int, int> skillIDs2 in profession.SkillsByPalette)
					{
						API.Skill skill2 = Skills.Find((API.Skill e) => e.Id == skillIDs2.Value);
						if (skill2 != null && !temp.Skills.Contains(skill2))
						{
							skill2.PaletteId = skillIDs2.Key;
							temp.Skills.Add(skill2);
							if (!System.IO.File.Exists(Paths.BasePath + skill2.Icon.Path))
							{
								downloadList.Add(new APIDownload_Image
								{
									display_text = $"Downloading Skill Icon '{skill2.Name}'",
									url = skill2.Icon.Url,
									path = Paths.BasePath + skill2.Icon.Path
								});
							}
						}
					}
				}
				else
				{
					foreach (KeyValuePair<int, int> skillIDs in profession.SkillsByPalette)
					{
						API.Skill skill = Skills.Find((API.Skill e) => e.Id == skillIDs.Value);
						if (skill != null)
						{
							skill.PaletteId = skillIDs.Key;
							temp.Skills.Add(skill);
							if (!System.IO.File.Exists(Paths.BasePath + skill.Icon.Path))
							{
								downloadList.Add(new APIDownload_Image
								{
									display_text = $"Downloading Skill Icon '{skill.Name}'",
									url = skill.Icon.Url,
									path = Paths.BasePath + skill.Icon.Path
								});
							}
						}
					}
				}
				if (!System.IO.File.Exists(Paths.BasePath + temp.Icon.Path))
				{
					downloadList.Add(new APIDownload_Image
					{
						display_text = $"Downloading Profession Icon '{temp.Name}'",
						url = temp.Icon.Url,
						path = Paths.BasePath + temp.Icon.Path
					});
				}
				if (!System.IO.File.Exists(Paths.BasePath + temp.IconBig.Path))
				{
					downloadList.Add(new APIDownload_Image
					{
						display_text = $"Downloading Profession Icon '{temp.Name}'",
						url = temp.IconBig.Url,
						path = Paths.BasePath + temp.IconBig.Path
					});
				}
				Professions.Add(temp);
			}
			Logger.Debug("Saving Professions ....");
			System.IO.File.WriteAllText(Paths.professions + "professions [" + culture + "].json", JsonConvert.SerializeObject((object)Professions.ToArray(), settings));
			downloadBar.Progress = 0.0;
			total2 = downloadList.Count;
			completed2 = 0.0;
			Logger.Debug("All required Images queued. Downloading now ....");
			foreach (APIDownload_Image image in downloadList)
			{
				Logger.Debug("Downloading: '{0}' from url '{1}' to '{2}'", new object[3] { image.display_text, image.url, image.path });
				FileStream stream = new FileStream(image.path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
				await Gw2ApiManager.get_Gw2ApiClient().Render.DownloadToStreamAsync(stream, image.url);
				stream.Close();
				completed2 += 1.0;
				progress = completed2 / total2;
				downloadBar.Progress = progress;
				downloadBar.Text = string.Format("{0} / {1} ({2})", completed2, downloadList.Count, Math.Round(progress * 100.0, 2) + "%");
				((Control)downloadBar).set_BasicTooltipText(image.display_text);
				((Control)downloadBar._Label).set_BasicTooltipText(image.display_text);
				((Control)downloadBar._BackgroundTexture).set_BasicTooltipText(image.display_text);
				((Control)downloadBar._FilledTexture).set_BasicTooltipText(image.display_text);
			}
			((Control)loadingSpinner).set_Visible(false);
			((Control)downloadBar).set_Visible(false);
			GameVersion.set_Value(GameService.Gw2Mumble.get_Info().get_Version());
			ModuleVersion.set_Value(((object)((Module)this).get_Version().BaseVersion()).ToString());
			Logger.Debug("API Data sucessfully fetched!");
			FetchingAPI = false;
		}

		private async Task LoadData()
		{
			string culture = getCultureString();
			await Fetch_APIData(!System.IO.File.Exists(Paths.professions + "professions [" + culture + "].json"));
			if (Data == null)
			{
				Data = new iData();
			}
			else
			{
				Data.UpdateLanguage();
			}
			GameService.Overlay.get_UserLocale().add_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
		}

		private async void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			CultureString = getCultureString();
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
			await LoadData();
			OnLanguageChanged(null, null);
		}

		private void CreateUI()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			LoadTemplates();
			Selected_Template = new Template();
			int Height = 670;
			int Width = 915;
			Window_MainWindow window_MainWindow = new Window_MainWindow(TextureManager.getBackground(_Backgrounds.MainWindow), new Rectangle(30, 30, Width, Height + 30), new Rectangle(30, 15, Width - 3, Height + 25));
			((Control)window_MainWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)window_MainWindow).set_Title("Builds Manager");
			((WindowBase2)window_MainWindow).set_Emblem(TextureManager._Emblems[0]);
			((WindowBase2)window_MainWindow).set_Subtitle("v." + ((object)((Module)this).get_Version().BaseVersion()).ToString());
			((WindowBase2)window_MainWindow).set_SavesPosition(true);
			((WindowBase2)window_MainWindow).set_Id("BuildsManager New");
			MainWindow = window_MainWindow;
			Selected_Template = Selected_Template;
		}
	}
}
