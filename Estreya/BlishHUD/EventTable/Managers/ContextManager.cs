using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Estreya.BlishHUD.EventTable.Contexts;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Services;
using Estreya.BlishHUD.Shared.Contexts;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Threading.Events;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.Managers
{
	public class ContextManager : IDisposable, IUpdatable
	{
		private static Logger _logger = Logger.GetLogger<ContextManager>();

		private List<EventCategory> _temporaryEventCategories = new List<EventCategory>();

		private AsyncLock _eventLock = new AsyncLock();

		private EventTableContext _context;

		private readonly ModuleSettings _moduleSettings;

		private readonly DynamicEventService _dynamicEventService;

		private readonly IconService _iconService;

		public event AsyncEventHandler ReloadEvents;

		public ContextManager(EventTableContext context, ModuleSettings moduleSettings, DynamicEventService dynamicEventService, IconService iconService)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (moduleSettings == null)
			{
				throw new ArgumentNullException("moduleSettings");
			}
			if (dynamicEventService == null)
			{
				throw new ArgumentNullException("dynamicEventService");
			}
			if (iconService == null)
			{
				throw new ArgumentNullException("iconService");
			}
			_context = context;
			_moduleSettings = moduleSettings;
			_dynamicEventService = dynamicEventService;
			_iconService = iconService;
			_context.RequestAddCategory += RequestAddCategory;
			_context.RequestAddEvent += RequestAddEvent;
			_context.RequestRemoveCategory += RequestRemoveCategory;
			_context.RequestRemoveEvent += RequestRemoveEvent;
			_context.RequestReloadEvents += RequestReloadEvents;
			_context.RequestShowReminder += RequestShowReminder;
			_context.RequestAddDynamicEvent += RequestAddDynamicEvent;
			_context.RequestRemoveDynamicEvent += RequestRemoveDynamicEvent;
		}

		public List<EventCategory> GetContextCategories()
		{
			using (_eventLock.Lock())
			{
				return _temporaryEventCategories;
			}
		}

		private async Task RequestRemoveDynamicEvent(object sender, ContextEventArgs<Guid> e)
		{
			await _dynamicEventService.RemoveCustomEvent(e.Content.ToString());
			await _dynamicEventService.NotifyCustomEventsUpdated();
		}

		private async Task RequestAddDynamicEvent(object sender, ContextEventArgs<AddDynamicEvent> e)
		{
			AddDynamicEvent eArgsContent = e.Content;
			await _dynamicEventService.AddCustomEvent(new DynamicEvent
			{
				ID = eArgsContent.Id.ToString(),
				Name = eArgsContent.Name,
				MapId = eArgsContent.MapId,
				ColorCode = eArgsContent.ColorCode,
				Flags = eArgsContent.Flags,
				Icon = ((!eArgsContent.Icon.HasValue) ? null : new DynamicEvent.DynamicEventIcon
				{
					FileID = eArgsContent.Icon.Value.FileID,
					Signature = eArgsContent.Icon.Value.Signature
				}),
				Level = eArgsContent.Level,
				Location = ((!eArgsContent.Location.HasValue) ? null : new DynamicEvent.DynamicEventLocation
				{
					Center = eArgsContent.Location.Value.Center,
					Height = eArgsContent.Location.Value.Height,
					Points = eArgsContent.Location.Value.Points,
					Radius = eArgsContent.Location.Value.Radius,
					Rotation = eArgsContent.Location.Value.Rotation,
					Type = eArgsContent.Location.Value.Type,
					ZRange = eArgsContent.Location.Value.ZRange
				})
			});
			await _dynamicEventService.NotifyCustomEventsUpdated();
		}

		private Task RequestShowReminder(object sender, ContextEventArgs<ShowReminder> e)
		{
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			ShowReminder eArgsContent = e.Content;
			EventNotification eventNotification = new EventNotification(null, eArgsContent.Title, eArgsContent.Message, (!string.IsNullOrWhiteSpace(eArgsContent.Icon)) ? _iconService.GetIcon(eArgsContent.Icon) : null, _moduleSettings.ReminderPosition.X.get_Value(), _moduleSettings.ReminderPosition.Y.get_Value(), _moduleSettings.ReminderSize.X.get_Value(), _moduleSettings.ReminderSize.Y.get_Value(), _moduleSettings.ReminderSize.Icon.get_Value(), _moduleSettings.ReminderStackDirection.get_Value(), _moduleSettings.ReminderFonts.TitleSize.get_Value(), _moduleSettings.ReminderFonts.MessageSize.get_Value(), _iconService, _moduleSettings.ReminderLeftClickAction.get_Value() != LeftClickAction.None);
			eventNotification.BackgroundOpacity = _moduleSettings.ReminderOpacity.get_Value();
			eventNotification.Show(TimeSpan.FromSeconds(_moduleSettings.ReminderDuration.get_Value()));
			return Task.CompletedTask;
		}

		private async Task RequestReloadEvents(object sender, ContextEventArgs e)
		{
			await (this.ReloadEvents?.Invoke(this) ?? Task.FromException(new NotImplementedException()));
		}

		private async Task RequestRemoveEvent(object sender, ContextEventArgs<RemoveEvent> e)
		{
			RemoveEvent eArgsContent = e.Content;
			using (await _eventLock.LockAsync())
			{
				EventCategory obj = _temporaryEventCategories.FirstOrDefault((EventCategory ec) => ec.Key == eArgsContent.CategoryKey) ?? throw new ArgumentException("Category with key \"" + eArgsContent.CategoryKey + "\" does not exist.");
				if (!obj.Events.Any((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.Key == eArgsContent.EventKey))
				{
					throw new ArgumentException("Event with the key \"" + eArgsContent.EventKey + "\" does not exist.");
				}
				obj.UpdateOriginalEvents(obj.OriginalEvents.Where((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.Key != eArgsContent.EventKey).ToList());
				obj.UpdateFillers(obj.FillerEvents.Where((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.Key != eArgsContent.EventKey).ToList());
				_logger.Info("Event \"" + eArgsContent.EventKey + "\" of category \"" + eArgsContent.CategoryKey + "\" was removed via context.");
			}
		}

		private async Task RequestRemoveCategory(object sender, ContextEventArgs<string> e)
		{
			using (await _eventLock.LockAsync())
			{
				EventCategory category = _temporaryEventCategories.FirstOrDefault((EventCategory ec) => ec.Key == e.Content) ?? throw new ArgumentException("Category with key \"" + e.Content + "\" does not exist.");
				_temporaryEventCategories.Remove(category);
				_logger.Info("Category \"" + category.Name + "\" (" + category.Key + ") was removed via context.");
			}
		}

		private async Task RequestAddEvent(object sender, ContextEventArgs<AddEvent> e)
		{
			AddEvent eArgsContent = e.Content;
			using (await _eventLock.LockAsync())
			{
				EventCategory category = _temporaryEventCategories.FirstOrDefault((EventCategory ec) => ec.Key == eArgsContent.CategoryKey) ?? throw new ArgumentException("Category with key \"" + eArgsContent.Key + "\" does not exist.");
				if (category.Events.Any((Estreya.BlishHUD.EventTable.Models.Event ev) => ev.Key == eArgsContent.Key))
				{
					throw new ArgumentException("Event with the key \"" + eArgsContent.Key + "\" already exists.");
				}
				Estreya.BlishHUD.EventTable.Models.Event newEvent = new Estreya.BlishHUD.EventTable.Models.Event
				{
					Key = eArgsContent.Key,
					Name = eArgsContent.Name,
					APICode = eArgsContent.APICode,
					APICodeType = eArgsContent.APICodeType,
					BackgroundColorCode = eArgsContent.BackgroundColorCode,
					BackgroundColorGradientCodes = eArgsContent.BackgroundColorGradientCodes,
					Duration = eArgsContent.Duration,
					Filler = eArgsContent.Filler,
					Icon = eArgsContent.Icon,
					Location = eArgsContent.Location,
					MapIds = eArgsContent.MapIds,
					Offset = eArgsContent.Offset,
					Repeat = eArgsContent.Repeat,
					StartingDate = eArgsContent.StartingDate,
					Waypoint = eArgsContent.Waypoint,
					Wiki = eArgsContent.Wiki
				};
				if (eArgsContent.Occurences != null)
				{
					newEvent.Occurences.AddRange(eArgsContent.Occurences);
				}
				if (eArgsContent.ReminderTimes != null)
				{
					newEvent.UpdateReminderTimes(eArgsContent.ReminderTimes);
				}
				if (newEvent.Filler)
				{
					category.UpdateFillers(new List<Estreya.BlishHUD.EventTable.Models.Event>(category.FillerEvents) { newEvent });
				}
				else
				{
					category.UpdateOriginalEvents(new List<Estreya.BlishHUD.EventTable.Models.Event>(category.OriginalEvents) { newEvent });
				}
				_logger.Info("Event \"" + eArgsContent.Name + "\" (" + eArgsContent.Key + ") of category \"" + category.Name + "\" (" + category.Key + ") was registered via context.");
			}
		}

		private async Task RequestAddCategory(object sender, ContextEventArgs<AddCategory> e)
		{
			AddCategory eArgsContent = e.Content;
			using (await _eventLock.LockAsync())
			{
				if (_temporaryEventCategories.Any((EventCategory ec) => ec.Key == eArgsContent.Key))
				{
					throw new ArgumentException("Category with key \"" + eArgsContent.Key + "\" already exists.");
				}
				_temporaryEventCategories.Add(new EventCategory
				{
					Key = eArgsContent.Key,
					Name = eArgsContent.Name,
					Icon = eArgsContent.Icon,
					ShowCombined = eArgsContent.ShowCombined,
					FromContext = true
				});
				_logger.Info("Category \"" + eArgsContent.Name + "\" (" + eArgsContent.Key + ") was registered via context.");
			}
		}

		public void Update(GameTime gameTime)
		{
		}

		public void Dispose()
		{
			using (_eventLock.Lock())
			{
				_temporaryEventCategories.Clear();
				_temporaryEventCategories = null;
			}
			if (_context != null)
			{
				_context.RequestAddCategory -= RequestAddCategory;
				_context.RequestAddEvent -= RequestAddEvent;
				_context.RequestRemoveCategory -= RequestRemoveCategory;
				_context.RequestRemoveEvent -= RequestRemoveEvent;
				_context.RequestReloadEvents -= RequestReloadEvents;
				_context.RequestShowReminder -= RequestShowReminder;
				_context.RequestAddDynamicEvent -= RequestAddDynamicEvent;
				_context.RequestRemoveDynamicEvent -= RequestRemoveDynamicEvent;
			}
			_context = null;
		}
	}
}
