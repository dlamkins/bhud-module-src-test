using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estreya.BlishHUD.Shared.Contexts;
using Estreya.BlishHUD.Shared.Threading.Events;

namespace Estreya.BlishHUD.EventTable.Contexts
{
	public class EventTableContext : BaseContext
	{
		internal event AsyncEventHandler<ContextEventArgs<AddCategory>> RequestAddCategory;

		internal event AsyncEventHandler<ContextEventArgs<string>> RequestRemoveCategory;

		internal event AsyncEventHandler<ContextEventArgs<AddEvent>> RequestAddEvent;

		internal event AsyncEventHandler<ContextEventArgs<RemoveEvent>> RequestRemoveEvent;

		internal event AsyncEventHandler<ContextEventArgs> RequestReloadEvents;

		internal event AsyncEventHandler<ContextEventArgs<ShowReminder>> RequestShowReminder;

		internal event AsyncEventHandler<ContextEventArgs<AddDynamicEvent>> RequestAddDynamicEvent;

		internal event AsyncEventHandler<ContextEventArgs<Guid>> RequestRemoveDynamicEvent;

		internal event AsyncReturnEventHandler<ContextEventArgs, IEnumerable<string>> RequestEventSettingKeys;

		internal event AsyncReturnEventHandler<ContextEventArgs, IEnumerable<string>> RequestAreaNames;

		internal event AsyncEventHandler<ContextEventArgs<AddEventState>> RequestAddEventState;

		internal event AsyncEventHandler<ContextEventArgs<RemoveEventState>> RequestRemoveEventState;

		public async Task AddCategory(AddCategory newCategory)
		{
			CheckReady();
			Type caller = GetCaller();
			base.Logger.Info("\"" + caller.FullName + "\" triggered a context action: AddCategory(\"" + newCategory.Name + " (" + newCategory.Key + ")\").");
			await (this.RequestAddCategory?.Invoke(this, new ContextEventArgs<AddCategory>(caller, newCategory)) ?? Task.FromException(new NotImplementedException()));
		}

		public async Task RemoveCategory(string key)
		{
			CheckReady();
			Type caller = GetCaller();
			base.Logger.Info("\"" + caller.FullName + "\" triggered a context action: RemoveCategory(\"" + key + "\").");
			await (this.RequestRemoveCategory?.Invoke(this, new ContextEventArgs<string>(caller, key)) ?? Task.FromException(new NotImplementedException()));
		}

		public async Task AddEvent(AddEvent newEvent)
		{
			CheckReady();
			Type caller = GetCaller();
			base.Logger.Info("\"" + caller.FullName + "\" triggered a context action: AddEvent(\"" + newEvent.CategoryKey + "\", \"" + newEvent.Name + " (" + newEvent.Key + ")\").");
			await (this.RequestAddEvent?.Invoke(this, new ContextEventArgs<AddEvent>(caller, newEvent)) ?? Task.FromException(new NotImplementedException()));
		}

		public async Task RemoveEvent(RemoveEvent removeEvent)
		{
			CheckReady();
			Type caller = GetCaller();
			base.Logger.Info("\"" + caller.FullName + "\" triggered a context action: RemoveEvent(\"" + removeEvent.CategoryKey + "\", \"" + removeEvent.EventKey + "\").");
			await (this.RequestRemoveEvent?.Invoke(this, new ContextEventArgs<RemoveEvent>(caller, removeEvent)) ?? Task.FromException(new NotImplementedException()));
		}

		public async Task ReloadEvents()
		{
			CheckReady();
			Type caller = GetCaller();
			base.Logger.Info("\"" + caller.FullName + "\" triggered a context action: ReloadEvents().");
			await (this.RequestReloadEvents?.Invoke(this, new ContextEventArgs(caller)) ?? Task.FromException(new NotImplementedException()));
		}

		public async Task ShowReminder(ShowReminder reminder)
		{
			CheckReady();
			Type caller = GetCaller();
			base.Logger.Info("\"" + caller.FullName + "\" triggered a context action: ShowReminder().");
			await (this.RequestShowReminder?.Invoke(this, new ContextEventArgs<ShowReminder>(caller, reminder)) ?? Task.FromException(new NotImplementedException()));
		}

		public async Task AddDynamicEvent(AddDynamicEvent addDynamicEvent)
		{
			CheckReady();
			Type caller = GetCaller();
			base.Logger.Info(string.Format("\"{0}\" triggered a context action: {1}(\"{2} ({3})\").", caller.FullName, "AddDynamicEvent", addDynamicEvent.Name, addDynamicEvent.Id));
			await (this.RequestAddDynamicEvent?.Invoke(this, new ContextEventArgs<AddDynamicEvent>(caller, addDynamicEvent)) ?? Task.FromException(new NotImplementedException()));
		}

		public async Task RemoveDynamicEvent(Guid id)
		{
			CheckReady();
			Type caller = GetCaller();
			base.Logger.Info(string.Format("\"{0}\" triggered a context action: {1}(\"{2}\").", caller.FullName, "RemoveDynamicEvent", id));
			await (this.RequestRemoveDynamicEvent?.Invoke(this, new ContextEventArgs<Guid>(caller, id)) ?? Task.FromException(new NotImplementedException()));
		}

		public async Task<IEnumerable<string>> GetEventSettingKeys()
		{
			CheckReady();
			Type caller = GetCaller();
			base.Logger.Info("\"" + caller.FullName + "\" triggered a context action: GetEventSettingKeys().");
			if (this.RequestEventSettingKeys == null)
			{
				throw new NotImplementedException();
			}
			return await this.RequestEventSettingKeys(this, new ContextEventArgs(caller));
		}

		public async Task<IEnumerable<string>> GetAreaNames()
		{
			CheckReady();
			Type caller = GetCaller();
			base.Logger.Info("\"" + caller.FullName + "\" triggered a context action: GetAreaNames().");
			if (this.RequestAreaNames == null)
			{
				throw new NotImplementedException();
			}
			return await this.RequestAreaNames(this, new ContextEventArgs(caller));
		}

		public async Task AddEventState(AddEventState addEventState)
		{
			CheckReady();
			Type caller = GetCaller();
			base.Logger.Info(string.Format("\"{0}\" triggered a context action: {1}(\"{2}\",\"{3}\",\"{4}\").", caller.FullName, "AddEventState", addEventState.AreaName, addEventState.EventKey, addEventState.Until.ToUniversalTime()));
			await (this.RequestAddEventState?.Invoke(this, new ContextEventArgs<AddEventState>(caller, addEventState)) ?? Task.FromException(new NotImplementedException()));
		}

		public async Task RemoveEventState(RemoveEventState removeEventState)
		{
			CheckReady();
			Type caller = GetCaller();
			base.Logger.Info("\"" + caller.FullName + "\" triggered a context action: RemoveEventState(\"" + removeEventState.AreaName + "\",\"" + removeEventState.EventKey + "\").");
			await (this.RequestRemoveEventState?.Invoke(this, new ContextEventArgs<RemoveEventState>(caller, removeEventState)) ?? Task.FromException(new NotImplementedException()));
		}
	}
}
