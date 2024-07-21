using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.RecipeTree.TreeView.Nodes
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
					Checkbox.set_Checked(value);
				}
			}
		}

		public Checkbox Checkbox { get; set; }

		public event EventHandler<CheckChangedEvent> OnSelected;

		protected TreeNodeSelect(Container parent)
			: base(parent)
		{
		}

		public virtual void Build()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			Checkbox val = new Checkbox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(15, 12));
			Checkbox = val;
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
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
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
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		private void DrawOutline(SpriteBatch spriteBatch)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			Color lineColor = Color.get_LightYellow() * 0.8f;
			int lineSize = 1;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(base.PanelRectangle.X, 0, base.PanelRectangle.Width, lineSize), lineColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(base.PanelRectangle.X, base.PanelCollapsedHeight - lineSize, base.PanelRectangle.Width, lineSize), lineColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, lineSize, base.PanelRectangle.Height + lineSize * 4), lineColor);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(base.PanelRectangle.Width - lineSize, 0, lineSize, base.PanelCollapsedHeight), lineColor);
		}
	}
}
