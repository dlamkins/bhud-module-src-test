using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using KpRefresher.Domain;
using KpRefresher.Extensions;
using KpRefresher.Ressources;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KpRefresher.Services
{
	public class KpMeService
	{
		private readonly Logger _logger;

		private const string _kpMeBaseUrl = "https://killproof.me/";

		public KpMeService(Logger logger)
		{
			_logger = logger;
		}

		public async Task<KpApiModel> GetAccountData(string kpId, bool showNotification = true)
		{
			if (string.IsNullOrWhiteSpace(kpId))
			{
				return null;
			}
			try
			{
				string url = "https://killproof.me/api/kp/" + kpId + "?lang=en";
				_logger.Info("[KpRefresher] Calling " + url);
				HttpClient client = new HttpClient();
				try
				{
					HttpResponseMessage response = await client.GetAsync(url);
					if (response != null)
					{
						if (response.get_StatusCode() == HttpStatusCode.OK)
						{
							return JsonConvert.DeserializeObject<KpApiModel>(await response.get_Content().ReadAsStringAsync());
						}
						if (response.get_StatusCode() == HttpStatusCode.NotFound && showNotification)
						{
							ScreenNotification.ShowNotification(string.Format(strings.Notification_KpAccountUnknown, kpId), (NotificationType)2, (Texture2D)null, 4);
						}
						else
						{
							_logger.Warn($"Unknown status while getting account data : {response.get_StatusCode()}");
						}
					}
				}
				finally
				{
					((IDisposable)client)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				_logger.Warn("Error while getting account info : " + ex.Message);
			}
			return null;
		}

		public async Task<List<RaidBoss>> GetClearData(string kpId)
		{
			if (string.IsNullOrWhiteSpace(kpId))
			{
				return null;
			}
			try
			{
				string url = "https://killproof.me/api/clear/" + kpId;
				_logger.Info("[KpRefresher] Calling " + url);
				HttpClient client = new HttpClient();
				try
				{
					List<RaidBoss> res = new List<RaidBoss>();
					HttpResponseMessage response = await client.GetAsync(url);
					if (response != null)
					{
						if (response.get_StatusCode() == HttpStatusCode.OK)
						{
							foreach (KeyValuePair<string, JToken> item in JObject.Parse(await response.get_Content().ReadAsStringAsync()))
							{
								foreach (JToken item2 in (IEnumerable<JToken>)item.Value)
								{
									JToken first = item2.get_First();
									string encounterName = ((JProperty)first).get_Name();
									if ((bool)((JProperty)first).get_Value())
									{
										res.Add(encounterName.GetValueFromName<RaidBoss>());
									}
								}
							}
							return res;
						}
						if (response.get_StatusCode() == HttpStatusCode.NotFound)
						{
							ScreenNotification.ShowNotification(string.Format(strings.Notification_KpAccountUnknown, kpId), (NotificationType)2, (Texture2D)null, 4);
						}
						else
						{
							_logger.Warn($"Unknown status while getting clear data : {response.get_StatusCode()}");
						}
					}
				}
				finally
				{
					((IDisposable)client)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				_logger.Warn("Error while refreshing kp.me : " + ex.Message);
			}
			return null;
		}

		public async Task<bool?> RefreshApi(string kpId)
		{
			if (string.IsNullOrWhiteSpace(kpId))
			{
				return null;
			}
			try
			{
				string url = "https://killproof.me/proof/" + kpId + "/refresh";
				_logger.Info("[KpRefresher] Calling " + url);
				HttpClient client = new HttpClient();
				try
				{
					HttpResponseMessage response = await client.GetAsync(url);
					if (response != null)
					{
						if (response.get_StatusCode() == HttpStatusCode.OK)
						{
							return true;
						}
						if (response.get_StatusCode() == HttpStatusCode.NotModified)
						{
							return false;
						}
						if (response.get_StatusCode() == HttpStatusCode.Forbidden)
						{
							ScreenNotification.ShowNotification(strings.Notification_KpAccountAnonymous, (NotificationType)2, (Texture2D)null, 4);
						}
						else if (response.get_StatusCode() == HttpStatusCode.NotFound)
						{
							ScreenNotification.ShowNotification(string.Format(strings.Notification_KpAccountUnknown, kpId), (NotificationType)2, (Texture2D)null, 4);
						}
						else
						{
							_logger.Warn($"Unknown status while refreshing kp.me : {response.get_StatusCode()}");
						}
					}
				}
				finally
				{
					((IDisposable)client)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				_logger.Warn("Error while refreshing kp.me : " + ex.Message);
			}
			return null;
		}

		public string GetBaseUrl()
		{
			return "https://killproof.me/";
		}
	}
}
