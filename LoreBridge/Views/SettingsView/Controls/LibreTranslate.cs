using System;
using Blish_HUD;
using Blish_HUD.Controls;
using LoreBridge.Models;
using Microsoft.Xna.Framework;

namespace LoreBridge.Views.SettingsView.Controls
{
	public sealed class LibreTranslate : FlowPanel
	{
		public LibreTranslate(Panel mainPanel, Settings settings)
			: this()
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Expected O, but got Unknown
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			LibreTranslate libreTranslate = this;
			((Control)this).set_Parent((Container)(object)mainPanel);
			((Panel)this).set_Title("LibreTranslate");
			((Panel)this).set_CanCollapse(true);
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)this).set_OuterControlPadding(new Vector2(6f, 6f));
			((FlowPanel)this).set_ControlPadding(new Vector2(6f, 6f));
			((Panel)this).set_ShowBorder(true);
			((Control)this).set_Visible(settings.TranslationTranslator.get_Value() == 4);
			((Control)this).set_Height((settings.TranslationTranslator.get_Value() == 4) ? 1 : 0);
			settings.TranslationTranslator.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate(object o, ValueChangedEventArgs<int> e)
			{
				((Control)libreTranslate).set_Visible(e.get_NewValue() == 4);
				((Control)libreTranslate).set_Height((settings.TranslationTranslator.get_Value() == 4) ? 1 : 0);
			});
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_FlowDirection((ControlFlowDirection)0);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ControlPadding(new Vector2(6f, 0f));
			FlowPanel urlPanel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)urlPanel);
			val2.set_Text("API URL");
			val2.set_ShowShadow(true);
			((Control)val2).set_Height(28);
			((Control)val2).set_Width(180);
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent((Container)(object)urlPanel);
			((TextInputBase)val3).set_Text(settings.TranslationLibreTranslateUrl.get_Value());
			TextBox urlInput = val3;
			((TextInputBase)urlInput).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate(object o, ValueEventArgs<bool> e)
			{
				if (!e.get_Value())
				{
					settings.TranslationLibreTranslateUrl.set_Value(((TextInputBase)urlInput).get_Text());
				}
			});
		}
	}
}
