using System;
using System.Collections.Generic;
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
		protected readonly FlowPanel ContentPanel;

		public BaseSettingsWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: base(background, windowRegion, contentRegion)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)this);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			((Control)flowPanel).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)flowPanel).set_Height(((Container)this).get_ContentRegion().Height);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(0f, 10f));
			((Panel)flowPanel).set_CanScroll(true);
			ContentPanel = flowPanel;
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
			((IEnumerable<IDisposable>)((Container)this).get_Children()).DisposeAll();
			((IEnumerable<IDisposable>)((Container)ContentPanel).get_Children()).DisposeAll();
			base.SubWindowEmblem = null;
			base.MainWindowEmblem = null;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		protected (Panel, Label, TrackBar) LabeledTrackbar(Container parent, Func<string> setLocalizedText, Func<string> setLocalizedTooltip)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent(parent);
			((Control)panel).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			Panel subP = panel;
			Label label = new Label();
			((Control)label).set_Parent((Container)(object)subP);
			((Label)label).set_AutoSizeWidth(true);
			label.SetLocalizedText = setLocalizedText;
			label.SetLocalizedTooltip = setLocalizedTooltip;
			Label scaleLabel = label;
			TrackBar trackBar2 = new TrackBar();
			((Control)trackBar2).set_Parent((Container)(object)subP);
			trackBar2.SetLocalizedTooltip = setLocalizedTooltip;
			((Control)trackBar2).set_Location(new Point(250, 0));
			TrackBar trackBar = trackBar2;
			return (subP, scaleLabel, trackBar);
		}
	}
}
