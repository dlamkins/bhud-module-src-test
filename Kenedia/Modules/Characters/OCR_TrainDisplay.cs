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
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0537: Unknown result type (might be due to invalid IL or missing references)
			//IL_054b: Unknown result type (might be due to invalid IL or missing references)
			//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_05be: Unknown result type (might be due to invalid IL or missing references)
			//IL_0600: Unknown result type (might be due to invalid IL or missing references)
			//IL_0614: Unknown result type (might be due to invalid IL or missing references)
			//IL_0656: Unknown result type (might be due to invalid IL or missing references)
			//IL_066d: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
			base.Parent = GameService.Graphics.SpriteScreen;
			base.Width = GameService.Graphics.SpriteScreen.Width;
			base.Height = GameService.Graphics.SpriteScreen.Height;
			ZIndex = 0;
			base.Visible = true;
			base.BackgroundColor = Color.get_White();
			_characterString = new string(_characters);
			_characterString += " ";
			string text = "";
			for (int i = 0; i < 50; i++)
			{
				text = text + RandomString(s_random.Next(3, 20)) + " ";
			}
			textBox = new MultilineTextBox
			{
				Parent = this,
				Text = text,
				Width = Control.Graphics.SpriteScreen.Width,
				Height = 30
			};
			FlowPanel flow = new FlowPanel
			{
				Location = new Point(10, 50),
				ControlPadding = new Vector2(50f, 50f),
				Parent = this,
				Width = Control.Graphics.SpriteScreen.Width - 20,
				Height = Control.Graphics.SpriteScreen.Height
			};
			new Label
			{
				Location = new Point(10, 30),
				Parent = flow,
				Text = text,
				WrapText = true,
				Width = Control.Graphics.SpriteScreen.Width - 20,
				AutoSizeHeight = true,
				Font = GameService.Content.DefaultFont12,
				TextColor = Color.get_Black(),
				Visible = false
			};
			new Label
			{
				Location = new Point(10, 30),
				Parent = flow,
				Text = text,
				WrapText = true,
				Width = Control.Graphics.SpriteScreen.Width - 20,
				AutoSizeHeight = true,
				Font = GameService.Content.DefaultFont16,
				TextColor = Color.get_Black(),
				Visible = false
			};
			new Label
			{
				Location = new Point(10, 30),
				Parent = flow,
				Text = "Kyleigh Stirling",
				WrapText = true,
				Width = Control.Graphics.SpriteScreen.Width - 20,
				AutoSizeHeight = true,
				Font = GameService.Content.DefaultFont32,
				TextColor = Color.get_Black(),
				Visible = false
			};
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
			new Label
			{
				Location = new Point(10, 30),
				Parent = flow,
				Text = string.Join("\n", smithkt.GetRange(0, 36)),
				WrapText = true,
				Width = 400,
				AutoSizeHeight = true,
				Font = GameService.Content.DefaultFont32,
				TextColor = Color.get_Black()
			};
			new Label
			{
				Location = new Point(10, 30),
				Parent = flow,
				Text = string.Join("\n", smithkt.GetRange(36, smithkt.Count - 1 - 36)),
				WrapText = true,
				Width = 400,
				AutoSizeHeight = true,
				Font = GameService.Content.DefaultFont32,
				TextColor = Color.get_Black()
			};
			new Label
			{
				Location = new Point(10, 30),
				Parent = flow,
				Text = kene,
				WrapText = true,
				Width = 400,
				AutoSizeHeight = true,
				Font = GameService.Content.DefaultFont32,
				TextColor = Color.get_Black()
			};
			new Label
			{
				Location = new Point(10, 30),
				Parent = flow,
				Text = kene2,
				WrapText = true,
				Width = 400,
				AutoSizeHeight = true,
				Font = GameService.Content.DefaultFont32,
				TextColor = Color.get_Black()
			};
			Label label = new Label
			{
				Location = new Point(10, 30),
				Parent = flow,
				Text = string.Format("{1}{0}{0}{2}{0}{0}{3}", Environment.NewLine, upper, lower, numbers),
				WrapText = true,
				Width = Control.Graphics.SpriteScreen.Width - 20,
				AutoSizeHeight = true,
				Font = GameService.Content.DefaultFont32,
				TextColor = Color.get_Black()
			};
		}
	}
}
