using System;
using System.Collections.Generic;
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
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Views
{
	public class GeoGuessEditView : AccountRestrictedView
	{
		protected AsyncTexture2D _texture = AsyncTexture2D.op_Implicit(Textures.get_Pixel());

		protected FlowPanel _guessesPanel;

		protected double _aspectRatio;

		protected List<EditableGuessListItem> _guessItems;

		protected Puzzle PuzzleModel { get; set; } = windowState.DetailsModel;


		protected GeoGuessWindowStateService WindowState { get; set; }

		public GeoGuessEditView(GeoGuessWindowStateService windowState)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			WindowState = windowState;
			_guessesPanel = new FlowPanel();
			_aspectRatio = 1.0;
			_guessItems = new List<EditableGuessListItem>();
			base._002Ector(showBackButton: true);
		}

		protected override void DoBuild(Container buildPanel)
		{
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Expected O, but got Unknown
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Expected O, but got Unknown
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Expected O, but got Unknown
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Expected O, but got Unknown
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)GoBackClickHandler);
			if (!PuzzleModel.IsAuthor(WindowState.Account.get_Name()))
			{
				_flowPanel.AddString("You can only edit your own puzzles.");
				return;
			}
			_texture = PuzzleModel.GetImageTexture();
			_aspectRatio = (float)_texture.get_Width() / (float)_texture.get_Height();
			((Panel)_flowPanel).set_CanScroll(true);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_controlsHeader);
			((Control)val).set_Location(new Point(((Control)_backButton).get_Right() + 5, 0));
			val.set_AutoSizeWidth(true);
			((Control)val).set_Height(40);
			val.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)2));
			val.set_TextColor(Color.get_PaleGoldenrod());
			val.set_Text("Editing: " + PuzzleModel.Title);
			_flowPanel.AddString("Puzzle: " + PuzzleModel.Title).AddString("Created by: " + PuzzleModel.AccountName).AddString($"Total Guesses: {PuzzleModel.ActualGuessCount}")
				.AddString($"Upvotes: {PuzzleModel.Upvotes}")
				.AddSpace();
			_flowPanel.AddString("Edit Title:", out var titleLabel);
			titleLabel.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)2));
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)_flowPanel);
			val2.set_FlowDirection((ControlFlowDirection)0);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_ControlPadding(new Vector2(5f, 5f));
			FlowPanel titleEditPanel = val2;
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent((Container)(object)titleEditPanel);
			((Control)val3).set_Width(300);
			((TextInputBase)val3).set_Text(PuzzleModel.Title);
			TextBox titleTextBox = val3;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)titleEditPanel);
			val4.set_Text("Save Title");
			((Control)val4).set_Enabled(false);
			StandardButton saveButton = val4;
			((TextInputBase)titleTextBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				((Control)saveButton).set_Enabled(((TextInputBase)titleTextBox).get_Text() != PuzzleModel.Title && !string.IsNullOrWhiteSpace(((TextInputBase)titleTextBox).get_Text()));
			});
			((Control)saveButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				try
				{
					((Control)saveButton).set_Enabled(false);
					ScreenNotification.ShowNotification("Updating title...", (NotificationType)0, (Texture2D)null, 2);
					await Service.GeoServerWrapper.UpdatePuzzleTitle(PuzzleModel.Id, ((TextInputBase)titleTextBox).get_Text(), WindowState.Account.get_Name(), delegate(Puzzle? updatedPuzzle)
					{
						if (updatedPuzzle != null)
						{
							PuzzleModel = updatedPuzzle;
							WindowState.DetailsModel = updatedPuzzle;
							ScreenNotification.ShowNotification("Title updated successfully", (NotificationType)5, (Texture2D)null, 3);
						}
						else
						{
							ScreenNotification.ShowNotification("Failed to update title", (NotificationType)6, (Texture2D)null, 3);
							((TextInputBase)titleTextBox).set_Text(PuzzleModel.Title);
						}
					});
				}
				catch (Exception ex)
				{
					Logger.GetLogger<Module>().Warn(ex, "Error updating title");
					ScreenNotification.ShowNotification("Error updating title", (NotificationType)6, (Texture2D)null, 3);
					((TextInputBase)titleTextBox).set_Text(PuzzleModel.Title);
				}
				finally
				{
					((Control)saveButton).set_Enabled(true);
				}
			});
			_flowPanel.AddSpace();
			_flowPanel.AddString("Hold Ctrl+Shift and click 'Remove' next to any guess to remove it from your puzzle.").AddString("Warning: Removed guesses cannot be restored!", out var warningLabel).AddSpace();
			warningLabel.set_TextColor(Color.get_Red());
			if (PuzzleModel.Guesses.Count == 0)
			{
				_flowPanel.AddString("No guesses yet.");
			}
			else
			{
				_flowPanel.AddString($"Guesses ({PuzzleModel.Guesses.Count}):");
				FlowPanel val5 = new FlowPanel();
				val5.set_FlowDirection((ControlFlowDirection)0);
				((Container)val5).set_WidthSizingMode((SizingMode)2);
				((Container)val5).set_HeightSizingMode((SizingMode)1);
				val5.set_ControlPadding(new Vector2(5f, 5f));
				_guessesPanel = val5;
				_flowPanel.AddControl<FlowPanel>(_guessesPanel);
				RefreshGuessesList();
			}
			_flowPanel.AddSpace();
		}

		private void RefreshGuessesList()
		{
			foreach (EditableGuessListItem guessItem2 in _guessItems)
			{
				guessItem2.RemoveRequested -= new EventHandler<GuessUser>(OnRemoveGuessRequested);
				((Control)guessItem2).Dispose();
			}
			_guessItems.Clear();
			((Container)_guessesPanel).ClearChildren();
			foreach (GuessUser guess in PuzzleModel.Guesses)
			{
				EditableGuessListItem editableGuessListItem = new EditableGuessListItem(guess, PuzzleModel.Location);
				((Control)editableGuessListItem).set_Parent((Container)(object)_guessesPanel);
				EditableGuessListItem guessItem = editableGuessListItem;
				guessItem.RemoveRequested += new EventHandler<GuessUser>(OnRemoveGuessRequested);
				_guessItems.Add(guessItem);
			}
		}

		private async void OnRemoveGuessRequested(object sender, GuessUser guessToRemove)
		{
			await RemoveGuess(guessToRemove);
		}

		private async Task RemoveGuess(GuessUser guessToRemove)
		{
			GuessUser guessToRemove2 = guessToRemove;
			try
			{
				ScreenNotification.ShowNotification("Removing guess...", (NotificationType)0, (Texture2D)null, 2);
				await Service.GeoServerWrapper.RemoveGuessFromPuzzle(PuzzleModel.Id, guessToRemove2.AccountName, WindowState.Account.get_Name(), delegate(Puzzle? updatedPuzzle)
				{
					if (updatedPuzzle != null)
					{
						PuzzleModel = updatedPuzzle;
						WindowState.DetailsModel = updatedPuzzle;
						RefreshGuessesList();
						ScreenNotification.ShowNotification("Removed " + guessToRemove2.AccountName + "'s guess", (NotificationType)5, (Texture2D)null, 3);
					}
					else
					{
						ScreenNotification.ShowNotification("Failed to remove guess", (NotificationType)6, (Texture2D)null, 3);
					}
				});
			}
			catch (Exception ex)
			{
				Logger.GetLogger<Module>().Warn(ex, "Error removing guess");
				ScreenNotification.ShowNotification("Error removing guess", (NotificationType)6, (Texture2D)null, 3);
			}
		}

		private void GoBackClickHandler(object sender, MouseEventArgs e)
		{
			WindowState.SelectModelForDetails(PuzzleModel);
		}

		protected override void Unload()
		{
			((Control)_backButton).remove_Click((EventHandler<MouseEventArgs>)GoBackClickHandler);
			foreach (EditableGuessListItem guessItem in _guessItems)
			{
				guessItem.RemoveRequested -= new EventHandler<GuessUser>(OnRemoveGuessRequested);
			}
			base.Unload();
		}
	}
}
