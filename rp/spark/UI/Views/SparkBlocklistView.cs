using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using rp.spark.Services;

namespace rp.spark.UI.Views
{
	internal sealed class SparkBlocklistView : View
	{
		private const int ContentWidth = 760;

		private const int ContentHeight = 610;

		private const int ListWidth = 390;

		private const int InfoWidth = 330;

		private const int ListHeight = 490;

		private readonly SparkBlocklist _blocklist;

		public SparkBlocklistView(SparkSettings settings, Func<string, string> blockAccount, Func<string, string> unblockAccount, Action<Action> watchBlockedAccountsChanged, Action<Action> unwatchBlockedAccountsChanged)
			: this()
		{
			_blocklist = new SparkBlocklist(settings, blockAccount, unblockAccount, watchBlockedAccountsChanged, unwatchBlockedAccountsChanged);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(760);
			((Control)val).set_Height(610);
			val.set_FlowDirection((ControlFlowDirection)2);
			val.set_ControlPadding(new Vector2(20f, 0f));
			FlowPanel row = val;
			FlowPanel blockStack = SparkFormLayout.AddVerticalStack((Container)(object)row, 0, 0, 390, 610, 8);
			_blocklist.Build(blockStack, 390, 490);
			BuildExplanation((Container)(object)row);
		}

		private void BuildExplanation(Container parent)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel parent2 = SparkFormLayout.AddVerticalStack(parent, 0, 0, 330, 610);
			SparkFormLayout.AddLabel((Container)(object)parent2, "How blocking works:", 330, 28, GameService.Content.get_DefaultFont16(), Color.get_White(), strokeText: true);
			AddInfoParagraph((Container)(object)parent2, "Blocks first happen locally before they are sent to SPARK when you are online. This lets the server prevent blocked accounts from seeing your profiles, online status, and location. You also will not see them in SPARK.");
			AddInfoParagraph((Container)(object)parent2, "Someone you blocked might have a copy of your profile in bookmarks or in recently viewed. Once blocked, they will not receive a new copy of your profile anymore. SPARK does this on purpose so someone cannot tell that they have been blocked.");
			AddInfoParagraph((Container)(object)parent2, "If someone's profile breaks any of SPARK's content policies, please report them for moderation. Users who misuse SPARK will be banned from accessing the service. You can find SPARK's content policies in the About' window on the main page.");
		}

		private static void AddInfoParagraph(Container parent, string text)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			SparkFormLayout.AddLabel(parent, text, 330, 95, GameService.Content.get_DefaultFont14(), SparkViewUI.SecondaryTextColor).set_WrapText(true);
		}

		protected override void Unload()
		{
			_blocklist.Dispose();
		}
	}
}
