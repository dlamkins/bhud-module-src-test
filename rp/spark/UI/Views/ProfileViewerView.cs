using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using rp.spark.Models;
using rp.spark.UI.Controls;

namespace rp.spark.UI.Views
{
	public class ProfileViewerView : View
	{
		private sealed class Layout
		{
			private const int ScrollbarWidth = 12;

			private const int ScrollbarGap = 0;

			private const int ViewportMinWidth = 260;

			private const int ViewportRightPadding = 8;

			private const int ViewportY = 144;

			private const int ViewportBottomPadding = 8;

			private const int IconY = 94;

			private const int IconSize = 50;

			private const int IconGap = 8;

			private const int StatusWidth = 220;

			private const int TextXValue = 1;

			private const int TextRightPadding = 24;

			public const int HeaderCharacterLimit = 28;

			public const int WrappedHeaderOffset = 24;

			public const int ProfileTraitsOffset = 28;

			public const int StatusGap = 5;

			private readonly int _headerOffset;

			private readonly Point _contentSize;

			public int TextX => 1;

			public int TextStartY => 8;

			public int TextWidth => ViewportWidth - 2 - 24;

			public int TextLabelWidth => ViewportWidth - 1 - 18;

			public int SectionGap => 16;

			public int TextLineHeight => 26;

			public int ParagraphGap => 10;

			private int ViewportHeight => Math.Max(160, _contentSize.Y - 144 - 8);

			private int ViewportWidth => Math.Max(260, _contentSize.X - 12 - 8);

			public Rectangle ViewportBounds => new Rectangle(0, 144 + _headerOffset, ViewportWidth, ViewportHeight - _headerOffset);

			public Rectangle ScrollbarBounds => new Rectangle(ViewportWidth, 144 + _headerOffset, 12, ViewportHeight - _headerOffset);

			public Rectangle ProfileStatusBounds => new Rectangle(Math.Max(0, ViewportWidth - 220), 104 + _headerOffset, Math.Min(220, ViewportWidth), 28);

			public Layout(int headerOffset, Point contentSize)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				_headerOffset = headerOffset;
				_contentSize = contentSize;
			}

			public Rectangle GlanceIconBounds(int index)
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				return new Rectangle(index * 58, 94 + _headerOffset, 50, 50);
			}

			public static int SecondaryHeaderY(bool wrappedHeader)
			{
				if (!wrappedHeader)
				{
					return 14;
				}
				return 42;
			}

			public static int CharacterDetailsY(bool wrappedHeader)
			{
				if (!wrappedHeader)
				{
					return 42;
				}
				return 66;
			}

			public static int MetadataY(bool wrappedHeader)
			{
				if (!wrappedHeader)
				{
					return 70;
				}
				return 90;
			}

