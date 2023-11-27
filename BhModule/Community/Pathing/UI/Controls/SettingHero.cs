using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.UI.Controls
{
	internal class SettingHero : Control
	{
		private AsyncTexture2D _backgroundTexture = AsyncTexture2D.FromAssetId(156353);

		private AsyncTexture2D _icon;

		private Rectangle _rectIcon = Rectangle.get_Empty();

		private Rectangle _rectText = Rectangle.get_Empty();

		public AsyncTexture2D Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				((Control)this).SetProperty<AsyncTexture2D>(ref _icon, value, true, "Icon");
			}
		}

		public string Text { get; set; }

		public override void RecalculateLayout()
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			if (_icon != null && _icon.get_HasTexture())
			{
				_rectIcon = new Rectangle(((Control)this).get_Width() / 2 - _icon.get_Width() / 2, 20, _icon.get_Width(), _icon.get_Height());
			}
			_rectText = new Rectangle(0, ((Control)this).get_Height() - 60, ((Control)this).get_Width(), 40);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			if (_backgroundTexture.get_HasTexture() && ((Control)this).get_MouseOver())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_backgroundTexture), new Rectangle(0, ((Control)this).get_Height() - 256, _backgroundTexture.get_Width(), _backgroundTexture.get_Height()), (Rectangle?)null, Color.get_Black(), 0f, Vector2.get_Zero(), (SpriteEffects)2);
			}
			if (_icon != null && _icon.get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon), _rectIcon);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Text, GameService.Content.get_DefaultFont18(), _rectText, ((Control)this).get_MouseOver() ? Color.get_White() : StandardColors.get_Tinted(), false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
		}

		public SettingHero()
			: this()
		{
		}//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)

	}
}
