using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace GW2app
{
	internal class GW2appSettingsView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<GW2appSettingsView>();

		private const int LeftPad = 15;

		private const int ColumnGap = 30;

		private const int TitleTopPad = 8;

		private const int SectionGap = 28;

		private const int DropdownWidth = 140;

		private const int TopPad = 8;

		private readonly SettingEntry<GW2appWindow.WindowTheme> _windowTheme;

		private readonly SettingEntry<int> _bgOpacityPct;

		private readonly SettingEntry<bool> _showAccountName;

		private readonly SettingEntry<bool> _showCopyWaypointsButton;

		private readonly SettingEntry<int> _uiScalePct;

		private readonly Action _onResetScale;

		private EventHandler<ValueChangedEventArgs<int>> _scaleChangedHandler;

		private EventHandler<ValueChangedEventArgs<int>> _opacityChangedHandler;

		private EventHandler<ValueChangedEventArgs<bool>> _accountChangedHandler;

		private EventHandler<ValueChangedEventArgs<bool>> _copyBtnChangedHandler;

		private EventHandler<ValueChangedEventArgs<GW2appWindow.WindowTheme>> _themeChangedHandler;

		public GW2appSettingsView(SettingEntry<GW2appWindow.WindowTheme> windowTheme, SettingEntry<int> bgOpacityPct, SettingEntry<bool> showAccountName, SettingEntry<bool> showCopyWaypointsButton, SettingEntry<int> uiScalePct, Action onResetScale)
			: this()
		{
			_windowTheme = windowTheme;
			_bgOpacityPct = bgOpacityPct;
			_showAccountName = showAccountName;
			_showCopyWaypointsButton = showCopyWaypointsButton;
			_uiScalePct = uiScalePct;
			_onResetScale = onResetScale;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Expected O, but got Unknown
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Expected O, but got Unknown
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Expected O, but got Unknown
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Expected O, but got Unknown
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Expected O, but got Unknown
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Expected O, but got Unknown
			//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Expected O, but got Unknown
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0449: Unknown result type (might be due to invalid IL or missing references)
			//IL_0451: Unknown result type (might be due to invalid IL or missing references)
			//IL_0457: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Unknown result type (might be due to invalid IL or missing references)
			//IL_049b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_050e: Unknown result type (might be due to invalid IL or missing references)
			//IL_051f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0529: Unknown result type (might be due to invalid IL or missing references)
			//IL_052b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0535: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Expected O, but got Unknown
			int columnWidth = (Math.Max(420, ((Control)buildPanel).get_Width() - 30) - 30) / 2;
			int leftX = 15;
			int rightX = 15 + columnWidth + 30;
			Label val = new Label();
			val.set_Text("Appearance");
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_TextColor(Color.get_White());
			val.set_AutoSizeWidth(true);
			((Control)val).set_Location(new Point(leftX, 8));
			((Control)val).set_Parent(buildPanel);
			int ly = 36;
			Label val2 = new Label();
			val2.set_Text("Window background");
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(Color.get_LightGray());
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(leftX, ly + 7));
			((Control)val2).set_Parent(buildPanel);
			Dropdown val3 = new Dropdown();
			((Control)val3).set_Width(140);
			((Control)val3).set_Location(new Point(leftX + columnWidth - 140, ly));
			((Control)val3).set_Parent(buildPanel);
			Dropdown bgDropdown = val3;
			GW2appWindow.WindowTheme[] array = new GW2appWindow.WindowTheme[3]
			{
				GW2appWindow.WindowTheme.Game,
				GW2appWindow.WindowTheme.GW2app,
				GW2appWindow.WindowTheme.Black
			};
			foreach (GW2appWindow.WindowTheme v in array)
			{
				bgDropdown.get_Items().Add(EnumLabel(v));
			}
			bgDropdown.set_SelectedItem(EnumLabel(_windowTheme.get_Value()));
			bgDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object s, ValueChangedEventArgs e)
			{
				if (TryParseEnum<GW2appWindow.WindowTheme>(e.get_CurrentValue(), out var value))
				{
					_windowTheme.set_Value(value);
				}
			});
			ly += 36;
			Label val4 = new Label();
			val4.set_Text(FormatOpacity(_bgOpacityPct.get_Value()));
			val4.set_Font(GameService.Content.get_DefaultFont14());
			val4.set_TextColor(Color.get_LightGray());
			val4.set_AutoSizeWidth(true);
			((Control)val4).set_Location(new Point(leftX, ly));
			((Control)val4).set_Parent(buildPanel);
			Label opacityLabel = val4;
			ly += 22;
			TrackBar val5 = new TrackBar();
			val5.set_MinValue(75f);
			val5.set_MaxValue(100f);
			val5.set_Value((float)_bgOpacityPct.get_Value());
			val5.set_SmallStep(true);
			((Control)val5).set_Width(columnWidth - 16);
			((Control)val5).set_Height(16);
			((Control)val5).set_Location(new Point(leftX, ly));
			((Control)val5).set_Parent(buildPanel);
			TrackBar opacityBar = val5;
			opacityBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
			{
				_bgOpacityPct.set_Value((int)Math.Round(e.get_Value()));
			});
			ly += 28;
			Checkbox val6 = new Checkbox();
			val6.set_Text("Show GW2 account name in list header");
			val6.set_Checked(_showAccountName.get_Value());
			((Control)val6).set_Location(new Point(leftX, ly + 4));
			((Control)val6).set_Parent(buildPanel);
			Checkbox acctCheckbox = val6;
			acctCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_showAccountName.set_Value(e.get_Checked());
			});
			ly += 30;
			Checkbox val7 = new Checkbox();
			val7.set_Text("Show \"Copy waypoints\" button in lists");
			val7.set_Checked(_showCopyWaypointsButton.get_Value());
			((Control)val7).set_Location(new Point(leftX, ly + 4));
			((Control)val7).set_Parent(buildPanel);
			Checkbox copyBtnCheckbox = val7;
			copyBtnCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_showCopyWaypointsButton.set_Value(e.get_Checked());
			});
			ly += 30;
			int val8 = ly;
			Label val9 = new Label();
			val9.set_Text("Sizing");
			val9.set_Font(GameService.Content.get_DefaultFont18());
			val9.set_TextColor(Color.get_White());
			val9.set_AutoSizeWidth(true);
			((Control)val9).set_Location(new Point(rightX, 8));
			((Control)val9).set_Parent(buildPanel);
			int ry = 36;
			Label val10 = new Label();
			val10.set_Text(FormatScale(_uiScalePct.get_Value()));
			val10.set_Font(GameService.Content.get_DefaultFont14());
			val10.set_TextColor(Color.get_LightGray());
			val10.set_AutoSizeWidth(true);
			((Control)val10).set_Location(new Point(rightX, ry));
			((Control)val10).set_Parent(buildPanel);
			Label scaleLabel = val10;
			ry += 22;
			TrackBar val11 = new TrackBar();
			val11.set_MinValue(75f);
			val11.set_MaxValue(125f);
			val11.set_Value((float)_uiScalePct.get_Value());
			val11.set_SmallStep(true);
			((Control)val11).set_Width(columnWidth - 16);
			((Control)val11).set_Height(16);
			((Control)val11).set_Location(new Point(rightX, ry));
			((Control)val11).set_Parent(buildPanel);
			TrackBar trackBar = val11;
			trackBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
			{
				_uiScalePct.set_Value((int)Math.Round(e.get_Value()));
			});
			ry += 24;
			StandardButton val12 = new StandardButton();
			val12.set_Text("Reset to 100%");
			((Control)val12).set_Width(130);
			((Control)val12).set_Height(26);
			((Control)val12).set_Location(new Point(rightX, ry + 6));
			((Control)val12).set_Parent(buildPanel);
			((Control)val12).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_onResetScale();
			});
			int rightBottom = ry + 6 + 26;
			int bottomY = Math.Max(val8, rightBottom) + 24;
			Color brandColor = new Color(255, 123, 198);
			Color brandHover = new Color(229, 110, 178);
			Label val13 = new Label();
			val13.set_Text("Open gw2.app/blish");
			val13.set_Font(GameService.Content.get_DefaultFont16());
			val13.set_TextColor(new Color(20, 4, 13));
			val13.set_HorizontalAlignment((HorizontalAlignment)1);
			val13.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val13).set_Width(200);
			((Control)val13).set_Height(30);
			((Control)val13).set_Location(new Point((((Control)buildPanel).get_Width() - 200) / 2, bottomY));
			((Control)val13).set_BackgroundColor(brandColor);
			((Control)val13).set_Parent(buildPanel);
			Label openSiteBtn = val13;
			((Control)openSiteBtn).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)openSiteBtn).set_BackgroundColor(brandHover);
			});
			((Control)openSiteBtn).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)openSiteBtn).set_BackgroundColor(brandColor);
			});
			((Control)openSiteBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					Process.Start("https://gw2.app/blish");
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to open browser.");
				}
			});
			_scaleChangedHandler = delegate(object s, ValueChangedEventArgs<int> e)
			{
				scaleLabel.set_Text(FormatScale(e.get_NewValue()));
				if ((int)Math.Round(trackBar.get_Value()) != e.get_NewValue())
				{
					trackBar.set_Value((float)e.get_NewValue());
				}
			};
			_uiScalePct.add_SettingChanged(_scaleChangedHandler);
			_accountChangedHandler = delegate(object s, ValueChangedEventArgs<bool> e)
			{
				if (acctCheckbox.get_Checked() != e.get_NewValue())
				{
					acctCheckbox.set_Checked(e.get_NewValue());
				}
			};
			_showAccountName.add_SettingChanged(_accountChangedHandler);
			_copyBtnChangedHandler = delegate(object s, ValueChangedEventArgs<bool> e)
			{
				if (copyBtnCheckbox.get_Checked() != e.get_NewValue())
				{
					copyBtnCheckbox.set_Checked(e.get_NewValue());
				}
			};
			_showCopyWaypointsButton.add_SettingChanged(_copyBtnChangedHandler);
			_themeChangedHandler = delegate(object s, ValueChangedEventArgs<GW2appWindow.WindowTheme> e)
			{
				string text = EnumLabel(e.get_NewValue());
				if (bgDropdown.get_SelectedItem() != text)
				{
					bgDropdown.set_SelectedItem(text);
				}
			};
			_windowTheme.add_SettingChanged(_themeChangedHandler);
			_opacityChangedHandler = delegate(object s, ValueChangedEventArgs<int> e)
			{
				opacityLabel.set_Text(FormatOpacity(e.get_NewValue()));
				if ((int)Math.Round(opacityBar.get_Value()) != e.get_NewValue())
				{
					opacityBar.set_Value((float)e.get_NewValue());
				}
			};
			_bgOpacityPct.add_SettingChanged(_opacityChangedHandler);
		}

		protected override void Unload()
		{
			if (_scaleChangedHandler != null && _uiScalePct != null)
			{
				_uiScalePct.remove_SettingChanged(_scaleChangedHandler);
			}
			if (_opacityChangedHandler != null && _bgOpacityPct != null)
			{
				_bgOpacityPct.remove_SettingChanged(_opacityChangedHandler);
			}
			if (_accountChangedHandler != null && _showAccountName != null)
			{
				_showAccountName.remove_SettingChanged(_accountChangedHandler);
			}
			if (_copyBtnChangedHandler != null && _showCopyWaypointsButton != null)
			{
				_showCopyWaypointsButton.remove_SettingChanged(_copyBtnChangedHandler);
			}
			if (_themeChangedHandler != null && _windowTheme != null)
			{
				_windowTheme.remove_SettingChanged(_themeChangedHandler);
			}
		}

		private static string FormatScale(int pct)
		{
			return "List UI scale: " + pct + "%";
		}

		private static string FormatOpacity(int pct)
		{
			return "Background opacity: " + pct + "%";
		}

		private static string EnumLabel<T>(T value) where T : Enum
		{
			MemberInfo[] member = typeof(T).GetMember(value.ToString());
			if (member.Length != 0)
			{
				DescriptionAttribute attr = member[0].GetCustomAttribute<DescriptionAttribute>();
				if (attr != null)
				{
					return attr.Description;
				}
			}
			return value.ToString();
		}

		private static bool TryParseEnum<T>(string label, out T value) where T : struct, Enum
		{
			foreach (T v in Enum.GetValues(typeof(T)))
			{
				if (EnumLabel(v) == label)
				{
					value = v;
					return true;
				}
			}
			value = default(T);
			return false;
		}
	}
}
