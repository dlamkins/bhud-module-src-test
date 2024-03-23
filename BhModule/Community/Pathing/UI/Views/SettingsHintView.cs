using System;
using System.Diagnostics;
using BhModule.Community.Pathing.UI.Controls;
using BhModule.Community.Pathing.UI.Presenters;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.UI.Views
{
	public class SettingsHintView : View
	{
		public event EventHandler<EventArgs> OpenSettingsClicked;

		public event EventHandler<EventArgs> OpenMarkerPacksClicked;

		public SettingsHintView()
			: this()
		{
		}

		public SettingsHintView((Action OpenSettings, Action OpenMarkerPacks, PackInitiator packInitiator) model)
			: this()
		{
			((View)this).WithPresenter((IPresenter)(object)new SettingsHintPresenter(this, model));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			SettingHero obj = new SettingHero
			{
				Icon = AsyncTexture2D.FromAssetId(156027),
				Text = "Open  Settings"
			};
			((Control)obj).set_Size(new Point(((Control)buildPanel).get_Width() / 3, ((Control)buildPanel).get_Height() - 48));
			((Control)obj).set_Parent(buildPanel);
			((Control)obj).add_Click((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs e)
			{
				this.OpenSettingsClicked?.Invoke(this, (EventArgs)(object)e);
			});
			SettingHero obj2 = new SettingHero
			{
				Icon = AsyncTexture2D.FromAssetId(543438),
				Text = "Download  Marker  Packs"
			};
			((Control)obj2).set_Size(new Point(((Control)buildPanel).get_Width() / 3, ((Control)buildPanel).get_Height() - 48));
			((Control)obj2).set_Left(((Control)buildPanel).get_Width() / 3);
			((Control)obj2).set_Parent(buildPanel);
			((Control)obj2).add_Click((EventHandler<MouseEventArgs>)delegate(object sender, MouseEventArgs e)
			{
				this.OpenMarkerPacksClicked?.Invoke(this, (EventArgs)(object)e);
			});
			SettingHero obj3 = new SettingHero
			{
				Icon = AsyncTexture2D.FromAssetId(2604584),
				Text = "Open  Setup  Guide"
			};
			((Control)obj3).set_Size(new Point(((Control)buildPanel).get_Width() / 3, ((Control)buildPanel).get_Height() - 48));
			((Control)obj3).set_Left(((Control)buildPanel).get_Width() / 3 * 2);
			((Control)obj3).set_Parent(buildPanel);
			((Control)obj3).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					Process.Start("https://link.blishhud.com/pathingsetup");
				}
				catch (Exception)
				{
				}
			});
			DonateHero donateHero = new DonateHero();
			((Control)donateHero).set_Size(new Point(((Control)buildPanel).get_Width(), 48));
			((Control)donateHero).set_Bottom(((Control)buildPanel).get_Height());
			((Control)donateHero).set_Parent(buildPanel);
		}
	}
}
