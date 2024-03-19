using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Flurl.Http;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using Nekres.ChatMacros.Properties;
using Newtonsoft.Json;

namespace Nekres.ChatMacros.Core.UI.Credits
{
	internal class CreditsView : View
	{
		private class DonorEntryView : View
		{
			private Donor _donor;

			public DonorEntryView(Donor donor)
				: this()
			{
				_donor = donor;
			}

			protected override void Build(Container buildPanel)
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Expected O, but got Unknown
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_0095: Unknown result type (might be due to invalid IL or missing references)
				//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				//IL_0110: Unknown result type (might be due to invalid IL or missing references)
				//IL_0117: Unknown result type (might be due to invalid IL or missing references)
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent(buildPanel);
				((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
				((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
				val.set_FlowDirection((ControlFlowDirection)0);
				val.set_OuterControlPadding(new Vector2(5f, 5f));
				val.set_ControlPadding(new Vector2(5f, 5f));
				FlowPanel flow = val;
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)flow);
				((Control)val2).set_Height(((Container)flow).get_ContentRegion().Height);
				val2.set_AutoSizeWidth(true);
				val2.set_Text(_donor.Name);
				val2.set_Font((BitmapFont)(object)ChatMacros.Instance.Resources.LatoRegular24);
				((Control)val2).set_BasicTooltipText(string.Format(Resources.Supporter_since__0_, _donor.SupporterSince.ToShortDateString()));
				if (_donor.Socials != null)
				{
					if (!string.IsNullOrEmpty(_donor.Socials.GuildWars2))
					{
						Label val3 = new Label();
						((Control)val3).set_Parent((Container)(object)flow);
						((Control)val3).set_Height(((Container)flow).get_ContentRegion().Height);
						val3.set_AutoSizeWidth(true);
						val3.set_Text("(" + _donor.Socials.GuildWars2 + ")");
					}
					AddSocialButton((Container)(object)flow, _donor.Socials.Homepage, _donor.Socials.Homepage, _donor.Socials.Homepage, GameService.Content.get_DatAssetCache().GetTextureFromAssetId(255369));
					AddSocialButton((Container)(object)flow, _donor.Socials.Twitch, "twitch.tv/" + _donor.Socials.Twitch, "https://www.twitch.tv/" + _donor.Socials.Twitch, AsyncTexture2D.op_Implicit(ChatMacros.Instance.Resources.TwitchLogo));
					AddSocialButton((Container)(object)flow, _donor.Socials.Youtube, "youtube.com/@" + _donor.Socials.Youtube, "https://www.youtube.com/@" + _donor.Socials.Youtube, AsyncTexture2D.op_Implicit(ChatMacros.Instance.Resources.YoutubeLogo));
					((View<IPresenter>)this).Build(buildPanel);
				}
			}

			private void AddSocialButton(Container parent, string raw, string name, string url, AsyncTexture2D buttonTexture)
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				if (!string.IsNullOrEmpty(raw))
				{
					Image val = new Image();
					((Control)val).set_Parent(parent);
					((Control)val).set_Height(32);
					((Control)val).set_Width(32);
					val.set_Texture(buttonTexture);
					((Control)val).set_BasicTooltipText(name);
					((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						Process.Start(url);
					});
				}
			}
		}

		public class Donor
		{
			public class SocialUrls
			{
				[JsonProperty("guildwars2")]
				public string GuildWars2 { get; set; }

				[JsonProperty("homepage")]
				public string Homepage { get; set; }

				[JsonProperty("twitch")]
				public string Twitch { get; set; }

				[JsonProperty("youtube")]
				public string Youtube { get; set; }
			}

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("socials")]
			public SocialUrls Socials { get; set; }

			[JsonProperty("supporter_since")]
			public DateTime SupporterSince { get; set; }
		}

		private const string DONORS_URI = "https://pastebin.com/raw/1Wd03Bmg";

		private IReadOnlyList<Donor> _donors;

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			_donors = await HttpUtil.RetryAsync(() => GeneratedExtensions.GetJsonAsync<List<Donor>>("https://pastebin.com/raw/1Wd03Bmg", default(CancellationToken), (HttpCompletionOption)0));
			return await Task.FromResult(await ((View<IPresenter>)this).Load(progress));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Expected O, but got Unknown
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
			((Panel)val).set_Title(Resources.Credits_and_Thanks);
			FlowPanel creditsWrap = val;
			FormattedLabel thanks = new FormattedLabelBuilder().SetHeight(100).SetWidth(((Container)creditsWrap).get_ContentRegion().Width - 8).Wrap()
				.SetHorizontalAlignment((HorizontalAlignment)1)
				.SetVerticalAlignment((VerticalAlignment)1)
				.CreatePart(Resources.Thanks_to_my_awesome_supporters_who_keep_me_motivated_, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
				{
					o.SetFontSize((FontSize)18);
					o.MakeBold();
				})
				.Build();
			((Control)thanks).set_Parent((Container)(object)creditsWrap);
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)creditsWrap);
			((Control)val2).set_Width(((Container)creditsWrap).get_ContentRegion().Width);
			((Control)val2).set_Height(((Container)creditsWrap).get_ContentRegion().Height - 50 - ((Control)thanks).get_Height());
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_OuterControlPadding(new Vector2(5f, 5f));
			val2.set_ControlPadding(new Vector2(5f, 5f));
			((Panel)val2).set_CanScroll(true);
			((Panel)val2).set_ShowBorder(true);
			FlowPanel donorsList = val2;
			if (_donors != null)
			{
				foreach (Donor donor in _donors)
				{
					AddDonor((Container)(object)donorsList, donor);
				}
			}
			ViewContainer val3 = new ViewContainer();
			((Control)val3).set_Parent((Container)(object)creditsWrap);
			((Control)val3).set_Width(((Control)donorsList).get_Width());
			((Control)val3).set_Height(50);
			val3.Show((IView)(object)new KofiButton());
			((View<IPresenter>)this).Build(buildPanel);
		}

		private void AddDonor(Container parent, Donor donor)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent(parent);
			((Control)val).set_Width(parent.get_ContentRegion().Width);
			((Control)val).set_Height(50);
			val.Show((IView)(object)new DonorEntryView(donor));
		}

		public CreditsView()
			: this()
		{
		}
	}
}
