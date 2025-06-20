using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class GuessesListItem : Control, IDisposable
	{
		private readonly GuessUser _model;

		private readonly Location _sourceCoords;

		private readonly string _score;

		public GuessesListItem(GuessUser GuessModel, Location SourceCoords)
			: this()
		{
			_model = GuessModel;
			_sourceCoords = SourceCoords;
			((Control)this).set_Width(200);
			((Control)this).set_Height(50);
			_score = _model.PuzzleGuess.Location.Score(_sourceCoords);
		}

		protected override void DisposeControl()
		{
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
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
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, 200, 3), Color.get_LightGoldenrodYellow());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 47, 200, 50), Color.get_LightGoldenrodYellow());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, 3, 50), Color.get_LightGoldenrodYellow());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(197, 0, 200, 50), Color.get_LightGoldenrodYellow());
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _model.AccountName ?? "", Control.get_Content().get_DefaultFont18(), new Rectangle(5, 5, 195, 20), Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _score ?? "", Control.get_Content().get_DefaultFont16(), new Rectangle(5, 25, 175, 20), Color.get_LightGoldenrodYellow(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, $"{_model.PuzzleGuess.Distance}", Control.get_Content().get_DefaultFont12(), new Rectangle(5, 50, 175, 20), Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
