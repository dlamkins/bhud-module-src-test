using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.WebApi;
using KillProofModule.Models;
using Newtonsoft.Json;

namespace KillProofModule.Manager
{
	public class KillProofApi
	{
		private const string KILLPROOF_API_URL = "https://killproof.me/api/";

		private const int RefreshProofSec = 3600;

		private const int RefreshClearSec = 300;

		private static Locale LastLocale;

		private static List<KillProof> _cachedKillProofs;

		static KillProofApi()
		{
			_cachedKillProofs = new List<KillProof>();
		}

		public static async Task<Resources> LoadResources()
		{
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			return await TaskUtil.GetJsonResponse<Resources>("https://killproof.me/api/resources?lang=" + ((object)(Locale)(ref value)).ToString()).ContinueWith(delegate(Task<(bool, Resources)> result)
			{
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Expected O, but got Unknown
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				if (!result.IsCompleted || !result.Result.Item1)
				{
					using (Stream stream = KillProofModule.ModuleInstance.ContentsManager.GetFileStream("resources.json"))
					{
						stream.Position = 0L;
						JsonTextReader val = new JsonTextReader((TextReader)new StreamReader(stream));
						try
						{
							return new JsonSerializer().Deserialize<Resources>((JsonReader)(object)val);
						}
						finally
						{
							((IDisposable)val)?.Dispose();
						}
					}
				}
				return result.Result.Item2;
			});
		}

		public static async Task<bool> ProfileAvailable(string account)
		{
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			var (responseSuccess, optionalKillProof) = await TaskUtil.GetJsonResponse<KillProof>("https://killproof.me/api/kp/" + account + "?lang=" + ((object)(Locale)(ref value)).ToString());
			return responseSuccess && optionalKillProof != null && string.IsNullOrEmpty(optionalKillProof.Error);
		}

		public static async Task<KillProof> GetKillProofContent(string account)
		{
			KillProof killproof = _cachedKillProofs.FirstOrDefault(delegate(KillProof kp)
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				string accountName = kp.AccountName;
				string text2 = account;
				Locale value2 = GameService.Overlay.get_UserLocale().get_Value();
				return accountName.Equals(text2 + ((object)(Locale)(ref value2)).ToString(), StringComparison.InvariantCultureIgnoreCase) || kp.KpId.Equals(account, StringComparison.InvariantCulture);
			});
			if (killproof != null && string.IsNullOrEmpty(killproof.Error))
			{
				if (LastLocale == GameService.Overlay.get_UserLocale().get_Value())
				{
					if (DateTime.Now.Subtract(killproof.LastRefresh).TotalSeconds < 3600.0)
					{
						return killproof;
					}
				}
				else
				{
					_cachedKillProofs.Remove(killproof);
					LastLocale = GameService.Overlay.get_UserLocale().get_Value();
				}
			}
			string text = account;
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			var (responseSuccess, newKillproof) = await TaskUtil.GetJsonResponse<KillProof>("https://killproof.me/api/kp/" + text + "?lang=" + ((object)(Locale)(ref value)).ToString()).ConfigureAwait(continueOnCapturedContext: false);
			if (responseSuccess && string.IsNullOrEmpty(newKillproof?.Error))
			{
				_cachedKillProofs.Add(newKillproof);
				return newKillproof;
			}
			return null;
		}
	}
}
