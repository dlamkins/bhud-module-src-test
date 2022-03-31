using System;
using System.Drawing;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Nekres.Inquest_Module.UI.Controls;

namespace Nekres.Inquest_Module.Core.Controllers
{
	internal class AutoClickController : IDisposable
	{
		private float _soundVolume;

		private SoundEffect[] _doubleClickSfx;

		private DateTime _nextHoldClick = DateTime.UtcNow;

		private DateTime _nextToggleClick = DateTime.UtcNow;

		private bool _paused;

		private TimeSpan _pausedRemainingTime;

		private int _toggleIntervalMs;

		private Point _togglePos;

		private bool _toggleActive;

		private Color _redShift;

		private bool _inTogglePrompt;

		private TaskIndicator _indicator;

		private ClickIndicator _clickIndicator;

		private bool _isDisposing;

		public float SoundVolume
		{
			get
			{
				return Math.Min(GameService.GameIntegration.get_Audio().get_Volume(), _soundVolume);
			}
			set
			{
				_soundVolume = value;
			}
		}

		private KeyBinding AutoClickHoldKey => InquestModule.ModuleInstance.AutoClickHoldKeySetting.get_Value();

		private KeyBinding AutoClickToggleKey => InquestModule.ModuleInstance.AutoClickToggleKeySetting.get_Value();

		public SoundEffect DoubleClickSfx => _doubleClickSfx[RandomUtil.GetRandom(0, 2)];

		public AutoClickController()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			SoundVolume = 1f;
			_redShift = new Color(255, 57, 57);
			AutoClickHoldKey.set_Enabled(true);
			AutoClickHoldKey.add_Activated((EventHandler<EventArgs>)OnHoldActivated);
			AutoClickToggleKey.set_Enabled(true);
			AutoClickToggleKey.add_Activated((EventHandler<EventArgs>)OnToggleActivate);
			_doubleClickSfx = (SoundEffect[])(object)new SoundEffect[3]
			{
				InquestModule.ModuleInstance.ContentsManager.GetSound("audio\\double-click-1.wav"),
				InquestModule.ModuleInstance.ContentsManager.GetSound("audio\\double-click-2.wav"),
				InquestModule.ModuleInstance.ContentsManager.GetSound("audio\\double-click-3.wav")
			};
			GameService.Gw2Mumble.get_PlayerCharacter().add_IsInCombatChanged((EventHandler<ValueEventArgs<bool>>)OnIsInCombatChanged);
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
		}

