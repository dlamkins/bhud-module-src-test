using System.Collections.Generic;
using Blish_HUD.Controls;

namespace EmoteTome
{
	internal class Emote
	{
		private string imagePath;

		private List<string> toolTipp;

		private string chatCode;

		private bool canTarget;

		private Image img = new Image();

		private string category;

		private bool deactivatedByTargeting;

		private bool deactivatedByLocked;

		private bool deactivatedByCooldown;

		public Emote(string imagePath, List<string> toolTipp, string chatCode, bool canTarget, string category)
		{
			this.imagePath = imagePath;
			this.toolTipp = toolTipp;
			this.chatCode = chatCode;
			this.canTarget = canTarget;
			this.category = category;
		}

		public string getImagePath()
		{
			return imagePath;
		}

		public List<string> getToolTipp()
		{
			return toolTipp;
		}

		public string getChatCode()
		{
			return chatCode;
		}

		public bool hasTarget()
		{
			return canTarget;
		}

		public void setImg(Image img)
		{
			this.img = img;
		}

		public Image getImg()
		{
			return img;
		}

		public string getCategory()
		{
			return category;
		}

		public bool isDeactivatedByTargeting()
		{
			return deactivatedByTargeting;
		}

		public void isDeactivatedByTargeting(bool newBool)
		{
			deactivatedByTargeting = newBool;
		}

		public bool isDeactivatedByLocked()
		{
			return deactivatedByLocked;
		}

		public void isDeactivatedByLocked(bool newBool)
		{
			deactivatedByLocked = newBool;
		}

		public bool isDeactivatedByCooldown()
		{
			return deactivatedByCooldown;
		}

		public void isDeactivatedByCooldown(bool newBool)
		{
			deactivatedByCooldown = newBool;
		}
	}
}
