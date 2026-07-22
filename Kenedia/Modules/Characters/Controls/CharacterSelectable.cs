using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters.Controls
{
	public class CharacterSelectable : SelectableItem<Character_Model>
	{
		public Character_Model Character { get; }

		public CharacterSelectable(AutoSuggestComboBox<Character_Model> owner, Character_Model character)
			: base(owner, character)
		{
			Character = character;
			base.Height = 20;
			base.Width = 100;
		}

		public override bool MatchesQuery(string query)
		{
			return Character.Name.ToLowerInvariant().Contains(query);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.Paint(spriteBatch, bounds);
		}

		protected override void DrawItem(SpriteBatch spriteBatch, Rectangle bounds, bool is_selected)
		{
			if (Character != null)
			{
				Color textColor = ((base.MouseOver || is_selected) ? SelectableItem<Character_Model>.HighlightForeground : SelectableItem<Character_Model>.DefaultTextColor);
				spriteBatch.DrawStringOnCtrl(this, Character.Name, GameService.Content.DefaultFont14, new Rectangle(6, 0, Math.Max(0, base.Width - 12), base.Height), textColor);
			}
		}

		protected override void OnParent_Resized(object sender, ResizedEventArgs e)
		{
			base.OnParent_Resized(sender, e);
			this.SetSize(base.Parent?.ContentRegion.Width ?? base.Width, base.Height);
		}
	}
}
