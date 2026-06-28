using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class TutorialListItem : DetailsButton
	{
		private readonly Tutorial _model;

		private readonly int _iconSize = 140;

		private AsyncTexture2D _tutorialIcon = AsyncTexture2D.op_Implicit(Textures.get_Pixel());

		public TutorialListItem(Tutorial model)
			: this()
		{
			_model = model;
			((Control)this).set_Width(400);
			((Control)this).set_Height(150);
			((DetailsButton)this).set_Text(BuildDescription());
			BuildIcon();
			((Control)this).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Service.GeoGuessWindow.State.SelectTutorialForDetails(_model);
			});
		}

		private string BuildDescription()
		{
			string status = (_model.HasGuessed ? ("Completed — " + _model.ScoreText()) : "Not completed");
			string authorLine = (string.IsNullOrWhiteSpace(_model.Author) ? string.Empty : ("Created by: " + _model.Author + "\n"));
			return _model.Title + "\n" + authorLine + status;
		}

		private void BuildIcon()
		{
			_tutorialIcon = _model.GetImageTexture();
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			((DetailsButton)this).PaintBeforeChildren(spriteBatch, bounds);
			if (_tutorialIcon.get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_tutorialIcon), new Rectangle(5, 5, _iconSize, _iconSize));
			}
			if (_model.HasGuessed)
			{
				AsyncTexture2D check = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156950);
				if (check.get_HasTexture())
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(check), new Rectangle(bounds.Width - 35, 8, 24, 24));
				}
			}
		}
	}
}
