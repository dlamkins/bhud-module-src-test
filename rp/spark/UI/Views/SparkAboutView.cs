using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace rp.spark.UI.Views
{
	internal class SparkAboutView : View
	{
		private const int ContentPadding = 12;

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			((Panel)val).set_CanScroll(true);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 10f));
			val.set_OuterControlPadding(new Vector2(12f, 12f));
			FlowPanel contentStack = val;
			AddSection(contentStack, "What is SPARK?", true, "SPARK is built for roleplay and discovering potential RP partners. It lets you create character profiles, share profiles, and view profiles from other SPARK users. The name stands for 'Simple Profile and Roleplay Kit', and comes from the precursor of my first legendary, Incinerator.");
			AddSection(contentStack, "SPARK Content Guidelines / Policies", false, "SPARK offers a report function and a block function to help curate your experience when using the tool.", "By using SPARK, you agree to the following:", " ", "1. No hate speech or bigotry", "Profiles may not include racist, sexist, homophobic, transphobic, ableist, or otherwise discriminatory content.", " ", "2. No harassment or targeted abuse", "Do not make profiles that insults, shames, threatens, stalks, or attempts to organize/coordinate harassment against another player.", " ", "3. NSFW Profiles must be marked as Mature/18+", "Profiles should be set to Mature/18+ if it contains explicit content not suitable for minors. Please report any profiles that are not properly set for moderation/review.", "SPARK may permanently mark all of the character profiles on your account as 18+ for breaking this rule.", " ", "4. No abusive sexual content", "Sexual content involving minors (real or fictional underage characters) is strictly prohibited.", " ", "SPARK may permanently block access to this service to any GW2 account that breaks these rules.");
			AddSection(contentStack, "Profile sharing", false, "When 'Share my profile' is enabled, SPARK publishes your active profile and information to the SPARK service. If your online status is invisible, this will no longer publish.");
			AddSection(contentStack, "Privacy", false, "The 'Invisible' status removes your info from the online user list. Hide my location sets your location to 'Hidden' for privacy.", "If you block someone, all profiles on that GW2 account get filtered. Blocked users can only see the last profile they ever saw from you, if any. They won't ever receive your profile updates or information.");
			AddSection(contentStack, "Saved Data", false, "Your profiles and any viewed profiles get saved locally to your PC. When your profile syncs to the server, it only transmits the information in the profile, plus your RP status, account name, character name, and location (if not hidden).", "Your profile data is removed from the SPARK server after 24 hours of being offline. The webserver does not retain user data, except reported profiles for moderation purposes. Your information only used to transmit profiles to other players.", "SPARK does NOT use analytics, tracking, and will never use your data for machine learning or AI.");
			AddSection(contentStack, "Questions / Feedback", false, "Any feedback can be sent to Bat.8570 in-game, or emailed to taw@a-bat.com.");
		}

		private static void AddParagraph(FlowPanel parent, string text, Color? textColor = null, BitmapFont font = null)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text(text ?? string.Empty);
			((Control)val).set_Width(GetTextWidth(parent));
			val.set_Font(font ?? GameService.Content.get_DefaultFont14());
			val.set_TextColor((Color)(((_003F?)textColor) ?? SparkViewUI.SecondaryTextColor));
			val.set_WrapText(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Parent((Container)(object)parent);
		}

		private static bool IsRuleHeading(string text)
		{
			if (!string.IsNullOrWhiteSpace(text) && text.Length >= 3 && char.IsDigit(text[0]) && text[1] == '.')
			{
				return text[2] == ' ';
			}
			return false;
		}

		private static int GetTextWidth(FlowPanel parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Container)parent).get_ContentRegion().Width - 24;
		}

		private static void AddSection(FlowPanel parent, string title, bool expandedByDefault, params string[] paragraphs)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Width(GetTextWidth(parent));
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 10));
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 8f));
			val.set_OuterControlPadding(new Vector2(8f, 10f));
			((Panel)val).set_ShowBorder(true);
			((Panel)val).set_CanCollapse(true);
			((Panel)val).set_Title(title ?? string.Empty);
			FlowPanel section = val;
			foreach (string paragraph in paragraphs)
			{
				bool isRuleHeading = IsRuleHeading(paragraph);
				AddParagraph(section, paragraph, (Color)(isRuleHeading ? new Color(255, 190, 0) : SparkViewUI.SecondaryTextColor), isRuleHeading ? GameService.Content.get_DefaultFont16() : GameService.Content.get_DefaultFont14());
			}
			if (expandedByDefault)
			{
				((Panel)section).Expand();
			}
			else
			{
				((Panel)section).Collapse();
			}
		}

		public SparkAboutView()
			: this()
		{
		}
	}
}
