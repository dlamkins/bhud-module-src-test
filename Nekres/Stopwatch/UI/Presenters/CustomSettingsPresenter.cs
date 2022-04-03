using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Nekres.Stopwatch.Core.Controls;
using Nekres.Stopwatch.UI.Models;
using Nekres.Stopwatch.UI.Views;
using Stopwatch;

namespace Nekres.Stopwatch.UI.Presenters
{
	public class CustomSettingsPresenter : Presenter<CustomSettingsView, CustomSettingsModel>
	{
		public CustomSettingsPresenter(CustomSettingsView view, CustomSettingsModel model)
			: base(view, model)
		{
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			base.get_View().BrowserButtonClick += View_BrowserButtonClicked;
			base.get_View().PositionButtonClick += View_PositionButtonClicked;
			return base.Load(progress);
		}

		protected override void Unload()
		{
			base.get_View().BrowserButtonClick -= View_BrowserButtonClicked;
		}

		private void View_BrowserButtonClicked(object o, EventArgs e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			((Control)GameService.Overlay.get_BlishHudWindow()).Hide();
			BrowserUtil.OpenInDefaultBrowser(((Control)o).get_BasicTooltipText());
		}

		private void View_PositionButtonClicked(object o, EventArgs e)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			SettingEntry<Point> obj = new SettingEntry<Point>();
			obj.set_Value(new Point(400, 100));
			SettingEntry<Point> tempSizeSetting = obj;
			SpriteScreenMover spriteScreenMover = new SpriteScreenMover(new ScreenRegion("Stopwatch", StopwatchModule.ModuleInstance.Position, tempSizeSetting));
			((Control)spriteScreenMover).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			Rectangle contentRegion = ((Container)GameService.Graphics.get_SpriteScreen()).get_ContentRegion();
			((Control)spriteScreenMover).set_Size(((Rectangle)(ref contentRegion)).get_Size());
		}
	}
}
