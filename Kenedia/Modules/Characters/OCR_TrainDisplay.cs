using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters
{
	public class OCR_TrainDisplay : Panel
	{
		private static readonly Random s_random = new Random();

		private MultilineTextBox textBox;

		private readonly string _characterString;

		private readonly char[] _characters = new char[96]
		{
			'A', 'a', 'B', 'b', 'C', 'c', 'D', 'd', 'E', 'e',
			'F', 'f', 'G', 'g', 'H', 'h', 'I', 'i', 'J', 'j',
			'K', 'k', 'L', 'l', 'M', 'm', 'N', 'n', 'O', 'o',
			'P', 'p', 'Q', 'q', 'R', 'r', 'S', 's', 'T', 't',
			'U', 'u', 'V', 'v', 'W', 'w', 'X', 'x', 'Y', 'y',
			'Z', 'z', 'Á', 'á', 'Â', 'â', 'Ä', 'ä', 'À', 'à',
			'Æ', 'æ', 'Ç', 'ç', 'Ê', 'ê', 'É', 'é', 'Ë', 'ë',
			'È', 'è', 'Ï', 'ï', 'Í', 'í', 'Î', 'î', 'Ñ', 'ñ',
			'Œ', 'œ', 'Ô', 'ô', 'Ö', 'ö', 'Ó', 'ó', 'Ú', 'ú',
			'Ü', 'ü', 'Û', 'û', 'Ù', 'ù'
		};

		public string RandomString(int length)
		{
			return new string((from s in Enumerable.Repeat(_characterString, length)
				select s[s_random.Next(s.Length)]).ToArray());
		}

		public OCR_TrainDisplay()
			: this()
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Expected O, but got Unknown
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Expected O, but got Unknown
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04de: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_050e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0515: Unknown result type (might be due to invalid IL or missing references)
			//IL_0520: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			//IL_0537: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Unknown result type (might be due to invalid IL or missing references)
			//IL_0546: Unknown result type (might be due to invalid IL or missing references)
			//IL_054b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0555: Unknown result type (might be due to invalid IL or missing references)
			//IL_055c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0581: Unknown result type (might be due to invalid IL or missing references)
			//IL_0588: Unknown result type (might be due to invalid IL or missing references)
			//IL_0593: Unknown result type (might be due to invalid IL or missing references)
			//IL_059a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05be: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05de: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0600: Unknown result type (might be due to invalid IL or missing references)
			//IL_060a: Unknown result type (might be due to invalid IL or missing references)
			//IL_060f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0614: Unknown result type (might be due to invalid IL or missing references)
			//IL_061e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0625: Unknown result type (might be due to invalid IL or missing references)
			//IL_062d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0634: Unknown result type (might be due to invalid IL or missing references)
			//IL_063f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0646: Unknown result type (might be due to invalid IL or missing references)
			//IL_0656: Unknown result type (might be due to invalid IL or missing references)
			//IL_0660: Unknown result type (might be due to invalid IL or missing references)
			//IL_0667: Expected O, but got Unknown
			//IL_066d: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Width(((Control)GameService.Graphics.get_SpriteScreen()).get_Width());
			((Control)this).set_Height(((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			((Control)this).set_ZIndex(0);
			((Control)this).set_Visible(true);
			((Control)this).set_BackgroundColor(Color.get_White());
			_characterString = new string(_characters);
			_characterString += " ";
			string text = "";
			for (int i = 0; i < 50; i++)
			{
				text = text + RandomString(s_random.Next(3, 20)) + " ";
			}
			MultilineTextBox val = new MultilineTextBox();
			((Control)val).set_Parent((Container)(object)this);
			((TextInputBase)val).set_Text(text);
			((Control)val).set_Width(((Control)Control.get_Graphics().get_SpriteScreen()).get_Width());
			((Control)val).set_Height(30);
			textBox = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Location(new Point(10, 50));
			val2.set_ControlPadding(new Vector2(50f, 50f));
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Width(((Control)Control.get_Graphics().get_SpriteScreen()).get_Width() - 20);
			((Control)val2).set_Height(((Control)Control.get_Graphics().get_SpriteScreen()).get_Height());
			FlowPanel flow = val2;
			Label val3 = new Label();
			((Control)val3).set_Location(new Point(10, 30));
			((Control)val3).set_Parent((Container)(object)flow);
			val3.set_Text(text);
			val3.set_WrapText(true);
			((Control)val3).set_Width(((Control)Control.get_Graphics().get_SpriteScreen()).get_Width() - 20);
			val3.set_AutoSizeHeight(true);
			val3.set_Font(GameService.Content.get_DefaultFont12());
			val3.set_TextColor(Color.get_Black());
			((Control)val3).set_Visible(false);
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(10, 30));
			((Control)val4).set_Parent((Container)(object)flow);
			val4.set_Text(text);
			val4.set_WrapText(true);
			((Control)val4).set_Width(((Control)Control.get_Graphics().get_SpriteScreen()).get_Width() - 20);
			val4.set_AutoSizeHeight(true);
			val4.set_Font(GameService.Content.get_DefaultFont16());
			val4.set_TextColor(Color.get_Black());
			((Control)val4).set_Visible(false);
			Label val5 = new Label();
			((Control)val5).set_Location(new Point(10, 30));
			((Control)val5).set_Parent((Container)(object)flow);
			val5.set_Text("Kyleigh Stirling");
			val5.set_WrapText(true);
			((Control)val5).set_Width(((Control)Control.get_Graphics().get_SpriteScreen()).get_Width() - 20);
			val5.set_AutoSizeHeight(true);
			val5.set_Font(GameService.Content.get_DefaultFont32());
			val5.set_TextColor(Color.get_Black());
			((Control)val5).set_Visible(false);
			string upper = "A   B   C   D   E   F   G   H   I   J   K   L   M   N   O   P   Q   R   S   T   U   V   W   X   Y   Z   Á   Â   Ä   À   Æ   Ç   Ê   É   Ë   È   Ï   Í   Î   Ñ   Œ   Ô   Ö   Ó   Ú   Ü   Û   Ù";
			string lower = "a   b   c   d   e   f   g   h   i   j   k   l   m   n   o   p   q   r   s   t   u   v   w   x   y   z   á   â   ä   à   æ   ç   ê   é   ë   è   ï   í   î   ñ   œ   ô   ö   ó   ú   ü   û   ù";
			string numbers = "1   2   3   4   5   6   7   8   9   0";
			string kene = "      Keñedia Sand\r\n      Keñedia Martell\r\n      Kenedira Stark\r\n      Kenedia Tyrell\r\n      Keñedira Tyrell\r\n      Keñedia Tyrell\r\n      Keñedira Martell\r\n      Kénedia Martell\r\n      Kenedia Key\r\n      Kénedira Sand\r\n      Loot Bae\r\n      Keñedira Stark\r\n      Keñedina Stark\r\n      Kénedia Stark\r\n      Emira Sand\r\n      Keñedir Arryn\r\n      Keñedira Sand\r\n      Kenedia Stark\r\n      Kenedina Stark\r\n      Kenedir Stark\r\n      Keñedira Arryn\r\n      Keñedir Stark\r\n      Kénedina Stark\r\n      Kenedina Sand\r\n      Kenedira Sand\r\n      Ser Tristan Stark\r\n      Kenediar Stark\r\n      Kenedia Arryn\r\n      Kenedias Stark\r\n      Lady Anissa Stark\r\n      Kenedira Arryn\r\n      Lady Adriana Stark\r\n      Kenedias Arryn\r\n      Keñedia Arryn\r\n      Keñedias Stark\r\n      Keñedia Stark";
			string kene2 = "      Kénedira Tyrell\r\n      Kénedira Arryn\r\n      Kénedias Stark\r\n      Kénedias Arryn\r\n      Kénedia Arryn\r\n      Kénedia Sand\r\n      Kénedia Tyrell\r\n      Keñedina Arryn\r\n      Keñediar Arryn\r\n      Keñedina Tyrell\r\n      Keñedina Sand\r\n      Kénedina Sand\r\n      Kénediar Arryn\r\n      Kénediar Stark\r\n      Kénedina Tyrell\r\n      Kénedina Arryn\r\n      Kénedira Stark\r\n      Keñediar Stark\r\n      Kenedir Arryn\r\n      Kenedina Tyrell\r\n      Kénedir Arryn\r\n      Kenediæ Stark\r\n      Kenedia Sand\r\n      Lady Arianne Stark\r\n      Keñedias Arryn\r\n      Kenedina Arryn\r\n      Kénedir Stark\r\n      Kenedira Tyrell\r\n      Kenediar Arryn\r\n      Kenedia Martell\r\n      Kénedina Martell\r\n      Keñedina Martell\r\n      Kenedina Martell\r\n      Kénedira Martell\r\n      Kenedira Martell";
			List<string> smithkt = new List<string>
			{
				"Kyle Stirling", "Keane Stark", "Keijj", "Kaireadi", "Kaolin Sthairdottir", "Kraedon", "Koreia Swiftmaker", "Lightbringer Kagg", "Kehha", "Kora Strongforge",
				"Kaibeart", "Kassair", "Keeper Kiwwa", "Kendra Stirling", "Korva Shadowreign", "Katie Stirling", "Keallache", "Peacemaker Keig", "Kaolin Svartrson", "Kelena Striperazor",
				"Karressa Stirling", "Khloe Stirling", "Kasendra Stoneheart", "Knarlg Soulburn", "Kniukr Steelshout", "Kyri Sunderbane", "Kyleigh Stirling", "Konya Shioko", "Kiandra Stirling", "Konum Steelbringer",
				"Kyto Sureshot", "Ks Bags", "Kurudum Swordshield", "Krumr Styrbiornnson", "Kzaira", "Kelyf", "Kar Steelwound", "Kosurr Swiftbreeze", "Krandubh", "Kiiven",
				"Kendric Stirling", "Keirdrea", "Kuiren", "Kylie Stoneheart", "Kerrana Bonelasher", "Kate Stirling", "Arcanist Kaivva", "Kkiven"
			};
			Label val6 = new Label();
			((Control)val6).set_Location(new Point(10, 30));
			((Control)val6).set_Parent((Container)(object)flow);
			val6.set_Text(string.Join("\n", smithkt.GetRange(0, 36)));
			val6.set_WrapText(true);
			((Control)val6).set_Width(400);
			val6.set_AutoSizeHeight(true);
			val6.set_Font(GameService.Content.get_DefaultFont32());
			val6.set_TextColor(Color.get_Black());
			Label val7 = new Label();
			((Control)val7).set_Location(new Point(10, 30));
			((Control)val7).set_Parent((Container)(object)flow);
			val7.set_Text(string.Join("\n", smithkt.GetRange(36, smithkt.Count - 1 - 36)));
			val7.set_WrapText(true);
			((Control)val7).set_Width(400);
			val7.set_AutoSizeHeight(true);
			val7.set_Font(GameService.Content.get_DefaultFont32());
			val7.set_TextColor(Color.get_Black());
			Label val8 = new Label();
			((Control)val8).set_Location(new Point(10, 30));
			((Control)val8).set_Parent((Container)(object)flow);
			val8.set_Text(kene);
			val8.set_WrapText(true);
			((Control)val8).set_Width(400);
			val8.set_AutoSizeHeight(true);
			val8.set_Font(GameService.Content.get_DefaultFont32());
			val8.set_TextColor(Color.get_Black());
			Label val9 = new Label();
			((Control)val9).set_Location(new Point(10, 30));
			((Control)val9).set_Parent((Container)(object)flow);
			val9.set_Text(kene2);
			val9.set_WrapText(true);
			((Control)val9).set_Width(400);
			val9.set_AutoSizeHeight(true);
			val9.set_Font(GameService.Content.get_DefaultFont32());
			val9.set_TextColor(Color.get_Black());
			Label val10 = new Label();
			((Control)val10).set_Location(new Point(10, 30));
			((Control)val10).set_Parent((Container)(object)flow);
			val10.set_Text(string.Format("{1}{0}{0}{2}{0}{0}{3}", Environment.NewLine, upper, lower, numbers));
			val10.set_WrapText(true);
			((Control)val10).set_Width(((Control)Control.get_Graphics().get_SpriteScreen()).get_Width() - 20);
			val10.set_AutoSizeHeight(true);
			val10.set_Font(GameService.Content.get_DefaultFont32());
			val10.set_TextColor(Color.get_Black());
		}
	}
}
