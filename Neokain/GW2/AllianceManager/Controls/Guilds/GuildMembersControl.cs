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
	public class GuildMembersControl : Panel
	{
		private readonly Guid _guildId;

		private readonly IGw2WebClient _webClient;

		private readonly Module _module;

		private readonly Menu _menu;

		private List<GuildMemberDetailDto> _members;

		public int MemberCount => _members?.Count ?? 0;

		public GuildMembersControl(Guid guildId, IGw2WebClient webClient, Module module)
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
			LoadMembersAsync();
		}

		public async Task LoadMembersAsync()
		{
			try
			{
				_members = await _webClient.GetGuildMembers(_guildId);
				if (_members != null)
				{
					PopulateMenu();
				}
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Failed to load guild members: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private void PopulateMenu()
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			foreach (Control item in ((Container)_menu).get_Children().ToList())
			{
				item.Dispose();
			}
			if (_members == null || _members.Count == 0)
			{
				MenuItem val = new MenuItem("No members found");
				((Control)val).set_Parent((Container)(object)_menu);
				((Control)val).set_Enabled(false);
				return;
			}
			foreach (GuildMemberDetailDto member in (from m in _members
				orderby m.RankOrder, m.Name
				select m).ToList())
			{
				string displayText = member.Name;
				if (!string.IsNullOrEmpty(member.RankId))
				{
					displayText = member.Name + " (" + member.RankId + ")";
				}
				string tooltip = $"Rank Order: {member.RankOrder}";
				if (member.Joined.HasValue)
				{
					tooltip += $"\nJoined: {member.Joined.Value.ToLocalTime():g}";
				}
				if (member.WvwMember)
				{
					tooltip += "\nWvW Member";
				}
				MenuItem val2 = new MenuItem(displayText);
				((Control)val2).set_Parent((Container)(object)_menu);
				((Control)val2).set_BasicTooltipText(tooltip);
			}
		}

		public async Task RefreshAsync()
		{
			await LoadMembersAsync();
		}
	}
}
