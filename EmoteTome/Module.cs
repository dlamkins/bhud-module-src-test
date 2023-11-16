using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WindowsInput;

namespace EmoteTome
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private CornerIcon tomeCornerIcon;

		private StandardWindow tomeWindow;

		private int language = BadLocalization.ENGLISH;

		private Vector3 currentPositionA;

		private Vector3 currentPositionB;

		private Vector3 currentPositionC;

		private int checkPositionSwitch;

		private List<Emote> coreEmoteList = new List<Emote>();

		private List<Emote> unlockEmoteList = new List<Emote>();

		private List<Emote> rankEmoteList = new List<Emote>();

		private Color activatedColor = new Color(250, 250, 250);

		private Color lockedColor = new Color(30, 30, 30);

		private Color noTargetColor = new Color(130, 130, 130);

		private Color cooldownColor = new Color(50, 50, 50);

		private bool checkedAPIForUnlock;

		private EventHandler<MouseEventArgs> coreEmoteClickEvent = delegate
		{
		};

		private EventHandler<MouseEventArgs> unlockEmoteClickEvent = delegate
		{
		};

		private EventHandler<MouseEventArgs> rankEmoteClickEvent = delegate
		{
		};

		private EventHandler<MouseEventArgs> testEmoteClickEvent = delegate
		{
		};

		private SettingEntry<bool> _showEmoteNames;

		private SettingEntry<bool> _adjustLabelLength;

		private SettingEntry<bool> _halloweenMode;

		private SettingEntry<string> _coreEmoteSeparator;

		private SettingEntry<bool> _showBeckon;

		private SettingEntry<bool> _showBow;

		private SettingEntry<bool> _showCheer;

		private SettingEntry<bool> _showCower;

		private SettingEntry<bool> _showCrossarms;

		private SettingEntry<bool> _showCry;

		private SettingEntry<bool> _showDance;

		private SettingEntry<bool> _showFacepalm;

		private SettingEntry<bool> _showKneel;

		private SettingEntry<bool> _showLaugh;

		private SettingEntry<bool> _showNo;

		private SettingEntry<bool> _showPoint;

		private SettingEntry<bool> _showPonder;

		private SettingEntry<bool> _showSad;

		private SettingEntry<bool> _showSalute;

		private SettingEntry<bool> _showShrug;

		private SettingEntry<bool> _showSit;

		private SettingEntry<bool> _showSleep;

		private SettingEntry<bool> _showSurprised;

		private SettingEntry<bool> _showTalk;

		private SettingEntry<bool> _showThanks;

		private SettingEntry<bool> _showThreaten;

		private SettingEntry<bool> _showWave;

		private SettingEntry<bool> _showYes;

		private List<Tuple<SettingEntry<bool>, Emote>> coreEmoteSettingMap = new List<Tuple<SettingEntry<bool>, Emote>>();

		private SettingEntry<string> _unlockEmoteSeparator;

		private SettingEntry<bool> _showBless;

		private SettingEntry<bool> _showGeargrind;

		private SettingEntry<bool> _showHeroic;

		private SettingEntry<bool> _showHiss;

		private SettingEntry<bool> _showMagicjuggle;

		private SettingEntry<bool> _showPaper;

		private SettingEntry<bool> _showPlaydead;

		private SettingEntry<bool> _showPossessed;

		private SettingEntry<bool> _showReadbook;

		private SettingEntry<bool> _showRock;

		private SettingEntry<bool> _showRockout;

		private SettingEntry<bool> _showScissors;

		private SettingEntry<bool> _showServe;

		private SettingEntry<bool> _showShiver;

		private SettingEntry<bool> _showShiverplus;

		private SettingEntry<bool> _showShuffle;

		private SettingEntry<bool> _showSipcoffee;

		private SettingEntry<bool> _showStep;

		private SettingEntry<bool> _showStretch;

		private List<Tuple<SettingEntry<bool>, Emote>> unlockEmoteSettingMap = new List<Tuple<SettingEntry<bool>, Emote>>();

		private SettingEntry<string> _rankEmoteSeparator;

		private SettingEntry<bool> _showYourRank;

		private SettingEntry<bool> _showRankRabbit;

		private SettingEntry<bool> _showRankDeer;

		private SettingEntry<bool> _showRankDolyak;

		private SettingEntry<bool> _showRankWolf;

		private SettingEntry<bool> _showRankTiger;

		private SettingEntry<bool> _showRankBear;

		private SettingEntry<bool> _showRankShark;

		private SettingEntry<bool> _showRankPhoenix;

		private SettingEntry<bool> _showRankDragon;

		private List<Tuple<SettingEntry<bool>, Emote>> rankEmoteSettingMap = new List<Tuple<SettingEntry<bool>, Emote>>();

		private int size = 64;

		private int labelSize = 16;

		private int labelWidth = 120;

		private static readonly Logger Logger = Logger.GetLogger<Module>();

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)


		protected override void DefineSettings(SettingCollection settings)
		{
			_showEmoteNames = settings.DefineSetting<bool>("Show Names", false, (Func<string>)(() => BadLocalization.SHOWNAMES[language]), (Func<string>)(() => BadLocalization.SHOWNAMESTEXT[language]));
			_adjustLabelLength = settings.DefineSetting<bool>("Larger Name Labels", false, (Func<string>)(() => BadLocalization.LARGERNAMELABELS[language]), (Func<string>)(() => BadLocalization.LARGERNAMELABELSTEXT[language]));
			_halloweenMode = settings.DefineSetting<bool>("Halloween Mode", false, (Func<string>)(() => BadLocalization.HALLOWEENMODE[language]), (Func<string>)(() => BadLocalization.HALLOWEENMODETEXT[language]));
			_coreEmoteSeparator = settings.DefineSetting<string>("Core Separator", "", (Func<string>)(() => BadLocalization.COREPANELTITLE[language]), (Func<string>)(() => ""));
			_showBeckon = settings.DefineSetting<bool>("Show Beckon", true, (Func<string>)(() => BadLocalization.BECKON[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showBow = settings.DefineSetting<bool>("Show Bow", true, (Func<string>)(() => BadLocalization.BOW[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showCheer = settings.DefineSetting<bool>("Show Cheer", true, (Func<string>)(() => BadLocalization.CHEER[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showCower = settings.DefineSetting<bool>("Show Cower", true, (Func<string>)(() => BadLocalization.COWER[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showCrossarms = settings.DefineSetting<bool>("Show Crossarms", true, (Func<string>)(() => BadLocalization.CROSSARMS[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showCry = settings.DefineSetting<bool>("Show Cry", true, (Func<string>)(() => BadLocalization.CRY[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showDance = settings.DefineSetting<bool>("Show Dance", true, (Func<string>)(() => BadLocalization.DANCE[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showFacepalm = settings.DefineSetting<bool>("Show Facepalm", true, (Func<string>)(() => BadLocalization.FACEPALM[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showKneel = settings.DefineSetting<bool>("Show Kneel", true, (Func<string>)(() => BadLocalization.KNEEL[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showLaugh = settings.DefineSetting<bool>("Show Laugh", true, (Func<string>)(() => BadLocalization.LAUGH[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showNo = settings.DefineSetting<bool>("Show No", true, (Func<string>)(() => BadLocalization.NO[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showPoint = settings.DefineSetting<bool>("Show Point", true, (Func<string>)(() => BadLocalization.POINT[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showPonder = settings.DefineSetting<bool>("Show Ponder", true, (Func<string>)(() => BadLocalization.PONDER[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showSad = settings.DefineSetting<bool>("Show Sad", true, (Func<string>)(() => BadLocalization.SAD[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showSalute = settings.DefineSetting<bool>("Show Salute", true, (Func<string>)(() => BadLocalization.SALUTE[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showShrug = settings.DefineSetting<bool>("Show Shrug", true, (Func<string>)(() => BadLocalization.SHRUG[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showSit = settings.DefineSetting<bool>("Show Sit", true, (Func<string>)(() => BadLocalization.SIT[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showSleep = settings.DefineSetting<bool>("Show Sleep", true, (Func<string>)(() => BadLocalization.SLEEP[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showSurprised = settings.DefineSetting<bool>("Show Surprised", true, (Func<string>)(() => BadLocalization.SURPRISED[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showTalk = settings.DefineSetting<bool>("Show Talk", true, (Func<string>)(() => BadLocalization.TALK[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showThanks = settings.DefineSetting<bool>("Show Thanks", true, (Func<string>)(() => BadLocalization.THANKS[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showThreaten = settings.DefineSetting<bool>("Show Threaten", true, (Func<string>)(() => BadLocalization.THREATEN[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showWave = settings.DefineSetting<bool>("Show Wave", true, (Func<string>)(() => BadLocalization.WAVE[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showYes = settings.DefineSetting<bool>("Show Yes", true, (Func<string>)(() => BadLocalization.YES[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_unlockEmoteSeparator = settings.DefineSetting<string>("Unlock Separator", "", (Func<string>)(() => BadLocalization.UNLOCKABLEPANELTITLE[language]), (Func<string>)(() => ""));
			_showBless = settings.DefineSetting<bool>("Show Bless", true, (Func<string>)(() => BadLocalization.BLESS[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showGeargrind = settings.DefineSetting<bool>("Show Geargrind", true, (Func<string>)(() => BadLocalization.GEARGRIND[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showHeroic = settings.DefineSetting<bool>("Show Heroic", true, (Func<string>)(() => BadLocalization.HEROIC[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showHiss = settings.DefineSetting<bool>("Show Hiss", true, (Func<string>)(() => BadLocalization.HISS[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showMagicjuggle = settings.DefineSetting<bool>("Show Magicjuggle", true, (Func<string>)(() => BadLocalization.MAGICJUGGLE[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showPaper = settings.DefineSetting<bool>("Show Paper", true, (Func<string>)(() => BadLocalization.PAPER[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showPlaydead = settings.DefineSetting<bool>("Show Playdead", true, (Func<string>)(() => BadLocalization.PLAYDEAD[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showPossessed = settings.DefineSetting<bool>("Show Possessed", true, (Func<string>)(() => BadLocalization.POSSESSED[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showReadbook = settings.DefineSetting<bool>("Show Readbook", true, (Func<string>)(() => BadLocalization.READBOOK[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showRock = settings.DefineSetting<bool>("Show Rock", true, (Func<string>)(() => BadLocalization.ROCK[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showRockout = settings.DefineSetting<bool>("Show Rockout", true, (Func<string>)(() => BadLocalization.ROCKOUT[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showScissors = settings.DefineSetting<bool>("Show Scissors", true, (Func<string>)(() => BadLocalization.SCISSORS[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showServe = settings.DefineSetting<bool>("Show Serve", true, (Func<string>)(() => BadLocalization.SERVE[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showShiver = settings.DefineSetting<bool>("Show Shiver", true, (Func<string>)(() => BadLocalization.SHIVER[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showShiverplus = settings.DefineSetting<bool>("Show Shiverplus", true, (Func<string>)(() => BadLocalization.SHIVERPLUS[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showShuffle = settings.DefineSetting<bool>("Show Shuffle", true, (Func<string>)(() => BadLocalization.SHUFFLE[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showSipcoffee = settings.DefineSetting<bool>("Show Sipcoffee", true, (Func<string>)(() => BadLocalization.SIPCOFFEE[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showStep = settings.DefineSetting<bool>("Show Step", true, (Func<string>)(() => BadLocalization.STEP[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showStretch = settings.DefineSetting<bool>("Show Stretch", true, (Func<string>)(() => BadLocalization.STRETCH[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_rankEmoteSeparator = settings.DefineSetting<string>("Rank Separator", "", (Func<string>)(() => BadLocalization.RANKPANELTITLE[language]), (Func<string>)(() => ""));
			_showYourRank = settings.DefineSetting<bool>("Show Your Rank", true, (Func<string>)(() => BadLocalization.YOURRANK[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showRankRabbit = settings.DefineSetting<bool>("Show Rank Rabbit", true, (Func<string>)(() => BadLocalization.RABBIT[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showRankDeer = settings.DefineSetting<bool>("Show Rank Deer", true, (Func<string>)(() => BadLocalization.DEER[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showRankDolyak = settings.DefineSetting<bool>("Show Rank Dolyak", true, (Func<string>)(() => BadLocalization.DOLYAK[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showRankWolf = settings.DefineSetting<bool>("Show Rank Wolf", true, (Func<string>)(() => BadLocalization.WOLF[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showRankTiger = settings.DefineSetting<bool>("Show Rank Tiger", true, (Func<string>)(() => BadLocalization.TIGER[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showRankBear = settings.DefineSetting<bool>("Show Rank Bear", true, (Func<string>)(() => BadLocalization.BEAR[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showRankShark = settings.DefineSetting<bool>("Show Rank Shark", true, (Func<string>)(() => BadLocalization.SHARK[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showRankPhoenix = settings.DefineSetting<bool>("Show Rank Phoenix", true, (Func<string>)(() => BadLocalization.PHOENIX[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showRankDragon = settings.DefineSetting<bool>("Show Rank Dragon", true, (Func<string>)(() => BadLocalization.DRAGON[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
		}

		protected override async Task LoadAsync()
		{
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			switch ((int)value)
			{
			case 0:
				language = BadLocalization.ENGLISH;
				break;
			case 3:
				language = BadLocalization.FRENCH;
				break;
			case 2:
				language = BadLocalization.GERMAN;
				break;
			case 1:
				language = BadLocalization.SPANISH;
				break;
			default:
				language = BadLocalization.ENGLISH;
				break;
			}
			Module module = this;
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("CornerIcon.png")));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Visible(false);
			module.tomeCornerIcon = val;
			((Control)tomeCornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((WindowBase2)tomeWindow).ToggleWindow();
				if (((Control)tomeWindow).get_Visible() && !checkedAPIForUnlock)
				{
					checkUnlockedEmotesByAPI();
					checkedAPIForUnlock = true;
				}
			});
			Module module2 = this;
			StandardWindow val2 = new StandardWindow(ContentsManager.GetTexture("WindowBackground.png"), new Rectangle(40, 26, 913, 691), new Rectangle(70, 71, 839, 605));
			((Control)val2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val2).set_Title(BadLocalization.WINDOWTITLE[language]);
			((WindowBase2)val2).set_SavesPosition(true);
			((WindowBase2)val2).set_SavesSize(true);
			((WindowBase2)val2).set_Id("0001");
			((WindowBase2)val2).set_CanResize(true);
			module2.tomeWindow = val2;
			Checkbox val3 = new Checkbox();
			val3.set_Text(BadLocalization.TARGETCHECKBOXTEXT[language]);
			((Control)val3).set_Location(new Point(0, 0));
			((Control)val3).set_BasicTooltipText(BadLocalization.TARGETCHECKBOXTOOLTIP[language]);
			((Control)val3).set_Parent((Container)(object)tomeWindow);
			Checkbox targetCheckbox = val3;
			Checkbox val4 = new Checkbox();
			val4.set_Text(BadLocalization.SYNCHRONCHECKBOXTEXT[language]);
			((Control)val4).set_Location(new Point(0, 20));
			((Control)val4).set_BasicTooltipText(BadLocalization.SYNCHRONCHECKBOXTOOLTIP[language]);
			((Control)val4).set_Parent((Container)(object)tomeWindow);
			Checkbox synchronCheckbox = val4;
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Size(new Point(((Container)tomeWindow).get_ContentRegion().Width, ((Container)tomeWindow).get_ContentRegion().Height));
			((Control)val5).set_Location(new Point(0, 50));
			val5.set_FlowDirection((ControlFlowDirection)3);
			((Control)val5).set_Parent((Container)(object)tomeWindow);
			((Panel)val5).set_CanScroll(true);
			((Control)val5).set_Padding(new Thickness(20f));
			FlowPanel mainPanel = val5;
			EmoteLibrary library = new EmoteLibrary(ContentsManager);
			coreEmoteList = library.loadCoreEmotes();
			List<EmoteContainer> coreEmoteContainers = new List<EmoteContainer>();
			FlowPanel val6 = new FlowPanel();
			((Panel)val6).set_ShowBorder(true);
			((Panel)val6).set_Title(BadLocalization.COREPANELTITLE[language]);
			((Control)val6).set_Size(new Point(((Container)mainPanel).get_ContentRegion().Width, ((Container)mainPanel).get_ContentRegion().Height));
			((Control)val6).set_Parent((Container)(object)mainPanel);
			((Panel)val6).set_CanCollapse(true);
			val6.set_FlowDirection((ControlFlowDirection)0);
			((Container)val6).set_HeightSizingMode((SizingMode)1);
			((Container)val6).set_AutoSizePadding(new Point(5, 5));
			val6.set_ControlPadding(new Vector2(5f, 5f));
			val6.set_OuterControlPadding(new Vector2(5f, 5f));
			FlowPanel corePanel = val6;
			foreach (Emote emote3 in coreEmoteList)
			{
				if (!emote3.getCategory().Equals(EmoteLibrary.CORECODE))
				{
					continue;
				}
				EmoteContainer emoteContainer4 = new EmoteContainer();
				((Control)emoteContainer4).set_Size(new Point(size, size));
				((Control)emoteContainer4).set_BasicTooltipText(emote3.getToolTipp()[language]);
				((Control)emoteContainer4).set_Parent((Container)(object)corePanel);
				EmoteContainer emoteContainer3 = emoteContainer4;
				Image val7 = new Image(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture(emote3.getImagePath())));
				((Control)val7).set_Size(new Point(size, size));
				((Control)val7).set_BasicTooltipText(emote3.getToolTipp()[language]);
				((Control)val7).set_ZIndex(1);
				((Control)val7).set_Parent((Container)(object)emoteContainer3);
				Image emoteImage3 = val7;
				Label val8 = new Label();
				val8.set_Text(emote3.getToolTipp()[language]);
				val8.set_HorizontalAlignment((HorizontalAlignment)1);
				((Control)val8).set_Size(new Point(size, labelSize));
				((Control)val8).set_ZIndex(2);
				((Control)val8).set_Parent((Container)(object)emoteContainer3);
				val8.set_AutoSizeWidth(false);
				((Control)val8).set_Visible(false);
				((Control)val8).set_BackgroundColor(Color.get_Black());
				((Control)val8).set_Location(new Point(0, size - 1));
				Label emoteLabel3 = val8;
				emoteContainer3.setImage(emoteImage3);
				emoteContainer3.setLabel(emoteLabel3);
				coreEmoteClickEvent = delegate
				{
					if (emoteAllowed())
					{
						activateEmote(emote3.getChatCode(), targetCheckbox.get_Checked(), synchronCheckbox.get_Checked());
					}
				};
				((Control)emoteContainer3).add_Click(coreEmoteClickEvent);
				coreEmoteContainers.Add(emoteContainer3);
				emote3.setContainer(emoteContainer3);
			}
			unlockEmoteList = library.loadUnlockEmotes();
			List<EmoteContainer> unlockEmoteContainers = new List<EmoteContainer>();
			FlowPanel val9 = new FlowPanel();
			((Panel)val9).set_ShowBorder(true);
			((Panel)val9).set_Title(BadLocalization.UNLOCKABLEPANELTITLE[language]);
			((Control)val9).set_Size(new Point(((Container)mainPanel).get_ContentRegion().Width, ((Container)mainPanel).get_ContentRegion().Height));
			((Control)val9).set_Parent((Container)(object)mainPanel);
			((Panel)val9).set_CanCollapse(true);
			val9.set_FlowDirection((ControlFlowDirection)0);
			((Container)val9).set_HeightSizingMode((SizingMode)1);
			((Container)val9).set_AutoSizePadding(new Point(5, 5));
			val9.set_ControlPadding(new Vector2(5f, 5f));
			val9.set_OuterControlPadding(new Vector2(5f, 5f));
			FlowPanel unlockablePanel = val9;
			foreach (Emote emote2 in unlockEmoteList)
			{
				if (!emote2.getCategory().Equals(EmoteLibrary.UNLOCKCODE))
				{
					continue;
				}
				EmoteContainer emoteContainer5 = new EmoteContainer();
				((Control)emoteContainer5).set_Size(new Point(size, size));
				((Control)emoteContainer5).set_BasicTooltipText(emote2.getToolTipp()[language]);
				((Control)emoteContainer5).set_Parent((Container)(object)unlockablePanel);
				EmoteContainer emoteContainer2 = emoteContainer5;
				Image val10 = new Image(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture(emote2.getImagePath())));
				((Control)val10).set_Size(new Point(size, size));
				((Control)val10).set_BasicTooltipText(emote2.getToolTipp()[language]);
				((Control)val10).set_ZIndex(1);
				((Control)val10).set_Parent((Container)(object)emoteContainer2);
				val10.set_Tint(lockedColor);
				((Control)val10).set_Enabled(false);
				Image emoteImage2 = val10;
				Label val11 = new Label();
				val11.set_Text(emote2.getToolTipp()[language]);
				val11.set_HorizontalAlignment((HorizontalAlignment)1);
				((Control)val11).set_Size(new Point(size, labelSize));
				((Control)val11).set_ZIndex(2);
				((Control)val11).set_Parent((Container)(object)emoteContainer2);
				val11.set_AutoSizeWidth(false);
				((Control)val11).set_Visible(false);
				((Control)val11).set_BackgroundColor(Color.get_Black());
				((Control)val11).set_Location(new Point(0, size - 1));
				Label emoteLabel2 = val11;
				emoteContainer2.setImage(emoteImage2);
				emoteContainer2.setLabel(emoteLabel2);
				unlockEmoteClickEvent = delegate
				{
					if (emoteAllowed())
					{
						activateEmote(emote2.getChatCode(), targetCheckbox.get_Checked(), synchronCheckbox.get_Checked());
					}
				};
				((Control)emoteContainer2).add_Click(unlockEmoteClickEvent);
				unlockEmoteContainers.Add(emoteContainer2);
				emote2.setContainer(emoteContainer2);
				emote2.isDeactivatedByLocked(newBool: true);
			}
			rankEmoteList = library.loadRankEmotes();
			List<EmoteContainer> cooldownEmoteContainers = new List<EmoteContainer>();
			FlowPanel val12 = new FlowPanel();
			((Panel)val12).set_ShowBorder(true);
			((Panel)val12).set_Title(BadLocalization.RANKPANELTITLE[language]);
			((Control)val12).set_Size(new Point(((Container)mainPanel).get_ContentRegion().Width, ((Container)mainPanel).get_ContentRegion().Height));
			((Control)val12).set_Parent((Container)(object)mainPanel);
			((Panel)val12).set_CanCollapse(true);
			val12.set_FlowDirection((ControlFlowDirection)0);
			((Container)val12).set_HeightSizingMode((SizingMode)1);
			((Container)val12).set_AutoSizePadding(new Point(5, 5));
			val12.set_ControlPadding(new Vector2(5f, 5f));
			val12.set_OuterControlPadding(new Vector2(5f, 5f));
			FlowPanel rankPanel = val12;
			foreach (Emote emote in rankEmoteList)
			{
				if (!emote.getCategory().Equals(EmoteLibrary.RANKCODE))
				{
					continue;
				}
				EmoteContainer emoteContainer6 = new EmoteContainer();
				((Control)emoteContainer6).set_Size(new Point(size, size));
				((Control)emoteContainer6).set_BasicTooltipText(emote.getToolTipp()[language]);
				((Control)emoteContainer6).set_Parent((Container)(object)rankPanel);
				EmoteContainer emoteContainer = emoteContainer6;
				Image val13 = new Image(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture(emote.getImagePath())));
				((Control)val13).set_Size(new Point(size, size));
				((Control)val13).set_BasicTooltipText(emote.getToolTipp()[language]);
				((Control)val13).set_ZIndex(1);
				((Control)val13).set_Parent((Container)(object)emoteContainer);
				val13.set_Tint(lockedColor);
				((Control)val13).set_Enabled(false);
				Image emoteImage = val13;
				Label val14 = new Label();
				val14.set_Text(emote.getToolTipp()[language]);
				val14.set_HorizontalAlignment((HorizontalAlignment)1);
				((Control)val14).set_Size(new Point(size, labelSize));
				((Control)val14).set_ZIndex(2);
				((Control)val14).set_Parent((Container)(object)emoteContainer);
				val14.set_AutoSizeWidth(false);
				((Control)val14).set_Visible(false);
				((Control)val14).set_BackgroundColor(Color.get_Black());
				((Control)val14).set_Location(new Point(0, size - 1));
				Label emoteLabel = val14;
				Label val15 = new Label();
				val15.set_Text("60");
				val15.set_HorizontalAlignment((HorizontalAlignment)1);
				((Control)val15).set_Size(new Point(size, size));
				((Control)val15).set_ZIndex(3);
				val15.set_Font(GameService.Content.get_DefaultFont32());
				((Control)val15).set_Parent((Container)(object)emoteContainer);
				val15.set_AutoSizeWidth(false);
				((Control)val15).set_Visible(false);
				Label cooldownLabel = val15;
				emoteContainer.setImage(emoteImage);
				emoteContainer.setLabel(emoteLabel);
				emoteContainer.setCooldownLabel(cooldownLabel);
				rankEmoteClickEvent = delegate
				{
					if (emoteAllowed())
					{
						activateEmote(emote.getChatCode(), targetCheckbox.get_Checked(), synchronCheckbox.get_Checked());
						activateCooldown();
					}
				};
				((Control)emoteContainer).add_Click(rankEmoteClickEvent);
				cooldownEmoteContainers.Add(emoteContainer);
				emote.setContainer(emoteContainer);
				emote.isDeactivatedByLocked(newBool: true);
			}
			Panel val16 = new Panel();
			((Control)val16).set_Size(new Point(((Container)mainPanel).get_ContentRegion().Width, 50));
			((Control)val16).set_Parent((Container)(object)mainPanel);
			Panel spacePanel = val16;
			if (_showEmoteNames.get_Value())
			{
				activateNameLabel(coreEmoteList);
				activateNameLabel(unlockEmoteList);
				activateNameLabel(rankEmoteList);
			}
			if (_adjustLabelLength.get_Value())
			{
				activateLongLabel(coreEmoteList);
				activateLongLabel(unlockEmoteList);
				activateLongLabel(rankEmoteList);
			}
			List<SettingEntry<bool>> coreSettingList = new List<SettingEntry<bool>>();
			coreSettingList.Add(_showBeckon);
			coreSettingList.Add(_showBow);
			coreSettingList.Add(_showCheer);
			coreSettingList.Add(_showCower);
			coreSettingList.Add(_showCrossarms);
			coreSettingList.Add(_showCry);
			coreSettingList.Add(_showDance);
			coreSettingList.Add(_showFacepalm);
			coreSettingList.Add(_showKneel);
			coreSettingList.Add(_showLaugh);
			coreSettingList.Add(_showNo);
			coreSettingList.Add(_showPoint);
			coreSettingList.Add(_showPonder);
			coreSettingList.Add(_showSad);
			coreSettingList.Add(_showSalute);
			coreSettingList.Add(_showShrug);
			coreSettingList.Add(_showSit);
			coreSettingList.Add(_showSleep);
			coreSettingList.Add(_showSurprised);
			coreSettingList.Add(_showTalk);
			coreSettingList.Add(_showThanks);
			coreSettingList.Add(_showThreaten);
			coreSettingList.Add(_showWave);
			coreSettingList.Add(_showYes);
			try
			{
				for (int k = 0; k < coreEmoteList.Count; k++)
				{
					coreEmoteSettingMap.Add(new Tuple<SettingEntry<bool>, Emote>(coreSettingList[k], coreEmoteList[k]));
				}
				foreach (Tuple<SettingEntry<bool>, Emote> tuple3 in coreEmoteSettingMap)
				{
					if (!tuple3.Item1.get_Value())
					{
						((Control)tuple3.Item2.getContainer()).set_Visible(false);
					}
				}
			}
			catch (Exception)
			{
				ScreenNotification.ShowNotification("Some Error occured on loading.", (NotificationType)0, (Texture2D)null, 4);
			}
			((Panel)corePanel).Collapse();
			await Task.Delay(75);
			((Panel)corePanel).Expand();
			List<SettingEntry<bool>> unlockSettingList = new List<SettingEntry<bool>>();
			unlockSettingList.Add(_showBless);
			unlockSettingList.Add(_showGeargrind);
			unlockSettingList.Add(_showHeroic);
			unlockSettingList.Add(_showHiss);
			unlockSettingList.Add(_showMagicjuggle);
			unlockSettingList.Add(_showPaper);
			unlockSettingList.Add(_showPlaydead);
			unlockSettingList.Add(_showPossessed);
			unlockSettingList.Add(_showReadbook);
			unlockSettingList.Add(_showRock);
			unlockSettingList.Add(_showRockout);
			unlockSettingList.Add(_showScissors);
			unlockSettingList.Add(_showServe);
			unlockSettingList.Add(_showShiver);
			unlockSettingList.Add(_showShiverplus);
			unlockSettingList.Add(_showShuffle);
			unlockSettingList.Add(_showSipcoffee);
			unlockSettingList.Add(_showStep);
			unlockSettingList.Add(_showStretch);
			try
			{
				for (int j = 0; j < unlockEmoteList.Count; j++)
				{
					unlockEmoteSettingMap.Add(new Tuple<SettingEntry<bool>, Emote>(unlockSettingList[j], unlockEmoteList[j]));
				}
				foreach (Tuple<SettingEntry<bool>, Emote> tuple2 in unlockEmoteSettingMap)
				{
					if (!tuple2.Item1.get_Value())
					{
						((Control)tuple2.Item2.getContainer()).set_Visible(false);
					}
				}
			}
			catch (Exception)
			{
				ScreenNotification.ShowNotification("Some Error occured on loading.", (NotificationType)0, (Texture2D)null, 4);
			}
			((Panel)unlockablePanel).Collapse();
			await Task.Delay(75);
			((Panel)unlockablePanel).Expand();
			List<SettingEntry<bool>> rankSettingList = new List<SettingEntry<bool>>();
			rankSettingList.Add(_showYourRank);
			rankSettingList.Add(_showRankRabbit);
			rankSettingList.Add(_showRankDeer);
			rankSettingList.Add(_showRankDolyak);
			rankSettingList.Add(_showRankWolf);
			rankSettingList.Add(_showRankTiger);
			rankSettingList.Add(_showRankBear);
			rankSettingList.Add(_showRankShark);
			rankSettingList.Add(_showRankPhoenix);
			rankSettingList.Add(_showRankDragon);
			try
			{
				for (int i = 0; i < rankEmoteList.Count; i++)
				{
					rankEmoteSettingMap.Add(new Tuple<SettingEntry<bool>, Emote>(rankSettingList[i], rankEmoteList[i]));
				}
				foreach (Tuple<SettingEntry<bool>, Emote> tuple in rankEmoteSettingMap)
				{
					if (!tuple.Item1.get_Value())
					{
						((Control)tuple.Item2.getContainer()).set_Visible(false);
					}
				}
			}
			catch (Exception)
			{
				ScreenNotification.ShowNotification("Some Error occured on loading.", (NotificationType)0, (Texture2D)null, 4);
			}
			((Panel)rankPanel).Collapse();
			await Task.Delay(75);
			((Panel)rankPanel).Expand();
			((SettingEntry)_showEmoteNames).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				if (_showEmoteNames.get_Value())
				{
					activateNameLabel(coreEmoteList);
					activateNameLabel(unlockEmoteList);
					activateNameLabel(rankEmoteList);
				}
				else
				{
					deactivateNameLabel(coreEmoteList);
					deactivateNameLabel(unlockEmoteList);
					deactivateNameLabel(rankEmoteList);
				}
			});
			((SettingEntry)_adjustLabelLength).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				if (_adjustLabelLength.get_Value())
				{
					activateLongLabel(coreEmoteList);
					activateLongLabel(unlockEmoteList);
					activateLongLabel(rankEmoteList);
				}
				else
				{
					deactivateLongLabel(coreEmoteList);
					deactivateLongLabel(unlockEmoteList);
					deactivateLongLabel(rankEmoteList);
				}
			});
			((SettingEntry)_halloweenMode).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				if (_halloweenMode.get_Value())
				{
					halloweenMode(_halloweenMode, value: true);
				}
				else
				{
					halloweenMode(_halloweenMode, value: false);
				}
			});
			showHideEmotes(coreEmoteSettingMap, (Panel)(object)corePanel);
			showHideEmotes(unlockEmoteSettingMap, (Panel)(object)unlockablePanel);
			showHideEmotes(rankEmoteSettingMap, (Panel)(object)rankPanel);
			((Control)tomeWindow).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				//IL_009d: Unknown result type (might be due to invalid IL or missing references)
				((Control)mainPanel).set_Width(((Container)tomeWindow).get_ContentRegion().Width);
				((Control)mainPanel).set_Height(((Container)tomeWindow).get_ContentRegion().Height);
				((Control)corePanel).set_Width(((Container)mainPanel).get_ContentRegion().Width);
				((Control)unlockablePanel).set_Width(((Container)mainPanel).get_ContentRegion().Width);
				((Control)rankPanel).set_Width(((Container)mainPanel).get_ContentRegion().Width);
				((Control)spacePanel).set_Width(((Container)mainPanel).get_ContentRegion().Width);
			});
			targetCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_0118: Unknown result type (might be due to invalid IL or missing references)
				//IL_0170: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
				//IL_023b: Unknown result type (might be due to invalid IL or missing references)
				if (targetCheckbox.get_Checked())
				{
					foreach (Emote current in coreEmoteList)
					{
						if (!current.hasTarget())
						{
							current.getContainer().getImage().set_Tint(noTargetColor);
							current.isDeactivatedByTargeting(newBool: true);
						}
					}
					foreach (Emote current2 in unlockEmoteList)
					{
						if (!current2.hasTarget() && !current2.isDeactivatedByLocked())
						{
							current2.getContainer().getImage().set_Tint(noTargetColor);
							current2.isDeactivatedByTargeting(newBool: true);
						}
					}
					foreach (Emote current3 in rankEmoteList)
					{
						if (!current3.hasTarget() && !current3.isDeactivatedByLocked())
						{
							if (!current3.isDeactivatedByCooldown())
							{
								current3.getContainer().getImage().set_Tint(noTargetColor);
							}
							current3.isDeactivatedByTargeting(newBool: true);
						}
					}
				}
				else
				{
					foreach (Emote coreEmote in coreEmoteList)
					{
						coreEmote.getContainer().getImage().set_Tint(activatedColor);
						coreEmote.isDeactivatedByTargeting(newBool: false);
					}
					foreach (Emote current5 in unlockEmoteList)
					{
						if (!current5.isDeactivatedByLocked())
						{
							current5.getContainer().getImage().set_Tint(activatedColor);
						}
						current5.isDeactivatedByTargeting(newBool: false);
					}
					foreach (Emote current6 in rankEmoteList)
					{
						if (!current6.isDeactivatedByCooldown() && !current6.isDeactivatedByLocked())
						{
							current6.getContainer().getImage().set_Tint(activatedColor);
						}
						current6.isDeactivatedByTargeting(newBool: false);
					}
				}
			});
			((Control)tomeCornerIcon).set_Visible(true);
			void activateCooldown()
			{
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				int cooldown = 60;
				foreach (Emote emote4 in rankEmoteList)
				{
					if (!emote4.isDeactivatedByLocked())
					{
						emote4.getContainer().getImage().set_Tint(cooldownColor);
						((Control)emote4.getContainer()).set_Enabled(false);
						emote4.isDeactivatedByCooldown(newBool: true);
						emote4.getContainer().getCooldownLabel().set_Text(cooldown.ToString());
						((Control)emote4.getContainer().getCooldownLabel()).set_Visible(true);
					}
				}
				System.Timers.Timer aTimer = new System.Timers.Timer();
				aTimer.Elapsed += OnTimedEvent;
				aTimer.Interval = 1000.0;
				aTimer.Enabled = true;
				void OnTimedEvent(object source, ElapsedEventArgs e)
				{
					//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
					//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
					if (cooldown >= 1)
					{
						cooldown--;
						foreach (Emote rankEmote in rankEmoteList)
						{
							rankEmote.getContainer().getCooldownLabel().set_Text(cooldown.ToString());
						}
					}
					else
					{
						cooldown = 60;
						aTimer.Enabled = false;
						foreach (Emote emote5 in rankEmoteList)
						{
							((Control)emote5.getContainer().getCooldownLabel()).set_Visible(false);
							if (!emote5.isDeactivatedByLocked())
							{
								if (emote5.isDeactivatedByTargeting())
								{
									emote5.getContainer().getImage().set_Tint(noTargetColor);
								}
								else
								{
									emote5.getContainer().getImage().set_Tint(activatedColor);
								}
								((Control)emote5.getContainer()).set_Enabled(true);
								emote5.isDeactivatedByCooldown(newBool: false);
							}
						}
					}
				}
			}
		}

		private void showHideEmotes(List<Tuple<SettingEntry<bool>, Emote>> tupleList, Panel panel)
		{
			foreach (Tuple<SettingEntry<bool>, Emote> tuple in tupleList)
			{
				((SettingEntry)tuple.Item1).add_PropertyChanged((PropertyChangedEventHandler)async delegate
				{
					if (tuple.Item1.get_Value())
					{
						((Control)tuple.Item2.getContainer()).set_Visible(true);
					}
					else
					{
						((Control)tuple.Item2.getContainer()).set_Visible(false);
					}
					panel.Collapse();
					await Task.Delay(75);
					panel.Expand();
				});
			}
		}

		private void halloweenMode(SettingEntry<bool> setting, bool value)
		{
			_showBeckon.set_Value(true);
			_showBow.set_Value(true);
			_showCheer.set_Value(true);
			_showCower.set_Value(true);
			_showDance.set_Value(true);
			_showKneel.set_Value(true);
			_showLaugh.set_Value(true);
			_showNo.set_Value(true);
			_showPoint.set_Value(true);
			_showPonder.set_Value(true);
			_showSalute.set_Value(true);
			_showShrug.set_Value(true);
			_showSit.set_Value(true);
			_showSleep.set_Value(true);
			_showSurprised.set_Value(true);
			_showThreaten.set_Value(true);
			_showWave.set_Value(true);
			_showYes.set_Value(true);
			_showCrossarms.set_Value(!value);
			_showCry.set_Value(!value);
			_showFacepalm.set_Value(!value);
			_showSad.set_Value(!value);
			_showTalk.set_Value(!value);
			_showThanks.set_Value(!value);
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)tomeWindow).get_Visible())
			{
				switch (checkPositionSwitch)
				{
				case 0:
					currentPositionA = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
					break;
				case 1:
					currentPositionB = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
					break;
				case 2:
					currentPositionC = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
					break;
				}
				if (checkPositionSwitch >= 2)
				{
					checkPositionSwitch = 0;
				}
				else
				{
					checkPositionSwitch++;
				}
			}
		}

		protected override void Unload()
		{
			foreach (Emote coreEmote in coreEmoteList)
			{
				((Control)coreEmote.getContainer()).remove_Click(coreEmoteClickEvent);
				EmoteContainer container = coreEmote.getContainer();
				if (container != null)
				{
					((Control)container).Dispose();
				}
			}
			foreach (Emote unlockEmote in unlockEmoteList)
			{
				((Control)unlockEmote.getContainer()).remove_Click(unlockEmoteClickEvent);
				EmoteContainer container2 = unlockEmote.getContainer();
				if (container2 != null)
				{
					((Control)container2).Dispose();
				}
			}
			foreach (Emote rankEmote in rankEmoteList)
			{
				((Control)rankEmote.getContainer()).remove_Click(rankEmoteClickEvent);
				EmoteContainer container3 = rankEmote.getContainer();
				if (container3 != null)
				{
					((Control)container3).Dispose();
				}
			}
			StandardWindow obj = tomeWindow;
			if (obj != null)
			{
				((Control)obj).Dispose();
			}
			CornerIcon obj2 = tomeCornerIcon;
			if (obj2 != null)
			{
				((Control)obj2).Dispose();
			}
		}

		private void activateLongLabel(List<Emote> emoteList)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			foreach (Emote emote in emoteList)
			{
				((Container)emote.getContainer()).set_WidthSizingMode((SizingMode)1);
				((Control)emote.getContainer()).set_Size(new Point(size, size + labelSize));
				((Control)emote.getContainer().getLabel()).set_Width(labelWidth);
				((Control)emote.getContainer().getImage()).set_Location(new Point(labelWidth / 2 - size / 2, 0));
				if (emote.getContainer().getCooldownLabel() != null)
				{
					((Control)emote.getContainer().getCooldownLabel()).set_Location(((Control)emote.getContainer().getImage()).get_Location());
				}
			}
			_showEmoteNames.set_Value(true);
		}

		private void deactivateLongLabel(List<Emote> emoteList)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			foreach (Emote emote in emoteList)
			{
				((Container)emote.getContainer()).set_WidthSizingMode((SizingMode)0);
				((Control)emote.getContainer().getLabel()).set_Width(size);
				((Control)emote.getContainer().getImage()).set_Location(new Point(0, 0));
				if (_showEmoteNames.get_Value())
				{
					((Control)emote.getContainer()).set_Size(new Point(size, size + labelSize));
					emote.getContainer().getLabel().set_AutoSizeWidth(false);
				}
				else
				{
					((Control)emote.getContainer()).set_Size(new Point(size, size));
				}
				if (emote.getContainer().getCooldownLabel() != null)
				{
					((Control)emote.getContainer().getCooldownLabel()).set_Location(((Control)emote.getContainer().getImage()).get_Location());
				}
			}
		}

		private void activateNameLabel(List<Emote> emoteList)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			foreach (Emote emote in emoteList)
			{
				((Control)emote.getContainer().getLabel()).set_Visible(true);
				((Control)emote.getContainer()).set_Size(new Point(size, size + labelSize));
			}
		}

		private void deactivateNameLabel(List<Emote> emoteList)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			foreach (Emote emote in emoteList)
			{
				((Control)emote.getContainer().getLabel()).set_Visible(false);
				((Control)emote.getContainer()).set_Size(new Point(size, size));
			}
			_adjustLabelLength.set_Value(false);
		}

		private bool emoteAllowed()
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			if (IsAnyKeyDown())
			{
				ScreenNotification.ShowNotification(BadLocalization.NOEMOTEONKEYPRESSED[language], (NotificationType)0, (Texture2D)null, 4);
				return false;
			}
			if (isPlayerMoving())
			{
				ScreenNotification.ShowNotification(BadLocalization.NOEMOTEWHENMOVING[language], (NotificationType)0, (Texture2D)null, 4);
				return false;
			}
			if (GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat())
			{
				ScreenNotification.ShowNotification(BadLocalization.NOEMOTEINCOMBAT[language], (NotificationType)0, (Texture2D)null, 4);
				return false;
			}
			MountType currentMount = GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount();
			if (!((object)(MountType)(ref currentMount)).ToString().Equals("None"))
			{
				ScreenNotification.ShowNotification(BadLocalization.NOEMOTEONMOUNT[language], (NotificationType)0, (Texture2D)null, 4);
				return false;
			}
			return true;
		}

		private async void activateEmote(string emote, bool targetChecked, bool synchronChecked)
		{
			string chatCommand = "/" + emote;
			if (targetChecked)
			{
				chatCommand += " @";
			}
			if (synchronChecked)
			{
				chatCommand += " *";
			}
			if (!GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
			{
				Keyboard.Stroke((VirtualKeyShort)13, false);
				await Task.Delay(25);
			}
			Keyboard.Press((VirtualKeyShort)17, true);
			Keyboard.Stroke((VirtualKeyShort)65, true);
			await Task.Delay(25);
			Keyboard.Release((VirtualKeyShort)17, true);
			Keyboard.Release((VirtualKeyShort)65, false);
			Keyboard.Release((VirtualKeyShort)68, false);
			new InputSimulator().Keyboard.TextEntry(chatCommand);
			await Task.Delay(50);
			Keyboard.Stroke((VirtualKeyShort)13, false);
		}

		private bool isPlayerMoving()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			if (((Vector3)(ref currentPositionA)).Equals(currentPositionB) && ((Vector3)(ref currentPositionA)).Equals(currentPositionC))
			{
				return false;
			}
			return true;
		}

		public bool IsAnyKeyDown()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			foreach (object v in Enum.GetValues(typeof(Key)))
			{
				if ((Key)v != 0)
				{
					KeyboardState state = Keyboard.GetState();
					if (((KeyboardState)(ref state)).IsKeyDown((Keys)(Key)v))
					{
						return true;
					}
				}
			}
			return false;
		}

		private async Task checkUnlockedEmotesByAPI()
		{
			List<TokenPermission> apiKeyPermissions = new List<TokenPermission>
			{
				(TokenPermission)1,
				(TokenPermission)6,
				(TokenPermission)9,
				(TokenPermission)7
			};
			try
			{
				if (Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)apiKeyPermissions))
				{
					await ((IBlobClient<IApiV2ObjectList<AccountFinisher>>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
						.get_Finishers()).GetAsync(default(CancellationToken));
					PvpStats ranks = await ((IBlobClient<PvpStats>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Pvp()
						.get_Stats()).GetAsync(default(CancellationToken));
					foreach (Emote emote7 in rankEmoteList)
					{
						if (emote7.getChatCode().Equals("rank"))
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 1"))
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 10") && ranks.get_PvpRank() >= 10)
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 20") && ranks.get_PvpRank() >= 20)
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 30") && ranks.get_PvpRank() >= 30)
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 40") && ranks.get_PvpRank() >= 40)
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 50") && ranks.get_PvpRank() >= 50)
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 60") && ranks.get_PvpRank() >= 60)
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 70") && ranks.get_PvpRank() >= 70)
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 80") && ranks.get_PvpRank() >= 80)
						{
							enableRankEmote(emote7);
						}
					}
					List<string> unlockedEmotes = new List<string>((IEnumerable<string>)(await ((IBlobClient<IApiV2ObjectList<string>>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
						.get_Emotes()).GetAsync(default(CancellationToken))));
					unlockedEmotes = unlockedEmotes.ConvertAll((string d) => d.ToLower());
					foreach (Emote emote6 in unlockEmoteList)
					{
						if (emote6.getChatCode().Equals("bless") || emote6.getChatCode().Equals("heroic") || emote6.getChatCode().Equals("hiss") || emote6.getChatCode().Equals("magicjuggle") || emote6.getChatCode().Equals("paper") || emote6.getChatCode().Equals("possessed") || emote6.getChatCode().Equals("readbook") || emote6.getChatCode().Equals("rock") || emote6.getChatCode().Equals("scissors") || emote6.getChatCode().Equals("serve") || emote6.getChatCode().Equals("sipcoffee"))
						{
							((Control)emote6.getContainer()).set_Enabled(true);
							emote6.getContainer().getImage().set_Tint(activatedColor);
							emote6.isDeactivatedByLocked(newBool: false);
						}
						if (unlockedEmotes.Contains(emote6.getChatCode()))
						{
							((Control)emote6.getContainer()).set_Enabled(true);
							emote6.getContainer().getImage().set_Tint(activatedColor);
							emote6.isDeactivatedByLocked(newBool: false);
						}
					}
					return;
				}
				foreach (Emote emote5 in unlockEmoteList)
				{
					enableLockedEmote(emote5);
				}
				foreach (Emote emote4 in rankEmoteList)
				{
					enableLockedEmote(emote4);
				}
			}
			catch (Exception)
			{
				foreach (Emote emote3 in unlockEmoteList)
				{
					enableLockedEmote(emote3);
				}
				foreach (Emote emote2 in rankEmoteList)
				{
					enableLockedEmote(emote2);
				}
			}
			void enableLockedEmote(Emote emote)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				emote.getContainer().getImage().set_Tint(activatedColor);
				((Control)emote.getContainer()).set_Enabled(true);
				emote.isDeactivatedByLocked(newBool: false);
			}
			void enableLockedEmote(Emote emote)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				emote.getContainer().getImage().set_Tint(activatedColor);
				((Control)emote.getContainer()).set_Enabled(true);
				emote.isDeactivatedByLocked(newBool: false);
			}
			void enableRankEmote(Emote emote)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				emote.getContainer().getImage().set_Tint(activatedColor);
				((Control)emote.getContainer()).set_Enabled(true);
				emote.isDeactivatedByLocked(newBool: false);
			}
		}
	}
}
