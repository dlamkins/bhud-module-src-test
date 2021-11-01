using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
			Resign,
			Surrender,
			Forfeit,
			Concede,
			Aufgeben,
			Rendirse,
			Capitular
		}

		private static readonly Logger Logger = Logger.GetLogger(typeof(QuickSurrenderModule));

		internal static QuickSurrenderModule ModuleInstance;

		private const uint KEY_PRESSED = 32768u;

		private const uint VK_LCONTROL = 162u;

		private const uint VK_LSHIFT = 160u;

		private AsyncTexture2D _surrenderTooltip_texture;

		private AsyncTexture2D _surrenderFlag_hover;

		private AsyncTexture2D _surrenderFlag;

		private AsyncTexture2D _surrenderFlag_pressed;

		private Image _surrenderButton;

		private SettingEntry<bool> SurrenderButtonEnabled;

		private SettingEntry<KeyBinding> SurrenderBinding;

		private SettingEntry<Ping> SurrenderPing;

		private DateTime _lastSurrenderTime;

		private int _cooldownMs;

		private Dictionary<Ping, string> _pingMap;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public QuickSurrenderModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Expected O, but got Unknown
			SurrenderButtonEnabled = settings.DefineSetting<bool>("SurrenderButtonEnabled", true, (Func<string>)(() => "Show Surrender Skill"), (Func<string>)(() => "Shows a skill with a white flag to the right of\nyour skill bar while in an instance. Clicking it defeats you.\n(Sends \"/gg\" into chat when in supported modes.)"));
			SurrenderPing = settings.DefineSetting<Ping>("SurrenderButtonPing", Ping.GG, (Func<string>)(() => "Chat Display"), (Func<string>)(() => "Determines how the surrender skill is displayed in chat using [Ctrl]/[Left Shift] + [Left Mouse]."));
			SettingCollection keyBindingCol = settings.AddSubCollection("Hotkey", true, false);
			SurrenderBinding = keyBindingCol.DefineSetting<KeyBinding>("SurrenderButtonKey", new KeyBinding((Keys)190), (Func<string>)(() => "Surrender"), (Func<string>)(() => "Defeats you.\n(Sends \"/gg\" into chat when in supported modes.)"));
		}

		[DllImport("USER32.dll")]
		private static extern short GetKeyState(uint vk);

		private bool IsPressed(uint key)
		{
			return Convert.ToBoolean((long)GetKeyState(key) & 0x8000L);
		}

		protected override void Initialize()
		{
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected O, but got Unknown
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Expected O, but got Unknown
			_pingMap = new Dictionary<Ping, string>
			{
				{
					Ping.GG,
					"[/gg]"
				},
				{
					Ping.FF,
					"[/ff]"
				},
				{
					Ping.QQ,
					"[/qq]"
				},
				{
					Ping.Resign,
					"[/resign]"
				},
				{
					Ping.Surrender,
					"[/surrender]"
				},
				{
					Ping.Forfeit,
					"[/forfeit]"
				},
				{
					Ping.Concede,
					"[/concede]"
				},
				{
					Ping.Aufgeben,
					"[/aufgeben]"
				},
				{
					Ping.Rendirse,
					"[/rendirse]"
				},
				{
					Ping.Capitular,
					"[/capitular]"
				}
			};
			_surrenderTooltip_texture = new AsyncTexture2D();
			_surrenderFlag = new AsyncTexture2D();
			_surrenderFlag_hover = new AsyncTexture2D();
			_surrenderFlag_pressed = new AsyncTexture2D();
			_lastSurrenderTime = DateTime.Now;
			_cooldownMs = 2000;
		}

		protected override async Task LoadAsync()
		{
			await LoadTextures();
		}

		private Task LoadTextures()
		{
			return Task.Run(delegate
			{
				_surrenderTooltip_texture.SwapTexture(ContentsManager.GetTexture("surrender_tooltip.png"));
				_surrenderFlag.SwapTexture(ContentsManager.GetTexture("surrender_flag.png"));
				_surrenderFlag_hover.SwapTexture(ContentsManager.GetTexture("surrender_flag_hover.png"));
				_surrenderFlag_pressed.SwapTexture(ContentsManager.GetTexture("surrender_flag_pressed.png"));
			});
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			SurrenderBinding.get_Value().set_Enabled(true);
			SurrenderBinding.get_Value().add_Activated((EventHandler<EventArgs>)OnSurrenderBindingActivated);
			SurrenderButtonEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSurrenderButtonEnabledSettingChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			BuildSurrenderButton();
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			if (_surrenderButton != null)
			{
				((Control)_surrenderButton).set_Location(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 - ((Control)_surrenderButton).get_Width() / 2 + 431, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)_surrenderButton).get_Height() * 2 + 7));
			}
		}

		protected override void Unload()
		{
			SurrenderBinding.get_Value().remove_Activated((EventHandler<EventArgs>)OnSurrenderBindingActivated);
			SurrenderButtonEnabled.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSurrenderButtonEnabledSettingChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			Image surrenderButton = _surrenderButton;
			if (surrenderButton != null)
			{
				((Control)surrenderButton).Dispose();
			}
			ModuleInstance = null;
		}

		private void DoSurrender()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Invalid comparison between Unknown and I4
			if (IsUiAvailable() && !GameService.Gw2Mumble.get_UI().get_IsTextInputFocused() && (int)GameService.Gw2Mumble.get_CurrentMap().get_Type() == 4)
			{
				if (DateTimeOffset.Now.Subtract(_lastSurrenderTime).TotalMilliseconds < (double)_cooldownMs)
				{
					ScreenNotification.ShowNotification("Skill recharging.", (NotificationType)2, (Texture2D)null, 4);
					return;
				}
				GameService.GameIntegration.get_Chat().Send("/gg");
				_lastSurrenderTime = DateTime.Now;
			}
		}

		private bool IsUiAvailable()
		{
			return GameService.Gw2Mumble.get_IsAvailable() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
		}

		private void OnSurrenderBindingActivated(object o, EventArgs e)
		{
			DoSurrender();
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
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Invalid comparison between Unknown and I4
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Expected O, but got Unknown
			Image surrenderButton = _surrenderButton;
			if (surrenderButton != null)
			{
				((Control)surrenderButton).Dispose();
			}
			if (!SurrenderButtonEnabled.get_Value() || !IsUiAvailable() || (int)GameService.Gw2Mumble.get_CurrentMap().get_Type() != 4)
			{
				return;
			}
			Tooltip surrenderButtonTooltip = new Tooltip();
			_surrenderTooltip_texture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)delegate
			{
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				((Control)surrenderButtonTooltip).set_Size(new Point(_surrenderTooltip_texture.get_Texture().get_Width(), _surrenderTooltip_texture.get_Texture().get_Height()));
			});
			Image val = new Image(_surrenderTooltip_texture);
			((Control)val).set_Parent((Container)(object)surrenderButtonTooltip);
			((Control)val).set_Location(new Point(0, 0));
			Image surrenderButtonTooltipImage = val;
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val2).set_Size(new Point(45, 45));
			((Control)val2).set_Location(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 - 22, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 45));
			val2.set_Texture(_surrenderFlag);
			((Control)val2).set_Tooltip(surrenderButtonTooltip);
			((Control)val2).set_Opacity(0f);
			_surrenderButton = val2;
			((Control)_surrenderButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				_surrenderButton.set_Texture(_surrenderFlag_hover);
			});
			((Control)_surrenderButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				_surrenderButton.set_Texture(_surrenderFlag);
			});
			((Control)_surrenderButton).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				((Control)_surrenderButton).set_Size(new Point(43, 43));
				_surrenderButton.set_Texture(_surrenderFlag_pressed);
			});
			((Control)_surrenderButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				((Control)_surrenderButton).set_Size(new Point(45, 45));
				_surrenderButton.set_Texture(_surrenderFlag);
			});
			((Control)_surrenderButton).add_Click((EventHandler<MouseEventArgs>)delegate(object o, MouseEventArgs e)
			{
				if (IsPressed(162u))
				{
					GameService.GameIntegration.get_Chat().Send(_pingMap[SurrenderPing.get_Value()]);
				}
				else if (IsPressed(160u))
				{
					GameService.GameIntegration.get_Chat().Paste(_pingMap[SurrenderPing.get_Value()]);
				}
				else
				{
					OnSurrenderBindingActivated(o, (EventArgs)(object)e);
				}
			});
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Image>(_surrenderButton, (object)new
			{
				Opacity = 1f
			}, 0.35f, 0f, true);
		}
	}
}
