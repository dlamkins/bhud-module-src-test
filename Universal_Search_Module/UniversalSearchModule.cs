using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Universal_Search_Module.Controls;
using Universal_Search_Module.Services.SearchHandler;
using Universal_Search_Module.Strings;

namespace Universal_Search_Module
{
	[Export(typeof(Module))]
	public class UniversalSearchModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(UniversalSearchModule));

		private readonly List<SearchHandler> _searchHandlers = new List<SearchHandler>();

		private SearchWindow _searchWindow;

		private CornerIcon _searchIcon;

		internal static UniversalSearchModule ModuleInstance;

		internal SettingEntry<bool> SettingShowNotificationWhenLandmarkIsCopied;

		internal SettingEntry<bool> SettingHideWindowAfterSelection;

		internal SettingEntry<bool> SettingEnterSelectionIntoChatAutomatically;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public UniversalSearchModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settingsManager)
		{
			SettingShowNotificationWhenLandmarkIsCopied = settingsManager.DefineSetting<bool>("ShowNotificationOnCopy", true, (Func<string>)(() => Common.Settings_NotificationAfterCopy_Title), (Func<string>)(() => Common.Settings_NotificationAfterCopy_Text));
			SettingHideWindowAfterSelection = settingsManager.DefineSetting<bool>("HideWindowOnSelection", true, (Func<string>)(() => Common.Settings_HideWindowAfterSelection_Title), (Func<string>)(() => Common.Settings_HideWindowAfterSelection_Text));
			SettingEnterSelectionIntoChatAutomatically = settingsManager.DefineSetting<bool>("EnterSelectionIntoChat", false, (Func<string>)(() => Common.Settings_EnterSelectionIntoChat_Title), (Func<string>)(() => Common.Settings_EnterSelectionIntoChat_Text));
		}

		protected override void Initialize()
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			_searchHandlers.AddRange(new SearchHandler[3]
			{
				new LandmarkSearchHandler(Gw2ApiManager),
				new SkillSearchHandler(Gw2ApiManager),
				new TraitSearchHandler(Gw2ApiManager)
			});
			SearchWindow searchWindow = new SearchWindow(_searchHandlers);
			((Control)searchWindow).set_Location(((Control)GameService.Graphics.get_SpriteScreen()).get_Size() / new Point(2) - new Point(256, 178) / new Point(2));
			((Control)searchWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)searchWindow).set_Id("SearchWindow_UniversalSearchModule_090afc97-559c-4f1d-8196-0b77f5d0a9c9");
			((WindowBase2)searchWindow).set_SavesPosition(true);
			_searchWindow = searchWindow;
		}

		protected override async Task LoadAsync()
		{
			UniversalSearchModule universalSearchModule = this;
			CornerIcon val = new CornerIcon();
			val.set_IconName(Common.Icon_Title);
			val.set_Icon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("textures\\landmark-search.png")));
			val.set_HoverIcon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("textures\\landmark-search-hover.png")));
			val.set_Priority(5);
			universalSearchModule._searchIcon = val;
			foreach (SearchHandler searchHandler in _searchHandlers)
			{
				await searchHandler.Initialize(delegate(string progress)
				{
					_searchIcon.set_LoadingMessage(progress);
				});
			}
			_searchIcon.set_LoadingMessage((string)null);
			((Control)_searchIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((WindowBase2)_searchWindow).ToggleWindow();
			});
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			((Control)_searchWindow).Dispose();
			((Control)_searchIcon).Dispose();
			ModuleInstance = null;
		}
	}
}
