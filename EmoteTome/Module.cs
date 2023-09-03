using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using WindowsInput;

namespace EmoteTome
{
	[Export(typeof(Blish_HUD.Modules.Module))]
	public class Module : Blish_HUD.Modules.Module
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

		private Microsoft.Xna.Framework.Color activatedColor = new Microsoft.Xna.Framework.Color(250, 250, 250);

		private Microsoft.Xna.Framework.Color lockedColor = new Microsoft.Xna.Framework.Color(30, 30, 30);

		private Microsoft.Xna.Framework.Color noTargetColor = new Microsoft.Xna.Framework.Color(130, 130, 130);

		private Microsoft.Xna.Framework.Color cooldownColor = new Microsoft.Xna.Framework.Color(50, 50, 50);

		private bool checkedAPIForUnlock;

		private static readonly Logger Logger = Logger.GetLogger<Module>();

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll")]
		public static extern bool PostMessage(int hWnd, uint Msg, int wParam, int lParam);

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
		}

		protected override void Initialize()
		{
			switch (GameService.Overlay.UserLocale.Value)
			{
			case Locale.English:
				language = BadLocalization.ENGLISH;
				break;
			case Locale.French:
				language = BadLocalization.FRENCH;
				break;
			case Locale.German:
				language = BadLocalization.GERMAN;
				break;
			case Locale.Spanish:
				language = BadLocalization.SPANISH;
				break;
			default:
				language = BadLocalization.ENGLISH;
				break;
			}
		}

		protected override async Task LoadAsync()
		{
			tomeWindow = new StandardWindow(ContentsManager.GetTexture("WindowBackground.png"), new Microsoft.Xna.Framework.Rectangle(40, 26, 913, 691), new Microsoft.Xna.Framework.Rectangle(70, 71, 839, 605))
			{
				Parent = GameService.Graphics.SpriteScreen,
				Title = BadLocalization.WINDOWTITLE[language],
				SavesPosition = true,
				SavesSize = true,
				Id = "0001",
				CanResize = true
			};
			Checkbox targetCheckbox = new Checkbox
			{
				Text = BadLocalization.TARGETCHECKBOXTEXT[language],
				Location = new Point(0, 0),
				BasicTooltipText = BadLocalization.TARGETCHECKBOXTOOLTIP[language],
				Parent = tomeWindow
			};
			Checkbox synchronCheckbox = new Checkbox
			{
				Text = BadLocalization.SYNCHRONCHECKBOXTEXT[language],
				Location = new Point(0, 20),
				BasicTooltipText = BadLocalization.SYNCHRONCHECKBOXTOOLTIP[language],
				Parent = tomeWindow
			};
			FlowPanel mainPanel = new FlowPanel
			{
				Size = new Point(tomeWindow.ContentRegion.Width, tomeWindow.ContentRegion.Height),
				Location = new Point(0, 50),
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				Parent = tomeWindow,
				CanScroll = true,
				Padding = new Thickness(20f)
			};
			FlowPanel corePanel = new FlowPanel
			{
				ShowBorder = true,
				Title = BadLocalization.COREPANELTITLE[language],
				Size = new Point(mainPanel.ContentRegion.Width, mainPanel.ContentRegion.Height),
				Parent = mainPanel,
				CanCollapse = true,
				FlowDirection = ControlFlowDirection.LeftToRight,
				HeightSizingMode = SizingMode.AutoSize,
				AutoSizePadding = new Point(5, 5),
				ControlPadding = new Vector2(5f, 5f),
				OuterControlPadding = new Vector2(5f, 5f)
			};
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
				Image emoteImage3 = new Image(ContentsManager.GetTexture(emote3.getImagePath()))
				{
					Size = new Point(size, size),
					BasicTooltipText = emote3.getToolTipp()[language],
					Parent = corePanel
				};
				emoteImage3.Click += delegate
				{
					if (emoteAllowed())
					{
						activateEmote(emote3.getChatCode(), targetCheckbox.Checked, synchronCheckbox.Checked);
					}
				};
				coreEmoteImages.Add(emoteImage3);
				emote3.setImg(emoteImage3);
			}
			FlowPanel unlockablePanel = new FlowPanel
			{
				ShowBorder = true,
				Title = BadLocalization.UNLOCKABLEPANELTITLE[language],
				Size = new Point(mainPanel.ContentRegion.Width, mainPanel.ContentRegion.Height),
				Parent = mainPanel,
				CanCollapse = true,
				FlowDirection = ControlFlowDirection.LeftToRight,
				HeightSizingMode = SizingMode.AutoSize,
				AutoSizePadding = new Point(5, 5),
				ControlPadding = new Vector2(5f, 5f),
				OuterControlPadding = new Vector2(5f, 5f)
			};
			unlockEmoteList = library.loadUnlockEmotes();
			List<Image> unlockEmoteImages = new List<Image>();
			foreach (Emote emote2 in unlockEmoteList)
			{
				if (!emote2.getCategory().Equals(EmoteLibrary.UNLOCKCODE))
				{
					continue;
				}
				Image emoteImage2 = new Image(ContentsManager.GetTexture(emote2.getImagePath()))
				{
					Size = new Point(size, size),
					BasicTooltipText = emote2.getToolTipp()[language],
					Parent = unlockablePanel,
					Tint = lockedColor,
					Enabled = false
				};
				emoteImage2.Click += delegate
				{
					if (emoteAllowed())
					{
						activateEmote(emote2.getChatCode(), targetCheckbox.Checked, synchronCheckbox.Checked);
					}
				};
				unlockEmoteImages.Add(emoteImage2);
				emote2.setImg(emoteImage2);
				emote2.isDeactivatedByLocked(newBool: true);
			}
			FlowPanel rankPanel = new FlowPanel
			{
				ShowBorder = true,
				Title = BadLocalization.RANKPANELTITLE[language],
				Size = new Point(mainPanel.ContentRegion.Width, mainPanel.ContentRegion.Height),
				Parent = mainPanel,
				CanCollapse = true,
				FlowDirection = ControlFlowDirection.LeftToRight,
				HeightSizingMode = SizingMode.AutoSize,
				AutoSizePadding = new Point(5, 5),
				ControlPadding = new Vector2(5f, 5f),
				OuterControlPadding = new Vector2(5f, 5f)
			};
			rankEmoteList = library.loadRankEmotes();
			List<Image> cooldownEmoteImages = new List<Image>();
			foreach (Emote emote in rankEmoteList)
			{
				if (!emote.getCategory().Equals(EmoteLibrary.RANKCODE))
				{
					continue;
				}
				Image emoteImage = new Image(ContentsManager.GetTexture(emote.getImagePath()))
				{
					Size = new Point(size, size),
					BasicTooltipText = emote.getToolTipp()[language],
					Parent = rankPanel,
					Tint = lockedColor,
					Enabled = false
				};
				emoteImage.Click += delegate
				{
					if (emoteAllowed())
					{
						activateEmote(emote.getChatCode(), targetCheckbox.Checked, synchronCheckbox.Checked);
						activateCooldown();
					}
				};
				cooldownEmoteImages.Add(emoteImage);
				emote.setImg(emoteImage);
				emote.isDeactivatedByLocked(newBool: true);
			}
			Panel spacePanel = new Panel
			{
				Size = new Point(mainPanel.ContentRegion.Width, 50),
				Parent = mainPanel
			};
			tomeWindow.Resized += delegate
			{
				mainPanel.Width = tomeWindow.ContentRegion.Width;
				mainPanel.Height = tomeWindow.ContentRegion.Height;
				corePanel.Width = mainPanel.ContentRegion.Width;
				unlockablePanel.Width = mainPanel.ContentRegion.Width;
				rankPanel.Width = mainPanel.ContentRegion.Width;
				spacePanel.Width = mainPanel.ContentRegion.Width;
			};
			targetCheckbox.CheckedChanged += delegate
			{
				if (targetCheckbox.Checked)
				{
					foreach (Emote current in coreEmoteList)
					{
						if (!current.hasTarget())
						{
							current.getImg().Tint = noTargetColor;
							current.isDeactivatedByTargeting(newBool: true);
						}
					}
					foreach (Emote current2 in unlockEmoteList)
					{
						if (!current2.hasTarget() && !current2.isDeactivatedByLocked())
						{
							current2.getImg().Tint = noTargetColor;
							current2.isDeactivatedByTargeting(newBool: true);
						}
					}
					foreach (Emote current3 in rankEmoteList)
					{
						if (!current3.hasTarget() && !current3.isDeactivatedByLocked())
						{
							if (!current3.isDeactivatedByCooldown())
							{
								current3.getImg().Tint = noTargetColor;
							}
							current3.isDeactivatedByTargeting(newBool: true);
						}
					}
				}
				else
				{
					foreach (Emote coreEmote in coreEmoteList)
					{
						coreEmote.getImg().Tint = activatedColor;
						coreEmote.isDeactivatedByTargeting(newBool: false);
					}
					foreach (Emote current5 in unlockEmoteList)
					{
						if (!current5.isDeactivatedByLocked())
						{
							current5.getImg().Tint = activatedColor;
						}
						current5.isDeactivatedByTargeting(newBool: false);
					}
					foreach (Emote current6 in rankEmoteList)
					{
						if (!current6.isDeactivatedByCooldown() && !current6.isDeactivatedByLocked())
						{
							current6.getImg().Tint = activatedColor;
						}
						current6.isDeactivatedByTargeting(newBool: false);
					}
				}
			};
			void activateCooldown()
			{
				foreach (Emote emote4 in rankEmoteList)
				{
					if (!emote4.isDeactivatedByLocked())
					{
						emote4.getImg().Tint = cooldownColor;
						emote4.getImg().Enabled = false;
						emote4.isDeactivatedByCooldown(newBool: true);
					}
				}
				System.Timers.Timer aTimer = new System.Timers.Timer();
				aTimer.Elapsed += OnTimedEvent;
				aTimer.Interval = 60000.0;
				aTimer.Enabled = true;
				void OnTimedEvent(object source, ElapsedEventArgs e)
				{
					aTimer.Enabled = false;
					foreach (Emote emote5 in rankEmoteList)
					{
						if (!emote5.isDeactivatedByLocked())
						{
							if (emote5.isDeactivatedByTargeting())
							{
								emote5.getImg().Tint = noTargetColor;
							}
							else
							{
								emote5.getImg().Tint = activatedColor;
							}
							emote5.getImg().Enabled = true;
							emote5.isDeactivatedByCooldown(newBool: false);
						}
					}
				}
			}
		}

		private void TomeWindow_ContentResized(object sender, RegionChangedEventArgs e)
		{
			throw new NotImplementedException();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			tomeCornerIcon = new CornerIcon
			{
				Icon = ContentsManager.GetTexture("CornerIcon.png"),
				Parent = GameService.Graphics.SpriteScreen
			};
			tomeCornerIcon.Click += delegate
			{
				tomeWindow.ToggleWindow();
				if (tomeWindow.Visible && !checkedAPIForUnlock)
				{
					checkUnlockedEmotesByAPI();
					checkedAPIForUnlock = true;
				}
			};
			base.OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			if (tomeWindow.Visible)
			{
				switch (checkPositionSwitch)
				{
				case 0:
					currentPositionA = GameService.Gw2Mumble.PlayerCharacter.Position;
					break;
				case 1:
					currentPositionB = GameService.Gw2Mumble.PlayerCharacter.Position;
					break;
				case 2:
					currentPositionC = GameService.Gw2Mumble.PlayerCharacter.Position;
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
		}

		private bool emoteAllowed()
		{
			if (IsAnyKeyDown())
			{
				ScreenNotification.ShowNotification(BadLocalization.NOEMOTEONKEYPRESSED[language]);
				return false;
			}
			if (isPlayerMoving())
			{
				ScreenNotification.ShowNotification(BadLocalization.NOEMOTEWHENMOVING[language]);
				return false;
			}
			if (GameService.Gw2Mumble.PlayerCharacter.IsInCombat)
			{
				ScreenNotification.ShowNotification(BadLocalization.NOEMOTEINCOMBAT[language]);
				return false;
			}
			if (!GameService.Gw2Mumble.PlayerCharacter.CurrentMount.ToString().Equals("None"))
			{
				ScreenNotification.ShowNotification(BadLocalization.NOEMOTEONMOUNT[language]);
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
			if (!GameService.Gw2Mumble.UI.IsTextInputFocused)
			{
				Blish_HUD.Controls.Intern.Keyboard.Stroke(VirtualKeyShort.RETURN);
				Thread.Sleep(25);
			}
			Blish_HUD.Controls.Intern.Keyboard.Press(VirtualKeyShort.CONTROL, sendToSystem: true);
			Blish_HUD.Controls.Intern.Keyboard.Stroke(VirtualKeyShort.KEY_A, sendToSystem: true);
			Thread.Sleep(25);
			Blish_HUD.Controls.Intern.Keyboard.Release(VirtualKeyShort.CONTROL, sendToSystem: true);
			Blish_HUD.Controls.Intern.Keyboard.Release(VirtualKeyShort.KEY_A);
			Blish_HUD.Controls.Intern.Keyboard.Release(VirtualKeyShort.KEY_D);
			new InputSimulator().Keyboard.TextEntry(chatCommand);
			Thread.Sleep(50);
			Blish_HUD.Controls.Intern.Keyboard.Stroke(VirtualKeyShort.RETURN);
		}

		private bool isPlayerMoving()
		{
			if (currentPositionA.Equals(currentPositionB) && currentPositionA.Equals(currentPositionC))
			{
				return false;
			}
			return true;
		}

		public bool IsAnyKeyDown()
		{
			foreach (object v in Enum.GetValues(typeof(Key)))
			{
				if ((Key)v != 0 && System.Windows.Input.Keyboard.IsKeyDown((Key)v))
				{
					return true;
				}
			}
			return false;
		}

		private async Task checkUnlockedEmotesByAPI()
		{
			List<TokenPermission> apiKeyPermissions = new List<TokenPermission>
			{
				TokenPermission.Account,
				TokenPermission.Progression,
				TokenPermission.Unlocks,
				TokenPermission.Pvp
			};
			try
			{
				if (Gw2ApiManager.HasPermissions(apiKeyPermissions))
				{
					await Gw2ApiManager.Gw2ApiClient.V2.Account.Finishers.GetAsync();
					PvpStats ranks = await Gw2ApiManager.Gw2ApiClient.V2.Pvp.Stats.GetAsync();
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
						else if (emote7.getChatCode().Equals("rank 10") && ranks.PvpRank >= 10)
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 20") && ranks.PvpRank >= 20)
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 30") && ranks.PvpRank >= 30)
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 40") && ranks.PvpRank >= 40)
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 50") && ranks.PvpRank >= 50)
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 60") && ranks.PvpRank >= 60)
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 70") && ranks.PvpRank >= 70)
						{
							enableRankEmote(emote7);
						}
						else if (emote7.getChatCode().Equals("rank 80") && ranks.PvpRank >= 80)
						{
							enableRankEmote(emote7);
						}
					}
					List<string> unlockedEmotes = new List<string>(await Gw2ApiManager.Gw2ApiClient.V2.Account.Emotes.GetAsync());
					unlockedEmotes = unlockedEmotes.ConvertAll((string d) => d.ToLower());
					foreach (Emote emote6 in unlockEmoteList)
					{
						if (emote6.getChatCode().Equals("bless") || emote6.getChatCode().Equals("heroic") || emote6.getChatCode().Equals("hiss") || emote6.getChatCode().Equals("magicjuggle") || emote6.getChatCode().Equals("paper") || emote6.getChatCode().Equals("possessed") || emote6.getChatCode().Equals("readbook") || emote6.getChatCode().Equals("rock") || emote6.getChatCode().Equals("scissors") || emote6.getChatCode().Equals("serve") || emote6.getChatCode().Equals("sipcoffee"))
						{
							emote6.getImg().Enabled = true;
							emote6.getImg().Tint = activatedColor;
							emote6.isDeactivatedByLocked(newBool: false);
						}
						if (unlockedEmotes.Contains(emote6.getChatCode()))
						{
							emote6.getImg().Enabled = true;
							emote6.getImg().Tint = activatedColor;
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
				emote.getImg().Tint = activatedColor;
				emote.getImg().Enabled = true;
				emote.isDeactivatedByLocked(newBool: false);
			}
			void enableLockedEmote(Emote emote)
			{
				emote.getImg().Tint = activatedColor;
				emote.getImg().Enabled = true;
				emote.isDeactivatedByLocked(newBool: false);
			}
			void enableRankEmote(Emote emote)
			{
				emote.getImg().Tint = activatedColor;
				emote.getImg().Enabled = true;
				emote.isDeactivatedByLocked(newBool: false);
			}
		}
	}
}
