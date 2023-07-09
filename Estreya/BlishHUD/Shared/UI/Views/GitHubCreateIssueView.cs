using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Threading.Events;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.Shared.UI.Views
{
	public class GitHubCreateIssueView : BaseView
	{
		private readonly string _message;

		private readonly string _moduleName;

		private readonly string _title;

		public event AsyncEventHandler<(string Title, string Message, string DiscordName, bool IncludeSystemInformation)> CreateClicked;

		public event EventHandler CancelClicked;

		public GitHubCreateIssueView(string moduleName, IconService iconService, TranslationService translationService, BitmapFont font = null)
			: base(null, iconService, translationService, font)
		{
			_moduleName = moduleName;
		}

		public GitHubCreateIssueView(string moduleName, IconService iconService, TranslationService translationService, BitmapFont font = null, string title = null, string message = null)
			: this(moduleName, iconService, translationService, font)
		{
			_title = title;
			_message = message;
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			Rectangle contentRegion = ((Container)parent).get_ContentRegion();
			parent.set_CanScroll(true);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_Text("New Issue for Module " + _moduleName);
			((Control)val).set_Width(contentRegion.Width);
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val).set_Height(GameService.Content.get_DefaultFont32().get_LineHeight());
			val.set_Font(GameService.Content.get_DefaultFont32());
			Label titleLabel = val;
			Label issueTitleLabel = RenderLabel(parent, "Title").TitleLabel;
			((Control)issueTitleLabel).set_Top(((Control)titleLabel).get_Bottom() + 50);
			TextBox issueTitleTextBox = RenderTextbox(parent, new Point(base.LABEL_WIDTH, ((Control)issueTitleLabel).get_Top()), ((Control)parent).get_Width() - base.LABEL_WIDTH, (!string.IsNullOrWhiteSpace(_title)) ? _title : ("[BUG/FEATURE] " + _moduleName + " ...."), "Issue Title");
			((Control)issueTitleTextBox).set_BasicTooltipText("Should contain a clear title describing the feature or bug.");
			Label issueMessageLabel = RenderLabel(parent, "Issue").TitleLabel;
			((Control)issueMessageLabel).set_Top(((Control)issueTitleLabel).get_Bottom() + 20);
			TextBox issueMessageTextBox = RenderTextbox(parent, new Point(base.LABEL_WIDTH, ((Control)issueMessageLabel).get_Top()), ((Control)parent).get_Width() - base.LABEL_WIDTH, (!string.IsNullOrWhiteSpace(_message)) ? _message : string.Empty, "Issue Message");
			((Control)issueMessageTextBox).set_BasicTooltipText("Describe your feature or bug here.");
			Label discordNameLabel = RenderLabel(parent, "Discord Username (optional)").TitleLabel;
			((Control)discordNameLabel).set_Top(((Control)issueMessageLabel).get_Bottom() + 20);
			TextBox discordNameTextBox = RenderTextbox(parent, new Point(base.LABEL_WIDTH, ((Control)discordNameLabel).get_Top()), ((Control)parent).get_Width() - base.LABEL_WIDTH, "", "Discord#0001");
			((Control)discordNameTextBox).set_BasicTooltipText("If provided, its used to ask additional questions or notify you about the status.");
			Label includeSystemInformationLabel = RenderLabel(parent, "Include System Info").TitleLabel;
			((Control)includeSystemInformationLabel).set_Top(((Control)discordNameLabel).get_Bottom() + 20);
			Checkbox includeSystemInformationCheckbox = RenderCheckbox(parent, new Point(base.LABEL_WIDTH, ((Control)includeSystemInformationLabel).get_Top()), value: false);
			((Control)includeSystemInformationCheckbox).set_BasicTooltipText("If checked, additional system information will be included to assist looking into your issue.");
			Button cancelButton = RenderButton(parent, "Cancel", delegate
			{
				this.CancelClicked?.Invoke(this, EventArgs.Empty);
			});
			((Control)cancelButton).set_Bottom(((Rectangle)(ref contentRegion)).get_Bottom());
			((Control)cancelButton).set_Right(((Rectangle)(ref contentRegion)).get_Right());
			Button button = RenderButtonAsync(parent, "Create", async delegate
			{
				await (this.CreateClicked?.Invoke(this, (((TextInputBase)issueTitleTextBox).get_Text(), ((TextInputBase)issueMessageTextBox).get_Text(), ((TextInputBase)discordNameTextBox).get_Text(), includeSystemInformationCheckbox.get_Checked())));
			});
			((Control)button).set_Top(((Control)cancelButton).get_Top());
			((Control)button).set_Right(((Control)cancelButton).get_Left() + 10);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