		private void OnIsInCombatChanged(object o, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				Deactivate();
			}
		}

		private void OnIsInGameChanged(object o, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				Deactivate();
			}
		}

		private void OnHoldActivated(object o, EventArgs e)
		{
			if (_toggleActive)
			{
				Deactivate();
			}
		}

		private void CreateClickIndicator(bool attachToCursor, bool forceRecreation = false)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			if (forceRecreation)
			{
				Deactivate();
			}
			if (_clickIndicator == null)
			{
				ClickIndicator clickIndicator = new ClickIndicator(attachToCursor);
				((Control)clickIndicator).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((Control)clickIndicator).set_Size(new Point(32, 32));
				((Control)clickIndicator).set_Location(new Point(GameService.Input.get_Mouse().get_Position().X - 14, GameService.Input.get_Mouse().get_Position().Y - 14));
				_clickIndicator = clickIndicator;
			}
		}

		private void OnToggleActivate(object o, EventArgs e)
		{
			if (_toggleActive || IsBusy())
			{
				Deactivate();
				return;
			}
			_inTogglePrompt = true;
			_togglePos = Mouse.GetPosition();
			CreateClickIndicator(attachToCursor: false, forceRecreation: true);
			NumericInputPrompt.ShowPrompt(OnToggleInputPromptCallback, "Enter an interval in seconds:");
		}

		private void OnToggleInputPromptCallback(bool confirmed, double input)
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			_inTogglePrompt = false;
			if (!confirmed)
			{
				Deactivate();
				return;
			}
			_toggleActive = true;
			_toggleIntervalMs = Math.Min(300000, Math.Max(250, (int)(input * 1000.0)));
			_nextToggleClick = DateTime.UtcNow;
			if (_indicator == null)
			{
				TaskIndicator taskIndicator = new TaskIndicator(attachToCursor: false);
				((Control)taskIndicator).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((Control)taskIndicator).set_Size(new Point(50, 50));
				((Control)taskIndicator).set_Location(new Point(((Control)_clickIndicator).get_Location().X + 25, ((Control)_clickIndicator).get_Location().Y - 32));
				_indicator = taskIndicator;
			}
		}

		private bool HoldIsTriggering()
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Invalid comparison between Unknown and I4
			int num;
			if (!InquestModule.ModuleInstance.HoldKeyWithLeftClickEnabledSetting.get_Value())
			{
				num = (AutoClickHoldKey.get_IsTriggering() ? 1 : 0);
			}
			else if (AutoClickHoldKey.get_IsTriggering())
			{
				MouseState state = GameService.Input.get_Mouse().get_State();
				num = (((int)((MouseState)(ref state)).get_LeftButton() == 1) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			if (num != 0 && !_inTogglePrompt)
			{
				return !_isDisposing;
			}
			return false;
		}

		public void Update()
		{
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			if (_toggleActive)
			{
				bool isBusy = IsBusy();
				_clickIndicator.Paused = isBusy;
				_indicator.Paused = isBusy;
				((Control)_indicator).set_Visible(!isBusy);
				if (isBusy)
				{
					_nextToggleClick = DateTime.UtcNow.Add(_pausedRemainingTime);
					return;
				}
				TimeSpan remainingTime = DateTime.UtcNow.Subtract(_nextToggleClick);
				_indicator.Text = remainingTime.ToString((remainingTime.TotalSeconds > -1.0) ? "\\.ff" : ((remainingTime.TotalMinutes > -1.0) ? "ss" : "m\\:ss")).TrimStart('0');
				_indicator.TextColor = Color.Lerp(Color.get_White(), _redShift, 1f + (float)remainingTime.TotalMilliseconds / (float)_toggleIntervalMs);
				if (!(DateTime.UtcNow <= _nextToggleClick))
				{
					_clickIndicator.LeftClick();
					if (!InquestModule.ModuleInstance.AutoClickSoundDisabledSetting.get_Value())
					{
						DoubleClickSfx.Play(SoundVolume, 0f, 0f);
					}
					Mouse.DoubleClick((MouseButton)0, _togglePos.X, _togglePos.Y, false);
					Mouse.Click((MouseButton)0, _togglePos.X, _togglePos.Y, false);
					_nextToggleClick = DateTime.UtcNow.AddMilliseconds(_toggleIntervalMs);
				}
			}
			else if (HoldIsTriggering())
			{
				CreateClickIndicator(attachToCursor: true);
				if (!(DateTime.UtcNow <= _nextHoldClick) && GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus())
				{
					_clickIndicator.LeftClick(40);
					if (!InquestModule.ModuleInstance.AutoClickSoundDisabledSetting.get_Value())
					{
						DoubleClickSfx.Play(SoundVolume, 0f, 0f);
					}
					Mouse.DoubleClick((MouseButton)0, -1, -1, false);
					Mouse.Click((MouseButton)0, -1, -1, false);
					_nextHoldClick = DateTime.UtcNow.AddMilliseconds(50.0);
				}
			}
			else if (!_inTogglePrompt)
			{
				Deactivate();
			}
		}

		private bool IsBusy()
		{
			if (!GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() || !GameService.GameIntegration.get_Gw2Instance().get_IsInGame() || GameService.Gw2Mumble.get_UI().get_IsTextInputFocused() || GameService.Input.get_Mouse().get_CameraDragging() || GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat())
			{
				if (_paused)
				{
					return true;
				}
				_paused = true;
				_pausedRemainingTime = _nextToggleClick.Subtract(DateTime.UtcNow);
				return true;
			}
			if (_paused)
			{
				_paused = false;
			}
			return false;
		}

		private void Deactivate()
		{
			_toggleActive = false;
			ClickIndicator clickIndicator = _clickIndicator;
			if (clickIndicator != null)
			{
				((Control)clickIndicator).Dispose();
			}
			_clickIndicator = null;
			TaskIndicator indicator = _indicator;
			if (indicator != null)
			{
				((Control)indicator).Dispose();
			}
			_indicator = null;
		}

		public void Dispose()
		{
			_isDisposing = true;
			AutoClickToggleKey.remove_Activated((EventHandler<EventArgs>)OnToggleActivate);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_IsInCombatChanged((EventHandler<ValueEventArgs<bool>>)OnIsInCombatChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			Deactivate();
			SoundEffect[] doubleClickSfx = _doubleClickSfx;
			foreach (SoundEffect obj in doubleClickSfx)
			{
				if (obj != null)
				{
					obj.Dispose();
				}
			}
		}
	}
}
