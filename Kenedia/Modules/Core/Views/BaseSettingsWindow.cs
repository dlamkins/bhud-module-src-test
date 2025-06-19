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
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		protected (Kenedia.Modules.Core.Controls.Panel, Kenedia.Modules.Core.Controls.Label, Kenedia.Modules.Core.Controls.TrackBar) LabeledTrackbar(Container parent, Func<string> setLocalizedText, Func<string> setLocalizedTooltip)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
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
