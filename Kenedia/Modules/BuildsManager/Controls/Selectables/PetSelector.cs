using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Selectables
{
	public class PetSelector : Selector<Pet>
	{
		private readonly DetailedTexture _selectingFrame = new DetailedTexture(157147);

		private readonly DetailedTexture _selectedPet = new DetailedTexture
		{
			TextureRegion = new Rectangle(16, 16, 200, 200)
		};

		public PetSelector()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			ContentPanel.BorderWidth = new RectangleDimensions(2, 0, 2, 2);
			base.SelectableSize = new Point(64);
			base.SelectablePerRow = 6;
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
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			ContentPanel.BorderWidth = new RectangleDimensions(2);
			HeaderPanel.BorderWidth = new RectangleDimensions(0, 0, 0, 2);
			spriteBatch.DrawCenteredRotationOnCtrl(this, (Texture2D)_selectingFrame.Texture, _selectingFrame.Bounds, _selectingFrame.TextureRegion, Color.get_White(), 0f, flipVertically: true, flipHorizontally: true);
			_selectedPet.Draw(this, spriteBatch, null, Color.get_White());
		}

		protected override void Recalculate(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Point> e)
		{
			base.Recalculate(sender, e);
		}

		public override void RecalculateLayout()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (ContentPanel != null && HeaderPanel != null)
			{
				ContentPanel.ContentPadding = new RectangleDimensions(8);
				int p = 4;
				Rectangle r = default(Rectangle);
				((Rectangle)(ref r))._002Ector(Point.get_Zero(), new Point(ContentPanel.Width - p * 4, base.SelectableSize.Y + 20));
				HeaderPanel.SetBounds(r);
				int pad = 40;
				_selectingFrame.Bounds = new Rectangle(-pad - pad / 5, 0, HeaderPanel.Width + pad * 3, HeaderPanel.Height + pad / 10);
				int selectorWidth = (int)(27.0 / 128.0 * (double)HeaderPanel.Width);
				pad = 7;
				BlockInputRegion = Blish_HUD.RectangleExtension.Add(new Rectangle(HeaderPanel.Location.Add(new Point((HeaderPanel.Width - selectorWidth) / 2 + pad, pad)), new Point(selectorWidth, HeaderPanel.Height)), new Rectangle(-pad, -pad, pad * 4, pad * 4));
				pad = 16;
				Point pos = ((Rectangle)(ref BlockInputRegion)).get_Center();
				_selectedPet.Bounds = new Rectangle(pos.X - 64, pos.Y - 60 - pad, 120, 120);
			}
		}

		protected override CaptureType CapturesInput()
		{
			if (!HeaderPanel.MouseOver)
			{
				return base.CapturesInput();
			}
			return CaptureType.None;
		}

		protected override void OnDataApplied(Pet item)
		{
			base.OnDataApplied(item);
			_selectedPet.Texture = item?.SelectedIcon;
			base.Controls.ForEach(delegate(Selectable<Pet> c)
			{
				c.IsSelected = c.Data == base.SelectedItem;
			});
		}

		protected override Selectable<Pet> CreateSelectable(Pet item)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			base.Type = SelectableType.Pet;
			base.Visible = true;
			return new PetSelectable
			{
				Parent = FlowPanel,
				Size = base.SelectableSize,
				Data = item,
				OnClickAction = base.OnClickAction,
				IsSelected = (base.PassSelected && item.Equals(base.SelectedItem))
			};
		}

		protected override void SetCapture()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			if (HeaderPanel != null)
			{
				base.CaptureInput = !HeaderPanel.MouseOver || ((Rectangle)(ref BlockInputRegion)).Contains(base.RelativeMousePosition);
				HeaderPanel.CaptureInput = !HeaderPanel.MouseOver || ((Rectangle)(ref BlockInputRegion)).Contains(base.RelativeMousePosition);
			}
		}
	}
}
