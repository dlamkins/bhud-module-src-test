using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace WhereIsMyPSNA
{
	internal class RecipeTooltip : Panel
	{
		private const int TipWidth = 360;

		private const int Pad = 9;

		private const int SmLineH = 18;

		private const int LgLineH = 24;

		private const int CoinSize = 18;

		private const int IconSize = 36;

		private readonly Image _itemIcon;

		private readonly Label _nameLabel;

		private readonly Label _descLabel;

		private readonly Label _durLabel;

		private readonly Label _bindLabel;

		private readonly Image _goldIcon;

		private readonly Label _goldLabel;

		private readonly Image _silverIcon;

		private readonly Label _silverLabel;

		private readonly Image _copperIcon;

		private readonly Label _copperLabel;

		public RecipeTooltip()
			: this()
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Expected O, but got Unknown
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Expected O, but got Unknown
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Expected O, but got Unknown
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Expected O, but got Unknown
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Expected O, but got Unknown
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Expected O, but got Unknown
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Expected O, but got Unknown
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Expected O, but got Unknown
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Visible(false);
			((Control)this).set_ZIndex(9999);
			((Control)this).set_Width(360);
			((Control)this).set_Height(60);
			((Control)this).set_BackgroundColor(new Color(0, 0, 0, 220));
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(new Point(36, 36));
			_itemIcon = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Font(GameService.Content.get_DefaultFont18());
			val2.set_WrapText(false);
			_nameLabel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Font(GameService.Content.get_DefaultFont14());
			val3.set_TextColor(Color.get_White());
			val3.set_WrapText(true);
			_descLabel = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Font(GameService.Content.get_DefaultFont14());
			val4.set_TextColor(new Color(220, 220, 60));
			val4.set_WrapText(false);
			_durLabel = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Font(GameService.Content.get_DefaultFont14());
			val5.set_TextColor(Color.get_LightGray());
			val5.set_WrapText(false);
			_bindLabel = val5;
			Image val6 = new Image();
			((Control)val6).set_Parent((Container)(object)this);
			((Control)val6).set_Size(new Point(18, 18));
			_goldIcon = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Font(GameService.Content.get_DefaultFont14());
			val7.set_TextColor(new Color(255, 215, 0));
			_goldLabel = val7;
			Image val8 = new Image();
			((Control)val8).set_Parent((Container)(object)this);
			((Control)val8).set_Size(new Point(18, 18));
			_silverIcon = val8;
			Label val9 = new Label();
			((Control)val9).set_Parent((Container)(object)this);
			val9.set_Font(GameService.Content.get_DefaultFont14());
			val9.set_TextColor(new Color(192, 192, 192));
			_silverLabel = val9;
			Image val10 = new Image();
			((Control)val10).set_Parent((Container)(object)this);
			((Control)val10).set_Size(new Point(18, 18));
			_copperIcon = val10;
			Label val11 = new Label();
			((Control)val11).set_Parent((Container)(object)this);
			val11.set_Font(GameService.Content.get_DefaultFont14());
			val11.set_TextColor(new Color(184, 115, 51));
			_copperLabel = val11;
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)1;
		}

		public void SetCoinTextures(AsyncTexture2D gold, AsyncTexture2D silver, AsyncTexture2D copper)
		{
			_goldIcon.set_Texture(gold);
			_silverIcon.set_Texture(silver);
			_copperIcon.set_Texture(copper);
		}

		public void SetRecipe(RecipeDef def, AsyncTexture2D icon = null)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			_itemIcon.set_Texture(icon);
			_nameLabel.set_Text(def.Name);
			_nameLabel.set_TextColor(GetRarityColor(def.Rarity));
			_descLabel.set_Text(def.Description);
			((Control)_descLabel).set_Visible(!string.IsNullOrEmpty(def.Description));
			_durLabel.set_Text((def.DurationSecs > 0) ? FormatDuration(def.DurationSecs) : "");
			((Control)_durLabel).set_Visible(def.DurationSecs > 0);
			string bindText = FormatBinding(def.Binding);
			_bindLabel.set_Text(bindText);
			((Control)_bindLabel).set_Visible(!string.IsNullOrEmpty(bindText));
			int g = def.VendorValue / 10000;
			int s = def.VendorValue % 10000 / 100;
			int c = def.VendorValue % 100;
			((Control)_goldIcon).set_Visible(g > 0);
			((Control)_goldLabel).set_Visible(g > 0);
			_goldLabel.set_Text(g.ToString());
			((Control)_silverIcon).set_Visible(def.VendorValue >= 100);
			((Control)_silverLabel).set_Visible(def.VendorValue >= 100);
			_silverLabel.set_Text(s.ToString());
			((Control)_copperIcon).set_Visible(def.VendorValue > 0);
			((Control)_copperLabel).set_Visible(def.VendorValue > 0);
			_copperLabel.set_Text(c.ToString());
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			int textW = 342;
			int y = 9;
			int headerH = Math.Max(36, 24);
			((Control)_itemIcon).set_Location(new Point(9, y + (headerH - 36) / 2));
			int nameX = 49;
			((Control)_nameLabel).set_Location(new Point(nameX, y + (headerH - 24) / 2));
			((Control)_nameLabel).set_Size(new Point(360 - nameX - 9, 24));
			y += headerH + 3;
			if (((Control)_descLabel).get_Visible())
			{
				y += 4;
				BitmapFont font = _descLabel.get_Font();
				float spW = font.MeasureString(" ").Width;
				int lines = 0;
				string[] array = _descLabel.get_Text().Split('\n');
				foreach (string obj in array)
				{
					int paraLines = 1;
					float lineW = 0f;
					string[] array2 = obj.Split(' ');
					foreach (string word in array2)
					{
						if (word.Length != 0)
						{
							float wordW = font.MeasureString(word).Width;
							if (lineW > 0f && lineW + spW + wordW > (float)textW)
							{
								paraLines++;
								lineW = wordW;
							}
							else
							{
								lineW += ((lineW > 0f) ? spW : 0f) + wordW;
							}
						}
					}
					lines += paraLines;
				}
				lines = Math.Max(1, lines);
				((Control)_descLabel).set_Location(new Point(9, y));
				((Control)_descLabel).set_Size(new Point(textW, 18 * lines));
				y += 18 * lines + 3;
			}
			Place(_durLabel, 18);
			Place(_bindLabel, 18);
			int x;
			int coinY;
			if (((Control)_copperIcon).get_Visible())
			{
				x = 9;
				coinY = y;
				PlaceCoin(_goldIcon, _goldLabel);
				PlaceCoin(_silverIcon, _silverLabel);
				PlaceCoin(_copperIcon, _copperLabel);
				y += 21;
			}
			((Control)this).set_Height(y + 9);
			void Place(Label lbl, int lineH)
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				if (((Control)lbl).get_Visible())
				{
					((Control)lbl).set_Location(new Point(9, y));
					((Control)lbl).set_Size(new Point(textW, lineH));
					y += lineH + 3;
				}
			}
			void PlaceCoin(Image icon, Label label)
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				if (((Control)icon).get_Visible())
				{
					((Control)icon).set_Location(new Point(x, coinY));
					x += 20;
					int labelW = label.get_Text().Length * 8 + 4;
					((Control)label).set_Location(new Point(x, y));
					((Control)label).set_Size(new Point(labelW, 18));
					x += labelW + 4;
				}
			}
		}

		public void MoveTo(int mouseX, int mouseY)
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			Screen screen = GameService.Graphics.get_SpriteScreen();
			int x = ((mouseX + 360 > ((Control)screen).get_Width()) ? (mouseX - 360) : mouseX);
			int y = ((mouseY - ((Control)this).get_Height() < 0) ? mouseY : (mouseY - ((Control)this).get_Height()));
			((Control)this).set_Location(new Point(Math.Max(0, x), Math.Max(0, y)));
		}

		private static string FormatDuration(int secs)
		{
			int h = secs / 3600;
			int i = secs % 3600 / 60;
			if (h > 0 && i > 0)
			{
				return string.Format(Strings.Get("Duration_HoursMinutes"), h, i);
			}
			if (h > 0)
			{
				return string.Format(Strings.Get("Duration_HoursOnly"), h);
			}
			return string.Format(Strings.Get("Duration_MinutesOnly"), i);
		}

		private static string FormatBinding(string binding)
		{
			return binding switch
			{
				"AccountBound" => Strings.Get("Binding_AccountBound"), 
				"SoulboundOnAcquire" => Strings.Get("Binding_SoulboundOnAcquire"), 
				"SoulboundOnUse" => Strings.Get("Binding_SoulboundOnUse"), 
				_ => "", 
			};
		}

		internal static Color GetRarityColor(string rarity)
		{
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			if (rarity != null)
			{
				switch (rarity.Length)
				{
				case 4:
					switch (rarity[0])
					{
					case 'J':
						if (!(rarity == "Junk"))
						{
							break;
						}
						return new Color(170, 170, 170);
					case 'F':
						if (!(rarity == "Fine"))
						{
							break;
						}
						return new Color(98, 164, 218);
					case 'R':
						if (!(rarity == "Rare"))
						{
							break;
						}
						return new Color(252, 208, 11);
					}
					break;
				case 10:
					if (!(rarity == "Masterwork"))
					{
						break;
					}
					return new Color(26, 147, 6);
				case 6:
					if (!(rarity == "Exotic"))
					{
						break;
					}
					return new Color(255, 164, 5);
				case 8:
					if (!(rarity == "Ascended"))
					{
						break;
					}
					return new Color(251, 62, 141);
				case 9:
					if (!(rarity == "Legendary"))
					{
						break;
					}
					return new Color(76, 19, 157);
				}
			}
			return Color.get_White();
		}
	}
}
