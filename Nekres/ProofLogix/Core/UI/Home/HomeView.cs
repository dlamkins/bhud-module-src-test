using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;
using Nekres.ProofLogix.Core.Services.PartySync.Models;
using Nekres.ProofLogix.Core.UI.Clears;
using Nekres.ProofLogix.Core.UI.KpProfile;

namespace Nekres.ProofLogix.Core.UI.Home
{
	public class HomeView : View
	{
		private class AccountItemsView : View
		{
			private readonly List<AccountItem> _bank;

			private readonly List<AccountItem> _sharedBags;

			private readonly Dictionary<Character, List<AccountItem>> _bags;

			public AccountItemsView(List<AccountItem> bank, List<AccountItem> sharedBags, Dictionary<Character, List<AccountItem>> bags)
				: this()
			{
				_bank = bank;
				_sharedBags = sharedBags;
				_bags = bags;
			}

			protected override void Build(Container buildPanel)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Expected O, but got Unknown
				//IL_019e: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a6: Expected I4, but got Unknown
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent(buildPanel);
				((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
				((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
				((Panel)val).set_CanScroll(true);
				val.set_OuterControlPadding(new Vector2(4f, 7f));
				FlowPanel itemsPanel = val;
				buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_001d: Unknown result type (might be due to invalid IL or missing references)
					((Control)itemsPanel).set_Width(e.get_CurrentRegion().Width);
					((Control)itemsPanel).set_Height(e.get_CurrentRegion().Height);
				});
				AddItems(itemsPanel, _bank, "Account Vault", GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156699));
				AddItems(itemsPanel, _sharedBags, "Shared Inventory Slots", GameService.Content.get_DatAssetCache().GetTextureFromAssetId(1314214));
				foreach (KeyValuePair<Character, List<AccountItem>> bagsByChar in _bags)
				{
					IReadOnlyList<CharacterBuildTabSlot> buildTabs = bagsByChar.Key.get_BuildTabs();
					int elite = ((buildTabs != null) ? buildTabs.First((CharacterBuildTabSlot tab) => tab.get_IsActive()).get_Build().get_Specializations()[2].get_Id() : null).GetValueOrDefault();
					if (!Enum.TryParse<ProfessionType>(bagsByChar.Key.get_Profession(), ignoreCase: true, out ProfessionType profession))
					{
						ProofLogix.Logger.Warn("Unable to cast '{0}' to {1}.", new object[2]
						{
							bagsByChar.Key.get_Profession(),
							"ProfessionType"
						});
					}
					else
					{
						AddItems(itemsPanel, bagsByChar.Value, bagsByChar.Key.get_Name(), ProofLogix.Instance.Resources.GetClassIcon((int)profession, elite));
					}
				}
				((View<IPresenter>)this).Build(buildPanel);
			}

