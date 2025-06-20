using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Settings.Controls;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class EditableGuessListItem : Container, IDisposable
	{
		private readonly GuessUser _model;

		private readonly Location _sourceCoords;

		private readonly string _score;

		private readonly NuclearOptionButton _removeButton;

		public event EventHandler<GuessUser> RemoveRequested;

		public EditableGuessListItem(GuessUser guessModel, Location sourceCoords)
			: this()
		{
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			_model = guessModel;
			_sourceCoords = sourceCoords;
			((Control)this).set_Width(250);
			((Control)this).set_Height(50);
			_score = _model.PuzzleGuess.Location.Score(_sourceCoords);
			NuclearOptionButton nuclearOptionButton = new NuclearOptionButton();
			((Control)nuclearOptionButton).set_Parent((Container)(object)this);
			((StandardButton)nuclearOptionButton).set_Text("Remove");
			((Control)nuclearOptionButton).set_Width(60);
			((Control)nuclearOptionButton).set_Height(20);
			((Control)nuclearOptionButton).set_Location(new Point(180, 25));
			((Control)nuclearOptionButton).set_BasicTooltipText("Remove this user's guess (hold Ctrl+Shift and click)");
			_removeButton = nuclearOptionButton;
			((Control)_removeButton).add_Click((EventHandler<MouseEventArgs>)OnRemoveClick);
		}

		private void OnRemoveClick(object sender, MouseEventArgs e)
		{
			this.RemoveRequested?.Invoke(this, _model);
		}

		protected override void DisposeControl()
		{
			((Control)_removeButton).remove_Click((EventHandler<MouseEventArgs>)OnRemoveClick);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, 250, 3), Color.get_LightGoldenrodYellow());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 47, 250, 50), Color.get_LightGoldenrodYellow());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, 3, 50), Color.get_LightGoldenrodYellow());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(247, 0, 250, 50), Color.get_LightGoldenrodYellow());
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _model.AccountName ?? "", Control.get_Content().get_DefaultFont18(), new Rectangle(5, 5, 170, 20), Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _score ?? "", Control.get_Content().get_DefaultFont16(), new Rectangle(5, 25, 170, 20), Color.get_LightGoldenrodYellow(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
