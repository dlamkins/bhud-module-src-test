using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Models;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class GeneralSettingsView : BaseSettingsView
	{
		private readonly ModuleSettings _moduleSettings;

		public GeneralSettingsView(ModuleSettings moduleSettings, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, SettingEventService settingEventService, BitmapFont font = null)
			: base(apiManager, iconService, translationService, settingEventService, font)
		{
			_moduleSettings = moduleSettings;
		}

		protected override void BuildView(FlowPanel parent)
		{
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Expected O, but got Unknown
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.GlobalDrawerVisible);
			RenderKeybindingSetting((Panel)(object)parent, _moduleSettings.GlobalDrawerVisibleHotkey);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.RegisterCornerIcon);
			RenderEnumSetting<CornerIconClickAction>((Panel)(object)parent, _moduleSettings.CornerIconLeftClickAction);
			RenderEnumSetting<CornerIconClickAction>((Panel)(object)parent, _moduleSettings.CornerIconRightClickAction);
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
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
