using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Gw2Sharp.Mumble.Models;
using Kenedia.Modules.QoL.Classes;
using Kenedia.Modules.QoL.Strings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Kenedia.Modules.QoL.SubModules
{
	public class Resets : SubModule
	{
		private CustomFlowPanel Container;

		private IconLabel ServerReset;

		private IconLabel WeeklyReset;

		public SettingEntry<Point> ControlPosition;

		public Resets()
		{
			Name = "Resets";
			ModuleIcon = QoL.ModuleInstance.TextureManager.getIcon("Resets", _Icons.ModuleIcon);
			ModuleIconHovered = QoL.ModuleInstance.TextureManager.getIcon("Resets", _Icons.ModuleIcon_HoveredWhite);
			ModuleIcon_Active = QoL.ModuleInstance.TextureManager.getIcon("Resets", _Icons.ModuleIcon_Active);
			ModuleIconHovered_Active = QoL.ModuleInstance.TextureManager.getIcon("Resets", _Icons.ModuleIcon_Active_HoveredWhite);
		}

		private void EnsureControlsOnScreen()
		{
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
				Rectangle localBounds = ((Control)GameService.Graphics.get_SpriteScreen()).get_LocalBounds();
				if (((Rectangle)(ref localBounds)).Contains(((Control)Container).get_Location()) && ((Rectangle)(ref localBounds)).Contains(((Control)Container).get_Location().Add(((Control)Container).get_Size())))
				{
					ControlPosition.set_Value(((Control)Container).get_Location());
				}
				else
				{
					((Control)Container).set_Location(new Point(Math.Max(0, Math.Min(((Rectangle)(ref localBounds)).get_Right() - ((Control)Container).get_Width(), ((Control)Container).get_Location().X)), Math.Max(0, Math.Min(((Rectangle)(ref localBounds)).get_Bottom() - ((Control)Container).get_Height(), ((Control)Container).get_Location().Y))));
				}
			});
		}

		private void Container_Shown(object sender, EventArgs e)
		{
			EnsureControlsOnScreen();
		}

		private void Container_Moved(object sender, MovedEventArgs e)
		{
			if (Loaded)
			{
				EnsureControlsOnScreen();
			}
		}

		public override void DefineSettings(SettingCollection settings)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			base.DefineSettings(settings);
			ToggleModule_Key = settings.DefineSetting<KeyBinding>(Name + "ToggleModule_Key", new KeyBinding((Keys)0), (Func<string>)(() => string.Format(common.Toggle, Name)), (Func<string>)null);
			Enabled = settings.DefineSetting<bool>(Name + "Enabled", true, (Func<string>)(() => string.Format(common.Enable_Name, Name)), (Func<string>)(() => string.Format(common.Enable_Tooltip, Name)));
			ShowOnBar = settings.DefineSetting<bool>(Name + "ShowOnBar", true, (Func<string>)(() => string.Format(common.ShowIcon_Name, Name)), (Func<string>)(() => string.Format(common.ShowIcon_Tooltip, Name)));
			SettingCollection internal_settings = settings.AddSubCollection("Internal Settings " + Name, false);
			ControlPosition = internal_settings.DefineSetting<Point>("ControlPosition", new Point(150, 150), (Func<string>)null, (Func<string>)null);
		}

		public override void Dispose()
		{
			CustomFlowPanel container = Container;
			if (container != null)
			{
				((Control)container).Dispose();
			}
			IconLabel weeklyReset = WeeklyReset;
			if (weeklyReset != null)
			{
				((Control)weeklyReset).Dispose();
			}
			IconLabel serverReset = ServerReset;
			if (serverReset != null)
			{
				((Control)serverReset).Dispose();
			}
			GameService.Gw2Mumble.get_UI().remove_UISizeChanged((EventHandler<ValueEventArgs<UiSize>>)UI_UISizeChanged);
			base.Dispose();
		}

		public override void Initialize()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			base.Initialize();
			RectangleF tRect = GameService.Content.get_DefaultFont14().GetStringRectangle("7 Tage 00:00:00");
			CustomFlowPanel customFlowPanel = new CustomFlowPanel();
			((Control)customFlowPanel).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)customFlowPanel).set_Visible(base.Active);
			((Control)customFlowPanel).set_Width((int)(tRect.Width + 6f + tRect.Height));
			((Container)customFlowPanel).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)customFlowPanel).set_FlowDirection((ControlFlowDirection)0);
			customFlowPanel.Background = QoL.ModuleInstance.TextureManager.getBackground(_Backgrounds.Tooltip);
			((FlowPanel)customFlowPanel).set_OuterControlPadding(new Vector2(2f, 2f));
			((FlowPanel)customFlowPanel).set_ControlPadding(new Vector2(0f, 2f));
			((Control)customFlowPanel).set_Location(ControlPosition.get_Value());
			Container = customFlowPanel;
			((Control)Container).add_Moved((EventHandler<MovedEventArgs>)Container_Moved);
			((Control)Container).add_Shown((EventHandler<EventArgs>)Container_Shown);
			IconLabel iconLabel = new IconLabel();
			((Control)iconLabel).set_Parent((Container)(object)Container);
			iconLabel.Texture = QoL.ModuleInstance.TextureManager.getIcon(_Icons.TyriaDayNight);
			iconLabel.Text = "00:00:00";
			((Control)iconLabel).set_BasicTooltipText("Server Reset");
			iconLabel.AutoSize = true;
			ServerReset = iconLabel;
			IconLabel iconLabel2 = new IconLabel();
			((Control)iconLabel2).set_Parent((Container)(object)Container);
			iconLabel2.Texture = QoL.ModuleInstance.TextureManager.getIcon(_Icons.Calendar);
			iconLabel2.Text = "0 days 00:00:00";
			((Control)iconLabel2).set_BasicTooltipText("Weekly Reset");
			iconLabel2.AutoSize = true;
			WeeklyReset = iconLabel2;
			GameService.Gw2Mumble.get_UI().add_UISizeChanged((EventHandler<ValueEventArgs<UiSize>>)UI_UISizeChanged);
			LoadData();
		}

		private void UI_UISizeChanged(object sender, ValueEventArgs<UiSize> e)
		{
			EnsureControlsOnScreen();
		}

		public override void ToggleModule()
		{
			base.ToggleModule();
			((Control)Container).set_Visible(base.Active);
		}

		public override void LoadData()
		{
			Loaded = true;
		}

		public override void Update(GameTime gameTime)
		{
			DateTime now = DateTime.UtcNow;
			DateTime nextDay = DateTime.UtcNow.AddDays(1.0);
			DateTime nextWeek = DateTime.UtcNow;
			for (int i = 0; i < 8; i++)
			{
				nextWeek = DateTime.UtcNow.AddDays(i);
				if (nextWeek.DayOfWeek == DayOfWeek.Monday && (nextWeek.Day != now.Day || now.Hour < 7 || (now.Hour == 7 && now.Minute < 30)))
				{
					break;
				}
			}
			DateTime t = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 0, 0, 0);
			TimeSpan weeklyReset = new DateTime(nextWeek.Year, nextWeek.Month, nextWeek.Day, 7, 30, 0).Subtract(now);
			WeeklyReset.Text = string.Format("{1:0} {0} {2:00}:{3:00}:{4:00}", common.days, weeklyReset.Days, weeklyReset.Hours, weeklyReset.Minutes, weeklyReset.Seconds);
			TimeSpan serverReset = t.Subtract(now);
			ServerReset.Text = $"{serverReset.Hours:00}:{serverReset.Minutes:00}:{serverReset.Seconds:00}";
		}

		public override void UpdateLanguage(object sender, EventArgs e)
		{
			base.UpdateLanguage(sender, e);
		}
	}
}
