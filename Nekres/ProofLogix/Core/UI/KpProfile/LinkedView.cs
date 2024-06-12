using System;
using System.Linq;
using Blish_HUD;
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
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Expected O, but got Unknown
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Expected O, but got Unknown
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
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
				MenuItem val4 = new MenuItem(AssetUtil.Truncate(profile.Name, ((Container)menu).get_ContentRegion().Width - 14, GameService.Content.get_DefaultFont16()));
				((Control)val4).set_Parent((Container)(object)menu);
				((Control)val4).set_BasicTooltipText(profile.Name);
				((Control)val4).set_Width(((Container)menu).get_ContentRegion().Width);
				val4.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate
				{
					profileContainer.Show((IView)(object)new ProfileView(profile));
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
