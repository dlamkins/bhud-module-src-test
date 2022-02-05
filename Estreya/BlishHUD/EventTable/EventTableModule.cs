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
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Extensions;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.State;
using Estreya.BlishHUD.EventTable.UI.Container;
using Estreya.BlishHUD.EventTable.UI.Views;
using Estreya.BlishHUD.EventTable.UI.Views.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.EventTable
{
	[Export(typeof(Module))]
	public class EventTableModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<EventTableModule>();

		internal static EventTableModule ModuleInstance;

		internal ModuleSettings ModuleSettings;

		private BitmapFont _font;

		private TimeSpan _eventTimeSpan = TimeSpan.Zero;

		private List<EventCategory> _eventCategories;

		private EventTableContainer Container { get; set; }

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

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

		internal List<EventCategory> EventCategories
		{
			get
			{
				return _eventCategories.Where((EventCategory ec) => !ec.IsDisabled()).ToList();
			}
			set
			{
				_eventCategories = value;
			}
		}

		internal Collection<ManagedState> States { get; private set; } = new Collection<ManagedState>();


		public HiddenState HiddenState { get; private set; }

		public WorldbossState WorldbossState { get; private set; }

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
			_eventCategories.ForEach(delegate(EventCategory ec)
			{
				ec.Events.ForEach(delegate(Event e)
				{
					e.EventCategory = ec;
				});
			});
			ModuleSettings.InitializeEventSettings(_eventCategories);
			await InitializeStates();
			EventTableModule eventTableModule = this;
			EventTableContainer eventTableContainer = new EventTableContainer();
			((Control)eventTableContainer).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)eventTableContainer).set_BackgroundColor(Color.get_Transparent());
			((Control)eventTableContainer).set_Opacity(0f);
			eventTableContainer.Visible = false;
			eventTableModule.Container = eventTableContainer;
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
				}
			};
		}

		private async Task InitializeStates()
		{
			string eventsDirectory = DirectoriesManager.GetFullDirectoryPath("events");
			HiddenState = new HiddenState(eventsDirectory);
			WorldbossState = new WorldbossState(Gw2ApiManager);
			WorldbossState.WorldbossCompleted += delegate(object s, string e)
			{
				if (ModuleSettings.WorldbossCompletedAcion.get_Value() == WorldbossCompletedAction.Hide)
				{
					(from ev in _eventCategories.SelectMany((EventCategory ec) => ec.Events)
						where ev.APICode == e
						select ev).ToList().ForEach(delegate(Event ev)
					{
						ev.Finish();
					});
				}
			};
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
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Expected O, but got Unknown
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Expected O, but got Unknown
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Expected O, but got Unknown
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Expected O, but got Unknown
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Expected O, but got Unknown
			((Module)this).OnModuleLoaded(e);
			Container.UpdatePosition(ModuleSettings.LocationX.get_Value(), ModuleSettings.LocationY.get_Value());
			Container.UpdateSize(ModuleSettings.Width.get_Value(), -1);
			Texture2D windowBackground = AsyncTexture2D.op_Implicit(ContentsManager.GetIcon("images\\502049.png", checkRenderAPI: false));
			Rectangle settingsWindowSize = default(Rectangle);
			((Rectangle)(ref settingsWindowSize))._002Ector(35, 26, 1100, 714);
			int contentRegionPaddingY = settingsWindowSize.Y - 15;
			int contentRegionPaddingX = settingsWindowSize.X + 46;
			Rectangle contentRegion = default(Rectangle);
			((Rectangle)(ref contentRegion))._002Ector(contentRegionPaddingX, contentRegionPaddingY, settingsWindowSize.Width - 46, settingsWindowSize.Height - contentRegionPaddingY);
			TabbedWindow2 val = new TabbedWindow2(windowBackground, settingsWindowSize, contentRegion);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Event Table");
			((WindowBase2)val).set_Emblem(AsyncTexture2D.op_Implicit(ContentsManager.GetIcon("images\\event_boss.png")));
			((WindowBase2)val).set_Subtitle("Settings");
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id("EventTableModule_6bd04be4-dc19-4914-a2c3-8160ce76818b");
			SettingsWindow = val;
			SettingsWindow.get_Tabs().Add(new Tab(ContentsManager.GetIcon("images\\event_boss_grey.png"), (Func<IView>)(() => (IView)(object)new ManageEventsView(_eventCategories, ModuleSettings.AllEvents)), "Manage Events", (int?)null));
			SettingsWindow.get_Tabs().Add(new Tab(ContentsManager.GetIcon("156736"), (Func<IView>)(() => (IView)(object)new GeneralSettingsView(ModuleSettings)), "General Settings", (int?)null));
			SettingsWindow.get_Tabs().Add(new Tab(ContentsManager.GetIcon("images\\graphics_settings.png"), (Func<IView>)(() => (IView)(object)new GraphicsSettingsView(ModuleSettings)), "Graphic Settings", (int?)null));
			SettingsWindow.get_Tabs().Add(new Tab(ContentsManager.GetIcon("155052"), (Func<IView>)(() => (IView)(object)new EventSettingsView(ModuleSettings)), "Event Settings", (int?)null));
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
			foreach (ManagedState state in States)
			{
				state.Update(gameTime);
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
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Invalid comparison between Unknown and I4
			if (GameService.Gw2Mumble.get_IsAvailable() && Container != null)
			{
				bool show = true;
				show &= !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
				if (ModuleSettings.HideOnMissingMumbleTicks.get_Value())
				{
					show &= GameService.Gw2Mumble.get_TimeSinceTick().TotalSeconds < 0.5;
				}
				if (ModuleSettings.HideInCombat.get_Value())
				{
					show &= !GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat();
				}
				show &= (int)GameService.Gw2Mumble.get_CurrentMap().get_Type() != 1;
				ToggleContainer(show);
			}
		}

		protected override void Unload()
		{
			if (Container != null)
			{
				((Control)Container).Dispose();
			}
			if (SettingsWindow != null)
			{
				((Control)SettingsWindow).Hide();
			}
			HandleCornerIcon(show: false);
			Logger.Debug("Unloading states...");
			Task.WaitAll((from state in States.ToList()
				select state.Unload()).ToArray());
			Logger.Debug("Finished unloading states.");
		}
	}
}