			private void AddItems(FlowPanel parent, List<AccountItem> items, string category, AsyncTexture2D icon)
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				if (!items.Any())
				{
					return;
				}
				FlowPanelWithIcon flowPanelWithIcon = new FlowPanelWithIcon(icon);
				((Control)flowPanelWithIcon).set_Parent((Container)(object)parent);
				((Control)flowPanelWithIcon).set_Width(((Container)parent).get_ContentRegion().Width - 24);
				((Container)flowPanelWithIcon).set_HeightSizingMode((SizingMode)1);
				((Panel)flowPanelWithIcon).set_Title(category);
				((Panel)flowPanelWithIcon).set_CanCollapse(true);
				((Panel)flowPanelWithIcon).set_CanScroll(true);
				((FlowPanel)flowPanelWithIcon).set_OuterControlPadding(new Vector2(5f, 5f));
				((FlowPanel)flowPanelWithIcon).set_ControlPadding(new Vector2(5f, 5f));
				FlowPanelWithIcon slotsCategory = flowPanelWithIcon;
				((Container)parent).add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					((Control)slotsCategory).set_Width(e.get_CurrentRegion().Width - 24);
				});
				foreach (AccountItem item in items)
				{
					string name = ProofLogix.Instance.Resources.GetItems().FirstOrDefault((Resource i) => i.Id.Equals(item.get_Id()))?.Name;
					if (!string.IsNullOrEmpty(name))
					{
						name = $"{item.get_Count()}x {name}";
					}
					ItemWithAmount itemWithAmount = new ItemWithAmount(ProofLogix.Instance.Resources.GetItem(item.get_Id()).Icon);
					((Control)itemWithAmount).set_Parent((Container)(object)slotsCategory);
					((Control)itemWithAmount).set_Width(64);
					((Control)itemWithAmount).set_Height(64);
					itemWithAmount.Amount = item.get_Count();
					((Control)itemWithAmount).set_BasicTooltipText(name);
				}
			}
		}

		private AsyncTexture2D _kpIcon;

		public HomeView()
			: this()
		{
			_kpIcon = AsyncTexture2D.op_Implicit(ProofLogix.Instance.ContentsManager.GetTexture("killproof_icon.png"));
		}

		protected override void Unload()
		{
			_kpIcon.Dispose();
			((View<IPresenter>)this).Unload();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Expected O, but got Unknown
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Expected O, but got Unknown
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Expected O, but got Unknown
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Expected O, but got Unknown
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Expected O, but got Unknown
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(200);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
			val.set_CanScroll(true);
			val.set_Title("Game Account");
			Panel menuPanel = val;
			Menu val2 = new Menu();
			((Control)val2).set_Parent((Container)(object)menuPanel);
			((Control)val2).set_Top(0);
			((Control)val2).set_Left(0);
			((Control)val2).set_Width(((Container)menuPanel).get_ContentRegion().Width);
			((Control)val2).set_Height(((Container)menuPanel).get_ContentRegion().Height);
			Menu navMenu = val2;
			MenuItem val3 = new MenuItem();
			((Control)val3).set_Parent((Container)(object)navMenu);
			val3.set_Text("Owned Proofs");
			((Control)val3).set_Width(((Container)navMenu).get_ContentRegion().Width);
			val3.set_Icon(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156699));
			MenuItem proofsEntry = val3;
			MenuItem val4 = new MenuItem();
			((Control)val4).set_Parent((Container)(object)navMenu);
			val4.set_Text("Weekly Clears");
			((Control)val4).set_Width(((Container)navMenu).get_ContentRegion().Width);
			val4.set_Icon(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(1234912));
			MenuItem clearsEntry = val4;
			MenuItem val5 = new MenuItem();
			((Control)val5).set_Parent((Container)(object)navMenu);
			val5.set_Text("Other");
			((Control)val5).set_Width(((Container)navMenu).get_ContentRegion().Width);
			val5.set_Collapsed(false);
			MenuItem separatorEntry = val5;
			((Control)separatorEntry).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ProofLogix.Instance.Resources.PlayMenuClick();
			});
			MenuItem val6 = new MenuItem();
			((Control)val6).set_Parent((Container)(object)separatorEntry);
			val6.set_Text("My Profile");
			((Control)val6).set_Width(((Container)navMenu).get_ContentRegion().Width);
			val6.set_Icon(_kpIcon);
			MenuItem myProfileEntry = val6;
			MenuItem val7 = new MenuItem();
			((Control)val7).set_Parent((Container)(object)separatorEntry);
			val7.set_Text("Party Table");
			((Control)val7).set_Width(((Container)navMenu).get_ContentRegion().Width);
			val7.set_Icon(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156407));
			MenuItem squadTableEntry = val7;
			ViewContainer val8 = new ViewContainer();
			((Control)val8).set_Parent(buildPanel);
			((Control)val8).set_Left(((Control)menuPanel).get_Right());
			((Control)val8).set_Width(buildPanel.get_ContentRegion().Width - ((Control)menuPanel).get_Width());
			((Control)val8).set_Height(buildPanel.get_ContentRegion().Height);
			((Panel)val8).set_ShowBorder(true);
			((Control)val8).set_BasicTooltipText("Shows the current snapshot of your account data.\nEnables you to verify if newly acquired progress is\nalready available for tracking by third-parties.");
			ViewContainer plyPanel = val8;
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				((Control)plyPanel).set_Width(e.get_CurrentRegion().Width - ((Control)menuPanel).get_Width());
				((Control)plyPanel).set_Height(e.get_CurrentRegion().Height);
			});
			((Container)menuPanel).add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				((Control)navMenu).set_Width(e.get_CurrentRegion().Width);
				((Control)navMenu).set_Height(e.get_CurrentRegion().Height);
			});
			((Container)navMenu).add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				((Control)proofsEntry).set_Width(e.get_CurrentRegion().Width);
				((Control)clearsEntry).set_Height(e.get_CurrentRegion().Height);
				((Control)separatorEntry).set_Width(e.get_CurrentRegion().Width);
				((Control)myProfileEntry).set_Height(e.get_CurrentRegion().Height);
				((Control)squadTableEntry).set_Height(e.get_CurrentRegion().Height);
			});
			((Control)proofsEntry).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (!IsApiAvailable())
				{
					GameService.Content.PlaySoundEffectByName("error");
				}
				else
				{
					ProofLogix.Instance.Resources.PlayMenuItemClick();
					AsyncString loadingText = new AsyncString();
					plyPanel.Show((IView)(object)new LoadingView("Loading items..", loadingText));
					loadingText.String = "Turning Bank upside down.";
					List<AccountItem> bank = await ProofLogix.Instance.Gw2WebApi.GetBank();
					loadingText.String = "Borrowing bag slots.";
					List<AccountItem> sharedBags = await ProofLogix.Instance.Gw2WebApi.GetSharedBags();
					loadingText.String = "Tickling characters.";
					Dictionary<Character, List<AccountItem>> bagsByCharacters = await ProofLogix.Instance.Gw2WebApi.GetBagsByCharacter();
					plyPanel.Show((IView)(object)new AccountItemsView(bank, sharedBags, bagsByCharacters));
				}
			});
			((Control)clearsEntry).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (!IsApiAvailable())
				{
					GameService.Content.PlaySoundEffectByName("error");
				}
				else
				{
					ProofLogix.Instance.Resources.PlayMenuItemClick();
					plyPanel.Show((IView)(object)new LoadingView("Loading clears.."));
					ViewContainer val9 = plyPanel;
					val9.Show((IView)(object)new ClearsView(await ProofLogix.Instance.Gw2WebApi.GetClears()));
				}
			});
			((Control)myProfileEntry).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MumblePlayer localPlayer = ProofLogix.Instance.PartySync.LocalPlayer;
				if (!localPlayer.HasKpProfile)
				{
					GameService.Content.PlaySoundEffectByName("error");
					ScreenNotification.ShowNotification("Not yet loaded. Please, try again.", (NotificationType)2, (Texture2D)null, 4);
				}
				else
				{
					ProofLogix.Instance.Resources.PlayMenuItemClick();
					if (localPlayer.KpProfile.NotFound)
					{
						ProofLogix.Instance.ToggleRegisterWindow();
					}
					else
					{
						ProfileView.Open(localPlayer.KpProfile);
					}
				}
			});
			((Control)squadTableEntry).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ProofLogix.Instance.Resources.PlayMenuItemClick();
				ProofLogix.Instance.ToggleTable();
			});
			((View<IPresenter>)this).Build(buildPanel);
		}

		private bool IsApiAvailable()
		{
			if (string.IsNullOrWhiteSpace(GameService.Gw2Mumble.get_PlayerCharacter().get_Name()))
			{
				ScreenNotification.ShowNotification("API unavailable. Please, login to a character.", (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			if (!ProofLogix.Instance.Gw2WebApi.HasSubtoken)
			{
				ScreenNotification.ShowNotification("Missing API key. Please, add an API key to BlishHUD.", (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			if (ProofLogix.Instance.Gw2WebApi.MissingPermissions.Any())
			{
				string missing = string.Join(", ", ProofLogix.Instance.Gw2WebApi.MissingPermissions);
				ScreenNotification.ShowNotification("Insufficient API permissions.\nRequired: " + missing, (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			return true;
		}
	}
}
