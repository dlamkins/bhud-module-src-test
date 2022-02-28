using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using KillProofModule.Models;
using KillProofModule.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KillProofModule.Controls.Views
{
	public class ProfileView : IView
	{
		private const int TOP_MARGIN = 0;

		private const int RIGHT_MARGIN = 5;

		private const int BOTTOM_MARGIN = 10;

		private const int LEFT_MARGIN = 8;

		private Logger Logger = Logger.GetLogger<ProfileView>();

		private readonly Point LABEL_BIG = new Point(400, 40);

		private readonly Point LABEL_SMALL = new Point(400, 30);

		private string _currentSortMethod;

		private Texture2D _sortByWorldBossesTexture;

		private Texture2D _sortByTokenTexture;

		private Texture2D _sortByTitleTexture;

		private Texture2D _sortByRaidTexture;

		private Texture2D _sortByFractalTexture;

		private readonly List<KillProofButton> _displayedKillProofs;

		private readonly KillProof _profile;

		public event EventHandler<EventArgs> Loaded;

		public event EventHandler<EventArgs> Built;

		public event EventHandler<EventArgs> Unloaded;

		public ProfileView(KillProof profile)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			_profile = profile;
			_displayedKillProofs = new List<KillProofButton>();
			_currentSortMethod = global::KillProofModule.Properties.Resources.Everything;
		}

		public async Task<bool> DoLoad(IProgress<string> progress)
		{
			_sortByWorldBossesTexture = KillProofModule.ModuleInstance.ContentsManager.GetTexture("world-bosses.png");
			_sortByTokenTexture = KillProofModule.ModuleInstance.ContentsManager.GetTexture("icon_token.png");
			_sortByTitleTexture = KillProofModule.ModuleInstance.ContentsManager.GetTexture("icon_title.png");
			_sortByRaidTexture = KillProofModule.ModuleInstance.ContentsManager.GetTexture("icon_raid.png");
			_sortByFractalTexture = KillProofModule.ModuleInstance.ContentsManager.GetTexture("icon_fractal.png");
			return true;
		}

		public void DoBuild(Container buildPanel)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			BuildFooter(BuildBody(BuildHeader(buildPanel)));
			BackButton val = new BackButton((WindowBase)(object)GameService.Overlay.get_BlishHudWindow());
			val.set_Text(KillProofModule.ModuleInstance.KillProofTabName);
			val.set_NavTitle(global::KillProofModule.Properties.Resources.Profile);
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Location(new Point(20, 20));
		}

		public void DoUnload()
		{
		}

		private Panel BuildHeader(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Expected O, but got Unknown
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Expected O, but got Unknown
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Expected O, but got Unknown
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Expected O, but got Unknown
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Expected O, but got Unknown
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f4: Expected O, but got Unknown
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0451: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0466: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_049e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width(), 200));
			((Control)val).set_Location(new Point(0, 0));
			val.set_CanScroll(false);
			Panel header = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)header);
			((Control)val2).set_Size(LABEL_BIG);
			((Control)val2).set_Location(new Point(8, 90));
			val2.set_ShowShadow(true);
			val2.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)36, (FontStyle)0));
			val2.set_Text(_profile.AccountName);
			Label currentAccountName = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)header);
			((Control)val3).set_Size(LABEL_SMALL);
			((Control)val3).set_Location(new Point(8, ((Control)currentAccountName).get_Bottom() + 10));
			val3.set_Text(global::KillProofModule.Properties.Resources.Last_Refresh_ + $" {_profile.LastRefresh:dddd, d. MMMM yyyy - HH:mm:ss}");
			Label currentAccountLastRefresh = val3;
			Panel val4 = new Panel();
			((Control)val4).set_Parent((Container)(object)header);
			((Control)val4).set_Size(new Point(260, 32));
			((Control)val4).set_Location(new Point(((Control)header).get_Right() - 310 - 5, ((Control)currentAccountLastRefresh).get_Location().Y));
			val4.set_ShowTint(true);
			Panel sortingsMenu = val4;
			Image val5 = new Image();
			((Control)val5).set_Parent((Container)(object)sortingsMenu);
			((Control)val5).set_Size(new Point(32, 32));
			((Control)val5).set_Location(new Point(5, 0));
			val5.set_Texture(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("255369")));
			((Control)val5).set_BackgroundColor(Color.get_Transparent());
			((Control)val5).set_BasicTooltipText(global::KillProofModule.Properties.Resources.Everything);
			Image bSortByAll = val5;
			((Control)bSortByAll).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)UpdateSort);
			((Control)bSortByAll).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)MousePressedSortButton);
			((Control)bSortByAll).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			((Control)bSortByAll).add_MouseLeft((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			Image val6 = new Image();
			((Control)val6).set_Parent((Container)(object)sortingsMenu);
			((Control)val6).set_Size(new Point(32, 32));
			((Control)val6).set_Location(new Point(((Control)bSortByAll).get_Right() + 20 + 5, 0));
			val6.set_Texture(AsyncTexture2D.op_Implicit(_sortByWorldBossesTexture));
			((Control)val6).set_BasicTooltipText(global::KillProofModule.Properties.Resources.Progress_Proofs);
			Image bSortByKillProof = val6;
			((Control)bSortByKillProof).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)UpdateSort);
			((Control)bSortByKillProof).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)MousePressedSortButton);
			((Control)bSortByKillProof).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			((Control)bSortByKillProof).add_MouseLeft((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			Image val7 = new Image();
			((Control)val7).set_Parent((Container)(object)sortingsMenu);
			((Control)val7).set_Size(new Point(32, 32));
			((Control)val7).set_Location(new Point(((Control)bSortByKillProof).get_Right() + 5, 0));
			val7.set_Texture(AsyncTexture2D.op_Implicit(_sortByTokenTexture));
			((Control)val7).set_BasicTooltipText(global::KillProofModule.Properties.Resources.Tokens);
			Image bSortByToken = val7;
			((Control)bSortByToken).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)UpdateSort);
			((Control)bSortByToken).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)MousePressedSortButton);
			((Control)bSortByToken).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			((Control)bSortByToken).add_MouseLeft((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			Image val8 = new Image();
			((Control)val8).set_Parent((Container)(object)sortingsMenu);
			((Control)val8).set_Size(new Point(32, 32));
			((Control)val8).set_Location(new Point(((Control)bSortByToken).get_Right() + 20 + 5, 0));
			val8.set_Texture(AsyncTexture2D.op_Implicit(_sortByTitleTexture));
			((Control)val8).set_BasicTooltipText(global::KillProofModule.Properties.Resources.Titles);
			Image bSortByTitle = val8;
			((Control)bSortByTitle).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)UpdateSort);
			((Control)bSortByTitle).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)MousePressedSortButton);
			((Control)bSortByTitle).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			((Control)bSortByTitle).add_MouseLeft((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			Image val9 = new Image();
			((Control)val9).set_Parent((Container)(object)sortingsMenu);
			((Control)val9).set_Size(new Point(32, 32));
			((Control)val9).set_Location(new Point(((Control)bSortByTitle).get_Right() + 5, 0));
			val9.set_Texture(AsyncTexture2D.op_Implicit(_sortByRaidTexture));
			((Control)val9).set_BasicTooltipText(global::KillProofModule.Properties.Resources.Raid_Titles);
			Image bSortByRaid = val9;
			((Control)bSortByRaid).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)UpdateSort);
			((Control)bSortByRaid).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)MousePressedSortButton);
			((Control)bSortByRaid).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			((Control)bSortByRaid).add_MouseLeft((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			Image val10 = new Image();
			((Control)val10).set_Parent((Container)(object)sortingsMenu);
			((Control)val10).set_Size(new Point(32, 32));
			((Control)val10).set_Location(new Point(((Control)bSortByRaid).get_Right() + 5, 0));
			val10.set_Texture(AsyncTexture2D.op_Implicit(_sortByFractalTexture));
			((Control)val10).set_BasicTooltipText(global::KillProofModule.Properties.Resources.Fractal_Titles);
			((Control)val10).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)UpdateSort);
			((Control)val10).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)MousePressedSortButton);
			((Control)val10).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			((Control)val10).add_MouseLeft((EventHandler<MouseEventArgs>)MouseLeftSortButton);
			return header;
		}

		private void MousePressedSortButton(object sender, MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			Control bSortMethod = (Control)sender;
			bSortMethod.set_Size(new Point(bSortMethod.get_Size().X - 4, bSortMethod.get_Size().Y - 4));
		}

		private void MouseLeftSortButton(object sender, MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			((Control)sender).set_Size(new Point(32, 32));
		}

		private void UpdateSort(object sender, EventArgs e)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			if (sender != null)
			{
				_currentSortMethod = ((Control)sender).get_BasicTooltipText();
			}
			if (_currentSortMethod.Equals(global::KillProofModule.Properties.Resources.Everything, StringComparison.InvariantCultureIgnoreCase))
			{
				_displayedKillProofs.Sort(delegate(KillProofButton e1, KillProofButton e2)
				{
					int num = e1.IsTitleDisplay.CompareTo(e2.IsTitleDisplay);
					return (num != 0) ? num : string.Compare(e1.BottomText, e2.BottomText, StringComparison.InvariantCultureIgnoreCase);
				});
				foreach (KillProofButton displayedKillProof in _displayedKillProofs)
				{
					((Control)displayedKillProof).set_Visible(true);
				}
			}
			else if (_currentSortMethod.Equals(global::KillProofModule.Properties.Resources.Progress_Proofs, StringComparison.InvariantCultureIgnoreCase))
			{
				_displayedKillProofs.Sort((KillProofButton e1, KillProofButton e2) => string.Compare(e1.BottomText, e2.BottomText, StringComparison.InvariantCultureIgnoreCase));
				foreach (KillProofButton e4 in _displayedKillProofs)
				{
					((Control)e4).set_Visible(_profile.Killproofs != null && _profile.Killproofs.Any((Token x) => x.Name.Equals(((DetailsButton)e4).get_Text(), StringComparison.InvariantCultureIgnoreCase)));
				}
			}
			else if (_currentSortMethod.Equals(global::KillProofModule.Properties.Resources.Tokens, StringComparison.InvariantCultureIgnoreCase))
			{
				_displayedKillProofs.Sort((KillProofButton e1, KillProofButton e2) => string.Compare(e1.BottomText, e2.BottomText, StringComparison.InvariantCultureIgnoreCase));
				foreach (KillProofButton e3 in _displayedKillProofs)
				{
					((Control)e3).set_Visible(_profile.Tokens != null && _profile.Tokens.Any((Token x) => x.Name.Equals(((DetailsButton)e3).get_Text(), StringComparison.InvariantCultureIgnoreCase)));
				}
			}
			else if (_currentSortMethod.Equals(global::KillProofModule.Properties.Resources.Titles, StringComparison.InvariantCultureIgnoreCase))
			{
				_displayedKillProofs.Sort((KillProofButton e1, KillProofButton e2) => string.Compare(e1.BottomText, e2.BottomText, StringComparison.InvariantCultureIgnoreCase));
				foreach (KillProofButton displayedKillProof2 in _displayedKillProofs)
				{
					((Control)displayedKillProof2).set_Visible(displayedKillProof2.IsTitleDisplay);
				}
			}
			else if (_currentSortMethod.Equals(global::KillProofModule.Properties.Resources.Fractal_Titles, StringComparison.InvariantCultureIgnoreCase))
			{
				_displayedKillProofs.Sort((KillProofButton e1, KillProofButton e2) => string.Compare(((DetailsButton)e1).get_Text(), ((DetailsButton)e2).get_Text(), StringComparison.InvariantCultureIgnoreCase));
				foreach (KillProofButton displayedKillProof3 in _displayedKillProofs)
				{
					((Control)displayedKillProof3).set_Visible(displayedKillProof3.BottomText.ToLower().Contains("fractal"));
				}
			}
			else if (_currentSortMethod.Equals(global::KillProofModule.Properties.Resources.Raid_Titles, StringComparison.InvariantCultureIgnoreCase))
			{
				_displayedKillProofs.Sort((KillProofButton e1, KillProofButton e2) => string.Compare(((DetailsButton)e1).get_Text(), ((DetailsButton)e2).get_Text(), StringComparison.InvariantCultureIgnoreCase));
				foreach (KillProofButton displayedKillProof4 in _displayedKillProofs)
				{
					((Control)displayedKillProof4).set_Visible(displayedKillProof4.BottomText.ToLower().Contains("raid"));
				}
			}
			RepositionKillProofs();
		}

		private Panel BuildBody(Panel header)
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
			Panel val = new Panel();
			((Control)val).set_Parent(((Control)header).get_Parent());
			((Control)val).set_Size(new Point(((Control)header).get_Size().X, ((Control)((Control)header).get_Parent()).get_Height() - ((Control)header).get_Height() - 100));
			((Control)val).set_Location(new Point(0, ((Control)header).get_Bottom()));
			val.set_ShowBorder(true);
			val.set_CanScroll(true);
			val.set_ShowTint(true);
			Panel contentPanel = val;
			if (_profile.Killproofs != null)
			{
				foreach (Token killproof in _profile.Killproofs)
				{
					KillProofButton killProofButton3 = new KillProofButton();
					((Control)killProofButton3).set_Parent((Container)(object)contentPanel);
					((DetailsButton)killProofButton3).set_Icon(KillProofModule.ModuleInstance.GetTokenRender(killproof.Id));
					killProofButton3.Font = GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)0);
					((DetailsButton)killProofButton3).set_Text(killproof.Name);
					killProofButton3.BottomText = killproof.Amount.ToString();
					KillProofButton killProofButton2 = killProofButton3;
					_displayedKillProofs.Add(killProofButton2);
				}
			}
			else
			{
				Logger.Info("PlayerProfile '" + _profile.AccountName + "' has LI details explicitly hidden.");
			}
			if (_profile.Tokens != null)
			{
				foreach (Token token in _profile.Tokens)
				{
					KillProofButton killProofButton4 = new KillProofButton();
					((Control)killProofButton4).set_Parent((Container)(object)contentPanel);
					((DetailsButton)killProofButton4).set_Icon(KillProofModule.ModuleInstance.GetTokenRender(token.Id));
					killProofButton4.Font = GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)0);
					((DetailsButton)killProofButton4).set_Text(token.Name);
					killProofButton4.BottomText = token.Amount.ToString();
					KillProofButton killProofButton = killProofButton4;
					_displayedKillProofs.Add(killProofButton);
				}
			}
			else
			{
				Logger.Info("PlayerProfile '" + _profile.AccountName + "' has tokens explicitly hidden.");
			}
			if (_profile.Titles != null)
			{
				foreach (Title title in _profile.Titles)
				{
					KillProofButton killProofButton5 = new KillProofButton();
					((Control)killProofButton5).set_Parent((Container)(object)contentPanel);
					killProofButton5.Font = GameService.Content.get_DefaultFont16();
					((DetailsButton)killProofButton5).set_Text(title.Name);
					killProofButton5.BottomText = title.Mode.ToString();
					killProofButton5.IsTitleDisplay = true;
					KillProofButton titleButton = killProofButton5;
					switch (title.Mode)
					{
					case Mode.Raid:
						((DetailsButton)titleButton).set_Icon(AsyncTexture2D.op_Implicit(_sortByRaidTexture));
						break;
					case Mode.Fractal:
						((DetailsButton)titleButton).set_Icon(AsyncTexture2D.op_Implicit(_sortByFractalTexture));
						break;
					}
					_displayedKillProofs.Add(titleButton);
				}
			}
			else
			{
				Logger.Info("PlayerProfile '" + _profile.AccountName + "' has titles and achievements explicitly hidden.");
			}
			RepositionKillProofs();
			return contentPanel;
		}

		private void BuildFooter(Panel body)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Expected O, but got Unknown
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent(((Control)body).get_Parent());
			((Control)val).set_Size(LABEL_SMALL);
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			((Control)val).set_Location(new Point(8, ((Control)body).get_Bottom() + 10));
			val.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)11, (FontStyle)0));
			val.set_Text(global::KillProofModule.Properties.Resources.ID_ + " " + _profile.KpId);
			Label currentAccountKpId = val;
			Label val2 = new Label();
			((Control)val2).set_Parent(((Control)body).get_Parent());
			((Control)val2).set_Size(LABEL_SMALL);
			val2.set_HorizontalAlignment((HorizontalAlignment)0);
			((Control)val2).set_Location(new Point(8, ((Control)currentAccountKpId).get_Location().Y + 10 + 2));
			val2.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)11, (FontStyle)0));
			val2.set_Text(_profile.ProofUrl);
			Label currentAccountProofUrl = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent(((Control)body).get_Parent());
			((Control)val3).set_Size(LABEL_SMALL);
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val3).set_Location(new Point(((Control)body).get_Width() / 2 - LABEL_SMALL.X / 2, ((Control)body).get_Bottom() + 10));
			val3.set_StrokeText(true);
			val3.set_ShowShadow(true);
			val3.set_Text(global::KillProofModule.Properties.Resources.Powered_by_www_killproof_me);
			if (Uri.IsWellFormedUriString(currentAccountProofUrl.get_Text(), UriKind.Absolute))
			{
				((Control)currentAccountProofUrl).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					currentAccountProofUrl.set_TextColor(Color.get_LightBlue());
				});
				((Control)currentAccountProofUrl).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					currentAccountProofUrl.set_TextColor(Color.get_White());
				});
				((Control)currentAccountProofUrl).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0015: Unknown result type (might be due to invalid IL or missing references)
					currentAccountProofUrl.set_TextColor(new Color(206, 174, 250));
				});
				((Control)currentAccountProofUrl).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					currentAccountProofUrl.set_TextColor(Color.get_LightBlue());
					Process.Start(currentAccountProofUrl.get_Text());
				});
			}
		}

		private void RepositionKillProofs()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			int pos = 0;
			foreach (KillProofButton e in _displayedKillProofs)
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
