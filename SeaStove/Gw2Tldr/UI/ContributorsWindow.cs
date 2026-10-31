using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;

namespace SeaStove.Gw2Tldr.UI
{
	internal sealed class ContributorsWindow : IDisposable
	{
		private readonly UtilityWindow _window;

		public ContributorsWindow(Action<string> openLink)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Expected O, but got Unknown
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Expected O, but got Unknown
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			_window = new UtilityWindow("Contributors", "GW2 TLDR", new Point(300, 180));
			DarkButton darkButton = new DarkButton();
			((Control)darkButton).set_Parent((Container)(object)_window.Body);
			((Control)darkButton).set_Location(new Point(15, 8));
			((Control)darkButton).set_Size(new Point(440, 26));
			((Label)darkButton).set_Text("Submit corrections or update your contributor listing");
			((Control)darkButton).set_BasicTooltipText("Open feedback form");
			((Control)darkButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				openLink("https://forms.gle/YgpeijBaASgATMD1A");
			});
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_window.Body);
			((Control)val).set_Location(new Point(15, 45));
			((Control)val).set_Width(480);
			val.set_WrapText(true);
			val.set_AutoSizeHeight(true);
			val.set_Text("Thank you to everyone who helps improve the guides!");
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_window.Body);
			((Control)val2).set_Location(new Point(15, 82));
			((Control)val2).set_Size(new Point(485, 453));
			val2.set_CanScroll(true);
			Panel list = val2;
			using (Stream stream = typeof(ContributorsWindow).Assembly.GetManifestResourceStream("SeaStove.Gw2Tldr.contributors.json"))
			{
				using StreamReader reader = new StreamReader(stream);
				IOrderedEnumerable<JToken> contributors = from item in ((IEnumerable<JToken>)JArray.Parse(reader.ReadToEnd())).Where(delegate(JToken item)
					{
						//IL_0011: Unknown result type (might be due to invalid IL or missing references)
						//IL_0017: Invalid comparison between Unknown and I4
						JToken obj = item.get_Item((object)"name");
						return obj != null && (int)obj.get_Type() == 8;
					})
					orderby ((int?)item.get_Item((object)"contributions")).GetValueOrDefault() descending
					select item;
				int top = 0;
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)list);
				((Control)val3).set_Location(new Point(0, top));
				((Control)val3).set_Width(450);
				val3.set_AutoSizeHeight(true);
				val3.set_WrapText(true);
				val3.set_Text("Special thanks to the Guild Wars 2 Wiki, GuildJen, Hardstuck, Snow Crows, and Mukluk Labs for their guides, and to Arlowynn for his mentorship.");
				Label thanks = val3;
				top += ((Control)thanks).get_Height() + 10;
				string[] array = new string[5] { "wiki.guildwars2.com", "guildjen.com", "hardstuck.gg", "snowcrows.com", "mukluklabs.com" };
				foreach (string source in array)
				{
					string name = source;
					DarkButton darkButton2 = new DarkButton();
					((Control)darkButton2).set_Parent((Container)(object)list);
					((Control)darkButton2).set_Location(new Point(0, top));
					((Control)darkButton2).set_Size(new Point(200, 26));
					((Label)darkButton2).set_Text(name);
					((Control)darkButton2).set_BasicTooltipText("Open " + name);
					((Control)darkButton2).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						openLink("https://" + name);
					});
					top += 30;
				}
				Label val4 = new Label();
				((Control)val4).set_Parent((Container)(object)list);
				((Control)val4).set_Location(new Point(0, top));
				((Control)val4).set_Size(new Point(440, 25));
				val4.set_Text("Community contributors");
				top += 30;
				foreach (JToken contributor in contributors)
				{
					int count = ((int?)contributor.get_Item((object)"contributions")).GetValueOrDefault();
					SearchResultPanel searchResultPanel = new SearchResultPanel();
					((Control)searchResultPanel).set_Parent((Container)(object)list);
					((Control)searchResultPanel).set_Location(new Point(0, top));
					((Control)searchResultPanel).set_Size(new Point(450, 32));
					SearchResultPanel row = searchResultPanel;
					Label val5 = new Label();
					((Control)val5).set_Parent((Container)(object)row);
					((Control)val5).set_Location(new Point(10, 3));
					((Control)val5).set_Size(new Point(325, 26));
					val5.set_Text((string)contributor.get_Item((object)"name"));
					Label val6 = new Label();
					((Control)val6).set_Parent((Container)(object)row);
					((Control)val6).set_Location(new Point(335, 3));
					((Control)val6).set_Size(new Point(105, 26));
					val6.set_Text(count + ((count == 1) ? " contribution" : " contributions"));
					top += 37;
				}
			}
			_window.RefreshTheme();
			((Control)_window).Show();
		}

		public void Update()
		{
			_window.UpdateDrag();
		}

		public void Dispose()
		{
			((Control)_window).Dispose();
		}
	}
}
