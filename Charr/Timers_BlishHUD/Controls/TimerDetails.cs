using System;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
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

		private bool _enabled;

		private Encounter _encounter;

		public bool Enabled
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
					_enableSetting.set_Value(_enabled);
				}
				if (_encounter != null)
				{
					_encounter.Enabled = _enabled;
				}
				if (_toggleButton != null)
				{
					_toggleButton.set_Checked(_enabled);
				}
				((DetailsButton)this).set_ToggleState(_enabled);
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
				if (!TimersModule.ModuleInstance._timerSettingCollection.TryGetSetting<bool>(_enableSettingName, ref _enableSetting))
				{
					_enableSetting = TimersModule.ModuleInstance._timerSettingCollection.DefineSetting<bool>(_enableSettingName, _encounter.Enabled, (Func<string>)null, (Func<string>)null);
				}
				Enabled = encounterValid && _enableSetting.get_Value();
				((DetailsButton)this).set_Text(_encounter.Name + (encounterValid ? "" : "\nLoad Error - Check Description for Details\n"));
				((DetailsButton)this).set_Icon(_encounter.Icon);
				((Control)_authButton).set_Visible(!string.IsNullOrEmpty(_encounter.Author));
				((Control)_authButton).set_BasicTooltipText("Timer Author: " + _encounter.Author);
				((Control)_descButton).set_Visible(!string.IsNullOrEmpty(_encounter.Description));
				((Control)_descButton).set_BasicTooltipText("---Timer Description---\n" + _encounter.Description);
				((Control)_toggleButton).set_Enabled(encounterValid);
				_toggleButton.set_Icon(AsyncTexture2D.op_Implicit(encounterValid ? TimersModule.ModuleInstance.Resources.TextureEye : TimersModule.ModuleInstance.Resources.TextureX));
				if (encounterValid)
				{
					((Control)_toggleButton).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						Enabled = _toggleButton.get_Checked();
						this.TimerToggled?.Invoke(this, Encounter);
					});
				}
				((Control)this).Invalidate();
			}
		}

		public event EventHandler<Encounter> TimerToggled;

		public event EventHandler<Encounter> ReloadClicked;

		public TimerDetails()
			: this()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Expected O, but got Unknown
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Expected O, but got Unknown
			((DetailsButton)this).set_IconSize((DetailsIconSize)0);
			((DetailsButton)this).set_ShowVignette(false);
			((DetailsButton)this).set_HighlightType((DetailsHighlightType)2);
			GlowButton val = new GlowButton();
			val.set_Icon(AsyncTexture2D.op_Implicit(TimersModule.ModuleInstance.Resources.TextureDescription));
			((Control)val).set_Visible(false);
			_authButton = val;
			GlowButton val2 = new GlowButton();
			val2.set_Icon(AsyncTexture2D.op_Implicit(TimersModule.ModuleInstance.Resources.TextureScout));
			((Control)val2).set_Visible(false);
			_descButton = val2;
			GlowButton val3 = new GlowButton();
			val3.set_Icon(AsyncTexture2D.op_Implicit(TimersModule.ModuleInstance.Resources.TextureRefresh));
			((Control)val3).set_BasicTooltipText("Click to reload timer");
			((Control)val3).set_Visible(true);
			_reloadButton = val3;
			((Control)_reloadButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.ReloadClicked?.Invoke(this, Encounter);
			});
			((DetailsButton)this).set_ShowToggleButton(true);
			GlowButton val4 = new GlowButton();
			val4.set_ActiveIcon(AsyncTexture2D.op_Implicit(TimersModule.ModuleInstance.Resources.TextureEyeActive));
			((Control)val4).set_BasicTooltipText("Click to toggle timer");
			val4.set_ToggleGlow(true);
			_toggleButton = val4;
		}

		public void Initialize()
		{
			((Control)_authButton).set_Parent((Container)(object)this);
			((Control)_descButton).set_Parent((Container)(object)this);
			((Control)_reloadButton).set_Parent((Container)(object)this);
			((Control)_toggleButton).set_Parent((Container)(object)this);
		}
	}
}
