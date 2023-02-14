using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using SemVer;

namespace Kenedia.Modules.Core.Views
{
	public class BaseSettingsWindow : StandardWindow
	{
		private readonly AsyncTexture2D _subWindowEmblem = AsyncTexture2D.FromAssetId(156027);

		private readonly BitmapFont _titleFont = GameService.Content.get_DefaultFont32();

		protected readonly FlowPanel ContentPanel;

		private Rectangle _subEmblemRectangle;

		private Rectangle _mainEmblemRectangle;

		private Rectangle _titleRectangle;

		public new Version Version { get; set; }

		public new string Name { get; set; } = "Module";


		public new AsyncTexture2D MainWindowEmblem { get; set; } = AsyncTexture2D.FromAssetId(156015);


		public BaseSettingsWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: base(background, windowRegion, contentRegion)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)this);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			((Control)flowPanel).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)flowPanel).set_Height(((Container)this).get_ContentRegion().Height);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(0f, 10f));
			((Panel)flowPanel).set_CanScroll(true);
			ContentPanel = flowPanel;
		}

		public override void RecalculateLayout()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_subEmblemRectangle = new Rectangle(21, 6, 64, 64);
			_mainEmblemRectangle = new Rectangle(-43, -58, 128, 128);
			RectangleF titleBounds = _titleFont.GetStringRectangle(Name);
			_titleRectangle = new Rectangle(80, 5, (int)titleBounds.Width, Math.Max(30, (int)titleBounds.Height));
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			if (MainWindowEmblem != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(MainWindowEmblem), _mainEmblemRectangle, (Rectangle?)MainWindowEmblem.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_subWindowEmblem), _subEmblemRectangle, (Rectangle?)_subWindowEmblem.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			if (_titleRectangle.Width < bounds.Width - (_subEmblemRectangle.Width - 20))
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Name, _titleFont, _titleRectangle, Colors.ColonialWhite, false, (HorizontalAlignment)0, (VerticalAlignment)2);
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			((IEnumerable<IDisposable>)((Container)this).get_Children()).DisposeAll();
			((IEnumerable<IDisposable>)((Container)ContentPanel).get_Children()).DisposeAll();
			_subWindowEmblem.Dispose();
			MainWindowEmblem = null;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintBeforeChildren(spriteBatch, bounds);
			if (Version != (Version)null)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, $"v. {Version}", Control.get_Content().get_DefaultFont16(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 150, ((Rectangle)(ref bounds)).get_Top() + 10, 100, 30), Color.get_White(), false, true, 1, (HorizontalAlignment)2, (VerticalAlignment)0);
			}
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
