using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.Utils;
using Glide;
using Microsoft.Xna.Framework;
using NodaTime;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class UpcomingEventsView : BaseView
	{
		private class EventListData
		{
			public Event Event { get; set; }

			public Instant Occurrence { get; set; }
		}

		private static bool _debugColorsActive = false;

		private static TimeSpan _updateListInterval = TimeSpan.FromMilliseconds(1000.0);

		private double _lastListUpdate;

		private readonly Func<List<EventCategory>> _getAllEvents;

		private readonly Func<Instant> _getNow;

		private readonly ModuleSettings _moduleSettings;

		private readonly bool _isExternal;

		private readonly SettingEventService _settingEventService;

		private readonly AccountService _accountService;

		private FlowPanel _listParent;

		private StandardWindow _settingsWindow;

		public event EventHandler OpenExternallyClicked;

		public UpcomingEventsView(Func<List<EventCategory>> getAllEvents, Func<Instant> getNow, ModuleSettings moduleSettings, bool isExternal, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, SettingEventService settingEventService, AccountService accountService)
			: base(apiManager, iconService, translationService)
		{
			_getAllEvents = getAllEvents;
			_getNow = getNow;
			_moduleSettings = moduleSettings;
			_isExternal = isExternal;
			_settingEventService = settingEventService;
			_accountService = accountService;
		}

		protected override async Task<bool> InternalLoad(IProgress<string> progress)
		{
			return true;
		}

		private async Task OpenSettingsViewAsync()
		{
			if (_settingsWindow == null)
			{
				_settingsWindow = WindowUtil.CreateStandardWindow(_moduleSettings, "Event Times Settings", ((object)this).GetType(), Guid.Parse("7dc52c82-67aa-4cfb-9fe3-a16a8b30892d"), base.IconService);
			}
			_ = _settingsWindow.CurrentView;
			EventTimeTableSettingsView view = new EventTimeTableSettingsView(_moduleSettings, base.APIManager, base.IconService, base.TranslationService, _settingEventService);
			await _settingsWindow.Show((IView)(object)view);
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected O, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Panel)val).set_CanScroll(false);
			val.set_FlowDirection((ControlFlowDirection)3);
			FlowPanel flowPanel = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)flowPanel);
			((Control)val2).set_Width(((Container)flowPanel).get_ContentRegion().Width);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_CanScroll(false);
			Panel titleGroup = val2;
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)titleGroup);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Width(150);
			((Panel)val3).set_CanScroll(false);
			val3.set_FlowDirection((ControlFlowDirection)4);
			FlowPanel buttonGroup = val3;
			Rectangle contentRegion = ((Container)titleGroup).get_ContentRegion();
			((Control)buttonGroup).set_Right(((Rectangle)(ref contentRegion)).get_Right() - 20);
			if (!_isExternal || _moduleSettings.ShowEventTimeTableSettingButtonInExternalWindow.get_Value())
			{
				RenderButtonAsync((Panel)(object)buttonGroup, "Settings", OpenSettingsViewAsync);
			}
			if (!_isExternal)
			{
				RenderButton((Panel)(object)buttonGroup, "Open Externally", delegate
				{
					this.OpenExternallyClicked?.Invoke(this, EventArgs.Empty);
				});
			}
			FormattedLabel obj = new FormattedLabelBuilder().AutoSizeHeight().SetWidth(((Container)titleGroup).get_ContentRegion().Width - ((Control)buttonGroup).get_Width()).SetHorizontalAlignment((HorizontalAlignment)1)
				.CreatePart("Event Times", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder x)
				{
					x.MakeBold().SetFontSize((FontSize)36);
				})
				.CreatePart("\n\n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("Find the next event to conquer!", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder x)
				{
					x.SetFontSize((FontSize)18);
				})
				.Build();
			((Control)obj).set_Parent((Container)(object)titleGroup);
			((Control)obj).set_BackgroundColor(_debugColorsActive ? Color.get_Gray() : Color.get_Transparent());
			RenderEmptyLine((Panel)(object)flowPanel, 10);
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)flowPanel);
			((Control)val4).set_Width(((Container)flowPanel).get_ContentRegion().Width);
			((Container)val4).set_HeightSizingMode((SizingMode)2);
			((Panel)val4).set_CanScroll(true);
			val4.set_FlowDirection((ControlFlowDirection)3);
			val4.set_ControlPadding(new Vector2(0f, 10f));
			_listParent = val4;
			UpdateList();
		}

		private void SortList()
		{
			_getNow();
			_listParent.SortChildren<DataFlowPanel<EventListData>>((Comparison<DataFlowPanel<EventListData>>)delegate(DataFlowPanel<EventListData> a, DataFlowPanel<EventListData> b)
			{
				int num = a.Data.Occurrence.CompareTo(b.Data.Occurrence);
				return (num != 0) ? num : string.Compare(a.Data.Event.Name, b.Data.Event.Name, StringComparison.OrdinalIgnoreCase);
			});
		}

		private void RemoveEntry(DataFlowPanel<EventListData> child)
		{
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<DataFlowPanel<EventListData>>(child, (object)new
			{
				Opacity = 0f,
				Height = 0
			}, 1f, 0f, true).OnComplete((Action)delegate
			{
				((Control)child).set_Parent((Container)null);
				((Container)child).ClearChildren();
			});
		}

		private void UpdateList()
		{
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Expected O, but got Unknown
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0416: Unknown result type (might be due to invalid IL or missing references)
			//IL_041b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0423: Unknown result type (might be due to invalid IL or missing references)
			//IL_042a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_0453: Expected O, but got Unknown
			//IL_0473: Unknown result type (might be due to invalid IL or missing references)
			//IL_0478: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a2: Expected O, but got Unknown
			//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Expected O, but got Unknown
			//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0502: Unknown result type (might be due to invalid IL or missing references)
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_0523: Unknown result type (might be due to invalid IL or missing references)
			//IL_0530: Expected O, but got Unknown
			//IL_05af: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
			if (_listParent == null)
			{
				return;
			}
			Instant now = _getNow();
			foreach (DataFlowPanel<EventListData> child in ((Container)_listParent).GetChildrenOfType<DataFlowPanel<EventListData>>().ToList())
			{
				if (!(child.Data.Occurrence > now))
				{
					RemoveEntry(child);
				}
			}
			List<EventListData> eligibleEvents = GetEligibleEvents();
			AsyncTexture2D bookIcon = base.IconService.GetIcon("102353.png");
			List<DataFlowPanel<EventListData>> addedList = new List<DataFlowPanel<EventListData>>();
			int height2 = 70;
			foreach (EventListData e in eligibleEvents)
			{
				if (((Container)_listParent).GetChildrenOfType<DataFlowPanel<EventListData>>().Any((DataFlowPanel<EventListData> x) => x.Data.Event.SettingKey == e.Event.SettingKey && x.Data.Occurrence == e.Occurrence))
				{
					continue;
				}
				DataFlowPanel<EventListData> dataFlowPanel = new DataFlowPanel<EventListData>(e);
				((Control)dataFlowPanel).set_Width(((Container)_listParent).get_ContentRegion().Width - 20);
				((Panel)dataFlowPanel).set_CanScroll(false);
				((FlowPanel)dataFlowPanel).set_FlowDirection((ControlFlowDirection)2);
				((FlowPanel)dataFlowPanel).set_ControlPadding(new Vector2(10f, 0f));
				DataFlowPanel<EventListData> innerFlowPanel = dataFlowPanel;
				ZonedDateTime occurrenceInSystemTimezone = e.Occurrence.InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault());
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent((Container)(object)innerFlowPanel);
				((Container)val).set_HeightSizingMode((SizingMode)2);
				((Container)val).set_WidthSizingMode((SizingMode)1);
				val.set_FlowDirection((ControlFlowDirection)3);
				val.set_OuterControlPadding(new Vector2(10f, 0f));
				FlowPanel timeFlowPanel = val;
				int timeWidth = 120;
				UpdatingTextControl updatingTextControl = new UpdatingTextControl(() => occurrenceInSystemTimezone.ToString("HH:mm", CultureInfo.CurrentUICulture), () => GameService.Content.get_DefaultFont32(), () => Color.get_White());
				((Control)updatingTextControl).set_Parent((Container)(object)timeFlowPanel);
				((Control)updatingTextControl).set_Width(timeWidth);
				((Control)updatingTextControl).set_Height(height2 / 2);
				((Control)updatingTextControl).set_BackgroundColor(_debugColorsActive ? Color.get_Brown() : Color.get_Transparent());
				updatingTextControl.HorizontalAlignment = (HorizontalAlignment)1;
				UpdatingTextControl updatingTextControl2 = new UpdatingTextControl(() => (occurrenceInSystemTimezone - _getNow().InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault())).ToString("hh:mm:ss", CultureInfo.CurrentUICulture), () => GameService.Content.get_DefaultFont32(), () => (!(occurrenceInSystemTimezone - _getNow().InZone(DateTimeZoneProviders.Tzdb.GetSystemDefault()) < Duration.FromMinutes(5L))) ? Color.get_White() : Color.get_Red());
				((Control)updatingTextControl2).set_Parent((Container)(object)timeFlowPanel);
				((Control)updatingTextControl2).set_Width(timeWidth);
				((Control)updatingTextControl2).set_Height(height2 / 2);
				((Control)updatingTextControl2).set_BackgroundColor(_debugColorsActive ? Color.get_Green() : Color.get_Transparent());
				updatingTextControl2.HorizontalAlignment = (HorizontalAlignment)1;
				IconControl iconControl = new IconControl(base.IconService.GetIcon(e.Event.Icon), Textures.get_TransparentPixel());
				((Control)iconControl).set_Parent((Container)(object)innerFlowPanel);
				((Control)iconControl).set_Size(new Point(height2, height2));
				((Control)iconControl).set_BackgroundColor(_debugColorsActive ? Color.get_AliceBlue() : Color.get_Transparent());
				FormattedLabel nameLabel = new FormattedLabelBuilder().SetWidth(50).SetHeight(height2).SetVerticalAlignment((VerticalAlignment)1)
					.CreatePart(e.Event.Name, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
					{
						b.SetFontSize((FontSize)36);
					})
					.CreatePart("\n", (Action<FormattedLabelPartBuilder>)delegate
					{
					})
					.CreatePart(e.Event.Location ?? "", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
					{
						b.SetFontSize((FontSize)20);
					})
					.Build();
				((Control)nameLabel).set_Parent((Container)(object)innerFlowPanel);
				((Control)nameLabel).set_BackgroundColor(_debugColorsActive ? Color.get_Red() : Color.get_Transparent());
				IconControl iconControl2 = new IconControl(base.IconService.GetIcon("870380.png"), Textures.get_TransparentPixel());
				((Control)iconControl2).set_Parent((Container)(object)innerFlowPanel);
				((Control)iconControl2).set_Size(new Point(4, height2));
				IconControl actionSeparatorIcon = iconControl2;
				FlowPanel val2 = new FlowPanel();
				((Control)val2).set_Parent((Container)(object)innerFlowPanel);
				((Container)val2).set_WidthSizingMode((SizingMode)1);
				((Container)val2).set_HeightSizingMode((SizingMode)2);
				val2.set_FlowDirection((ControlFlowDirection)2);
				((Control)val2).set_BackgroundColor(_debugColorsActive ? Color.get_Blue() : Color.get_Transparent());
				FlowPanel actionFlowPanel = val2;
				if (!string.IsNullOrWhiteSpace(e.Event.Wiki))
				{
					GlowButton val3 = new GlowButton();
					val3.set_ToggleGlow(false);
					((Control)val3).set_Tooltip(new Tooltip((ITooltipView)(object)new TooltipView("Wiki", "Click to open wiki!", bookIcon, base.TranslationService)));
					val3.set_Icon(bookIcon);
					((Control)val3).set_Height(height2 - height2 / 3);
					((Control)val3).set_Width(height2 - height2 / 3);
					GlowButton wikiButton = val3;
					WrapActionButtonInPanel((Control)(object)wikiButton, ((Control)wikiButton).get_Width(), height2, (Container)(object)actionFlowPanel);
					((Control)wikiButton).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						if (!string.IsNullOrWhiteSpace(e.Event.Wiki))
						{
							Process.Start(e.Event.Wiki);
						}
					});
					GlowButton val4 = new GlowButton();
					val4.set_ToggleGlow(false);
					((Control)val4).set_BasicTooltipText("Copy waypoint");
					val4.set_Icon(base.IconService.GetIcon("156628.png"));
					((Control)val4).set_Height(height2 - height2 / 3);
					((Control)val4).set_Width(height2 - height2 / 3);
					GlowButton copyWaypointButton = val4;
					((Control)copyWaypointButton).add_Click((EventHandler<MouseEventArgs>)async delegate
					{
						try
						{
							string waypoint = e.Event.GetWaypoint(_accountService.Account);
							if (!string.IsNullOrWhiteSpace(waypoint))
							{
								await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(waypoint);
								ShowInfo("Copied waypoint.");
							}
						}
						catch (Exception exception)
						{
							_logger.Warn(exception, "Can't copy waypoint.");
							ShowError("Failed to copy waypoint.");
						}
					});
					WrapActionButtonInPanel((Control)(object)copyWaypointButton, ((Control)copyWaypointButton).get_Width(), height2, (Container)(object)actionFlowPanel);
				}
				((Control)innerFlowPanel).set_Height(0);
				((Control)innerFlowPanel).set_Opacity(0f);
				((Control)innerFlowPanel).set_Parent((Container)(object)_listParent);
				((Control)actionFlowPanel).DoUpdate(GameService.Overlay.get_CurrentGameTime());
				((Control)actionFlowPanel).RecalculateLayout();
				((Control)innerFlowPanel).DoUpdate(GameService.Overlay.get_CurrentGameTime());
				((Control)innerFlowPanel).RecalculateLayout();
				((Control)nameLabel).set_Width(((Container)innerFlowPanel).get_ContentRegion().Width - ((Control)nameLabel).get_Left() - ((Control)actionFlowPanel).get_Width() - ((Control)actionSeparatorIcon).get_Width() - (int)((FlowPanel)innerFlowPanel).get_ControlPadding().X * 3);
				addedList.Add(innerFlowPanel);
			}
			if (addedList.Count <= 0)
			{
				return;
			}
			SortList();
			foreach (DataFlowPanel<EventListData> added in addedList)
			{
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<DataFlowPanel<EventListData>>(added, (object)new
				{
					Height = height2
				}, 0.2f, 0f, true);
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<DataFlowPanel<EventListData>>(added, (object)new
				{
					Opacity = 1f
				}, 1f, 0f, true);
			}
			static void WrapActionButtonInPanel(Control ctrl, int width, int height, Container parent)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected O, but got Unknown
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				Panel val5 = new Panel();
				((Control)val5).set_Parent(parent);
				((Control)val5).set_Size(new Point(width, height));
				Panel panel = val5;
				ctrl.set_Parent((Container)(object)panel);
				ctrl.set_Location(new Point(((Container)panel).get_ContentRegion().Width / 2 - ctrl.get_Width() / 2, ((Container)panel).get_ContentRegion().Height / 2 - ctrl.get_Height() / 2));
			}
		}

		private void InnerFlowPanelOnClick(object sender, MouseEventArgs e)
		{
		}

		private List<EventListData> GetEligibleEvents()
		{
			Instant now = _getNow();
			Instant maxTime = now.Plus(Duration.FromHours(6));
			return (from x in _getAllEvents().SelectMany((EventCategory x) => x.Events)
				where !x.Filler
				select x).SelectMany((Event x) => from start in x.Occurences
				where start > now && start < maxTime
				select start into o
				select new EventListData
				{
					Event = x,
					Occurrence = o
				}).ToList();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			UpdateUtil.Update(UpdateList, gameTime, _updateListInterval.TotalMilliseconds, ref _lastListUpdate);
		}

		protected override void Unload()
		{
			base.Unload();
			FlowPanel listParent = _listParent;
			if (listParent != null)
			{
				((Container)listParent).ClearChildren();
			}
			_listParent = null;
			StandardWindow settingsWindow = _settingsWindow;
			if (settingsWindow != null)
			{
				((Control)settingsWindow).Dispose();
			}
			_settingsWindow = null;
		}
	}
}
