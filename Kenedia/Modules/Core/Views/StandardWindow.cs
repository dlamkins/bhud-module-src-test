using System;
using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Views
{
	public class StandardWindow : StandardWindow
	{
		private readonly List<AnchoredContainer> _attachedContainers = new List<AnchoredContainer>();

		public bool IsActive => WindowBase2.get_ActiveWindow() == this;

		public StandardWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(background, windowRegion, contentRegion)
		{
		}//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)


		public override void UpdateContainer(GameTime gameTime)
		{
			((WindowBase2)this).UpdateContainer(gameTime);
			foreach (AnchoredContainer container in _attachedContainers)
			{
				if (((Control)container).get_ZIndex() != ((Control)this).get_ZIndex())
				{
					((Control)container).set_ZIndex(((Control)this).get_ZIndex());
				}
			}
		}

		public void ShowAttached(AnchoredContainer container = null)
		{
			foreach (AnchoredContainer c in _attachedContainers)
			{
				if (container != c && ((Control)c).get_Visible())
				{
					((Control)c).Hide();
				}
			}
			if (container != null)
			{
				((Control)container).Show();
			}
		}

		protected virtual void AttachContainer(AnchoredContainer container)
		{
			_attachedContainers.Add(container);
		}

		protected virtual void UnAttachContainer(AnchoredContainer container)
		{
			_attachedContainers.Remove(container);
		}

		protected override void OnHidden(EventArgs e)
		{
			((Control)this).OnHidden(e);
			foreach (AnchoredContainer container in _attachedContainers)
			{
				if (((Control)container).get_Parent() == Control.get_Graphics().get_SpriteScreen())
				{
					((Control)container).Hide();
				}
			}
		}
	}
}
