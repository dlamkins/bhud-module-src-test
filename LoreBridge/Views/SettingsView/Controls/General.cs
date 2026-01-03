using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using LoreBridge.Models;
using LoreBridge.Translation.Language;
using LoreBridge.Translation.Translators;
using Microsoft.Xna.Framework;

namespace LoreBridge.Views.SettingsView.Controls
{
	public class General
	{
		public General(Panel mainPanel, Settings settings)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Expected O, but got Unknown
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Expected O, but got Unknown
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Expected O, but got Unknown
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)mainPanel);
			((Panel)val).set_Title("General");
			((Panel)val).set_CanCollapse(true);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(6f, 6f));
			val.set_ControlPadding(new Vector2(6f, 6f));
			((Panel)val).set_ShowBorder(true);
			FlowPanel generalPanel = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)generalPanel);
			val2.set_FlowDirection((ControlFlowDirection)0);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_ControlPadding(new Vector2(6f, 0f));
			FlowPanel languagePanel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)languagePanel);
			val3.set_Text("Translation language");
			val3.set_ShowShadow(true);
			((Control)val3).set_Height(28);
			((Control)val3).set_Width(180);
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Parent((Container)(object)languagePanel);
			((Control)val4).set_Width(160);
			val4.set_SelectedItem(LanguagesInfo.GetByLanguage(settings.TranslationLanguage.get_Value())?.Name);
			Dropdown languageDropdown = val4;
			foreach (LanguageInfo languageDetail in LanguagesInfo.List.OrderBy((LanguageInfo ld) => ld.Name))
			{
				languageDropdown.get_Items().Add(languageDetail.Name);
			}
			languageDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object o, ValueChangedEventArgs e)
			{
				LanguageInfo byName = LanguagesInfo.GetByName(e.get_CurrentValue());
				if (byName != null)
				{
					_ = byName.Language;
					settings.TranslationLanguage.set_Value((int)byName.Language);
				}
			});
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent((Container)(object)generalPanel);
			val5.set_FlowDirection((ControlFlowDirection)0);
			((Container)val5).set_WidthSizingMode((SizingMode)2);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			val5.set_ControlPadding(new Vector2(6f, 0f));
			FlowPanel translatorPanel = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)translatorPanel);
			val6.set_Text("Translator");
			val6.set_ShowShadow(true);
			((Control)val6).set_Height(28);
			((Control)val6).set_Width(180);
			Dictionary<string, Translators> translators = new Dictionary<string, Translators>
			{
				{
					"Google",
					Translators.Google2
				},
				{
					"Yandex",
					Translators.Yandex
				},
				{
					"LibreTranslate",
					Translators.LibreTranslate
				}
			};
			Dropdown val7 = new Dropdown();
			((Control)val7).set_Parent((Container)(object)translatorPanel);
			((Control)val7).set_Width(160);
			Dropdown translatorDropdown = val7;
			Translators currentTranslator = (Translators)settings.TranslationTranslator.get_Value();
			string selectedItem = translators.FirstOrDefault((KeyValuePair<string, Translators> x) => x.Value == currentTranslator).Key;
			translatorDropdown.set_SelectedItem(selectedItem);
			foreach (string item in translators.Keys)
			{
				translatorDropdown.get_Items().Add(item);
			}
			translatorDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object o, ValueChangedEventArgs e)
			{
				if (translators.TryGetValue(e.get_CurrentValue(), out var value))
				{
					settings.TranslationTranslator.set_Value((int)value);
				}
			});
			new LibreTranslate(mainPanel, settings);
		}
	}
}
