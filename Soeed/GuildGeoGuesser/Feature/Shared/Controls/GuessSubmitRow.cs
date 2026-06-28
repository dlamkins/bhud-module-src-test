using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Settings.Controls;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public sealed class GuessSubmitRow : FlowPanel, IDisposable
	{
		private const int ButtonWidth = 300;

		private const int ButtonHeight = 40;

		private const int SettingWidth = 320;

		private StandardButton? _standardButton;

		private NuclearOptionButton? _protectedButton;

		private string _tooltipText = string.Empty;

		private bool _guessAllowed = true;

		private bool _submitting;

		public string ButtonText { get; set; } = "Make my guess here";


		public string ProtectedButtonText { get; set; } = "Make my guess here (HOLD CTRL+SHIFT)";


		public string TooltipText
		{
			get
			{
				return _tooltipText;
			}
			set
			{
				_tooltipText = value;
				SyncButtonState();
			}
		}

		public bool GuessAllowed
		{
			get
			{
				return _guessAllowed;
			}
			set
			{
				_guessAllowed = value;
				SyncButtonState();
			}
		}

		public event EventHandler<MouseEventArgs>? GuessClicked;

		public GuessSubmitRow()
			: this()
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)2);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)this).set_ControlPadding(new Vector2(10f, 0f));
			((FlowPanel)this).set_OuterControlPadding(Vector2.get_Zero());
			Service.Settings.ProtectedGuessButton.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnProtectedModeChanged);
			RebuildGuessButton();
		}

		public void SetSubmitting(bool submitting)
		{
			_submitting = submitting;
			SyncButtonState();
		}

		private void OnProtectedModeChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			RebuildGuessButton();
		}

		private void RebuildGuessButton()
		{
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Expected O, but got Unknown
			if (_standardButton != null)
			{
				((Control)_standardButton).remove_Click((EventHandler<MouseEventArgs>)OnGuessClick);
				((Control)_standardButton).Dispose();
				_standardButton = null;
			}
			if (_protectedButton != null)
			{
				((Control)_protectedButton).remove_Click((EventHandler<MouseEventArgs>)OnGuessClick);
				((Control)_protectedButton).Dispose();
				_protectedButton = null;
			}
			foreach (Control item in ((Container)this).get_Children().ToList())
			{
				item.Dispose();
			}
			((Container)this).get_Children().Clear();
			if (Service.Settings.ProtectedGuessButton.get_Value())
			{
				NuclearOptionButton nuclearOptionButton = new NuclearOptionButton();
				((Control)nuclearOptionButton).set_Parent((Container)(object)this);
				((StandardButton)nuclearOptionButton).set_Text(ProtectedButtonText);
				((Control)nuclearOptionButton).set_Width(300);
				((Control)nuclearOptionButton).set_Height(40);
				((Control)nuclearOptionButton).set_BasicTooltipText(TooltipText);
				_protectedButton = nuclearOptionButton;
				((Control)_protectedButton).add_Click((EventHandler<MouseEventArgs>)OnGuessClick);
			}
			else
			{
				StandardButton val = new StandardButton();
				((Control)val).set_Parent((Container)(object)this);
				val.set_Text(ButtonText);
				((Control)val).set_Width(300);
				((Control)val).set_Height(40);
				((Control)val).set_BasicTooltipText(TooltipText);
				_standardButton = val;
				((Control)_standardButton).add_Click((EventHandler<MouseEventArgs>)OnGuessClick);
			}
			((FlowPanel)(object)this).AddSetting((SettingEntry)(object)Service.Settings.ProtectedGuessButton, 320);
			SyncButtonState();
		}

		private void SyncButtonState()
		{
			if (_standardButton != null)
			{
				_standardButton!.set_Text(ButtonText);
				((Control)_standardButton).set_BasicTooltipText(TooltipText);
				((Control)_standardButton).set_Enabled(GuessAllowed && !_submitting);
			}
			if (_protectedButton != null)
			{
				((StandardButton)_protectedButton).set_Text(ProtectedButtonText);
				((Control)_protectedButton).set_BasicTooltipText(TooltipText);
				_protectedButton!.OperationAllowed = GuessAllowed && !_submitting;
			}
		}

		private void OnGuessClick(object sender, MouseEventArgs e)
		{
			this.GuessClicked?.Invoke(this, e);
		}

		public Vector2 GetBurstOrigin(MouseEventArgs e, Control? clickSource)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (clickSource != null)
			{
				Point screenLocation = GetControlScreenLocation(clickSource);
				return new Vector2((float)(screenLocation.X + e.get_MousePosition().X), (float)(screenLocation.Y + e.get_MousePosition().Y));
			}
			return GetGuessButtonScreenCenter();
		}

		private Vector2 GetGuessButtonScreenCenter()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			Control button = (Control)(((object)_standardButton) ?? ((object)_protectedButton));
			if (button == null)
			{
				return new Vector2((float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2f, (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height() / 2f);
			}
			Point screenLocation = GetControlScreenLocation(button);
			return new Vector2((float)screenLocation.X + (float)button.get_Width() / 2f, (float)screenLocation.Y + (float)button.get_Height() / 2f);
		}

		private static Point GetControlScreenLocation(Control control)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			Point location = Point.get_Zero();
			for (Control current = control; current != null; current = (Control)(object)current.get_Parent())
			{
				location += current.get_Location();
			}
			return location;
		}

		public void Dispose()
		{
			Service.Settings.ProtectedGuessButton.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnProtectedModeChanged);
			if (_standardButton != null)
			{
				((Control)_standardButton).remove_Click((EventHandler<MouseEventArgs>)OnGuessClick);
			}
			if (_protectedButton != null)
			{
				((Control)_protectedButton).remove_Click((EventHandler<MouseEventArgs>)OnGuessClick);
			}
			((Control)this).Dispose();
		}
	}
}
