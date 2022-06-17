using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Charr.Timers_BlishHUD.Controls;
using Charr.Timers_BlishHUD.Controls.BigWigs;
using Charr.Timers_BlishHUD.Pathing.Content;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Charr.Timers_BlishHUD.Models
{
	public class Alert : IDisposable
	{
		private bool _activated;

		private bool _showAlert = true;

		[JsonProperty("uid")]
		public string UID { get; set; }

		[JsonProperty("warningDuration")]
		public float WarningDuration { get; set; } = 15f;


		[JsonProperty("alertDuration")]
		public float AlertDuration { get; set; } = 5f;


		[JsonProperty("warning")]
		public string WarningText { get; set; }

		[JsonProperty("warningColor")]
		public List<float> WarningTextColor { get; set; }

		[JsonProperty("alert")]
		public string AlertText { get; set; }

		[JsonProperty("alertColor")]
		public List<float> AlertTextColor { get; set; }

		[JsonProperty("icon")]
		public string IconString { get; set; } = "raid";


		[JsonProperty("fillColor")]
		public List<float> FillColor { get; set; }

		[JsonProperty("timestamps")]
		public List<float> Timestamps { get; set; }

		public bool Activated
		{
			get
			{
				return _activated;
			}
			set
			{
				if (value)
				{
					Activate();
				}
				else
				{
					Deactivate();
				}
			}
		}

		public bool ShowAlert
		{
			get
			{
				return _showAlert;
			}
			set
			{
				if (activePanels != null)
				{
					foreach (KeyValuePair<float, IAlertPanel> activePanel in activePanels)
					{
						activePanel.Value.ShouldShow = value;
					}
				}
				_showAlert = value;
			}
		}

		public Color Fill { get; set; } = Color.get_DarkGray();


		public Color WarningColor { get; set; } = Color.get_White();


		public Color AlertColor { get; set; } = Color.get_White();


		public AsyncTexture2D Icon { get; set; }

		public Dictionary<float, IAlertPanel> activePanels { get; set; }

		public string Initialize(PathableResourceManager resourceManager)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(WarningText))
			{
				WarningDuration = 0f;
			}
			if (string.IsNullOrEmpty(AlertText))
			{
				AlertDuration = 0f;
			}
			if (Timestamps == null || Timestamps.Count == 0)
			{
				return WarningText + "/" + AlertText + " timestamps property invalid";
			}
			Fill = Resources.ParseColor(Fill, FillColor);
			WarningColor = Resources.ParseColor(WarningColor, WarningTextColor);
			AlertColor = Resources.ParseColor(AlertColor, AlertTextColor);
			Icon = TimersModule.ModuleInstance.Resources.GetIcon(IconString);
			if (Icon == null)
			{
				Icon = resourceManager.LoadTexture(IconString);
			}
			activePanels = new Dictionary<float, IAlertPanel>();
			return null;
		}

		public void Activate()
		{
			if (!Activated && activePanels != null)
			{
				_activated = true;
			}
		}

		public void Stop()
		{
			if (Activated)
			{
				Dispose();
			}
		}

		public void Deactivate()
		{
			if (Activated)
			{
				Dispose();
				_activated = false;
			}
		}

		private IAlertPanel CreatePanel()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			IAlertPanel alertPanel2;
			if (TimersModule.ModuleInstance._alertSizeSetting.Value != AlertType.BigWigStyle)
			{
				IAlertPanel alertPanel = new AlertPanel
				{
					ControlPadding = new Vector2(8f, 8f),
					PadLeftBeforeControl = true,
					PadTopBeforeControl = true
				};
				alertPanel2 = alertPanel;
			}
			else
			{
				IAlertPanel alertPanel = new BigWigAlert();
				alertPanel2 = alertPanel;
			}
			alertPanel2.Text = (string.IsNullOrEmpty(WarningText) ? AlertText : WarningText);
			alertPanel2.TextColor = (string.IsNullOrEmpty(WarningText) ? AlertColor : WarningColor);
			alertPanel2.Icon = Texture2DExtension.Duplicate(Icon);
			alertPanel2.FillColor = Fill;
			alertPanel2.MaxFill = (string.IsNullOrEmpty(WarningText) ? 0f : WarningDuration);
			alertPanel2.CurrentFill = 0f;
			alertPanel2.ShouldShow = ShowAlert;
			((Control)alertPanel2).Parent = TimersModule.ModuleInstance._alertContainer;
			TimersModule.ModuleInstance._alertContainer.UpdateDisplay();
			return alertPanel2;
		}

		public void Update(float elapsedTime)
		{
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			if (!Activated)
			{
				return;
			}
			foreach (float time in Timestamps)
			{
				if (!activePanels.TryGetValue(time, out var activePanel))
				{
					if (string.IsNullOrEmpty(WarningText) && elapsedTime >= time && elapsedTime < time + AlertDuration)
					{
						activePanels.Add(time, CreatePanel());
					}
					else if (!string.IsNullOrEmpty(WarningText) && elapsedTime >= time - WarningDuration && elapsedTime < time + AlertDuration)
					{
						activePanels.Add(time, CreatePanel());
					}
					continue;
				}
				float activeTime = elapsedTime - (time - WarningDuration);
				if (activeTime >= WarningDuration + AlertDuration)
				{
					activePanel.Dispose();
					activePanels.Remove(time);
				}
				else if (activeTime >= WarningDuration)
				{
					if (activePanel.CurrentFill != WarningDuration)
					{
						activePanel.CurrentFill = WarningDuration;
					}
					activePanel.Text = (string.IsNullOrEmpty(AlertText) ? WarningText : AlertText);
					activePanel.TimerText = "";
					activePanel.TextColor = AlertColor;
				}
				else
				{
					activePanel.CurrentFill = activeTime + TimersModule.ModuleInstance.Resources.TICKINTERVAL;
					if (WarningDuration - activeTime < 5f)
					{
						activePanel.TimerText = ((float)Math.Round((decimal)(WarningDuration - activeTime), 1)).ToString("0.0");
						activePanel.TimerTextColor = Color.get_Yellow();
					}
					else
					{
						activePanel.TimerText = ((float)Math.Floor((decimal)(WarningDuration - activeTime))).ToString();
					}
				}
			}
		}

		public void Dispose()
		{
			if (activePanels == null)
			{
				return;
			}
			foreach (KeyValuePair<float, IAlertPanel> activePanel in activePanels)
			{
				activePanel.Value.Dispose();
			}
			activePanels.Clear();
		}
	}
}
