using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class GuessesListItem : Control, IDisposable
	{
		private readonly GuessUser _model;

		private readonly Location _sourceCoords;

		private readonly string _score;

		private readonly Color _accentColor;

		public GuessesListItem(GuessUser GuessModel, Location SourceCoords)
			: this()
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			_model = GuessModel;
			_sourceCoords = SourceCoords;
			((Control)this).set_Width(200);
			((Control)this).set_Height(50);
			_score = _model.PuzzleGuess.Location.Score(_sourceCoords);
			_accentColor = ScoreVisuals.AccentColorForGuess(_model.PuzzleGuess.Location, _sourceCoords);
		}

		protected override void DisposeControl()
		{
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, 200, 3), _accentColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 47, 200, 50), _accentColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, 3, 50), _accentColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(197, 0, 200, 50), _accentColor);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _model.AccountName ?? "", Control.get_Content().get_DefaultFont18(), new Rectangle(5, 5, 195, 20), Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _score ?? "", Control.get_Content().get_DefaultFont16(), new Rectangle(5, 25, 175, 20), _accentColor, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, $"{_model.PuzzleGuess.Distance}", Control.get_Content().get_DefaultFont12(), new Rectangle(5, 50, 175, 20), Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
