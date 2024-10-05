using Blish_HUD;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Selectables
{
	public class LegendSelector : Selector<Legend>
	{
		private readonly DetailedTexture _selectingFrame = new DetailedTexture(157147);

		public LegendSlotType LegendSlot { get; set; } = LegendSlotType.TerrestrialActive;


		public LegendSelector()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			ContentPanel.BorderWidth = new RectangleDimensions(2, 0, 2, 2);
			ContentPanel.ContentPadding = new RectangleDimensions(10);
			base.SelectableSize = new Point(48);
		}

		protected override void OnDataApplied(Legend item)
		{
			base.OnDataApplied(item);
			base.Controls.ForEach(delegate(Selectable<Legend> c)
			{
				c.IsSelected = c.Data == base.SelectedItem;
			});
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			ContentPanel.BorderWidth = new RectangleDimensions(2);
			HeaderPanel.BorderWidth = new RectangleDimensions(0, 0, 0, 2);
			spriteBatch.DrawCenteredRotationOnCtrl(this, (Texture2D)_selectingFrame.Texture, _selectingFrame.Bounds, _selectingFrame.TextureRegion, Color.get_White(), 0f, flipVertically: true, flipHorizontally: true);
		}

		protected override void Recalculate(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Point> e)
		{
			base.Recalculate(sender, e);
		}

		protected override Selectable<Legend> CreateSelectable(Legend item)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			base.Type = SelectableType.Legend;
			base.Visible = true;
			return new LegendSelectable
			{
				Parent = FlowPanel,
				Size = base.SelectableSize,
				Data = item,
				OnClickAction = base.OnClickAction,
				IsSelected = (base.PassSelected && item.Equals(base.SelectedItem)),
				LegendSlot = LegendSlot
			};
		}

		public override void RecalculateLayout()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (ContentPanel != null && HeaderPanel != null)
			{
				ContentPanel.ContentPadding = new RectangleDimensions(8);
				Rectangle r = default(Rectangle);
				((Rectangle)(ref r))._002Ector(Point.get_Zero(), new Point(ContentPanel.Width, base.SelectableSize.Y + 10));
				HeaderPanel?.SetBounds(r);
				int pad = 20;
				_selectingFrame.Bounds = new Rectangle(-pad - 1, 0, HeaderPanel.Width + pad * 2 + 4, HeaderPanel.Height + 2);
			}
		}

		protected override void SetCapture()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			if (HeaderPanel != null)
			{
				int selectorWidth = (int)(27.0 / 128.0 * (double)HeaderPanel.Width);
				int pad = 5;
				BlockInputRegion = Blish_HUD.RectangleExtension.Add(new Rectangle(HeaderPanel.Location.Add(new Point((HeaderPanel.Width - selectorWidth) / 2, 0)), new Point(selectorWidth, HeaderPanel.Height)), new Rectangle(-pad - 2, -pad, pad * 2 + 6, pad * 2));
				base.CaptureInput = !HeaderPanel.MouseOver || ((Rectangle)(ref BlockInputRegion)).Contains(base.RelativeMousePosition);
				HeaderPanel.CaptureInput = !HeaderPanel.MouseOver || ((Rectangle)(ref BlockInputRegion)).Contains(base.RelativeMousePosition);
			}
		}
	}
}
