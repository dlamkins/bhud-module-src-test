using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Guilds;

namespace Neokain.GW2.AllianceManager.Controls.Guilds
{
	public class GuildRanksControl : Panel
	{
		private readonly Guid _guildId;

		private readonly IGw2WebClient _webClient;

		private readonly Module _module;

		private readonly Menu _menu;

		private List<GuildRankDto> _ranks;

		public int RankCount => _ranks?.Count ?? 0;

		public GuildRanksControl(Guid guildId, IGw2WebClient webClient, Module module)
			: this()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			_guildId = guildId;
			_webClient = webClient ?? throw new ArgumentNullException("webClient");
			_module = module ?? throw new ArgumentNullException("module");
			((Panel)this).set_CanScroll(true);
			((Panel)this).set_ShowBorder(true);
			Menu val = new Menu();
			((Control)val).set_Parent((Container)(object)this);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			_menu = val;
			LoadRanksAsync();
		}

		public async Task LoadRanksAsync()
		{
			try
			{
				_ranks = await _webClient.GetGuildRanks(_guildId);
				if (_ranks != null)
				{
					PopulateMenu();
				}
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Failed to load guild ranks: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private void PopulateMenu()
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			foreach (Control item in ((Container)_menu).get_Children().ToList())
			{
				item.Dispose();
			}
			if (_ranks == null || _ranks.Count == 0)
			{
				MenuItem val = new MenuItem("No ranks found");
				((Control)val).set_Parent((Container)(object)_menu);
				((Control)val).set_Enabled(false);
				return;
			}
			foreach (GuildRankDto rank in _ranks.OrderBy((GuildRankDto r) => r.Order).ToList())
			{
				MenuItem val2 = new MenuItem($"{rank.Order}. {rank.Name}");
				((Control)val2).set_Parent((Container)(object)_menu);
				((Control)val2).set_BasicTooltipText($"Order: {rank.Order}");
			}
		}

		public async Task RefreshAsync()
		{
			await LoadRanksAsync();
		}
	}
}
