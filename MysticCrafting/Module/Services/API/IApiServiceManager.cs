using System.Collections.Generic;
using System.Threading.Tasks;

namespace MysticCrafting.Module.Services.API
{
	public interface IApiServiceManager
	{
		Task LoadServicesAsync();

		void RegisterService(IApiService service);

		List<IApiService> GetServices();
	}
}
