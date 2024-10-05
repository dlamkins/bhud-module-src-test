using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public abstract class Selectable<T> : Control
	{
		protected Rectangle ContentBounds;

		protected Rectangle IconBounds;

		protected Rectangle TextBounds;

		private bool _selected;

		public T Item { get; }

		public Action<T>? OnClickAction { get; set; }

		public bool Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				Common.SetProperty(ref _selected, value, new ValueChangedEventHandler<bool>(OnSelectedChanged));
			}
		}

		public Selectable(T item, Container parent)
		{
			Item = item;
			base.Parent = parent;
			base.Height = 30;
			base.Width = base.Parent.Width - 25;
			base.Parent.Resized += Parent_Resized;
		}

		private void Parent_Resized(object sender, ResizedEventArgs e)
		{
			base.Width = base.Parent.Width - 25;
		}

		private void OnSelectedChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<bool> e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			base.BackgroundColor = (Selected ? (ContentService.Colors.ColonialWhite * 0.1f) : Color.get_Transparent());
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (base.MouseOver)
			{
				spriteBatch.DrawFrame(this, bounds, ContentService.Colors.ColonialWhite, 2);
			}
			if (Selected)
			{
				spriteBatch.DrawFrame(this, bounds, ContentService.Colors.ColonialWhite * 0.5f, 2);
			}
			DrawItem(spriteBatch, bounds);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			if (OnClickAction != null)
			{
				OnClickAction!(Item);
			}
		}

		public override void RecalculateLayout()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			ContentBounds = new Rectangle(5, 2, base.Width - 30, base.Height - 4);
			IconBounds = new Rectangle(((Rectangle)(ref ContentBounds)).get_Left(), ((Rectangle)(ref ContentBounds)).get_Top(), ContentBounds.Height, ContentBounds.Height);
			TextBounds = new Rectangle(((Rectangle)(ref IconBounds)).get_Right() + 5, ((Rectangle)(ref ContentBounds)).get_Top(), ContentBounds.Width - IconBounds.Width - 5, ContentBounds.Height);
		}

		protected abstract void DrawItem(SpriteBatch spriteBatch, Rectangle bounds);
	}
}
