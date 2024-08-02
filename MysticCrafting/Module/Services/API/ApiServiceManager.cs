using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MysticCrafting.Module.Services.API
{
	public class ApiServiceManager : IApiServiceManager
	{
		public List<IApiService> RecurringServices { get; set; } = new List<IApiService>();


		public async Task LoadServicesAsync()
		{
			List<Task> tasks = new List<Task>();
			foreach (IApiService service in RecurringServices)
			{
				if (DateTime.Now > service.LastLoaded.AddMinutes(service.ExecutionIntervalMinutes))
				{
					tasks.Add(service.StartTimedLoadingAsync(service.ExecutionIntervalMinutes));
				}
			}
			await Task.WhenAll(tasks);
		}

		public void RegisterService(IApiService service)
		{
			if (!RecurringServices.Any((IApiService s) => s.Name.Equals(service.Name)))
			{
				RecurringServices.Add(service);
			}
		}

		public List<IApiService> GetServices()
		{
			return RecurringServices;
		}
	}
}
