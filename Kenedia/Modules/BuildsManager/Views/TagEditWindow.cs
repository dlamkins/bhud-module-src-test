using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Views;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class TagEditWindow : Kenedia.Modules.Core.Views.TabbedWindow
	{
		public TemplateTags TemplateTags { get; }

		public TagGroups TagGroups { get; }

		public Tab TagEditViewTab { get; }

		public Tab TagGroupViewTab { get; }

		public TagEditWindow(TemplateTags templateTags, TagGroups tagGroups)
			: base((AsyncTexture2D)TexturesService.GetTextureFromRef("textures\\mainwindow_background.png", "mainwindow_background"), new Rectangle(30, 30, 915, 700), new Rectangle(40, 40, 912, 670))
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			TemplateTags = templateTags;
			TagGroups = tagGroups;
			base.Id = "BuildsManagerEditTagAndGroups";
			base.Title = "Edit Tags and Groups";
			base.MainWindowEmblem = AsyncTexture2D.FromAssetId(156020);
			base.SubWindowEmblem = AsyncTexture2D.FromAssetId(156027);
			base.Parent = Control.Graphics.SpriteScreen;
			base.SavesPosition = true;
			base.SavesSize = true;
			base.CanResize = true;
			base.Tabs.Add(TagEditViewTab = new Tab(AsyncTexture2D.FromAssetId(156025), () => new TagEditView(TemplateTags, TagGroups), strings.Tags));
			base.Tabs.Add(TagGroupViewTab = new Tab(AsyncTexture2D.FromAssetId(578844), () => new TagGroupView(TagGroups), strings.Group));
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			Point minSize = default(Point);
			((Point)(ref minSize))._002Ector(600, 300);
			if (e.CurrentSize.X >= minSize.X && e.CurrentSize.Y >= minSize.Y)
			{
				base.OnResized(e);
			}
			else
			{
				base.Size = new Point(Math.Max(e.CurrentSize.X, minSize.X), Math.Max(e.CurrentSize.Y, minSize.Y));
			}
		}

		public void Show(TemplateTag e)
		{
			Show();
		}

		protected override void OnTabChanged(ValueChangedEventArgs<Tab> e)
		{
			base.OnTabChanged(e);
			base.Subtitle = ((e.NewValue == TagEditViewTab) ? strings.Tags : strings.Group);
		}

		protected override void DisposeControl()
		{
			Hide();
			base.DisposeControl();
		}
	}
}
