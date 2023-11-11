using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nekres.QuickSurrender.Properties;

namespace Nekres.Quick_Surrender_Module
{
	[Export(typeof(Module))]
	public class QuickSurrenderModule : Module
	{
		private enum Ping
		{
			GG,
			FF,
			QQ,
			Surrender,
			Concede,
			Forfeit,
			Resign,
			Capituler,
			Abandonner,
			Renoncer,
			Concéder,
			Kapitulieren,
			Resignieren,
			Ergeben,
			Aufgeben,
			Rendirse,
			Renunciar,
			Capitular
		}

		private static readonly Logger Logger = Logger.GetLogger(typeof(QuickSurrenderModule));

		internal static QuickSurrenderModule Instance;

		private Texture2D _surrenderFlagHover;

		private Texture2D _surrenderFlag;

		private Texture2D _surrenderFlagPressed;

		private Image _surrenderButton;

		private SettingEntry<bool> _surrenderButtonEnabled;

		private SettingEntry<KeyBinding> _surrenderBinding;

		private SettingEntry<Ping> _surrenderPing;

		private SettingEntry<KeyBinding> _chatMessageKeySetting;

		private const string SURRENDER_TEXT = "/gg";

		private const int COOLDOWN_MS = 5000;

		private DateTime _lastSurrenderTime;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public QuickSurrenderModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Expected O, but got Unknown
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Expected O, but got Unknown
			_surrenderButtonEnabled = settings.DefineSetting<bool>("SurrenderButtonEnabled", true, (Func<string>)(() => Resources.Show_Surrender_Skill), (Func<string>)(() => Resources.Displays_a_skill_to_assist_in_conceding_defeat_));
			_surrenderPing = settings.DefineSetting<Ping>("SurrenderButtonPing", Ping.GG, (Func<string>)(() => Resources.Chat_Display), (Func<string>)(() => Resources.Determines_how_the_surrender_skill_is_displayed_in_chat_using__Ctrl__or__Shift_____Left_Mouse__));
			SettingCollection keyBindingCol = settings.AddSubCollection("Hotkey", true, (Func<string>)(() => Resources.Hotkeys));
			_surrenderBinding = keyBindingCol.DefineSetting<KeyBinding>("SurrenderButtonKey", new KeyBinding((Keys)190), (Func<string>)(() => Resources.Surrender), (Func<string>)(() => Resources.Concede_defeat_by_finishing_yourself_));
			SettingCollection controlOptions = settings.AddSubCollection("control_options", true, (Func<string>)(() => Resources.Control_Options + " (" + Resources.User_Interface + ")"));
			_chatMessageKeySetting = controlOptions.DefineSetting<KeyBinding>("ChatMessageKey", new KeyBinding((Keys)13), (Func<string>)(() => Resources.Chat_Message), (Func<string>)(() => Resources.Give_focus_to_the_chat_edit_box_));
		}

		protected override void Initialize()
		{
			LoadTextures();
		}

