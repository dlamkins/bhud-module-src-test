using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.ArcDps.Common;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using KillProofModule.Manager;
using KillProofModule.Models;
using KillProofModule.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KillProofModule.Controls.Views
{
	public class MainView : IView
	{
		private const int TOP_MARGIN = 0;

		private const int RIGHT_MARGIN = 5;

		private const int BOTTOM_MARGIN = 10;

		private const int LEFT_MARGIN = 8;

		private Logger Logger = Logger.GetLogger<MainView>();

		private readonly Point LABEL_SMALL = new Point(400, 30);

		private static readonly Regex Gw2AccountName = new Regex(".{3,32}", RegexOptions.Compiled | RegexOptions.Singleline);

		private Texture2D _killProofMeLogoTexture;

		private List<PlayerButton> _displayedPlayers;

		private Panel _squadPanel;

		public event EventHandler<EventArgs> Loaded;

		public event EventHandler<EventArgs> Built;

		public event EventHandler<EventArgs> Unloaded;

		public MainView()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			_displayedPlayers = new List<PlayerButton>();
			KillProofModule.ModuleInstance.PartyManager.PlayerAdded += PlayerAddedEvent;
			KillProofModule.ModuleInstance.PartyManager.PlayerLeft += PlayerLeavesEvent;
		}

		public async Task<bool> DoLoad(IProgress<string> progress)
		{
			_killProofMeLogoTexture = KillProofModule.ModuleInstance.ContentsManager.GetTexture("killproof_logo.png");
			this.Loaded?.Invoke(this, EventArgs.Empty);
			return true;
		}

		public void DoBuild(Container buildPanel)
		{
			_squadPanel = BuildBody((Container)(object)BuildHeader(buildPanel));
			BuildFooter(_squadPanel);
			foreach (PlayerProfile profile in KillProofModule.ModuleInstance.PartyManager.Players)
			{
				AddPlayerButton(profile);
			}
			this.Built?.Invoke(this, EventArgs.Empty);
		}

		public void DoUnload()
		{
			_displayedPlayers.Clear();
			this.Unloaded?.Invoke(this, EventArgs.Empty);
		}

		private Panel BuildHeader(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected O, but got Unknown
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width(), 200));
			((Control)val).set_Location(new Point(0, 0));
			val.set_CanScroll(false);
			Panel header = val;
			Image val2 = new Image(AsyncTexture2D.op_Implicit(_killProofMeLogoTexture));
			((Control)val2).set_Parent((Container)(object)header);
			((Control)val2).set_Size(new Point(128, 128));
			((Control)val2).set_Location(new Point(18, 5));
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)header);
			((Control)val3).set_Size(new Point(300, 30));
			((Control)val3).set_Location(new Point(((Control)header).get_Width() / 2 - 100, ((Control)header).get_Height() / 2 + 30));
			val3.set_StrokeText(true);
			val3.set_ShowShadow(true);
			val3.set_Text(global::KillProofModule.Properties.Resources.Account_Name_or_KillProof_me_ID_);
			Label labAccountName = val3;
			TextBox val4 = new TextBox();
			((Control)val4).set_Parent((Container)(object)header);
			((Control)val4).set_Size(new Point(200, 30));
			((Control)val4).set_Location(new Point(((Control)header).get_Width() / 2 - 100, ((Control)labAccountName).get_Bottom()));
			((TextInputBase)val4).set_PlaceholderText("Player.0000");
			TextBox tbAccountName = val4;
			tbAccountName.add_EnterPressed((EventHandler<EventArgs>)delegate
			{
				LoadProfileView(((TextInputBase)tbAccountName).get_Text());
				((TextInputBase)tbAccountName).set_Focused(false);
			});
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)header);
			((Control)val5).set_Size(new Point(300, 40));
			val5.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0));
			val5.set_StrokeText(true);
			((Control)val5).set_Location(new Point(8, ((Control)header).get_Bottom() - 40));
			val5.set_Text(global::KillProofModule.Properties.Resources.Recent_profiles_);
			if (!KillProofModule.ModuleInstance.PartyManager.Self.HasKillProof())
			{
				KillProofModule.ModuleInstance.PartyManager.Self.KillProofChanged += OnSelfKillProofAvailable;
			}
			else
			{
				OnSelfKillProofAvailable(null, null);
			}
			return header;
			void OnSelfKillProofAvailable(object o, ValueEventArgs<KillProof> e)
			{
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0089: Expected O, but got Unknown
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Unknown result type (might be due to invalid IL or missing references)
				//IL_009a: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
				//IL_013f: Unknown result type (might be due to invalid IL or missing references)
				if (KillProofModule.ModuleInstance.PartyManager.Self.HasKillProof())
				{
					KillProofModule.ModuleInstance.PartyManager.Self.KillProofChanged -= OnSelfKillProofAvailable;
					Panel val6 = new Panel();
					((Control)val6).set_Parent((Container)(object)header);
					((Control)val6).set_Size(new Point(335, 114));
					val6.set_ShowBorder(true);
					val6.set_ShowTint(true);
					((Control)val6).set_Location(new Point(((Control)header).get_Right() - 335 - 5, 15));
					Panel selfButtonPanel = val6;
					Checkbox val7 = new Checkbox();
					((Control)val7).set_Parent((Container)(object)header);
					((Control)val7).set_Location(new Point(((Control)selfButtonPanel).get_Location().X + 8, ((Control)selfButtonPanel).get_Bottom()));
					((Control)val7).set_Size(new Point(((Control)selfButtonPanel).get_Width(), 30));
					val7.set_Text(global::KillProofModule.Properties.Resources.Show_Smart_Ping_Menu);
					((Control)val7).set_BasicTooltipText(global::KillProofModule.Properties.Resources.Shows_a_menu_on_the_top_left_corner_of_your_screen_which_allows_you_to_quickly_access_and_ping_your_killproofs_);
					val7.set_Checked(KillProofModule.ModuleInstance.SmartPingMenuEnabled.get_Value());
					val7.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent ev)
					{
						KillProofModule.ModuleInstance.SmartPingMenuEnabled.set_Value(ev.get_Checked());
					});
					PlayerButton playerButton = new PlayerButton(KillProofModule.ModuleInstance.PartyManager.Self);
					((Control)playerButton).set_Parent((Container)(object)selfButtonPanel);
					playerButton.IsNew = false;
					((Control)playerButton).set_Location(new Point(0, 0));
				}
			}
		}

		private Panel BuildBody(Container header)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Expected O, but got Unknown
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent(((Control)header).get_Parent());
			((Control)val).set_Size(new Point(((Control)header).get_Size().X, ((Control)((Control)header).get_Parent()).get_Height() - ((Control)header).get_Height() - 100));
			((Control)val).set_Location(new Point(0, ((Control)header).get_Bottom()));
			val.set_ShowBorder(true);
			val.set_CanScroll(true);
			val.set_ShowTint(true);
			Panel body = val;
			if (GameService.ArcDps.get_RenderPresent())
			{
				StandardButton val2 = new StandardButton();
				((Control)val2).set_Parent(((Control)header).get_Parent());
				((Control)val2).set_Size(new Point(100, 30));
				((Control)val2).set_Location(new Point(((Control)body).get_Right() - 100 - 5, ((Control)body).get_Bottom() + 10));
				val2.set_Text(global::KillProofModule.Properties.Resources.Clear);
				((Control)val2).set_BasicTooltipText(global::KillProofModule.Properties.Resources.Removes_profiles_of_players_which_are_not_in_squad_);
				StandardButton clearButton = val2;
				Checkbox val3 = new Checkbox();
				((Control)val3).set_Parent(((Control)header).get_Parent());
				((Control)val3).set_Size(new Point(20, 30));
				((Control)val3).set_Location(new Point(((Control)clearButton).get_Left() - 20 - 5, ((Control)clearButton).get_Location().Y));
				val3.set_Text("");
				((Control)val3).set_BasicTooltipText(global::KillProofModule.Properties.Resources.Remove_leavers_automatically_);
				val3.set_Checked(KillProofModule.ModuleInstance.AutomaticClearEnabled.get_Value());
				val3.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
				{
					KillProofModule.ModuleInstance.AutomaticClearEnabled.set_Value(e.get_Checked());
				});
				((Control)clearButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					PlayerButton[] array = _displayedPlayers.ToArray();
					foreach (PlayerButton c in array)
					{
						if (c == null)
						{
							_displayedPlayers.Remove(null);
						}
						else if (!GameService.ArcDps.get_Common().get_PlayersInSquad().Any(delegate(KeyValuePair<string, Player> p)
						{
							//IL_000d: Unknown result type (might be due to invalid IL or missing references)
							//IL_0012: Unknown result type (might be due to invalid IL or missing references)
							PlayerProfile playerProfile = c.PlayerProfile;
							Player value = p.Value;
							return playerProfile.IsOwner(((Player)(ref value)).get_AccountName());
						}))
						{
							_displayedPlayers.Remove(c);
							((Control)c).Dispose();
						}
					}
				});
			}
			return body;
		}

		private void BuildFooter(Panel body)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent(((Control)body).get_Parent());
			((Control)val).set_Size(LABEL_SMALL);
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val).set_Location(new Point(((Control)body).get_Width() / 2 - LABEL_SMALL.X / 2, ((Control)body).get_Bottom() + 10));
			val.set_StrokeText(true);
			val.set_ShowShadow(true);
			val.set_Text(global::KillProofModule.Properties.Resources.Powered_by_www_killproof_me);
		}

		private void PlayerAddedEvent(object o, ValueEventArgs<PlayerProfile> profile)
		{
			if (_displayedPlayers.FirstOrDefault((PlayerButton x) => x.PlayerProfile.Equals(profile.get_Value())) == null)
			{
				AddPlayerButton(profile.get_Value(), isNew: true);
			}
			RepositionPlayers();
		}

		private async void AddPlayerButton(PlayerProfile profile, bool isNew = false)
		{
			if (await KillProofApi.ProfileAvailable(profile.AccountName))
			{
				List<PlayerButton> displayedPlayers = _displayedPlayers;
				PlayerButton playerButton = new PlayerButton(profile);
				((Control)playerButton).set_Parent((Container)(object)_squadPanel);
				playerButton.IsNew = isNew;
				displayedPlayers.Add(playerButton);
				RepositionPlayers();
			}
		}

		public static void LoadProfileView(string searchTerm)
		{
			if (string.IsNullOrEmpty(searchTerm) || !Gw2AccountName.IsMatch(searchTerm))
			{
				return;
			}
			KillProofApi.GetKillProofContent(searchTerm).ContinueWith(delegate(Task<KillProof> kpResult)
			{
				if (kpResult.IsCompleted && !kpResult.IsFaulted)
				{
					KillProof result = kpResult.Result;
					if (result != null && string.IsNullOrEmpty(result.Error))
					{
						GameService.Overlay.get_BlishHudWindow().Navigate((IView)(object)new ProfileView(result), true);
					}
					else
					{
						GameService.Overlay.get_BlishHudWindow().Navigate((IView)(object)new NotFoundView(searchTerm), true);
					}
				}
			});
		}

		private void PlayerLeavesEvent(object o, ValueEventArgs<PlayerProfile> profile)
		{
			if (KillProofModule.ModuleInstance.AutomaticClearEnabled.get_Value())
			{
				PlayerButton profileBtn = _displayedPlayers.FirstOrDefault((PlayerButton x) => x.PlayerProfile.Equals(profile.get_Value()));
				_displayedPlayers.Remove(profileBtn);
				if (profileBtn != null)
				{
					((Control)profileBtn).Dispose();
				}
			}
		}

		private void RepositionPlayers()
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			IOrderedEnumerable<PlayerButton> orderedEnumerable = _displayedPlayers.OrderByDescending((PlayerButton player) => player.IsNew);
			int pos = 0;
			foreach (PlayerButton e in orderedEnumerable)
			{
				int x = pos % 3;
				int y = pos / 3;
				((Control)e).set_Location(new Point(x * (((Control)e).get_Width() + 8), y * (((Control)e).get_Height() + 8)));
				((Container)(Panel)((Control)e).get_Parent()).set_VerticalScrollOffset(0);
				((Control)((Control)e).get_Parent()).Invalidate();
				if (((Control)e).get_Visible())
				{
					pos++;
				}
			}
		}
	}
}
