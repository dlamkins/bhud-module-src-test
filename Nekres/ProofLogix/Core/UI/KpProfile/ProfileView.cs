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

			private readonly Texture2D _iconTitle;

			public ItemsView(Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile profile)
				: this()
			{
				_profile = profile;
				_iconTitle = ProofLogix.Instance.ContentsManager.GetTexture("icon_title.png");
			}

			protected override void Unload()
			{
				((GraphicsResource)_iconTitle).Dispose();
				((View<IPresenter>)this).Unload();
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
				//IL_0091: Unknown result type (might be due to invalid IL or missing references)
				//IL_0098: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00af: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent(buildPanel);
				((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
				((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
				val.set_FlowDirection((ControlFlowDirection)0);
				val.set_OuterControlPadding(new Vector2(4f, 7f));
				FlowPanel panel = val;
				buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_001d: Unknown result type (might be due to invalid IL or missing references)
					((Control)panel).set_Width(e.get_CurrentRegion().Width);
					((Control)panel).set_Height(e.get_CurrentRegion().Height);
				});
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
						.Build()).set_Parent((Container)(object)panel);
					((View<IPresenter>)this).Build((Container)(object)panel);
					return;
				}
				Proofs totals = _profile.Totals;
				List<Resource> fractalResources = ProofLogix.Instance.Resources.GetItemsForFractals();
				List<Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token> source = totals.GetTokens().ToList();
				IEnumerable<Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token> fractalTokens = source.Where((Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token token) => fractalResources.Any((Resource res) => res.Id == token.Id));
				IEnumerable<Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token> raidTokens = source.Where((Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token token) => fractalResources.All((Resource res) => res.Id != token.Id));
				ProfileItems fractalResults = new ProfileItems(totals.Titles.Where((Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Title title) => title.Mode == Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models.Title.TitleMode.Fractal), fractalTokens);
				ProfileItems raidResults = new ProfileItems(totals.Titles.Where((Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Title title) => title.Mode == Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models.Title.TitleMode.Raid), raidTokens);
				CreateItemPanel((Container)(object)panel, fractalResults, "Fractals");
				CreateItemPanel((Container)(object)panel, raidResults, "Raids");
				((View<IPresenter>)this).Build((Container)(object)panel);
			}

			private void CreateItemPanel(Container parent, ProfileItems items, string panelTitle)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				//IL_007a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Expected O, but got Unknown
				//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
				//IL_0196: Unknown result type (might be due to invalid IL or missing references)
				//IL_019b: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent(parent);
				((Control)val).set_Width(parent.get_ContentRegion().Width / 2 - 4);
				((Control)val).set_Height(parent.get_ContentRegion().Height - 4);
				val.set_ControlPadding(new Vector2(5f, 4f));
				val.set_OuterControlPadding(new Vector2(5f, 4f));
				val.set_FlowDirection((ControlFlowDirection)3);
				((Panel)val).set_CanScroll(true);
				((Panel)val).set_Title(panelTitle);
				FlowPanel flow = val;
				parent.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_001f: Unknown result type (might be due to invalid IL or missing references)
					((Control)flow).set_Height(e.get_CurrentRegion().Height - 4);
					((Control)flow).set_Width(e.get_CurrentRegion().Width / 2 - 4);
				});
				foreach (Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Title title in items.Titles)
				{
					Point size = LabelUtil.GetLabelSize((FontSize)20, title.Name, hasPrefix: true);
					((Control)new FormattedLabelBuilder().SetWidth(size.X).SetHeight(size.Y).CreatePart(title.Name, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
					{
						//IL_003c: Unknown result type (might be due to invalid IL or missing references)
						o.SetFontSize((FontSize)20);
						o.SetPrefixImage(AsyncTexture2D.op_Implicit(_iconTitle));
						o.SetPrefixImageSize(new Point(size.Y, size.Y));
					})
						.Build()).set_Parent((Container)(object)flow);
				}
				foreach (Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Token token in items.Tokens)
				{
					if (token.Amount != 0)
					{
						string text = AssetUtil.GetItemDisplayName(token.Name, token.Amount);
						Point size2 = LabelUtil.GetLabelSize((FontSize)20, text, hasPrefix: true);
						AsyncTexture2D icon = ProofLogix.Instance.Resources.GetItem(token.Id).Icon;
						((Control)new FormattedLabelBuilder().SetWidth(size2.X).SetHeight(size2.Y + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y).CreatePart(text, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
						{
							//IL_001b: Unknown result type (might be due to invalid IL or missing references)
							//IL_0020: Unknown result type (might be due to invalid IL or missing references)
							//IL_0058: Unknown result type (might be due to invalid IL or missing references)
							o.SetTextColor(ProofLogix.Instance.Resources.GetItem(token.Id).Rarity.AsColor());
							o.SetFontSize((FontSize)20);
							o.SetPrefixImage(icon);
							o.SetPrefixImageSize(new Point(size2.Y, size2.Y));
						})
							.Build()).set_Parent((Container)(object)flow);
					}
				}
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
					wnd.Show((IView)(object)new LinkedView(profile));
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
					((ViewContainer)buildPanel).Show((IView)(object)new LoadingView("Refreshing...", loadingText, basicTooltipText));
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
								basicTooltipText.String = "Checking completion.. " + retryStr;
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
