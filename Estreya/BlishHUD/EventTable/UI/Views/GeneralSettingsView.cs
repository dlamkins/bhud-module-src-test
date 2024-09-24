using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Controls.Input;
using Estreya.BlishHUD.Shared.Models;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class GeneralSettingsView : BaseSettingsView
	{
		private readonly ModuleSettings _moduleSettings;

		private readonly MetricsService _metricsService;

		public GeneralSettingsView(ModuleSettings moduleSettings, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, SettingEventService settingEventService, MetricsService metricsService)
			: base(apiManager, iconService, translationService, settingEventService)
		{
			_moduleSettings = moduleSettings;
			_metricsService = metricsService;
		}

		protected override void BuildView(FlowPanel parent)
		{
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Expected O, but got Unknown
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.GlobalDrawerVisible);
			RenderKeybindingSetting((Panel)(object)parent, _moduleSettings.GlobalDrawerVisibleHotkey);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.RegisterCornerIcon);
			RenderEnumSetting<CornerIconClickAction>((Panel)(object)parent, _moduleSettings.CornerIconLeftClickAction);
			RenderEnumSetting<CornerIconClickAction>((Panel)(object)parent, _moduleSettings.CornerIconRightClickAction);
			RenderEmptyLine((Panel)(object)parent);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.RegisterContext);
			RenderButtonAsync((Panel)(object)parent, "Change Metrics Consent", async delegate
			{
				await _metricsService.AskMetricsConsent(forceAsk: true);
			});
			RenderEmptyLine((Panel)(object)parent);
			RenderKeybindingSetting((Panel)(object)parent, _moduleSettings.MapKeybinding);
			RenderEmptyLine((Panel)(object)parent);
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Width(MathHelper.Clamp((int)((double)((Container)parent).get_ContentRegion().Width * 0.55), 0, ((Container)parent).get_ContentRegion().Width));
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_OuterControlPadding(new Vector2(10f, 20f));
			((Panel)val).set_ShowBorder(true);
			val.set_FlowDirection((ControlFlowDirection)3);
			FlowPanel visibilityOptionGroup = val;
			((Control)new FormattedLabelBuilder().SetWidth(((Container)visibilityOptionGroup).get_ContentRegion().Width - 20).AutoSizeHeight().Wrap()
				.CreatePart(base.TranslationService.GetTranslation("generalSettingsView-uiVisibilityWarning", "These options are global. The individual area options have priority and will hide it if any matches!"), (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.MakeBold().SetFontSize((FontSize)18);
				})
				.Build()).set_Parent((Container)(object)visibilityOptionGroup);
			RenderEmptyLine((Panel)(object)visibilityOptionGroup, 10);
			RenderBoolSetting((Panel)(object)visibilityOptionGroup, _moduleSettings.HideOnMissingMumbleTicks);
			RenderBoolSetting((Panel)(object)visibilityOptionGroup, _moduleSettings.HideOnOpenMap);
			RenderBoolSetting((Panel)(object)visibilityOptionGroup, _moduleSettings.HideInCombat);
			RenderBoolSetting((Panel)(object)visibilityOptionGroup, _moduleSettings.HideInPvE_OpenWorld);
			RenderBoolSetting((Panel)(object)visibilityOptionGroup, _moduleSettings.HideInPvE_Competetive);
			RenderBoolSetting((Panel)(object)visibilityOptionGroup, _moduleSettings.HideInWvW);
			RenderBoolSetting((Panel)(object)visibilityOptionGroup, _moduleSettings.HideInPvP);
			RenderEmptyLine((Panel)(object)visibilityOptionGroup, 20);
			RenderEmptyLine((Panel)(object)parent);
			RenderEnumSetting<MenuEventSortMode>((Panel)(object)parent, _moduleSettings.MenuEventSortMode);
			RenderEmptyLine((Panel)(object)parent);
			RenderDevelopmentGroup(parent);
			RenderEmptyLine((Panel)(object)parent);
		}

		private void RenderDevelopmentGroup(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_OuterControlPadding(new Vector2(20f, 20f));
			((Panel)val).set_ShowBorder(true);
			((Panel)val).set_Title("Development");
			((Panel)val).set_CanCollapse(true);
			((Panel)val).set_Collapsed(true);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width - (int)(parent.get_OuterControlPadding().X * 2f));
			val.set_FlowDirection((ControlFlowDirection)3);
			FlowPanel groupPanel = val;
			FormattedLabelBuilder obj = new FormattedLabelBuilder().SetWidth(((Container)groupPanel).get_ContentRegion().Width).AutoSizeHeight().SetVerticalAlignment((VerticalAlignment)0);
			obj.CreatePart("Using the Development API happens on your own risk!", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
			{
				b.MakeBold().SetFontSize((FontSize)24);
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("The API can be offline or in a broken state for your module version at any given time!", (Action<FormattedLabelPartBuilder>)delegate
			{
			})
				.CreatePart("\n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("This could lead to you not being able to use the module at all to reset this setting via the UI.", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("In this case open the settings.json file and set the option \"" + ((SettingEntry)_moduleSettings.UseDevelopmentAPI).get_EntryKey() + "\" back to false.", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("Changing this option needs a complete restart of BlishHUD!", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					b.MakeItalic();
				});
			((Control)obj.Build()).set_Parent((Container)(object)groupPanel);
			RenderEmptyLine((Panel)(object)groupPanel);
			RenderBoolSetting((Panel)(object)groupPanel, _moduleSettings.UseDevelopmentAPI, async (bool oldVal, bool newVal) => !newVal || await new ConfirmDialog("Activating Development API", "You are in the process of enabling the development api.\n\nThis API can be offline at any time preventing you from using the module!", base.IconService).ShowDialog() == DialogResult.OK);
			RenderEmptyLine((Panel)(object)groupPanel, (int)groupPanel.get_OuterControlPadding().Y);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
