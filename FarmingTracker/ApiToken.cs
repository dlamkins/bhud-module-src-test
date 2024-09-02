using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.V2.Models;

namespace FarmingTracker
{
	public class ApiToken
	{
		private readonly IReadOnlyList<TokenPermission> REQUIRED_API_TOKEN_PERMISSIONS = new List<TokenPermission>
		{
			(TokenPermission)1,
			(TokenPermission)5,
			(TokenPermission)3,
			(TokenPermission)10,
			(TokenPermission)2,
			(TokenPermission)8
		}.AsReadOnly();

		public bool CanAccessApi => ApiTokenState == ApiTokenState.CanAccessApi;

		public bool ApiTokenMissing => ApiTokenState == ApiTokenState.ApiTokenMissing;

		public ApiTokenState ApiTokenState { get; }

		public List<TokenPermission> MissingPermissions { get; } = new List<TokenPermission>();


		public List<TokenPermission> RequiredPermissions { get; }

		private static IReadOnlyList<TokenPermission> API_TOKEN_PERMISSIONS_EVERY_API_KEY_HAS_BY_DEFAULT => new List<TokenPermission> { (TokenPermission)1 };

		public ApiToken(Gw2ApiManager gw2ApiManager)
		{
			IEnumerable<TokenPermission> missingPermissions = GetMissingPermissions(REQUIRED_API_TOKEN_PERMISSIONS, gw2ApiManager);
			MissingPermissions.AddRange(missingPermissions);
			ApiTokenState = GetApiTokenState(REQUIRED_API_TOKEN_PERMISSIONS, gw2ApiManager);
			RequiredPermissions = REQUIRED_API_TOKEN_PERMISSIONS.ToList();
		}

		public string CreateApiTokenErrorTooltipText()
		{
			return ApiTokenState switch
			{
				ApiTokenState.hasNotLoggedIntoCharacterSinceStartingGw2 => "Error: You have to log into a character once after starting Guild Wars 2.\nOtherwise the module gets no GW2 API access from blish.", 
				ApiTokenState.ApiTokenMissing => "Error: GW2 Api key missing. Please add an api key with these permissions: " + string.Join(", ", RequiredPermissions) + ".\nIf that does not fix the issue try disabling the module and then enabling it again.", 
				ApiTokenState.RequiredPermissionsMissing => "Error: GW2 Api key is missing these permissions: " + string.Join(", ", MissingPermissions) + ".\nPlease add a new api key with all required permissions.", 
				_ => $"This should not happen. ApiTokenState: {ApiTokenState}", 
			};
		}

		public string CreateApiTokenErrorLabelText()
		{
			return ApiTokenState switch
			{
				ApiTokenState.hasNotLoggedIntoCharacterSinceStartingGw2 => "Log into character!", 
				ApiTokenState.ApiTokenMissing => "Add GW2 API key!", 
				ApiTokenState.RequiredPermissionsMissing => "Missing GW2 API key permissions!", 
				_ => $"This should not happen. ApiTokenState: {ApiTokenState}", 
			};
		}

		private ApiTokenState GetApiTokenState(IReadOnlyList<TokenPermission> requiredApiTokenPermissions, Gw2ApiManager gw2ApiManager)
		{
			if (string.IsNullOrWhiteSpace(GameService.Gw2Mumble.get_PlayerCharacter().get_Name()))
			{
				return ApiTokenState.hasNotLoggedIntoCharacterSinceStartingGw2;
			}
			if (!gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)API_TOKEN_PERMISSIONS_EVERY_API_KEY_HAS_BY_DEFAULT))
			{
				return ApiTokenState.ApiTokenMissing;
			}
			if (!gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)requiredApiTokenPermissions))
			{
				return ApiTokenState.RequiredPermissionsMissing;
			}
			return ApiTokenState.CanAccessApi;
		}

		private IEnumerable<TokenPermission> GetMissingPermissions(IReadOnlyList<TokenPermission> requiredPermissions, Gw2ApiManager gw2ApiManager)
		{
			foreach (TokenPermission requiredPermission in requiredPermissions)
			{
				if (!gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)new List<TokenPermission> { requiredPermission }))
				{
					yield return requiredPermission;
				}
			}
		}
	}
}
