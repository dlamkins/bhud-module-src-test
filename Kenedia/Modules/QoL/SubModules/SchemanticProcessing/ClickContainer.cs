using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.QoL.SubModules.SchemanticProcessing
{
	public class ClickContainer : Panel
	{
		private bool _selected;

		public bool ShowCenter { get; set; } = true;


		public bool Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				Common.SetProperty(ref _selected, value, OnSelectedChanged);
			}
		}

		public Color CenterColor { get; private set; } = Colors.ColonialWhite * 0.5f;


		public Rectangle MaskedRegion { get; private set; }

		public ClickContainer()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			base.BorderColor = Colors.ColonialWhite * 0.5f;
			base.BorderWidth = new RectangleDimensions(1);
		}

		private void OnSelectedChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			Color value = (CenterColor = (Selected ? Color.get_Lime() : (Colors.ColonialWhite * 0.5f)));
			base.BorderColor = value;
		}

		public override void RecalculateLayout()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			MaskedRegion = new Rectangle(((Control)this).get_Location().X + base.BorderWidth.Left, ((Control)this).get_Location().Y + base.BorderWidth.Top, ((Control)this).get_Size().X - base.BorderWidth.Horizontal, ((Control)this).get_Size().Y - base.BorderWidth.Vertical);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			if (ShowCenter)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Control)this).get_Width() / 2 - 2, ((Control)this).get_Height() / 2 - 2, 4, 4), (Rectangle?)Textures.get_Pixel().get_Bounds(), CenterColor, 0f, default(Vector2), (SpriteEffects)0);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			((Container)this).UpdateContainer(gameTime);
			base.CaptureInput = (int)GameService.Input.get_Keyboard().get_ActiveModifiers() == 2;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			Selected = !Selected;
		}
	}
}
