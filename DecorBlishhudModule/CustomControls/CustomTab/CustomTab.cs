using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DecorBlishhudModule.CustomControls.CustomTab
{
	public class CustomTab
	{
		public AsyncTexture2D Icon { get; set; }

		public CustomLoadingSpinner Spinner { get; set; }

		public int OrderPriority { get; set; }

		public string Name { get; set; }

		public bool Enabled { get; set; } = true;


		public CustomTab(AsyncTexture2D icon, string name = null, int? priority = null)
		{
			Icon = icon;
			Name = name;
			OrderPriority = priority.GetValueOrDefault();
			Spinner = new CustomLoadingSpinner();
		}

		public void Draw(Control tabbedControl, SpriteBatch spriteBatch, Rectangle bounds, bool selected, bool hovered)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			if (Icon.get_HasTexture())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, tabbedControl, AsyncTexture2D.op_Implicit(Icon), new Rectangle(bounds.X + (bounds.Width - Icon.get_Texture().get_Width()) / 2, bounds.Y + (bounds.Height - Icon.get_Texture().get_Height()) / 2, Icon.get_Texture().get_Width(), Icon.get_Texture().get_Height()), (selected || hovered) ? Color.get_White() : Colors.DullColor);
			}
			if (!Enabled && Spinner != null)
			{
				Rectangle spinnerBounds = default(Rectangle);
				((Rectangle)(ref spinnerBounds))._002Ector(tabbedControl.get_Location().X + bounds.X + (bounds.Width - ((Control)Spinner).get_Size().X) / 2, tabbedControl.get_Location().Y + bounds.Y + (bounds.Height - ((Control)Spinner).get_Size().Y) / 2, ((Control)Spinner).get_Size().X, ((Control)Spinner).get_Size().Y);
				Spinner.Paint(spriteBatch, spinnerBounds);
			}
		}
	}
}
