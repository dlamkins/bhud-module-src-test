using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Practice;
using Maestro.UI.Controls;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Practice
{
	public class PracticeHud : Panel
	{
		private readonly PracticeSession _session;

		private readonly Dropdown _speedDropdown;

		private readonly IconButton _pauseButton;

		private readonly IconButton _restartButton;

		private readonly IconButton _closeButton;

		private readonly Label _scoreLabel;

		private readonly Label _comboLabel;

		public event Action PauseRequested;

		public event Action RestartRequested;

		public event Action CloseRequested;

		public event Action<float> SpeedChanged;

		public PracticeHud(PracticeSession session, PracticeSettings settings)
			: this()
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Expected O, but got Unknown
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Expected O, but got Unknown
			PracticeHud practiceHud = this;
			_session = session ?? throw new ArgumentNullException("session");
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}
			((Container)this).set_HeightSizingMode((SizingMode)1);
			Dropdown val = new Dropdown();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(8, 8));
			((Control)val).set_Width(80);
			_speedDropdown = val;
			_speedDropdown.get_Items().Add("0.5x");
			_speedDropdown.get_Items().Add("0.75x");
			_speedDropdown.get_Items().Add("1.0x");
			_speedDropdown.set_SelectedItem(SpeedLabel(settings.LastUsedSpeed.get_Value()));
			_speedDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object sender, ValueChangedEventArgs e)
			{
				float num = ParseSpeed(e.get_CurrentValue());
				settings.LastUsedSpeed.set_Value(num);
				practiceHud._session.Clock.Speed = num;
				practiceHud.SpeedChanged?.Invoke(num);
			});
			IconButton iconButton = new IconButton(MaestroIcons.Pause, MaestroTheme.IconGlyph);
			((Control)iconButton).set_Parent((Container)(object)this);
			((Control)iconButton).set_BasicTooltipText("Pause");
			((Control)iconButton).set_Location(new Point(96, 8));
			((Control)iconButton).set_Width(40);
			((Control)iconButton).set_Height(22);
			_pauseButton = iconButton;
			((Control)_pauseButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				practiceHud.PauseRequested?.Invoke();
			});
			IconButton iconButton2 = new IconButton(MaestroIcons.Refresh, MaestroTheme.IconGlyph);
			((Control)iconButton2).set_Parent((Container)(object)this);
			((Control)iconButton2).set_BasicTooltipText("Restart");
			((Control)iconButton2).set_Location(new Point(140, 8));
			((Control)iconButton2).set_Width(40);
			((Control)iconButton2).set_Height(22);
			_restartButton = iconButton2;
			((Control)_restartButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				practiceHud.RestartRequested?.Invoke();
			});
			IconButton iconButton3 = new IconButton(MaestroIcons.Cancel, MaestroTheme.IconGlyph);
			((Control)iconButton3).set_Parent((Container)(object)this);
			((Control)iconButton3).set_BasicTooltipText("Close practice");
			((Control)iconButton3).set_Location(new Point(184, 8));
			((Control)iconButton3).set_Width(40);
			((Control)iconButton3).set_Height(22);
			_closeButton = iconButton3;
			((Control)_closeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				practiceHud.CloseRequested?.Invoke();
			});
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(8, 38));
			((Control)val2).set_Width(200);
			val2.set_Text("SCORE 0");
			_scoreLabel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(220, 38));
			((Control)val3).set_Width(120);
			val3.set_Text("COMBO x0");
			_comboLabel = val3;
		}

		public void RefreshStats()
		{
			int score = _session.PerfectCount * 100 + _session.GoodCount * 50;
			_scoreLabel.set_Text($"SCORE {score}");
			_comboLabel.set_Text($"COMBO x{_session.Combo}");
			bool paused = _session.Clock.IsPaused;
			_pauseButton.IconTexture = (paused ? MaestroIcons.Play : MaestroIcons.Pause);
			((Control)_pauseButton).set_BasicTooltipText(paused ? "Resume" : "Pause");
		}

		private static string SpeedLabel(float s)
		{
			if (s <= 0.5f)
			{
				return "0.5x";
			}
			if (s <= 0.75f)
			{
				return "0.75x";
			}
			return "1.0x";
		}

		private static float ParseSpeed(string label)
		{
			if (label == "0.5x")
			{
				return 0.5f;
			}
			if (label == "0.75x")
			{
				return 0.75f;
			}
			return 1f;
		}
	}
}
