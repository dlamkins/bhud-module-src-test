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

		private readonly Data _data;

		private readonly SettingsModel _settings;

		private BitmapFont _font = GameService.Content.get_DefaultFont14();

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

		public CraftingControl(Data data, SettingsModel settings)
			: this()
		{
			_data = data;
			_settings = settings;
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
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			string toolTipText = null;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_craftingIcon), new Rectangle(4, 4, bounds.Height - 7, bounds.Height - 7), (Rectangle?)new Rectangle(6, 6, 20, 20), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			bool craftingDisplayed = false;
			if (Character != null && Character.Crafting.Count > 0)
			{
				Dictionary<int, Data.CraftingProfession> craftingDictionary = _data.CrafingProfessions;
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
					bool onlyMax = _settings.DisplayToggles.get_Value()["OnlyMaxCrafting"].Show;
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
