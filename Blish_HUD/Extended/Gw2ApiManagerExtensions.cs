using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Blish_HUD.Controls;
using Blish_HUD.Extended.Properties;
using Blish_HUD.Modules.Managers;
using Flurl.Http;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Graphics;

namespace Blish_HUD.Extended
{
	public static class Gw2ApiManagerExtensions
	{
		public static bool IsApiAvailable(this Gw2ApiManager gw2ApiManager, bool showMessage = false)
		{
			string l_err = string.Empty;
			try
			{
				HttpResponseMessage response = GeneratedExtensions.GetAsync(SettingsExtensions.AllowHttpStatus<IFlurlRequest>(GeneratedExtensions.AllowHttpStatus("https://api.guildwars2.com/", new HttpStatusCode[1] { HttpStatusCode.ServiceUnavailable }), new HttpStatusCode[1] { HttpStatusCode.InternalServerError }), default(CancellationToken), (HttpCompletionOption)1).Result;
				try
				{
					if (response.get_StatusCode() == HttpStatusCode.InternalServerError)
					{
						l_err = Resources.API_is_down_ + " " + Resources.Please__try_again_later_;
					}
					else if (response.get_StatusCode() == HttpStatusCode.ServiceUnavailable)
					{
						string result = response.get_Content().ReadAsStringAsync().Result;
						string header = result.GetTextBetweenTags("h1").Trim();
						string paragraph = (result.GetTextBetweenTags("p").Split(new char[1] { '.' }, StringSplitOptions.RemoveEmptyEntries).Reverse()
							.FirstOrDefault() ?? string.Empty).Trim();
						l_err = header + ". " + paragraph + ".";
					}
				}
				finally
				{
					((IDisposable)response)?.Dispose();
				}
			}
			catch (Exception e)
			{
				l_err = "Failed to check API status.";
				Logger.GetLogger<Gw2ApiManager>().Info(e, l_err);
				showMessage = false;
			}
			bool isDown = !string.IsNullOrEmpty(l_err);
			if (showMessage && isDown)
			{
				ScreenNotification.ShowNotification(l_err, (NotificationType)2, (Texture2D)null, 4);
			}
			return !isDown;
		}

		public static bool IsAuthorized(this Gw2ApiManager gw2ApiManager, bool showMessage = false, params TokenPermission[] requiredPermissions)
		{
			if (!gw2ApiManager.IsApiAvailable(showMessage))
			{
				return false;
			}
			if (string.IsNullOrWhiteSpace(GameService.Gw2Mumble.get_PlayerCharacter().get_Name()))
			{
				if (showMessage)
				{
					ScreenNotification.ShowNotification(Resources.API_unavailable_ + "\n" + Resources.Please__login_to_a_character_, (NotificationType)2, (Texture2D)null, 4);
				}
				Logger.GetLogger<Gw2ApiManager>().Info("API unavailable: No character logged in. - No key can be selected because a character has to be logged in once.");
				return false;
			}
			if (!gw2ApiManager.HasPermission((TokenPermission)1))
			{
				if (showMessage)
				{
					ScreenNotification.ShowNotification(Resources.Missing_API_key_ + "\n" + string.Format(Resources.Please__add_an_API_key_to__0__, "Blish HUD"), (NotificationType)2, (Texture2D)null, 4);
				}
				Logger.GetLogger<Gw2ApiManager>().Info("Missing API key: Foreign account. - No key associated with the logged in account was found.");
				return false;
			}
			if (!gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)requiredPermissions))
			{
				string missing = string.Join(", ", requiredPermissions.Except(Enum.GetValues(typeof(TokenPermission)).Cast<TokenPermission>().Where((Func<TokenPermission, bool>)gw2ApiManager.HasPermission)));
				if (showMessage)
				{
					ScreenNotification.ShowNotification(Resources.Insufficient_API_permissions_ + "\n" + string.Format(Resources.Required___0_, missing), (NotificationType)2, (Texture2D)null, 4);
				}
				Logger.GetLogger<Gw2ApiManager>().Info("Insufficient API permissions. Required: " + missing);
				return false;
			}
			return true;
		}
	}
}
