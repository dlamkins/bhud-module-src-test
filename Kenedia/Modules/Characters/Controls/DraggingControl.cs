using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters.Controls
{
	public class DraggingControl : Control
	{
		private CharacterCard _characterControl;

		public CharacterCard CharacterControl
		{
			get
			{
				return _characterControl;
			}
			set
			{
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				_characterControl = value;
				((Control)this).set_Visible(value != null);
				if (((Control)this).get_Visible())
				{
					((Control)this).set_Size(((Control)value).get_Size());
					((Control)this).set_BackgroundColor(new Color(0, 0, 0, 175));
				}
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).DoUpdate(gameTime);
			if (CharacterControl != null)
			{
				MouseHandler i = Control.get_Input().get_Mouse();
				((Control)this).set_Location(new Point(i.get_Position().X - 15, i.get_Position().Y - 15));
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Visible())
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, CharacterControl.Character.Name, CharacterControl.NameFont, bounds, new Color(208, 188, 142, 255), false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
		}

		public DraggingControl()
			: this()
		{
		}
	}
}
