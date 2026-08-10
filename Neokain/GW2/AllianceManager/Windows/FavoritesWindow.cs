using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neokain.GW2.AllianceManager.Controls.Favorites;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.AllianceManager.Repositories;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Accounts;
using Neokain.GW2.WebClient.Models.Alliances;
using Neokain.GW2.WebClient.Models.Enums;
using Neokain.GW2.WebClient.Models.Guilds;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Windows
{
	public class FavoritesWindow : StandardWindow
	{
		private const int WINDOW_WIDTH = 350;

		private const int WINDOW_HEIGHT = 400;

		private const int PADDING = 10;

		private readonly Module _module;

		private readonly ISpamFavoriteRepository _favoriteRepository;

		private readonly Gw2WebClient _webClient;

		private FavoritesListControl _listControl;

		public FavoritesWindow(Module module, ISpamFavoriteRepository favoriteRepository, Gw2WebClient webClient)
			: this(Textures.get_Pixel(), new Rectangle(0, 0, 350, 400), new Rectangle(10, 10, 330, 380))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			_module = module ?? throw new ArgumentNullException("module");
			_favoriteRepository = favoriteRepository ?? throw new ArgumentNullException("favoriteRepository");
			_webClient = webClient ?? throw new ArgumentNullException("webClient");
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Visible(false);
			((WindowBase2)this).set_Title("Spam Favorites");
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("Neokain_GW2_AllianceManager_favoritesWindow");
			((Control)this).set_Top(100);
			((Control)this).set_Left(100);
			((Control)this).set_BackgroundColor(Color.get_Black());
			((WindowBase2)this).set_CanCloseWithEscape(true);
			((WindowBase2)this).set_CanResize(false);
			CreateControls();
			_favoriteRepository.FavoriteAdded += OnFavoriteAddedHandler;
			_favoriteRepository.FavoriteRemoved += OnFavoriteRemovedHandler;
		}

		private void CreateControls()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			FavoritesListControl favoritesListControl = new FavoritesListControl(_favoriteRepository);
			((Control)favoritesListControl).set_Parent((Container)(object)this);
			((Control)favoritesListControl).set_Left(0);
			((Control)favoritesListControl).set_Top(0);
			((Control)favoritesListControl).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)favoritesListControl).set_Height(((Container)this).get_ContentRegion().Height);
			_listControl = favoritesListControl;
			_listControl.FavoriteUseRequested += OnFavoriteUseRequested;
			_listControl.FavoriteRemoveRequested += OnFavoriteRemoveRequested;
		}

		public override void Show()
		{
			((WindowBase2)this).Show();
			_listControl.LoadFavoritesAsync();
		}

		private async void OnFavoriteUseRequested(object sender, SpamFavoriteDto favorite)
		{
			_ = 2;
			try
			{
				SpamSource? source = await BuildFavoriteSourceAsync(favorite);
				if (!source.HasValue)
				{
					ScreenNotification.ShowNotification("Failed to build spam source for favorite", (NotificationType)2, (Texture2D)null, 4);
					return;
				}
				SpamContext context = await BuildFavoriteContextAsync(favorite);
				if (context == null)
				{
					ScreenNotification.ShowNotification("Failed to build spam context for favorite", (NotificationType)2, (Texture2D)null, 4);
				}
				else
				{
					if (!(await Module.Instance.SpamOrchestrator.UseAsync(source.Value, favorite.SpamId, context, Module.Instance.ShowCooldownOverridePromptAsync)))
					{
						return;
					}
					DateTimeOffset now = DateTimeOffset.UtcNow;
					if (favorite.HasMapLines)
					{
						int currentMapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
						SpamMapHistoryDto mapCooldown = favorite.MapCooldowns?.Find((SpamMapHistoryDto mc) => mc.MapId == currentMapId);
						if (mapCooldown != null)
						{
							mapCooldown.LastSpammed = now;
						}
						else
						{
							if (favorite.MapCooldowns == null)
							{
								favorite.MapCooldowns = new List<SpamMapHistoryDto>();
							}
							favorite.MapCooldowns.Add(new SpamMapHistoryDto
							{
								MapId = currentMapId,
								LastSpammed = now
							});
						}
					}
					favorite.LastSpammed = now;
					_listControl.UpdateFavorite(favorite);
					return;
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.IndexOf("not found", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					ScreenNotification.ShowNotification("This spam no longer exists — please refresh.", (NotificationType)1, (Texture2D)null, 4);
				}
				else
				{
					ScreenNotification.ShowNotification("Failed to use favorite: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
				}
			}
		}

		private async Task<SpamSource?> BuildFavoriteSourceAsync(SpamFavoriteDto favorite)
		{
			switch (favorite.SourceType)
			{
			case SpamSourceType.Guild:
				if (favorite.SourceGuildId.HasValue)
				{
					return SpamSource.Guild(favorite.SourceGuildId.Value);
				}
				return null;
			case SpamSourceType.Alliance:
				if (favorite.SourceAllianceId.HasValue)
				{
					return SpamSource.Alliance(favorite.SourceAllianceId.Value);
				}
				return null;
			case SpamSourceType.Account:
			{
				AccountDataDto account = await _webClient.GetMyAccount();
				if (account != null)
				{
					return SpamSource.Account(account.Id);
				}
				return null;
			}
			default:
				return null;
			}
		}

		private async Task<SpamContext> BuildFavoriteContextAsync(SpamFavoriteDto favorite)
		{
			switch (favorite.SourceType)
			{
			case SpamSourceType.Guild:
			{
				if (!favorite.SourceGuildId.HasValue)
				{
					return null;
				}
				AccountDataDto account = await _webClient.GetMyAccount();
				GuildMembershipDto guildMembership = (await _webClient.GetAccountGuildMemberships(account.Id))?.FirstOrDefault((GuildMembershipDto m) => m.GuildId == favorite.SourceGuildId.Value);
				return SpamContext.ForGuild(favorite.SourceGuildId.Value, guildMembership);
			}
			case SpamSourceType.Alliance:
			{
				if (!favorite.SourceAllianceId.HasValue)
				{
					return null;
				}
				AccountDataDto account3 = await _webClient.GetMyAccount();
				List<GuildMembershipDto> memberships = await _webClient.GetAccountGuildMemberships(account3.Id);
				HashSet<Guid> allianceGuildIds = new HashSet<Guid>((await _webClient.GetAllianceDetail(favorite.SourceAllianceId.Value))?.GuildIds ?? new List<Guid>());
				List<GuildMembershipDto> allianceGuilds = memberships?.Where((GuildMembershipDto m) => allianceGuildIds.Contains(m.GuildId)).ToList() ?? new List<GuildMembershipDto>();
				return SpamContext.ForAlliance(favorite.SourceAllianceId.Value, allianceGuilds);
			}
			case SpamSourceType.Account:
			{
				AccountDataDto account2 = await _webClient.GetMyAccount();
				if (account2 == null)
				{
					return null;
				}
				Task<List<GuildMembershipDto>> guildTask = _webClient.GetAccountGuildMemberships(account2.Id);
				Task<List<AllianceMembershipDto>> allianceTask = _webClient.GetAccountAllianceMemberships(account2.Id);
				await Task.WhenAll(guildTask, allianceTask);
				List<GuildMembershipDto> memberships = (await guildTask) ?? new List<GuildMembershipDto>();
				List<AllianceMembershipDto> alliances = (await allianceTask) ?? new List<AllianceMembershipDto>();
				return SpamContext.ForAccount(account2.Id, memberships, alliances);
			}
			default:
				return null;
			}
		}

		private async void OnFavoriteRemoveRequested(object sender, Guid favoriteId)
		{
			try
			{
				await _favoriteRepository.RemoveFavoriteAsync(favoriteId);
				ScreenNotification.ShowNotification("Removed from favorites", (NotificationType)0, (Texture2D)null, 4);
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Failed to remove favorite: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private void OnFavoriteAddedHandler(object sender, SpamFavoriteDto favorite)
		{
			OnFavoriteAdded(favorite);
		}

		private void OnFavoriteRemovedHandler(object sender, Guid favoriteId)
		{
			OnFavoriteRemoved(favoriteId);
		}

		public void OnFavoriteAdded(SpamFavoriteDto favorite)
		{
			_listControl?.AddFavorite(favorite);
		}

		public void OnFavoriteRemoved(Guid favoriteId)
		{
			_listControl?.RemoveFavorite(favoriteId);
		}

		protected override void DisposeControl()
		{
			_favoriteRepository.FavoriteAdded -= OnFavoriteAddedHandler;
			_favoriteRepository.FavoriteRemoved -= OnFavoriteRemovedHandler;
			if (_listControl != null)
			{
				_listControl.FavoriteUseRequested -= OnFavoriteUseRequested;
				_listControl.FavoriteRemoveRequested -= OnFavoriteRemoveRequested;
				((Control)_listControl).Dispose();
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
