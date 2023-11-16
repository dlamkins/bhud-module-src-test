namespace Blish_HUD.Controls
{
	internal class EmoteContainer : Container
	{
		private Image img;

		private Label label;

		private Label cooldownLabel;

		public EmoteContainer()
			: this()
		{
		}

		public void setImage(Image newImage)
		{
			img = newImage;
		}

		public Image getImage()
		{
			return img;
		}

		public void setLabel(Label newLabel)
		{
			label = newLabel;
		}

		public Label getLabel()
		{
			return label;
		}

		public void setCooldownLabel(Label newLabel)
		{
			cooldownLabel = newLabel;
		}

		public Label getCooldownLabel()
		{
			return cooldownLabel;
		}
	}
}
