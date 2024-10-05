using System;
using Blish_HUD;
using Blish_HUD.Content;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Services
{
	public class TagEditWindowXFactory
	{
		public TagEditWindowX? TagEditWindow { get; private set; }

		public TemplateTags TemplateTags { get; }

		public TagEditWindowXFactory(TemplateTags templateTags)
		{
			TemplateTags = templateTags;
		}

		public void DisposeEditWindow()
		{
			if (TagEditWindow != null)
			{
				TagEditWindow!.Dispose();
				TagEditWindow = null;
			}
		}

		public void ShowEditWindow(TemplateTag tag)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			int Height = 670;
			int Width = 915;
			bool num = TagEditWindow == null;
			if (TagEditWindow == null)
			{
				TagEditWindowX obj = new TagEditWindowX((AsyncTexture2D)TexturesService.GetTextureFromRef("textures\\mainwindow_background.png", "mainwindow_background"), new Rectangle(30, 30, Width, Height + 30), new Rectangle(40, 40, Width - 3, Height), TemplateTags)
				{
					Parent = GameService.Graphics.SpriteScreen,
					Title = "❤",
					Subtitle = "❤",
					SavesPosition = true,
					Id = BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.Name + " TagWindow",
					MainWindowEmblem = AsyncTexture2D.FromAssetId(536043),
					SubWindowEmblem = AsyncTexture2D.FromAssetId(156031),
					Name = "Tag Editing",
					Width = 580,
					Height = 800,
					CanResize = true
				};
				TagEditWindowX tagEditWindowX = obj;
				TagEditWindow = obj;
			}
			TagEditWindow!.Show(tag);
			if (num)
			{
				TagEditWindow!.Hidden += Window_Hidden;
			}
		}

		private void Window_Hidden(object sender, EventArgs e)
		{
			TagEditWindowX window = sender as TagEditWindowX;
			if (window != null)
			{
				window.Dispose();
				TagEditWindow = null;
			}
		}
	}
}
