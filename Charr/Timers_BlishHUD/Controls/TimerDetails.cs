using System;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Charr.Timers_BlishHUD.Models;

namespace Charr.Timers_BlishHUD.Controls
{
	public class TimerDetails : DetailsButton
	{
		private string _enableSettingName;

		private SettingEntry<bool> _enableSetting;

		private readonly GlowButton _authButton;

		private readonly GlowButton _toggleButton;

		private readonly GlowButton _descButton;

		private readonly GlowButton _reloadButton;

		private new bool _enabled;

		private Encounter _encounter;

		public new bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				_enabled = value;
				if (_enableSetting != null)
				{
					_enableSetting.Value = _enabled;
				}
				if (_encounter != null)
				{
					_encounter.Enabled = _enabled;
				}
				if (_toggleButton != null)
				{
					_toggleButton.Checked = _enabled;
				}
				base.ToggleState = _enabled;
			}
		}

		public Encounter Encounter
		{
			get
			{
				return _encounter;
			}
			set
			{
				_encounter = value;
				bool encounterValid = _encounter.State != Encounter.EncounterStates.Error;
				_enableSettingName = "TimerEnable:" + _encounter.Id;
				if (!TimersModule.ModuleInstance._timerSettingCollection.TryGetSetting(_enableSettingName, out _enableSetting))
				{
					_enableSetting = TimersModule.ModuleInstance._timerSettingCollection.DefineSetting(_enableSettingName, _encounter.Enabled);
				}
				Enabled = encounterValid && _enableSetting.Value;
				base.Text = _encounter.Name + (encounterValid ? "" : "\nLoad Error - Check Description for Details\n");
				base.Icon = _encounter.Icon;
				_authButton.Visible = !string.IsNullOrEmpty(_encounter.Author);
				_authButton.BasicTooltipText = "Timer Author: " + _encounter.Author;
				_descButton.Visible = !string.IsNullOrEmpty(_encounter.Description);
				_descButton.BasicTooltipText = "---Timer Description---\n" + _encounter.Description;
				_toggleButton.Enabled = encounterValid;
				_toggleButton.Icon = (encounterValid ? TimersModule.ModuleInstance.Resources.TextureEye : TimersModule.ModuleInstance.Resources.TextureX);
				if (encounterValid)
				{
					_toggleButton.Click += delegate
					{
						Enabled = _toggleButton.Checked;
						this.TimerToggled?.Invoke(this, Encounter);
					};
				}
				Invalidate();
			}
		}

		public event EventHandler<Encounter> TimerToggled;

		public event EventHandler<Encounter> ReloadClicked;

		public TimerDetails()
		{
			base.IconSize = DetailsIconSize.Small;
			base.ShowVignette = false;
			base.HighlightType = DetailsHighlightType.LightHighlight;
			_authButton = new GlowButton
			{
				Icon = TimersModule.ModuleInstance.Resources.TextureDescription,
				Visible = false
			};
			_descButton = new GlowButton
			{
				Icon = TimersModule.ModuleInstance.Resources.TextureScout,
				Visible = false
			};
			_reloadButton = new GlowButton
			{
				Icon = TimersModule.ModuleInstance.Resources.TextureRefresh,
				BasicTooltipText = "Click to reload timer",
				Visible = true
			};
			_reloadButton.Click += delegate
			{
				this.ReloadClicked?.Invoke(this, Encounter);
			};
			base.ShowToggleButton = true;
			_toggleButton = new GlowButton
			{
				ActiveIcon = TimersModule.ModuleInstance.Resources.TextureEyeActive,
				BasicTooltipText = "Click to toggle timer",
				ToggleGlow = true
			};
		}

		public void Initialize()
		{
			_authButton.Parent = this;
			_descButton.Parent = this;
			_reloadButton.Parent = this;
			_toggleButton.Parent = this;
		}
	}
}
