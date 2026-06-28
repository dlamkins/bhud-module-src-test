using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Controls;
using Soeed.GuildGeoGuesser.Feature.Shared.Models;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Feature.Shared.Services;
using Soeed.GuildGeoGuesser.Settings.Controls;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Views
{
	public class GeoGuessDetailsView : AccountRestrictedView
	{
		[CompilerGenerated]
		private GeoGuessWindowStateService _003CWindowState_003EP;

		private const string COMPETITIVE_MODE_TOOLTIP = "Cannot make guesses in competitive mode maps (PvP, WvW, etc.)";

		protected readonly int IMAGE_PADDING;

		protected Image _image;

		protected Image _solutionImage;

		protected AsyncTexture2D _texture;

		protected MumbleDistance _mumbleDistance;

		protected Container? _buildPanel;

		protected GuessSubmitRow _guessSubmitRow;

		protected StandardButton _deleteGame;

		protected StandardButton _scoringRingsButton;

		protected TutorialScoringRingsOverlay? _scoringRingsOverlay;

		protected int _puzzleMapId;

		protected bool _onPuzzleMap;

		protected double _aspectRatio;

		protected Puzzle PuzzleModel { get; set; }

		public GeoGuessDetailsView(GeoGuessWindowStateService WindowState)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			_003CWindowState_003EP = WindowState;
			IMAGE_PADDING = 30;
			_image = new Image();
			_solutionImage = new Image();
			_texture = AsyncTexture2D.op_Implicit(Textures.get_Pixel());
			PuzzleModel = _003CWindowState_003EP.DetailsModel;
			_mumbleDistance = new MumbleDistance(new Location());
			_guessSubmitRow = new GuessSubmitRow();
			_deleteGame = new StandardButton();
			_scoringRingsButton = new StandardButton();
			_aspectRatio = 1.0;
			base._002Ector(showBackButton: true);
		}

		protected string GetByLineString()
		{
			if (PuzzleModel.ActualGuessCount == 0)
			{
				return "Be the first in your guild to guess this location";
			}
			return $"Make your guess where you are standing to compare against the {PuzzleModel.ActualGuessCount} others who have already guessed.";
		}

		protected override void DoBuild(Container buildPanel)
		{
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Expected O, but got Unknown
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_034b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Expected O, but got Unknown
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04be: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Expected O, but got Unknown
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Unknown result type (might be due to invalid IL or missing references)
			//IL_055a: Expected O, but got Unknown
			//IL_0610: Unknown result type (might be due to invalid IL or missing references)
			//IL_0641: Unknown result type (might be due to invalid IL or missing references)
			//IL_0646: Unknown result type (might be due to invalid IL or missing references)
			//IL_0652: Unknown result type (might be due to invalid IL or missing references)
			//IL_0661: Unknown result type (might be due to invalid IL or missing references)
			//IL_066b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0672: Unknown result type (might be due to invalid IL or missing references)
			//IL_067a: Unknown result type (might be due to invalid IL or missing references)
			//IL_068e: Unknown result type (might be due to invalid IL or missing references)
			//IL_068f: Unknown result type (might be due to invalid IL or missing references)
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)GoBackClickHandler);
			_texture = PuzzleModel.GetImageTexture();
			if (_texture.get_Width() > 0 && _texture.get_Height() > 0)
			{
				_aspectRatio = (float)_texture.get_Width() / (float)_texture.get_Height();
			}
			else
			{
				_aspectRatio = 1.0;
			}
			if (!_texture.get_HasSwapped())
			{
				_texture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnTextureLoaded);
			}
			((Panel)_flowPanel).set_CanScroll(true);
			_puzzleMapId = PuzzleModel.Location.MapId;
			_onPuzzleMap = IsOnPuzzleMap();
			if (GameService.Gw2Mumble.get_IsAvailable())
			{
				GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnPuzzleMapChanged);
			}
			bool isCompetitiveMode = GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode();
			if (!PuzzleModel.Location.IsFirstPersonCamera())
			{
				_flowPanel.AddString("This puzzle was created in Third-Person Camera mode", out var cameraModeLabel);
				cameraModeLabel.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)2));
				cameraModeLabel.set_TextColor(Color.get_Orange());
			}
			if (!PuzzleModel.UserHasGuessed(_003CWindowState_003EP.Account.get_Name()))
			{
				if (isCompetitiveMode)
				{
					_flowPanel.AddString("Cannot make guesses in competitive mode maps (PvP, WvW, etc.)", out var warningLabel);
					warningLabel.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)2));
					warningLabel.set_TextColor(Color.get_Orange());
					_flowPanel.AddSpace();
				}
				_guessSubmitRow.GuessClicked += new EventHandler<MouseEventArgs>(MakeGuess);
				_guessSubmitRow.GuessAllowed = !isCompetitiveMode;
				_guessSubmitRow.TooltipText = (isCompetitiveMode ? "Cannot make guesses in competitive mode maps (PvP, WvW, etc.)" : GuessTooltipText());
				_flowPanel.AddControl<GuessSubmitRow>(_guessSubmitRow);
				Service.Settings.ProtectedGuessButton.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnProtectedGuessModeChanged);
				_flowPanel.AddString("You only get ONE(1) guess. Be sure to make it count!").AddSpace().AddString(GetByLineString())
					.AddSpace();
			}
			else
			{
				FlowPanel val = new FlowPanel();
				val.set_FlowDirection((ControlFlowDirection)0);
				((Container)val).set_WidthSizingMode((SizingMode)2);
				((Container)val).set_HeightSizingMode((SizingMode)1);
				FlowPanel f = val;
				f.set_FlowDirection((ControlFlowDirection)0);
				f.set_ControlPadding(new Vector2(5f, 5f));
				_flowPanel.AddControl<PuzzleLikeControl>(new PuzzleLikeControl(PuzzleModel, _003CWindowState_003EP.Account.get_Name()));
				if (Service.Config.ShowSolutionMap)
				{
					bool isAuthor = PuzzleModel.IsAuthor(_003CWindowState_003EP.Account.get_Name());
					_flowPanel.AddControl<Image>((Image)(object)new PuzzleSolutionMap(PuzzleModel, isAuthor), out _solutionImage);
					int min = Math.Min(((Control)buildPanel).get_Width() - IMAGE_PADDING, 768);
					((Control)_solutionImage).set_Size(new Point(min, min));
					if (isAuthor)
					{
						StandardButton val2 = new StandardButton();
						val2.set_Text("Refresh Map");
						((Control)val2).set_BasicTooltipText("Refresh the solution map to show any new guesses");
						((Control)val2).set_Width(120);
						((Control)val2).set_Height(30);
						val2.set_Icon(Service.Textures.DatAsset(156736));
						StandardButton refreshMapButton = val2;
						((Control)refreshMapButton).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							(_solutionImage as PuzzleSolutionMap)?.RefreshMap(PuzzleModel, showAuthorView: true);
						});
						_flowPanel.AddControl<StandardButton>(refreshMapButton);
						_flowPanel.AddSpace();
					}
				}
				_flowPanel.AddControl(new MumbleDistance(PuzzleModel.Location), out _mumbleDistance);
				if (Service.Config.ShowScoreRingButton)
				{
					AddScoringRingsButton();
				}
				if (Service.Config.ShowScoreDistribution && PuzzleModel.Guesses.Count > 0)
				{
					_flowPanel.AddControl(new ScoreDistributionChart(PuzzleModel.Guesses, PuzzleModel.Location)).AddSpace();
				}
				_flowPanel.AddControl<FlowPanel>(f);
				if (PuzzleModel.Guesses.Count == 0)
				{
					f.AddString("No guesses yet. Share this GeoGuess with your guildmates!");
				}
				foreach (GuessUser guess in PuzzleModel.Guesses)
				{
					f.AddControl(new GuessesListItem(guess, _003CWindowState_003EP.DetailsModel.Location));
				}
				_flowPanel.AddSpace();
			}
			FlowPanel flowPanel = _flowPanel;
			Image val3 = new Image();
			val3.set_Texture(_texture);
			((Control)val3).set_Size(new Point(((Control)buildPanel).get_Width() - IMAGE_PADDING, (int)((double)(((Control)buildPanel).get_Width() - IMAGE_PADDING) / _aspectRatio)));
			flowPanel.AddControl<Image>(val3, out _image);
			if (PuzzleModel.IsAuthor(_003CWindowState_003EP.Account.get_Name()))
			{
				StandardButton val4 = new StandardButton();
				val4.set_Text("Edit Puzzle");
				((Control)val4).set_BasicTooltipText("Edit your puzzle and manage guesses");
				((Control)val4).set_Width(120);
				((Control)val4).set_Height(30);
				val4.set_Icon(Service.Textures.DatAsset(156736));
				StandardButton editButton = val4;
				((Control)editButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					_003CWindowState_003EP.SwapToEdit(PuzzleModel);
				});
				_flowPanel.AddControl<StandardButton>(editButton);
				_flowPanel.AddSpace();
				FlowPanel flowPanel2 = _flowPanel;
				NuclearOptionButton nuclearOptionButton = new NuclearOptionButton();
				((StandardButton)nuclearOptionButton).set_Text("Delete Puzzle");
				((Control)nuclearOptionButton).set_BasicTooltipText("Delete your puzzle, press and hold your keyboard CTRL and SHIFT keys and then click");
				((StandardButton)nuclearOptionButton).set_Icon(Service.Textures.DatAsset(1444524));
				flowPanel2.AddControl<StandardButton>((StandardButton)(object)nuclearOptionButton, out _deleteGame);
				((Control)_deleteGame).add_Click((EventHandler<MouseEventArgs>)Delete_Click);
			}
			_flowPanel.AddSpace(30);
			ReloadButton reloadButton = new ReloadButton();
			((Control)reloadButton).set_Parent((Container)(object)_controlsHeader);
			((Control)reloadButton).set_Location(new Point(((Control)_backButton).get_Right() + 5, 0));
			((Control)reloadButton).set_BasicTooltipText("Refresh the puzzle information");
			ReloadButton reload = reloadButton;
			((Control)reload).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)reload).set_Enabled(false);
				_003CWindowState_003EP.RefreshDetailsModel();
			});
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)_controlsHeader);
			((Control)val5).set_Location(new Point(((Control)reload).get_Right() + 5, 0));
			val5.set_AutoSizeWidth(true);
			((Control)val5).set_Height(40);
			val5.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)2));
			val5.set_TextColor(Color.get_PaleGoldenrod());
			val5.set_Text(PuzzleModel.AccountName + "'s " + PuzzleModel.Title);
			_buildPanel = buildPanel;
			((Control)_buildPanel).add_Resized((EventHandler<ResizedEventArgs>)BuildPanel_Resized);
		}

		private static string GuessTooltipText()
		{
			if (!Service.Settings.ProtectedGuessButton.get_Value())
			{
				return "Record your guess where you are currently standing";
			}
			return "Press and hold Ctrl+Shift, then click to record your guess where you are currently standing";
		}

		private void OnProtectedGuessModeChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (!PuzzleModel.UserHasGuessed(_003CWindowState_003EP.Account.get_Name()) && !GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode())
			{
				_guessSubmitRow.TooltipText = GuessTooltipText();
			}
		}

		private void AddScoringRingsButton()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			if (PuzzleModel.Location.MapId > 0)
			{
				FlowPanel flowPanel = _flowPanel;
				StandardButton val = new StandardButton();
				val.set_Text(ScoringRingsButtonText());
				((Control)val).set_Width(220);
				((Control)val).set_Height(36);
				((Control)val).set_Enabled(CanShowScoringRings());
				((Control)val).set_BasicTooltipText(ScoringRingsTooltip());
				flowPanel.AddControl<StandardButton>(val, out _scoringRingsButton);
				((Control)_scoringRingsButton).add_Click((EventHandler<MouseEventArgs>)ToggleScoringRings);
				_flowPanel.AddSpace();
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

		private string ScoringRingsButtonText()
		{
			if (_scoringRingsOverlay == null)
			{
				return "Show scoring rings";
			}
			return "Hide scoring rings";
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

		private void SyncScoringRingsButton()
		{
			_scoringRingsButton.set_Text(ScoringRingsButtonText());
			((Control)_scoringRingsButton).set_Enabled(CanShowScoringRings());
			((Control)_scoringRingsButton).set_BasicTooltipText(ScoringRingsTooltip());
		}

		private void OnPuzzleMapChanged(object sender, ValueEventArgs<int> e)
		{
			_onPuzzleMap = e.get_Value() == _puzzleMapId;
			SyncScoringRingsButton();
		}

		private void ToggleScoringRings(object sender, MouseEventArgs e)
		{
			if (PuzzleModel.Location.MapId > 0 && (_scoringRingsOverlay != null || _onPuzzleMap))
			{
				if (_scoringRingsOverlay == null)
				{
					_scoringRingsOverlay = new TutorialScoringRingsOverlay(PuzzleModel.Location);
					SyncScoringRingsButton();
					ScreenNotification.ShowNotification("Scoring rings active\nColored circles show each score distance threshold.", (NotificationType)0, (Texture2D)null, 5);
				}
				else
				{
					HideScoringRings();
				}
			}
		}

		private void HideScoringRings()
		{
			_scoringRingsOverlay?.Dispose();
			_scoringRingsOverlay = null;
			SyncScoringRingsButton();
		}

		private void Delete_Click(object sender, MouseEventArgs e)
		{
			((Control)_deleteGame).set_Visible(false);
			Task.Run(async delegate
			{
				await Service.GeoServerWrapper.DeleteGeoGame(_003CWindowState_003EP.SelectedGuild.Id.ToString(), _003CWindowState_003EP.DetailsModel.Id.ToString(), new Action<bool>(callback));
			});
			void callback(bool success)
			{
				if (success)
				{
					_003CWindowState_003EP.SwapToGuildList();
				}
				else
				{
					((Control)_deleteGame).set_Visible(true);
				}
			}
		}

		private void MakeGuess(object? sender, MouseEventArgs e)
		{
			if (GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode())
			{
				ScreenNotification.ShowNotification("Cannot make guesses in competitive mode maps (PvP, WvW, etc.)", (NotificationType)6, (Texture2D)null, 3);
				return;
			}
			_guessSubmitRow.SetSubmitting(submitting: true);
			Task.Run(async delegate
			{
				GuessCreate model = new GuessCreate
				{
					AccountName = _003CWindowState_003EP.Account.get_Name(),
					Location = Location.SetFromMumble()
				};
				await Service.GeoServerWrapper.SubmitGeoGuess(_003CWindowState_003EP.SelectedGuild.Id.ToString(), _003CWindowState_003EP.DetailsModel.Id.ToString(), model, new Action<Puzzle>(callback));
				void callback(Puzzle? updatedModel)
				{
					if (updatedModel != null)
					{
						ScoreModel scoreBand = model.Location.GetScoreBand(updatedModel!.Location);
						if (scoreBand != null && scoreBand.Vfx)
						{
							TruePerfectionConfettiOverlay.RequestBurst(scoreBand);
						}
						_003CWindowState_003EP.SelectModelForDetails(updatedModel);
					}
					else
					{
						_003CWindowState_003EP.SwapToGuildList();
					}
				}
			});
		}

		private void BuildPanel_Resized(object sender, ResizedEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			((Control)_image).set_Size(new Point(e.get_CurrentSize().X - IMAGE_PADDING, (int)((double)(e.get_CurrentSize().X - IMAGE_PADDING) / _aspectRatio)));
			int min = Math.Min(e.get_CurrentSize().X - IMAGE_PADDING, 768);
			((Control)_solutionImage).set_Size(new Point(min, min));
		}

		private void GoBackClickHandler(object sender, MouseEventArgs e)
		{
			Service.GeoGuessWindow.State.SwapToGuildList();
		}

		private void OnTextureLoaded(object sender, ValueChangedEventArgs<Texture2D> e)
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			if (e.get_NewValue().get_Width() > 0 && e.get_NewValue().get_Height() > 0)
			{
				_aspectRatio = (float)e.get_NewValue().get_Width() / (float)e.get_NewValue().get_Height();
				if (_image != null && _buildPanel != null)
				{
					((Control)_image).set_Size(new Point(((Control)_buildPanel).get_Width() - IMAGE_PADDING, (int)((double)(((Control)_buildPanel).get_Width() - IMAGE_PADDING) / _aspectRatio)));
				}
			}
		}

		protected override void Unload()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnPuzzleMapChanged);
			HideScoringRings();
			((Control)_scoringRingsButton).remove_Click((EventHandler<MouseEventArgs>)ToggleScoringRings);
			((Control)_deleteGame).remove_Click((EventHandler<MouseEventArgs>)Delete_Click);
			_guessSubmitRow.GuessClicked -= new EventHandler<MouseEventArgs>(MakeGuess);
			Service.Settings.ProtectedGuessButton.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnProtectedGuessModeChanged);
			_guessSubmitRow.Dispose();
			((Control)_backButton).remove_Click((EventHandler<MouseEventArgs>)GoBackClickHandler);
			_texture.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnTextureLoaded);
			MumbleDistance mumbleDistance = _mumbleDistance;
			if (mumbleDistance != null)
			{
				((Control)mumbleDistance).Dispose();
			}
			if (_buildPanel != null)
			{
				((Control)_buildPanel).remove_Resized((EventHandler<ResizedEventArgs>)BuildPanel_Resized);
			}
			base.Unload();
		}
	}
}