			public static int ProfileTraitsY(bool wrappedHeader)
			{
				if (!wrappedHeader)
				{
					return 94;
				}
				return 118;
			}
		}

		private class MouseWheelPanel : Panel
		{
			protected override CaptureType CapturesInput()
			{
				return (CaptureType)12;
			}

			public MouseWheelPanel()
				: this()
			{
			}
		}

		private const string OfficialDeveloperAccount = "Bat.8570";

		private const int DeveloperBadgeAssetId = 3307061;

		private const int DeveloperBadgeSlot = 5;

		private readonly Func<CharacterProfile, PlayerPresence, string> _toggleBookmark;

		private readonly Func<CharacterProfile, PlayerPresence, bool> _isBookmarked;

		private readonly Func<CharacterProfile, PlayerPresence, string> _toggleBlock;

		private readonly Func<CharacterProfile, PlayerPresence, bool> _isBlocked;

		private readonly Func<CharacterProfile, PlayerPresence, string, Task<string>> _reportProfile;

		private CharacterProfile _profile;

		private PlayerPresence _presence;

		private Container _buildPanel;

		private Panel _contentPanel;

		private Panel _scrollViewport;

		private Panel _reportPanel;

		private Layout _layout;

		private Label _status;

		private int _headerOffset;

		private bool _isSubmittingReport;

		private static readonly Logger Logger = Logger.GetLogger<ProfileViewerView>();

		private const string ReportUnavailableMessage = "Reported Profile not found on SPARK server. Please try again later.";

		public ProfileViewerView(CharacterProfile profile, PlayerPresence presence, Func<CharacterProfile, PlayerPresence, string> toggleBookmark = null, Func<CharacterProfile, PlayerPresence, bool> isBookmarked = null, Func<CharacterProfile, PlayerPresence, string> toggleBlock = null, Func<CharacterProfile, PlayerPresence, bool> isBlocked = null, Func<CharacterProfile, PlayerPresence, string, Task<string>> reportProfile = null)
			: this()
		{
			_toggleBookmark = toggleBookmark;
			_isBookmarked = isBookmarked;
			_toggleBlock = toggleBlock;
			_isBlocked = isBlocked;
			_reportProfile = reportProfile;
			RememberProfile(profile, presence);
		}

		protected override void Build(Container buildPanel)
		{
			_buildPanel = buildPanel;
			CreateContentRoot();
			RefreshProfile();
		}

		public void SetProfile(CharacterProfile profile, PlayerPresence presence)
		{
			RememberProfile(profile, presence);
			if (_contentPanel != null)
			{
				RefreshProfile();
			}
		}

		private void RememberProfile(CharacterProfile profile, PlayerPresence presence)
		{
			_profile = profile ?? new CharacterProfile();
			_presence = presence ?? new PlayerPresence();
		}

		private void CreateContentRoot()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			ClearChildren(_buildPanel);
			Panel val = new Panel();
			val.set_ShowBorder(false);
			((Control)val).set_Location(Point.get_Zero());
			Rectangle contentRegion = _buildPanel.get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			((Control)val).set_Parent(_buildPanel);
			_contentPanel = val;
		}

		private void RefreshProfile()
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			Panel contentPanel = _contentPanel;
			if (contentPanel == null)
			{
				return;
			}
			ClearChildren((Container)(object)contentPanel);
			if (_contentPanel == contentPanel)
			{
				_scrollViewport = null;
				_layout = null;
				_status = null;
				_headerOffset = BuildHeader((Container)(object)contentPanel);
				if (_contentPanel == contentPanel)
				{
					_layout = new Layout(_headerOffset, ((Control)contentPanel).get_Size());
					BuildGlance((Container)(object)contentPanel);
					BuildStatus((Container)(object)contentPanel);
					BuildBody((Container)(object)contentPanel);
					BuildScrollBar((Container)(object)contentPanel);
				}
			}
		}

		private static void ClearChildren(Container container)
		{
			if (container != null)
			{
				Control[] array = container.get_Children().ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Dispose();
				}
			}
		}

		private int BuildHeader(Container buildPanel)
		{
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_035f: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Expected O, but got Unknown
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Expected O, but got Unknown
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Unknown result type (might be due to invalid IL or missing references)
			//IL_043a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Expected O, but got Unknown
			//IL_045d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0462: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_047e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0483: Unknown result type (might be due to invalid IL or missing references)
			//IL_048d: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0504: Unknown result type (might be due to invalid IL or missing references)
			//IL_050b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Expected O, but got Unknown
			string displayName = ProfileText.DisplayName(_profile);
			string pronouns = GetPronounsText();
			string officialName = _profile.CharacterName?.Trim() ?? string.Empty;
			bool showOfficialName = !string.IsNullOrWhiteSpace(officialName) && !string.Equals(displayName, officialName, StringComparison.OrdinalIgnoreCase);
			bool num = ShouldWrapHeader(displayName, pronouns, showOfficialName ? officialName : string.Empty);
			int secondaryHeaderY = Layout.SecondaryHeaderY(num);
			int characterDetailsY = Layout.CharacterDetailsY(num);
			int metadataY = Layout.MetadataY(num);
			int profileTraitsY = Layout.ProfileTraitsY(num);
			bool showProfileTraits = HasProfileTraits();
			int nameWidth = Math.Min(500, (int)Math.Ceiling(GameService.Content.get_DefaultFont32().MeasureString(displayName).Width) + 4);
			Label val = new Label();
			val.set_Text(displayName);
			val.set_Font(GameService.Content.get_DefaultFont32());
			val.set_TextColor(Color.get_White());
			val.set_StrokeText(true);
			val.set_WrapText(false);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(nameWidth, 45));
			((Control)val).set_Parent(buildPanel);
			int nextHeaderX = ((!num) ? (nameWidth + 8) : 0);
			if (!string.IsNullOrWhiteSpace(pronouns))
			{
				string pronounsText = "(" + pronouns + ")";
				int pronounsWidth = GetSecondaryHeaderWidth(pronounsText);
				Label val2 = new Label();
				val2.set_Text(pronounsText);
				val2.set_Font(GameService.Content.get_DefaultFont16());
				val2.set_TextColor(new Color(220, 220, 220));
				val2.set_WrapText(false);
				((Control)val2).set_Location(new Point(nextHeaderX, secondaryHeaderY));
				((Control)val2).set_Size(new Point(pronounsWidth, 25));
				((Control)val2).set_Parent(buildPanel);
				nextHeaderX += pronounsWidth + 8;
			}
			if (showOfficialName)
			{
				string officialNameText = "(" + officialName + ")";
				Label val3 = new Label();
				val3.set_Text(officialNameText);
				val3.set_Font(GameService.Content.get_DefaultFont16());
				val3.set_TextColor(new Color(220, 220, 220));
				val3.set_WrapText(false);
				((Control)val3).set_Location(new Point(nextHeaderX, secondaryHeaderY));
				((Control)val3).set_Size(new Point(GetSecondaryHeaderWidth(officialNameText), 25));
				((Control)val3).set_Parent(buildPanel);
			}
			string characterDetails = ProfileText.ProfileCharacterDetails(_profile);
			if (!string.IsNullOrWhiteSpace(characterDetails))
			{
				Label val4 = new Label();
				val4.set_Text(characterDetails);
				val4.set_Font(GameService.Content.get_DefaultFont16());
				val4.set_TextColor(new Color(220, 220, 220));
				val4.set_WrapText(false);
				((Control)val4).set_Location(new Point(0, characterDetailsY));
				((Control)val4).set_Size(new Point(480, 25));
				((Control)val4).set_Parent(buildPanel);
			}
			string metadata = "Current location: " + ProfileText.PresenceLocation(_presence) + " | Account: " + ProfileText.AccountName(_profile, _presence, string.Empty);
			if (!string.IsNullOrWhiteSpace(metadata))
			{
				Label val5 = new Label();
				val5.set_Text(metadata);
				val5.set_Font(GameService.Content.get_DefaultFont16());
				val5.set_TextColor(new Color(220, 220, 220));
				val5.set_WrapText(false);
				((Control)val5).set_Location(new Point(0, metadataY));
				((Control)val5).set_Size(new Point(480, 25));
				((Control)val5).set_Parent(buildPanel);
			}
			if (showProfileTraits)
			{
				BuildProfileTraits(buildPanel, profileTraitsY);
			}
			StandardButton val6 = new StandardButton();
			val6.set_Text("Copy Name");
			((Control)val6).set_Location(new Point(405, 8));
			((Control)val6).set_Size(new Point(90, 30));
			((Control)val6).set_Parent(buildPanel);
			SparkUiActions.BindClick(val6, CopyAccountNameAsync, SetStatusText, "Couldn't copy the account name right now.");
			StandardButton val7 = new StandardButton();
			val7.set_Text(GetBookmarkButtonText());
			((Control)val7).set_Location(new Point(503, 8));
			((Control)val7).set_Size(new Point(125, 30));
			((Control)val7).set_Parent(buildPanel);
			StandardButton bookmarkButton = val7;
			((Control)bookmarkButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_status.set_Text((_toggleBookmark == null) ? "Bookmark cache unavailable." : _toggleBookmark(_profile, _presence));
				bookmarkButton.set_Text(GetBookmarkButtonText());
			});
			StandardButton val8 = new StandardButton();
			val8.set_Text(GetBlockButtonText());
			((Control)val8).set_Location(new Point(636, 8));
			((Control)val8).set_Size(new Point(64, 30));
			((Control)val8).set_Parent(buildPanel);
			StandardButton blockButton = val8;
			((Control)blockButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_status.set_Text((_toggleBlock == null) ? "Block list unavailable." : _toggleBlock(_profile, _presence));
				blockButton.set_Text(GetBlockButtonText());
			});
			StandardButton val9 = new StandardButton();
			val9.set_Text("Report");
			((Control)val9).set_Location(new Point(708, 8));
			((Control)val9).set_Size(new Point(70, 30));
			((Control)val9).set_Parent(buildPanel);
			((Control)val9).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!CanReportViewedProfile())
				{
					SetStatusText("Reported Profile not found on SPARK server. Please try again later.");
				}
				else
				{
					OpenReportPanel();
				}
			});
			Label val10 = new Label();
			val10.set_Text(string.Empty);
			val10.set_Font(GameService.Content.get_DefaultFont12());
			val10.set_TextColor(new Color(220, 220, 220));
			((Control)val10).set_Location(new Point(500, 42));
			((Control)val10).set_Size(new Point(278, 48));
			val10.set_WrapText(true);
			((Control)val10).set_Parent(buildPanel);
			_status = val10;
			return (num ? 24 : 0) + (showProfileTraits ? 28 : 0);
		}

		private bool HasProfileTraits()
		{
			if (_profile.Experience == ProfileExperience.Hidden && _profile.Preferences == ProfilePreferenceFlags.None && _profile.Themes == ProfileThemeFlags.None && _profile.Styles == ProfileStyleFlags.None)
			{
				return IsMatureProfile();
			}
			return true;
		}

		private bool IsMatureProfile()
		{
			if (!(_profile?.IsMature ?? false))
			{
				return _presence?.IsMature ?? false;
			}
			return true;
		}

		private bool IsOfficialDeveloper()
		{
			return string.Equals(_presence?.AccountName?.Trim(), "Bat.8570", StringComparison.OrdinalIgnoreCase);
		}

		private void BuildGlance(Container buildPanel)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			Layout layout = _layout;
			if (buildPanel != null && layout != null)
			{
				List<AtAGlanceEntry> entries = GetGlanceEntries().ToList();
				for (int i = 0; i < entries.Count; i++)
				{
					AtAGlanceEntry entry = entries[i];
					Rectangle bounds = layout.GlanceIconBounds(i);
					AssetIcon assetIcon = new AssetIcon();
					((Control)assetIcon).set_Location(((Rectangle)(ref bounds)).get_Location());
					((Control)assetIcon).set_Size(((Rectangle)(ref bounds)).get_Size());
					((Control)assetIcon).set_Parent(buildPanel);
					((Control)assetIcon).set_BackgroundColor(new Color(20, 20, 20, 180));
					((Control)assetIcon).set_Tooltip(MakeGlanceTooltip(entry));
					assetIcon.SetAssetId(entry.AssetId);
				}
				if (IsOfficialDeveloper())
				{
					AddDeveloperBadge(buildPanel, layout);
				}
			}
		}

		private static void AddDeveloperBadge(Container parent, Layout layout)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			Rectangle bounds = layout.GlanceIconBounds(5);
			Tooltip tooltip = new Tooltip((ITooltipView)(object)new ProfileTooltipView("Official SPARK Developer", "This account belongs to an official developer of SPARK.", "At A Glance"));
			Panel val = new Panel();
			val.set_ShowBorder(false);
			((Control)val).set_Location(((Rectangle)(ref bounds)).get_Location());
			((Control)val).set_Size(((Rectangle)(ref bounds)).get_Size());
			((Control)val).set_Parent(parent);
			((Control)val).set_BackgroundColor(new Color(255, 194, 55));
			((Control)val).set_Tooltip(tooltip);
			Panel border = val;
			AssetIcon assetIcon = new AssetIcon();
			((Control)assetIcon).set_Location(new Point(2, 2));
			((Control)assetIcon).set_Size(new Point(bounds.Width - 4, bounds.Height - 4));
			((Control)assetIcon).set_Parent((Container)(object)border);
			((Control)assetIcon).set_BackgroundColor(new Color(20, 20, 20, 180));
			((Control)assetIcon).set_Tooltip(tooltip);
			assetIcon.SetAssetId(3307061);
		}

		private void BuildStatus(Container buildPanel)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			Layout layout = _layout;
			if (buildPanel != null && layout != null && _presence.Status != RPStatus.Invisible)
			{
				Rectangle bounds = layout.ProfileStatusBounds;
				string statusText = ProfileLabels.StatusLabel(_presence.Status);
				int statusWidth = GetStatusTextWidth(statusText);
				int prefixWidth = GetStatusTextWidth("Status:");
				int statusX = ((Rectangle)(ref bounds)).get_Right() - statusWidth;
				Label val = new Label();
				val.set_Text("Status:");
				val.set_Font(GameService.Content.get_DefaultFont18());
				val.set_TextColor(new Color(255, 233, 180));
				val.set_StrokeText(true);
				val.set_WrapText(false);
				val.set_HorizontalAlignment((HorizontalAlignment)2);
				((Control)val).set_Location(new Point(statusX - prefixWidth - 5, bounds.Y));
				((Control)val).set_Size(new Point(prefixWidth, bounds.Height));
				((Control)val).set_Parent(buildPanel);
				Label val2 = new Label();
				val2.set_Text(statusText);
				val2.set_Font(GameService.Content.get_DefaultFont18());
				val2.set_TextColor(ProfileStatusColors.Get(_presence.Status));
				val2.set_StrokeText(true);
				val2.set_WrapText(false);
				val2.set_HorizontalAlignment((HorizontalAlignment)2);
				((Control)val2).set_Location(new Point(statusX, bounds.Y));
				((Control)val2).set_Size(new Point(statusWidth, bounds.Height));
				((Control)val2).set_Parent(buildPanel);
			}
		}

		private static int GetStatusTextWidth(string text)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			return (int)Math.Ceiling(GameService.Content.get_DefaultFont18().MeasureString(text ?? string.Empty).Width) + 2;
		}

		private void BuildBody(Container buildPanel)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			Layout layout = _layout;
			if (buildPanel != null && layout != null)
			{
				Rectangle viewportBounds = layout.ViewportBounds;
				MouseWheelPanel mouseWheelPanel = new MouseWheelPanel();
				((Panel)mouseWheelPanel).set_ShowBorder(false);
				((Control)mouseWheelPanel).set_Location(((Rectangle)(ref viewportBounds)).get_Location());
				((Control)mouseWheelPanel).set_Size(((Rectangle)(ref viewportBounds)).get_Size());
				((Control)mouseWheelPanel).set_Parent(buildPanel);
				((Control)mouseWheelPanel).set_ClipsBounds(true);
				((Control)mouseWheelPanel).set_BackgroundColor(new Color(0, 0, 0, 60));
				MouseWheelPanel scrollViewport = (MouseWheelPanel)(object)(_scrollViewport = (Panel)(object)mouseWheelPanel);
				int y = layout.TextStartY;
				string currently = GetCurrentlyText();
				if (!string.IsNullOrWhiteSpace(currently))
				{
					y = AddSection((Container)(object)scrollViewport, layout, "Currently:", currently, y);
					y += layout.SectionGap;
				}
				y = AddSection((Container)(object)scrollViewport, layout, "Known for:", GetKnownForText(), y);
				y += layout.SectionGap;
				y = AddSection((Container)(object)scrollViewport, layout, "Description:", GetDescriptionText(), y);
				string outOfCharacterInfo = GetOtherInfoText();
				if (!string.IsNullOrWhiteSpace(outOfCharacterInfo))
				{
					y += layout.SectionGap;
					AddSection((Container)(object)scrollViewport, layout, "Other information:", outOfCharacterInfo, y);
				}
				((Container)scrollViewport).set_VerticalScrollOffset(0);
			}
		}

		private void BuildScrollBar(Container buildPanel)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			Layout layout = _layout;
			Panel scrollViewport = _scrollViewport;
			if (buildPanel != null && layout != null && scrollViewport != null)
			{
				Rectangle bounds = layout.ScrollbarBounds;
				Scrollbar val = new Scrollbar((Container)(object)scrollViewport);
				((Control)val).set_Location(((Rectangle)(ref bounds)).get_Location());
				((Control)val).set_Size(((Rectangle)(ref bounds)).get_Size());
				((Control)val).set_Parent(buildPanel);
			}
		}

		private int AddSection(Container parent, Layout layout, string title, string text, int y)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			if (parent == null || layout == null)
			{
				return y;
			}
			Label val = new Label();
			val.set_Text(title);
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_TextColor(new Color(255, 233, 180));
			val.set_StrokeText(true);
			((Control)val).set_Location(new Point(layout.TextX, y));
			((Control)val).set_Size(new Point(layout.TextLabelWidth, 28));
			((Control)val).set_Parent(parent);
			y += 32;
			return AddWrappedLabel(parent, layout, text, y, GameService.Content.get_DefaultFont16(), layout.TextLineHeight);
		}

		private void BuildProfileTraits(Container parent, int y)
		{
			bool experienceSet = _profile.Experience != ProfileExperience.Hidden;
			string preferences = SelectedPreferences(_profile.Preferences, Environment.NewLine);
			string themes = SelectedThemes(_profile.Themes, Environment.NewLine);
			string styles = SelectedStyles(_profile.Styles, Environment.NewLine);
			int x = 0;
			AddProfileTraitLabel(parent, ref x, y, experienceSet ? ("Experience: " + ProfileLabels.GetExperienceLabel(_profile.Experience)) : "Experience", experienceSet ? null : MakeProfileTraitTooltip("Experience", "No experience set."), experienceSet);
			AddProfileTraitSeparator(parent, ref x, y);
			AddProfileTraitLabel(parent, ref x, y, "Preferences", MakeProfileTraitTooltip("Preferences", string.IsNullOrWhiteSpace(preferences) ? "No preferences set." : preferences), !string.IsNullOrWhiteSpace(preferences));
			AddProfileTraitSeparator(parent, ref x, y);
			AddProfileTraitLabel(parent, ref x, y, "Themes", MakeProfileTraitTooltip("Themes", string.IsNullOrWhiteSpace(themes) ? "No themes set." : themes), !string.IsNullOrWhiteSpace(themes));
			AddProfileTraitSeparator(parent, ref x, y);
			AddProfileTraitLabel(parent, ref x, y, "Styles", MakeProfileTraitTooltip("Styles", string.IsNullOrWhiteSpace(styles) ? "No styles set." : styles), !string.IsNullOrWhiteSpace(styles));
			if (IsMatureProfile())
			{
				AddProfileTraitSeparator(parent, ref x, y);
				AddProfileTraitLabel(parent, ref x, y, "Mature", MakeProfileTraitTooltip("Mature", "This profile is marked Mature/18+."), isSet: true);
			}
		}

		private static void AddProfileTraitLabel(Container parent, ref int x, int y, string text, Tooltip tooltip, bool isSet)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			int width = GetProfileTraitWidth(text);
			Label val = new Label();
			val.set_Text(text);
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(isSet ? new Color(220, 220, 220) : new Color(125, 125, 125));
			val.set_WrapText(false);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Size(new Point(width, 25));
			((Control)val).set_Tooltip(tooltip);
			((Control)val).set_Parent(parent);
			x += width;
		}

		private static Tooltip MakeProfileTraitTooltip(string title, string description)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			return new Tooltip((ITooltipView)(object)new ProfileTooltipView(title, description));
		}

		private static void AddProfileTraitSeparator(Container parent, ref int x, int y)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			int width = GetProfileTraitWidth(" | ");
			Label val = new Label();
			val.set_Text(" | ");
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(new Color(150, 150, 150));
			val.set_WrapText(false);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Size(new Point(width, 25));
			((Control)val).set_Parent(parent);
			x += width;
		}

		private static int GetProfileTraitWidth(string text)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			return (int)Math.Ceiling(GameService.Content.get_DefaultFont16().MeasureString(text ?? string.Empty).Width) + 2;
		}

		private static string SelectedPreferences(ProfilePreferenceFlags flags, string separator)
		{
			return string.Join(separator, from option in ProfileLabels.PreferenceOptions
				where (flags & option.Key) == option.Key
				select option.Value);
		}

		private static string SelectedThemes(ProfileThemeFlags flags, string separator)
		{
			return string.Join(separator, from option in ProfileLabels.ThemeOptions
				where (flags & option.Key) == option.Key
				select option.Value);
		}

		private static string SelectedStyles(ProfileStyleFlags flags, string separator)
		{
			return string.Join(separator, from option in ProfileLabels.StyleOptions
				where (flags & option.Key) == option.Key
				select option.Value);
		}

		private int AddWrappedLabel(Container parent, Layout layout, string text, int y, BitmapFont font, int lineHeight)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			if (parent == null || layout == null)
			{
				return y;
			}
			foreach (string line in WrapTextLines(text, layout.TextWidth, font))
			{
				if (line.Length == 0)
				{
					y += layout.ParagraphGap;
					continue;
				}
				Label val = new Label();
				val.set_Text(line);
				val.set_Font(font);
				val.set_TextColor(Color.get_White());
				val.set_WrapText(false);
				((Control)val).set_Location(new Point(layout.TextX, y));
				((Control)val).set_Size(new Point(layout.TextLabelWidth, lineHeight));
				((Control)val).set_Parent(parent);
				y += lineHeight;
			}
			return y + layout.ParagraphGap;
		}

		private IEnumerable<AtAGlanceEntry> GetGlanceEntries()
		{
			return (_profile.AtAGlance ?? new List<AtAGlanceEntry>()).Where((AtAGlanceEntry entry) => entry != null && entry.AssetId > 0).Take(5);
		}

		private static Tooltip MakeGlanceTooltip(AtAGlanceEntry entry)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			if (entry != null && (!string.IsNullOrWhiteSpace(entry.Title) || !string.IsNullOrWhiteSpace(entry.Description)))
			{
				return new Tooltip((ITooltipView)(object)new ProfileTooltipView(entry.Title, entry.Description, "At A Glance"));
			}
			return null;
		}

		private string GetBookmarkButtonText()
		{
			if (!IsBookmarked())
			{
				return "Bookmark";
			}
			return "Remove Bookmark";
		}

		private bool IsBookmarked()
		{
			try
			{
				return _isBookmarked?.Invoke(_profile, _presence) ?? false;
			}
			catch
			{
				return false;
			}
		}

		private string GetBlockButtonText()
		{
			if (!IsBlocked())
			{
				return "Block";
			}
			return "Unblock";
		}

		private bool IsBlocked()
		{
			try
			{
				return _isBlocked?.Invoke(_profile, _presence) ?? false;
			}
			catch
			{
				return false;
			}
		}

		private void OpenReportPanel()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Expected O, but got Unknown
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Expected O, but got Unknown
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Expected O, but got Unknown
			CloseReportPanel();
			Container popupParent = (Container)(_contentPanel ?? ((object)_buildPanel) ?? ((object)GameService.Graphics.get_SpriteScreen()));
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("Report Profile");
			((Control)val).set_Size(new Point(460, 170));
			((Control)val).set_Location(GetCenteredPopupLocation(popupParent, 460, 170));
			((Control)val).set_Parent(popupParent);
			((Control)val).set_BackgroundColor(new Color(38, 35, 32));
			((Control)val).set_ClipsBounds(false);
			((Control)val).set_ZIndex(100);
			_reportPanel = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("X");
			((Control)val2).set_Location(new Point(428, -28));
			((Control)val2).set_Size(new Point(24, 24));
			((Control)val2).set_Parent((Container)(object)_reportPanel);
			((Control)val2).set_ClipsBounds(false);
			((Control)val2).set_ZIndex(10011);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CloseReportPanel();
			});
			Label val3 = new Label();
			val3.set_Text($"Report reason ({140} characters)");
			val3.set_Font(GameService.Content.get_DefaultFont14());
			val3.set_TextColor(Color.get_White());
			((Control)val3).set_Location(new Point(12, 14));
			((Control)val3).set_Size(new Point(390, 24));
			((Control)val3).set_Parent((Container)(object)_reportPanel);
			TextBox val4 = new TextBox();
			((TextInputBase)val4).set_Text(string.Empty);
			((TextInputBase)val4).set_PlaceholderText("Please provide a reason for reporting this profile.");
			((TextInputBase)val4).set_MaxLength(140);
			((Control)val4).set_Location(new Point(12, 42));
			((Control)val4).set_Size(new Point(432, 32));
			((Control)val4).set_Parent((Container)(object)_reportPanel);
			TextBox reasonBox = val4;
			Label val5 = new Label();
			val5.set_Text(string.Empty);
			val5.set_Font(GameService.Content.get_DefaultFont12());
			val5.set_TextColor(new Color(220, 220, 220));
			val5.set_WrapText(true);
			((Control)val5).set_Location(new Point(12, 122));
			((Control)val5).set_Size(new Point(432, 38));
			((Control)val5).set_Parent((Container)(object)_reportPanel);
			Label popupStatus = val5;
			StandardButton val6 = new StandardButton();
			val6.set_Text("Submit");
			((Control)val6).set_Location(new Point(354, 84));
			((Control)val6).set_Size(new Point(90, 30));
			((Control)val6).set_Parent((Container)(object)_reportPanel);
			SparkUiActions.BindClick(val6, async delegate
			{
				await SubmitReportAsync(((TextInputBase)reasonBox).get_Text(), popupStatus);
			}, delegate(string text)
			{
				popupStatus.set_Text(text ?? string.Empty);
			}, "Report failed.");
			reasonBox.add_EnterPressed((EventHandler<EventArgs>)delegate
			{
				SubmitReportSafelyAsync(((TextInputBase)reasonBox).get_Text(), popupStatus);
			});
		}

		private async Task SubmitReportAsync(string reason, Label popupStatus)
		{
			reason = reason?.Trim() ?? string.Empty;
			if (string.IsNullOrWhiteSpace(reason))
			{
				popupStatus.set_Text("Please add a reason for reporting this character.");
				return;
			}
			if (_isSubmittingReport)
			{
				if (popupStatus != null)
				{
					popupStatus.set_Text("Report is already being submitted.");
				}
				return;
			}
			_isSubmittingReport = true;
			try
			{
				if (popupStatus != null)
				{
					popupStatus.set_Text("Submitting report...");
				}
				string message = ((_reportProfile != null) ? (await _reportProfile(_profile, _presence, reason)) : "Report failed.");
				SparkUiThread.Queue(delegate
				{
					if (_contentPanel != null)
					{
						SetStatusText(message);
						CloseReportPanel();
					}
				});
			}
			finally
			{
				_isSubmittingReport = false;
			}
		}

		private async Task SubmitReportSafelyAsync(string reason, Label popupStatus)
		{
			try
			{
				await SubmitReportAsync(reason, popupStatus);
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Spark report failed");
				SparkUiThread.Queue(delegate
				{
					if (_contentPanel != null && popupStatus != null)
					{
						popupStatus.set_Text("Report failed. Please try again.");
					}
				});
			}
		}

		private void CloseReportPanel()
		{
			Panel reportPanel = _reportPanel;
			if (reportPanel != null)
			{
				((Control)reportPanel).Dispose();
			}
			_reportPanel = null;
		}

		private static Point GetCenteredPopupLocation(Container parent, int width, int height)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			Point size;
			if (parent == null)
			{
				size = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			}
			else
			{
				Rectangle contentRegion = parent.get_ContentRegion();
				size = ((Rectangle)(ref contentRegion)).get_Size();
			}
			int x = (size.X - width) / 2;
			int y = (size.Y - height) / 2;
			return new Point(Math.Max(8, x), Math.Max(8, y));
		}

		private bool CanReportViewedProfile()
		{
			if (!string.IsNullOrWhiteSpace(TextUtil.FirstNonEmpty(_presence?.AccountName, _profile?.AccountName)) && !string.IsNullOrWhiteSpace(TextUtil.FirstNonEmpty(_presence?.OfficialCharacterName, _profile?.CharacterName)))
			{
				return !string.IsNullOrWhiteSpace(TextUtil.FirstNonEmpty(_presence?.ActiveProfileId, _profile?.ProfileId));
			}
			return false;
		}

		private string GetPronounsText()
		{
			return _profile.Pronouns?.Trim() ?? string.Empty;
		}

		private static bool ShouldWrapHeader(string displayName, string pronouns, string officialName)
		{
			string combinedHeader = displayName ?? string.Empty;
			if (!string.IsNullOrWhiteSpace(pronouns))
			{
				combinedHeader = combinedHeader + " (" + pronouns.Trim() + ")";
			}
			if (!string.IsNullOrWhiteSpace(officialName))
			{
				combinedHeader = combinedHeader + " (" + officialName.Trim() + ")";
			}
			if (combinedHeader.Length > 28)
			{
				if (string.IsNullOrWhiteSpace(pronouns))
				{
					return !string.IsNullOrWhiteSpace(officialName);
				}
				return true;
			}
			return false;
		}

		private static int GetSecondaryHeaderWidth(string text)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			return Math.Min(480, (int)Math.Ceiling(GameService.Content.get_DefaultFont16().MeasureString(text ?? string.Empty).Width) + 4);
		}

		private string GetKnownForText()
		{
			if (!string.IsNullOrWhiteSpace(_profile.KnownFor))
			{
				return _profile.KnownFor.Trim();
			}
			return "Not set.";
		}

		private string GetDescriptionText()
		{
			if (!string.IsNullOrWhiteSpace(_profile.Description))
			{
				return _profile.Description.Trim();
			}
			return "Missing description.";
		}

		private string GetCurrentlyText()
		{
			if (!string.IsNullOrWhiteSpace(_profile.Currently))
			{
				return _profile.Currently.Trim();
			}
			return string.Empty;
		}

		private string GetOtherInfoText()
		{
			string outOfCharacterInfo = ((!_profile.UseGlobalOutOfCharacterInfo) ? _profile.OutOfCharacterInfo : _presence?.OutOfCharacterInfo);
			if (!string.IsNullOrWhiteSpace(outOfCharacterInfo))
			{
				return outOfCharacterInfo.Trim();
			}
			return string.Empty;
		}

		[IteratorStateMachine(typeof(_003CWrapTextLines_003Ed__65))]
		private static IEnumerable<string> WrapTextLines(string text, float maxWidth, BitmapFont font)
		{
			return new _003CWrapTextLines_003Ed__65(-2)
			{
				_003C_003E3__text = text,
				_003C_003E3__maxWidth = maxWidth,
				_003C_003E3__font = font
			};
		}

		[IteratorStateMachine(typeof(_003CBreakLongWord_003Ed__66))]
		private static IEnumerable<string> BreakLongWord(string word, float maxWidth, BitmapFont font)
		{
			return new _003CBreakLongWord_003Ed__66(-2)
			{
				_003C_003E3__word = word,
				_003C_003E3__maxWidth = maxWidth,
				_003C_003E3__font = font
			};
		}

		private async Task CopyAccountNameAsync()
		{
			string accountName = ProfileText.AccountName(_profile, _presence, string.Empty);
			if (string.IsNullOrWhiteSpace(accountName))
			{
				_status.set_Text("No account name available for this profile.");
				return;
			}
			string statusText;
			try
			{
				await CopyTextAsync(accountName.Trim());
				statusText = "Copied " + accountName.Trim() + ".";
			}
			catch
			{
				statusText = "Couldn't copy the account name right now.";
			}
			SparkUiThread.Queue(delegate
			{
				SetStatusText(statusText);
			});
		}

		private void SetStatusText(string text)
		{
			if (_status != null)
			{
				_status.set_Text(text ?? string.Empty);
			}
		}

		private static async Task CopyTextAsync(string text)
		{
			if (!(await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text ?? string.Empty)))
			{
				throw new InvalidOperationException("Could not copy text.");
			}
		}

		protected override void Unload()
		{
			CloseReportPanel();
			ClearChildren((Container)(object)_contentPanel);
			Panel contentPanel = _contentPanel;
			if (contentPanel != null)
			{
				((Control)contentPanel).Dispose();
			}
			_contentPanel = null;
			_scrollViewport = null;
			_layout = null;
			_status = null;
			_buildPanel = null;
		}
	}
}
