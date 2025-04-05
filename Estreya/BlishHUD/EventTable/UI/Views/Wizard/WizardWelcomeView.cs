using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Wizard
{
	public class WizardWelcomeView : WizardView
	{
		public WizardWelcomeView(Gw2ApiManager apiManager, IconService iconService, TranslationService translationService)
			: base(apiManager, iconService, translationService)
		{
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			FormattedLabel obj = new FormattedLabelBuilder().SetWidth(((Container)parent).get_ContentRegion().Width).Wrap().AutoSizeHeight()
				.SetHorizontalAlignment((HorizontalAlignment)1)
				.CreatePart("Welcome to the Event Table Setup Wizard!", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					b.SetFontSize((FontSize)24);
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("This wizard will take you through the most essential steps to get a base configuration for event table!", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					b.SetFontSize((FontSize)18);
				})
				.CreatePart("\n \n \n \n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("Please click on next if you are ready to start.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					b.SetFontSize((FontSize)18);
				})
				.Build();
			((Control)obj).set_Top((int)((float)((Container)parent).get_ContentRegion().Height * 0.2f));
			((Control)obj).set_Parent((Container)(object)parent);
			FlowPanel buttons = GetButtonPanel(parent);
			Rectangle contentRegion = ((Container)parent).get_ContentRegion();
			((Control)buttons).set_Top(((Rectangle)(ref contentRegion)).get_Bottom() - 20 - ((Control)buttons).get_Height());
			((Control)buttons).set_Left(((Container)parent).get_ContentRegion().Width / 2 - ((Control)buttons).get_Width() / 2);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
