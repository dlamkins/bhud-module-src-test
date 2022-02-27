using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using lk0001.CurrentTemplates.Controls;
using lk0001.CurrentTemplates.Utility;
using lk0001.CurrentTemplates.Views;

namespace lk0001.CurrentTemplates
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		public static readonly Logger Logger = Logger.GetLogger<Module>();

		public static string[] _fontSizes = new string[12]
		{
			"8", "11", "12", "14", "16", "18", "20", "22", "24", "32",
			"34", "36"
		};

		public static string[] _fontAlign = new string[3] { "Left", "Center", "Right" };

		public static SettingEntry<string> _settingFontSize;

		public static SettingEntry<string> _settingAlign;

		public static SettingEntry<Point> _settingLoc;

		public static SettingEntry<bool> _settingDrag;

		public static SettingEntry<bool> _settingBuildPad;

		public static SettingEntry<string> _settingBuildPadPath;

		public static bool _hasBuildPad = false;

		private static readonly double INTERVAL_CHECKTEMPLATES = 30010.0;

		private static readonly List<string> GW2_PATHS = new List<string> { "c:/Program Files/Guild Wars 2", "c:/Program Files (x86)/Guild Wars 2", "d:/Guild Wars 2", ".", ".." };

		private static readonly string BUILD_PAD_PATH = "addons/arcdps/arcdps.buildpad/config.ini";

		private double _lastTemplateCheck = -1.0;

		private DrawTemplates templatesControl;

		private Character character;

		private string characterName = "";

		private BuildPad buildPad;

		private SettingsView settingsView;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			_settingFontSize = settings.DefineSetting<string>("CurrentTemplatesFont", "16", "Font Size", "", (SettingTypeRendererDelegate)null);
			_settingAlign = settings.DefineSetting<string>("CurrentTemplatesAlign", "Left", "Align", "", (SettingTypeRendererDelegate)null);
			_settingLoc = settings.DefineSetting<Point>("CurrentTemplatesLoc", new Point(1, 30), "Location", "", (SettingTypeRendererDelegate)null);
			_settingDrag = settings.DefineSetting<bool>("CurrentTemplatesDrag", false, "Enable Dragging", "", (SettingTypeRendererDelegate)null);
			_settingBuildPad = settings.DefineSetting<bool>("CurrentTemplatesBP", false, "BuildPad integration", "Displays build template name based on your builds saved in BuildPad (requires setting the path below)", (SettingTypeRendererDelegate)null);
			_settingBuildPadPath = settings.DefineSetting<string>("CurrentTemplatesBPPath", "", "Path to BuildPad config", "For example " + "C:\\Program Files\\Guild Wars 2\\addons\\arcdps\\arcdps.buildpad\\config.ini".Replace(" ", "\u00a0"), (SettingTypeRendererDelegate)null);
			_settingFontSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateCurrentTemplatesSettings_Font);
			_settingAlign.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateCurrentTemplatesSettings_Font);
			_settingLoc.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateCurrentTemplatesSettings_Location);
			_settingDrag.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateCurrentTemplatesSettings_Show);
			_settingBuildPad.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateCurrentTemplatesSettings_BuildPadShow);
			_settingBuildPadPath.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateCurrentTemplatesSettings_BuildPadConfigPath);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)settingsView;
		}

		protected override void Initialize()
		{
			settingsView = new SettingsView();
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			_lastTemplateCheck = INTERVAL_CHECKTEMPLATES;
		}

		protected override async Task LoadAsync()
		{
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			templatesControl = new DrawTemplates();
			((Control)templatesControl).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			UpdateCurrentTemplatesSettings_Font();
			UpdateCurrentTemplatesSettings_Location();
			UpdateCurrentTemplatesSettings_Show();
			UpdateCurrentTemplatesSettings_BuildPadShow();
			UpdateCurrentTemplatesSettings_BuildPadConfigPath();
			DetectBuildPadPath();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				((Control)templatesControl).Show();
			}
			else
			{
				((Control)templatesControl).Hide();
			}
			if (characterName != GameService.Gw2Mumble.get_PlayerCharacter().get_Name())
			{
				characterName = GameService.Gw2Mumble.get_PlayerCharacter().get_Name();
				Logger.Debug("Changing character to '{0}'", new object[1] { characterName });
				ResetTemplates();
				_lastTemplateCheck = INTERVAL_CHECKTEMPLATES;
			}
			UpdateCadenceUtil.UpdateAsyncWithCadence(CheckTemplates, gameTime, INTERVAL_CHECKTEMPLATES, ref _lastTemplateCheck);
		}

		protected override void Unload()
		{
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			_settingFontSize.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateCurrentTemplatesSettings_Font);
			_settingAlign.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateCurrentTemplatesSettings_Font);
			_settingLoc.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateCurrentTemplatesSettings_Location);
			_settingDrag.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateCurrentTemplatesSettings_Show);
			_settingBuildPad.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateCurrentTemplatesSettings_BuildPadShow);
			_settingBuildPadPath.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateCurrentTemplatesSettings_BuildPadConfigPath);
			DrawTemplates drawTemplates = templatesControl;
			if (drawTemplates != null)
			{
				((Control)drawTemplates).Dispose();
			}
		}

		private void InitBuildPad()
		{
			if (BuildPad.ValidPath(_settingBuildPadPath.get_Value()))
			{
				buildPad = new BuildPad(_settingBuildPadPath.get_Value());
				_hasBuildPad = true;
			}
			else
			{
				buildPad = null;
				_hasBuildPad = false;
			}
			settingsView.ToggleIncorrectPathWarning();
		}

		private void DetectBuildPadPath()
		{
			if (_settingBuildPadPath.get_Value() != "")
			{
				return;
			}
			GW2_PATHS.ForEach(delegate(string path)
			{
				string path2 = Path.Combine(new string[2] { path, BUILD_PAD_PATH });
				if (File.Exists(path2))
				{
					_settingBuildPadPath.set_Value(Path.GetFullPath(path2));
				}
			});
		}

		private async Task CheckTemplates(GameTime gameTime)
		{
			if (_lastTemplateCheck < 0.0)
			{
				Logger.Debug("No token yet, skipping.");
				return;
			}
			try
			{
				Gw2ApiManager gw2ApiManager = Gw2ApiManager;
				TokenPermission[] array = new TokenPermission[4];
				RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
				if (gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)array))
				{
					Logger.Debug("Getting character '{0}' from the API.", new object[1] { characterName });
					templatesControl.ShowSpinner();
					character = await ((IBulkExpandableClient<Character, string>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).GetAsync(characterName, default(CancellationToken));
					templatesControl.HideSpinner();
					UpdateTemplates();
					Logger.Debug("Loaded '{0}' and '{1}' from the API.", new object[2] { templatesControl.buildName, templatesControl.equipmentName });
				}
				else
				{
					Logger.Debug("Skipping API call - API key does not give us permissions.");
				}
			}
			catch (Exception ex)
			{
				Logger.Debug(ex, "Failed to load character '{0}'.", new object[1] { characterName });
			}
		}

		private void ResetTemplates()
		{
			templatesControl.buildName = "";
			templatesControl.equipmentName = "";
		}

		private void UpdateTemplates()
		{
			if (character != null)
			{
				templatesControl.buildName = TemplateName(character.get_ActiveBuildTab(), character.get_BuildTabs());
				templatesControl.equipmentName = TemplateName(character.get_ActiveEquipmentTab(), character.get_EquipmentTabs());
			}
		}

		private string TemplateName<T>(int? activeTab, IReadOnlyList<T> tabs)
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			if (!activeTab.HasValue)
			{
				return "Missing!";
			}
			T tab = tabs[activeTab.Value - 1];
			string tabName = "";
			string suffix = "";
			if (typeof(T) == typeof(CharacterBuildTabSlot))
			{
				string buildPadName = ((_settingBuildPad.get_Value() && buildPad != null) ? buildPad.GetName(((CharacterBuildTabSlot)(object)tab).get_Build()) : null);
				if (buildPadName != null)
				{
					tabName = buildPadName;
					suffix = " [BP]";
				}
				else
				{
					tabName = ((CharacterBuildTabSlot)(object)tab).get_Build().get_Name();
					suffix = (_settingBuildPad.get_Value() ? " [Not in BP]" : "");
				}
			}
			else if (typeof(T) == typeof(CharacterEquipmentTabSlot))
			{
				tabName = ((CharacterEquipmentTabSlot)(object)tab).get_Name();
			}
			if (tabName == "")
			{
				tabName = "Tab " + activeTab;
			}
			return tabName + suffix;
		}

		private void UpdateCurrentTemplatesSettings_Show(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			templatesControl.Drag = _settingDrag.get_Value();
		}

		private void UpdateCurrentTemplatesSettings_Font(object sender = null, ValueChangedEventArgs<string> e = null)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			templatesControl.Font_Size = (FontSize)Enum.Parse(typeof(FontSize), "Size" + _settingFontSize.get_Value());
			templatesControl.Align = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), _settingAlign.get_Value());
		}

		private void UpdateCurrentTemplatesSettings_Location(object sender = null, ValueChangedEventArgs<Point> e = null)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			((Control)templatesControl).set_Location(_settingLoc.get_Value());
		}

		private void UpdateCurrentTemplatesSettings_BuildPadShow(object sender = null, ValueChangedEventArgs<bool> e = null)
		{
			if (!templatesControl.BuildPad && _settingBuildPad.get_Value())
			{
				DetectBuildPadPath();
				InitBuildPad();
			}
			templatesControl.BuildPad = _settingBuildPad.get_Value();
			UpdateTemplates();
		}

		private void UpdateCurrentTemplatesSettings_BuildPadConfigPath(object sender = null, ValueChangedEventArgs<string> e = null)
		{
			if (templatesControl.BuildPadConfigPath != _settingBuildPadPath.get_Value())
			{
				templatesControl.BuildPadConfigPath = _settingBuildPadPath.get_Value();
				InitBuildPad();
				UpdateTemplates();
			}
		}
	}
}
