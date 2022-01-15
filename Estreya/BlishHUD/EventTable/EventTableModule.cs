using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
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
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable
{
	[Export(typeof(Module))]
	public class EventTableModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<EventTableModule>();

		internal static EventTableModule ModuleInstance;

		internal ModuleSettings ModuleSettings;

		private bool visibleStateFromTick = true;

		private TimeSpan _eventTimeSpan = TimeSpan.Zero;

		private EventTableContainer Container { get; set; }

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		private WindowTab ManageEventTab { get; set; }

		internal TabbedWindow2 SettingsWindow { get; private set; }

		private IEnumerable<EventCategory> EventCategories { get; set; }

		internal bool Debug => ModuleSettings.DebugEnabled.get_Value();

		internal int EventHeight => ModuleSettings?.EventHeight?.get_Value() ?? 30;

		internal DateTime DateTimeNow => DateTime.Now;

		internal TimeSpan EventTimeSpan
		{
			get
			{
				if (_eventTimeSpan == TimeSpan.Zero)
				{
					_eventTimeSpan = TimeSpan.FromMinutes(ModuleSettings.EventTimeSpan.get_Value());
				}
				return _eventTimeSpan;
			}
		}

		internal DateTime EventTimeMin => ModuleInstance.DateTimeNow.Subtract(EventTimeSpan.Subtract(TimeSpan.FromMilliseconds(EventTimeSpan.TotalMilliseconds / 2.0)));

		internal DateTime EventTimeMax => ModuleInstance.DateTimeNow.Add(EventTimeSpan.Subtract(TimeSpan.FromMilliseconds(EventTimeSpan.TotalMilliseconds / 2.0)));

		internal Collection<ManagedState> States { get; private set; } = new Collection<ManagedState>();


		internal HiddenState HiddenState { get; private set; }

		internal WorldbossState WorldbossState { get; private set; }

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
		}

		protected override async Task LoadAsync()
		{
			using (StreamReader eventsReader = new StreamReader(ContentsManager.GetFileStream("events.json")))
			{
				EventCategories = JsonConvert.DeserializeObject<List<EventCategory>>(await eventsReader.ReadToEndAsync());
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
				case "EventTimeSpan":
					_eventTimeSpan = TimeSpan.Zero;
					break;
				}
			};
			await InitializeStates();
		}

		private async Task InitializeStates()
		{
			string eventsDirectory = DirectoriesManager.GetFullDirectoryPath("events");
			HiddenState = new HiddenState(eventsDirectory);
			WorldbossState = new WorldbossState(Gw2ApiManager);
			lock (States)
			{
				States.Add(HiddenState);
				States.Add(WorldbossState);
			}
			foreach (ManagedState state in States)
			{
				await state.Start();
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
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			CheckMumble();
			Container.UpdatePosition(ModuleSettings.LocationX.get_Value(), ModuleSettings.LocationY.get_Value());
			SettingComplianceExtensions.SetRange(ModuleSettings.LocationX, 0, (int)((float)GameService.Graphics.get_Resolution().X / GameService.Graphics.get_UIScaleMultiplier()));
			SettingComplianceExtensions.SetRange(ModuleSettings.LocationY, 0, (int)((float)GameService.Graphics.get_Resolution().Y / GameService.Graphics.get_UIScaleMultiplier()));
			SettingComplianceExtensions.SetRange(ModuleSettings.Width, 0, (int)((float)GameService.Graphics.get_Resolution().X / GameService.Graphics.get_UIScaleMultiplier()));
			SettingComplianceExtensions.SetRange(ModuleSettings.Height, 0, (int)((float)GameService.Graphics.get_Resolution().Y / GameService.Graphics.get_UIScaleMultiplier()));
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
