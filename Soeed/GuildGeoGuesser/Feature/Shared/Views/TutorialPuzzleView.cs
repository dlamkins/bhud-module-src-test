using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Controls;
using Soeed.GuildGeoGuesser.Feature.Shared.Models;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Feature.Shared.Services;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Views
{
	public class TutorialPuzzleView : AccountRestrictedView
	{
		[CompilerGenerated]
		private GeoGuessWindowStateService _003CwindowState_003EP;

		private const int ImagePadding = 30;

		private Tutorial _tutorial;

		private AsyncTexture2D _texture;

		private Image _image;

		private Container? _buildPanel;

		private GuessSubmitRow _guessSubmitRow;

		private StandardButton _resetButton;

		private readonly List<StandardButton> _waypointButtons;

		private readonly List<StandardButton> _locationMarkerButtons;

		private readonly List<StandardButton> _scoringRingsButtons;

		private TutorialPuzzleBillboardOverlay? _puzzleBillboardOverlay;

		private TutorialScoringRingsOverlay? _scoringRingsOverlay;

		private TutorialWaypointMapOverlay? _waypointMapOverlay;

		private double _aspectRatio;

		private int _puzzleMapId;

		private bool _onPuzzleMap;

		public TutorialPuzzleView(GeoGuessWindowStateService windowState)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			_003CwindowState_003EP = windowState;
			_tutorial = _003CwindowState_003EP.TutorialModel;
			_texture = AsyncTexture2D.op_Implicit(Textures.get_Pixel());
			_image = new Image();
			_guessSubmitRow = new GuessSubmitRow();
			_resetButton = new StandardButton();
			_waypointButtons = new List<StandardButton>();
			_locationMarkerButtons = new List<StandardButton>();
			_scoringRingsButtons = new List<StandardButton>();
			_aspectRatio = 1.0;
			base._002Ector(showBackButton: true);
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			await base.Load(progress);
			progress.Report("Loading practice puzzle " + _tutorial.Title);
			Tutorial model = await Service.GeoServerWrapper.GetTutorialAsync(_tutorial.Id);
			if (model != null)
			{
				_tutorial = model;
				_003CwindowState_003EP.TutorialModel = model;
			}
			return true;
		}

		protected override void DoBuild(Container buildPanel)
		{
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Expected O, but got Unknown
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Expected O, but got Unknown
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_003CwindowState_003EP.SwapToGuildList();
			});
			_texture = _tutorial.GetImageTexture();
			if (_texture.get_Width() > 0 && _texture.get_Height() > 0)
			{
				_aspectRatio = (float)_texture.get_Width() / (float)_texture.get_Height();
			}
			((Panel)_flowPanel).set_CanScroll(true);
			_puzzleMapId = _tutorial.Location.MapId;
			_onPuzzleMap = IsOnPuzzleMap();
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnPuzzleMapChanged);
			BuildInstructions();
			bool isCompetitiveMode = GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode();
			if (!_tutorial.HasGuessed)
			{
				if (isCompetitiveMode)
				{
					_flowPanel.AddString("Cannot make guesses in competitive mode maps (PvP, WvW, etc.)", out var warningLabel);
					warningLabel.set_TextColor(Color.get_Orange());
					_flowPanel.AddSpace();
				}
				_flowPanel.AddControl<GuessSubmitRow>(_guessSubmitRow);
				_guessSubmitRow.GuessClicked += new EventHandler<MouseEventArgs>(MakeGuess);
				_guessSubmitRow.GuessAllowed = CanMakeGuess(isCompetitiveMode);
				((Control)_guessSubmitRow).set_BasicTooltipText(MakeGuessTooltip(isCompetitiveMode));
				Service.Settings.ProtectedGuessButton.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnProtectedGuessModeChanged);
				_flowPanel.AddString("You only get one guess. Use the waypoint for travel help, or show the location marker when you are close to the spot.");
				_flowPanel.AddSpace();
				_flowPanel.AddControl(new MumbleDistance(_tutorial.Location), out var distance2);
				((Control)distance2).set_Parent((Container)(object)_flowPanel);
			}
			else
			{
				_flowPanel.AddString("Your score: " + _tutorial.ScoreText(), out var scoreLabel);
				scoreLabel.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)2));
				scoreLabel.set_TextColor(Color.get_LightGreen());
				if (_tutorial.Location.MapId > 0)
				{
					_flowPanel.AddControl(new MumbleDistance(_tutorial.Location), out var distance);
					((Control)distance).set_Parent((Container)(object)_flowPanel);
				}
				FlowPanel flowPanel = _flowPanel;
				StandardButton val = new StandardButton();
				val.set_Text("Reset and try again");
				((Control)val).set_Width(220);
				((Control)val).set_Height(36);
				((Control)val).set_BasicTooltipText("Clear your guess and attempt this practice puzzle again");
				flowPanel.AddControl<StandardButton>(val, out _resetButton);
				((Control)_resetButton).add_Click((EventHandler<MouseEventArgs>)ResetAndTryAgain);
				_flowPanel.AddSpace();
			}
			FlowPanel flowPanel2 = _flowPanel;
			Image val2 = new Image();
			val2.set_Texture(_texture);
			((Control)val2).set_Size(new Point(((Control)buildPanel).get_Width() - 30, (int)((double)(((Control)buildPanel).get_Width() - 30) / _aspectRatio)));
			flowPanel2.AddControl<Image>(val2, out _image);
			_buildPanel = buildPanel;
			((Control)_buildPanel).add_Resized((EventHandler<ResizedEventArgs>)OnResized);
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)_controlsHeader);
			((Control)val3).set_Location(new Point(((Control)_backButton).get_Right() + 50, 0));
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Height(string.IsNullOrWhiteSpace(_tutorial.Author) ? 40 : 56);
			val3.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)2));
			val3.set_TextColor(Color.get_PaleGoldenrod());
			val3.set_Text(string.IsNullOrWhiteSpace(_tutorial.Author) ? _tutorial.Title : (_tutorial.Title + "\nby " + _tutorial.Author));
		}

		private void BuildInstructions()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			if (_tutorial.Instructions.Count == 0)
			{
				return;
			}
			_flowPanel.AddString("Instructions", out var header);
			header.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)2));
			header.set_TextColor(Color.get_PaleGoldenrod());
			foreach (TutorialInstructionStep step in _tutorial.Instructions)
			{
				if (step.IsHintType)
				{
					AddRevealableHint(step.Text);
				}
				else if (step.IsWaypointType)
				{
					if (HasWaypointHint())
					{
						AddWaypointButton();
					}
				}
				else if (step.IsLocationType)
				{
					if (HasPuzzleLocation())
					{
						AddLocationMarkerButton();
					}
				}
				else if (step.IsScoringRingsType)
				{
					if (HasPuzzleLocation())
					{
						AddScoringRingsButton();
					}
				}
				else if (!string.IsNullOrWhiteSpace(step.Text))
				{
					_flowPanel.AddString(step.Text);
				}
			}
			_flowPanel.AddSpace();
		}

		private void AddRevealableHint(string text)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			string text2 = text;
			if (!string.IsNullOrWhiteSpace(text2))
			{
				FlowPanel flowPanel = _flowPanel;
				FlowPanel val = new FlowPanel();
				((Container)val).set_WidthSizingMode((SizingMode)2);
				((Container)val).set_HeightSizingMode((SizingMode)1);
				val.set_FlowDirection((ControlFlowDirection)3);
				((Control)val).set_Width(((Control)_flowPanel).get_Width() - 20);
				flowPanel.AddControl<FlowPanel>(val, out FlowPanel container);
				StandardButton val2 = new StandardButton();
				((Control)val2).set_Parent((Container)(object)container);
				val2.set_Text("Click to reveal a hint");
				((Control)val2).set_Width(180);
				((Control)val2).set_Height(36);
				StandardButton revealButton = val2;
				((Control)revealButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					//IL_001d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0029: Unknown result type (might be due to invalid IL or missing references)
					//IL_0030: Unknown result type (might be due to invalid IL or missing references)
					//IL_0037: Unknown result type (might be due to invalid IL or missing references)
					((Control)revealButton).set_Visible(false);
					Label val3 = new Label();
					((Control)val3).set_Parent((Container)(object)container);
					val3.set_Text(text2);
					val3.set_AutoSizeWidth(true);
					val3.set_AutoSizeHeight(true);
					val3.set_WrapText(false);
					((Control)val3).Invalidate();
					((Control)container).Invalidate();
					((Control)_flowPanel).Invalidate();
				});
			}
		}

		private void AddWaypointButton()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			FlowPanel flowPanel = _flowPanel;
			StandardButton val = new StandardButton();
			val.set_Text(WaypointButtonText());
			((Control)val).set_Width(180);
			((Control)val).set_Height(36);
			((Control)val).set_BasicTooltipText("Show the nearest waypoint on your map and copy its chat code");
			flowPanel.AddControl<StandardButton>(val, out StandardButton button);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)ToggleWaypoint);
			_waypointButtons.Add(button);
			_flowPanel.AddSpace();
		}

		private void AddLocationMarkerButton()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			FlowPanel flowPanel = _flowPanel;
			StandardButton val = new StandardButton();
			val.set_Text(LocationMarkerButtonText());
			((Control)val).set_Width(220);
			((Control)val).set_Height(36);
			((Control)val).set_Enabled(CanShowLocationMarker());
			((Control)val).set_BasicTooltipText(LocationMarkerTooltip());
			flowPanel.AddControl<StandardButton>(val, out StandardButton button);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)ToggleLocationMarker);
			_locationMarkerButtons.Add(button);
			_flowPanel.AddSpace();
		}

		private void AddScoringRingsButton()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			FlowPanel flowPanel = _flowPanel;
			StandardButton val = new StandardButton();
			val.set_Text(ScoringRingsButtonText());
			((Control)val).set_Width(220);
			((Control)val).set_Height(36);
			((Control)val).set_Enabled(CanShowScoringRings());
			((Control)val).set_BasicTooltipText(ScoringRingsTooltip());
			flowPanel.AddControl<StandardButton>(val, out StandardButton button);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)ToggleScoringRings);
			_scoringRingsButtons.Add(button);
			_flowPanel.AddSpace();
		}

		private string WaypointButtonText()
		{
			if (_waypointMapOverlay == null)
			{
				return "Show waypoint";
			}
			return "Hide waypoint";
		}

		private string LocationMarkerButtonText()
		{
			if (_puzzleBillboardOverlay == null)
			{
				return "Show location marker";
			}
			return "Hide location marker";
		}

		private string ScoringRingsButtonText()
		{
			if (_scoringRingsOverlay == null)
			{
				return "Show scoring rings";
			}
			return "Hide scoring rings";
		}

		private void SyncWaypointButtonLabels()
		{
			string text = WaypointButtonText();
			foreach (StandardButton waypointButton in _waypointButtons)
			{
				waypointButton.set_Text(text);
			}
		}

		private void SyncLocationMarkerButtonLabels()
		{
			string text = LocationMarkerButtonText();
			foreach (StandardButton locationMarkerButton in _locationMarkerButtons)
			{
				locationMarkerButton.set_Text(text);
				((Control)locationMarkerButton).set_Enabled(CanShowLocationMarker());
				((Control)locationMarkerButton).set_BasicTooltipText(LocationMarkerTooltip());
			}
		}

		private void SyncScoringRingsButtonLabels()
		{
			string text = ScoringRingsButtonText();
			foreach (StandardButton scoringRingsButton in _scoringRingsButtons)
			{
				scoringRingsButton.set_Text(text);
				((Control)scoringRingsButton).set_Enabled(CanShowScoringRings());
				((Control)scoringRingsButton).set_BasicTooltipText(ScoringRingsTooltip());
			}
		}

		private bool IsOnPuzzleMap()
		{
			if (_puzzleMapId > 0 && GameService.Gw2Mumble.get_IsAvailable())
			{
				return GameService.Gw2Mumble.get_CurrentMap().get_Id() == _puzzleMapId;
			}
			return false;
		}

		private bool CanMakeGuess(bool isCompetitiveMode)
		{
			if (!isCompetitiveMode)
			{
				return _onPuzzleMap;
			}
			return false;
		}

		private string MakeGuessTooltip(bool isCompetitiveMode)
		{
			if (isCompetitiveMode)
			{
				return "Cannot make guesses in competitive mode maps (PvP, WvW, etc.)";
			}
			if (!_onPuzzleMap)
			{
				return "Travel to the puzzle map before making your guess";
			}
			if (Service.Settings.ProtectedGuessButton.get_Value())
			{
				return "Press and hold Ctrl+Shift to record your guess where you are currently standing";
			}
			return "Record your guess where you are currently standing";
		}

		private bool CanShowLocationMarker()
		{
			if (_puzzleBillboardOverlay == null)
			{
				return _onPuzzleMap;
			}
			return true;
		}

		private string LocationMarkerTooltip()
		{
			if (_puzzleBillboardOverlay != null)
			{
				return "Hide the billboard at the puzzle location";
			}
			if (!_onPuzzleMap)
			{
				return "Travel to the puzzle map before showing the location marker";
			}
			return "Show a billboard in the world at the puzzle location";
		}

		private bool CanShowScoringRings()
		{
			if (_scoringRingsOverlay == null)
			{
				return _onPuzzleMap;
			}
			return true;
		}

		private string ScoringRingsTooltip()
		{
			if (_scoringRingsOverlay != null)
			{
				return "Hide score threshold rings around the puzzle location";
			}
			if (!_onPuzzleMap)
			{
				return "Travel to the puzzle map before showing scoring rings";
			}
			return "Show colored rings for each score distance threshold";
		}

		private void OnPuzzleMapChanged(object sender, ValueEventArgs<int> e)
		{
			_onPuzzleMap = e.get_Value() == _puzzleMapId;
			SyncMapRestrictedControls();
		}

		private void SyncMapRestrictedControls()
		{
			if (!_tutorial.HasGuessed)
			{
				bool isCompetitiveMode = GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode();
				_guessSubmitRow.GuessAllowed = CanMakeGuess(isCompetitiveMode);
				((Control)_guessSubmitRow).set_BasicTooltipText(MakeGuessTooltip(isCompetitiveMode));
			}
			SyncLocationMarkerButtonLabels();
			SyncScoringRingsButtonLabels();
		}

		private void OnProtectedGuessModeChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (!_tutorial.HasGuessed)
			{
				bool isCompetitiveMode = GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode();
				((Control)_guessSubmitRow).set_BasicTooltipText(MakeGuessTooltip(isCompetitiveMode));
			}
		}

		private bool HasWaypointHint()
		{
			if (_tutorial.Hint != null)
			{
				return _tutorial.Hint!.Position.Length >= 3;
			}
			return false;
		}

		private bool HasPuzzleLocation()
		{
			return _tutorial.Location.MapId > 0;
		}

		private void ToggleLocationMarker(object sender, MouseEventArgs e)
		{
			if (HasPuzzleLocation() && (_puzzleBillboardOverlay != null || _onPuzzleMap))
			{
				if (_puzzleBillboardOverlay == null)
				{
					_puzzleBillboardOverlay = new TutorialPuzzleBillboardOverlay(_tutorial.Location);
					SyncLocationMarkerButtonLabels();
					ScreenNotification.ShowNotification("Puzzle marker active\nLook for the pulsing icon in the world.", (NotificationType)0, (Texture2D)null, 5);
				}
				else
				{
					HideLocationMarker();
				}
			}
		}

		private void ToggleScoringRings(object sender, MouseEventArgs e)
		{
			if (HasPuzzleLocation() && (_scoringRingsOverlay != null || _onPuzzleMap))
			{
				if (_scoringRingsOverlay == null)
				{
					_scoringRingsOverlay = new TutorialScoringRingsOverlay(_tutorial.Location);
					SyncScoringRingsButtonLabels();
					ScreenNotification.ShowNotification("Scoring rings active\nColored circles show each score distance threshold.", (NotificationType)0, (Texture2D)null, 5);
				}
				else
				{
					HideScoringRings();
				}
			}
		}

		private void ToggleWaypoint(object sender, MouseEventArgs e)
		{
			if (_tutorial.Hint != null)
			{
				if (_waypointMapOverlay == null)
				{
					_waypointMapOverlay = new TutorialWaypointMapOverlay(_tutorial.Hint);
					_waypointMapOverlay!.HiddenByProximity += new EventHandler(OnWaypointHiddenByProximity);
					_waypointMapOverlay!.CopyChatCodeToClipboard();
					SyncWaypointButtonLabels();
					ScreenNotification.ShowNotification((GameService.Gw2Mumble.get_IsAvailable() && GameService.Gw2Mumble.get_CurrentMap().get_Id() != _tutorial.Hint!.MapId) ? "Waypoint marker active\nopen the world map and pan to the puzzle zone to see it." : "Waypoint marker active\ncheck your compass/minimap for the green ring.", (NotificationType)0, (Texture2D)null, 5);
				}
				else
				{
					HideWaypoint();
				}
			}
		}

		private void OnWaypointHiddenByProximity(object? sender, EventArgs e)
		{
			if (_waypointMapOverlay != null)
			{
				_waypointMapOverlay!.HiddenByProximity -= new EventHandler(OnWaypointHiddenByProximity);
				_waypointMapOverlay!.Dispose();
				_waypointMapOverlay = null;
			}
			SyncWaypointButtonLabels();
		}

		private void ResetAndTryAgain(object sender, MouseEventArgs e)
		{
			((Control)_resetButton).set_Enabled(false);
			Task.Run(async delegate
			{
				Tutorial updated = await Service.GeoServerWrapper.ResetTutorialGuessAsync(_tutorial.Id);
				if (updated == null)
				{
					((Control)_resetButton).set_Enabled(true);
					ScreenNotification.ShowNotification("Could not reset this practice puzzle. Try again in a moment.", (NotificationType)1, (Texture2D)null, 4);
				}
				else
				{
					_tutorial = updated;
					_003CwindowState_003EP.TutorialModel = updated;
					if (((Control)Service.GeoGuessWindow).get_Visible())
					{
						Service.GeoGuessWindow.OpenWindowState();
					}
				}
			});
		}

		private void MakeGuess(object? sender, MouseEventArgs e)
		{
			if (GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode())
			{
				ScreenNotification.ShowNotification("Cannot make guesses in competitive mode maps (PvP, WvW, etc.)", (NotificationType)6, (Texture2D)null, 3);
				return;
			}
			if (!_onPuzzleMap)
			{
				ScreenNotification.ShowNotification("Travel to the puzzle map before making your guess.", (NotificationType)0, (Texture2D)null, 3);
				return;
			}
			_guessSubmitRow.SetSubmitting(submitting: true);
			HideAllHints();
			Task.Run(async delegate
			{
				Location guess = Location.SetFromMumble();
				if (await Service.GeoServerWrapper.SubmitTutorialGuessAsync(_tutorial.Id, guess, async delegate(Tutorial? result)
				{
					if (result == null)
					{
						_guessSubmitRow.SetSubmitting(submitting: false);
						_guessSubmitRow.GuessAllowed = CanMakeGuess(GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode());
					}
					else
					{
						_tutorial = result;
						_003CwindowState_003EP.TutorialModel = result;
						GeoServerWrapper geoServerWrapper = Service.GeoServerWrapper;
						Account? account = Service.UserManager.Account;
						UserCheckServerResponse progress = await geoServerWrapper.PerformUserCheck(((account != null) ? account!.get_Name() : null) ?? "");
						if (progress?.Tutorial != null)
						{
							Service.UserManager.SetTutorialState(progress.Tutorial);
						}
						if (Service.UserManager.TutorialState.PublicUnlocked)
						{
							ScreenNotification.ShowNotification((!Service.UserManager.TutorialState.PublicUnlocked) ? "Practice puzzle complete!\nPublic puzzles are now available." : "Practice puzzle complete!", (NotificationType)5, (Texture2D)null, 5);
							await Service.UserManager.RefreshGuildsAsync();
						}
					}
				}) != null)
				{
					ScoreModel scoreBand = guess.GetScoreBand(_tutorial.Location);
					if (scoreBand != null && scoreBand.Vfx)
					{
						TruePerfectionConfettiOverlay.RequestBurst(scoreBand);
					}
					if (((Control)Service.GeoGuessWindow).get_Visible())
					{
						Service.GeoGuessWindow.OpenWindowState();
					}
				}
			});
		}

		private void HideWaypoint()
		{
			if (_waypointMapOverlay != null)
			{
				_waypointMapOverlay!.HiddenByProximity -= new EventHandler(OnWaypointHiddenByProximity);
				_waypointMapOverlay!.Dispose();
				_waypointMapOverlay = null;
			}
			SyncWaypointButtonLabels();
		}

		private void HideLocationMarker()
		{
			_puzzleBillboardOverlay?.Dispose();
			_puzzleBillboardOverlay = null;
			SyncLocationMarkerButtonLabels();
		}

		private void HideScoringRings()
		{
			_scoringRingsOverlay?.Dispose();
			_scoringRingsOverlay = null;
			SyncScoringRingsButtonLabels();
		}

		private void HideAllHints()
		{
			HideWaypoint();
			HideLocationMarker();
			HideScoringRings();
		}

		private void OnResized(object sender, ResizedEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			((Control)_image).set_Size(new Point(e.get_CurrentSize().X - 30, (int)((double)(e.get_CurrentSize().X - 30) / _aspectRatio)));
		}

		protected override void Unload()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnPuzzleMapChanged);
			HideAllHints();
			_guessSubmitRow.GuessClicked -= new EventHandler<MouseEventArgs>(MakeGuess);
			Service.Settings.ProtectedGuessButton.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnProtectedGuessModeChanged);
			_guessSubmitRow.Dispose();
			((Control)_resetButton).remove_Click((EventHandler<MouseEventArgs>)ResetAndTryAgain);
			foreach (StandardButton locationMarkerButton in _locationMarkerButtons)
			{
				((Control)locationMarkerButton).remove_Click((EventHandler<MouseEventArgs>)ToggleLocationMarker);
			}
			foreach (StandardButton scoringRingsButton in _scoringRingsButtons)
			{
				((Control)scoringRingsButton).remove_Click((EventHandler<MouseEventArgs>)ToggleScoringRings);
			}
			foreach (StandardButton waypointButton in _waypointButtons)
			{
				((Control)waypointButton).remove_Click((EventHandler<MouseEventArgs>)ToggleWaypoint);
			}
			if (_buildPanel != null)
			{
				((Control)_buildPanel).remove_Resized((EventHandler<ResizedEventArgs>)OnResized);
			}
			base.Unload();
		}
	}
}
