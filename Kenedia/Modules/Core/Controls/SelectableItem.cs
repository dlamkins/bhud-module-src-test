using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public abstract class SelectableItem<T> : Control
	{
		protected static readonly Color HighlightBackground = new Color(45, 37, 25, 255);

		protected static readonly Color DefaultTextColor = Color.FromNonPremultiplied(239, 240, 239, 255);

		protected static readonly Color HighlightForeground = ContentService.Colors.Chardonnay;

		public new Container Parent
		{
			get
			{
				return base.Parent;
			}
			set
			{
				Container parent2 = base.Parent;
				if (parent2 != null)
				{
					parent2.Resized -= OnParent_Resized;
				}
				Container container2 = (base.Parent = value);
				Container parent = container2;
				if (parent != null)
				{
					parent.Resized += OnParent_Resized;
				}
				base.Width = ((parent != null) ? (parent.Width - 12) : base.Width);
			}
		}

		public AutoSuggestComboBox<T> Owner { get; }

		public T Item { get; set; }

		public SelectableItem(AutoSuggestComboBox<T> owner, T item)
		{
			Owner = owner;
			Item = item;
		}

		protected virtual void OnParent_Resized(object sender, ResizedEventArgs e)
		{
		}

		public abstract bool MatchesQuery(string query);

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			AutoSuggestComboBox<T> owner = Owner;
			bool? obj;
			if (owner == null)
			{
				obj = null;
			}
			else
			{
				T selected = owner.Selected;
				obj = ((selected != null) ? new bool?(selected.Equals(Item)) : null);
			}
			bool? flag = obj;
			bool is_selected = flag.GetValueOrDefault();
			bounds = new Rectangle(bounds.X, bounds.Y, bounds.Width - AutoSuggestComboBox<T>.TextureArrow.Width - 5, bounds.Height);
			if (base.MouseOver || is_selected)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(2, 2, Math.Max(0, base.Width - AutoSuggestComboBox<T>.TextureArrow.Width - 5), Math.Max(0, base.Height - 4)), HighlightBackground);
			}
			DrawItem(spriteBatch, bounds, is_selected);
		}

		protected virtual void DrawItem(SpriteBatch spriteBatch, Rectangle bounds, bool is_selected)
		{
			Color textColor = (base.MouseOver ? HighlightForeground : DefaultTextColor);
			spriteBatch.DrawStringOnCtrl(this, Item.ToString(), GameService.Content.DefaultFont14, new Rectangle(6, 0, Math.Max(0, base.Width - 12), base.Height), textColor);
		}

		public virtual string GetDisplayText()
		{
			return Item.ToString();
		}
	}
}
