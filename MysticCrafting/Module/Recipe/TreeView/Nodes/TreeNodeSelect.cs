using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	public abstract class TreeNodeSelect : TreeNodeBase
	{
		private bool _selected;

		public bool Selected
		{
			get
			{
				return _selected;
			}
			private set
			{
				_selected = value;
				if (Checkbox != null)
				{
					Checkbox.Checked = value;
				}
			}
		}

		public Checkbox Checkbox { get; set; }

		public event EventHandler<CheckChangedEvent> OnSelected;

		public virtual void Build()
		{
			Checkbox = new Checkbox
			{
				Parent = this,
				Location = new Point(10, 12)
			};
		}

		public void ToggleSelect()
		{
			if (!Selected)
			{
				Select();
			}
			else if (Selected)
			{
				Deselect();
			}
		}

		public void Select()
		{
			if (!Selected)
			{
				Selected = true;
				this.OnSelected?.Invoke(this, new CheckChangedEvent(Selected));
			}
		}

		public void Deselect()
		{
			if (Selected)
			{
				Selected = false;
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			if (!Selected)
			{
				ToggleSelect();
			}
			ServiceContainer.AudioService.PlayMenuItemClick();
			base.OnClick(e);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (Selected)
			{
				DrawOutline(spriteBatch);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		private void DrawOutline(SpriteBatch spriteBatch)
		{
			Color lineColor = Color.LightYellow * 0.8f;
			int lineSize = 1;
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(base.PanelRectangle.X, 0, base.PanelRectangle.Width, lineSize), lineColor);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(base.PanelRectangle.X, base.PanelCollapsedHeight - lineSize, base.PanelRectangle.Width, lineSize), lineColor);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(0, 0, lineSize, base.PanelRectangle.Height + lineSize * 4), lineColor);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(base.PanelRectangle.Width - lineSize, 0, lineSize, base.PanelCollapsedHeight), lineColor);
		}
	}
}
