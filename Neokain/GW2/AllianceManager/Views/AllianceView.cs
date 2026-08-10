using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Neokain.GW2.AllianceManager.Controls.Shared;
using Neokain.GW2.AllianceManager.Controls.Spam;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.AllianceManager.Repositories;
using Neokain.GW2.AllianceManager.Windows;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Accounts;
using Neokain.GW2.WebClient.Models.Alliances;
using Neokain.GW2.WebClient.Models.Enums;
using Neokain.GW2.WebClient.Models.Guilds;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Views
{
	internal class AllianceView : View
	{
		private readonly Guid _allianceId;

		private readonly Module _module;

		private readonly Gw2WebClient _webClient;

		private AllianceDetailDto _allianceDetail;

		private AccountDataDto _accountData;

		private List<GuildMembershipDto> _allianceGuildMemberships;

		private ISpamFavoriteRepository _favoriteRepository;

		private HashSet<Guid> _favoritedSpamIds = new HashSet<Guid>();

		private SpamsControl _spamsControl;

		private SpamContext _spamContext;

		private CollapsibleSection _spamsSection;

		private CategoryInputWindow _categoryInputWindow;

		private NestedScrollableFlowPanel _flowPanel;

		public AllianceView(Guid allianceId, Module module, Gw2WebClient webClient)
			: this()
		{
			_allianceId = allianceId;
			_module = module ?? throw new ArgumentNullException("module");
			_webClient = webClient ?? throw new ArgumentNullException("webClient");
		}

		private async Task LoadAllianceDetailAsync()
		{
			if (!_module.IsConnected)
			{
				return;
			}
			try
			{
				Task<AllianceDetailDto> allianceTask = _webClient.GetAllianceDetail(_allianceId);
				Task<AccountDataDto> accountTask = _webClient.GetMyAccount();
				await Task.WhenAll(allianceTask, accountTask);
				_allianceDetail = await allianceTask;
				_accountData = await accountTask;
				if (_allianceDetail == null)
				{
					ScreenNotification.ShowNotification("Failed to load alliance details", (NotificationType)2, (Texture2D)null, 4);
				}
				if (_accountData != null && _allianceDetail != null)
				{
					List<GuildMembershipDto> allMemberships = await _webClient.GetAccountGuildMemberships(_accountData.Id);
					HashSet<Guid> allianceGuildIds = new HashSet<Guid>(_allianceDetail.GuildIds ?? new List<Guid>());
					_allianceGuildMemberships = allMemberships?.Where((GuildMembershipDto m) => allianceGuildIds.Contains(m.GuildId)).ToList() ?? new List<GuildMembershipDto>();
					_spamContext = SpamContext.ForAlliance(_allianceId, _allianceGuildMemberships);
					_favoriteRepository = Module.Instance.FavoriteRepository;
					await LoadFavoritesAsync();
				}
				else
				{
					_spamContext = SpamContext.ForAlliance(_allianceId, new List<GuildMembershipDto>());
				}
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Failed to load alliance: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
			}
		}

		protected override void Unload()
		{
			if (_webClient != null)
			{
				_webClient.AllianceSpamAdded -= new EventHandler<AllianceSpamAddedEventArgs>(OnAllianceSpamAdded);
				_webClient.AllianceSpamChanged -= new EventHandler<AllianceSpamChangedEventArgs>(OnAllianceSpamChanged);
				_webClient.AllianceSpamRemoved -= new EventHandler<AllianceSpamRemovedEventArgs>(OnAllianceSpamRemoved);
				_webClient.AllianceSpamUsed -= new EventHandler<AllianceSpamUsedEventArgs>(OnAllianceSpamUsed);
				_webClient.AllianceSpamCategoryAdded -= new EventHandler<AllianceSpamCategoryAddedEventArgs>(OnAllianceSpamCategoryAdded);
				_webClient.AllianceSpamCategoryChanged -= new EventHandler<AllianceSpamCategoryChangedEventArgs>(OnAllianceSpamCategoryChanged);
				_webClient.AllianceSpamCategoryRemoved -= new EventHandler<AllianceSpamCategoryRemovedEventArgs>(OnAllianceSpamCategoryRemoved);
			}
			if (_spamsControl?.DetailsControl != null)
			{
				_spamsControl.DetailsControl.SpamUseRequested -= OnSpamUseRequested;
				_spamsControl.DetailsControl.FavoriteToggleRequested -= new EventHandler<(Guid, bool)>(OnFavoriteToggleRequested);
			}
			if (_spamsControl?.ListControl != null)
			{
				_spamsControl.ListControl.SelectedSpamChanged -= OnSpamSelected;
				_spamsControl.ListControl.CreateCategoryRequested -= OnCreateCategoryRequested;
				_spamsControl.ListControl.EditCategoryRequested -= OnEditCategoryRequested;
				_spamsControl.ListControl.DeleteCategoryRequested -= OnDeleteCategoryRequested;
			}
			CategoryInputWindow categoryInputWindow = _categoryInputWindow;
			if (categoryInputWindow != null)
			{
				((Control)categoryInputWindow).Dispose();
			}
			NestedScrollableFlowPanel flowPanel = _flowPanel;
			if (flowPanel != null)
			{
				((Control)flowPanel).Dispose();
			}
			((View<IPresenter>)this).Unload();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			NestedScrollableFlowPanel nestedScrollableFlowPanel = new NestedScrollableFlowPanel();
			((Panel)nestedScrollableFlowPanel).set_CanScroll(true);
			((FlowPanel)nestedScrollableFlowPanel).set_FlowDirection((ControlFlowDirection)3);
			((Container)nestedScrollableFlowPanel).set_WidthSizingMode((SizingMode)2);
			((Container)nestedScrollableFlowPanel).set_HeightSizingMode((SizingMode)2);
			((Control)nestedScrollableFlowPanel).set_Parent(buildPanel);
			((Control)nestedScrollableFlowPanel).set_Padding(new Thickness(5f));
			((Container)nestedScrollableFlowPanel).set_AutoSizePadding(new Point(5));
			((FlowPanel)nestedScrollableFlowPanel).set_ControlPadding(new Vector2(5f));
			((FlowPanel)nestedScrollableFlowPanel).set_OuterControlPadding(new Vector2(5f));
			_flowPanel = nestedScrollableFlowPanel;
			string headerText = ((_allianceDetail != null) ? ("Alliance: [" + _allianceDetail.Tag + "] " + _allianceDetail.Name) : "Alliance: Unknown");
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_flowPanel);
			val.set_Text(headerText);
			val.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansHeader);
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Padding(new Thickness(10f));
			if (_spamContext == null)
			{
				_spamContext = SpamContext.ForAlliance(_allianceId, _allianceGuildMemberships ?? new List<GuildMembershipDto>());
			}
			CollapsibleSection collapsibleSection = new CollapsibleSection("Spams");
			((Control)collapsibleSection).set_Parent((Container)(object)_flowPanel);
			((Container)collapsibleSection).set_WidthSizingMode((SizingMode)2);
			collapsibleSection.ExpandedContentHeight = 400;
			_spamsSection = collapsibleSection;
			SpamsControl spamsControl = new SpamsControl(Module.Instance.SpamClient, SpamSource.Alliance(_allianceId), _module, _spamContext);
			((Control)spamsControl).set_Parent((Container)(object)_spamsSection.ContentPanel);
			((Container)spamsControl).set_WidthSizingMode((SizingMode)2);
			((Container)spamsControl).set_HeightSizingMode((SizingMode)2);
			_spamsControl = spamsControl;
			_spamsControl.DetailsControl.SpamUseRequested += OnSpamUseRequested;
			_spamsControl.DetailsControl.FavoriteToggleRequested += new EventHandler<(Guid, bool)>(OnFavoriteToggleRequested);
			_spamsControl.ListControl.SelectedSpamChanged += OnSpamSelected;
			_spamsControl.ListControl.CreateCategoryRequested += OnCreateCategoryRequested;
			_spamsControl.ListControl.EditCategoryRequested += OnEditCategoryRequested;
			_spamsControl.ListControl.DeleteCategoryRequested += OnDeleteCategoryRequested;
			_webClient.AllianceSpamAdded += new EventHandler<AllianceSpamAddedEventArgs>(OnAllianceSpamAdded);
			_webClient.AllianceSpamChanged += new EventHandler<AllianceSpamChangedEventArgs>(OnAllianceSpamChanged);
			_webClient.AllianceSpamRemoved += new EventHandler<AllianceSpamRemovedEventArgs>(OnAllianceSpamRemoved);
			_webClient.AllianceSpamUsed += new EventHandler<AllianceSpamUsedEventArgs>(OnAllianceSpamUsed);
			_webClient.AllianceSpamCategoryAdded += new EventHandler<AllianceSpamCategoryAddedEventArgs>(OnAllianceSpamCategoryAdded);
			_webClient.AllianceSpamCategoryChanged += new EventHandler<AllianceSpamCategoryChangedEventArgs>(OnAllianceSpamCategoryChanged);
			_webClient.AllianceSpamCategoryRemoved += new EventHandler<AllianceSpamCategoryRemovedEventArgs>(OnAllianceSpamCategoryRemoved);
			((View<IPresenter>)this).Build(buildPanel);
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			progress.Report("Loading alliance details...");
			await LoadAllianceDetailAsync();
			return await ((View<IPresenter>)this).Load(progress);
		}

		private void OnAllianceSpamAdded(object sender, AllianceSpamAddedEventArgs e)
		{
			if (!(e?.AllianceId != _allianceId) && e.Spam != null)
			{
				_spamsControl?.ListControl?.AddOrUpdateSpam(e.Spam);
			}
		}

		private void OnAllianceSpamChanged(object sender, AllianceSpamChangedEventArgs e)
		{
			if (!(e?.AllianceId != _allianceId) && e.Spam != null)
			{
				_spamsControl?.ListControl?.AddOrUpdateSpam(e.Spam);
				_spamsControl?.DetailsControl?.UpdateSpamData(e.Spam);
			}
		}

		private void OnAllianceSpamRemoved(object sender, AllianceSpamRemovedEventArgs e)
		{
			if (!(e?.AllianceId != _allianceId))
			{
				_spamsControl?.ListControl?.RemoveSpam(e.SpamId);
			}
		}

		private void OnAllianceSpamUsed(object sender, AllianceSpamUsedEventArgs e)
		{
		}

		private async void OnSpamUseRequested(object sender, Guid spamId)
		{
			try
			{
				await Module.Instance.SpamOrchestrator.UseAsync(SpamSource.Alliance(_allianceId), spamId, _spamContext, Module.Instance.ShowCooldownOverridePromptAsync);
			}
			catch (Exception ex)
			{
				if (ex.Message.IndexOf("not found", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					ScreenNotification.ShowNotification("This spam no longer exists — please refresh.", (NotificationType)1, (Texture2D)null, 4);
				}
				else
				{
					ScreenNotification.ShowNotification("Failed to use spam: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
				}
			}
		}

		private void OnSpamSelected(object sender, SelectedSpamChangedEventArgs e)
		{
			if (e?.SelectedSpam == null)
			{
				_spamsControl?.DetailsControl?.SetIsFavorited(isFavorited: false);
				return;
			}
			bool isFavorited = _favoritedSpamIds.Contains(e.SelectedSpam.Id);
			_spamsControl?.DetailsControl?.SetIsFavorited(isFavorited);
		}

		private async void OnFavoriteToggleRequested(object sender, (Guid SpamId, bool AddToFavorites) args)
		{
			if (_favoriteRepository == null || _accountData == null)
			{
				ScreenNotification.ShowNotification("Please wait for account data to load", (NotificationType)1, (Texture2D)null, 4);
				return;
			}
			try
			{
				if (args.AddToFavorites)
				{
					SpamFavoriteCreateDto createDto = new SpamFavoriteCreateDto
					{
						SpamId = args.SpamId,
						SourceType = SpamSourceType.Alliance,
						SourceAllianceId = _allianceId
					};
					if (await _favoriteRepository.AddFavoriteAsync(createDto) != null)
					{
						_favoritedSpamIds.Add(args.SpamId);
						_spamsControl?.DetailsControl?.SetIsFavorited(isFavorited: true);
						ScreenNotification.ShowNotification("Added to favorites", (NotificationType)0, (Texture2D)null, 4);
					}
				}
				else
				{
					SpamFavoriteDto favorite = (await _favoriteRepository.GetFavoritesAsync())?.FirstOrDefault((SpamFavoriteDto f) => f.SpamId == args.SpamId);
					if (favorite != null)
					{
						await _favoriteRepository.RemoveFavoriteAsync(favorite.Id);
						_favoritedSpamIds.Remove(args.SpamId);
						_spamsControl?.DetailsControl?.SetIsFavorited(isFavorited: false);
						ScreenNotification.ShowNotification("Removed from favorites", (NotificationType)0, (Texture2D)null, 4);
					}
				}
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Failed to update favorite: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private async Task LoadFavoritesAsync()
		{
			if (_favoriteRepository == null)
			{
				return;
			}
			try
			{
				List<SpamFavoriteDto> favorites = await _favoriteRepository.GetFavoritesAsync();
				_favoritedSpamIds.Clear();
				if (favorites == null)
				{
					return;
				}
				foreach (SpamFavoriteDto favorite in favorites)
				{
					_favoritedSpamIds.Add(favorite.SpamId);
				}
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Failed to load favorites: " + ex.Message, (NotificationType)1, (Texture2D)null, 4);
			}
		}

		private void OnCreateCategoryRequested(object sender, EventArgs e)
		{
			ShowCategoryInputWindow(null);
		}

		private void OnEditCategoryRequested(object sender, CategoryEventArgs e)
		{
			if (e?.Category != null)
			{
				ShowCategoryInputWindow(e.Category);
			}
		}

		private async void OnDeleteCategoryRequested(object sender, CategoryEventArgs e)
		{
			if (e?.Category != null)
			{
				try
				{
					await Module.Instance.SpamClient.DeleteCategory(SpamSource.Alliance(_allianceId), e.Category.Id);
					ScreenNotification.ShowNotification("Deleted category '" + e.Category.Name + "'", (NotificationType)0, (Texture2D)null, 4);
				}
				catch (Exception ex)
				{
					ScreenNotification.ShowNotification("Failed to delete category: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
				}
			}
		}

		private void ShowCategoryInputWindow(SpamCategoryDto existingCategory)
		{
			CategoryInputWindow categoryInputWindow = _categoryInputWindow;
			if (categoryInputWindow != null)
			{
				((Control)categoryInputWindow).Dispose();
			}
			_categoryInputWindow = new CategoryInputWindow(existingCategory);
			_categoryInputWindow.Confirmed += OnCategoryInputConfirmed;
			((Control)_categoryInputWindow).Show();
		}

		private async void OnCategoryInputConfirmed(object sender, CategoryInputResult result)
		{
			_ = 1;
			try
			{
				if (result.CategoryId.HasValue)
				{
					SpamCategoryUpdateDto updateDto = new SpamCategoryUpdateDto
					{
						Name = result.Name,
						Description = result.Description,
						DisplayOrder = result.DisplayOrder
					};
					await Module.Instance.SpamClient.UpdateCategory(SpamSource.Alliance(_allianceId), result.CategoryId.Value, updateDto);
					ScreenNotification.ShowNotification("Updated category '" + result.Name + "'", (NotificationType)0, (Texture2D)null, 4);
				}
				else
				{
					SpamCategoryCreateDto createDto = new SpamCategoryCreateDto
					{
						Name = result.Name,
						Description = result.Description
					};
					await Module.Instance.SpamClient.AddCategory(SpamSource.Alliance(_allianceId), createDto);
					ScreenNotification.ShowNotification("Created category '" + result.Name + "'", (NotificationType)0, (Texture2D)null, 4);
				}
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Failed to save category: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private void OnAllianceSpamCategoryAdded(object sender, AllianceSpamCategoryAddedEventArgs e)
		{
			if (!(e?.AllianceId != _allianceId) && e.Category != null)
			{
				_spamsControl?.ListControl?.AddCategory(e.Category);
			}
		}

		private void OnAllianceSpamCategoryChanged(object sender, AllianceSpamCategoryChangedEventArgs e)
		{
			if (!(e?.AllianceId != _allianceId) && e.Category != null)
			{
				_spamsControl?.ListControl?.UpdateCategory(e.Category);
			}
		}

		private void OnAllianceSpamCategoryRemoved(object sender, AllianceSpamCategoryRemovedEventArgs e)
		{
			if (!(e?.AllianceId != _allianceId))
			{
				_spamsControl?.ListControl?.RemoveCategory(e.CategoryId);
			}
		}
	}
}
