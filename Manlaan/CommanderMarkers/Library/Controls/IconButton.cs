using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Library.Controls
{
	public class IconButton : Image
	{
		private bool _checked;

		protected float _desiredOpacity = 1f;

		public bool Checked
		{
			get
			{
				return _checked;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _checked, value, false, "Checked");
			}
		}

		public Texture2D Icon
		{
			get
			{
				return AsyncTexture2D.op_Implicit(((Image)this).get_Texture());
			}
			set
			{
				((Image)this).set_Texture(AsyncTexture2D.op_Implicit(value));
			}
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			_desiredOpacity = ((Control)this).get_Opacity();
			((Control)this).set_Opacity(1f);
			((Control)this).OnMouseEntered(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).set_Opacity(_desiredOpacity);
			((Control)this).OnMouseLeft(e);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			_desiredOpacity = 1f;
			((Control)this).OnClick(e);
		}

		public IconButton()
			: this()
		{
		}
	}
}
