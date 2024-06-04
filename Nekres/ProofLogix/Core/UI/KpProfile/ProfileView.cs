using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;
using Nekres.ProofLogix.Core.UI.Clears;

namespace Nekres.ProofLogix.Core.UI.KpProfile
{
	public class ProfileView : View
	{
		private sealed class ItemsView : View
		{
			private readonly Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile _profile;

			public ItemsView(Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile profile)
				: this()
			{
				_profile = profile;
			}

			protected override void Build(Container buildPanel)
			{
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ed: Expected O, but got Unknown
				//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
				//IL_0106: Unknown result type (might be due to invalid IL or missing references)
				//IL_010d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0114: Unknown result type (might be due to invalid IL or missing references)
				//IL_0123: Unknown result type (might be due to invalid IL or missing references)
				//IL_012a: Unknown result type (might be due to invalid IL or missing references)
				//IL_013e: Expected O, but got Unknown
				//IL_013f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0144: Unknown result type (might be due to invalid IL or missing references)
				//IL_0150: Unknown result type (might be due to invalid IL or missing references)
				//IL_015b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0162: Unknown result type (might be due to invalid IL or missing references)
				//IL_0171: Unknown result type (might be due to invalid IL or missing references)
				//IL_018b: Unknown result type (might be due to invalid IL or missing references)
				//IL_019b: Expected O, but got Unknown
				//IL_019c: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f8: Expected O, but got Unknown
				//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
				//IL_020a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0215: Unknown result type (might be due to invalid IL or missing references)
				//IL_021c: Unknown result type (might be due to invalid IL or missing references)
				//IL_022b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0245: Unknown result type (might be due to invalid IL or missing references)
				//IL_0255: Expected O, but got Unknown
				//IL_0256: Unknown result type (might be due to invalid IL or missing references)
				//IL_025b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0262: Unknown result type (might be due to invalid IL or missing references)
				//IL_0273: Unknown result type (might be due to invalid IL or missing references)
				//IL_0275: Unknown result type (might be due to invalid IL or missing references)
				//IL_0290: Unknown result type (might be due to invalid IL or missing references)
				//IL_0292: Unknown result type (might be due to invalid IL or missing references)
				//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ad: Expected O, but got Unknown
				//IL_0356: Unknown result type (might be due to invalid IL or missing references)
				if (_profile.IsEmpty)
				{
					string nothingFound = "Nothing found.";
					string description = "\n  Player is registered but either has proofs explicitly hidden or none at all.";
					FontSize fontSize = (FontSize)24;
					Point labelSize = LabelUtil.GetLabelSize(fontSize, nothingFound + description, hasPrefix: true);
					((Control)new FormattedLabelBuilder().SetHeight(labelSize.Y).SetWidth(labelSize.X).CreatePart(nothingFound, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
					{
						//IL_0002: Unknown result type (might be due to invalid IL or missing references)
						//IL_0029: Unknown result type (might be due to invalid IL or missing references)
						o.SetFontSize(fontSize);
						o.SetPrefixImage(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("common/1444522")));
						o.SetTextColor(Color.get_Yellow());
					})
						.CreatePart(description, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
						{
							//IL_000a: Unknown result type (might be due to invalid IL or missing references)
							o.SetFontSize((FontSize)18);
							o.SetTextColor(Color.get_White());
						})
						.Build()).set_Parent(buildPanel);
					return;
				}
				Panel val = new Panel();
				((Control)val).set_Parent(buildPanel);
				((Control)val).set_Width(200);
				((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
				val.set_CanScroll(true);
				val.set_Title("Mode");
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
				val3.set_Text("Fractals");
				((Control)val3).set_Width(((Container)navMenu).get_ContentRegion().Width);
				val3.set_Icon(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(514379));
				((Control)val3).set_BasicTooltipText("Rewards related to Fractals including tokens and titles.");
				MenuItem fractalsEntry = val3;
				MenuItem val4 = new MenuItem();
				((Control)val4).set_Parent((Container)(object)navMenu);
				val4.set_Text("Raids");
				((Control)val4).set_Width(((Container)navMenu).get_ContentRegion().Width);
				val4.set_Icon(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(1128644));
				((Control)val4).set_BasicTooltipText("Rewards related to Raids including tokens and titles.");
				MenuItem raidsEntry = val4;
				MenuItem val5 = new MenuItem();
				((Control)val5).set_Parent((Container)(object)navMenu);
				val5.set_Text("Strikes");
				((Control)val5).set_Width(((Container)navMenu).get_ContentRegion().Width);
				val5.set_Icon(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(2200049));
				((Control)val5).set_BasicTooltipText("Rewards related to Strike Missions including tokens and titles.");
				MenuItem strikesEntry = val5;
				ViewContainer val6 = new ViewContainer();
				((Control)val6).set_Parent(buildPanel);
				((Control)val6).set_Left(((Control)menuPanel).get_Right());
				((Control)val6).set_Width(buildPanel.get_ContentRegion().Width - ((Control)menuPanel).get_Width());
				((Control)val6).set_Height(buildPanel.get_ContentRegion().Height);
				((Panel)val6).set_ShowBorder(true);
				ViewContainer plyPanel = val6;
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
					((Control)fractalsEntry).set_Width(e.get_CurrentRegion().Width);
					((Control)raidsEntry).set_Height(e.get_CurrentRegion().Height);
					((Control)strikesEntry).set_Height(e.get_CurrentRegion().Height);
				});
				fractalsEntry.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate
				{
					List<Resource> fractalResources = ProofLogix.Instance.Resources.GetItemsForFractals();
					List<Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token> tokens3 = (from token in _profile.Totals.GetTokens()
						where fractalResources.Any((Resource res) => res.Id == token.Id)
						select token).ToList();
					ProfileItems items3 = new ProfileItems(_profile.Totals.Titles.Where((Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Title title) => title.Mode == Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models.Title.TitleMode.Fractal), tokens3);
					ProofLogix.Instance.ProfileConfig.get_Value().SelectedTab = 0;
					plyPanel.Show((IView)(object)new ProfileItemsView(items3));
				});
				raidsEntry.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate
				{
					IEnumerable<Resource> raidResources = ProofLogix.Instance.Resources.GetItems(77302, 88485, 12251, 12773).Concat(ProofLogix.Instance.Resources.GetItemsForRaids());
					IEnumerable<Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token> tokens2 = from token in _profile.Totals.GetTokens()
						where raidResources.Any((Resource res) => res.Id == token.Id)
						select token;
					ProfileItems items2 = new ProfileItems(_profile.Totals.Titles.Where((Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Title title) => title.Mode == Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models.Title.TitleMode.Raid), tokens2);
					ProofLogix.Instance.ProfileConfig.get_Value().SelectedTab = 1;
					plyPanel.Show((IView)(object)new ProfileItemsView(items2));
				});
				strikesEntry.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate
				{
					IEnumerable<Resource> strikeResources = ProofLogix.Instance.Resources.GetItems(77302, 93781).Concat(ProofLogix.Instance.Resources.GetItemsForStrikes());
					IEnumerable<Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token> tokens = from token in _profile.Totals.GetTokens()
						where strikeResources.Any((Resource res) => res.Id == token.Id)
						select token;
					ProfileItems items = new ProfileItems(_profile.Totals.Titles.Where((Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Title title) => title.Mode == Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models.Title.TitleMode.Strike), tokens);
					ProofLogix.Instance.ProfileConfig.get_Value().SelectedTab = 2;
					plyPanel.Show((IView)(object)new ProfileItemsView(items, displayAsText: true));
				});
				((MenuItem)((Container)navMenu).get_Children().get_Item(ProofLogix.Instance.ProfileConfig.get_Value().SelectedTab)).Select();
			}
		}

		private sealed class ProfileItems
		{
			public IReadOnlyList<Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Title> Titles { get; init; }

			public IReadOnlyList<Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token> Tokens { get; init; }

			public ProfileItems(IEnumerable<Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Title> titles, IEnumerable<Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token> tokens)
			{
				Titles = titles.ToList();
				Tokens = tokens.ToList();
			}
		}

		private sealed class ProfileItemsView : View
		{
			private ProfileItems _items;

			private readonly Texture2D _iconTitle;

			private bool _displayAsText;

			public ProfileItemsView(ProfileItems items, bool displayAsText = false)
				: this()
			{
				_items = items;
				_iconTitle = ProofLogix.Instance.ContentsManager.GetTexture("icon_title.png");
				_displayAsText = displayAsText;
			}

			protected override void Unload()
			{
				((GraphicsResource)_iconTitle).Dispose();
				((View<IPresenter>)this).Unload();
			}

			protected override void Build(Container buildPanel)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0079: Expected O, but got Unknown
				//IL_0105: Unknown result type (might be due to invalid IL or missing references)
				//IL_010a: Unknown result type (might be due to invalid IL or missing references)
				//IL_010f: Unknown result type (might be due to invalid IL or missing references)
				//IL_011a: Unknown result type (might be due to invalid IL or missing references)
				//IL_01db: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent(buildPanel);
				((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
				((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
				val.set_FlowDirection((ControlFlowDirection)0);
				val.set_OuterControlPadding(new Vector2(5f, 5f));
				val.set_ControlPadding(new Vector2(5f, 5f));
				((Panel)val).set_CanScroll(true);
				FlowPanel panel = val;
				buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_001d: Unknown result type (might be due to invalid IL or missing references)
					((Control)panel).set_Width(e.get_CurrentRegion().Width);
					((Control)panel).set_Height(e.get_CurrentRegion().Height);
				});
				foreach (Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token token in _items.Tokens)
				{
					if (token.Amount <= 0)
					{
						continue;
					}
					if (_displayAsText)
					{
						Resource item = ProofLogix.Instance.Resources.GetItem(token.Id);
						string text2 = " " + AssetUtil.GetItemDisplayName(token.Name, token.Amount, brackets: false);
						Point size2 = LabelUtil.GetLabelSize((FontSize)20, text2, hasPrefix: true);
						((Control)new FormattedLabelBuilder().SetWidth(((Container)panel).get_ContentRegion().Width).SetHeight(size2.Y).CreatePart(text2, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
						{
							//IL_0007: Unknown result type (might be due to invalid IL or missing references)
							//IL_000c: Unknown result type (might be due to invalid IL or missing references)
							//IL_0049: Unknown result type (might be due to invalid IL or missing references)
							o.SetTextColor(item.Rarity.AsColor());
							o.SetFontSize((FontSize)20);
							o.SetPrefixImage(item.Icon);
							o.SetPrefixImageSize(new Point(size2.Y, size2.Y));
						})
							.Build()).set_Parent((Container)(object)panel);
					}
					else
					{
						((Control)ItemWithAmount.Create(token.Id, token.Amount)).set_Parent((Container)(object)panel);
					}
				}
				foreach (Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Title title in _items.Titles)
				{
					string text = " " + title.Name;
					Point size = LabelUtil.GetLabelSize((FontSize)20, text, hasPrefix: true);
					((Control)new FormattedLabelBuilder().SetWidth(((Container)panel).get_ContentRegion().Width).SetHeight(size.Y).CreatePart(text, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
					{
						//IL_003c: Unknown result type (might be due to invalid IL or missing references)
						o.SetFontSize((FontSize)20);
						o.SetPrefixImage(AsyncTexture2D.op_Implicit(_iconTitle));
						o.SetPrefixImageSize(new Point(size.Y, size.Y));
					})
						.Build()).set_Parent((Container)(object)panel);
				}
			}
		}

		private readonly Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile _profile;

		public ProfileView(Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile profile)
			: this()
		{
			_profile = profile;
		}

		public static void Open(Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile profile)
		{
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(profile.Name))
			{
				return;
			}
			foreach (string item in profile.Accounts.Select((Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile link) => link.Name))
			{
				if (TrackableWindow.TryGetById(item.ToLowerInvariant(), out var wnd))
				{
					((Control)wnd).set_Left((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Control)wnd).get_Width()) / 2);
					((Control)wnd).set_Top((((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)wnd).get_Height()) / 2);
					((WindowBase2)wnd).BringWindowToFront();
					((StandardWindow)wnd).Show((IView)(object)new LinkedView(profile));
					return;
				}
			}
			TrackableWindow trackableWindow = new TrackableWindow(profile.Name.ToLowerInvariant(), GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155985), new Rectangle(40, 26, 913, 691), new Rectangle(70, 36, 839, 605));
			((Control)trackableWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)trackableWindow).set_Title("Profile");
			((WindowBase2)trackableWindow).set_Subtitle(profile.Id);
			((WindowBase2)trackableWindow).set_Id("ProofLogix_Profile_a32c972dd9fe4025a01d3256025ab1dc");
			((WindowBase2)trackableWindow).set_CanResize(true);
			((WindowBase2)trackableWindow).set_SavesSize(true);
			((Control)trackableWindow).set_Width(800);
			((Control)trackableWindow).set_Height(600);
			((Control)trackableWindow).set_Left((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - 700) / 2);
			((Control)trackableWindow).set_Top((((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 600) / 2);
			((WindowBase2)trackableWindow).set_Emblem(AsyncTexture2D.op_Implicit(ProofLogix.Instance.Emblem));
			((StandardWindow)trackableWindow).Show((IView)(object)new LinkedView(profile));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected O, but got Unknown
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Expected O, but got Unknown
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Expected O, but got Unknown
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val).set_Height(100);
			val.set_ControlPadding(new Vector2(5f, 5f));
			val.set_OuterControlPadding(new Vector2(5f, 5f));
			val.set_FlowDirection((ControlFlowDirection)2);
			((Panel)val).set_ShowBorder(true);
			FlowPanel header = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)header);
			((Control)val2).set_Width((int)(0.4f * (float)((Container)header).get_ContentRegion().Width));
			((Control)val2).set_Height(((Container)header).get_ContentRegion().Height);
			val2.set_ControlPadding(new Vector2(5f, 5f));
			val2.set_OuterControlPadding(new Vector2(5f, 5f));
			val2.set_FlowDirection((ControlFlowDirection)3);
			FlowPanel info = val2;
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)header);
			((Control)val3).set_Width((int)(0.6f * (float)((Container)header).get_ContentRegion().Width));
			((Control)val3).set_Height(((Container)header).get_ContentRegion().Height);
			((Control)val3).set_Right(((Container)header).get_ContentRegion().Width);
			val3.set_ControlPadding(new Vector2(5f, 5f));
			val3.set_OuterControlPadding(new Vector2(5f, 5f));
			val3.set_FlowDirection((ControlFlowDirection)5);
			FlowPanel navMenu = val3;
			Point nameSize = LabelUtil.GetLabelSize((FontSize)18, _profile.Name);
			((Control)new FormattedLabelBuilder().SetWidth(nameSize.X).SetHeight(nameSize.Y).CreatePart(_profile.Name, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
			{
				o.SetFontSize((FontSize)18);
				o.SetLink((Action)delegate
				{
					GameService.Content.PlaySoundEffectByName("button-click");
					Process.Start(_profile.ProofUrl);
				});
			})
				.Build()).set_Parent((Container)(object)info);
			string lastRefreshText = _profile.LastRefresh.ToLocalTime().AsTimeAgo();
			Point size = LabelUtil.GetLabelSize((FontSize)11, lastRefreshText);
			((Control)new FormattedLabelBuilder().SetWidth(size.X).SetHeight(size.Y).CreatePart(lastRefreshText, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
			{
				o.SetFontSize((FontSize)11);
				o.MakeItalic();
			})
				.Build()).set_Parent((Container)(object)info);
			((Container)header).add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				((Control)info).set_Width((int)(0.4f * (float)((Container)header).get_ContentRegion().Width));
				((Control)info).set_Height(e.get_CurrentRegion().Height);
				((Control)navMenu).set_Width((int)(0.6f * (float)((Container)header).get_ContentRegion().Width));
				((Control)navMenu).set_Height(e.get_CurrentRegion().Height);
				((Control)navMenu).set_Right(e.get_CurrentRegion().Width);
			});
			ViewContainer val4 = new ViewContainer();
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Top(((Control)header).get_Bottom() + 7);
			((Control)val4).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val4).set_Height(buildPanel.get_ContentRegion().Height - ((Control)header).get_Height() - 7);
			((Panel)val4).set_ShowBorder(true);
			ViewContainer body = val4;
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				((Control)header).set_Width(e.get_CurrentRegion().Width);
				((Control)body).set_Width(e.get_CurrentRegion().Width);
				((Control)body).set_Height(e.get_CurrentRegion().Height - ((Control)header).get_Height() - 7);
			});
			body.Show((IView)(object)new ItemsView(_profile));
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)navMenu);
			((Control)val5).set_Width(150);
			((Control)val5).set_Height(30);
			val5.set_Text("Weekly Clears");
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GameService.Content.PlaySoundEffectByName("button-click");
				body.Show((IView)(object)new ClearsView(_profile.Clears));
			});
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)navMenu);
			((Control)val6).set_Width(150);
			((Control)val6).set_Height(30);
			val6.set_Text("Proofs");
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GameService.Content.PlaySoundEffectByName("button-click");
				body.Show((IView)(object)new ItemsView(_profile));
			});
			RefreshButton refreshButton = new RefreshButton();
			((Control)refreshButton).set_Parent((Container)(object)navMenu);
			((Control)refreshButton).set_Width(32);
			((Control)refreshButton).set_Height(32);
			refreshButton.NextRefresh = _profile.NextRefresh;
			bool isRefreshing = false;
			((Control)refreshButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (isRefreshing || _profile.NextRefresh > DateTime.UtcNow)
				{
					GameService.Content.PlaySoundEffectByName("error");
				}
				else
				{
					isRefreshing = true;
					AsyncString basicTooltipText = new AsyncString();
					AsyncString loadingText = new AsyncString();
					((ViewContainer)buildPanel).Show((IView)(object)new LoadingView("Refreshing…", loadingText, basicTooltipText));
					if (!(await ProofLogix.Instance.KpWebApi.Refresh(_profile.Id)))
					{
						GameService.Content.PlaySoundEffectByName("error");
						ProofLogix.Logger.Warn("Refresh for '" + _profile.Id + "' failed - perhaps user API key is bad or API is down.");
						ScreenNotification.ShowNotification("Refresh failed. Please, try again.", (NotificationType)2, (Texture2D)null, 4);
						Open(_profile);
					}
					else
					{
						int retries = 60;
						Timer timer = new Timer(1250.0);
						timer.Elapsed += async delegate
						{
							if (retries <= 0)
							{
								Open(await ProofLogix.Instance.KpWebApi.GetProfile(_profile.Id));
								timer.Stop();
								timer.Dispose();
							}
							else
							{
								retries--;
								string retryStr = $"({60 - retries} / 60)";
								basicTooltipText.String = "Checking completion… " + retryStr;
								loadingText.String = ProofLogix.Instance.Resources.GetLoadingSubtitle();
								if (!(await ProofLogix.Instance.KpWebApi.IsProofBusy(_profile.Id)))
								{
									await Task.Delay(1000).ContinueWith((Func<Task, Task>)async delegate
									{
										Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile profile = await ProofLogix.Instance.KpWebApi.GetProfile(_profile.Id);
										ProofLogix.Instance.PartySync.AddKpProfile(profile);
										GameService.Content.PlaySoundEffectByName("color-change");
										Open(profile);
									});
									timer.Stop();
									timer.Dispose();
								}
							}
						};
						timer.Start();
					}
				}
			});
			((View<IPresenter>)this).Build(buildPanel);
		}
	}
}
