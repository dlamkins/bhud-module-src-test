using System;
using System.Runtime.CompilerServices;
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

		public T Item { get; }

		public Action<T>? OnClickAction { get; set; }

		public bool Selected
		{
			[CompilerGenerated]
			get
			{
				return _003CSelected_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CSelected_003Ek__BackingField, value, delegate(bool v)
				{
					_003CSelected_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<bool>(OnSelectedChanged));
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
			base.BackgroundColor = (Selected ? (ContentService.Colors.ColonialWhite * 0.1f) : Color.Transparent);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
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
			base.RecalculateLayout();
			ContentBounds = new Rectangle(5, 2, base.Width - 30, base.Height - 4);
			IconBounds = new Rectangle(ContentBounds.Left, ContentBounds.Top, ContentBounds.Height, ContentBounds.Height);
			TextBounds = new Rectangle(IconBounds.Right + 5, ContentBounds.Top, ContentBounds.Width - IconBounds.Width - 5, ContentBounds.Height);
		}

		protected abstract void DrawItem(SpriteBatch spriteBatch, Rectangle bounds);
	}
}
