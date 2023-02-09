using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Characters.Controls
{
	public class CraftingControl : Control, IFontControl
	{
		private readonly AsyncTexture2D _craftingIcon = AsyncTexture2D.FromAssetId(156711);

		private BitmapFont _font = GameService.Content.get_DefaultFont14();

		public Data Data { get; set; }

		public SettingsModel Settings { get; set; }

		public Character_Model Character { get; set; }

		public BitmapFont Font
		{
			get
			{
				return _font;
			}
			set
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				_font = value;
				if (value != null)
				{
					((Control)this).set_Width(((Control)this).get_Height() + 4 + (int)value.MeasureString(strings.NoCraftingProfession).Width);
				}
			}
		}

		public string Text { get; set; }

		public CraftingControl()
			: this()
		{
		}

		public CraftingControl(Data data, SettingsModel settings)
			: this()
		{
			Data = data;
			Settings = settings;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			string toolTipText = null;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_craftingIcon), new Rectangle(4, 4, bounds.Height - 7, bounds.Height - 7), (Rectangle?)new Rectangle(6, 6, 20, 20), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			bool craftingDisplayed = false;
			if (Character != null && Character.Crafting.Count > 0 && Settings != null && Data != null)
			{
				Dictionary<int, Data.CraftingProfession> craftingDictionary = Data.CrafingProfessions;
				int i = 0;
				Rectangle craftBounds = default(Rectangle);
				foreach (CharacterCrafting crafting in Character.Crafting)
				{
					craftingDictionary.TryGetValue(crafting.Id, out var craftingProfession);
					if (craftingProfession == null)
					{
						continue;
					}
					Text = "NA";
					bool onlyMax = Settings.DisplayToggles.get_Value()["OnlyMaxCrafting"].Show;
					if (craftingProfession.Icon != null && (!onlyMax || crafting.Rating == craftingProfession.MaxRating))
					{
						craftingDisplayed = true;
						((Rectangle)(ref craftBounds))._002Ector(bounds.Height + 6 + i * bounds.Height, 2, bounds.Height - 4, bounds.Height - 4);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(craftingProfession.Icon), craftBounds, (Rectangle?)new Rectangle(8, 8, 16, 16), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
						i++;
						if (((Rectangle)(ref craftBounds)).Contains(((Control)this).get_RelativeMousePosition()))
						{
							toolTipText = craftingProfession.Name + " (" + crafting.Rating + "/" + craftingProfession.MaxRating + ")";
						}
					}
				}
			}
			if (!craftingDisplayed)
			{
				string text = (Text = strings.NoCraftingProfession);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, Font, new Rectangle(bounds.Height + 4, 0, bounds.Width - (bounds.Height + 4), bounds.Height), Color.get_Gray(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			((Control)this).set_BasicTooltipText(toolTipText);
		}
	}
}
