using System;
using System.Threading.Tasks;
using Blish_HUD;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class StaticHosting : Kenedia.Modules.Core.Services.StaticHosting
	{
		public override string BaseUrl { get; } = "https://bhm.blishhud.com/Kenedia.Modules.BuildsManager/";


		public StaticHosting(Logger logger)
			: base(logger)
		{
		}

		public async Task<StaticStats?> GetStaticStats()
		{
			try
			{
				return await GetStaticContent<StaticStats>("Stats.json");
			}
			catch (Exception ex)
			{
				base.Logger.Warn($"{ex}");
			}
			return null;
		}

		public async Task<StaticVersion?> GetStaticVersion()
		{
			try
			{
				return await GetStaticContent<StaticVersion>("DataMap.json");
			}
			catch (Exception ex)
			{
				base.Logger.Warn($"{ex}");
			}
			return null;
		}
	}
}
