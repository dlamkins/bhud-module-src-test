using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Guilds;

namespace Neokain.GW2.AllianceManager.Controls.Guilds
{
	public class GuildDetailsControl : Panel
	{
		private const int CTRL_PADDING = 5;

		private readonly Guid _guildId;

		private readonly Gw2WebClient _webClient;

		private GuildDetailDto _guildDetail;

		private readonly Label _labelForGuildName;

		private readonly Label _labelGuildName;

		private readonly Label _labelForGuildId;

		private readonly Label _labelGuildId;

		public GuildDetailsControl(Guid guildId, Gw2WebClient webClient)
			: this()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Expected O, but got Unknown
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Expected O, but got Unknown
			_guildId = guildId;
			_webClient = webClient ?? throw new ArgumentNullException("webClient");
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Left(10);
			((Control)val).set_Top(10);
			val.set_AutoSizeWidth(true);
			((Control)val).set_Height(24);
			val.set_Text("Guild Name:");
			_labelForGuildName = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Left(((Control)_labelForGuildName).get_Right() + 5);
			((Control)val2).set_Top(((Control)_labelForGuildName).get_Top());
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Height(24);
			val2.set_Text("Loading...");
			_labelGuildName = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Left(((Control)_labelForGuildName).get_Left());
			((Control)val3).set_Top(((Control)_labelForGuildName).get_Bottom() + 5);
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Height(24);
			val3.set_Text("Guild ID:");
			_labelForGuildId = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Left(((Control)_labelForGuildId).get_Right() + 5);
			((Control)val4).set_Top(((Control)_labelForGuildId).get_Top());
			val4.set_AutoSizeWidth(true);
			((Control)val4).set_Height(24);
			val4.set_Text(_guildId.ToString());
			_labelGuildId = val4;
			LoadGuildDetailsAsync();
		}

		private async Task LoadGuildDetailsAsync()
		{
			try
			{
				_guildDetail = await _webClient.GetGuildDetail(_guildId);
				UpdateDisplay();
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Failed to load guild details: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private void UpdateDisplay()
		{
			if (_guildDetail == null)
			{
				_labelGuildName.set_Text("N/A");
				return;
			}
			_labelGuildName.set_Text(_guildDetail.Name);
			_labelGuildId.set_Text(_guildDetail.Id.ToString());
		}
	}
}
