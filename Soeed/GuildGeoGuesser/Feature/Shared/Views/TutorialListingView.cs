using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.Shared.Controls;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Feature.Shared.Services;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Views
{
	public class TutorialListingView : AccountRestrictedView
	{
		[CompilerGenerated]
		private GeoGuessWindowStateService _003CwindowState_003EP;

		private List<Tutorial> _tutorials;

		public TutorialListingView(GeoGuessWindowStateService windowState)
		{
			_003CwindowState_003EP = windowState;
			_tutorials = new List<Tutorial>();
			base._002Ector(showBackButton: true);
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			await base.Load(progress);
			progress.Report("Loading practice puzzles");
			_tutorials = await Service.GeoServerWrapper.GetTutorialListAsync();
			return true;
		}

		protected override void DoBuild(Container buildPanel)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_003CwindowState_003EP.SwapToGuildSelect();
			});
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_controlsHeader);
			val.set_Text("Practice Puzzles");
			((Control)val).set_Location(new Point(50, 5));
			((Control)val).set_Width(350);
			((Control)val).set_Height(40);
			val.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0));
			val.set_TextColor(Color.get_LightGoldenrodYellow());
			ReloadButton reloadButton = new ReloadButton();
			((Control)reloadButton).set_Parent((Container)(object)_controlsHeader);
			((Control)reloadButton).set_Location(new Point(305, 0));
			((Control)reloadButton).set_BasicTooltipText("Refresh the practice puzzle list");
			((Control)reloadButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Service.GeoGuessWindow.OpenWindowState();
			});
			_flowPanel.set_FlowDirection((ControlFlowDirection)0);
			((Panel)_flowPanel).set_CanScroll(true);
			if (_tutorials.Count == 0)
			{
				_flowPanel.AddString("No practice puzzles are available yet. Check back soon!");
				return;
			}
			foreach (Tutorial tutorial in from t in _tutorials
				orderby t.SortOrder, t.Title
				select t)
			{
				_flowPanel.AddControl<TutorialListItem>(new TutorialListItem(tutorial));
			}
		}

		protected override void Unload()
		{
			base.Unload();
		}
	}
}
