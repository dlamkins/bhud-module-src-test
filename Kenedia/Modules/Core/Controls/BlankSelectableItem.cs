using System;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class BlankSelectableItem<T> : SelectableItem<T>
	{
		public string DisplayText { get; }

		public BlankSelectableItem(AutoSuggestComboBox<T> owner, string displayText = "")
			: base(owner, default(T))
		{
			DisplayText = displayText ?? string.Empty;
			base.Height = 20;
		}

		public override bool MatchesQuery(string query)
		{
			return true;
		}

		protected override void DrawItem(SpriteBatch spriteBatch, Rectangle bounds, bool is_selected)
		{
			if (!string.IsNullOrEmpty(DisplayText))
			{
				Color textColor = ((base.MouseOver || is_selected) ? SelectableItem<T>.HighlightForeground : SelectableItem<T>.DefaultTextColor);
				spriteBatch.DrawStringOnCtrl(this, DisplayText, GameService.Content.DefaultFont14, new Rectangle(6, 0, Math.Max(0, base.Width - 12), base.Height), textColor);
			}
		}

		public override string GetDisplayText()
		{
			return string.Empty;
		}
	}
}
