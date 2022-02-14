using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private const double INTERVAL_CHECKTEMPLATES = 30010.0;

		private double _lastTemplateCheck = -1.0;

		public static string[] _fontSizes = new string[12]
		{
			"8", "11", "12", "14", "16", "18", "20", "22", "24", "32",
			"34", "36"
		};

		public static string[] _fontAlign = new string[3] { "Left", "Center", "Right" };

		public static SettingEntry<string> _settingFontSize;

		public static SettingEntry<string> _settingAlign;

		public static SettingEntry<bool> _settingDrag;

		public static SettingEntry<Point> _settingLoc;

		private DrawTemplates templatesControl;

		private string characterName = "";

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
			_settingFontSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateCurrentTemplatesSettings_Font);
			_settingAlign.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateCurrentTemplatesSettings_Font);
			_settingLoc.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateCurrentTemplatesSettings_Location);
			_settingDrag.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateCurrentTemplatesSettings_Show);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsView();
		}

		protected override void Initialize()
		{
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			_lastTemplateCheck = 30010.0;
		}

		protected override async Task LoadAsync()
		{
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			templatesControl = new DrawTemplates();
			((Control)templatesControl).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			UpdateCurrentTemplatesSettings_Font();
			UpdateCurrentTemplatesSettings_Location();
			UpdateCurrentTemplatesSettings_Show();
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
				_lastTemplateCheck = 30010.0;
			}
			UpdateCadenceUtil.UpdateAsyncWithCadence(CheckTemplates, gameTime, 30010.0, ref _lastTemplateCheck);
		}

		protected override void Unload()
		{
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
			_settingFontSize.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateCurrentTemplatesSettings_Font);
			_settingAlign.remove_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)UpdateCurrentTemplatesSettings_Font);
			_settingLoc.remove_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)UpdateCurrentTemplatesSettings_Location);
			_settingDrag.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)UpdateCurrentTemplatesSettings_Show);
			DrawTemplates drawTemplates = templatesControl;
			if (drawTemplates != null)
			{
				((Control)drawTemplates).Dispose();
			}
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
					Character character = await ((IBulkExpandableClient<Character, string>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).GetAsync(characterName, default(CancellationToken));
					templatesControl.HideSpinner();
					templatesControl.buildName = TemplateName(character.get_ActiveBuildTab(), character.get_BuildTabs());
					templatesControl.equipmentName = TemplateName(character.get_ActiveEquipmentTab(), character.get_EquipmentTabs());
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

		private string TemplateName<T>(int? activeTab, IReadOnlyList<T> tabs)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			if (!activeTab.HasValue)
			{
				return "Missing!";
			}
			T tab = tabs[activeTab.Value - 1];
			string tabName = "";
			if (typeof(T) == typeof(CharacterBuildTabSlot))
			{
				tabName = ((CharacterBuildTabSlot)(object)tab).get_Build().get_Name();
			}
			else if (typeof(T) == typeof(CharacterEquipmentTabSlot))
			{
				tabName = ((CharacterEquipmentTabSlot)(object)tab).get_Name();
			}
			if (tabName == "")
			{
				tabName = "Tab " + activeTab;
			}
			return tabName;
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
	}
}
