using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Estreya.BlishHUD.Shared.Services;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.UI.Views
{
	public class ModuleSettingsView : BaseView
	{
		public event EventHandler OpenClicked;

		public event EventHandler CreateGithubIssueClicked;

		public ModuleSettingsView(IconService iconService, TranslationService translationService)
			: base(null, iconService, translationService)
		{
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Expected O, but got Unknown
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			Rectangle bounds = ((Container)parent).get_ContentRegion();
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(((Rectangle)(ref bounds)).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(5f, 2f));
			val.set_OuterControlPadding(new Vector2(10f, 15f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 15));
			((Control)val).set_Parent((Container)(object)parent);
			FlowPanel parentPanel = val;
			ViewContainer val2 = new ViewContainer();
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Parent((Container)(object)parentPanel);
			ViewContainer settingContainer = val2;
			string buttonText = base.TranslationService.GetTranslation("moduleSettingsView-openSettingsBtn", "Open Settings");
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)settingContainer);
			val3.set_Text(buttonText);
			StandardButton openSettingsButton = val3;
			if (base.Font != null)
			{
				((Control)openSettingsButton).set_Width((int)base.Font.MeasureString(buttonText).Width);
			}
			((Control)openSettingsButton).set_Location(new Point(Math.Max(((Control)parentPanel).get_Width() / 2 - ((Control)openSettingsButton).get_Width() / 2, 20), Math.Max(((Control)parentPanel).get_Height() / 2 - ((Control)openSettingsButton).get_Height(), 20)));
			((Control)openSettingsButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.OpenClicked?.Invoke(this, EventArgs.Empty);
			});
			string githubIssueText = base.TranslationService.GetTranslation("moduleSettingsView-createGitHubIssueBtn", "Create Bug/Feature Issue");
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)settingContainer);
			val4.set_Text(githubIssueText);
			StandardButton createGithubIssue = val4;
			if (base.Font != null)
			{
				((Control)createGithubIssue).set_Width((int)base.Font.MeasureString(githubIssueText).Width);
			}
			((Control)createGithubIssue).set_Location(new Point(Math.Max(((Control)parentPanel).get_Width() / 2 - ((Control)createGithubIssue).get_Width() / 2, 20), ((Control)openSettingsButton).get_Bottom() + 10));
			((Control)createGithubIssue).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.CreateGithubIssueClicked?.Invoke(this, EventArgs.Empty);
			});
		}
	}
}
