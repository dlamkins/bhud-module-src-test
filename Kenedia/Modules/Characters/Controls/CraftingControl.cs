using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Characters.Controls
{
	public class CraftingControl : Control, IFontControl
	{
		private readonly AsyncTexture2D _craftingIcon = AsyncTexture2D.FromAssetId(156711);

		public Data Data { get; set; }

		public Settings Settings { get; set; }

		public Character_Model Character { get; set; }

		public BitmapFont Font
		{
			[CompilerGenerated]
			get
			{
				return _003CFont_003Ek__BackingField;
			}
			set
			{
				_003CFont_003Ek__BackingField = value;
				if (value != null)
				{
					base.Width = base.Height + 4 + (int)value.MeasureString(strings.NoCraftingProfession).Width;
				}
			}
		}

		public string Text { get; set; }

		public CraftingControl()
		{
			_003CFont_003Ek__BackingField = GameService.Content.DefaultFont14;
			base._002Ector();
		}

		public CraftingControl(Data data, Settings settings)
		{
			_003CFont_003Ek__BackingField = GameService.Content.DefaultFont14;
			base._002Ector();
			Data = data;
			Settings = settings;
		}

		protected override void Paint(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Rectangle bounds)
		{
			string toolTipText = null;
			spriteBatch.DrawOnCtrl(this, _craftingIcon, new Microsoft.Xna.Framework.Rectangle(4, 4, bounds.Height - 7, bounds.Height - 7), new Microsoft.Xna.Framework.Rectangle(6, 6, 20, 20), Microsoft.Xna.Framework.Color.White, 0f, default(Vector2));
			bool craftingDisplayed = false;
			if (Character != null && Character.Crafting.Count > 0 && Settings != null && Data != null)
			{
				DataDictionary<CraftingDisciplineType, CraftingProfession> craftingDictionary = Data.CraftingProfessions;
				int i = 0;
				foreach (CharacterCrafting crafting in Character.Crafting)
				{
					craftingDictionary.TryGetValue(crafting.Id, out var craftingProfession);
					if (craftingProfession == null)
					{
						continue;
					}
					Text = "NA";
					bool onlyMax = Settings.DisplayToggles.Value["OnlyMaxCrafting"].Show;
					if (craftingProfession.Icon != null && (!onlyMax || crafting.Rating == craftingProfession.MaxRating))
					{
						craftingDisplayed = true;
						Microsoft.Xna.Framework.Rectangle craftBounds = new Microsoft.Xna.Framework.Rectangle(bounds.Height + 6 + i * bounds.Height, 2, bounds.Height - 4, bounds.Height - 4);
						spriteBatch.DrawOnCtrl(this, craftingProfession.Icon, craftBounds, new Microsoft.Xna.Framework.Rectangle(8, 8, 16, 16), Microsoft.Xna.Framework.Color.White, 0f, default(Vector2));
						i++;
						if (craftBounds.Contains(base.RelativeMousePosition))
						{
							toolTipText = craftingProfession.Name + " (" + crafting.Rating + "/" + craftingProfession.MaxRating + ")";
						}
					}
				}
			}
			if (!craftingDisplayed)
			{
				string text = (Text = strings.NoCraftingProfession);
				spriteBatch.DrawStringOnCtrl(this, text, Font, new Microsoft.Xna.Framework.Rectangle(bounds.Height + 4, 0, bounds.Width - (bounds.Height + 4), bounds.Height), Microsoft.Xna.Framework.Color.Gray);
			}
			base.BasicTooltipText = toolTipText;
		}
	}
}
