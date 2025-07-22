using System;
using System.Collections.Generic;
using BhModule.Community.Pathing.UI.Models;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.UI.Tooltips
{
	internal class EntityTextureTooltip : View, ITooltipView, IView
	{
		private List<PathingTexture> _textures;

		private List<Image> _imgIcons = new List<Image>();

		public EntityTextureTooltip(List<PathingTexture> textures)
			: this()
		{
			_textures = textures;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			if (_textures.Count <= 0)
			{
				return;
			}
			int xPosition = 0;
			foreach (PathingTexture texture in _textures)
			{
				(int width, int height) resizedDimensions = GetResizedDimensions(texture.Icon, 200, 200);
				int newWidth = resizedDimensions.width;
				int newHeight = resizedDimensions.height;
				List<Image> imgIcons = _imgIcons;
				Image val = new Image(texture.Icon);
				((Control)val).set_Size(new Point(newWidth, newHeight));
				val.set_Tint(texture.Tint);
				((Control)val).set_Location(new Point(xPosition, 0));
				((Control)val).set_Parent(buildPanel);
				imgIcons.Add(val);
				xPosition += newWidth;
			}
		}

		private (int width, int height) GetResizedDimensions(AsyncTexture2D texture, int maxWidth, int maxHeight)
		{
			int originalWidth = texture.get_Width();
			int originalHeight = texture.get_Height();
			if (originalWidth <= maxWidth && originalHeight <= maxHeight)
			{
				return (originalWidth, originalHeight);
			}
			float val = (float)maxWidth / (float)originalWidth;
			float heightRatio = (float)maxHeight / (float)originalHeight;
			float scale = Math.Min(val, heightRatio);
			int item = (int)((float)originalWidth * scale);
			int newHeight = (int)((float)originalHeight * scale);
			return (item, newHeight);
		}

		protected override void Unload()
		{
			DisposeControls((IEnumerable<Control>)_imgIcons);
			((View<IPresenter>)this).Unload();
		}

		private void DisposeControls(IEnumerable<Control> controls)
		{
			Queue<Control> controlsQueue = new Queue<Control>(controls);
			while (controlsQueue.Count > 0)
			{
				Control obj = controlsQueue.Dequeue();
				obj.set_Parent((Container)null);
				obj.Dispose();
			}
		}
	}
}
