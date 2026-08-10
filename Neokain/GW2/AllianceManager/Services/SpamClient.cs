using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Enums;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Services
{
	public sealed class SpamClient : IDisposable
	{
		private readonly IGw2WebClient _wc;

		private readonly Gw2WebClient _gwClient;

		private readonly EventHandler<AllianceSpamAddedEventArgs> _onAllianceSpamAdded;

		private readonly EventHandler<AllianceSpamChangedEventArgs> _onAllianceSpamChanged;

		private readonly EventHandler<AllianceSpamRemovedEventArgs> _onAllianceSpamRemoved;

		private readonly EventHandler<AllianceSpamUsedEventArgs> _onAllianceSpamUsed;

		private readonly EventHandler<AllianceSpamCategoryAddedEventArgs> _onAllianceSpamCatAdded;

		private readonly EventHandler<AllianceSpamCategoryChangedEventArgs> _onAllianceSpamCatChanged;

		private readonly EventHandler<AllianceSpamCategoryRemovedEventArgs> _onAllianceSpamCatRemoved;

		private readonly EventHandler<GuildSpamAddedEventArgs> _onGuildSpamAdded;

		private readonly EventHandler<GuildSpamChangedEventArgs> _onGuildSpamChanged;

		private readonly EventHandler<GuildSpamRemovedEventArgs> _onGuildSpamRemoved;

		private readonly EventHandler<GuildSpamUsedEventArgs> _onGuildSpamUsed;

		private readonly EventHandler<GuildSpamCategoryAddedEventArgs> _onGuildSpamCatAdded;

		private readonly EventHandler<GuildSpamCategoryChangedEventArgs> _onGuildSpamCatChanged;

		private readonly EventHandler<GuildSpamCategoryRemovedEventArgs> _onGuildSpamCatRemoved;

		public event Action<SpamSource, SpamDto> SpamAdded;

		public event Action<SpamSource, SpamDetailDto> SpamChanged;

		public event Action<SpamSource, Guid> SpamRemoved;

		public event Action<SpamSource, SpamUsedNotificationDto> SpamUsed;

		public event Action<SpamSource, SpamCategoryDto> CategoryAdded;

		public event Action<SpamSource, SpamCategoryDto> CategoryChanged;

		public event Action<SpamSource, Guid> CategoryRemoved;

		public SpamClient(IGw2WebClient client)
		{
			_wc = client ?? throw new ArgumentNullException("client");
		}

		public SpamClient(Gw2WebClient client)
			: this((IGw2WebClient)client)
		{
			_gwClient = client;
			_onAllianceSpamAdded = delegate(object s, AllianceSpamAddedEventArgs e)
			{
				this.SpamAdded?.Invoke(SpamSource.Alliance(e.AllianceId), e.Spam);
			};
			_onAllianceSpamChanged = delegate(object s, AllianceSpamChangedEventArgs e)
			{
				this.SpamChanged?.Invoke(SpamSource.Alliance(e.AllianceId), e.Spam);
			};
			_onAllianceSpamRemoved = delegate(object s, AllianceSpamRemovedEventArgs e)
			{
				this.SpamRemoved?.Invoke(SpamSource.Alliance(e.AllianceId), e.SpamId);
			};
			_onAllianceSpamUsed = delegate(object s, AllianceSpamUsedEventArgs e)
			{
				this.SpamUsed?.Invoke(SpamSource.Alliance(e.AllianceId), e.Notification);
			};
			_onAllianceSpamCatAdded = delegate(object s, AllianceSpamCategoryAddedEventArgs e)
			{
				this.CategoryAdded?.Invoke(SpamSource.Alliance(e.AllianceId), e.Category);
			};
			_onAllianceSpamCatChanged = delegate(object s, AllianceSpamCategoryChangedEventArgs e)
			{
				this.CategoryChanged?.Invoke(SpamSource.Alliance(e.AllianceId), e.Category);
			};
			_onAllianceSpamCatRemoved = delegate(object s, AllianceSpamCategoryRemovedEventArgs e)
			{
				this.CategoryRemoved?.Invoke(SpamSource.Alliance(e.AllianceId), e.CategoryId);
			};
			_onGuildSpamAdded = delegate(object s, GuildSpamAddedEventArgs e)
			{
				this.SpamAdded?.Invoke(SpamSource.Guild(e.GuildId), e.Spam);
			};
			_onGuildSpamChanged = delegate(object s, GuildSpamChangedEventArgs e)
			{
				this.SpamChanged?.Invoke(SpamSource.Guild(e.GuildId), e.Spam);
			};
			_onGuildSpamRemoved = delegate(object s, GuildSpamRemovedEventArgs e)
			{
				this.SpamRemoved?.Invoke(SpamSource.Guild(e.GuildId), e.SpamId);
			};
			_onGuildSpamUsed = delegate(object s, GuildSpamUsedEventArgs e)
			{
				this.SpamUsed?.Invoke(SpamSource.Guild(e.GuildId), e.Notification);
			};
			_onGuildSpamCatAdded = delegate(object s, GuildSpamCategoryAddedEventArgs e)
			{
				this.CategoryAdded?.Invoke(SpamSource.Guild(e.GuildId), e.Category);
			};
			_onGuildSpamCatChanged = delegate(object s, GuildSpamCategoryChangedEventArgs e)
			{
				this.CategoryChanged?.Invoke(SpamSource.Guild(e.GuildId), e.Category);
			};
			_onGuildSpamCatRemoved = delegate(object s, GuildSpamCategoryRemovedEventArgs e)
			{
				this.CategoryRemoved?.Invoke(SpamSource.Guild(e.GuildId), e.CategoryId);
			};
			client.AllianceSpamAdded += _onAllianceSpamAdded;
			client.AllianceSpamChanged += _onAllianceSpamChanged;
			client.AllianceSpamRemoved += _onAllianceSpamRemoved;
			client.AllianceSpamUsed += _onAllianceSpamUsed;
			client.AllianceSpamCategoryAdded += _onAllianceSpamCatAdded;
			client.AllianceSpamCategoryChanged += _onAllianceSpamCatChanged;
			client.AllianceSpamCategoryRemoved += _onAllianceSpamCatRemoved;
			client.GuildSpamAdded += _onGuildSpamAdded;
			client.GuildSpamChanged += _onGuildSpamChanged;
			client.GuildSpamRemoved += _onGuildSpamRemoved;
			client.GuildSpamUsed += _onGuildSpamUsed;
			client.GuildSpamCategoryAdded += _onGuildSpamCatAdded;
			client.GuildSpamCategoryChanged += _onGuildSpamCatChanged;
			client.GuildSpamCategoryRemoved += _onGuildSpamCatRemoved;
		}

		public Task<List<SpamDto>> GetSpams(SpamSource source)
		{
			return source.Type switch
			{
				SpamSourceType.Alliance => _wc.GetAllianceSpams(source.Id), 
				SpamSourceType.Guild => _wc.GetGuildSpams(source.Id), 
				SpamSourceType.Account => _wc.GetAccountSpams(source.Id), 
				_ => throw new ArgumentOutOfRangeException("source"), 
			};
		}

		public Task<SpamDetailDto> GetSpam(SpamSource source, Guid spamId)
		{
			return source.Type switch
			{
				SpamSourceType.Alliance => _wc.GetAllianceSpam(source.Id, spamId), 
				SpamSourceType.Guild => _wc.GetGuildSpam(source.Id, spamId), 
				SpamSourceType.Account => _wc.GetAccountSpam(source.Id, spamId), 
				_ => throw new ArgumentOutOfRangeException("source"), 
			};
		}

		public Task<SpamDetailDto> AddSpam(SpamSource source, SpamCreateDto dto)
		{
			return source.Type switch
			{
				SpamSourceType.Alliance => _wc.AddAllianceSpam(source.Id, dto), 
				SpamSourceType.Guild => _wc.AddGuildSpam(source.Id, dto), 
				SpamSourceType.Account => _wc.AddAccountSpam(source.Id, dto), 
				_ => throw new ArgumentOutOfRangeException("source"), 
			};
		}

		public Task<SpamDetailDto> UpdateSpam(SpamSource source, Guid spamId, SpamUpdateDto dto)
		{
			return source.Type switch
			{
				SpamSourceType.Alliance => _wc.UpdateAllianceSpam(source.Id, spamId, dto), 
				SpamSourceType.Guild => _wc.UpdateGuildSpam(source.Id, spamId, dto), 
				SpamSourceType.Account => _wc.UpdateAccountSpam(source.Id, spamId, dto), 
				_ => throw new ArgumentOutOfRangeException("source"), 
			};
		}

		public Task DeleteSpam(SpamSource source, Guid spamId)
		{
			return source.Type switch
			{
				SpamSourceType.Alliance => _wc.DeleteAllianceSpam(source.Id, spamId), 
				SpamSourceType.Guild => _wc.DeleteGuildSpam(source.Id, spamId), 
				SpamSourceType.Account => _wc.DeleteAccountSpam(source.Id, spamId), 
				_ => throw new ArgumentOutOfRangeException("source"), 
			};
		}

		public Task<SpamDetailDto> UseSpam(SpamSource source, Guid spamId, SpamUsageRequestDto request)
		{
			return source.Type switch
			{
				SpamSourceType.Alliance => _wc.UseAllianceSpam(source.Id, spamId, request), 
				SpamSourceType.Guild => _wc.UseGuildSpam(source.Id, spamId, request), 
				SpamSourceType.Account => _wc.UseAccountSpam(source.Id, spamId, request), 
				_ => throw new ArgumentOutOfRangeException("source"), 
			};
		}

		public Task<bool> CanUseSpam(SpamSource source, Guid spamId, int? mapId)
		{
			return source.Type switch
			{
				SpamSourceType.Alliance => _wc.CanUseAllianceSpam(source.Id, spamId, mapId), 
				SpamSourceType.Guild => _wc.CanUseGuildSpam(source.Id, spamId, mapId), 
				SpamSourceType.Account => _wc.CanUseAccountSpam(source.Id, spamId, mapId), 
				_ => throw new ArgumentOutOfRangeException("source"), 
			};
		}

		public Task<List<SpamCategoryDto>> GetCategories(SpamSource source)
		{
			return source.Type switch
			{
				SpamSourceType.Alliance => _wc.GetAllianceSpamCategories(source.Id), 
				SpamSourceType.Guild => _wc.GetGuildSpamCategories(source.Id), 
				SpamSourceType.Account => _wc.GetAccountSpamCategories(source.Id), 
				_ => throw new ArgumentOutOfRangeException("source"), 
			};
		}

		public Task<SpamCategoryDto> AddCategory(SpamSource source, SpamCategoryCreateDto dto)
		{
			return source.Type switch
			{
				SpamSourceType.Alliance => _wc.AddAllianceSpamCategory(source.Id, dto), 
				SpamSourceType.Guild => _wc.AddGuildSpamCategory(source.Id, dto), 
				SpamSourceType.Account => _wc.AddAccountSpamCategory(source.Id, dto), 
				_ => throw new ArgumentOutOfRangeException("source"), 
			};
		}

		public Task<SpamCategoryDto> UpdateCategory(SpamSource source, Guid categoryId, SpamCategoryUpdateDto dto)
		{
			return source.Type switch
			{
				SpamSourceType.Alliance => _wc.UpdateAllianceSpamCategory(source.Id, categoryId, dto), 
				SpamSourceType.Guild => _wc.UpdateGuildSpamCategory(source.Id, categoryId, dto), 
				SpamSourceType.Account => _wc.UpdateAccountSpamCategory(source.Id, categoryId, dto), 
				_ => throw new ArgumentOutOfRangeException("source"), 
			};
		}

		public Task DeleteCategory(SpamSource source, Guid categoryId)
		{
			return source.Type switch
			{
				SpamSourceType.Alliance => _wc.DeleteAllianceSpamCategory(source.Id, categoryId), 
				SpamSourceType.Guild => _wc.DeleteGuildSpamCategory(source.Id, categoryId), 
				SpamSourceType.Account => _wc.DeleteAccountSpamCategory(source.Id, categoryId), 
				_ => throw new ArgumentOutOfRangeException("source"), 
			};
		}

		public Task<List<SpamMapHistoryDto>> GetMapHistory(SpamSource source, Guid spamId)
		{
			return source.Type switch
			{
				SpamSourceType.Alliance => _wc.GetAllianceSpamMapHistory(source.Id, spamId), 
				SpamSourceType.Guild => _wc.GetGuildSpamMapHistory(source.Id, spamId), 
				SpamSourceType.Account => _wc.GetAccountSpamMapHistory(source.Id, spamId), 
				_ => throw new ArgumentOutOfRangeException("source"), 
			};
		}

		public Task<List<SpamUsageLogDto>> GetUsageLogs(SpamSource source, Guid spamId, int limit = 50)
		{
			return source.Type switch
			{
				SpamSourceType.Alliance => _wc.GetAllianceSpamUsageLogs(source.Id, spamId, limit), 
				SpamSourceType.Guild => _wc.GetGuildSpamUsageLogs(source.Id, spamId, limit), 
				SpamSourceType.Account => _wc.GetAccountSpamUsageLogs(source.Id, spamId, limit), 
				_ => throw new ArgumentOutOfRangeException("source"), 
			};
		}

		public void Dispose()
		{
			if (_gwClient != null)
			{
				_gwClient.AllianceSpamAdded -= _onAllianceSpamAdded;
				_gwClient.AllianceSpamChanged -= _onAllianceSpamChanged;
				_gwClient.AllianceSpamRemoved -= _onAllianceSpamRemoved;
				_gwClient.AllianceSpamUsed -= _onAllianceSpamUsed;
				_gwClient.AllianceSpamCategoryAdded -= _onAllianceSpamCatAdded;
				_gwClient.AllianceSpamCategoryChanged -= _onAllianceSpamCatChanged;
				_gwClient.AllianceSpamCategoryRemoved -= _onAllianceSpamCatRemoved;
				_gwClient.GuildSpamAdded -= _onGuildSpamAdded;
				_gwClient.GuildSpamChanged -= _onGuildSpamChanged;
				_gwClient.GuildSpamRemoved -= _onGuildSpamRemoved;
				_gwClient.GuildSpamUsed -= _onGuildSpamUsed;
				_gwClient.GuildSpamCategoryAdded -= _onGuildSpamCatAdded;
				_gwClient.GuildSpamCategoryChanged -= _onGuildSpamCatChanged;
				_gwClient.GuildSpamCategoryRemoved -= _onGuildSpamCatRemoved;
			}
		}
	}
}
