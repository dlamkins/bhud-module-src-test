using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Models.Settings;
using Estreya.BlishHUD.EventTable.Resources;
using Estreya.BlishHUD.EventTable.State;
using Estreya.BlishHUD.EventTable.UI.Container;
using Estreya.BlishHUD.EventTable.UI.Views;
using Estreya.BlishHUD.EventTable.UI.Views.Settings;
using Estreya.BlishHUD.EventTable.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable
{
	[Export(typeof(Module))]
	public class EventTableModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<EventTableModule>();

		internal static EventTableModule ModuleInstance;

		private BitmapFont _font;

		private TimeSpan _eventTimeSpan = TimeSpan.Zero;

		private SemaphoreSlim _eventCategorySemaphore = new SemaphoreSlim(1, 1);

		private List<EventCategory> _eventCategories = new List<EventCategory>();

		private readonly AsyncLock _stateLock = new AsyncLock();

		public bool IsPrerelease => !string.IsNullOrWhiteSpace(((Module)this).get_Version()?.PreRelease);

		private EventTableContainer Container { get; set; }

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		internal ModuleSettings ModuleSettings { get; private set; }

		private CornerIcon CornerIcon { get; set; }

		internal TabbedWindow2 SettingsWindow { get; private set; }

		internal bool Debug => ModuleSettings.DebugEnabled.get_Value();

		internal BitmapFont Font
		{
			get
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				if (_font == null)
				{
					_font = GameService.Content.GetFont((FontFace)0, ModuleSettings.EventFontSize.get_Value(), (FontStyle)0);
				}
				return _font;
			}
		}

		internal int EventHeight => ModuleSettings?.EventHeight?.get_Value() ?? 30;

		internal DateTime DateTimeNow => DateTime.Now;

		internal TimeSpan EventTimeSpan
		{
			get
			{
				if (_eventTimeSpan == TimeSpan.Zero)
				{
					if (double.TryParse(ModuleSettings.EventTimeSpan.get_Value(), out var timespan))
					{
						if (timespan > 1440.0)
						{
							timespan = 1440.0;
							Logger.Warn("Event Timespan over 1440. Cap at 1440 for performance reasons.");
						}
						_eventTimeSpan = TimeSpan.FromMinutes(timespan);
					}
					else
					{
						Logger.Error("Event Timespan '" + ModuleSettings.EventTimeSpan.get_Value() + "' no real number, default to 120");
						_eventTimeSpan = TimeSpan.FromMinutes(120.0);
					}
				}
				return _eventTimeSpan;
			}
		}

		internal float EventTimeSpanRatio => 0.5f + ((float)ModuleSettings.EventHistorySplit.get_Value() / 100f - 0.5f);

		internal DateTime EventTimeMin
		{
			get
			{
				TimeSpan timespan = TimeSpan.FromMilliseconds(EventTimeSpan.TotalMilliseconds * (double)EventTimeSpanRatio);
				return ModuleInstance.DateTimeNow.Subtract(timespan);
			}
		}

		internal DateTime EventTimeMax
		{
			get
			{
				TimeSpan timespan = TimeSpan.FromMilliseconds(EventTimeSpan.TotalMilliseconds * (double)(1f - EventTimeSpanRatio));
				return ModuleInstance.DateTimeNow.Add(timespan);
			}
		}

		public List<EventCategory> EventCategories => _eventCategories.Where((EventCategory ec) => !ec.IsDisabled()).ToList();

		private Collection<ManagedState> States { get; set; } = new Collection<ManagedState>();


		public HiddenState HiddenState { get; private set; }

		public WorldbossState WorldbossState { get; private set; }

		public MapchestState MapchestState { get; private set; }

		public EventFileState EventFileState { get; private set; }

		public IconState IconState { get; private set; }

		[ImportingConstructor]
		public EventTableModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			ModuleSettings = new ModuleSettings(settings);
		}

		protected override void Initialize()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			EventTableContainer eventTableContainer = new EventTableContainer();
			((Control)eventTableContainer).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)eventTableContainer).set_BackgroundColor(Color.get_Transparent());
			((Control)eventTableContainer).set_Opacity(0f);
			eventTableContainer.Visible = false;
			Container = eventTableContainer;
		}

		protected override async Task LoadAsync()
		{
			Logger.Debug("Load module settings.");
			await ModuleSettings.LoadAsync();
			Logger.Debug("Initialize states (before event file loading)");
			await InitializeStates(beforeFileLoaded: true);
			Logger.Debug("Load events.");
			await LoadEvents();
			lock (_eventCategories)
			{
				ModuleSettings.InitializeEventSettings(_eventCategories);
			}
			Logger.Debug("Initialize states (after event file loading)");
			await InitializeStates();
			await Container.LoadAsync();
			ModuleSettings.ModuleSettingsChanged += delegate(object sender, ModuleSettings.ModuleSettingsChangedEventArgs eventArgs)
			{
				switch (eventArgs.Name)
				{
				case "Width":
					Container.UpdateSize(ModuleSettings.Width.get_Value(), -1);
					break;
				case "GlobalEnabled":
					ToggleContainer(ModuleSettings.GlobalEnabled.get_Value());
					break;
				case "EventTimeSpan":
					_eventTimeSpan = TimeSpan.Zero;
					break;
				case "EventFontSize":
					_font = null;
					break;
				case "RegisterCornerIcon":
					HandleCornerIcon(ModuleSettings.RegisterCornerIcon.get_Value());
					break;
				case "BackgroundColor":
				case "BackgroundColorOpacity":
					Container.UpdateBackgroundColor();
					break;
				}
			};
		}

		public async Task LoadEvents()
		{
			string threadName = $"{Thread.CurrentThread.ManagedThreadId}";
			Logger.Debug("Try loading events from thread: {0}", new object[1] { threadName });
			await _eventCategorySemaphore.WaitAsync();
			Logger.Debug("Thread \"{0}\" started loading", new object[1] { threadName });
			try
			{
				if (_eventCategories != null)
				{
					lock (_eventCategories)
					{
						foreach (EventCategory eventCategory in _eventCategories)
						{
							eventCategory.Unload();
						}
						_eventCategories.Clear();
					}
				}
				EventSettingsFile eventSettingsFile = await EventFileState.GetExternalFile();
				if (eventSettingsFile == null)
				{
					Logger.Error("Failed to load event file.");
					return;
				}
				Logger.Info($"Loaded event file version: {eventSettingsFile.Version}");
				List<EventCategory> categories = eventSettingsFile.EventCategories ?? new List<EventCategory>();
				int eventCategoryCount = categories.Count;
				int eventCount = categories.Sum((EventCategory ec) => ec.Events.Count);
				Logger.Info($"Loaded {eventCategoryCount} Categories with {eventCount} Events.");
				await Task.WhenAll(categories.Select((EventCategory ec) => ec.LoadAsync()));
				lock (_eventCategories)
				{
					Logger.Debug("Overwrite current categories with newly loaded.");
					_eventCategories = categories;
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed loading events.");
				throw ex;
			}
			finally
			{
				_eventCategorySemaphore.Release();
				Logger.Debug("Thread \"{0}\" released loading lock", new object[1] { threadName });
			}
		}

		private async Task InitializeStates(bool beforeFileLoaded = false)
		{
			string eventsDirectory = DirectoriesManager.GetFullDirectoryPath("events");
			Action<string> hideEventAction = delegate(string apiCode)
			{
				if (ModuleSettings.EventCompletedAcion.get_Value() == EventCompletedAction.Hide)
				{
					lock (_eventCategories)
					{
						(from ev in _eventCategories.SelectMany((EventCategory ec) => ec.Events)
							where ev.APICode == apiCode
							select ev).ToList().ForEach(delegate(Event ev)
						{
							ev.Finish();
						});
					}
				}
			};
			if (!beforeFileLoaded)
			{
				HiddenState = new HiddenState(eventsDirectory);
				WorldbossState = new WorldbossState(Gw2ApiManager);
				WorldbossState.WorldbossCompleted += delegate(object s, string e)
				{
					hideEventAction(e);
				};
				MapchestState = new MapchestState(Gw2ApiManager);
				MapchestState.MapchestCompleted += delegate(object s, string e)
				{
					hideEventAction(e);
				};
			}
			else
			{
				EventFileState = new EventFileState(ContentsManager, eventsDirectory, "events.json");
				IconState = new IconState(ContentsManager, eventsDirectory);
			}
			lock (States)
			{
				if (!beforeFileLoaded)
				{
					States.Add(HiddenState);
					States.Add(WorldbossState);
					States.Add(MapchestState);
				}
				else
				{
					States.Add(EventFileState);
					States.Add(IconState);
				}
			}
			using (await _stateLock.LockAsync())
			{
				foreach (ManagedState state in States)
				{
					Logger.Debug("Starting managed state: {0}", new object[1] { state.GetType().Name });
					try
					{
						await state.Start();
					}
					catch (Exception ex)
					{
						Logger.Error(ex, "Failed starting state \"{0}\"", new object[1] { state.GetType().Name });
					}
				}
			}
		}

		private void HandleCornerIcon(bool show)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			if (show)
			{
				CornerIcon val = new CornerIcon();
				val.set_IconName("Event Table");
				val.set_Icon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("images\\event_boss_grey.png")));
				CornerIcon = val;
				((Control)CornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					((WindowBase2)SettingsWindow).ToggleWindow();
				});
			}
			else if (CornerIcon != null)
			{
				((Control)CornerIcon).Dispose();
				CornerIcon = null;
			}
		}

		private void ToggleContainer(bool show)
		{
			if (Container == null)
			{
				return;
			}
			if (!ModuleSettings.GlobalEnabled.get_Value())
			{
				if (Container.Visible)
				{
					Container.Hide();
				}
			}
			else if (show)
			{
				if (!Container.Visible)
				{
					Container.Show();
				}
			}
			else if (Container.Visible)
			{
				Container.Hide();
			}
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new ModuleSettingsView();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Expected O, but got Unknown
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Expected O, but got Unknown
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Expected O, but got Unknown
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Expected O, but got Unknown
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Expected O, but got Unknown
			((Module)this).OnModuleLoaded(e);
			Container.UpdatePosition(ModuleSettings.LocationX.get_Value(), ModuleSettings.LocationY.get_Value());
			Container.UpdateSize(ModuleSettings.Width.get_Value(), -1);
			Logger.Debug("Start building settings window.");
			Texture2D windowBackground = IconState.GetIcon("images\\502049.png", checkRenderAPI: false);
			Rectangle settingsWindowSize = default(Rectangle);
			((Rectangle)(ref settingsWindowSize))._002Ector(35, 26, 1100, 714);
			int contentRegionPaddingY = settingsWindowSize.Y - 15;
			int contentRegionPaddingX = settingsWindowSize.X + 46;
			Rectangle contentRegion = default(Rectangle);
			((Rectangle)(ref contentRegion))._002Ector(contentRegionPaddingX, contentRegionPaddingY, settingsWindowSize.Width - 52, settingsWindowSize.Height - contentRegionPaddingY);
			TabbedWindow2 val = new TabbedWindow2(windowBackground, settingsWindowSize, contentRegion);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title(Strings.SettingsWindow_Title);
			((WindowBase2)val).set_Emblem(IconState.GetIcon("images\\event_boss.png"));
			((WindowBase2)val).set_Subtitle(Strings.SettingsWindow_Subtitle);
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id("EventTableModule_6bd04be4-dc19-4914-a2c3-8160ce76818b");
			SettingsWindow = val;
			SettingsWindow.get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(IconState.GetIcon("images\\event_boss_grey.png")), (Func<IView>)(() => (IView)(object)new ManageEventsView()), Strings.SettingsWindow_ManageEvents_Title, (int?)null));
			SettingsWindow.get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(IconState.GetIcon("images\\bars.png")), (Func<IView>)(() => (IView)(object)new ReorderEventsView()), Strings.SettingsWindow_ReorderSettings_Title, (int?)null));
			SettingsWindow.get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(IconState.GetIcon("156736")), (Func<IView>)(() => (IView)(object)new GeneralSettingsView(ModuleSettings)), Strings.SettingsWindow_GeneralSettings_Title, (int?)null));
			SettingsWindow.get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(IconState.GetIcon("images\\graphics_settings.png")), (Func<IView>)(() => (IView)(object)new GraphicsSettingsView(ModuleSettings)), Strings.SettingsWindow_GraphicSettings_Title, (int?)null));
			SettingsWindow.get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(IconState.GetIcon("155052")), (Func<IView>)(() => (IView)(object)new EventSettingsView(ModuleSettings)), Strings.SettingsWindow_EventSettings_Title, (int?)null));
			Logger.Debug("Finished building settings window.");
			HandleCornerIcon(ModuleSettings.RegisterCornerIcon.get_Value());
			if (ModuleSettings.GlobalEnabled.get_Value())
			{
				ToggleContainer(show: true);
			}
		}

		protected override void Update(GameTime gameTime)
		{
			CheckMumble();
			Container.UpdatePosition(ModuleSettings.LocationX.get_Value(), ModuleSettings.LocationY.get_Value());
			CheckContainerSizeAndPosition();
			using (_stateLock.Lock())
			{
				foreach (ManagedState state in States)
				{
					state.Update(gameTime);
				}
			}
			lock (_eventCategories)
			{
				_eventCategories.ForEach(delegate(EventCategory ec)
				{
					ec.Update(gameTime);
				});
			}
		}

		private void CheckContainerSizeAndPosition()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			bool buildFromBottom = ModuleSettings.BuildDirection.get_Value() == BuildDirection.Bottom;
			int num = (int)((float)GameService.Graphics.get_Resolution().X / GameService.Graphics.get_UIScaleMultiplier());
			int maxResY = (int)((float)GameService.Graphics.get_Resolution().Y / GameService.Graphics.get_UIScaleMultiplier());
			int minLocationX = 0;
			int maxLocationX = num - ((Control)Container).get_Width();
			int minLocationY = (buildFromBottom ? ((Control)Container).get_Height() : 0);
			int maxLocationY = (buildFromBottom ? maxResY : (maxResY - ((Control)Container).get_Height()));
			int minWidth = 0;
			int maxWidth = num - ModuleSettings.LocationX.get_Value();
			SettingComplianceExtensions.SetRange(ModuleSettings.LocationX, minLocationX, maxLocationX);
			SettingComplianceExtensions.SetRange(ModuleSettings.LocationY, minLocationY, maxLocationY);
			SettingComplianceExtensions.SetRange(ModuleSettings.Width, minWidth, maxWidth);
		}

		private void CheckMumble()
		{
			if (GameService.Gw2Mumble.get_IsAvailable() && Container != null)
			{
				bool show = true;
				if (ModuleSettings.HideOnOpenMap.get_Value())
				{
					show &= !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
				}
				if (ModuleSettings.HideOnMissingMumbleTicks.get_Value())
				{
					show &= GameService.Gw2Mumble.get_TimeSinceTick().TotalSeconds < 0.5;
				}
				if (ModuleSettings.HideInCombat.get_Value())
				{
					show &= !GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat();
				}
				ToggleContainer(show);
			}
		}

		protected override void Unload()
		{
			Logger.Debug("Unload module.");
			Logger.Debug("Unload base.");
			((Module)this).Unload();
			Logger.Debug("Unload event categories.");
			foreach (EventCategory eventCategory in _eventCategories)
			{
				eventCategory.Unload();
			}
			Logger.Debug("Unloaded event categories.");
			Logger.Debug("Unload event container.");
			if (Container != null)
			{
				((Control)Container).Dispose();
			}
			Logger.Debug("Unloaded event container.");
			Logger.Debug("Unload settings window.");
			if (SettingsWindow != null)
			{
				((Control)SettingsWindow).Hide();
			}
			Logger.Debug("Unloaded settings window.");
			Logger.Debug("Unload corner icon.");
			HandleCornerIcon(show: false);
			Logger.Debug("Unloaded corner icon.");
			Logger.Debug("Unloading states...");
			using (_stateLock.Lock())
			{
				Task.WaitAll((from state in States.ToList()
					select state.Unload()).ToArray());
			}
			Logger.Debug("Finished unloading states.");
		}

		internal async Task ReloadStates()
		{
			using (await _stateLock.LockAsync())
			{
				await Task.WhenAll(States.Select((ManagedState state) => state.Reload()));
			}
		}

		internal async Task ClearStates()
		{
			using (await _stateLock.LockAsync())
			{
				await Task.WhenAll(States.Select((ManagedState state) => state.Clear()));
			}
		}
	}
}
