using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using DeimosOilSummary.Controls;
using DeimosOilSummary.Models;
using DeimosOilSummary.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeimosOilSummary
{
	[Export(typeof(Module))]
	public class DeimosOilSummaryModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<DeimosOilSummaryModule>();

		private SettingEntry<bool> _showOnNewLog;

		private SettingEntry<bool> _showPanel;

		private SettingEntry<Point> _panelLocation;

		private SettingCollection _internalSettings;

		private LogWatcher _watcher;

		private OilSummaryPanel _panel;

		private CornerIcon _cornerIcon;

		private OilHitSummary _pendingSummary;

		private string _pendingStatus;

		private readonly object _pendingLock = new object();

		[ImportingConstructor]
		public DeimosOilSummaryModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			_showOnNewLog = settings.DefineSetting<bool>("ShowOnNewLog", true, (Func<string>)(() => "Show panel when a new Deimos log is parsed"), (Func<string>)(() => "Automatically open the oil summary after each Deimos pull."));
			_showPanel = settings.DefineSetting<bool>("ShowPanel", true, (Func<string>)(() => "Show oil summary panel"), (Func<string>)(() => "Toggle the Deimos oil summary overlay."));
			_showPanel.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowPanelChanged);
			_internalSettings = settings.AddSubCollection("Internal", false);
			_panelLocation = _internalSettings.DefineSetting<Point>("PanelLocation", new Point(40, 240), (Func<string>)null, (Func<string>)null);
		}

		protected override async Task LoadAsync()
		{
			DeimosOilSummaryModule deimosOilSummaryModule = this;
			OilSummaryPanel oilSummaryPanel = new OilSummaryPanel();
			((Control)oilSummaryPanel).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)oilSummaryPanel).set_Location(_panelLocation.get_Value());
			((Control)oilSummaryPanel).set_Visible(_showPanel.get_Value());
			deimosOilSummaryModule._panel = oilSummaryPanel;
			((Control)_panel).add_Moved((EventHandler<MovedEventArgs>)delegate
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				_panelLocation.set_Value(((Control)_panel).get_Location());
			});
			_panel.ParseLatestClicked += OnParseLatestClicked;
			DeimosOilSummaryModule deimosOilSummaryModule2 = this;
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(base.ModuleParameters.get_ContentsManager().GetTexture("who-touched-oils-icon.png")));
			((Control)val).set_BasicTooltipText("Who Touched Oils");
			val.set_Priority(1847291054);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			deimosOilSummaryModule2._cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_showPanel.set_Value(!_showPanel.get_Value());
			});
			_watcher = new LogWatcher();
			_watcher.SummaryReady += OnSummaryReady;
			_watcher.StatusChanged += OnStatusChanged;
			_watcher.Start();
			Logger.Info("Deimos Oil Summary loaded. Watching {0} log root(s).", new object[1] { _watcher.WatchedPaths.Length });
			await Task.CompletedTask;
		}

		protected override void Update(GameTime gameTime)
		{
			OilHitSummary summary = null;
			string status = null;
			lock (_pendingLock)
			{
				if (_pendingSummary != null)
				{
					summary = _pendingSummary;
					_pendingSummary = null;
				}
				if (_pendingStatus != null)
				{
					status = _pendingStatus;
					_pendingStatus = null;
				}
			}
			if (_panel == null)
			{
				return;
			}
			if (status != null)
			{
				_panel.SetStatus(status);
			}
			if (summary != null)
			{
				_panel.Apply(summary);
				if (_showOnNewLog.get_Value())
				{
					_showPanel.set_Value(true);
					((Control)_panel).set_Visible(true);
				}
				ScreenNotification.ShowNotification((summary.Success && summary.TotalHits == 0) ? "Deimos: no oil hits" : (summary.Success ? $"Deimos oils: {summary.TotalHits} hit(s) — see panel" : "Deimos oil parse failed"), (NotificationType)((!summary.Success) ? 1 : 0), (Texture2D)null, 4);
			}
		}

		protected override void Unload()
		{
			if (_showPanel != null)
			{
				_showPanel.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowPanelChanged);
			}
			if (_panel != null)
			{
				_panel.ParseLatestClicked -= OnParseLatestClicked;
			}
			if (_watcher != null)
			{
				_watcher.SummaryReady -= OnSummaryReady;
				_watcher.StatusChanged -= OnStatusChanged;
				_watcher.Dispose();
				_watcher = null;
			}
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			_cornerIcon = null;
			OilSummaryPanel panel = _panel;
			if (panel != null)
			{
				((Control)panel).Dispose();
			}
			_panel = null;
		}

		private void OnShowPanelChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (_panel != null)
			{
				((Control)_panel).set_Visible(e.get_NewValue());
			}
		}

		private void OnStatusChanged(object sender, string status)
		{
			lock (_pendingLock)
			{
				_pendingStatus = status;
			}
		}

		private void OnSummaryReady(object sender, OilHitSummary summary)
		{
			lock (_pendingLock)
			{
				_pendingSummary = summary;
			}
		}

		private void OnParseLatestClicked(object sender, EventArgs e)
		{
			if (_watcher != null && _panel != null)
			{
				OilHitSummary summary = _watcher.ParseLatestDeimos();
				_panel.Apply(summary);
				_showPanel.set_Value(true);
				((Control)_panel).set_Visible(true);
			}
		}
	}
}
