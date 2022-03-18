using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Screenshot_Manager.UI.Controls
{
	public class ThumbnailBase : Container
	{
		private AsyncTexture2D _texture;

		public string FileName { get; set; }

		public ThumbnailBase(AsyncTexture2D texture, string fileName)
			: this()
		{
			_texture = texture;
			FileName = fileName;
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			((Control)this).OnMouseMoved(e);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
		}

		protected override void DisposeControl()
		{
			_texture.Dispose();
			((Container)this).DisposeControl();
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, new Color(44, 47, 51));
			if (_texture.get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _texture.get_Texture(), _texture.get_Texture().get_Bounds().Fit(bounds));
			}
			else
			{
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, bounds);
			}
		}
	}
}
