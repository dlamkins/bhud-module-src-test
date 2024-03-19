using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.ChatMacros.Core.UI
{
	public class RoundedImage : Control
	{
		private Effect _curvedBorder;

		private AsyncTexture2D _texture;

		private SpriteBatchParameters _defaultParams;

		private SpriteBatchParameters _curvedBorderParams;

		private float _radius = 0.215f;

		private Tween _tween;

		private Color _color;

		public Color Color
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _color;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _color, value, false, "Color");
			}
		}

		public RoundedImage(AsyncTexture2D texture)
			: this()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			_defaultParams = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			_curvedBorder = ChatMacros.Instance.ContentsManager.GetEffect<Effect>("effects\\curvedborder.mgfx");
			SpriteBatchParameters val = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			val.set_Effect(_curvedBorder);
			_curvedBorderParams = val;
			_texture = texture;
			_curvedBorder.get_Parameters().get_Item("Smooth").SetValue(false);
		}

		protected override void DisposeControl()
		{
			((GraphicsResource)_curvedBorder).Dispose();
			((Control)this).DisposeControl();
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			Tween tween = _tween;
			if (tween != null)
			{
				tween.Cancel();
			}
			_tween = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<RoundedImage>(this, (object)new
			{
				_radius = 0.315f
			}, 0.1f, 0f, true);
			((Control)this).OnMouseEntered(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			Tween tween = _tween;
			if (tween != null)
			{
				tween.Cancel();
			}
			_tween = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<RoundedImage>(this, (object)new
			{
				_radius = 0.215f
			}, 0.1f, 0f, true);
			((Control)this).OnMouseLeft(e);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			_curvedBorder.get_Parameters().get_Item("Radius").SetValue(_radius);
			_curvedBorder.get_Parameters().get_Item("Opacity").SetValue(((Control)this).get_Opacity());
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, _curvedBorderParams);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_texture), new Rectangle(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height()), _color);
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, _defaultParams);
		}
	}
}
