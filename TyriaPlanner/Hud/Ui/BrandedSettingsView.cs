using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TyriaPlanner.Hud.Ui
{
	public sealed class BrandedSettingsView : View
	{
		private const string SiteUrl = "https://tyriaplanner.com";

		private const int HeaderHeight = 80;

		private readonly SettingCollection _settings;

		private readonly Texture2D _icon;

		public BrandedSettingsView(SettingCollection settings, Texture2D icon)
			: this()
		{
			_settings = settings;
			_icon = icon;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val).set_Height(80);
			((Control)val).set_BackgroundColor(new Color(20, 20, 26, 255));
			Panel header = val;
			if (_icon != null)
			{
				Image val2 = new Image();
				((Control)val2).set_Parent((Container)(object)header);
				val2.set_Texture(AsyncTexture2D.op_Implicit(_icon));
				((Control)val2).set_Location(new Point(12, 12));
				((Control)val2).set_Size(new Point(56, 56));
				((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SafeUrl.Open("https://tyriaplanner.com");
				});
			}
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)header);
			val3.set_Text("Tyria Planner");
			val3.set_Font(GameService.Content.get_DefaultFont18());
			val3.set_TextColor(Color.get_Goldenrod());
			((Control)val3).set_Location(new Point(80, 14));
			((Control)val3).set_Width(((Control)header).get_Width() - 92);
			((Control)val3).set_Height(28);
			val3.set_AutoSizeWidth(false);
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SafeUrl.Open("https://tyriaplanner.com");
			});
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)header);
			val4.set_Text("tyriaplanner.com  ·  click to open");
			val4.set_Font(GameService.Content.get_DefaultFont14());
			val4.set_TextColor(new Color(180, 200, 255));
			((Control)val4).set_Location(new Point(80, 42));
			((Control)val4).set_Width(((Control)header).get_Width() - 92);
			((Control)val4).set_Height(22);
			val4.set_AutoSizeWidth(false);
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SafeUrl.Open("https://tyriaplanner.com");
			});
			ViewContainer val5 = new ViewContainer();
			((Control)val5).set_Parent(buildPanel);
			((Control)val5).set_Location(new Point(0, 84));
			((Control)val5).set_Size(new Point(buildPanel.get_ContentRegion().Width, buildPanel.get_ContentRegion().Height - 80 - 4));
			val5.Show((IView)new SettingsView(_settings, -1));
		}
	}
}
