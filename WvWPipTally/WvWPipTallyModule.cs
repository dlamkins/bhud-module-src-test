using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WvWPipTally.Controls;
using WvWPipTally.Models;
using WvWPipTally.Services;

namespace WvWPipTally
{
	[Export(typeof(Module))]
	public class WvWPipTallyModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<WvWPipTallyModule>();

		private readonly DirectoriesManager _directoriesManager;

		private readonly ContentsManager _contentsManager;

		private SettingEntry<bool> _showPanel;

		private SettingEntry<Point> _windowLocation;

		private SettingEntry<bool> _hasSavedLocation;

		private SettingEntry<int> _segmentIndex;

		private SettingEntry<int> _scannedPipsPerTick;

		private SettingEntry<Placement> _placement;

		private SettingEntry<RankTier> _rank;

		private SettingEntry<bool> _commitment;

		private SettingEntry<bool> _commander;

		private SettingEntry<bool> _publicCommander;

		private SettingCollection _internalSettings;

		private PipTallyWindow _window;

		private MatchOverviewHelpWindow _helpWindow;

		private CornerIcon _cornerIcon;

		private AsyncTexture2D _fallbackCornerIcon;

		private readonly Dictionary<string, AsyncTexture2D> _chestIcons = new Dictionary<string, AsyncTexture2D>(StringComparer.OrdinalIgnoreCase);

		private bool _scanInProgress;

