using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
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
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Controls;
using Kenedia.Modules.BuildsManager.Enums;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager
{
	[Export(typeof(Module))]
	public class BuildsManager : Module
	{
		internal static BuildsManager s_moduleInstance;

		public static readonly Logger Logger = Logger.GetLogger<BuildsManager>();

		public const uint WM_COMMAND = 273u;

		public const uint WM_PASTE = 770u;

		public TextureManager TextureManager;

		public iPaths Paths;

		public Data Data;

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

		public LoadingSpinner LoadingSpinner;

		public ProgressBar DownloadBar;

		private CornerIcon _cornerIcon;

		private bool _dataLoaded;

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
				return _dataLoaded;
			}
			set
			{
				_dataLoaded = value;
				if (value)
				{
					s_moduleInstance.OnDataLoaded();
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
			s_moduleInstance = this;
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
			MainWindow.TemplateSelection.RefreshList();
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
			ReloadKey = internal_settings.DefineSetting<KeyBinding>("ReloadKey", new KeyBinding((Keys)0), (Func<string>)(() => "Reload Button"), (Func<string>)(() => string.Empty));
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
			MainWindow.TemplateSelection.Refresh();
		}

		private void PlayerCharacter_SpecializationChanged(object sender, ValueEventArgs<int> eventArgs)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Invalid comparison between Unknown and I4
			PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
			if (player != null && (int)player.get_Profession() > 0 && Data != null && MainWindow != null)
			{
				CurrentProfession = Data.Professions.Find(delegate(API.Profession e)
				{
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					string id = e.Id;
					ProfessionType profession = player.get_Profession();
					return id == ((object)(ProfessionType)(ref profession)).ToString();
				});
				CurrentSpecialization = CurrentProfession?.Specializations.Find((API.Specialization e) => e.Id == player.get_Specialization());
				MainWindow.PlayerCharacter_NameChanged(null, null);
				Templates = (from a in Templates
					orderby a.Build?.Profession != s_moduleInstance.CurrentProfession, a.Profession.Id
					select a).ThenBy((Template b) => b.Specialization?.Id).ThenBy((Template b) => b.Name).ToList();
				MainWindow.TemplateSelection.RefreshList();
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
			await Task.Delay(0);
		}

		public void ImportTemplates()
		{
			Template saveTemplate = null;
			if (File.Exists(Paths.builds + "config.ini"))
			{
				string text = File.ReadAllText(Paths.builds + "config.ini");
				bool started = false;
				string[] array = text.Split('\n');
				foreach (string s in array)
				{
					if (!(s.Trim() != string.Empty))
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
						Template template2 = new Template
						{
							Template_json = new Template_json
							{
								Name = buildpadBuild[5],
								BuildCode = buildpadBuild[1]
							}
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
				File.Delete(Paths.builds + "config.ini");
			}
			List<string> files = Directory.GetFiles(Paths.builds, "*.json", SearchOption.TopDirectoryOnly).ToList();
			if (files.Contains(Paths.builds + "Builds.json"))
			{
				files.Remove(Paths.builds + "Builds.json");
			}
			foreach (string item in files)
			{
				Template template = new Template(item);
				File.Delete(item);
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
				string content = string.Empty;
				try
				{
					if (File.Exists(path))
					{
						content = File.ReadAllText(path);
					}
					if (content == null || !(content != string.Empty))
					{
						continue;
					}
					foreach (Template_json jsonTemplate2 in JsonConvert.DeserializeObject<List<Template_json>>(content))
					{
						Template template2 = new Template(jsonTemplate2.Name, jsonTemplate2.BuildCode, jsonTemplate2.GearCode)
						{
							Path = path
						};
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
					Template template = new Template(jsonTemplate.Name, jsonTemplate.BuildCode, jsonTemplate.GearCode)
					{
						Path = null
					};
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

		protected override async void OnModuleLoaded(EventArgs e)
		{
			TextureManager = new TextureManager();
			BuildsManager buildsManager = this;
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(TextureManager.getIcon(Icons.Template)));
			((Control)val).set_BasicTooltipText(((Module)this).get_Name() ?? "");
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Visible(ShowCornerIcon.get_Value());
			buildsManager._cornerIcon = val;
			BuildsManager buildsManager2 = this;
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Location(new Point(((Control)_cornerIcon).get_Location().X - ((Control)_cornerIcon).get_Width(), ((Control)_cornerIcon).get_Location().Y + ((Control)_cornerIcon).get_Height() + 5));
			((Control)val2).set_Size(((Control)_cornerIcon).get_Size());
			((Control)val2).set_Visible(false);
			((Control)val2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			buildsManager2.LoadingSpinner = val2;
			BuildsManager buildsManager3 = this;
			ProgressBar progressBar = new ProgressBar();
			((Control)progressBar).set_Location(new Point(((Control)_cornerIcon).get_Location().X, ((Control)_cornerIcon).get_Location().Y + ((Control)_cornerIcon).get_Height() + 5 + 3));
			((Control)progressBar).set_Size(new Point(150, ((Control)_cornerIcon).get_Height() - 6));
			((Control)progressBar).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			progressBar.Progress = 0.66;
			((Control)progressBar).set_Visible(false);
			buildsManager3.DownloadBar = progressBar;
			((Control)_cornerIcon).add_MouseEntered((EventHandler<MouseEventArgs>)CornerIcon_MouseEntered);
			((Control)_cornerIcon).add_MouseLeft((EventHandler<MouseEventArgs>)CornerIcon_MouseLeft);
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)CornerIcon_Click);
			((Control)_cornerIcon).add_Moved((EventHandler<MovedEventArgs>)CornerIcon_Moved);
			ShowCornerIcon.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowCornerIcon_SettingChanged);
			DataLoaded_Event += BuildsManager_DataLoaded_Event;
			_003C_003En__0(e);
			await LoadData();
		}

		private void CornerIcon_Moved(object sender, MovedEventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			((Control)LoadingSpinner).set_Location(new Point(((Control)_cornerIcon).get_Location().X - ((Control)_cornerIcon).get_Width(), ((Control)_cornerIcon).get_Location().Y + ((Control)_cornerIcon).get_Height() + 5));
			((Control)DownloadBar).set_Location(new Point(((Control)_cornerIcon).get_Location().X, ((Control)_cornerIcon).get_Location().Y + ((Control)_cornerIcon).get_Height() + 5 + 3));
		}

		private void BuildsManager_DataLoaded_Event(object sender, EventArgs e)
		{
			CreateUI();
		}

		private void CornerIcon_Click(object sender, MouseEventArgs e)
		{
			Window_MainWindow mainWindow = MainWindow;
			if (mainWindow != null)
			{
				((WindowBase2)mainWindow).ToggleWindow();
			}
		}

		private void CornerIcon_MouseLeft(object sender, MouseEventArgs e)
		{
			_cornerIcon.set_Icon(AsyncTexture2D.op_Implicit(TextureManager.getIcon(Icons.Template)));
		}

		private void CornerIcon_MouseEntered(object sender, MouseEventArgs e)
		{
			_cornerIcon.set_Icon(AsyncTexture2D.op_Implicit(TextureManager.getIcon(Icons.Template_White)));
		}

		private void ShowCornerIcon_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (_cornerIcon != null)
			{
				((Control)_cornerIcon).set_Visible(e.get_NewValue());
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
					((Control)MainWindow.ImportButton).set_Visible(File.Exists(Paths.builds + "config.ini"));
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
			if (_Selected_Template != null)
			{
				_Selected_Template.Edit -= OnSelected_Template_Edit;
			}
			Selected_Template = null;
			CurrentProfession = null;
			CurrentSpecialization = null;
			Data?.Dispose();
			Data = null;
			TextureManager = null;
			ToggleWindow.get_Value().set_Enabled(false);
			ToggleWindow.get_Value().remove_Activated((EventHandler<EventArgs>)ToggleWindow_Activated);
			ReloadKey.get_Value().set_Enabled(false);
			ReloadKey.get_Value().remove_Activated((EventHandler<EventArgs>)ReloadKey_Activated);
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			if (_cornerIcon != null)
			{
				((Control)_cornerIcon).remove_MouseEntered((EventHandler<MouseEventArgs>)CornerIcon_MouseEntered);
				((Control)_cornerIcon).remove_MouseLeft((EventHandler<MouseEventArgs>)CornerIcon_MouseLeft);
				((Control)_cornerIcon).remove_Click((EventHandler<MouseEventArgs>)CornerIcon_Click);
				((Control)_cornerIcon).remove_Moved((EventHandler<MovedEventArgs>)CornerIcon_Moved);
			}
			DataLoaded_Event -= BuildsManager_DataLoaded_Event;
			ShowCornerIcon.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowCornerIcon_SettingChanged);
			IncludeDefaultBuilds.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)IncludeDefaultBuilds_SettingChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_SpecializationChanged((EventHandler<ValueEventArgs<int>>)PlayerCharacter_SpecializationChanged);
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
			ProgressBar downloadBar = DownloadBar;
			if (downloadBar != null)
			{
				((Control)downloadBar).Dispose();
			}
			DownloadBar = null;
			DataLoaded = false;
			s_moduleInstance = null;
		}

		public static string getCultureString()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected I4, but got Unknown
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			return (value - 1) switch
			{
				2 => "fr-FR", 
				0 => "es-ES", 
				1 => "de-DE", 
				_ => "en-EN", 
			};
		}

		public async Task<bool> Fetch_APIData(bool force = false)
		{
			if (GameVersion.get_Value() != GameService.Gw2Mumble.get_Info().get_Version() || ModuleVersion.get_Value() != ((object)((Module)this).get_Version().BaseVersion()).ToString() || force)
			{
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
				((Control)LoadingSpinner).set_Visible(true);
				((Control)DownloadBar).set_Visible(true);
				DownloadBar.Progress = progress;
				DownloadBar.Text = $"{completed2} / {totalFetches}";
				try
				{
					IReadOnlyList<Item> sigils = await ((IBulkExpandableClient<Item, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Items()).ManyAsync((IEnumerable<int>)_sigils, default(CancellationToken));
					completed2 += 1.0;
					DownloadBar.Progress = completed2 / (double)totalFetches;
					DownloadBar.Text = $"{completed2} / {totalFetches}";
					Logger.Debug(string.Format("Fetched {0}", "Sigils"));
					IReadOnlyList<Item> runes = await ((IBulkExpandableClient<Item, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Items()).ManyAsync((IEnumerable<int>)_runes, default(CancellationToken));
					completed2 += 1.0;
					DownloadBar.Progress = completed2 / (double)totalFetches;
					DownloadBar.Text = $"{completed2} / {totalFetches}";
					Logger.Debug(string.Format("Fetched {0}", "Runes"));
					IReadOnlyList<Item> armory_items = await ((IBulkExpandableClient<Item, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Items()).ManyAsync((IEnumerable<int>)ArmoryItems, default(CancellationToken));
					completed2 += 1.0;
					DownloadBar.Progress = completed2 / (double)totalFetches;
					DownloadBar.Text = $"{completed2} / {totalFetches}";
					Logger.Debug(string.Format("Fetched {0}", "Armory"));
					IApiV2ObjectList<Profession> professions = await ((IAllExpandableClient<Profession>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Professions()).AllAsync(default(CancellationToken));
					completed2 += 1.0;
					DownloadBar.Progress = completed2 / (double)totalFetches;
					DownloadBar.Text = $"{completed2} / {totalFetches}";
					Logger.Debug(string.Format("Fetched {0}", "Professions"));
					IApiV2ObjectList<Specialization> specs = await ((IAllExpandableClient<Specialization>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Specializations()).AllAsync(default(CancellationToken));
					completed2 += 1.0;
					DownloadBar.Progress = completed2 / (double)totalFetches;
					DownloadBar.Text = $"{completed2} / {totalFetches}";
					Logger.Debug(string.Format("Fetched {0}", "Specs"));
					IApiV2ObjectList<Trait> traits = await ((IAllExpandableClient<Trait>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Traits()).AllAsync(default(CancellationToken));
					completed2 += 1.0;
					DownloadBar.Progress = completed2 / (double)totalFetches;
					DownloadBar.Text = $"{completed2} / {totalFetches}";
					Logger.Debug(string.Format("Fetched {0}", "Traits"));
					List<int> Skill_Ids = new List<int>();
					List<Data.Legend> legends = JsonConvert.DeserializeObject<List<Data.Legend>>(new StreamReader(ContentsManager.GetFileStream("data\\legends.json")).ReadToEnd());
					foreach (Data.Legend legend2 in legends)
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
					foreach (Profession profession2 in (IEnumerable<Profession>)professions)
					{
						Logger.Debug($"Checking {profession2.get_Name()} Skills");
						foreach (ProfessionSkill skill6 in profession2.get_Skills())
						{
							if (!Skill_Ids.Contains(skill6.get_Id()))
							{
								Skill_Ids.Add(skill6.get_Id());
							}
						}
					}
					Logger.Debug($"Fetching a total of {Skill_Ids.Count} Skills");
					IApiV2ObjectList<Skill> skills = await ((IAllExpandableClient<Skill>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Skills()).AllAsync(default(CancellationToken));
					completed2 += 1.0;
					DownloadBar.Progress = completed2 / (double)totalFetches;
					DownloadBar.Text = $"{completed2} / {totalFetches}";
					Logger.Debug(string.Format("Fetched {0}", "Skills"));
					IApiV2ObjectList<Itemstat> stats = await ((IAllExpandableClient<Itemstat>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Itemstats()).AllAsync(default(CancellationToken));
					completed2 += 1.0;
					DownloadBar.Progress = completed2 / (double)totalFetches;
					DownloadBar.Text = $"{completed2} / {totalFetches}";
					Logger.Debug(string.Format("Fetched {0}", "Itemstats"));
					List<API.RuneItem> Runes = new List<API.RuneItem>();
					RenderUrl val2;
					foreach (ItemUpgradeComponent item5 in runes)
					{
						ItemUpgradeComponent rune = item5;
						if (rune == null)
						{
							continue;
						}
						((Item)rune).get_Icon();
						val2 = ((Item)rune).get_Icon();
						if (((RenderUrl)(ref val2)).get_Url() != null)
						{
							API.RuneItem obj = new API.RuneItem
							{
								Name = ((Item)rune).get_Name(),
								Id = ((Item)rune).get_Id(),
								ChatLink = ((Item)rune).get_ChatLink()
							};
							API.Icon icon = new API.Icon();
							val2 = ((Item)rune).get_Icon();
							icon.Url = ((RenderUrl)(ref val2)).get_Url().ToString();
							icon.Path = Paths.rune_icons.Replace(Paths.BasePath, string.Empty) + Regex.Match(RenderUrl.op_Implicit(((Item)rune).get_Icon()), "[0-9]*.png");
							obj.Icon = icon;
							obj.Bonuses = rune.get_Details().get_Bonuses().ToList();
							API.RuneItem temp4 = obj;
							Runes.Add(temp4);
							if (!File.Exists(Paths.BasePath + temp4.Icon.Path))
							{
								downloadList.Add(new APIDownload_Image
								{
									Display_text = $"Downloading Item Icon '{((Item)rune).get_Name()}'",
									Url = temp4.Icon.Url,
									Path = Paths.BasePath + temp4.Icon.Path
								});
							}
							total2 += 1.0;
						}
					}
					File.WriteAllText(Paths.runes + "runes [" + culture + "].json", JsonConvert.SerializeObject((object)Runes.ToArray(), settings));
					List<API.SigilItem> Sigils = new List<API.SigilItem>();
					foreach (ItemUpgradeComponent item6 in sigils)
					{
						ItemUpgradeComponent sigil = item6;
						if (sigil == null)
						{
							continue;
						}
						((Item)sigil).get_Icon();
						val2 = ((Item)sigil).get_Icon();
						if (((RenderUrl)(ref val2)).get_Url() != null)
						{
							API.SigilItem obj2 = new API.SigilItem
							{
								Name = ((Item)sigil).get_Name(),
								Id = ((Item)sigil).get_Id(),
								ChatLink = ((Item)sigil).get_ChatLink()
							};
							API.Icon icon2 = new API.Icon();
							val2 = ((Item)sigil).get_Icon();
							icon2.Url = ((RenderUrl)(ref val2)).get_Url().ToString();
							icon2.Path = Paths.sigil_icons.Replace(Paths.BasePath, string.Empty) + Regex.Match(RenderUrl.op_Implicit(((Item)sigil).get_Icon()), "[0-9]*.png");
							obj2.Icon = icon2;
							obj2.Description = sigil.get_Details().get_InfixUpgrade().get_Buff()
								.get_Description();
							API.SigilItem temp5 = obj2;
							Sigils.Add(temp5);
							if (!File.Exists(Paths.BasePath + temp5.Icon.Path))
							{
								downloadList.Add(new APIDownload_Image
								{
									Display_text = $"Downloading Item Icon '{((Item)sigil).get_Name()}'",
									Url = temp5.Icon.Url,
									Path = Paths.BasePath + temp5.Icon.Path
								});
							}
							((ApiV2BaseObject)sigil).set_HttpResponseInfo((ApiV2HttpResponseInfo)null);
							total2 += 1.0;
						}
					}
					File.WriteAllText(Paths.sigils + "sigils [" + culture + "].json", JsonConvert.SerializeObject((object)Sigils.ToArray(), settings));
					List<API.Stat> Stats = new List<API.Stat>();
					foreach (Itemstat stat in (IEnumerable<Itemstat>)stats)
					{
						if (stat == null || Enum.GetName(typeof(EquipmentStats), stat.get_Id()) == null)
						{
							continue;
						}
						API.Stat temp6 = new API.Stat
						{
							Name = stat.get_Name(),
							Id = stat.get_Id(),
							Icon = new API.Icon
							{
								Path = "textures\\stat icons\\" + stat.get_Id() + ".png"
							}
						};
						foreach (ItemstatAttribute attribute in stat.get_Attributes())
						{
							temp6.Attributes.Add(new API.StatAttribute
							{
								Id = (int)attribute.get_Attribute().get_Value(),
								Name = API.UniformAttributeName(attribute.get_Attribute().get_RawValue()),
								Multiplier = attribute.get_Multiplier(),
								Value = attribute.get_Value(),
								Icon = new API.Icon
								{
									Path = "textures\\stats\\" + (int)attribute.get_Attribute().get_Value() + ".png"
								}
							});
						}
						Stats.Add(temp6);
					}
					File.WriteAllText(Paths.stats + "stats [" + culture + "].json", JsonConvert.SerializeObject((object)Stats.ToArray(), settings));
					List<API.ArmorItem> Armors = new List<API.ArmorItem>();
					List<API.WeaponItem> Weapons = new List<API.WeaponItem>();
					List<API.TrinketItem> Trinkets = new List<API.TrinketItem>();
					foreach (Item i in armory_items)
					{
						if (i == null)
						{
							continue;
						}
						i.get_Icon();
						val2 = i.get_Icon();
						if (!(((RenderUrl)(ref val2)).get_Url() != null))
						{
							continue;
						}
						if (i.get_Type().get_RawValue() == "Armor")
						{
							ItemArmor item2 = (ItemArmor)i;
							if (item2 != null)
							{
								API.ArmorItem obj3 = new API.ArmorItem
								{
									Name = ((Item)item2).get_Name(),
									Id = ((Item)item2).get_Id(),
									ChatLink = ((Item)item2).get_ChatLink()
								};
								API.Icon icon3 = new API.Icon();
								val2 = ((Item)item2).get_Icon();
								icon3.Url = ((RenderUrl)(ref val2)).get_Url().ToString();
								icon3.Path = Paths.armory_icons.Replace(Paths.BasePath, string.Empty) + Regex.Match(RenderUrl.op_Implicit(((Item)item2).get_Icon()), "[0-9]*.png");
								obj3.Icon = icon3;
								obj3.AttributeAdjustment = item2.get_Details().get_AttributeAdjustment();
								API.ArmorItem temp8 = obj3;
								Enum.TryParse<API.armorSlot>(item2.get_Details().get_Type().get_RawValue(), out temp8.Slot);
								Enum.TryParse<API.armorWeight>(item2.get_Details().get_WeightClass().get_RawValue(), out temp8.ArmorWeight);
								Armors.Add(temp8);
							}
						}
						if (i.get_Type().get_RawValue() == "Weapon")
						{
							ItemWeapon item3 = (ItemWeapon)i;
							if (item3 != null)
							{
								API.WeaponItem obj4 = new API.WeaponItem
								{
									Name = ((Item)item3).get_Name(),
									Id = ((Item)item3).get_Id(),
									ChatLink = ((Item)item3).get_ChatLink()
								};
								API.Icon icon4 = new API.Icon();
								val2 = ((Item)item3).get_Icon();
								icon4.Url = ((RenderUrl)(ref val2)).get_Url().ToString();
								icon4.Path = Paths.armory_icons.Replace(Paths.BasePath, string.Empty) + Regex.Match(RenderUrl.op_Implicit(((Item)item3).get_Icon()), "[0-9]*.png");
								obj4.Icon = icon4;
								obj4.AttributeAdjustment = item3.get_Details().get_AttributeAdjustment();
								API.WeaponItem temp9 = obj4;
								Enum.TryParse<API.weaponSlot>(item3.get_Details().get_Type().get_RawValue(), out temp9.Slot);
								Enum.TryParse<API.weaponType>(item3.get_Details().get_Type().get_RawValue(), out temp9.WeaponType);
								Weapons.Add(temp9);
							}
						}
						if (i.get_Type().get_RawValue() == "Trinket")
						{
							ItemTrinket item4 = (ItemTrinket)i;
							if (item4 != null)
							{
								API.TrinketItem obj5 = new API.TrinketItem
								{
									Name = ((Item)item4).get_Name(),
									Id = ((Item)item4).get_Id(),
									ChatLink = ((Item)item4).get_ChatLink()
								};
								API.Icon icon5 = new API.Icon();
								val2 = ((Item)item4).get_Icon();
								icon5.Url = ((RenderUrl)(ref val2)).get_Url().ToString();
								icon5.Path = Paths.armory_icons.Replace(Paths.BasePath, string.Empty) + Regex.Match(RenderUrl.op_Implicit(((Item)item4).get_Icon()), "[0-9]*.png");
								obj5.Icon = icon5;
								obj5.AttributeAdjustment = item4.get_Details().get_AttributeAdjustment();
								API.TrinketItem temp7 = obj5;
								Enum.TryParse<API.trinketType>(item4.get_Details().get_Type().get_RawValue(), out temp7.TrinketType);
								Trinkets.Add(temp7);
							}
						}
						if (i.get_Type().get_RawValue() == "Back")
						{
							ItemBack item = (ItemBack)i;
							if (item != null)
							{
								API.TrinketItem obj6 = new API.TrinketItem
								{
									Name = ((Item)item).get_Name(),
									Id = ((Item)item).get_Id(),
									ChatLink = ((Item)item).get_ChatLink()
								};
								API.Icon icon6 = new API.Icon();
								val2 = ((Item)item).get_Icon();
								icon6.Url = ((RenderUrl)(ref val2)).get_Url().ToString();
								icon6.Path = Paths.armory_icons.Replace(Paths.BasePath, string.Empty) + Regex.Match(RenderUrl.op_Implicit(((Item)item).get_Icon()), "[0-9]*.png");
								obj6.Icon = icon6;
								obj6.TrinketType = API.trinketType.Back;
								obj6.AttributeAdjustment = item.get_Details().get_AttributeAdjustment();
								Trinkets.Add(obj6);
							}
						}
						if (!File.Exists(Paths.armory_icons + Regex.Match(RenderUrl.op_Implicit(i.get_Icon()), "[0-9]*.png")))
						{
							downloadList.Add(new APIDownload_Image
							{
								Display_text = $"Downloading Item Icon '{i.get_Name()}'",
								Url = RenderUrl.op_Implicit(i.get_Icon()),
								Path = Paths.armory_icons + Regex.Match(RenderUrl.op_Implicit(i.get_Icon()), "[0-9]*.png")
							});
						}
					}
					File.WriteAllText(Paths.armory + "armors [" + culture + "].json", JsonConvert.SerializeObject((object)Armors.ToArray(), settings));
					File.WriteAllText(Paths.armory + "weapons [" + culture + "].json", JsonConvert.SerializeObject((object)Weapons.ToArray(), settings));
					File.WriteAllText(Paths.armory + "trinkets [" + culture + "].json", JsonConvert.SerializeObject((object)Trinkets.ToArray(), settings));
					Logger.Debug("Preparing Traits ....");
					List<API.Trait> Traits = new List<API.Trait>();
					foreach (Trait trait3 in (IEnumerable<Trait>)traits)
					{
						if (trait3 != null)
						{
							trait3.get_Icon();
							val2 = trait3.get_Icon();
							if (((RenderUrl)(ref val2)).get_Url() != null)
							{
								API.Trait obj7 = new API.Trait
								{
									Name = trait3.get_Name(),
									Description = trait3.get_Description(),
									Id = trait3.get_Id()
								};
								API.Icon icon7 = new API.Icon();
								val2 = trait3.get_Icon();
								icon7.Url = ((RenderUrl)(ref val2)).get_Url().ToString();
								icon7.Path = Paths.traits_icons.Replace(Paths.BasePath, string.Empty) + Regex.Match(RenderUrl.op_Implicit(trait3.get_Icon()), "[0-9]*.png");
								obj7.Icon = icon7;
								obj7.Specialization = trait3.get_Specialization();
								obj7.Tier = trait3.get_Tier();
								obj7.Order = trait3.get_Order();
								obj7.Type = (API.traitType)Enum.Parse(typeof(API.traitType), trait3.get_Slot().get_RawValue(), ignoreCase: true);
								Traits.Add(obj7);
							}
						}
					}
					Logger.Debug("Preparing Specializations ....");
					List<API.Specialization> Specializations = new List<API.Specialization>();
					foreach (Specialization spec in (IEnumerable<Specialization>)specs)
					{
						if (spec == null)
						{
							continue;
						}
						spec.get_Icon();
						val2 = spec.get_Icon();
						if (!(((RenderUrl)(ref val2)).get_Url() != null))
						{
							continue;
						}
						API.Specialization obj8 = new API.Specialization
						{
							Name = spec.get_Name(),
							Id = spec.get_Id()
						};
						API.Icon icon8 = new API.Icon();
						val2 = spec.get_Icon();
						icon8.Url = ((RenderUrl)(ref val2)).get_Url().ToString();
						icon8.Path = Paths.spec_icons.Replace(Paths.BasePath, string.Empty) + Regex.Match(RenderUrl.op_Implicit(spec.get_Icon()), "[0-9]*.png");
						obj8.Icon = icon8;
						API.Icon icon9 = new API.Icon();
						val2 = spec.get_Background();
						icon9.Url = ((RenderUrl)(ref val2)).get_Url().ToString();
						icon9.Path = Paths.spec_backgrounds.Replace(Paths.BasePath, string.Empty) + Regex.Match(RenderUrl.op_Implicit(spec.get_Background()), "[0-9]*.png");
						obj8.Background = icon9;
						object obj9;
						if (!spec.get_ProfessionIconBig().HasValue)
						{
							obj9 = null;
						}
						else
						{
							obj9 = new API.Icon
							{
								Url = spec.get_ProfessionIconBig().ToString()
							};
							object obj10 = obj9;
							string text = Paths.spec_icons.Replace(Paths.BasePath, string.Empty);
							RenderUrl? professionIconBig = spec.get_ProfessionIconBig();
							((API.Icon)obj10).Path = text + Regex.Match(professionIconBig.HasValue ? RenderUrl.op_Implicit(professionIconBig.GetValueOrDefault()) : null, "[0-9]*.png");
						}
						obj8.ProfessionIconBig = (API.Icon)obj9;
						object obj11;
						if (!spec.get_ProfessionIcon().HasValue)
						{
							obj11 = null;
						}
						else
						{
							obj11 = new API.Icon
							{
								Url = spec.get_ProfessionIcon().ToString()
							};
							object obj12 = obj11;
							string text2 = Paths.spec_icons.Replace(Paths.BasePath, string.Empty);
							RenderUrl? professionIconBig = spec.get_ProfessionIcon();
							((API.Icon)obj12).Path = text2 + Regex.Match(professionIconBig.HasValue ? RenderUrl.op_Implicit(professionIconBig.GetValueOrDefault()) : null, "[0-9]*.png");
						}
						obj8.ProfessionIcon = (API.Icon)obj11;
						obj8.Profession = spec.get_Profession();
						obj8.Elite = spec.get_Elite();
						obj8.WeaponTrait = Traits.Find((API.Trait e) => e.Id == spec.get_WeaponTrait());
						API.Specialization temp3 = obj8;
						if (temp3.WeaponTrait != null && !File.Exists(Paths.BasePath + temp3.WeaponTrait.Icon.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								Display_text = $"Downloading Trait Icon '{temp3.WeaponTrait.Name}'",
								Url = temp3.WeaponTrait.Icon.Url,
								Path = Paths.BasePath + temp3.WeaponTrait.Icon.Path
							});
						}
						foreach (int id4 in spec.get_MinorTraits())
						{
							API.Trait trait2 = Traits.Find((API.Trait e) => e.Id == id4);
							if (trait2 != null)
							{
								temp3.MinorTraits.Add(trait2);
								if (!File.Exists(Paths.BasePath + trait2.Icon.Path))
								{
									downloadList.Add(new APIDownload_Image
									{
										Display_text = $"Downloading Trait Icon '{trait2.Name}'",
										Url = trait2.Icon.Url,
										Path = Paths.BasePath + trait2.Icon.Path
									});
								}
							}
						}
						foreach (int id3 in spec.get_MajorTraits())
						{
							API.Trait trait = Traits.Find((API.Trait e) => e.Id == id3);
							if (trait != null)
							{
								temp3.MajorTraits.Add(trait);
								if (!File.Exists(Paths.BasePath + trait.Icon.Path))
								{
									downloadList.Add(new APIDownload_Image
									{
										Display_text = $"Downloading Trait Icon '{trait.Name}'",
										Url = trait.Icon.Url,
										Path = Paths.BasePath + trait.Icon.Path
									});
								}
							}
						}
						Specializations.Add(temp3);
					}
					Logger.Debug("Preparing Skills ....");
					List<API.Skill> Skills = new List<API.Skill>();
					foreach (Skill skill5 in (IEnumerable<Skill>)skills)
					{
						if (skill5 == null || !skill5.get_Icon().HasValue || skill5.get_Professions().Count != 1)
						{
							continue;
						}
						API.Skill obj13 = new API.Skill
						{
							Name = skill5.get_Name(),
							Id = skill5.get_Id()
						};
						API.Icon obj14 = new API.Icon
						{
							Url = skill5.get_Icon().ToString()
						};
						string text3 = Paths.skill_icons.Replace(Paths.BasePath, string.Empty);
						RenderUrl? professionIconBig = skill5.get_Icon();
						obj14.Path = text3 + Regex.Match(professionIconBig.HasValue ? RenderUrl.op_Implicit(professionIconBig.GetValueOrDefault()) : null, "[0-9]*.png");
						obj13.Icon = obj14;
						obj13.ChatLink = skill5.get_ChatLink();
						obj13.Description = skill5.get_Description();
						obj13.Specialization = (skill5.get_Specialization().HasValue ? skill5.get_Specialization().Value : 0);
						obj13.Flags = (from e in skill5.get_Flags().ToList()
							select e.get_RawValue()).ToList();
						obj13.Categories = new List<string>();
						API.Skill temp2 = obj13;
						if (skill5.get_Categories() != null)
						{
							foreach (string category in skill5.get_Categories())
							{
								temp2.Categories.Add(category);
							}
						}
						Enum.TryParse<API.skillSlot>(skill5.get_Slot().get_RawValue(), out temp2.Slot);
						Skills.Add(temp2);
					}
					Logger.Debug("Preparing Professions ....");
					List<API.Profession> Professions = new List<API.Profession>();
					foreach (Profession profession in (IEnumerable<Profession>)professions)
					{
						if (profession == null)
						{
							continue;
						}
						profession.get_Icon();
						val2 = profession.get_Icon();
						if (!(((RenderUrl)(ref val2)).get_Url() != null))
						{
							continue;
						}
						API.Profession obj15 = new API.Profession
						{
							Name = profession.get_Name(),
							Id = profession.get_Id()
						};
						API.Icon icon10 = new API.Icon();
						val2 = profession.get_Icon();
						icon10.Url = ((RenderUrl)(ref val2)).get_Url().ToString();
						icon10.Path = Paths.profession_icons.Replace(Paths.BasePath, string.Empty) + Regex.Match(RenderUrl.op_Implicit(profession.get_Icon()), "[0-9]*.png");
						obj15.Icon = icon10;
						API.Icon icon11 = new API.Icon();
						val2 = profession.get_IconBig();
						icon11.Url = ((RenderUrl)(ref val2)).get_Url().ToString();
						icon11.Path = Paths.profession_icons.Replace(Paths.BasePath, string.Empty) + Regex.Match(RenderUrl.op_Implicit(profession.get_IconBig()), "[0-9]*.png");
						obj15.IconBig = icon11;
						API.Profession temp = obj15;
						Logger.Debug("Adding Specs ....");
						foreach (int id2 in profession.get_Specializations())
						{
							API.Specialization spec2 = Specializations.Find((API.Specialization e) => e.Id == id2);
							if (spec2 != null)
							{
								temp.Specializations.Add(spec2);
								if (!File.Exists(Paths.BasePath + spec2.Icon.Path))
								{
									downloadList.Add(new APIDownload_Image
									{
										Display_text = $"Downloading Specialization Icon '{spec2.Name}'",
										Url = spec2.Icon.Url,
										Path = Paths.BasePath + spec2.Icon.Path
									});
								}
								if (!File.Exists(Paths.BasePath + spec2.Background.Path))
								{
									downloadList.Add(new APIDownload_Image
									{
										Display_text = $"Downloading Background '{spec2.Name}'",
										Url = spec2.Background.Url,
										Path = Paths.BasePath + spec2.Background.Path
									});
								}
								if (spec2.ProfessionIcon != null && !File.Exists(Paths.BasePath + spec2.ProfessionIcon.Path))
								{
									downloadList.Add(new APIDownload_Image
									{
										Display_text = $"Downloading ProfessionIcon '{spec2.Name}'",
										Url = spec2.ProfessionIcon.Url,
										Path = Paths.BasePath + spec2.ProfessionIcon.Path
									});
								}
								if (spec2.ProfessionIconBig != null && !File.Exists(Paths.BasePath + spec2.ProfessionIconBig.Path))
								{
									downloadList.Add(new APIDownload_Image
									{
										Display_text = $"Downloading ProfessionIconBig '{spec2.Name}'",
										Url = spec2.ProfessionIconBig.Url,
										Path = Paths.BasePath + spec2.ProfessionIconBig.Path
									});
								}
							}
						}
						Logger.Debug("Adding Weapons ....");
						foreach (KeyValuePair<string, ProfessionWeapon> weapon in profession.get_Weapons())
						{
							Enum.TryParse<API.weaponType>(weapon.Key, out var weaponType);
							temp.Weapons.Add(new API.ProfessionWeapon
							{
								Weapon = weaponType,
								Specialization = weapon.Value.get_Specialization(),
								Wielded = ((IEnumerable<ApiEnum<ProfessionWeaponFlag>>)weapon.Value.get_Flags()).Select((ApiEnum<ProfessionWeaponFlag> e) => (API.weaponHand)Enum.Parse(typeof(API.weaponHand), e.get_RawValue())).ToList()
							});
						}
						Logger.Debug("Adding Skills ....");
						List<Data.SkillID_Pair> SkillID_Pairs = JsonConvert.DeserializeObject<List<Data.SkillID_Pair>>(new StreamReader(ContentsManager.GetFileStream("data\\skillpalettes.json")).ReadToEnd());
						if (profession.get_Id() == "Revenant")
						{
							foreach (Data.Legend legend in legends)
							{
								API.Legend tempLegend = new API.Legend
								{
									Name = Skills.Find((API.Skill e) => e.Id == legend.Skill).Name,
									Skill = Skills.Find((API.Skill e) => e.Id == legend.Skill),
									Id = legend.Id,
									Specialization = legend.Specialization,
									Swap = Skills.Find((API.Skill e) => e.Id == legend.Swap),
									Heal = Skills.Find((API.Skill e) => e.Id == legend.Heal),
									Elite = Skills.Find((API.Skill e) => e.Id == legend.Elite)
								};
								if (!File.Exists(Paths.BasePath + tempLegend.Skill.Icon.Path))
								{
									downloadList.Add(new APIDownload_Image
									{
										Display_text = $"Downloading Skill Icon '{tempLegend.Skill.Name}'",
										Url = tempLegend.Skill.Icon.Url,
										Path = Paths.BasePath + tempLegend.Skill.Icon.Path
									});
								}
								if (!File.Exists(Paths.BasePath + tempLegend.Swap.Icon.Path))
								{
									downloadList.Add(new APIDownload_Image
									{
										Display_text = $"Downloading Skill Icon '{tempLegend.Swap.Name}'",
										Url = tempLegend.Swap.Icon.Url,
										Path = Paths.BasePath + tempLegend.Swap.Icon.Path
									});
								}
								if (!File.Exists(Paths.BasePath + tempLegend.Heal.Icon.Path))
								{
									downloadList.Add(new APIDownload_Image
									{
										Display_text = $"Downloading Skill Icon '{tempLegend.Heal.Name}'",
										Url = tempLegend.Heal.Icon.Url,
										Path = Paths.BasePath + tempLegend.Heal.Icon.Path
									});
								}
								if (!File.Exists(Paths.BasePath + tempLegend.Heal.Icon.Path))
								{
									downloadList.Add(new APIDownload_Image
									{
										Display_text = $"Downloading Skill Icon '{tempLegend.Elite.Name}'",
										Url = tempLegend.Elite.Icon.Url,
										Path = Paths.BasePath + tempLegend.Elite.Icon.Path
									});
								}
								tempLegend.Utilities = new List<API.Skill>();
								foreach (int id in legend.Utilities)
								{
									API.Skill skill4 = Skills.Find((API.Skill e) => e.Id == id);
									tempLegend.Utilities.Add(skill4);
									if (!File.Exists(Paths.BasePath + skill4.Icon.Path))
									{
										downloadList.Add(new APIDownload_Image
										{
											Display_text = $"Downloading Skill Icon '{skill4.Name}'",
											Url = skill4.Icon.Url,
											Path = Paths.BasePath + skill4.Icon.Path
										});
									}
								}
								temp.Legends.Add(tempLegend);
							}
							foreach (ProfessionSkill iSkill in profession.get_Skills())
							{
								API.Skill skill3 = Skills.Find((API.Skill e) => e.Id == iSkill.get_Id());
								Data.SkillID_Pair paletteID = SkillID_Pairs.Find((Data.SkillID_Pair e) => e.ID == iSkill.get_Id());
								if (skill3 != null && paletteID != null)
								{
									skill3.PaletteId = paletteID.PaletteID;
									temp.Skills.Add(skill3);
									if (!File.Exists(Paths.BasePath + skill3.Icon.Path))
									{
										downloadList.Add(new APIDownload_Image
										{
											Display_text = $"Downloading Skill Icon '{skill3.Name}'",
											Url = skill3.Icon.Url,
											Path = Paths.BasePath + skill3.Icon.Path
										});
									}
								}
							}
							foreach (KeyValuePair<int, int> skillIDs2 in profession.get_SkillsByPalette())
							{
								API.Skill skill2 = Skills.Find((API.Skill e) => e.Id == skillIDs2.Value);
								if (skill2 != null && !temp.Skills.Contains(skill2))
								{
									skill2.PaletteId = skillIDs2.Key;
									temp.Skills.Add(skill2);
									if (!File.Exists(Paths.BasePath + skill2.Icon.Path))
									{
										downloadList.Add(new APIDownload_Image
										{
											Display_text = $"Downloading Skill Icon '{skill2.Name}'",
											Url = skill2.Icon.Url,
											Path = Paths.BasePath + skill2.Icon.Path
										});
									}
								}
							}
						}
						else
						{
							foreach (KeyValuePair<int, int> skillIDs in profession.get_SkillsByPalette())
							{
								API.Skill skill = Skills.Find((API.Skill e) => e.Id == skillIDs.Value);
								if (skill != null)
								{
									skill.PaletteId = skillIDs.Key;
									temp.Skills.Add(skill);
									if (!File.Exists(Paths.BasePath + skill.Icon.Path))
									{
										downloadList.Add(new APIDownload_Image
										{
											Display_text = $"Downloading Skill Icon '{skill.Name}'",
											Url = skill.Icon.Url,
											Path = Paths.BasePath + skill.Icon.Path
										});
									}
								}
							}
						}
						if (!File.Exists(Paths.BasePath + temp.Icon.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								Display_text = $"Downloading Profession Icon '{temp.Name}'",
								Url = temp.Icon.Url,
								Path = Paths.BasePath + temp.Icon.Path
							});
						}
						if (!File.Exists(Paths.BasePath + temp.IconBig.Path))
						{
							downloadList.Add(new APIDownload_Image
							{
								Display_text = $"Downloading Profession Icon '{temp.Name}'",
								Url = temp.IconBig.Url,
								Path = Paths.BasePath + temp.IconBig.Path
							});
						}
						Professions.Add(temp);
					}
					Logger.Debug("Saving Professions ....");
					File.WriteAllText(Paths.professions + "professions [" + culture + "].json", JsonConvert.SerializeObject((object)Professions.ToArray(), settings));
					DownloadBar.Progress = 0.0;
					total2 = downloadList.Count;
					completed2 = 0.0;
					Logger.Debug("All required Images queued. Downloading now ....");
					foreach (APIDownload_Image image in downloadList)
					{
						Logger.Debug("Downloading: '{0}' from url '{1}' to '{2}'", new object[3] { image.Display_text, image.Url, image.Path });
						FileStream stream = new FileStream(image.Path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
						await Gw2ApiManager.get_Gw2ApiClient().get_Render().DownloadToStreamAsync((Stream)stream, image.Url, default(CancellationToken));
						stream.Close();
						completed2 += 1.0;
						progress = completed2 / total2;
						DownloadBar.Progress = progress;
						DownloadBar.Text = string.Format("{0} / {1} ({2})", completed2, downloadList.Count, Math.Round(progress * 100.0, 2) + "%");
						((Control)DownloadBar).set_BasicTooltipText(image.Display_text);
						((Control)DownloadBar._Label).set_BasicTooltipText(image.Display_text);
						((Control)DownloadBar._BackgroundTexture).set_BasicTooltipText(image.Display_text);
						((Control)DownloadBar._FilledTexture).set_BasicTooltipText(image.Display_text);
					}
					((Control)LoadingSpinner).set_Visible(false);
					((Control)DownloadBar).set_Visible(false);
					GameVersion.set_Value(GameService.Gw2Mumble.get_Info().get_Version());
					ModuleVersion.set_Value(((object)((Module)this).get_Version().BaseVersion()).ToString());
					Logger.Debug("API Data sucessfully fetched!");
					FetchingAPI = false;
					return true;
				}
				catch (Exception)
				{
					Logger.Debug("Fetching API Data failed!");
					((Control)LoadingSpinner).set_Visible(false);
					((Control)DownloadBar).set_Visible(false);
					return false;
				}
			}
			return false;
		}

		private async Task LoadData()
		{
			string culture = getCultureString();
			if (await Fetch_APIData(!File.Exists(Paths.professions + "professions [" + culture + "].json")))
			{
				if (Data == null)
				{
					Data = new Data();
				}
				else
				{
					Data.UpdateLanguage();
				}
				GameService.Overlay.get_UserLocale().add_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
			}
			else
			{
				((Module)this).Unload();
			}
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
			Window_MainWindow window_MainWindow = new Window_MainWindow(TextureManager.getBackground(Backgrounds.MainWindow), new Rectangle(30, 30, Width, Height + 30), new Rectangle(30, 15, Width - 3, Height + 25));
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
