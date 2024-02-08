using System;
using System.Threading.Tasks;
using Blish_HUD;

namespace MysticCrafting.Module.Repositories.Logging
{
	public class LoggingRepository : IRepository
	{
		private static readonly Logger Logger = Logger.GetLogger<IRepository>();

		private readonly IRepository _repository;

		public string FileName => _repository.FileName;

		public bool Loaded => _repository.Loaded;

		public bool LocalOnly => _repository.LocalOnly;

		public LoggingRepository(IRepository repository)
		{
			_repository = repository;
		}

		public async Task<string> LoadAsync()
		{
			string repositoryName = _repository.GetType().Name;
			Logger.Info("Repository '" + repositoryName + "' has started loading...");
			try
			{
				string result = await _repository.LoadAsync();
				Logger.Info("Repository '" + repositoryName + "' loaded successfully with result: " + result);
				return result;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Repository '" + repositoryName + "' loading failed with exception: " + ex.Message);
			}
			return "Repository '" + repositoryName + "' did not loaded.";
		}
	}
}
