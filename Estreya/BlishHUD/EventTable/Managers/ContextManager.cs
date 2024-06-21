using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Estreya.BlishHUD.EventTable.Contexts;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.EventTable.Models.Reminders;
using Estreya.BlishHUD.EventTable.Services;
using Estreya.BlishHUD.Shared.Contexts;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Services.Audio;
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

		private readonly EventStateService _eventStateService;

		private readonly AudioService _audioService;

		private readonly Func<Task<IEnumerable<Estreya.BlishHUD.EventTable.Models.Event>>> _getEvents;

		public event AsyncEventHandler ReloadEvents;

		public ContextManager(EventTableContext context, ModuleSettings moduleSettings, DynamicEventService dynamicEventService, IconService iconService, EventStateService eventStateService, AudioService audioService, Func<Task<IEnumerable<Estreya.BlishHUD.EventTable.Models.Event>>> getEvents)
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
			_eventStateService = eventStateService;
			_audioService = audioService;
			_getEvents = getEvents;
			_context.RequestAddCategory += RequestAddCategory;
			_context.RequestAddEvent += RequestAddEvent;
			_context.RequestRemoveCategory += RequestRemoveCategory;
			_context.RequestRemoveEvent += RequestRemoveEvent;
			_context.RequestReloadEvents += RequestReloadEvents;
			_context.RequestShowReminder += RequestShowReminder;
			_context.RequestAddDynamicEvent += RequestAddDynamicEvent;
			_context.RequestRemoveDynamicEvent += RequestRemoveDynamicEvent;
			_context.RequestEventSettingKeys += RequestEventSettingKeys;
			_context.RequestAreaNames += RequestAreaNames;
			_context.RequestAddEventState += RequestAddEventState;
			_context.RequestRemoveEventState += RequestRemoveEventState;
		}

		public List<EventCategory> GetContextCategories()
		{
			using (_eventLock.Lock())
			{
				return _temporaryEventCategories;
			}
		}

		private Task RequestRemoveEventState(object sender, ContextEventArgs<RemoveEventState> e)
		{
			_eventStateService.Remove(e.Content.AreaName, e.Content.EventKey);
			return Task.CompletedTask;
		}

		private Task RequestAddEventState(object sender, ContextEventArgs<AddEventState> e)
		{
			_eventStateService.Add(e.Content.AreaName, e.Content.EventKey, e.Content.Until, e.Content.State);
			return Task.CompletedTask;
		}

		private async Task<IEnumerable<string>> RequestEventSettingKeys(object sender, ContextEventArgs e)
		{
			if (_getEvents == null)
			{
				throw new ArgumentNullException("_getEvents", "Method to get events is null.");
			}
			IEnumerable<Estreya.BlishHUD.EventTable.Models.Event> events = await _getEvents();
			return (events == null) ? Enumerable.Empty<string>() : events.Select((Estreya.BlishHUD.EventTable.Models.Event e) => e.SettingKey);
		}

		private Task<IEnumerable<string>> RequestAreaNames(object sender, ContextEventArgs e)
		{
			List<string> areaNames = _moduleSettings.EventAreaNames.get_Value();
			IEnumerable<string> result;
			if (areaNames != null)
			{
				IEnumerable<string> enumerable = areaNames;
				result = enumerable;
			}
			else
			{
				result = Enumerable.Empty<string>();
			}
			return Task.FromResult(result);
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

		private async Task RequestShowReminder(object sender, ContextEventArgs<ShowReminder> e)
		{
			ShowReminder eArgsContent = e.Content;
			AsyncTexture2D icon = ((!string.IsNullOrWhiteSpace(eArgsContent.Icon)) ? _iconService.GetIcon(eArgsContent.Icon) : null);
			ReminderType value = _moduleSettings.ReminderType.get_Value();
			if ((value == ReminderType.Control || value == ReminderType.Both) ? true : false)
			{
				EventNotification.ShowAsControl(null, eArgsContent.Title, eArgsContent.Message, icon, _iconService, _moduleSettings);
				await EventNotification.PlaySound(_audioService);
			}
			value = _moduleSettings.ReminderType.get_Value();
			if ((value == ReminderType.Windows || value == ReminderType.Both) ? true : false)
			{
				await EventNotification.ShowAsWindowsNotification(eArgsContent.Title, eArgsContent.Message, icon);
			}
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
					Waypoints = eArgsContent.Waypoints,
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
				_context.RequestEventSettingKeys -= RequestEventSettingKeys;
				_context.RequestAreaNames -= RequestAreaNames;
				_context.RequestAddEventState -= RequestAddEventState;
				_context.RequestRemoveEventState -= RequestRemoveEventState;
			}
			_context = null;
		}
	}
}
