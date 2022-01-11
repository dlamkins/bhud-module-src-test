using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Extensions;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.State;
using Estreya.BlishHUD.EventTable.UI.Container;
using Estreya.BlishHUD.EventTable.UI.Views;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable
{
	[Export(typeof(Module))]
	public class EventTableModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<EventTableModule>();

		private const double INTERVAL_UPDATE_WORLDBOSSES = 300010.0;

		private TimeSpan TIME_SINCE_LAST_UPDATE_WORLDBOSSES = TimeSpan.FromMilliseconds(300010.0);

		internal static EventTableModule ModuleInstance;

		internal ModuleSettings ModuleSettings;

		private bool visibleStateFromTick = true;

		private EventTableContainer Container { get; set; }

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		private WindowTab ManageEventTab { get; set; }

		internal TabbedWindow2 SettingsWindow { get; private set; }

		private IEnumerable<EventCategory> EventCategories { get; set; }

		internal bool Debug => ModuleSettings.DebugEnabled.get_Value();

		internal DateTime DateTimeNow => DateTime.Now;

		internal Collection<ManagedState> States { get; private set; } = new Collection<ManagedState>();


		internal HiddenState HiddenState { get; private set; }

		internal List<string> CompletedWorldbosses { get; private set; } = new List<string>();


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
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)Gw2ApiManager_SubtokenUpdated);
		}

		private void Gw2ApiManager_SubtokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			Task.Run(async delegate
			{
				await UpdateCompletedWorldbosses(null);
			});
		}

		protected override async Task LoadAsync()
		{
			using (StreamReader eventsReader = new StreamReader(ContentsManager.GetFileStream("events.json")))
			{
				string json = await eventsReader.ReadToEndAsync();
				EventCategories = await Task.Run(() => JsonConvert.DeserializeObject<List<EventCategory>>(json));
			}
			ModuleSettings.InitializeEventSettings(EventCategories);
			EventTableModule eventTableModule = this;
			EventTableContainer eventTableContainer = new EventTableContainer(EventCategories, ModuleSettings);
			((Control)eventTableContainer).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)eventTableContainer).set_BackgroundColor(Color.get_Transparent());
			((Control)eventTableContainer).set_Opacity(0f);
			((Control)eventTableContainer).set_Visible(false);
			eventTableModule.Container = eventTableContainer;
			ModuleSettings.ModuleSettingsChanged += delegate(object sender, ModuleSettings.ModuleSettingsChangedEventArgs eventArgs)
			{
				switch (eventArgs.Name)
				{
				case "Width":
				case "Height":
					Container.UpdateSize(ModuleSettings.Width.get_Value(), ModuleSettings.Height.get_Value());
					break;
				case "GlobalEnabled":
					ToggleContainer(ModuleSettings.GlobalEnabled.get_Value());
					break;
				}
			};
			await InitializeStates();
		}

		private async Task InitializeStates()
		{
			string eventsDirectory = DirectoriesManager.GetFullDirectoryPath("events");
			HiddenState = new HiddenState(eventsDirectory);
			lock (States)
			{
				States.Add(HiddenState);
			}
			foreach (ManagedState state in States)
			{
				await state.Start();
			}
		}

		private async Task UpdateCompletedWorldbosses(GameTime gameTime)
		{
			if (gameTime != null)
			{
				TIME_SINCE_LAST_UPDATE_WORLDBOSSES += gameTime.get_ElapsedGameTime();
			}
			if (gameTime != null && !(TIME_SINCE_LAST_UPDATE_WORLDBOSSES.TotalMilliseconds >= 300010.0))
			{
				return;
			}
			try
			{
				lock (CompletedWorldbosses)
				{
					CompletedWorldbosses.Clear();
				}
				if (Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
				{
					(TokenPermission)1,
					(TokenPermission)6
				}))
				{
					IApiV2ObjectList<string> bosses = await ((IBlobClient<IApiV2ObjectList<string>>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
						.get_WorldBosses()).GetAsync(default(CancellationToken));
					lock (CompletedWorldbosses)
					{
						CompletedWorldbosses.AddRange((IEnumerable<string>)bosses);
					}
					TIME_SINCE_LAST_UPDATE_WORLDBOSSES = TimeSpan.Zero;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void ToggleContainer(bool show)
		{
			if (ModuleSettings.GlobalEnabled.get_Value() && show)
			{
				Container.Show();
			}
			else if (Container != null)
			{
				Container.Hide();
			}
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new ModuleSettingsView(ModuleSettings);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Expected O, but got Unknown
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Expected O, but got Unknown
			((Module)this).OnModuleLoaded(e);
			Container.UpdatePosition(ModuleSettings.LocationX.get_Value(), ModuleSettings.LocationY.get_Value());
			Container.UpdateSize(ModuleSettings.Width.get_Value(), ModuleSettings.Height.get_Value());
			ManageEventTab = GameService.Overlay.get_BlishHudWindow().AddTab("Event Table", ContentsManager.GetRenderIcon("images\\event_boss.png"), (Func<IView>)(() => (IView)(object)new ManageEventsView(EventCategories, ModuleSettings.AllEvents)), 0);
			Rectangle settingsWindowSize = default(Rectangle);
			((Rectangle)(ref settingsWindowSize))._002Ector(24, 30, 1000, 630);
			TabbedWindow2 val = new TabbedWindow2(AsyncTexture2D.op_Implicit(ContentsManager.GetRenderIcon("images\\windowBackground.png")), settingsWindowSize, new Rectangle(settingsWindowSize.X + 46, settingsWindowSize.Y, settingsWindowSize.Width - 46, settingsWindowSize.Height));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("TabbedWindow");
			((WindowBase2)val).set_Emblem(AsyncTexture2D.op_Implicit(ContentsManager.GetRenderIcon("images\\event_boss.png")));
			((WindowBase2)val).set_Subtitle("Example Subtitle");
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id("EventTableModule_6bd04be4-dc19-4914-a2c3-8160ce76818b");
			SettingsWindow = val;
			SettingsWindow.get_Tabs().Add(new Tab(AsyncTexture2D.op_Implicit(((WindowBase2)SettingsWindow).get_Emblem()), (Func<IView>)(() => (IView)(object)new ManageEventsView(EventCategories, ModuleSettings.AllEvents)), "Events", (int?)null));
			if (ModuleSettings.GlobalEnabled.get_Value())
			{
				ToggleContainer(show: true);
			}
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)delegate(object s, ValueEventArgs<bool> eventArgs)
			{
				ToggleContainer(!eventArgs.get_Value());
			});
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)delegate
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Invalid comparison between Unknown and I4
				ToggleContainer((int)GameService.Gw2Mumble.get_CurrentMap().get_Type() != 1);
			});
		}

		protected override void Update(GameTime gameTime)
		{
			CheckMumble();
			Container.UpdatePosition(ModuleSettings.LocationX.get_Value(), ModuleSettings.LocationY.get_Value());
			Task.Run(async delegate
			{
				await UpdateCompletedWorldbosses(gameTime);
			});
			foreach (ManagedState state in States)
			{
				state.Update(gameTime);
			}
		}

		private void CheckMumble()
		{
			if (Container != null && GameService.Gw2Mumble.get_IsAvailable() && ModuleSettings.HideOnMissingMumbleTicks.get_Value())
			{
				bool tickState = GameService.Gw2Mumble.get_TimeSinceTick().TotalSeconds < 0.5;
				if (tickState != visibleStateFromTick)
				{
					visibleStateFromTick = tickState;
					ToggleContainer(visibleStateFromTick);
				}
			}
		}

		protected override void Unload()
		{
			if (ManageEventTab != null)
			{
				GameService.Overlay.get_BlishHudWindow().RemoveTab(ManageEventTab);
			}
			if (Container != null)
			{
				((Control)Container).Dispose();
			}
			if (SettingsWindow != null)
			{
				((Control)SettingsWindow).Hide();
			}
			Logger.Debug("Unloading states...");
			Task.WaitAll((from state in States.ToList()
				select state.Unload()).ToArray());
			Logger.Debug("Finished unloading states.");
		}
	}
}
