using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Wizard
{
	public class WizardAreasView : WizardView
	{
		private bool _useAreas;

		private bool _useFillers;

		private readonly List<EventAreaConfiguration> _allAreas;

		protected override bool TestConfigurationsAvailable => true;

		public WizardAreasView(List<EventAreaConfiguration> allAreas, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService)
			: base(apiManager, iconService, translationService)
		{
			_allAreas = allAreas;
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			FormattedLabel welcomeLbl = new FormattedLabelBuilder().SetWidth(((Container)parent).get_ContentRegion().Width).Wrap().AutoSizeHeight()
				.SetHorizontalAlignment((HorizontalAlignment)1)
				.CreatePart("Event Areas", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					b.SetFontSize((FontSize)24);
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("You are probably already seeing your first area created and ready to use on your screen.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					b.SetFontSize((FontSize)18);
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("Please change the settings below to fit your needs regarding event areas.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					b.SetFontSize((FontSize)18);
				})
				.Build();
			((Control)welcomeLbl).set_Top((int)((float)((Container)parent).get_ContentRegion().Height * 0.1f));
			((Control)welcomeLbl).set_Parent((Container)(object)parent);
			Label useAreasLbl = RenderLabel(parent, "Use Areas:").TitleLabel;
			((Control)useAreasLbl).set_Parent((Container)(object)parent);
			useAreasLbl.set_AutoSizeWidth(false);
			((Control)useAreasLbl).set_Width(base.LABEL_WIDTH);
			((Control)useAreasLbl).set_Top(((Control)welcomeLbl).get_Bottom() + 100);
			((Control)useAreasLbl).set_Left(150);
			_useAreas = _allAreas.Any((EventAreaConfiguration a) => a.Enabled.get_Value());
			Checkbox useFillersCheckbox = null;
			((Control)RenderCheckbox(parent, new Point(((Control)useAreasLbl).get_Right() + 20, ((Control)useAreasLbl).get_Top()), _useAreas, delegate(bool val)
			{
				_useAreas = val;
				if (useFillersCheckbox != null)
				{
					((Control)useFillersCheckbox).set_Enabled(_useAreas);
				}
			})).set_BasicTooltipText("Check this option if you would like to keep the already created area.\nUncheck this option, if you only want to use the reminder feature of this module.");
			Label useFillersLbl = RenderLabel(parent, "Use Filler Events:").TitleLabel;
			((Control)useFillersLbl).set_Parent((Container)(object)parent);
			useFillersLbl.set_AutoSizeWidth(false);
			((Control)useFillersLbl).set_Width(base.LABEL_WIDTH);
			((Control)useFillersLbl).set_Top(((Control)useAreasLbl).get_Bottom() + 5);
			((Control)useFillersLbl).set_Left(150);
			_useFillers = _allAreas.Any((EventAreaConfiguration a) => a.UseFiller.get_Value());
			useFillersCheckbox = RenderCheckbox(parent, new Point(((Control)useFillersLbl).get_Right() + 20, ((Control)useFillersLbl).get_Top()), _useFillers, delegate(bool val)
			{
				_useFillers = val;
			});
			((Control)useFillersCheckbox).set_BasicTooltipText("Check this option if you would like to have events fill in the gaps between regular events and show the remaining time until they start.");
			FlowPanel buttons = GetButtonPanel(parent);
			Rectangle contentRegion = ((Container)parent).get_ContentRegion();
			((Control)buttons).set_Top(((Rectangle)(ref contentRegion)).get_Bottom() - 20 - ((Control)buttons).get_Height());
			((Control)buttons).set_Left(((Container)parent).get_ContentRegion().Width / 2 - ((Control)buttons).get_Width() / 2);
		}

		protected override Task ApplyConfigurations()
		{
			_allAreas.ForEach(delegate(EventAreaConfiguration a)
			{
				a.Enabled.set_Value(_useAreas);
			});
			_allAreas.ForEach(delegate(EventAreaConfiguration a)
			{
				a.UseFiller.set_Value(_useFillers);
			});
			return Task.CompletedTask;
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
