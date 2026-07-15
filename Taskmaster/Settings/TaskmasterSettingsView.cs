using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using Taskmaster.Models;
using Taskmaster.Services;
using Taskmaster.UI;

namespace Taskmaster.Settings
{
	public class TaskmasterSettingsView : View
	{
		private readonly ModuleSettings _settings;

		public TaskmasterSettingsView(ModuleSettings settings)
			: this()
		{
			_settings = settings;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			int y = 10;
			SettingEntry[] array = (SettingEntry[])(object)new SettingEntry[3]
			{
				(SettingEntry)_settings.ToggleWindow,
				(SettingEntry)_settings.UnfocusedOpacity,
				(SettingEntry)_settings.ShowOnMap
			};
			for (int i = 0; i < array.Length; i++)
			{
				IView view = SettingView.FromType(array[i], ((Control)buildPanel).get_Width());
				if (view != null)
				{
					ViewContainer val = new ViewContainer();
					((Control)val).set_Parent(buildPanel);
					((Control)val).set_Location(new Point(10, y));
					((Control)val).set_Width(((Control)buildPanel).get_Width() - 20);
					((Control)val).set_Height(32);
					val.Show(view);
					y += 38;
				}
			}
			y += 14;
			Label val2 = new Label();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Location(new Point(10, y));
			((Control)val2).set_Width(((Control)buildPanel).get_Width() - 20);
			((Control)val2).set_Height(24);
			val2.set_Text("Reset engine status (server time is UTC)");
			val2.set_ShowShadow(true);
			y += 26;
			DateTime nowUtc = DateTime.UtcNow;
			(string, ResetScheduleType)[] array2 = new(string, ResetScheduleType)[6]
			{
				("Daily reset", ResetScheduleType.DailyServer),
				("Weekly reset", ResetScheduleType.WeeklyServer),
				("Map bonus rotation", ResetScheduleType.MapBonus),
				("PSNA rotation", ResetScheduleType.Psna),
				("WvW EU reset", ResetScheduleType.WvwEu),
				("WvW NA reset", ResetScheduleType.WvwNa)
			};
			for (int i = 0; i < array2.Length; i++)
			{
				(string, ResetScheduleType) line = array2[i];
				DateTime? next = ResetEngine.NextBoundary(new TodoTask
				{
					Schedule = line.Item2
				}, nowUtc);
				string text = (next.HasValue ? $"{line.Item1}: in {TaskRow.FormatCountdown(next.Value - nowUtc)} ({next.Value:ddd HH:mm} UTC)" : (line.Item1 + ": n/a"));
				Label val3 = new Label();
				((Control)val3).set_Parent(buildPanel);
				((Control)val3).set_Location(new Point(20, y));
				((Control)val3).set_Width(((Control)buildPanel).get_Width() - 30);
				((Control)val3).set_Height(20);
				val3.set_Text(text);
				y += 22;
			}
			Label val4 = new Label();
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Location(new Point(20, y));
			((Control)val4).set_Width(((Control)buildPanel).get_Width() - 30);
			((Control)val4).set_Height(20);
			val4.set_Text($"Module clock now: {nowUtc:yyyy-MM-dd HH:mm} UTC / {TimeZoneInfo.ConvertTimeFromUtc(nowUtc, TimeZoneInfo.Local):HH:mm} local");
		}
	}
}
