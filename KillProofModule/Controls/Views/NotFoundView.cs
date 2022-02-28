using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using KillProofModule.Properties;
using Microsoft.Xna.Framework;

namespace KillProofModule.Controls.Views
{
	public class NotFoundView : IView
	{
		private readonly string _searchTerm;

		public event EventHandler<EventArgs> Loaded;

		public event EventHandler<EventArgs> Built;

		public event EventHandler<EventArgs> Unloaded;

		public NotFoundView(string searchTerm)
		{
			_searchTerm = searchTerm;
		}

		public async Task<bool> DoLoad(IProgress<string> progress)
		{
			return true;
		}

		public void DoBuild(Container hPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Expected O, but got Unknown
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent(hPanel);
			((Control)val).set_Size(new Point(((Control)hPanel).get_Size().X - 150, ((Control)hPanel).get_Size().Y - 150));
			((Control)val).set_Location(new Point(75, 75));
			val.set_ShowBorder(true);
			val.set_ShowTint(true);
			Label val2 = new Label();
			((Control)val2).set_Parent(hPanel);
			((Control)val2).set_Size(((Control)hPanel).get_Size());
			((Control)val2).set_Location(new Point(0, -20));
			val2.set_ShowShadow(true);
			val2.set_StrokeText(true);
			val2.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)36, (FontStyle)0));
			val2.set_Text(Resources.No_profile_for___0___found___.Replace("{0}", _searchTerm));
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			Label labNothingHere = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent(hPanel);
			((Control)val3).set_Size(((Control)hPanel).get_Size());
			((Control)val3).set_Location(new Point(0, -20));
			val3.set_ShowShadow(true);
			val3.set_StrokeText(true);
			val3.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0));
			val3.set_Text("\n\n" + Resources.Please__share_www_killproof_me_with_this_player_and_help_expand_our_database_);
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			Label labVisitUs = val3;
			if (KillProofModule.ModuleInstance.PartyManager.Self.IsOwner(_searchTerm))
			{
				labNothingHere.set_Text(Resources.Not_yet_registered___);
				labVisitUs.set_Text("\n\n" + Resources.Visit_www_killproof_me_and_allow_us_to_record_your_KillProofs_for_you_);
			}
		}

		public void DoUnload()
		{
		}
	}
}