		[ImportingConstructor]
		public WvWPipTallyModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			_directoriesManager = moduleParameters.get_DirectoriesManager();
			_contentsManager = moduleParameters.get_ContentsManager();
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			_showPanel = settings.DefineSetting<bool>("ShowPanel", true, (Func<string>)(() => "Show pip tally window"), (Func<string>)(() => "Toggle the WvW Pip Tally window."));
			_placement = settings.DefineSetting<Placement>("Placement", Placement.First, (Func<string>)(() => "World placement"), (Func<string>)(() => "Used when pips/tick was not scanned from Match Overview."));
			_rank = settings.DefineSetting<RankTier>("Rank", RankTier.Diamond, (Func<string>)(() => "Personal rank tier"), (Func<string>)(() => "Used when pips/tick was not scanned from Match Overview."));
			_commitment = settings.DefineSetting<bool>("Commitment", false, (Func<string>)(() => "Commitment bonus"), (Func<string>)(() => "+1 pip when eligible for world commitment."));
			_commander = settings.DefineSetting<bool>("Commander", false, (Func<string>)(() => "Commander bonus"), (Func<string>)(() => "+1 pip while commanding a squad."));
			_publicCommander = settings.DefineSetting<bool>("PublicCommander", false, (Func<string>)(() => "Public commander bonus"), (Func<string>)(() => "+3 pips while publicly tagged."));
			_showPanel.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowPanelChanged);
			_placement.add_SettingChanged((EventHandler<ValueChangedEventArgs<Placement>>)delegate
			{
				RefreshPanel("Updated placement.");
			});
			_rank.add_SettingChanged((EventHandler<ValueChangedEventArgs<RankTier>>)delegate
			{
				RefreshPanel("Updated rank.");
			});
			_commitment.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				RefreshPanel("Updated commitment.");
			});
			_commander.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				RefreshPanel("Updated commander.");
			});
			_publicCommander.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate
			{
				RefreshPanel("Updated public commander.");
			});
			_internalSettings = settings.AddSubCollection("Internal", false);
			_windowLocation = _internalSettings.DefineSetting<Point>("WindowLocation", new Point(80, 120), (Func<string>)null, (Func<string>)null);
			_hasSavedLocation = _internalSettings.DefineSetting<bool>("HasSavedLocation", false, (Func<string>)null, (Func<string>)null);
			_segmentIndex = _internalSettings.DefineSetting<int>("SegmentIndex", 0, (Func<string>)null, (Func<string>)null);
			_scannedPipsPerTick = _internalSettings.DefineSetting<int>("ScannedPipsPerTick", 0, (Func<string>)null, (Func<string>)null);
		}

		protected override async Task LoadAsync()
		{
			try
			{
				ScanService.DebugDirectory = _directoriesManager.GetFullDirectoryPath("wvw-pip-tally");
			}
			catch (Exception ex2)
			{
				Logger.Warn(ex2, "Could not resolve wvw-pip-tally debug directory.");
			}
			AsyncTexture2D windowBg = await Gw2AssetLoader.LoadAsync(155985, 200);
			Logger.Info("Window background ready: {0}x{1}", new object[2]
			{
				windowBg.get_Width(),
				windowBg.get_Height()
			});
			AsyncTexture2D emblem = null;
			try
			{
				emblem = AsyncTexture2D.op_Implicit(_contentsManager.GetTexture("emblem.png"));
			}
			catch (Exception ex4)
			{
				Logger.Warn(ex4, "Could not load emblem.png");
			}
			LoadChestIcons();
			try
			{
				_fallbackCornerIcon = AsyncTexture2D.op_Implicit(_contentsManager.GetTexture("corner-icon.png"));
			}
			catch (Exception ex3)
			{
				Logger.Warn(ex3, "Could not load corner-icon.png");
			}
			if (_fallbackCornerIcon == null)
			{
				_fallbackCornerIcon = emblem ?? AsyncTexture2D.FromAssetId(155150);
			}
			AsyncTexture2D helpTexture = null;
			try
			{
				helpTexture = AsyncTexture2D.op_Implicit(_contentsManager.GetTexture("match-overview-help.png"));
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Could not load match-overview-help.png");
			}
			_helpWindow = new MatchOverviewHelpWindow(windowBg, emblem, helpTexture);
			((Control)_helpWindow).Hide();
			_window = new PipTallyWindow(windowBg, emblem);
			_window.ScanClicked += OnScanClicked;
			_window.PrevChestClicked += delegate
			{
				NudgeSegment(-1);
			};
			_window.NextChestClicked += delegate
			{
				NudgeSegment(1);
			};
			_window.HelpClicked += delegate
			{
				ShowHelpExample();
			};
			((Control)_window).add_Moved((EventHandler<MovedEventArgs>)delegate
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				_windowLocation.set_Value(((Control)_window).get_Location());
				_hasSavedLocation.set_Value(true);
			});
			((Control)_window).add_Hidden((EventHandler<EventArgs>)delegate
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				_windowLocation.set_Value(((Control)_window).get_Location());
				_hasSavedLocation.set_Value(true);
				if (_showPanel.get_Value())
				{
					_showPanel.set_Value(false);
				}
			});
			WvWPipTallyModule wvWPipTallyModule = this;
			CornerIcon val = new CornerIcon();
			val.set_Icon(_fallbackCornerIcon);
			((Control)val).set_BasicTooltipText("WvW Pip Tally");
			val.set_Priority(1847291054);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			wvWPipTallyModule._cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_showPanel.set_Value(!_showPanel.get_Value());
			});
			if (_showPanel.get_Value())
			{
				((Control)_window).Show();
				RestoreWindowLocation();
			}
			else
			{
				((Control)_window).Hide();
			}
			RefreshPanel("Open Match Overview (B), then Scan.");
			Logger.Info("WvW Pip Tally loaded (StandardWindow + FromAssetId).");
			await Task.CompletedTask;
		}

		protected override void Unload()
		{
			if (_showPanel != null)
			{
				_showPanel.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowPanelChanged);
			}
			PipTallyWindow window = _window;
			if (window != null)
			{
				((Control)window).Dispose();
			}
			MatchOverviewHelpWindow helpWindow = _helpWindow;
			if (helpWindow != null)
			{
				((Control)helpWindow).Dispose();
			}
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			_window = null;
			_helpWindow = null;
			_cornerIcon = null;
		}

		private void OnShowPanelChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			if (_window != null)
			{
				if (e.get_NewValue())
				{
					((Control)_window).Show();
					RestoreWindowLocation();
				}
				else
				{
					_windowLocation.set_Value(((Control)_window).get_Location());
					_hasSavedLocation.set_Value(true);
					((Control)_window).Hide();
				}
			}
		}

		private void RestoreWindowLocation()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (_window != null && _hasSavedLocation.get_Value())
			{
				((Control)_window).set_Location(_windowLocation.get_Value());
			}
		}

		private void NudgeSegment(int delta)
		{
			int next = Math.Max(0, Math.Min(PipCalculator.ChestSegments.Length - 1, _segmentIndex.get_Value() + delta));
			_segmentIndex.set_Value(next);
			RefreshPanel("Chest adjusted manually.");
		}

		private void ShowHelpExample()
		{
			_helpWindow?.ShowCentered();
		}

		private async void OnScanClicked(object sender, EventArgs e)
		{
			if (_scanInProgress || _window == null)
			{
				return;
			}
			_scanInProgress = true;
			_window.SetScanBusy(busy: true);
			try
			{
				ScanResult result = await ScanService.ScanMatchOverviewAsync();
				if (result.Success && result.StartingSegmentIndex.HasValue)
				{
					_segmentIndex.set_Value(result.StartingSegmentIndex.Value);
					if (result.TickPrediction.HasValue && result.TickPrediction.Value > 0)
					{
						_scannedPipsPerTick.set_Value(result.TickPrediction.Value);
					}
					RefreshPanel(result.Message);
					ScreenNotification.ShowNotification(result.Message, (NotificationType)0, (Texture2D)null, 4);
					return;
				}
				string status = result.Message ?? "Match Overview not open. Press B, then Scan.";
				RefreshPanel(status);
				Logger.Warn("Scan failed: {0}\nOCR text:\n{1}", new object[2] { status, result.RawText });
				if (status.IndexOf("not open", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					ShowHelpExample();
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Scan crashed.");
				RefreshPanel("Scan failed: " + ex.Message);
			}
			finally
			{
				_window?.SetScanBusy(busy: false);
				_scanInProgress = false;
			}
		}

		private void RefreshPanel(string status)
		{
			if (_window != null)
			{
				PipInputs inputs = new PipInputs
				{
					StartingSegmentIndex = _segmentIndex.get_Value(),
					Placement = _placement.get_Value(),
					Rank = _rank.get_Value(),
					Commitment = _commitment.get_Value(),
					Commander = _commander.get_Value(),
					PublicCommander = _publicCommander.get_Value(),
					ScannedPipsPerTick = ((_scannedPipsPerTick.get_Value() > 0) ? new int?(_scannedPipsPerTick.get_Value()) : null)
				};
				PipResults results = PipCalculator.CalcResults(inputs);
				_window.Apply(results, status, inputs.StartingSegmentIndex);
				UpdateCornerIcon(inputs.StartingSegmentIndex);
			}
		}

		private void LoadChestIcons()
		{
			string[] array = new string[7] { "wood", "bronze", "silver", "gold", "platinum", "mithril", "diamond" };
			foreach (string division in array)
			{
				try
				{
					AsyncTexture2D tex = AsyncTexture2D.op_Implicit(_contentsManager.GetTexture("chests/" + division + "-closed.png"));
					if (tex != null)
					{
						_chestIcons[division] = tex;
					}
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Could not load chest icon {0}", new object[1] { division });
				}
			}
		}

		private void UpdateCornerIcon(int segmentIndex)
		{
			if (_cornerIcon != null)
			{
				string division = "wood";
				if (segmentIndex >= 0 && segmentIndex < PipCalculator.ChestSegments.Length)
				{
					division = PipCalculator.ChestSegments[segmentIndex].Division;
				}
				AsyncTexture2D icon = _fallbackCornerIcon;
				if (_chestIcons.TryGetValue(division, out var chestIcon) && chestIcon != null)
				{
					icon = chestIcon;
				}
				_cornerIcon.set_Icon(icon);
				((Control)_cornerIcon).set_BasicTooltipText("WvW Pip Tally — " + division);
			}
		}
	}
}
