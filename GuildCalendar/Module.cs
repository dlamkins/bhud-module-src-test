using System;
using System.Collections.Generic;
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GuildCalendar
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		public enum GuildIcon
		{
			Guardian,
			Warrior,
			Engineer,
			Ranger,
			Thief,
			Elementalist,
			Mesmer,
			Necromancer,
			Revenant
		}

		private class GuildSetting
		{
			public SettingEntry<string> Name;

			public SettingEntry<string> Link;

			public SettingEntry<KeyBinding> Hotkey;

			public SettingEntry<bool> IsVisible;

			public SettingEntry<GuildIcon> IconPicker;

			public Tab Tab;
		}

		private class SmartCalendarTab : CalendarView
		{
			private SettingEntry<string> _linkSetting;

			private CalendarService _service;

			public SmartCalendarTab(SettingEntry<string> linkSetting, CalendarService service, Func<bool> canEdit)
				: base(canEdit)
			{
				_linkSetting = linkSetting;
				_service = service;
				_linkSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate
				{
					LoadData();
				});
				LoadData();
			}

			private async Task LoadData()
			{
				UpdateStatus("Checking...", Color.get_Yellow());
				if (string.IsNullOrWhiteSpace(_linkSetting.get_Value()))
				{
					UpdateStatus("No Link", Color.get_Red());
					return;
				}
				try
				{
					List<GuildEvent> events = await _service.FetchEvents(_linkSetting.get_Value());
					UpdateEvents(events);
					UpdateStatus((events.Count > 0) ? $"Loaded {events.Count}" : "No Events", (events.Count > 0) ? Color.get_Green() : Color.get_Orange());
				}
				catch (Exception ex)
				{
					UpdateStatus("Error", Color.get_Red());
					Logger.Warn(ex, "Load Failed");
				}
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private List<GuildSetting> _guildSettings = new List<GuildSetting>();

		private SettingEntry<bool> _showEditorTools;

		private CalendarService _service;

		private TabbedWindow2 _window;

		private CornerIcon _icon;

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
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Expected O, but got Unknown
			_showEditorTools = settings.DefineSetting<bool>("ShowEditorTools_V30", false, (Func<string>)(() => "Enable Officer Tools"), (Func<string>)(() => "WARNING: You cannot add events to the Guild Calendar unless authorized by the calendar owner.\nAny events created here will save to your PERSONAL local calendar only."));
			for (int i = 1; i <= 6; i++)
			{
				int id = i;
				GuildSetting guild = new GuildSetting();
				guild.Name = settings.DefineSetting<string>($"Guild{id}_Name_V30", $"Guild {id}", (Func<string>)(() => $"[Guild {id}] Name"), (Func<string>)null);
				guild.IsVisible = settings.DefineSetting<bool>($"Guild{id}_Visible_V30", true, (Func<string>)(() => $"[Guild {id}] Show in Window"), (Func<string>)null);
				GuildIcon defaultIcon = (GuildIcon)((i - 1) % Enum.GetValues(typeof(GuildIcon)).Length);
				guild.IconPicker = settings.DefineSetting<GuildIcon>($"Guild{id}_IconSelect_V30", defaultIcon, (Func<string>)(() => $"[Guild {id}] Icon"), (Func<string>)null);
				guild.Link = settings.DefineSetting<string>($"Guild{id}_Link_V30", "", (Func<string>)(() => $"[Guild {id}] iCal Link"), (Func<string>)null);
				guild.Hotkey = settings.DefineSetting<KeyBinding>($"Guild{id}_Key_V30", new KeyBinding((Keys)0), (Func<string>)(() => $"[Guild {id}] Hotkey"), (Func<string>)null);
				_guildSettings.Add(guild);
			}
		}

		protected override void Initialize()
		{
			_service = new CalendarService();
		}

		protected override async Task LoadAsync()
		{
			AsyncTexture2D bgTexture = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155985);
			TabbedWindow2 val = new TabbedWindow2(bgTexture, new Rectangle(35, 36, 873, 691), new Rectangle(100, 50, 840, 630));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Guild Calendar");
			((WindowBase2)val).set_Emblem(AsyncTexture2D.op_Implicit(AsyncTexture2D.FromAssetId(156022)));
			((WindowBase2)val).set_Id("GuildCalWindow");
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_SavesSize(false);
			((WindowBase2)val).set_CanResize(false);
			((Control)val).set_Size(new Point(960, 700));
			_window = val;
			foreach (GuildSetting guildSetting in _guildSettings)
			{
				GuildIcon iconType = guildSetting.IconPicker?.get_Value() ?? GuildIcon.Guardian;
				guildSetting.Tab = new Tab(GetIconTexture(iconType), (Func<IView>)(() => (IView)(object)new SmartCalendarTab(guildSetting.Link, _service, () => _showEditorTools.get_Value())), guildSetting.Name.get_Value(), (int?)null);
				if (guildSetting.IsVisible.get_Value())
				{
					_window.get_Tabs().Add(guildSetting.Tab);
				}
				guildSetting.IsVisible.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
				{
					if (e.get_NewValue())
					{
						_window.get_Tabs().Add(guildSetting.Tab);
					}
					else
					{
						_window.get_Tabs().Remove(guildSetting.Tab);
					}
				});
				guildSetting.Name.add_SettingChanged((EventHandler<ValueChangedEventArgs<string>>)delegate(object s, ValueChangedEventArgs<string> e)
				{
					guildSetting.Tab.set_Name(e.get_NewValue());
				});
				guildSetting.IconPicker.add_SettingChanged((EventHandler<ValueChangedEventArgs<GuildIcon>>)delegate(object s, ValueChangedEventArgs<GuildIcon> e)
				{
					guildSetting.Tab.set_Icon(GetIconTexture(e.get_NewValue()));
				});
				if (guildSetting.Hotkey == null)
				{
					continue;
				}
				guildSetting.Hotkey.get_Value().set_Enabled(true);
				guildSetting.Hotkey.get_Value().add_Activated((EventHandler<EventArgs>)delegate
				{
					if (guildSetting.IsVisible.get_Value())
					{
						((WindowBase2)_window).ToggleWindow();
						if (((Control)_window).get_Visible())
						{
							_window.set_SelectedTab(guildSetting.Tab);
						}
					}
				});
			}
			CornerIcon val2 = new CornerIcon();
			val2.set_Icon(AsyncTexture2D.FromAssetId(156022));
			((Control)val2).set_BasicTooltipText("Guild Calendar");
			val2.set_Priority(5);
			_icon = val2;
			((Control)_icon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((WindowBase2)_window).ToggleWindow();
			});
		}

		private AsyncTexture2D GetIconTexture(GuildIcon iconSelection)
		{
			int assetId = 156633;
			return AsyncTexture2D.FromAssetId(iconSelection switch
			{
				GuildIcon.Warrior => 156642, 
				GuildIcon.Engineer => 156631, 
				GuildIcon.Ranger => 156639, 
				GuildIcon.Thief => 103581, 
				GuildIcon.Elementalist => 156629, 
				GuildIcon.Mesmer => 156635, 
				GuildIcon.Necromancer => 156637, 
				GuildIcon.Revenant => 965717, 
				_ => 156633, 
			});
		}

		protected override void Unload()
		{
			foreach (GuildSetting g in _guildSettings)
			{
				if (g.Hotkey != null)
				{
					g.Hotkey.get_Value().set_Enabled(false);
				}
			}
			TabbedWindow2 window = _window;
			if (window != null)
			{
				((Control)window).Dispose();
			}
			CornerIcon icon = _icon;
			if (icon != null)
			{
				((Control)icon).Dispose();
			}
		}
	}
}
