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

		private readonly DetailedTexture _selectedPet = new DetailedTexture(156794)
		{
			TextureRegion = new Rectangle(16, 16, 200, 200)
		};

		private readonly DetailedTexture _highlight = new DetailedTexture(156844)
		{
			TextureRegion = new Rectangle(16, 16, 200, 200)
		};

		public PetSelector()
		{
			ContentPanel.BorderWidth = new RectangleDimensions(2, 0, 2, 2);
			base.SelectableSize = new Point(64);
			base.SelectablePerRow = 9;
			FlowPanel.ContentPadding = new RectangleDimensions(20, 0);
			FlowPanel.ControlPadding = new Vector2(10f);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintAfterChildren(spriteBatch, bounds);
			ContentPanel.BorderWidth = new RectangleDimensions(2);
			HeaderPanel.BorderWidth = new RectangleDimensions(0, 0, 0, 2);
			spriteBatch.DrawCenteredRotationOnCtrl(this, (Texture2D)_selectingFrame.Texture, _selectingFrame.Bounds, _selectingFrame.TextureRegion, Color.White, 0f, flipVertically: true, flipHorizontally: true);
			_selectedPet.Draw(this, spriteBatch, null, Color.White);
			if (_highlight.Bounds.Contains(base.RelativeMousePosition))
			{
				_highlight.Draw(this, spriteBatch);
			}
		}

		protected override void Recalculate(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Point> e)
		{
			base.Recalculate(sender, e);
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			if (ContentPanel != null && HeaderPanel != null)
			{
				ContentPanel.ContentPadding = new RectangleDimensions(8);
				int p = 4;
				ControlExtensions.SetBounds(bounds: new Rectangle(Point.Zero, new Point(ContentPanel.Width - p * 4, base.SelectableSize.Y + 20)), c: HeaderPanel);
				int pad = 40;
				_selectingFrame.Bounds = new Rectangle(-pad - pad / 5, 0, HeaderPanel.Width + pad * 3, HeaderPanel.Height + pad / 10);
				int selectorWidth = (int)(27.0 / 128.0 * (double)HeaderPanel.Width);
				pad = 7;
				BlockInputRegion = new Rectangle(HeaderPanel.Location.Add(new Point((HeaderPanel.Width - selectorWidth) / 2 + pad, pad)), new Point(selectorWidth, HeaderPanel.Height)).Add(new Rectangle(-pad, -pad, pad * 4, pad * 4));
				pad = 16;
				Point pos = BlockInputRegion.Center;
				_selectedPet.Bounds = new Rectangle(pos.X - 64, pos.Y - 60 - pad, 120, 120);
				_highlight.Bounds = new Rectangle(pos.X - 64, pos.Y - 60 - pad, 120, 120);
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
			if (HeaderPanel != null)
			{
				base.CaptureInput = !HeaderPanel.MouseOver || BlockInputRegion.Contains(base.RelativeMousePosition);
				HeaderPanel.CaptureInput = !HeaderPanel.MouseOver || BlockInputRegion.Contains(base.RelativeMousePosition);
			}
		}
	}
}
