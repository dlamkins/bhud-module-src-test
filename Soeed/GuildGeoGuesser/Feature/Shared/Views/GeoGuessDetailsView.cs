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

		protected readonly int IMAGE_PADDING;

		protected Image _image;

		protected Image _solutionImage;

		protected AsyncTexture2D _texture;

		protected MumbleDistance _mumbleDistance;

		protected Container? _buildPanel;

		protected StandardButton _makeGuess;

		protected StandardButton _deleteGame;

		protected double _aspectRatio;

		protected Puzzle PuzzleModel { get; set; }

		public GeoGuessDetailsView(GeoGuessWindowStateService WindowState)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			_003CWindowState_003EP = WindowState;
			IMAGE_PADDING = 30;
			_image = new Image();
			_solutionImage = new Image();
			_texture = AsyncTexture2D.op_Implicit(Textures.get_Pixel());
			PuzzleModel = _003CWindowState_003EP.DetailsModel;
			_mumbleDistance = new MumbleDistance(new Location());
			_makeGuess = new StandardButton();
			_deleteGame = new StandardButton();
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
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Expected O, but got Unknown
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Expected O, but got Unknown
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Expected O, but got Unknown
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0443: Unknown result type (might be due to invalid IL or missing references)
			//IL_044f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0473: Unknown result type (might be due to invalid IL or missing references)
			//IL_0488: Expected O, but got Unknown
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Expected O, but got Unknown
			//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0603: Unknown result type (might be due to invalid IL or missing references)
			//IL_060b: Unknown result type (might be due to invalid IL or missing references)
			//IL_061f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0620: Unknown result type (might be due to invalid IL or missing references)
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
				FlowPanel flowPanel = _flowPanel;
				StandardButton val = new StandardButton();
				((Control)val).set_Parent((Container)(object)_controlsHeader);
				((Control)val).set_Height(40);
				((Control)val).set_Width(300);
				val.set_Text("Make my guess here");
				((Control)val).set_BasicTooltipText(isCompetitiveMode ? "Cannot make guesses in competitive mode maps (PvP, WvW, etc.)" : "Record your guess where you are currently standing");
				((Control)val).set_Enabled(!isCompetitiveMode);
				flowPanel.AddControl<StandardButton>(val, out _makeGuess).AddString("You only get ONE(1) guess. Be sure to make it count!").AddSpace()
					.AddString(GetByLineString())
					.AddSpace();
			}
			else
			{
				FlowPanel val2 = new FlowPanel();
				val2.set_FlowDirection((ControlFlowDirection)0);
				((Container)val2).set_WidthSizingMode((SizingMode)2);
				((Container)val2).set_HeightSizingMode((SizingMode)1);
				FlowPanel f = val2;
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
						StandardButton val3 = new StandardButton();
						val3.set_Text("Refresh Map");
						((Control)val3).set_BasicTooltipText("Refresh the solution map to show any new guesses");
						((Control)val3).set_Width(120);
						((Control)val3).set_Height(30);
						val3.set_Icon(Service.Textures.DatAsset(156736));
						StandardButton refreshMapButton = val3;
						((Control)refreshMapButton).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							(_solutionImage as PuzzleSolutionMap)?.RefreshMap(PuzzleModel, showAuthorView: true);
						});
						_flowPanel.AddControl<StandardButton>(refreshMapButton);
						_flowPanel.AddSpace();
					}
				}
				_flowPanel.AddControl(new MumbleDistance(PuzzleModel.Location), out _mumbleDistance);
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
			FlowPanel flowPanel2 = _flowPanel;
			Image val4 = new Image();
			val4.set_Texture(_texture);
			((Control)val4).set_Size(new Point(((Control)buildPanel).get_Width() - IMAGE_PADDING, (int)((double)(((Control)buildPanel).get_Width() - IMAGE_PADDING) / _aspectRatio)));
			flowPanel2.AddControl<Image>(val4, out _image);
			if (PuzzleModel.IsAuthor(_003CWindowState_003EP.Account.get_Name()))
			{
				StandardButton val5 = new StandardButton();
				val5.set_Text("Edit Puzzle");
				((Control)val5).set_BasicTooltipText("Edit your puzzle and manage guesses");
				((Control)val5).set_Width(120);
				((Control)val5).set_Height(30);
				val5.set_Icon(Service.Textures.DatAsset(156736));
				StandardButton editButton = val5;
				((Control)editButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					_003CWindowState_003EP.SwapToEdit(PuzzleModel);
				});
				_flowPanel.AddControl<StandardButton>(editButton);
				_flowPanel.AddSpace();
				FlowPanel flowPanel3 = _flowPanel;
				NuclearOptionButton nuclearOptionButton = new NuclearOptionButton();
				((StandardButton)nuclearOptionButton).set_Text("Delete Puzzle");
				((Control)nuclearOptionButton).set_BasicTooltipText("Delete your puzzle, press and hold your keyboard CTRL and SHIFT keys and then click");
				((StandardButton)nuclearOptionButton).set_Icon(Service.Textures.DatAsset(1444524));
				flowPanel3.AddControl<StandardButton>((StandardButton)(object)nuclearOptionButton, out _deleteGame);
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
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)_controlsHeader);
			((Control)val6).set_Location(new Point(((Control)reload).get_Right() + 5, 0));
			val6.set_AutoSizeWidth(true);
			((Control)val6).set_Height(40);
			val6.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)2));
			val6.set_TextColor(Color.get_PaleGoldenrod());
			val6.set_Text(PuzzleModel.AccountName + "'s " + PuzzleModel.Title);
			_buildPanel = buildPanel;
			((Control)_buildPanel).add_Resized((EventHandler<ResizedEventArgs>)BuildPanel_Resized);
			((Control)_makeGuess).add_Click((EventHandler<MouseEventArgs>)MakeGuess);
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

		private void MakeGuess(object sender, MouseEventArgs e)
		{
			if (GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode())
			{
				ScreenNotification.ShowNotification("Cannot make guesses in competitive mode maps (PvP, WvW, etc.)", (NotificationType)6, (Texture2D)null, 3);
				return;
			}
			((Control)_makeGuess).set_Enabled(false);
			Task.Run(async delegate
			{
				GuessCreate model = new GuessCreate
				{
					AccountName = _003CWindowState_003EP.Account.get_Name(),
					Location = Location.SetFromMumble()
				};
				await Service.GeoServerWrapper.SubmitGeoGuess(_003CWindowState_003EP.SelectedGuild.Id.ToString(), _003CWindowState_003EP.DetailsModel.Id.ToString(), model, new Action<Puzzle>(callback));
			});
			void callback(Puzzle? updatedModel)
			{
				if (updatedModel != null)
				{
					_003CWindowState_003EP.SelectModelForDetails(updatedModel);
				}
				else
				{
					_003CWindowState_003EP.SwapToGuildList();
				}
			}
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
			((Control)_deleteGame).remove_Click((EventHandler<MouseEventArgs>)Delete_Click);
			((Control)_makeGuess).remove_Click((EventHandler<MouseEventArgs>)MakeGuess);
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
