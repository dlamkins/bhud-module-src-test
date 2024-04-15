using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
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
				//IL_012e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0133: Unknown result type (might be due to invalid IL or missing references)
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
					Resource resource = ProofLogix.Instance.Resources.GetItem(item.get_Id());
					ItemWithAmount itemWithAmount = new ItemWithAmount(ProofLogix.Instance.Resources.GetItem(item.get_Id()).Icon);
					((Control)itemWithAmount).set_Parent((Container)(object)slotsCategory);
					((Control)itemWithAmount).set_Width(64);
					((Control)itemWithAmount).set_Height(64);
					itemWithAmount.Amount = item.get_Count();
					((Control)itemWithAmount).set_BasicTooltipText(AssetUtil.GetItemDisplayName(resource.Name, item.get_Count(), brackets: false));
					itemWithAmount.BorderColor = ProofLogix.Instance.Resources.GetItem(item.get_Id()).Rarity.AsColor();
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
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Expected O, but got Unknown
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Expected O, but got Unknown
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Expected O, but got Unknown
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Expected O, but got Unknown
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Expected O, but got Unknown
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Expected O, but got Unknown
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Expected O, but got Unknown
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Expected O, but got Unknown
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
			((Control)val3).set_BasicTooltipText("Shows the current snapshot of your inventories.\nEnables you to verify if recent rewarded proofs are\nalready available to be recorded by killproof.me.");
			MenuItem proofsEntry = val3;
			MenuItem val4 = new MenuItem();
			((Control)val4).set_Parent((Container)(object)navMenu);
			val4.set_Text("Weekly Clears");
			((Control)val4).set_Width(((Container)navMenu).get_ContentRegion().Width);
			val4.set_Icon(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(1234912));
			((Control)val4).set_BasicTooltipText("Shows the current snapshot of your clears.\nEnables you to verify if recent completed encounters are\nalready available to be recorded by killproof.me.");
			MenuItem clearsEntry = val4;
			MenuItem val5 = new MenuItem();
			((Control)val5).set_Parent((Container)(object)navMenu);
			val5.set_Text("Other");
			((Control)val5).set_Width(((Container)navMenu).get_ContentRegion().Width);
			val5.set_Collapsed(false);
			MenuItem separatorEntry = val5;
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
			MenuItem val8 = new MenuItem();
			((Control)val8).set_Parent((Container)(object)separatorEntry);
			val8.set_Text("Smart Ping");
			((Control)val8).set_Width(((Container)navMenu).get_ContentRegion().Width);
			val8.set_Icon(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155157));
			MenuItem smartPingEntry = val8;
			ViewContainer val9 = new ViewContainer();
			((Control)val9).set_Parent(buildPanel);
			((Control)val9).set_Left(((Control)menuPanel).get_Right());
			((Control)val9).set_Width(buildPanel.get_ContentRegion().Width - ((Control)menuPanel).get_Width());
			((Control)val9).set_Height(buildPanel.get_ContentRegion().Height);
			((Panel)val9).set_ShowBorder(true);
			ViewContainer plyPanel = val9;
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
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				((Control)proofsEntry).set_Width(e.get_CurrentRegion().Width);
				((Control)clearsEntry).set_Height(e.get_CurrentRegion().Height);
				((Control)separatorEntry).set_Width(e.get_CurrentRegion().Width);
				((Control)myProfileEntry).set_Height(e.get_CurrentRegion().Height);
				((Control)squadTableEntry).set_Height(e.get_CurrentRegion().Height);
				((Control)smartPingEntry).set_Height(e.get_CurrentRegion().Height);
			});
			((Control)proofsEntry).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (!ProofLogix.Instance.Resources.HasLoaded())
				{
					GameService.Content.PlaySoundEffectByName("error");
				}
				else if (!ProofLogix.Instance.Gw2WebApi.IsApiAvailable())
				{
					GameService.Content.PlaySoundEffectByName("error");
				}
				else
				{
					AsyncString loadingText = new AsyncString();
					plyPanel.Show((IView)(object)new LoadingView("Loading items…", loadingText));
					loadingText.String = ProofLogix.Instance.Resources.GetLoadingSubtitle();
					List<AccountItem> bank = await ProofLogix.Instance.Gw2WebApi.GetBank();
					loadingText.String = ProofLogix.Instance.Resources.GetLoadingSubtitle();
					List<AccountItem> sharedBags = await ProofLogix.Instance.Gw2WebApi.GetSharedBags();
					loadingText.String = ProofLogix.Instance.Resources.GetLoadingSubtitle();
					Dictionary<Character, List<AccountItem>> bagsByCharacters = await ProofLogix.Instance.Gw2WebApi.GetBagsByCharacter();
					plyPanel.Show((IView)(object)new AccountItemsView(bank, sharedBags, bagsByCharacters));
				}
			});
			((Control)clearsEntry).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (!ProofLogix.Instance.Resources.HasLoaded())
				{
					GameService.Content.PlaySoundEffectByName("error");
				}
				else if (!ProofLogix.Instance.Gw2WebApi.IsApiAvailable())
				{
					GameService.Content.PlaySoundEffectByName("error");
				}
				else
				{
					plyPanel.Show((IView)(object)new LoadingView("Loading clears.."));
					ViewContainer val10 = plyPanel;
					val10.Show((IView)(object)new ClearsView(await ProofLogix.Instance.Gw2WebApi.GetClears()));
				}
			});
			((Control)myProfileEntry).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!ProofLogix.Instance.Resources.HasLoaded())
				{
					GameService.Content.PlaySoundEffectByName("error");
				}
				else
				{
					MumblePlayer localPlayer = ProofLogix.Instance.PartySync.LocalPlayer;
					if (!localPlayer.HasKpProfile)
					{
						GameService.Content.PlaySoundEffectByName("error");
						if (string.IsNullOrWhiteSpace(GameService.Gw2Mumble.get_PlayerCharacter().get_Name()))
						{
							ScreenNotification.ShowNotification("Profile unavailable. Please, login to a character.", (NotificationType)2, (Texture2D)null, 4);
						}
						else
						{
							ScreenNotification.ShowNotification("Not yet loaded. Please, try again.", (NotificationType)2, (Texture2D)null, 4);
						}
					}
					else if (localPlayer.KpProfile.NotFound)
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
				ProofLogix.Instance.ToggleTable();
			});
			((Control)smartPingEntry).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ProofLogix.Instance.ToggleSmartPing();
			});
			((View<IPresenter>)this).Build(buildPanel);
		}
	}
}
