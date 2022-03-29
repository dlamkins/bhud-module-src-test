using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Nekres.Inquest_Module.UI.Controls
{
	internal sealed class TaskIndicator : Control
	{
		private static Texture2D _stopIcon = GameService.Content.GetTexture("common/154982");

		private static Texture2D _mouseIcon = GameService.Content.GetTexture("156734");

		private static BitmapFont _font = GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0);

		private bool _paused;

		private string _text;

		private Color _textColor;

		private Point _mousePos => GameService.Input.get_Mouse().get_Position();

		public bool Paused
		{
			get
			{
				return _paused;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _paused, value, false, "Paused");
			}
		}

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _text, value, false, "Text");
			}
		}

		public Color TextColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _textColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _textColor, value, false, "TextColor");
			}
		}

		public TaskIndicator()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			_textColor = Color.get_White();
			((Control)this).set_ZIndex(1000);
			Update();
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)1;
		}

		private void Update()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(new Point(_mousePos.X + ((Control)this).get_Width() / 2, _mousePos.Y - ((Control)this).get_Height() / 2));
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			Update();
			if (_paused)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _mouseIcon, new Rectangle((bounds.Width - _mouseIcon.get_Width()) / 2, (bounds.Height - _mouseIcon.get_Height()) / 2, _mouseIcon.get_Width(), _mouseIcon.get_Height()), (Rectangle?)_mouseIcon.get_Bounds());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _stopIcon, new Rectangle((bounds.Width - _stopIcon.get_Width()) / 2, (bounds.Height - _stopIcon.get_Height()) / 2, _stopIcon.get_Width(), _stopIcon.get_Height()), (Rectangle?)_stopIcon.get_Bounds(), Color.get_White() * 0.65f);
			}
			else
			{
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, bounds);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _text, _font, bounds, _textColor, false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
		}
	}
}
