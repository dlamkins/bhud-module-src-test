using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace Blish_HUD.Extended.Core.Views
{
	public class SocialsSettingsView : View<SocialsSettingsPresenter>
	{
		private FlowPanel _settingFlowPanel;

		private bool _lockBounds = true;

		private ViewContainer _lastSettingContainer;

		private FlowPanel _socialFlowPanel;

		public bool LockBounds
		{
			get
			{
				return _lockBounds;
			}
			set
			{
				if (_lockBounds != value)
				{
					_lockBounds = value;
					UpdateBoundsLocking(_lockBounds);
				}
			}
		}

		internal event EventHandler<EventArgs> BrowserButtonClick;

		private void UpdateBoundsLocking(bool locked)
		{
			if (_settingFlowPanel != null)
			{
				((Panel)_settingFlowPanel).set_ShowBorder(!locked);
				((Panel)_settingFlowPanel).set_CanCollapse(!locked);
			}
		}

		public SocialsSettingsView(SocialsSettingsModel model)
		{
			base.WithPresenter(new SocialsSettingsPresenter(this, model));
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			return await ((Presenter<SocialsSettingsView, SocialsSettingsModel>)base.get_Presenter()).get_Model().LoadSocials();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected O, but got Unknown
			BuildSocialsPanel(buildPanel);
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width() - 13, ((Control)buildPanel).get_Height()));
			FlowPanel socialFlowPanel = _socialFlowPanel;
			((Control)val).set_Location(new Point(-3, (socialFlowPanel != null) ? ((Control)socialFlowPanel).get_Bottom() : 0));
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(5f, 2f));
			val.set_OuterControlPadding(new Vector2(0f, 0f));
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 15));
			((Control)val).set_Parent(buildPanel);
			_settingFlowPanel = val;
			foreach (SettingEntry item in ((IEnumerable<SettingEntry>)((Presenter<SocialsSettingsView, SocialsSettingsModel>)base.get_Presenter()).get_Model().Settings).Where((SettingEntry s) => s.get_SessionDefined()))
			{
				IView settingView;
				if ((settingView = SettingView.FromType(item, ((Control)_settingFlowPanel).get_Width())) != null)
				{
					ViewContainer val2 = new ViewContainer();
					((Container)val2).set_WidthSizingMode((SizingMode)2);
					((Container)val2).set_HeightSizingMode((SizingMode)1);
					((Control)val2).set_Parent((Container)(object)_settingFlowPanel);
					_lastSettingContainer = val2;
					_lastSettingContainer.Show(settingView);
					SettingsView subSettingsView = (SettingsView)(object)((settingView is SettingsView) ? settingView : null);
					if (subSettingsView != null)
					{
						subSettingsView.set_LockBounds(false);
					}
				}
			}
		}

		private void BuildSocialsPanel(Container buildPanel)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Expected O, but got Unknown
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			List<SocialsSettingsModel.SocialType> socials = ((Presenter<SocialsSettingsView, SocialsSettingsModel>)base.get_Presenter()).get_Model().GetSocials().ToList();
			if (!socials.Any())
			{
				return;
			}
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width() - 17, 51));
			((Control)val).set_Location(new Point(0, 0));
			val.set_FlowDirection((ControlFlowDirection)5);
			val.set_ControlPadding(new Vector2(3f, 2f));
			val.set_OuterControlPadding(new Vector2(1f, 3f));
			((Panel)val).set_ShowBorder(false);
			((Panel)val).set_ShowTint(true);
			((Control)val).set_Parent(buildPanel);
			_socialFlowPanel = val;
			foreach (SocialsSettingsModel.SocialType social in socials)
			{
				string text = ((Presenter<SocialsSettingsView, SocialsSettingsModel>)base.get_Presenter()).get_Model().GetSocialText(social);
				StandardButton val2 = new StandardButton();
				((Control)val2).set_Parent((Container)(object)_socialFlowPanel);
				((Control)val2).set_Size(new Point((int)GameService.Content.get_DefaultFont14().MeasureString(text).Width + 48, 45));
				val2.set_Text(text);
				val2.set_Icon(AsyncTexture2D.op_Implicit(((Presenter<SocialsSettingsView, SocialsSettingsModel>)base.get_Presenter()).get_Model().GetSocialLogo(social)));
				val2.set_ResizeIcon(true);
				((Control)val2).set_BasicTooltipText(((Presenter<SocialsSettingsView, SocialsSettingsModel>)base.get_Presenter()).get_Model().GetSocialUrl(social));
				((Control)val2).add_Click((EventHandler<MouseEventArgs>)OnBrowserButtonClick);
			}
		}

		private void OnBrowserButtonClick(object sender, MouseEventArgs e)
		{
			this.BrowserButtonClick?.Invoke(sender, (EventArgs)(object)e);
		}

		protected override void Unload()
		{
		}
	}
}
