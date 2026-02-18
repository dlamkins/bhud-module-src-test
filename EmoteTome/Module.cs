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

		public Checkbox targetCheckbox;

		public Checkbox synchronCheckbox;

		public int language = BadLocalization.ENGLISH;

		private Vector3 currentPositionA;

		private Vector3 currentPositionB;

		private Vector3 currentPositionC;

		private int checkPositionSwitch;

		private List<Emote> coreEmoteList = new List<Emote>();

		private List<EmoteContainer> coreEmoteContainers = new List<EmoteContainer>();

		private List<Emote> unlockEmoteList = new List<Emote>();

		private List<EmoteContainer> unlockEmoteContainers = new List<EmoteContainer>();

		private List<Emote> rankEmoteList = new List<Emote>();

		private List<EmoteContainer> cooldownEmoteContainers = new List<EmoteContainer>();

		private Color activatedColor = new Color(250, 250, 250);

		private Color lockedColor = new Color(30, 30, 30);

		private Color noTargetColor = new Color(130, 130, 130);

		private Color cooldownColor = new Color(50, 50, 50);

		private bool checkedAPIForUnlock;

		private EventHandler<MouseEventArgs> emoteClickEvent = delegate
		{
		};

		private SettingEntry<bool> _showEmoteNames;

		private SettingEntry<bool> _adjustLabelLength;

		private SettingEntry<bool> _halloweenMode;

		private SettingEntry<bool> _checkForKeyPress;

		private SettingEntry<bool> _checkForMovement;

		private SettingEntry<List<string>> _favorites;

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

		private SettingEntry<bool> _showUnleash;

		private SettingEntry<bool> _showPetalthrow;

		private SettingEntry<bool> _showBreakdance;

		private SettingEntry<bool> _showBoogie;

		private SettingEntry<bool> _showPoseCover;

		private SettingEntry<bool> _showPoseHigh;

		private SettingEntry<bool> _showPoseLow;

		private SettingEntry<bool> _showPoseTwist;

		private SettingEntry<bool> _showBlowKiss;

		private SettingEntry<bool> _showMagicTrick;

		private SettingEntry<bool> _showChannel;

		private SettingEntry<bool> _showBarbecue;

		private SettingEntry<bool> _showDrink;

		private SettingEntry<bool> _showCrabDance;

		private SettingEntry<bool> _showShocked;

		private SettingEntry<bool> _showThumbsUp;

		private SettingEntry<bool> _showThumbsDown;

		private SettingEntry<bool> _showPoseHeart;

		private SettingEntry<bool> _showPosePeace;

		private SettingEntry<bool> _showPoseSassy;

		private SettingEntry<bool> _showPoseShy;

		private SettingEntry<bool> _showHappyDance;

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

		private SettingEntry<bool> _hideCoreEmotes;

		private SettingEntry<bool> _hideUnlockEmotes;

		private SettingEntry<bool> _hideRankEmotes;

		private int size = 64;

		private int labelSize = 16;

		private int labelWidth = 120;

		private FavoriteBar favoriteBar;

		private List<string> unlockedEmotes = new List<string>();

		private static readonly Logger Logger = Logger.GetLogger<Module>();

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)


		protected override void DefineSettings(SettingCollection settings)
		{
			_hideCoreEmotes = settings.DefineSetting<bool>("Hide Core Emote Panel", false, (Func<string>)(() => BadLocalization.HIDECOREEMOTE[language]), (Func<string>)(() => BadLocalization.HIDECOREEMOTE[language]));
			_hideUnlockEmotes = settings.DefineSetting<bool>("Hide Unlockable Emote Panel", false, (Func<string>)(() => BadLocalization.HIDEUNLOCKEMOTE[language]), (Func<string>)(() => BadLocalization.HIDEUNLOCKEMOTE[language]));
			_hideRankEmotes = settings.DefineSetting<bool>("Hide Rank Emote Panel", false, (Func<string>)(() => BadLocalization.HIDERANKEMOTE[language]), (Func<string>)(() => BadLocalization.HIDERANKEMOTE[language]));
			_checkForKeyPress = settings.DefineSetting<bool>("Check for Key Press", true, (Func<string>)(() => BadLocalization.CHECKKEY[language]), (Func<string>)(() => BadLocalization.CHECKKEYTEXT[language]));
			_checkForMovement = settings.DefineSetting<bool>("Check for Movement", true, (Func<string>)(() => BadLocalization.CHECKMOVE[language]), (Func<string>)(() => BadLocalization.CHECKMOVETEXT[language]));
			_showEmoteNames = settings.DefineSetting<bool>("Show Names", false, (Func<string>)(() => BadLocalization.SHOWNAMES[language]), (Func<string>)(() => BadLocalization.SHOWNAMESTEXT[language]));
			_adjustLabelLength = settings.DefineSetting<bool>("Larger Name Labels", false, (Func<string>)(() => BadLocalization.LARGERNAMELABELS[language]), (Func<string>)(() => BadLocalization.LARGERNAMELABELSTEXT[language]));
			_halloweenMode = settings.DefineSetting<bool>("Halloween Mode", false, (Func<string>)(() => BadLocalization.HALLOWEENMODE[language]), (Func<string>)(() => BadLocalization.HALLOWEENMODETEXT[language]));
			_favorites = settings.DefineSetting<List<string>>("favorite_emotes", new List<string>(), (Func<string>)(() => "Favorite Emotes"), (Func<string>)(() => "Liste der favorisierten Emotes"));
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
			_showUnleash = settings.DefineSetting<bool>("Show Unleash", true, (Func<string>)(() => BadLocalization.UNLEASH[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showPetalthrow = settings.DefineSetting<bool>("Show Petalthrow", true, (Func<string>)(() => BadLocalization.PETALTHROW[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showBreakdance = settings.DefineSetting<bool>("Show Breakdance", true, (Func<string>)(() => BadLocalization.BREAKDANCE[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showBoogie = settings.DefineSetting<bool>("Show Boogie", true, (Func<string>)(() => BadLocalization.BOOGIE[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showPoseCover = settings.DefineSetting<bool>("Show PoseCover", true, (Func<string>)(() => BadLocalization.POSECOVER[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showPoseHigh = settings.DefineSetting<bool>("Show PoseHigh", true, (Func<string>)(() => BadLocalization.POSEHIGH[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showPoseLow = settings.DefineSetting<bool>("Show PoseLow", true, (Func<string>)(() => BadLocalization.POSELOW[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showPoseTwist = settings.DefineSetting<bool>("Show PoseTwist", true, (Func<string>)(() => BadLocalization.POSETWIST[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showBlowKiss = settings.DefineSetting<bool>("Show BlowKiss", true, (Func<string>)(() => BadLocalization.BLOWKISS[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showMagicTrick = settings.DefineSetting<bool>("Show MagicTrick", true, (Func<string>)(() => BadLocalization.MAGICTRICK[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showChannel = settings.DefineSetting<bool>("Show Channel", true, (Func<string>)(() => BadLocalization.CHANNEL[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showBarbecue = settings.DefineSetting<bool>("Show Barbecue", true, (Func<string>)(() => BadLocalization.BARBECUE[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showDrink = settings.DefineSetting<bool>("Show Drink", true, (Func<string>)(() => BadLocalization.DRINK[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showCrabDance = settings.DefineSetting<bool>("Show CrabDance", true, (Func<string>)(() => BadLocalization.CRABDANCE[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showShocked = settings.DefineSetting<bool>("Show Shocked", true, (Func<string>)(() => BadLocalization.SHOCKED[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showThumbsUp = settings.DefineSetting<bool>("Show ThumbsUp", true, (Func<string>)(() => BadLocalization.THUMBSUP[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showThumbsDown = settings.DefineSetting<bool>("Show ThumbsDown", true, (Func<string>)(() => BadLocalization.POSEHEART[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showPoseHeart = settings.DefineSetting<bool>("Show PoseHeart", true, (Func<string>)(() => BadLocalization.POSEHEART[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showPosePeace = settings.DefineSetting<bool>("Show PosePeace", true, (Func<string>)(() => BadLocalization.POSEPEACE[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showPoseSassy = settings.DefineSetting<bool>("Show PoseSassy", true, (Func<string>)(() => BadLocalization.POSESASSY[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showPoseShy = settings.DefineSetting<bool>("Show PoseShy", true, (Func<string>)(() => BadLocalization.POSESHY[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
			_showHappyDance = settings.DefineSetting<bool>("Show HappyDance", true, (Func<string>)(() => BadLocalization.HAPPYDANCE[language]), (Func<string>)(() => BadLocalization.EMOTETEXT[language]));
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
			val.set_Priority(61747774);
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
			Module module3 = this;
			Checkbox val3 = new Checkbox();
			val3.set_Text(BadLocalization.TARGETCHECKBOXTEXT[language]);
			((Control)val3).set_Location(new Point(0, 0));
			((Control)val3).set_BasicTooltipText(BadLocalization.TARGETCHECKBOXTOOLTIP[language]);
			((Control)val3).set_Parent((Container)(object)tomeWindow);
			module3.targetCheckbox = val3;
			Module module4 = this;
			Checkbox val4 = new Checkbox();
			val4.set_Text(BadLocalization.SYNCHRONCHECKBOXTEXT[language]);
			((Control)val4).set_Location(new Point(0, 20));
			((Control)val4).set_BasicTooltipText(BadLocalization.SYNCHRONCHECKBOXTOOLTIP[language]);
			((Control)val4).set_Parent((Container)(object)tomeWindow);
			module4.synchronCheckbox = val4;
			Checkbox val5 = new Checkbox();
			val5.set_Text(BadLocalization.HALLOWEENMODE[language]);
			((Control)val5).set_Location(new Point(200, 0));
			((Control)val5).set_BasicTooltipText(BadLocalization.HALLOWEENMODETEXT[language]);
			((Control)val5).set_Parent((Container)(object)tomeWindow);
			val5.set_Checked(_halloweenMode.get_Value());
			Checkbox halloweenCheckbox = val5;
			StandardButton val6 = new StandardButton();
			val6.set_Text(BadLocalization.FAVORITESBAR[language]);
			((Control)val6).set_Location(new Point(200, 20));
			((Control)val6).set_Parent((Container)(object)tomeWindow);
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (favoriteBar != null)
				{
					((WindowBase2)favoriteBar).ToggleWindow();
				}
			});
			FlowPanel val7 = new FlowPanel();
			((Control)val7).set_Size(new Point(((Container)tomeWindow).get_ContentRegion().Width, ((Container)tomeWindow).get_ContentRegion().Height));
			((Control)val7).set_Location(new Point(0, 50));
			val7.set_FlowDirection((ControlFlowDirection)3);
			((Control)val7).set_Parent((Container)(object)tomeWindow);
			((Panel)val7).set_CanScroll(true);
			((Control)val7).set_Padding(new Thickness(20f));
			FlowPanel mainPanel = val7;
			EmoteLibrary library = new EmoteLibrary(ContentsManager);
			coreEmoteList = library.loadCoreEmotes();
			coreEmoteContainers = new List<EmoteContainer>();
			FlowPanel val8 = new FlowPanel();
			((Panel)val8).set_ShowBorder(true);
			((Panel)val8).set_Title(BadLocalization.COREPANELTITLE[language]);
			((Control)val8).set_Size(new Point(((Container)mainPanel).get_ContentRegion().Width, ((Container)mainPanel).get_ContentRegion().Height));
			((Control)val8).set_Parent((Container)(object)mainPanel);
			((Panel)val8).set_CanCollapse(true);
			val8.set_FlowDirection((ControlFlowDirection)0);
			((Container)val8).set_HeightSizingMode((SizingMode)1);
			((Container)val8).set_AutoSizePadding(new Point(5, 5));
			val8.set_ControlPadding(new Vector2(5f, 5f));
			val8.set_OuterControlPadding(new Vector2(5f, 5f));
			FlowPanel corePanel = val8;
			if (_hideCoreEmotes.get_Value())
			{
				((Control)corePanel).set_Visible(false);
			}
			createEmoteContainer(coreEmoteList, (Panel)(object)corePanel, "core", fav: false);
			unlockEmoteList = library.loadUnlockEmotes();
			unlockEmoteContainers = new List<EmoteContainer>();
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
			if (_hideUnlockEmotes.get_Value())
			{
				((Control)unlockablePanel).set_Visible(false);
			}
			createEmoteContainer(unlockEmoteList, (Panel)(object)unlockablePanel, "unlock", fav: false);
			rankEmoteList = library.loadRankEmotes();
			cooldownEmoteContainers = new List<EmoteContainer>();
			FlowPanel val10 = new FlowPanel();
			((Panel)val10).set_ShowBorder(true);
			((Panel)val10).set_Title(BadLocalization.RANKPANELTITLE[language]);
			((Control)val10).set_Size(new Point(((Container)mainPanel).get_ContentRegion().Width, ((Container)mainPanel).get_ContentRegion().Height));
			((Control)val10).set_Parent((Container)(object)mainPanel);
			((Panel)val10).set_CanCollapse(true);
			val10.set_FlowDirection((ControlFlowDirection)0);
			((Container)val10).set_HeightSizingMode((SizingMode)1);
			((Container)val10).set_AutoSizePadding(new Point(5, 5));
			val10.set_ControlPadding(new Vector2(5f, 5f));
			val10.set_OuterControlPadding(new Vector2(5f, 5f));
			FlowPanel rankPanel = val10;
			if (_hideRankEmotes.get_Value())
			{
				((Control)rankPanel).set_Visible(false);
			}
			createEmoteContainer(rankEmoteList, (Panel)(object)rankPanel, "rank", fav: false);
			favoriteBar = new FavoriteBar(this, ContentsManager, coreEmoteList, unlockEmoteList, rankEmoteList);
			Panel val11 = new Panel();
			((Control)val11).set_Size(new Point(((Container)mainPanel).get_ContentRegion().Width, 50));
			((Control)val11).set_Parent((Container)(object)mainPanel);
			Panel spacePanel = val11;
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
				int count2 = Math.Min(coreSettingList.Count, coreEmoteList.Count);
				for (int k = 0; k < count2; k++)
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
				ScreenNotification.ShowNotification("Emote Tome: Some Error occured on loading core emotes.", (NotificationType)0, (Texture2D)null, 4);
			}
			((Control)mainPanel).RecalculateLayout();
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
			unlockSettingList.Add(_showUnleash);
			unlockSettingList.Add(_showPetalthrow);
			unlockSettingList.Add(_showBreakdance);
			unlockSettingList.Add(_showBoogie);
			unlockSettingList.Add(_showPoseCover);
			unlockSettingList.Add(_showPoseHigh);
			unlockSettingList.Add(_showPoseLow);
			unlockSettingList.Add(_showPoseTwist);
			unlockSettingList.Add(_showBlowKiss);
			unlockSettingList.Add(_showMagicTrick);
			unlockSettingList.Add(_showChannel);
			unlockSettingList.Add(_showBarbecue);
			unlockSettingList.Add(_showDrink);
			unlockSettingList.Add(_showCrabDance);
			unlockSettingList.Add(_showShocked);
			unlockSettingList.Add(_showThumbsUp);
			unlockSettingList.Add(_showThumbsDown);
			unlockSettingList.Add(_showPoseHeart);
			unlockSettingList.Add(_showPosePeace);
			unlockSettingList.Add(_showPoseSassy);
			unlockSettingList.Add(_showPoseShy);
			unlockSettingList.Add(_showHappyDance);
			try
			{
				int count = Math.Min(unlockEmoteList.Count, unlockSettingList.Count);
				for (int j = 0; j < count; j++)
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
				ScreenNotification.ShowNotification("Emote Tome: Some Error occured on loading unlockable emotes.", (NotificationType)0, (Texture2D)null, 4);
			}
			((Control)mainPanel).RecalculateLayout();
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
				Math.Min(rankEmoteList.Count, rankSettingList.Count);
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
				ScreenNotification.ShowNotification("Emote Tome: Some Error occured on loading rank emotes.", (NotificationType)0, (Texture2D)null, 4);
			}
			((Control)mainPanel).RecalculateLayout();
			((SettingEntry)_hideCoreEmotes).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				if (_hideCoreEmotes.get_Value())
				{
					((Control)corePanel).set_Visible(false);
					((Control)mainPanel).RecalculateLayout();
				}
				else
				{
					((Control)corePanel).set_Visible(true);
					((Control)mainPanel).RecalculateLayout();
				}
			});
			((SettingEntry)_hideUnlockEmotes).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				if (_hideUnlockEmotes.get_Value())
				{
					((Control)unlockablePanel).set_Visible(false);
					((Control)mainPanel).RecalculateLayout();
				}
				else
				{
					((Control)unlockablePanel).set_Visible(true);
					((Control)mainPanel).RecalculateLayout();
				}
			});
			((SettingEntry)_hideRankEmotes).add_PropertyChanged((PropertyChangedEventHandler)delegate
			{
				if (_hideRankEmotes.get_Value())
				{
					((Control)rankPanel).set_Visible(false);
					((Control)mainPanel).RecalculateLayout();
				}
				else
				{
					((Control)rankPanel).set_Visible(true);
					((Control)mainPanel).RecalculateLayout();
				}
			});
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
					halloweenCheckbox.set_Checked(true);
				}
				else
				{
					halloweenMode(_halloweenMode, value: false);
					halloweenCheckbox.set_Checked(false);
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
				setTargetCheckbox();
				favoriteBar.targetCheckbox.set_Checked(targetCheckbox.get_Checked());
			});
			synchronCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				favoriteBar.synchronCheckbox.set_Checked(synchronCheckbox.get_Checked());
			});
			halloweenCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (halloweenCheckbox.get_Checked())
				{
					_halloweenMode.set_Value(true);
				}
				else
				{
					_halloweenMode.set_Value(false);
				}
			});
			((Control)tomeCornerIcon).set_Visible(true);
		}

		public void setTargetCheckbox()
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			if (targetCheckbox.get_Checked())
			{
				foreach (Emote emote6 in coreEmoteList)
				{
					if (!emote6.hasTarget())
					{
						emote6.getContainer().getImage().set_Tint(noTargetColor);
						if (emote6.getFavContainer() != null)
						{
							emote6.getFavContainer().getImage().set_Tint(noTargetColor);
						}
						emote6.isDeactivatedByTargeting(newBool: true);
					}
				}
				foreach (Emote emote5 in unlockEmoteList)
				{
					if (!emote5.hasTarget() && !emote5.isDeactivatedByLocked())
					{
						emote5.getContainer().getImage().set_Tint(noTargetColor);
						if (emote5.getFavContainer() != null)
						{
							emote5.getFavContainer().getImage().set_Tint(noTargetColor);
						}
						emote5.isDeactivatedByTargeting(newBool: true);
					}
				}
				foreach (Emote emote4 in rankEmoteList)
				{
					if (emote4.hasTarget() || emote4.isDeactivatedByLocked())
					{
						continue;
					}
					if (!emote4.isDeactivatedByCooldown())
					{
						emote4.getContainer().getImage().set_Tint(noTargetColor);
						if (emote4.getFavContainer() != null)
						{
							emote4.getFavContainer().getImage().set_Tint(noTargetColor);
						}
					}
					emote4.isDeactivatedByTargeting(newBool: true);
				}
				return;
			}
			foreach (Emote emote3 in coreEmoteList)
			{
				emote3.getContainer().getImage().set_Tint(activatedColor);
				if (emote3.getFavContainer() != null)
				{
					emote3.getFavContainer().getImage().set_Tint(activatedColor);
				}
				emote3.isDeactivatedByTargeting(newBool: false);
			}
			foreach (Emote emote2 in unlockEmoteList)
			{
				if (!emote2.isDeactivatedByLocked())
				{
					emote2.getContainer().getImage().set_Tint(activatedColor);
					if (emote2.getFavContainer() != null)
					{
						emote2.getFavContainer().getImage().set_Tint(activatedColor);
					}
				}
				emote2.isDeactivatedByTargeting(newBool: false);
			}
			foreach (Emote emote in rankEmoteList)
			{
				if (!emote.isDeactivatedByCooldown() && !emote.isDeactivatedByLocked())
				{
					emote.getContainer().getImage().set_Tint(activatedColor);
					if (emote.getFavContainer() != null)
					{
						emote.getFavContainer().getImage().set_Tint(activatedColor);
					}
				}
				emote.isDeactivatedByTargeting(newBool: false);
			}
		}

		public void activateCooldown()
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			int cooldown = 60;
			foreach (Emote emote in rankEmoteList)
			{
				if (!emote.isDeactivatedByLocked())
				{
					emote.getContainer().getImage().set_Tint(cooldownColor);
					((Control)emote.getContainer()).set_Enabled(false);
					emote.isDeactivatedByCooldown(newBool: true);
					emote.getContainer().getCooldownLabel().set_Text(cooldown.ToString());
					((Control)emote.getContainer().getCooldownLabel()).set_Visible(true);
					if (emote.getFavContainer() != null)
					{
						emote.getFavContainer().getImage().set_Tint(cooldownColor);
						((Control)emote.getFavContainer()).set_Enabled(false);
						emote.getFavContainer().getCooldownLabel().set_Text(cooldown.ToString());
						((Control)emote.getFavContainer().getCooldownLabel()).set_Visible(true);
					}
				}
			}
			System.Timers.Timer aTimer = new System.Timers.Timer();
			aTimer.Elapsed += OnTimedEvent;
			aTimer.Interval = 1000.0;
			aTimer.Enabled = true;
			void OnTimedEvent(object source, ElapsedEventArgs e)
			{
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0130: Unknown result type (might be due to invalid IL or missing references)
				//IL_014d: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
				if (cooldown >= 1)
				{
					cooldown--;
					foreach (Emote emote3 in rankEmoteList)
					{
						emote3.getContainer().getCooldownLabel().set_Text(cooldown.ToString());
						if (emote3.getFavContainer() != null)
						{
							emote3.getFavContainer().getImage().set_Tint(cooldownColor);
							((Control)emote3.getFavContainer()).set_Enabled(false);
							emote3.getFavContainer().getCooldownLabel().set_Text(cooldown.ToString());
							((Control)emote3.getFavContainer().getCooldownLabel()).set_Visible(true);
						}
					}
				}
				else
				{
					cooldown = 60;
					aTimer.Enabled = false;
					foreach (Emote emote2 in rankEmoteList)
					{
						((Control)emote2.getContainer().getCooldownLabel()).set_Visible(false);
						if (!emote2.isDeactivatedByLocked())
						{
							if (emote2.isDeactivatedByTargeting())
							{
								emote2.getContainer().getImage().set_Tint(noTargetColor);
							}
							else
							{
								emote2.getContainer().getImage().set_Tint(activatedColor);
							}
							((Control)emote2.getContainer()).set_Enabled(true);
							emote2.isDeactivatedByCooldown(newBool: false);
						}
						if (emote2.getFavContainer() != null)
						{
							((Control)emote2.getFavContainer().getCooldownLabel()).set_Visible(false);
							if (!emote2.isDeactivatedByLocked())
							{
								if (emote2.isDeactivatedByTargeting())
								{
									emote2.getFavContainer().getImage().set_Tint(noTargetColor);
								}
								else
								{
									emote2.getFavContainer().getImage().set_Tint(activatedColor);
								}
								((Control)emote2.getFavContainer()).set_Enabled(true);
								emote2.isDeactivatedByCooldown(newBool: false);
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

		public void addToFavoriteSetting(string favoriteEntry)
		{
			_favorites.get_Value().Add(favoriteEntry);
		}

		public void removeFromFavoriteSetting(string favoriteEntry)
		{
			_favorites.get_Value().Remove(favoriteEntry);
		}

		public List<string> getFavoriteList()
		{
			return _favorites.get_Value();
		}

		public bool getShowEmoteNames()
		{
			return _showEmoteNames.get_Value();
		}

		public bool getAdjustLabelLength()
		{
			return _adjustLabelLength.get_Value();
		}

		public bool getTargetChecked()
		{
			return targetCheckbox.get_Checked();
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
			favoriteBar?.unload();
			FavoriteBar obj = favoriteBar;
			if (obj != null)
			{
				((Control)obj).Dispose();
			}
			foreach (Emote coreEmote in coreEmoteList)
			{
				((Control)coreEmote.getContainer()).remove_Click(emoteClickEvent);
				EmoteContainer container = coreEmote.getContainer();
				if (container != null)
				{
					((Control)container).Dispose();
				}
				((Control)coreEmote.getFavContainer()).remove_Click(emoteClickEvent);
				EmoteContainer favContainer = coreEmote.getFavContainer();
				if (favContainer != null)
				{
					((Control)favContainer).Dispose();
				}
			}
			foreach (Emote unlockEmote in unlockEmoteList)
			{
				((Control)unlockEmote.getContainer()).remove_Click(emoteClickEvent);
				EmoteContainer container2 = unlockEmote.getContainer();
				if (container2 != null)
				{
					((Control)container2).Dispose();
				}
				((Control)unlockEmote.getFavContainer()).remove_Click(emoteClickEvent);
				EmoteContainer favContainer2 = unlockEmote.getFavContainer();
				if (favContainer2 != null)
				{
					((Control)favContainer2).Dispose();
				}
			}
			foreach (Emote rankEmote in rankEmoteList)
			{
				((Control)rankEmote.getContainer()).remove_Click(emoteClickEvent);
				EmoteContainer container3 = rankEmote.getContainer();
				if (container3 != null)
				{
					((Control)container3).Dispose();
				}
				((Control)rankEmote.getFavContainer()).remove_Click(emoteClickEvent);
				EmoteContainer favContainer3 = rankEmote.getFavContainer();
				if (favContainer3 != null)
				{
					((Control)favContainer3).Dispose();
				}
			}
			StandardWindow obj2 = tomeWindow;
			if (obj2 != null)
			{
				((Control)obj2).Dispose();
			}
			CornerIcon obj3 = tomeCornerIcon;
			if (obj3 != null)
			{
				((Control)obj3).Dispose();
			}
		}

		public void activateLongLabel(List<Emote> emoteList)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
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
				if (emote.getFavContainer() != null)
				{
					((Container)emote.getFavContainer()).set_WidthSizingMode((SizingMode)1);
					((Control)emote.getFavContainer()).set_Size(new Point(size, size + labelSize));
					((Control)emote.getFavContainer().getLabel()).set_Width(labelWidth);
					((Control)emote.getFavContainer().getImage()).set_Location(new Point(labelWidth / 2 - size / 2, 0));
					if (emote.getFavContainer().getCooldownLabel() != null)
					{
						((Control)emote.getFavContainer().getCooldownLabel()).set_Location(((Control)emote.getFavContainer().getImage()).get_Location());
					}
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
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
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
				if (emote.getFavContainer() != null)
				{
					((Container)emote.getFavContainer()).set_WidthSizingMode((SizingMode)0);
					((Control)emote.getFavContainer().getLabel()).set_Width(size);
					((Control)emote.getFavContainer().getImage()).set_Location(new Point(0, 0));
					if (_showEmoteNames.get_Value())
					{
						((Control)emote.getFavContainer()).set_Size(new Point(size, size + labelSize));
						emote.getFavContainer().getLabel().set_AutoSizeWidth(false);
					}
					else
					{
						((Control)emote.getFavContainer()).set_Size(new Point(size, size));
					}
					if (emote.getFavContainer().getCooldownLabel() != null)
					{
						((Control)emote.getFavContainer().getCooldownLabel()).set_Location(((Control)emote.getFavContainer().getImage()).get_Location());
					}
				}
			}
		}

		public void activateNameLabel(List<Emote> emoteList)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			foreach (Emote emote in emoteList)
			{
				((Control)emote.getContainer().getLabel()).set_Visible(true);
				((Control)emote.getContainer()).set_Size(new Point(size, size + labelSize));
				if (emote.getFavContainer() != null)
				{
					((Control)emote.getFavContainer().getLabel()).set_Visible(true);
					((Control)emote.getFavContainer()).set_Size(new Point(size, size + labelSize));
				}
			}
		}

		private void deactivateNameLabel(List<Emote> emoteList)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			foreach (Emote emote in emoteList)
			{
				((Control)emote.getContainer().getLabel()).set_Visible(false);
				((Control)emote.getContainer()).set_Size(new Point(size, size));
				if (emote.getFavContainer() != null)
				{
					((Control)emote.getFavContainer().getLabel()).set_Visible(false);
					((Control)emote.getFavContainer()).set_Size(new Point(size, size));
				}
			}
			_adjustLabelLength.set_Value(false);
		}

		public bool emoteAllowed()
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

		public async void activateEmote(string emote, bool targetChecked, bool synchronChecked)
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
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			if (_checkForMovement.get_Value())
			{
				if (((Vector3)(ref currentPositionA)).Equals(currentPositionB) && ((Vector3)(ref currentPositionA)).Equals(currentPositionC))
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public bool IsAnyKeyDown()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (_checkForKeyPress.get_Value())
			{
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
					unlockedEmotes = new List<string>((IEnumerable<string>)(await ((IBlobClient<IApiV2ObjectList<string>>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Account()
						.get_Emotes()).GetAsync(default(CancellationToken))));
					unlockedEmotes = unlockedEmotes.ConvertAll((string d) => d.ToLower());
					foreach (Emote emote6 in unlockEmoteList)
					{
						if (emote6.getChatCode().Equals("hiss") || emote6.getChatCode().Equals("magicjuggle") || emote6.getChatCode().Equals("readbook") || emote6.getChatCode().Equals("serve") || emote6.getChatCode().Equals("sipcoffee") || emote6.getChatCode().Equals("unleash") || emote6.getChatCode().Equals("petalthrow") || emote6.getChatCode().Equals("breakdance") || emote6.getChatCode().Equals("boogie") || emote6.getChatCode().Equals("posecover") || emote6.getChatCode().Equals("posehigh") || emote6.getChatCode().Equals("poselow") || emote6.getChatCode().Equals("posetwist") || emote6.getChatCode().Equals("blowkiss") || emote6.getChatCode().Equals("magictrick") || emote6.getChatCode().Equals("channel") || emote6.getChatCode().Equals("barbecue") || emote6.getChatCode().Equals("drink") || emote6.getChatCode().Equals("crabdance") || emote6.getChatCode().Equals("shocked") || emote6.getChatCode().Equals("thumbsup") || emote6.getChatCode().Equals("thumbsdown") || emote6.getChatCode().Equals("poseheart") || emote6.getChatCode().Equals("posepeace") || emote6.getChatCode().Equals("posesassy") || emote6.getChatCode().Equals("poseshy") || emote6.getChatCode().Equals("happydance"))
						{
							((Control)emote6.getContainer()).set_Enabled(true);
							emote6.getContainer().getImage().set_Tint(activatedColor);
							if (emote6.getFavContainer() != null)
							{
								((Control)emote6.getFavContainer()).set_Enabled(true);
								emote6.getFavContainer().getImage().set_Tint(activatedColor);
							}
							emote6.isDeactivatedByLocked(newBool: false);
						}
						if (unlockedEmotes.Contains(emote6.getChatCode()))
						{
							((Control)emote6.getContainer()).set_Enabled(true);
							emote6.getContainer().getImage().set_Tint(activatedColor);
							if (emote6.getFavContainer() != null)
							{
								((Control)emote6.getFavContainer()).set_Enabled(true);
								emote6.getFavContainer().getImage().set_Tint(activatedColor);
							}
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
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				emote.getContainer().getImage().set_Tint(activatedColor);
				((Control)emote.getContainer()).set_Enabled(true);
				if (emote.getFavContainer() != null)
				{
					emote.getFavContainer().getImage().set_Tint(activatedColor);
					((Control)emote.getFavContainer()).set_Enabled(true);
				}
				emote.isDeactivatedByLocked(newBool: false);
			}
			void enableLockedEmote(Emote emote)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				emote.getContainer().getImage().set_Tint(activatedColor);
				((Control)emote.getContainer()).set_Enabled(true);
				if (emote.getFavContainer() != null)
				{
					emote.getFavContainer().getImage().set_Tint(activatedColor);
					((Control)emote.getFavContainer()).set_Enabled(true);
				}
				emote.isDeactivatedByLocked(newBool: false);
			}
			void enableRankEmote(Emote emote)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				emote.getContainer().getImage().set_Tint(activatedColor);
				((Control)emote.getContainer()).set_Enabled(true);
				if (emote.getFavContainer() != null)
				{
					emote.getFavContainer().getImage().set_Tint(activatedColor);
					((Control)emote.getFavContainer()).set_Enabled(true);
				}
				emote.isDeactivatedByLocked(newBool: false);
			}
		}

		public void createEmoteContainer(List<Emote> emoteList, Panel panel, string category, bool fav)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Expected O, but got Unknown
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Expected O, but got Unknown
			foreach (Emote emote in emoteList)
			{
				EmoteContainer emoteContainer2 = new EmoteContainer();
				((Control)emoteContainer2).set_Size(new Point(size, size));
				((Control)emoteContainer2).set_BasicTooltipText(emote.getToolTipp()[language]);
				((Control)emoteContainer2).set_Parent((Container)(object)panel);
				EmoteContainer emoteContainer = emoteContainer2;
				Image val = new Image(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture(emote.getImagePath())));
				((Control)val).set_Size(new Point(size, size));
				((Control)val).set_BasicTooltipText(emote.getToolTipp()[language]);
				((Control)val).set_ZIndex(1);
				((Control)val).set_Parent((Container)(object)emoteContainer);
				Image emoteImage = val;
				Label val2 = new Label();
				val2.set_Text(emote.getToolTipp()[language]);
				val2.set_HorizontalAlignment((HorizontalAlignment)1);
				((Control)val2).set_Size(new Point(size, labelSize));
				((Control)val2).set_ZIndex(2);
				((Control)val2).set_Parent((Container)(object)emoteContainer);
				val2.set_AutoSizeWidth(false);
				((Control)val2).set_Visible(false);
				((Control)val2).set_BackgroundColor(Color.get_Black());
				((Control)val2).set_Location(new Point(0, size - 1));
				Label emoteLabel = val2;
				if (emote.getCategory().Equals("rank"))
				{
					Label val3 = new Label();
					val3.set_Text("60");
					val3.set_HorizontalAlignment((HorizontalAlignment)1);
					((Control)val3).set_Size(new Point(size, size));
					((Control)val3).set_ZIndex(3);
					val3.set_Font(GameService.Content.get_DefaultFont32());
					((Control)val3).set_Parent((Container)(object)emoteContainer);
					val3.set_AutoSizeWidth(false);
					((Control)val3).set_Visible(false);
					Label cooldownLabel = val3;
					emoteContainer.setCooldownLabel(cooldownLabel);
				}
				emoteContainer.setImage(emoteImage);
				emoteContainer.setLabel(emoteLabel);
				emoteClickEvent = delegate
				{
					if (emoteAllowed())
					{
						activateEmote(emote.getChatCode(), targetCheckbox.get_Checked(), synchronCheckbox.get_Checked());
						if (emote.getCategory().Equals("rank"))
						{
							activateCooldown();
						}
					}
				};
				((Control)emoteContainer).add_Click(emoteClickEvent);
				switch (category)
				{
				case "core":
					coreEmoteContainers.Add(emoteContainer);
					break;
				case "unlock":
					unlockEmoteContainers.Add(emoteContainer);
					break;
				case "rank":
					cooldownEmoteContainers.Add(emoteContainer);
					break;
				}
				if (!fav)
				{
					emote.setContainer(emoteContainer);
					continue;
				}
				((Control)emoteContainer).set_Visible(false);
				emote.setFavContainer(emoteContainer);
			}
		}
	}
}
