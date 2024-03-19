using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using DanceDanceRotationModule.Model;
using DanceDanceRotationModule.Storage;
using DanceDanceRotationModule.Util;
using DanceDanceRotationModule.Views;
using Microsoft.Xna.Framework;

namespace DanceDanceRotationModule
{
	[Export(typeof(Module))]
	public class DanceDanceRotationModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<DanceDanceRotationModule>();

		internal static DanceDanceRotationModule Instance;

		internal static ModuleSettings Settings;

		private CornerIcon _cornerIcon;

		private ContextMenuStrip _cornerContextMenu;

		private NotesWindow _notesWindow;

		private SongListWindow _songListWindow;

		private StandardWindow _songInfoWindow;

		private StandardWindow _helpWindow;

		private NotesView _notesView;

		private SongListView _songListView;

		private SongInfoView _songInfoView;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal static SongRepo SongRepo { get; private set; }

		[ImportingConstructor]
		public DanceDanceRotationModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			Settings = new ModuleSettings(settings);
		}

		protected override void Initialize()
		{
		}

		protected override async Task LoadAsync()
		{
			Resources.Instance = new Resources();
			Resources.Instance.LoadResources(ContentsManager);
			SongRepo = new SongRepo();
			SongRepo.StartDirectoryWatcher();
			await SongRepo.Load();
			Resources.Instance.SetUpCurrentSongListener(SongRepo);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			LoadWindows();
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(Resources.Instance.DdrLogoEmblemTexture));
			((Control)val).set_BasicTooltipText("Dance Dance Rotation");
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Logger.Trace("CornerIcon Clicked");
				ToggleNotesWindow();
			});
			_cornerContextMenu = new ContextMenuStrip();
			((Control)_cornerContextMenu.AddMenuItem("Help")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Logger.Trace("Help Clicked");
				UrlHelper.OpenUrl("https://github.com/campbt/DanceDanceRotation#setup-and-usage");
			});
			((Control)_cornerContextMenu.AddMenuItem("Report Issue")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Logger.Trace("ReportIssue Clicked");
				UrlHelper.OpenUrl("https://github.com/campbt/DanceDanceRotation/issues");
			});
			((Control)_cornerIcon).set_Menu(_cornerContextMenu);
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			_notesView?.Update(gameTime);
		}

		protected override void Unload()
		{
			Resources.Instance.Unload();
			Resources.Instance = null;
			NotesWindow notesWindow = _notesWindow;
			if (notesWindow != null)
			{
				((Control)notesWindow).Dispose();
			}
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			SongListWindow songListWindow = _songListWindow;
			if (songListWindow != null)
			{
				((Control)songListWindow).Dispose();
			}
			StandardWindow songInfoWindow = _songInfoWindow;
			if (songInfoWindow != null)
			{
				((Control)songInfoWindow).Dispose();
			}
			StandardWindow helpWindow = _helpWindow;
			if (helpWindow != null)
			{
				((Control)helpWindow).Dispose();
			}
			SongRepo?.Dispose();
			Instance = null;
			Settings.Dispose();
			Settings = null;
			SongRepo = null;
		}

		private void LoadWindows()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			NotesWindow notesWindow = new NotesWindow(Settings.Orientation.get_Value());
			((Control)notesWindow).set_Location(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 - 400, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 400 - 180));
			_notesWindow = notesWindow;
			SongListWindow songListWindow = new SongListWindow();
			((Control)songListWindow).set_Location(new Point(((Control)_notesWindow).get_Left(), ((Control)_notesWindow).get_Top() - 400 - 20));
			_songListWindow = songListWindow;
			SongInfoWindow songInfoWindow = new SongInfoWindow();
			((Control)songInfoWindow).set_Location(new Point(((Control)_notesWindow).get_Left() - 390, ((Control)_songListWindow).get_Top()));
			_songInfoWindow = (StandardWindow)(object)songInfoWindow;
			_notesView = new NotesView();
			_songListView = new SongListView();
			_songInfoView = new SongInfoView();
			if (!Settings.HasShownHelpWindow.get_Value())
			{
				Logger.Info("Showing Help Window");
				Settings.HasShownHelpWindow.set_Value(true);
				HelpWindow helpWindow = new HelpWindow();
				((Control)helpWindow).set_Location(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 - 415, Math.Max(((Control)_songListWindow).get_Top() - 360, 100)));
				_helpWindow = (StandardWindow)(object)helpWindow;
				_helpWindow.Show((IView)(object)new HelpView());
				((WindowBase2)_helpWindow).set_CanResize(false);
			}
			Settings.Orientation.add_SettingChanged((EventHandler<ValueChangedEventArgs<NotesOrientation>>)delegate(object sender, ValueChangedEventArgs<NotesOrientation> args)
			{
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				bool visible = ((Control)_notesWindow).get_Visible();
				((Control)_notesWindow).Dispose();
				NotesWindow notesWindow2 = new NotesWindow(args.get_NewValue());
				((Control)notesWindow2).set_Location(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 - 400, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 400 - 180));
				_notesWindow = notesWindow2;
				if (visible)
				{
					_notesWindow.Show((IView)(object)_notesView);
				}
			});
		}

		public NotesContainer GetNotesContainer()
		{
			return _notesView?.GetNotesContainer();
		}

		public bool IsNotesWindowVisible()
		{
			return ((Control)_notesWindow).get_Visible();
		}

		public void ToggleNotesWindow()
		{
			Logger.Trace("ToggleNotesWindow");
			_notesWindow.ToggleWindow((IView)(object)_notesView);
		}

		public void ToggleSongList()
		{
			Logger.Trace("ToggleSongList");
			((StandardWindow)_songListWindow).ToggleWindow((IView)(object)_songListView);
		}

		public void ToggleSongInfo()
		{
			Logger.Trace("ToggleSongInfo");
			_songInfoWindow.ToggleWindow((IView)(object)_songInfoView);
		}

		public void ShowNotesWindow()
		{
			Logger.Trace("Showing Notes Window");
			_notesWindow.Show((IView)(object)_notesView);
		}

		public void ShowSongList()
		{
			Logger.Trace("Showing Song List Window");
			((StandardWindow)_songListWindow).Show((IView)(object)_songListView);
		}

		public void ShowSongInfo()
		{
			Logger.Trace("Showing Song Info Window");
			_songInfoWindow.Show((IView)(object)_songInfoView);
		}
	}
}
