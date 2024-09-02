using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class ErrorWindow : StandardWindow
	{
		private const int WINDOW_WIDTH = 910;

		private const int WINDOW_HEIGHT = 300;

		public ErrorWindow(string windowTitle, string windowText)
			: this(Textures.get_TransparentPixel(), new Rectangle(0, 0, 910, 300), new Rectangle(30, 30, 880, 270))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).set_Title(windowTitle);
			((Control)this).set_BackgroundColor(new Color(Color.get_Black(), 0.8f));
			((Control)this).set_Location(new Point(50, 50));
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			FormattedLabel formattedLabel = new FormattedLabelBuilder().SetWidth(810).AutoSizeHeight().Wrap()
				.CreatePart(windowText, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.SetFontSize((FontSize)20);
				})
				.Build();
			((Control)formattedLabel).set_Parent((Container)(object)this);
			((Control)this).set_Height(((Control)formattedLabel).get_Height() + 100);
		}
	}
}
