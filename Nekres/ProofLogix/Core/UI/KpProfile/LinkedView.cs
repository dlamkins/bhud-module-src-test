using System;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;

namespace Nekres.ProofLogix.Core.UI.KpProfile
{
	public class LinkedView : View
	{
		private readonly Profile _profile;

		public LinkedView(Profile profile)
			: this()
		{
			_profile = profile;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Expected O, but got Unknown
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Expected O, but got Unknown
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Expected O, but got Unknown
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
			ViewContainer profileContainer = val;
			if (_profile.Linked == null || !_profile.Linked.Any())
			{
				buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_001d: Unknown result type (might be due to invalid IL or missing references)
					((Control)profileContainer).set_Width(e.get_CurrentRegion().Width);
					((Control)profileContainer).set_Height(e.get_CurrentRegion().Height);
				});
				profileContainer.Show((IView)(object)new ProfileView(_profile));
				((View<IPresenter>)this).Build(buildPanel);
				return;
			}
			Panel val2 = new Panel();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Width(100);
			((Control)val2).set_Height(buildPanel.get_ContentRegion().Height);
			val2.set_CanScroll(true);
			val2.set_Title("Accounts");
			Panel menuPanel = val2;
			Menu val3 = new Menu();
			((Control)val3).set_Parent((Container)(object)menuPanel);
			((Control)val3).set_Width(((Container)menuPanel).get_ContentRegion().Width);
			((Control)val3).set_Height(((Container)menuPanel).get_ContentRegion().Height);
			Menu menu = val3;
			((Container)menuPanel).add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)menu).set_Height(e.get_CurrentRegion().Height);
			});
			ViewContainer obj = profileContainer;
			((Control)obj).set_Width(((Control)obj).get_Width() - ((Control)menuPanel).get_Width());
			((Control)profileContainer).set_Left(((Control)menuPanel).get_Right());
			foreach (Profile profile in _profile.Accounts)
			{
				MenuItem val4 = new MenuItem(profile.Name);
				((Control)val4).set_Parent((Container)(object)menu);
				MenuItem entry = val4;
				entry.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate
				{
					if (_profile.BelongsTo(entry.get_Text(), out var linkedProfile))
					{
						profileContainer.Show((IView)(object)new ProfileView(linkedProfile));
					}
				});
			}
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				((Control)profileContainer).set_Width(e.get_CurrentRegion().Width - ((Control)menu).get_Width());
				((Control)profileContainer).set_Height(e.get_CurrentRegion().Height);
				((Control)profileContainer).set_Left(((Control)menuPanel).get_Right());
				((Control)menuPanel).set_Height(buildPanel.get_ContentRegion().Height);
			});
			profileContainer.Show((IView)(object)new ProfileView(_profile));
			((View<IPresenter>)this).Build(buildPanel);
		}
	}
}
