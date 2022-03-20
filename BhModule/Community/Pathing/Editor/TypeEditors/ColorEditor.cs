using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.Editor.TypeEditors
{
	public class ColorEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.None;
		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Color color = (Color)e.Value;
			Color convertedColor = Color.FromArgb(((Color)(ref color)).get_A(), ((Color)(ref color)).get_R(), ((Color)(ref color)).get_G(), ((Color)(ref color)).get_B());
			e.Graphics.FillRectangle(new SolidBrush(convertedColor), e.Bounds);
		}
	}
}