		private void LoadTextures()
		{
			_surrenderFlag = ContentsManager.GetTexture("surrender_flag.png");
			_surrenderFlagHover = ContentsManager.GetTexture("surrender_flag_hover.png");
			_surrenderFlagPressed = ContentsManager.GetTexture("surrender_flag_pressed.png");
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			_surrenderBinding.get_Value().set_Enabled(true);
			_surrenderBinding.get_Value().add_Activated((EventHandler<EventArgs>)OnSurrenderBindingActivated);
			_surrenderButtonEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSurrenderButtonEnabledSettingChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			((Control)GameService.Graphics.get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)OnSpriteScreenResized);
			GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			BuildSurrenderButton();
			((Module)this).OnModuleLoaded(e);
		}

		private void OnUserLocaleChanged(object sender, ValueEventArgs<CultureInfo> e)
		{
			BuildSurrenderButton();
		}

		protected override void Unload()
		{
			_surrenderBinding.get_Value().remove_Activated((EventHandler<EventArgs>)OnSurrenderBindingActivated);
			_surrenderButtonEnabled.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSurrenderButtonEnabledSettingChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			((Control)GameService.Graphics.get_SpriteScreen()).remove_Resized((EventHandler<ResizedEventArgs>)OnSpriteScreenResized);
			GameService.Overlay.remove_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			Image surrenderButton = _surrenderButton;
			if (surrenderButton != null)
			{
				((Control)surrenderButton).Dispose();
			}
			Texture2D surrenderFlag = _surrenderFlag;
			if (surrenderFlag != null)
			{
				((GraphicsResource)surrenderFlag).Dispose();
			}
			Texture2D surrenderFlagHover = _surrenderFlagHover;
			if (surrenderFlagHover != null)
			{
				((GraphicsResource)surrenderFlagHover).Dispose();
			}
			Texture2D surrenderFlagPressed = _surrenderFlagPressed;
			if (surrenderFlagPressed != null)
			{
				((GraphicsResource)surrenderFlagPressed).Dispose();
			}
			Instance = null;
		}

		private async Task DoSurrender()
		{
			if ((int)GameService.Gw2Mumble.get_CurrentMap().get_Type() == 4)
			{
				if (DateTime.UtcNow.Subtract(_lastSurrenderTime).TotalMilliseconds < 5000.0)
				{
					ScreenNotification.ShowNotification(Resources.Skill_recharging_, (NotificationType)2, (Texture2D)null, 4);
					return;
				}
				_lastSurrenderTime = DateTime.UtcNow;
				_surrenderBinding.get_Value().set_Enabled(false);
				await ChatUtil.Send("/gg", _chatMessageKeySetting.get_Value(), Logger);
				_surrenderBinding.get_Value().set_Enabled(true);
			}
		}

		private async void OnSurrenderBindingActivated(object o, EventArgs e)
		{
			await DoSurrender();
		}

		private void OnIsMapOpenChanged(object o, ValueEventArgs<bool> e)
		{
			ToggleSurrenderButton(!e.get_Value(), 0.45f);
		}

		private void OnIsInGameChanged(object o, ValueEventArgs<bool> e)
		{
			ToggleSurrenderButton(e.get_Value(), 0.1f);
		}

		private void OnSurrenderButtonEnabledSettingChanged(object o, ValueChangedEventArgs<bool> e)
		{
			ToggleSurrenderButton(e.get_NewValue(), 0.1f);
		}

		private void ToggleSurrenderButton(bool enabled, float tDuration)
		{
			if (enabled)
			{
				BuildSurrenderButton();
			}
			else
			{
				if (_surrenderButton == null)
				{
					return;
				}
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Image>(_surrenderButton, (object)new
				{
					Opacity = 0f
				}, tDuration, 0f, true).OnComplete((Action)delegate
				{
					Image surrenderButton = _surrenderButton;
					if (surrenderButton != null)
					{
						((Control)surrenderButton).Dispose();
					}
				});
			}
		}

		private void BuildSurrenderButton()
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Invalid comparison between Unknown and I4
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Expected O, but got Unknown
			Image surrenderButton = _surrenderButton;
			if (surrenderButton != null)
			{
				((Control)surrenderButton).Dispose();
			}
			if (!_surrenderButtonEnabled.get_Value() || (int)GameService.Gw2Mumble.get_CurrentMap().get_Type() != 4)
			{
				return;
			}
			Point tooltipSize = default(Point);
			((Point)(ref tooltipSize))._002Ector(300, ((int)GameService.Overlay.get_UserLocale().get_Value() == 3) ? 120 : 100);
			Tooltip val = new Tooltip();
			((Control)val).set_Size(tooltipSize);
			Tooltip surrenderButtonTooltip = val;
			((Control)new FormattedLabelBuilder().SetWidth(tooltipSize.X).SetHeight(tooltipSize.Y).SetHorizontalAlignment((HorizontalAlignment)2)
				.SetVerticalAlignment((VerticalAlignment)0)
				.CreatePart($"{Math.Round(5.0)}", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
				{
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					o.SetFontSize((FontSize)16);
					o.SetSuffixImageSize(new Point(18, 18));
					o.SetSuffixImage(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156651));
				})
				.Build()).set_Parent((Container)(object)surrenderButtonTooltip);
			((Control)new FormattedLabelBuilder().SetWidth(tooltipSize.X).SetHeight(tooltipSize.Y).SetVerticalAlignment((VerticalAlignment)0)
				.CreatePart(Resources.Surrender + "\n", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					o.SetTextColor(new Color(255, 204, 119));
					o.MakeBold();
					o.SetFontSize((FontSize)20);
				})
				.CreatePart(Resources.Chat_Command + ". ", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
				{
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					o.SetTextColor(new Color(240, 224, 129));
					o.SetFontSize((FontSize)16);
				})
				.CreatePart(Resources.Concede_defeat_by_finishing_yourself_ + "\n", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
				{
					o.SetFontSize((FontSize)16);
				})
				.CreatePart(Resources.You_are_defeated_, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
				{
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					o.SetTextColor(new Color(175, 175, 175));
					o.SetFontSize((FontSize)16);
					o.SetPrefixImage(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(102540));
				})
				.Wrap()
				.Build()).set_Parent((Container)(object)surrenderButtonTooltip);
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val2).set_Size(new Point(45, 45));
			val2.set_Texture(AsyncTexture2D.op_Implicit(_surrenderFlag));
			((Control)val2).set_Tooltip(surrenderButtonTooltip);
			((Control)val2).set_Opacity(0f);
			_surrenderButton = val2;
			((Control)_surrenderButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				_surrenderButton.set_Texture(AsyncTexture2D.op_Implicit(_surrenderFlagHover));
			});
			((Control)_surrenderButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				_surrenderButton.set_Texture(AsyncTexture2D.op_Implicit(_surrenderFlag));
			});
			((Control)_surrenderButton).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				((Control)_surrenderButton).set_Size(new Point(43, 43));
				_surrenderButton.set_Texture(AsyncTexture2D.op_Implicit(_surrenderFlagPressed));
			});
			((Control)_surrenderButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				((Control)_surrenderButton).set_Size(new Point(45, 45));
				_surrenderButton.set_Texture(AsyncTexture2D.op_Implicit(_surrenderFlag));
			});
			((Control)_surrenderButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (KeyboardUtil.IsCtrlPressed())
				{
					await ChatUtil.Send($"[/{_surrenderPing.get_Value()}]", _chatMessageKeySetting.get_Value(), Logger);
				}
				else if (KeyboardUtil.IsShiftPressed())
				{
					await ChatUtil.Insert($"[/{_surrenderPing.get_Value()}]", _chatMessageKeySetting.get_Value(), Logger);
				}
				else
				{
					await DoSurrender();
				}
			});
			ValidatePosition();
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Image>(_surrenderButton, (object)new
			{
				Opacity = 1f
			}, 0.35f, 0f, true);
		}

		private void ValidatePosition()
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			if (_surrenderButton != null)
			{
				((Control)_surrenderButton).set_Location(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 - ((Control)_surrenderButton).get_Width() / 2 + 431, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)_surrenderButton).get_Height() * 2 + 7));
			}
		}

		private void OnSpriteScreenResized(object sender, ResizedEventArgs e)
		{
			ValidatePosition();
		}
	}
}
