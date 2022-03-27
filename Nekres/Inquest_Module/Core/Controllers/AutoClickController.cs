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
		private SoundEffect[] _doubleClickSfx;

		private DateTime _nextHoldClick = DateTime.UtcNow;

		private DateTime _nextToggleClick = DateTime.UtcNow;

		private bool _paused;

		private TimeSpan _pausedRemainingTime;

		private int _toggleIntervalMs;

		private Point _togglePos;

		private bool _toggleActive;

		private TaskIndicator _indicator;

		private KeyBinding AutoClickHoldKey => InquestModule.ModuleInstance.AutoClickHoldKeySetting.get_Value();

		private KeyBinding AutoClickToggleKey => InquestModule.ModuleInstance.AutoClickToggleKeySetting.get_Value();

		public SoundEffect DoubleClickSfx => _doubleClickSfx[RandomUtil.GetRandom(0, 2)];

		public AutoClickController()
		{
			AutoClickHoldKey.set_Enabled(true);
			AutoClickToggleKey.set_Enabled(true);
			AutoClickToggleKey.add_Activated((EventHandler<EventArgs>)OnToggleActivate);
			_doubleClickSfx = (SoundEffect[])(object)new SoundEffect[3]
			{
				InquestModule.ModuleInstance.ContentsManager.GetSound("audio\\double-click-1.wav"),
				InquestModule.ModuleInstance.ContentsManager.GetSound("audio\\double-click-2.wav"),
				InquestModule.ModuleInstance.ContentsManager.GetSound("audio\\double-click-3.wav")
			};
		}

		private void OnToggleActivate(object o, EventArgs e)
		{
			if (_toggleActive)
			{
				_toggleActive = false;
				TaskIndicator indicator = _indicator;
				if (indicator != null)
				{
					((Control)indicator).Dispose();
				}
				_indicator = null;
			}
			else
			{
				_togglePos = Mouse.GetPosition();
				NumericInputPrompt.ShowPrompt(OnToggleInputPromptCallback, "Enter an interval in seconds:");
			}
		}

		private void OnToggleInputPromptCallback(bool confirmed, int input)
		{
			if (confirmed)
			{
				_toggleActive = true;
				_toggleIntervalMs = Math.Min(300000, Math.Max(75, input * 1000));
				_nextToggleClick = DateTime.UtcNow;
			}
		}

		public void UpdateIndicator()
		{
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			if (_toggleActive)
			{
				if (_indicator != null)
				{
					_indicator.Paused = IsBusy();
					_indicator.Text = DateTime.UtcNow.Subtract(_nextToggleClick).ToString("m\\:ss");
					((Control)_indicator).set_Visible(!GameService.Input.get_Mouse().get_CameraDragging());
				}
				else
				{
					TaskIndicator taskIndicator = new TaskIndicator();
					((Control)taskIndicator).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
					((Control)taskIndicator).set_Size(new Point(50, 50));
					_indicator = taskIndicator;
				}
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
			if (!_toggleActive && AutoClickHoldKey.get_IsTriggering() && DateTime.UtcNow > _nextHoldClick)
			{
				if (InquestModule.ModuleInstance.AutoClickSoundEnabledSetting.get_Value())
				{
					DoubleClickSfx.Play();
				}
				Mouse.DoubleClick((MouseButton)0, -1, -1, true);
				_nextHoldClick = DateTime.UtcNow.AddMilliseconds(50.0);
			}
			if (_toggleActive && DateTime.UtcNow > _nextToggleClick)
			{
				Mouse.SetPosition(_togglePos.X, _togglePos.Y, true);
				if (InquestModule.ModuleInstance.AutoClickSoundEnabledSetting.get_Value())
				{
					DoubleClickSfx.Play();
				}
				Mouse.DoubleClick((MouseButton)0, -1, -1, true);
				_nextToggleClick = DateTime.UtcNow.AddMilliseconds(_toggleIntervalMs);
			}
		}

		private bool IsBusy()
		{
			if (!GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() || !GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus() || GameService.Gw2Mumble.get_UI().get_IsTextInputFocused() || GameService.Input.get_Mouse().get_CameraDragging())
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
			SoundEffect[] doubleClickSfx = _doubleClickSfx;
			foreach (SoundEffect obj in doubleClickSfx)
			{
				if (obj != null)
				{
					obj.Dispose();
				}
			}
			AutoClickToggleKey.remove_Activated((EventHandler<EventArgs>)OnToggleActivate);
		}
	}
}
