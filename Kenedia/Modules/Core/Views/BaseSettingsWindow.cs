using System;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Views
{
	public class BaseSettingsWindow : StandardWindow
	{
		protected readonly Kenedia.Modules.Core.Controls.FlowPanel ContentPanel;

		public BaseSettingsWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: base(background, windowRegion, contentRegion)
		{
			ContentPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				Width = base.ContentRegion.Width,
				Height = base.ContentRegion.Height,
				ControlPadding = new Vector2(0f, 10f),
				CanScroll = true
			};
			base.MainWindowEmblem = AsyncTexture2D.FromAssetId(156015);
			base.SubWindowEmblem = AsyncTexture2D.FromAssetId(156027);
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintAfterChildren(spriteBatch, bounds);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			base.Children.DisposeAll();
			ContentPanel.Children.DisposeAll();
			base.SubWindowEmblem = null;
			base.MainWindowEmblem = null;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		protected (Kenedia.Modules.Core.Controls.Panel, Kenedia.Modules.Core.Controls.Label, Kenedia.Modules.Core.Controls.TrackBar) LabeledTrackbar(Container parent, Func<string> setLocalizedText, Func<string> setLocalizedTooltip)
		{
			Kenedia.Modules.Core.Controls.Panel subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = parent,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize
			};
			Kenedia.Modules.Core.Controls.Label scaleLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				SetLocalizedText = setLocalizedText,
				SetLocalizedTooltip = setLocalizedTooltip
			};
			Kenedia.Modules.Core.Controls.TrackBar trackBar = new Kenedia.Modules.Core.Controls.TrackBar
			{
				Parent = subP,
				SetLocalizedTooltip = setLocalizedTooltip,
				Location = new Point(250, 0)
			};
			return (subP, scaleLabel, trackBar);
		}
	}
}
