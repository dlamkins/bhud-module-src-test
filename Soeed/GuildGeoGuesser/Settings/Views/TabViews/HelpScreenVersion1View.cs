using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.Shared.Controls;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Settings.Views.TabViews
{
	public class HelpScreenVersion1View : View
	{
		private HelpScreen HelpScreens = new HelpScreen();

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			progress.Report("Load helpscreenData");
			HelpScreen data = await Service.GeoServerWrapper.GetHelpScreens();
			if (data != null)
			{
				HelpScreens = data;
			}
			return true;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanel obj = FlowPanelExtensions.BeginFlowFill(new FlowPanel(), buildPanel);
			obj.set_OuterControlPadding(new Vector2(0f, 0f));
			((Control)obj).set_Height(((Control)buildPanel).get_Height());
			((Panel)obj).set_CanScroll(true);
			HelpScreenContentBuilder.Build(obj, HelpScreens);
		}

		public HelpScreenVersion1View()
			: this()
		{
		}
	}
}
