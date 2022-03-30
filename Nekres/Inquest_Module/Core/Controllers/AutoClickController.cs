using System;
using System.Drawing;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

		private TaskIndicator _indicator;

		private ClickIndicator _clickIndicator;

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
				DeactivateToggle();
			}
		}

		private void OnIsInGameChanged(object o, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				DeactivateToggle();
			}
		}

		private void OnHoldActivated(object o, EventArgs e)
		{
			if (_toggleActive)
			{
				DeactivateToggle();
			}
		}

		private void SaveTogglePosition()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			_togglePos = Mouse.GetPosition();
			if (_clickIndicator == null)
			{
				ClickIndicator clickIndicator = new ClickIndicator(attachToCursor: false);
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
				DeactivateToggle();
				return;
			}
			SaveTogglePosition();
			NumericInputPrompt.ShowPrompt(OnToggleInputPromptCallback, "Enter an interval in seconds:");
		}

		private void DeactivateToggle()
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

		private void OnToggleInputPromptCallback(bool confirmed, double input)
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			if (!confirmed)
			{
				DeactivateToggle();
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

		public void UpdateIndicator()
		{
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			if (_toggleActive)
			{
				bool isBusy = IsBusy();
				_clickIndicator.Paused = isBusy;
				_indicator.Paused = isBusy;
				((Control)_indicator).set_Visible(!isBusy);
				TimeSpan remainingTime = DateTime.UtcNow.Subtract(_nextToggleClick);
				_indicator.Text = remainingTime.ToString((remainingTime.TotalSeconds > -1.0) ? "\\.ff" : ((remainingTime.TotalMinutes > -1.0) ? "ss" : "m\\:ss")).TrimStart('0');
				_indicator.TextColor = Color.Lerp(Color.get_White(), _redShift, 1f + (float)remainingTime.TotalMilliseconds / (float)_toggleIntervalMs);
			}
		}

		public void Update()
		{
			UpdateIndicator();
			if (IsBusy())
			{
				_nextToggleClick = DateTime.UtcNow.Add(_pausedRemainingTime);
				return;
			}
			if (!_toggleActive && AutoClickHoldKey.get_IsTriggering() && DateTime.UtcNow > _nextHoldClick && GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus())
			{
				if (!InquestModule.ModuleInstance.AutoClickSoundDisabledSetting.get_Value())
				{
					DoubleClickSfx.Play(SoundVolume, 0f, 0f);
				}
				Mouse.DoubleClick((MouseButton)0, -1, -1, true);
				_nextHoldClick = DateTime.UtcNow.AddMilliseconds(50.0);
			}
			if (_toggleActive && DateTime.UtcNow > _nextToggleClick)
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

		public void Dispose()
		{
			DeactivateToggle();
			SoundEffect[] doubleClickSfx = _doubleClickSfx;
			foreach (SoundEffect obj in doubleClickSfx)
			{
				if (obj != null)
				{
					obj.Dispose();
				}
			}
			AutoClickToggleKey.remove_Activated((EventHandler<EventArgs>)OnToggleActivate);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_IsInCombatChanged((EventHandler<ValueEventArgs<bool>>)OnIsInCombatChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
		}
	}
}
