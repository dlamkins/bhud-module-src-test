using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace Nekres.ProofLogix.Core.UI
{
	public class LoadingView : View
	{
		private AsyncString _title;

		private AsyncString _subtitle;

		private AsyncString _basicTooltipText;

		private Label _titleLbl;

		private Label _subTitleLbl;

		private Container _buildPanel;

		public LoadingView(AsyncString title = null, AsyncString subtitle = null, AsyncString basicTooltipText = null)
			: this()
		{
			_title = title ?? ((AsyncString)string.Empty);
			_subtitle = subtitle ?? ((AsyncString)string.Empty);
			_basicTooltipText = basicTooltipText ?? ((AsyncString)string.Empty);
			_title.Changed += OnTitleChanged;
			_subtitle.Changed += OnSubtitleChanged;
			_basicTooltipText.Changed += OnBasicTooltipChanged;
		}

		protected override void Unload()
		{
			_title.Changed -= OnTitleChanged;
			_subtitle.Changed -= OnSubtitleChanged;
			_basicTooltipText.Changed -= OnBasicTooltipChanged;
			((View<IPresenter>)this).Unload();
		}

		private void OnTitleChanged(object sender, EventArgs e)
		{
			if (_titleLbl != null)
			{
				_titleLbl.set_Text((string)_title);
			}
		}

		private void OnSubtitleChanged(object sender, EventArgs e)
		{
			if (_subTitleLbl != null)
			{
				_subTitleLbl.set_Text((string)_subtitle);
			}
		}

		private void OnBasicTooltipChanged(object sender, EventArgs e)
		{
			if (_buildPanel != null)
			{
				((Control)_buildPanel).set_BasicTooltipText((string)_basicTooltipText);
			}
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected O, but got Unknown
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Expected O, but got Unknown
			_buildPanel = buildPanel;
			LoadingSpinner val = new LoadingSpinner();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(64);
			((Control)val).set_Height(64);
			((Control)val).set_Left((buildPanel.get_ContentRegion().Width - 64) / 2);
			((Control)val).set_Top((buildPanel.get_ContentRegion().Height - 64) / 2);
			LoadingSpinner spinner = val;
			Label val2 = new Label();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val2).set_Height(30);
			val2.set_Text((string)_title);
			((Control)val2).set_Top(((Control)spinner).get_Bottom() + 7);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_Font(GameService.Content.get_DefaultFont18());
			_titleLbl = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val3).set_Height(30);
			val3.set_Text((string)_subtitle);
			((Control)val3).set_Top(((Control)_titleLbl).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			_subTitleLbl = val3;
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_0096: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
				((Control)spinner).set_Left((e.get_CurrentRegion().Width - ((Control)spinner).get_Width()) / 2);
				((Control)spinner).set_Top((e.get_CurrentRegion().Height - ((Control)spinner).get_Height()) / 2);
				((Control)_titleLbl).set_Width(buildPanel.get_ContentRegion().Width);
				((Control)_titleLbl).set_Top(((Control)spinner).get_Bottom() + 7);
				((Control)_subTitleLbl).set_Width(buildPanel.get_ContentRegion().Width);
				((Control)_subTitleLbl).set_Top(((Control)_titleLbl).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
			});
			((View<IPresenter>)this).Build(buildPanel);
		}
	}
}
