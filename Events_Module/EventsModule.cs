using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Events_Module.Properties;
using Humanizer;
using Humanizer.Localisation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Events_Module
{
	[Export(typeof(Module))]
	public class EventsModule : Module
	{
		internal static EventsModule ModuleInstance;

		private string _ddAlphabetical = Resources.Alphabetical;

		private string _ddNextup = Resources.Next_Up;

		private string _ecAllevents = Resources.All_Events;

		private string _ecWatchedEvents = Resources.Watched_Events;

		private string _ecHidden = Resources.Hidden_Events;

		private const int TIMER_RECALC_RATE = 5;

		private List<DetailsButton> _displayedEvents;

		private WindowTab _eventsTab;

		private Panel _tabPanel;

		private SettingCollection _watchCollection;

		private SettingEntry<bool> _settingNotificationsEnabled;

		private SettingEntry<bool> _settingChimeEnabled;

		private SettingEntry<Point> _settingNotificationsPosition;

		private Texture2D _textureWatch;

		private Texture2D _textureWatchActive;

		private double _elapsedSeconds;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		public bool NotificationsEnabled
		{
			get
			{
				return _settingNotificationsEnabled.get_Value();
			}
			set
			{
				_settingNotificationsEnabled.set_Value(value);
			}
		}

		public bool ChimeEnabled
		{
			get
			{
				return _settingChimeEnabled.get_Value();
			}
			set
			{
				_settingChimeEnabled.set_Value(value);
			}
		}

		public Point NotificationPosition
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return _settingNotificationsPosition.get_Value();
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_settingNotificationsPosition.set_Value(value);
			}
		}

		[ImportingConstructor]
		public EventsModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			SettingCollection selfManagedSettings = settings.AddSubCollection("Managed Settings", false);
			_settingNotificationsEnabled = selfManagedSettings.DefineSetting<bool>("notificationsEnabled", true, (Func<string>)null, (Func<string>)null);
			_settingChimeEnabled = selfManagedSettings.DefineSetting<bool>("chimeEnabled", true, (Func<string>)null, (Func<string>)null);
			_settingNotificationsPosition = selfManagedSettings.DefineSetting<Point>("notificationPosition", new Point(180, 60), (Func<string>)null, (Func<string>)null);
			_watchCollection = settings.AddSubCollection("Watching", false);
		}

		protected override void Initialize()
		{
			_displayedEvents = new List<DetailsButton>();
			GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)ChangeLocalization);
		}

		private void LoadTextures()
		{
			_textureWatch = ContentsManager.GetTexture("textures\\605021.png");
			_textureWatchActive = ContentsManager.GetTexture("textures\\605019.png");
		}

		protected override async Task LoadAsync()
		{
			await Meta.Load(ContentsManager);
			LoadTextures();
			_tabPanel = BuildSettingPanel(((Container)GameService.Overlay.get_BlishHudWindow()).get_ContentRegion());
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			_eventsTab = GameService.Overlay.get_BlishHudWindow().AddTab(Resources.Events_and_Metas, AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("textures\\1466345.png")), _tabPanel);
			((Module)this).OnModuleLoaded(e);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new BasicSettingsView();
		}

		internal void ShowSetNotificationPositions()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			SettingEntry<Point> obj = new SettingEntry<Point>();
			obj.set_Value(new Point(280, 512));
			SettingEntry<Point> tempSizeSetting = obj;
			NotificationMover notificationMover = new NotificationMover(new ScreenRegion("Notifications", _settingNotificationsPosition, tempSizeSetting));
			((Control)notificationMover).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			Rectangle contentRegion = ((Container)GameService.Graphics.get_SpriteScreen()).get_ContentRegion();
			((Control)notificationMover).set_Size(((Rectangle)(ref contentRegion)).get_Size());
		}

		private Panel BuildSettingPanel(Rectangle panelBounds)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Expected O, but got Unknown
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Expected O, but got Unknown
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Expected O, but got Unknown
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Expected O, but got Unknown
			//IL_0411: Unknown result type (might be due to invalid IL or missing references)
			//IL_0416: Unknown result type (might be due to invalid IL or missing references)
			//IL_041b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0425: Unknown result type (might be due to invalid IL or missing references)
			//IL_042f: Unknown result type (might be due to invalid IL or missing references)
			//IL_044f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_046e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_0482: Expected O, but got Unknown
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04de: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_052d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0532: Unknown result type (might be due to invalid IL or missing references)
			//IL_054c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0566: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_058f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0590: Unknown result type (might be due to invalid IL or missing references)
			//IL_059a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0606: Unknown result type (might be due to invalid IL or missing references)
			//IL_060d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0624: Unknown result type (might be due to invalid IL or missing references)
			//IL_0631: Expected O, but got Unknown
			//IL_0691: Unknown result type (might be due to invalid IL or missing references)
			//IL_0696: Unknown result type (might be due to invalid IL or missing references)
			//IL_0699: Unknown result type (might be due to invalid IL or missing references)
			//IL_069e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c5: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Size(((Rectangle)(ref panelBounds)).get_Size());
			Panel etPanel = val;
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Location(new Point(((Control)etPanel).get_Right() - 150 - ((DesignStandard)(ref Dropdown.Standard)).get_ControlOffset().X, ((DesignStandard)(ref Dropdown.Standard)).get_ControlOffset().Y));
			((Control)val2).set_Width(150);
			((Control)val2).set_Parent((Container)(object)etPanel);
			Dropdown ddSortMethod = val2;
			Checkbox val3 = new Checkbox();
			val3.set_Text(Resources.Enable_Notifications);
			val3.set_Checked(NotificationsEnabled);
			((Control)val3).set_Parent((Container)(object)etPanel);
			Checkbox notificationToggle = val3;
			((Control)notificationToggle).set_Location(new Point(((Control)ddSortMethod).get_Left() - ((Control)notificationToggle).get_Width() - 10, ((Control)ddSortMethod).get_Top() + 6));
			Checkbox val4 = new Checkbox();
			val4.set_Text(Resources.Mute_Notifications);
			val4.set_Checked(!ChimeEnabled);
			((Control)val4).set_Parent((Container)(object)etPanel);
			((Control)val4).set_Top(((Control)notificationToggle).get_Top());
			((Control)val4).set_Right(((Control)notificationToggle).get_Left() - 10);
			notificationToggle.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object sender, CheckChangedEvent e)
			{
				NotificationsEnabled = e.get_Checked();
			});
			val4.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object sender, CheckChangedEvent e)
			{
				ChimeEnabled = !e.get_Checked();
			});
			int topOffset = ((Control)ddSortMethod).get_Bottom() + ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().Y;
			Panel val5 = new Panel();
			val5.set_Title(Resources.Event_Categories);
			val5.set_ShowBorder(true);
			((Control)val5).set_Size(((DesignStandard)(ref Panel.MenuStandard)).get_Size() - new Point(0, topOffset + ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().Y));
			((Control)val5).set_Location(new Point(((DesignStandard)(ref Panel.MenuStandard)).get_PanelOffset().X, topOffset));
			((Control)val5).set_Parent((Container)(object)etPanel);
			Panel menuSection = val5;
			FlowPanel val6 = new FlowPanel();
			val6.set_FlowDirection((ControlFlowDirection)0);
			val6.set_ControlPadding(new Vector2(8f, 8f));
			((Control)val6).set_Location(new Point(((Control)menuSection).get_Right() + ((DesignStandard)(ref Panel.MenuStandard)).get_ControlOffset().X, ((Control)menuSection).get_Top()));
			((Control)val6).set_Size(new Point(((Control)ddSortMethod).get_Right() - ((Control)menuSection).get_Right() - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, ((Control)menuSection).get_Height()));
			((Panel)val6).set_CanScroll(true);
			((Control)val6).set_Parent((Container)(object)etPanel);
			FlowPanel eventPanel = val6;
			TextBox val7 = new TextBox();
			((TextInputBase)val7).set_PlaceholderText(Resources.Event_Search);
			((Control)val7).set_Width(((Control)menuSection).get_Width());
			((Control)val7).set_Location(new Point(((Control)ddSortMethod).get_Top(), ((Control)menuSection).get_Left()));
			((Control)val7).set_Parent((Container)(object)etPanel);
			TextBox searchBox = val7;
			((TextInputBase)searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				eventPanel.FilterChildren<DetailsButton>((Func<DetailsButton, bool>)((DetailsButton db) => db.get_Text().ToLower().Contains(((TextInputBase)searchBox).get_Text().ToLower())));
			});
			((Control)eventPanel).SuspendLayout();
			foreach (Meta meta in Meta.Events)
			{
				SettingEntry<bool> setting = _watchCollection.DefineSetting<bool>("watchEvent:" + meta.Name, true, (Func<string>)null, (Func<string>)null);
				meta.IsWatched = setting.get_Value();
				DetailsButton val8 = new DetailsButton();
				((Control)val8).set_Parent((Container)(object)eventPanel);
				((Control)val8).set_BasicTooltipText(Resources.ResourceManager.GetString(meta.Category) ?? meta.Category);
				val8.set_Text(Resources.ResourceManager.GetString(meta.Name) ?? meta.Name);
				val8.set_IconSize((DetailsIconSize)0);
				val8.set_ShowVignette(false);
				val8.set_HighlightType((DetailsHighlightType)2);
				val8.set_ShowToggleButton(true);
				DetailsButton es2 = val8;
				if (meta.Texture.get_HasTexture())
				{
					es2.set_Icon(meta.Texture);
				}
				Label val9 = new Label();
				((Control)val9).set_Size(new Point(65, ((Container)es2).get_ContentRegion().Height));
				val9.set_Text(meta.NextTime.ToShortTimeString());
				((Control)val9).set_BasicTooltipText(GetTimeDetails(meta));
				val9.set_HorizontalAlignment((HorizontalAlignment)1);
				val9.set_VerticalAlignment((VerticalAlignment)1);
				((Control)val9).set_Parent((Container)(object)es2);
				Label nextTimeLabel = val9;
				if (!string.IsNullOrEmpty(meta.Wiki))
				{
					GlowButton val10 = new GlowButton();
					val10.set_Icon(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("102530")));
					val10.set_ActiveIcon(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("glow-wiki")));
					((Control)val10).set_BasicTooltipText(Resources.Read_about_this_event_on_the_wiki_);
					((Control)val10).set_Parent((Container)(object)es2);
					val10.set_GlowColor(Color.get_White() * 0.1f);
					((Control)val10).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						if (UrlIsValid(meta.Wiki))
						{
							Process.Start(meta.Wiki);
						}
					});
				}
				if (!string.IsNullOrEmpty(meta.Waypoint))
				{
					GlowButton val11 = new GlowButton();
					val11.set_Icon(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("waypoint")));
					val11.set_ActiveIcon(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("glow-waypoint")));
					((Control)val11).set_BasicTooltipText(string.Format(Resources.Nearby_waypoint___0_, meta.Waypoint));
					((Control)val11).set_Parent((Container)(object)es2);
					val11.set_GlowColor(Color.get_White() * 0.1f);
					((Control)val11).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						ClipboardUtil.get_WindowsClipboardService().SetTextAsync(meta.Waypoint).ContinueWith(delegate(Task<bool> clipboardResult)
						{
							if (clipboardResult.IsFaulted)
							{
								ScreenNotification.ShowNotification(Resources.Failed_to_copy_waypoint_to_clipboard__Try_again_, (NotificationType)6, (Texture2D)null, 2);
							}
							else
							{
								ScreenNotification.ShowNotification(Resources.Copied_waypoint_to_clipboard_, (NotificationType)0, (Texture2D)null, 2);
							}
						});
					});
				}
				((Control)eventPanel).ResumeLayout(false);
				GlowButton val12 = new GlowButton();
				val12.set_Icon(AsyncTexture2D.op_Implicit(_textureWatch));
				val12.set_ActiveIcon(AsyncTexture2D.op_Implicit(_textureWatchActive));
				((Control)val12).set_BasicTooltipText(Resources.Click_to_toggle_tracking_for_this_event_);
				val12.set_ToggleGlow(true);
				val12.set_Checked(meta.IsWatched);
				((Control)val12).set_Parent((Container)(object)es2);
				GlowButton toggleFollowBttn = val12;
				((Control)toggleFollowBttn).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					meta.IsWatched = toggleFollowBttn.get_Checked();
					setting.set_Value(toggleFollowBttn.get_Checked());
				});
				meta.OnNextRunTimeChanged += delegate
				{
					UpdateSort(ddSortMethod, EventArgs.Empty);
					SortEventPanel(ddSortMethod.get_SelectedItem(), ref eventPanel);
					nextTimeLabel.set_Text(meta.NextTime.ToShortTimeString());
					((Control)nextTimeLabel).set_BasicTooltipText(GetTimeDetails(meta));
				};
				_displayedEvents.Add(es2);
			}
			Menu val13 = new Menu();
			Rectangle contentRegion = ((Container)menuSection).get_ContentRegion();
			((Control)val13).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val13.set_MenuItemHeight(40);
			((Control)val13).set_Parent((Container)(object)menuSection);
			val13.set_CanSelect(true);
			Menu eventCategories = val13;
			List<IGrouping<string, Meta>> submetas = (from e in Meta.Events
				group e by e.Category).ToList();
			MenuItem obj = eventCategories.AddMenuItem(_ecAllevents, (Texture2D)null);
			obj.Select();
			((Control)obj).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				eventPanel.FilterChildren<DetailsButton>((Func<DetailsButton, bool>)((DetailsButton db) => true));
			});
			((Control)eventCategories.AddMenuItem(_ecWatchedEvents, (Texture2D)null)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				eventPanel.FilterChildren<DetailsButton>((Func<DetailsButton, bool>)((DetailsButton db) => Meta.Events.Find((Meta m) => db.get_Text() == (Resources.ResourceManager.GetString(m.Name) ?? m.Name)).IsWatched));
			});
			foreach (IGrouping<string, Meta> e2 in submetas)
			{
				string category = Resources.ResourceManager.GetString(e2.Key) ?? e2.Key;
				((Control)eventCategories.AddMenuItem(category, (Texture2D)null)).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					eventPanel.FilterChildren<DetailsButton>((Func<DetailsButton, bool>)((DetailsButton db) => string.Equals(((Control)db).get_BasicTooltipText(), category)));
				});
			}
			ddSortMethod.get_Items().Add(_ddAlphabetical);
			ddSortMethod.get_Items().Add(_ddNextup);
			ddSortMethod.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object sender, ValueChangedEventArgs args)
			{
				SortEventPanel(args.get_CurrentValue(), ref eventPanel);
			});
			ddSortMethod.set_SelectedItem(_ddNextup);
			UpdateSort(ddSortMethod, EventArgs.Empty);
			return etPanel;
		}

		private void RepositionES()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			int pos = 0;
			foreach (DetailsButton es in _displayedEvents)
			{
				int x = pos % 2;
				int y = pos / 2;
				((Control)es).set_Location(new Point(x * 308, y * 108));
				if (((Control)es).get_Visible())
				{
					pos++;
				}
				if (((Control)es).get_Parent() != null)
				{
					((Container)(Panel)((Control)es).get_Parent()).set_VerticalScrollOffset(0);
					((Control)((Control)es).get_Parent()).Invalidate();
				}
			}
		}

		private string GetTimeDetails(Meta assignedMeta)
		{
			TimeSpan timeUntil = assignedMeta.NextTime - DateTime.Now;
			StringBuilder msg = new StringBuilder();
			msg.AppendLine(string.Format(Resources.Starts_in__0_, TimeSpanHumanizeExtensions.Humanize(timeUntil, 2, (CultureInfo)null, (TimeUnit)3, (TimeUnit)2, (string)null, false)));
			msg.Append(Environment.NewLine + Resources.Upcoming_Event_Times_);
			using (List<DateTime>.Enumerator enumerator = (from time in assignedMeta.Times
				select (!(time > DateTime.UtcNow)) ? (time.ToLocalTime() + NumberToTimeSpanExtensions.Days(1)) : time.ToLocalTime() into time
				orderby time.Ticks
				select time).ToList().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					msg.Append(string.Concat(str1: enumerator.Current.ToShortTimeString(), str0: Environment.NewLine));
				}
			}
			return msg.ToString();
		}

		private void UpdateSort(object sender, EventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			string item = ((Dropdown)sender).get_SelectedItem();
			if (item == _ddAlphabetical)
			{
				_displayedEvents.Sort((DetailsButton e1, DetailsButton e2) => string.Compare(e1.get_Text(), e2.get_Text(), StringComparison.CurrentCultureIgnoreCase));
			}
			else if (item == _ddNextup)
			{
				IList<string> orderedEvents = GetOrderedNextUpEventNames();
				_displayedEvents.Sort((DetailsButton db1, DetailsButton db2) => orderedEvents.IndexOf(db1.get_Text()) - orderedEvents.IndexOf(db2.get_Text()));
			}
			RepositionES();
		}

		private void SortEventPanel(string ddSortMethodValue, ref FlowPanel eventPanel)
		{
			if (ddSortMethodValue == _ddAlphabetical)
			{
				eventPanel.SortChildren<DetailsButton>((Comparison<DetailsButton>)((DetailsButton db1, DetailsButton db2) => string.Compare(db1.get_Text(), db2.get_Text(), StringComparison.CurrentCultureIgnoreCase)));
			}
			else if (ddSortMethodValue == _ddNextup)
			{
				IList<string> orderedEvents = GetOrderedNextUpEventNames();
				eventPanel.SortChildren<DetailsButton>((Comparison<DetailsButton>)((DetailsButton db1, DetailsButton db2) => orderedEvents.IndexOf(db1.get_Text()) - orderedEvents.IndexOf(db2.get_Text())));
			}
		}

		private static bool UrlIsValid(string source)
		{
			if (Uri.TryCreate(source, UriKind.Absolute, out var uriResult))
			{
				return uriResult.Scheme == Uri.UriSchemeHttps;
			}
			return false;
		}

		protected override void Update(GameTime gameTime)
		{
			_elapsedSeconds += gameTime.get_ElapsedGameTime().TotalSeconds;
			if (_elapsedSeconds > 5.0)
			{
				Meta.UpdateEventSchedules();
				_elapsedSeconds = 0.0;
			}
		}

		protected override void Unload()
		{
			ModuleInstance = null;
			GameService.Overlay.remove_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)ChangeLocalization);
			GameService.Overlay.get_BlishHudWindow().RemoveTab(_eventsTab);
		}

		private IList<string> GetOrderedNextUpEventNames()
		{
			return (from el in Meta.Events
				orderby el.NextTime
				select Resources.ResourceManager.GetString(el.Name) ?? el.Name).ToList();
		}

		private void ChangeLocalization(object sender, EventArgs e)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			_ddAlphabetical = Resources.Alphabetical;
			_ddNextup = Resources.Next_Up;
			_ecAllevents = Resources.All_Events;
			_ecHidden = Resources.Hidden_Events;
			if (_tabPanel != null)
			{
				Panel tabPanel = _tabPanel;
				if (tabPanel != null)
				{
					((Control)tabPanel).Dispose();
				}
				_tabPanel = BuildSettingPanel(((Container)GameService.Overlay.get_BlishHudWindow()).get_ContentRegion());
				if (_eventsTab != null)
				{
					GameService.Overlay.get_BlishHudWindow().RemoveTab(_eventsTab);
				}
				_eventsTab = GameService.Overlay.get_BlishHudWindow().AddTab(Resources.Events_and_Metas, AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("textures\\1466345.png")), _tabPanel);
			}
		}
	}
}
