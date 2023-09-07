using System;
using System.Collections.Generic;
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
			EmoteLibrary library = new EmoteLibrary(ContentsManager);
			coreEmoteList = library.loadCoreEmotes();
			int size = 64;
			List<Image> coreEmoteImages = new List<Image>();
			foreach (Emote emote3 in coreEmoteList)
			{
				if (!emote3.getCategory().Equals(EmoteLibrary.CORECODE))
				{
					continue;
				}
				Image val7 = new Image(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture(emote3.getImagePath())));
				((Control)val7).set_Size(new Point(size, size));
				((Control)val7).set_BasicTooltipText(emote3.getToolTipp()[language]);
				((Control)val7).set_Parent((Container)(object)corePanel);
				Image emoteImage3 = val7;
				coreEmoteClickEvent = delegate
				{
					if (emoteAllowed())
					{
						activateEmote(emote3.getChatCode(), targetCheckbox.get_Checked(), synchronCheckbox.get_Checked());
					}
				};
				((Control)emoteImage3).add_Click(coreEmoteClickEvent);
				coreEmoteImages.Add(emoteImage3);
				emote3.setImg(emoteImage3);
			}
			FlowPanel val8 = new FlowPanel();
			((Panel)val8).set_ShowBorder(true);
			((Panel)val8).set_Title(BadLocalization.UNLOCKABLEPANELTITLE[language]);
			((Control)val8).set_Size(new Point(((Container)mainPanel).get_ContentRegion().Width, ((Container)mainPanel).get_ContentRegion().Height));
			((Control)val8).set_Parent((Container)(object)mainPanel);
			((Panel)val8).set_CanCollapse(true);
			val8.set_FlowDirection((ControlFlowDirection)0);
			((Container)val8).set_HeightSizingMode((SizingMode)1);
			((Container)val8).set_AutoSizePadding(new Point(5, 5));
			val8.set_ControlPadding(new Vector2(5f, 5f));
			val8.set_OuterControlPadding(new Vector2(5f, 5f));
			FlowPanel unlockablePanel = val8;
			unlockEmoteList = library.loadUnlockEmotes();
			List<Image> unlockEmoteImages = new List<Image>();
			foreach (Emote emote2 in unlockEmoteList)
			{
				if (!emote2.getCategory().Equals(EmoteLibrary.UNLOCKCODE))
				{
					continue;
				}
				Image val9 = new Image(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture(emote2.getImagePath())));
				((Control)val9).set_Size(new Point(size, size));
				((Control)val9).set_BasicTooltipText(emote2.getToolTipp()[language]);
				((Control)val9).set_Parent((Container)(object)unlockablePanel);
				val9.set_Tint(lockedColor);
				((Control)val9).set_Enabled(false);
				Image emoteImage2 = val9;
				unlockEmoteClickEvent = delegate
				{
					if (emoteAllowed())
					{
						activateEmote(emote2.getChatCode(), targetCheckbox.get_Checked(), synchronCheckbox.get_Checked());
					}
				};
				((Control)emoteImage2).add_Click(unlockEmoteClickEvent);
				unlockEmoteImages.Add(emoteImage2);
				emote2.setImg(emoteImage2);
				emote2.isDeactivatedByLocked(newBool: true);
			}
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
			rankEmoteList = library.loadRankEmotes();
			List<Image> cooldownEmoteImages = new List<Image>();
			foreach (Emote emote in rankEmoteList)
			{
				if (!emote.getCategory().Equals(EmoteLibrary.RANKCODE))
				{
					continue;
				}
				Image val11 = new Image(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture(emote.getImagePath())));
				((Control)val11).set_Size(new Point(size, size));
				((Control)val11).set_BasicTooltipText(emote.getToolTipp()[language]);
				((Control)val11).set_Parent((Container)(object)rankPanel);
				val11.set_Tint(lockedColor);
				((Control)val11).set_Enabled(false);
				Image emoteImage = val11;
				rankEmoteClickEvent = delegate
				{
					if (emoteAllowed())
					{
						activateEmote(emote.getChatCode(), targetCheckbox.get_Checked(), synchronCheckbox.get_Checked());
						activateCooldown();
					}
				};
				((Control)emoteImage).add_Click(rankEmoteClickEvent);
				cooldownEmoteImages.Add(emoteImage);
				emote.setImg(emoteImage);
				emote.isDeactivatedByLocked(newBool: true);
			}
			Panel val12 = new Panel();
			((Control)val12).set_Size(new Point(((Container)mainPanel).get_ContentRegion().Width, 50));
			((Control)val12).set_Parent((Container)(object)mainPanel);
			Panel spacePanel = val12;
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
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_0109: Unknown result type (might be due to invalid IL or missing references)
				//IL_015c: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
				//IL_021d: Unknown result type (might be due to invalid IL or missing references)
				if (targetCheckbox.get_Checked())
				{
					foreach (Emote current in coreEmoteList)
					{
						if (!current.hasTarget())
						{
							current.getImg().set_Tint(noTargetColor);
							current.isDeactivatedByTargeting(newBool: true);
						}
					}
					foreach (Emote current2 in unlockEmoteList)
					{
						if (!current2.hasTarget() && !current2.isDeactivatedByLocked())
						{
							current2.getImg().set_Tint(noTargetColor);
							current2.isDeactivatedByTargeting(newBool: true);
						}
					}
					foreach (Emote current3 in rankEmoteList)
					{
						if (!current3.hasTarget() && !current3.isDeactivatedByLocked())
						{
							if (!current3.isDeactivatedByCooldown())
							{
								current3.getImg().set_Tint(noTargetColor);
							}
							current3.isDeactivatedByTargeting(newBool: true);
						}
					}
				}
				else
				{
					foreach (Emote coreEmote in coreEmoteList)
					{
						coreEmote.getImg().set_Tint(activatedColor);
						coreEmote.isDeactivatedByTargeting(newBool: false);
					}
					foreach (Emote current5 in unlockEmoteList)
					{
						if (!current5.isDeactivatedByLocked())
						{
							current5.getImg().set_Tint(activatedColor);
						}
						current5.isDeactivatedByTargeting(newBool: false);
					}
					foreach (Emote current6 in rankEmoteList)
					{
						if (!current6.isDeactivatedByCooldown() && !current6.isDeactivatedByLocked())
						{
							current6.getImg().set_Tint(activatedColor);
						}
						current6.isDeactivatedByTargeting(newBool: false);
					}
				}
			});
			((Control)tomeCornerIcon).set_Visible(true);
			void activateCooldown()
			{
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				foreach (Emote emote4 in rankEmoteList)
				{
					if (!emote4.isDeactivatedByLocked())
					{
						emote4.getImg().set_Tint(cooldownColor);
						((Control)emote4.getImg()).set_Enabled(false);
						emote4.isDeactivatedByCooldown(newBool: true);
					}
				}
				System.Timers.Timer aTimer = new System.Timers.Timer();
				aTimer.Elapsed += OnTimedEvent;
				aTimer.Interval = 60000.0;
				aTimer.Enabled = true;
				void OnTimedEvent(object source, ElapsedEventArgs e)
				{
					//IL_004d: Unknown result type (might be due to invalid IL or missing references)
					//IL_006a: Unknown result type (might be due to invalid IL or missing references)
					aTimer.Enabled = false;
					foreach (Emote emote5 in rankEmoteList)
					{
						if (!emote5.isDeactivatedByLocked())
						{
							if (emote5.isDeactivatedByTargeting())
							{
								emote5.getImg().set_Tint(noTargetColor);
							}
							else
							{
								emote5.getImg().set_Tint(activatedColor);
							}
							((Control)emote5.getImg()).set_Enabled(true);
							emote5.isDeactivatedByCooldown(newBool: false);
						}
					}
				}
			}
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
				((Control)coreEmote.getImg()).remove_Click(coreEmoteClickEvent);
				Image img = coreEmote.getImg();
				if (img != null)
				{
					((Control)img).Dispose();
				}
			}
			foreach (Emote unlockEmote in unlockEmoteList)
			{
				((Control)unlockEmote.getImg()).remove_Click(unlockEmoteClickEvent);
				Image img2 = unlockEmote.getImg();
				if (img2 != null)
				{
					((Control)img2).Dispose();
				}
			}
			foreach (Emote rankEmote in rankEmoteList)
			{
				((Control)rankEmote.getImg()).remove_Click(rankEmoteClickEvent);
				Image img3 = rankEmote.getImg();
				if (img3 != null)
				{
					((Control)img3).Dispose();
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

		private void activateEmote(string emote, bool targetChecked, bool synchronChecked)
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
				Thread.Sleep(25);
			}
			Keyboard.Press((VirtualKeyShort)17, true);
			Keyboard.Stroke((VirtualKeyShort)65, true);
			Thread.Sleep(25);
			Keyboard.Release((VirtualKeyShort)17, true);
			Keyboard.Release((VirtualKeyShort)65, false);
			Keyboard.Release((VirtualKeyShort)68, false);
			new InputSimulator().Keyboard.TextEntry(chatCommand);
			Thread.Sleep(50);
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
							((Control)emote6.getImg()).set_Enabled(true);
							emote6.getImg().set_Tint(activatedColor);
							emote6.isDeactivatedByLocked(newBool: false);
						}
						if (unlockedEmotes.Contains(emote6.getChatCode()))
						{
							((Control)emote6.getImg()).set_Enabled(true);
							emote6.getImg().set_Tint(activatedColor);
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
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				emote.getImg().set_Tint(activatedColor);
				((Control)emote.getImg()).set_Enabled(true);
				emote.isDeactivatedByLocked(newBool: false);
			}
			void enableLockedEmote(Emote emote)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				emote.getImg().set_Tint(activatedColor);
				((Control)emote.getImg()).set_Enabled(true);
				emote.isDeactivatedByLocked(newBool: false);
			}
			void enableRankEmote(Emote emote)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				emote.getImg().set_Tint(activatedColor);
				((Control)emote.getImg()).set_Enabled(true);
				emote.isDeactivatedByLocked(newBool: false);
			}
		}
	}
}
