using System.Collections.Generic;
using BhModule.Community.Pathing.UI.Models;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.UI.Controls.TreeNodes
{
	public class LabelNode : PathingNode
	{
		public LabelNode(string text, AsyncTexture2D icon = null)
			: base(text)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			IconPaddingTop = 2;
			IconSize = new Point(25, 25);
			IconTextures = new List<PathingTexture>(1)
			{
				new PathingTexture
				{
					Icon = icon
				}
			};
			ShowIconTooltip = false;
			base.ShowBackground = true;
			base.PanelHeight = 30;
			base.Checkable = false;
			BackgroundOpacity = 0.05f;
			BackgroundOpaqueColor = Color.get_LightYellow();
			base.DoBuildContextMenu = false;
		}

		protected override void BuildContextMenu()
		{
		}
	}
}
