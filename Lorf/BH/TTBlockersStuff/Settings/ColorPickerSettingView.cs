using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using TTBlockersStuff.Language;

namespace Lorf.BH.TTBlockersStuff.Settings
{
	internal class ColorPickerSettingView : SettingView<Color>
	{
		private IEnumerable<Color> colors;

		private ColorBox box;

		private Label nameLabel;

		private ColorPickerWindow window;

		public ColorPickerSettingView(SettingEntry<Color> setting, int definedWidth = -1)
			: base(setting, definedWidth)
		{
			colors = Module.Instance.Colors;
		}

		public override bool HandleComplianceRequisite(IComplianceRequisite complianceRequisite)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (complianceRequisite is SettingDisabledComplianceRequisite)
			{
				SettingDisabledComplianceRequisite disabledRequisite = (SettingDisabledComplianceRequisite)(object)complianceRequisite;
				((Control)box).set_Enabled(!((SettingDisabledComplianceRequisite)(ref disabledRequisite)).get_Disabled());
				return true;
			}
			return false;
		}

		protected override void BuildSetting(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected O, but got Unknown
			ColorBox val = new ColorBox();
			((Control)val).set_Visible(true);
			val.set_Color(colors.First());
			((Control)val).set_Parent(buildPanel);
			box = val;
			((Control)box).add_Click((EventHandler<MouseEventArgs>)UpdateActiveColorBox);
			Label val2 = new Label();
			val2.set_Text("");
			((Control)val2).set_Location(new Point(((Control)box).get_Right() + 10, ((Control)box).get_Top() + 5));
			val2.set_ShowShadow(true);
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent(buildPanel);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			nameLabel = val2;
		}

		private void UpdateActiveColorBox(object sender, MouseEventArgs e)
		{
			ColorPickerWindow colorPickerWindow = new ColorPickerWindow(Translations.ColorSelectionTitle, box);
			((Control)colorPickerWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			window = colorPickerWindow;
			((Control)window).Show();
			window.AssignmentAccepted += OnPickedColorChange;
		}

		private void OnPickedColorChange(object sender, EventArgs e)
		{
			base.OnValueChanged(new ValueEventArgs<Color>(box.get_Color()));
		}

		protected override void RefreshDisplayName(string displayName)
		{
			if (nameLabel != null)
			{
				nameLabel.set_Text(displayName);
			}
		}

		protected override void RefreshDescription(string description)
		{
			if (nameLabel != null)
			{
				((Control)nameLabel).set_BasicTooltipText(description);
			}
		}

		protected override void Unload()
		{
			((Control)box).remove_Click((EventHandler<MouseEventArgs>)UpdateActiveColorBox);
			((Control)box).Dispose();
			((Control)nameLabel).Dispose();
			window.AssignmentAccepted -= OnPickedColorChange;
			((Control)window).Dispose();
			((View<IPresenter>)(object)this).Unload();
		}

		protected override void RefreshValue(Color value)
		{
			box.set_Color(value);
		}
	}
}
