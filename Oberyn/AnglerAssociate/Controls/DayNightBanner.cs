using System;
using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Oberyn.AnglerAssociate.Models;
using Oberyn.AnglerAssociate.Services;

namespace Oberyn.AnglerAssociate.Controls
{
	public class DayNightBanner : Panel
	{
		private readonly Cycle _cycle;

		private readonly Dictionary<TimeOfDay, AsyncTexture2D> _largeTextures;

		private readonly Dictionary<TimeOfDay, AsyncTexture2D> _smallTextures;

		private readonly Label _stateLabel;

		private readonly Image _bigImage;

		private readonly Label _upcoming1Label;

		private readonly Image _upcoming1Image;

		private readonly Label _upcoming2Label;

		private readonly Image _upcoming2Image;

		public DayNightBanner(ContentsManager contentsManager, string assetPrefix, string displayName, Cycle cycle)
			: this()
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Expected O, but got Unknown
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Expected O, but got Unknown
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Expected O, but got Unknown
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Expected O, but got Unknown
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Expected O, but got Unknown
			if (cycle == Cycle.Global)
			{
				throw new ArgumentException("Global has no day/night cycle to display.", "cycle");
			}
			((Control)this).set_Width(120);
			((Control)this).set_Height(410);
			_cycle = cycle;
			_largeTextures = LoadTextures(contentsManager, assetPrefix, "l");
			_smallTextures = LoadTextures(contentsManager, assetPrefix, "s");
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(displayName);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(120);
			((Control)val).set_Height(24);
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(0, 24));
			((Control)val2).set_Width(120);
			((Control)val2).set_Height(22);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			_stateLabel = val2;
			Image val3 = new Image();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(15, 48));
			((Control)val3).set_Size(new Point(90, 180));
			_bigImage = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Location(new Point(0, 234));
			((Control)val4).set_Width(120);
			((Control)val4).set_Height(32);
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			_upcoming1Label = val4;
			Image val5 = new Image();
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Location(new Point(15, 268));
			((Control)val5).set_Size(new Point(90, 45));
			_upcoming1Image = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)this);
			((Control)val6).set_Location(new Point(0, 319));
			((Control)val6).set_Width(120);
			((Control)val6).set_Height(32);
			val6.set_HorizontalAlignment((HorizontalAlignment)1);
			_upcoming2Label = val6;
			Image val7 = new Image();
			((Control)val7).set_Parent((Container)(object)this);
			((Control)val7).set_Location(new Point(15, 353));
			((Control)val7).set_Size(new Point(90, 45));
			_upcoming2Image = val7;
			Refresh();
		}

		private static Dictionary<TimeOfDay, AsyncTexture2D> LoadTextures(ContentsManager contentsManager, string assetPrefix, string sizeSuffix)
		{
			Dictionary<TimeOfDay, AsyncTexture2D> dictionary = new Dictionary<TimeOfDay, AsyncTexture2D>();
			dictionary.Add(TimeOfDay.Dawn, AsyncTexture2D.op_Implicit(contentsManager.GetTexture("icons/" + assetPrefix + "_dawn_" + sizeSuffix + ".png")));
			dictionary.Add(TimeOfDay.Day, AsyncTexture2D.op_Implicit(contentsManager.GetTexture("icons/" + assetPrefix + "_day_" + sizeSuffix + ".png")));
			dictionary.Add(TimeOfDay.Dusk, AsyncTexture2D.op_Implicit(contentsManager.GetTexture("icons/" + assetPrefix + "_dusk_" + sizeSuffix + ".png")));
			dictionary.Add(TimeOfDay.Night, AsyncTexture2D.op_Implicit(contentsManager.GetTexture("icons/" + assetPrefix + "_night_" + sizeSuffix + ".png")));
			return dictionary;
		}

		public void Refresh()
		{
			List<(TimeOfDay State, TimeSpan TimeUntilStart)> upcomingStates = TyrianClock.GetUpcomingStates(_cycle, 3);
			(TimeOfDay, TimeSpan) current = upcomingStates[0];
			_stateLabel.set_Text(current.Item1.ToString());
			_bigImage.set_Texture(_largeTextures[current.Item1]);
			(TimeOfDay, TimeSpan) next1 = upcomingStates[1];
			_upcoming1Label.set_Text($"{next1.Item1} in {Format(next1.Item2)}");
			_upcoming1Image.set_Texture(_smallTextures[next1.Item1]);
			(TimeOfDay, TimeSpan) next2 = upcomingStates[2];
			_upcoming2Label.set_Text($"{next2.Item1} in {Format(next2.Item2)}");
			_upcoming2Image.set_Texture(_smallTextures[next2.Item1]);
		}

		private static string Format(TimeSpan span)
		{
			return $"{(int)span.TotalHours}:{span.Minutes:D2}:{span.Seconds:D2}";
		}
	}
}
