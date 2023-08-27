using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Characters.Views;
using Kenedia.Modules.Core.Models;
using Newtonsoft.Json;

namespace Kenedia.Modules.Characters.Models
{
	[DataContract]
	public class StaticInfo
	{
		private bool _isBetaUpcoming = true;

		private bool _isBeta;

		public static string Url = "https://bhm.blishhud.com/Kenedia.Modules.Characters/static_info.json";

		public static string githubUrl = "https://raw.githubusercontent.com/KenediaDev/Kenedia.BlishHUD/bhud-static/Kenedia.Modules.Characters/static_info.json";

		[DataMember]
		public DateTime BetaStart { get; private set; }

		[DataMember]
		public DateTime BetaEnd { get; private set; }

		public bool IsBeta
		{
			get
			{
				if (DateTime.UtcNow >= BetaStart)
				{
					return DateTime.UtcNow < BetaEnd;
				}
				return false;
			}
		}

		public event EventHandler<bool> BetaStateChanged;

		public StaticInfo(DateTime betaStart, DateTime betaEnd)
		{
			BetaStart = betaStart;
			BetaEnd = betaEnd;
		}

		public static async Task<StaticInfo> GetStaticInfo()
		{
			HttpClient httpClient = new HttpClient();
			try
			{
				return JsonConvert.DeserializeObject<StaticInfo>(await httpClient.GetStringAsync(Url));
			}
			finally
			{
				((IDisposable)httpClient)?.Dispose();
			}
		}

		public void CheckBeta()
		{
			if (_isBetaUpcoming)
			{
				_isBetaUpcoming = DateTime.UtcNow < BetaEnd;
				if (_isBeta != IsBeta)
				{
					_isBeta = IsBeta;
					BaseModule<Characters, MainWindow, Settings, PathCollection>.Logger.Debug("Beta has " + (IsBeta ? "started." : "ended."));
					this.BetaStateChanged?.Invoke(this, _isBeta);
				}
			}
		}
	}
}
