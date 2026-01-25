using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Models;
using RaidClears.Localization;

namespace RaidClears.Settings.Controls
{
	public class EncounterLabelCustomerizer : Panel
	{
		private Labelable _labelable;

		private Label title = new Label();

		private TextBox input = new TextBox();

		private Label abbrivLabel = new Label();

		private StandardButton resetBtn = new StandardButton();

		public EncounterLabelCustomerizer(FlowPanel parent, Labelable labelable, EncounterInterface encounter, Color? labelColor = null)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			_labelable = labelable;
			((Control)this).set_Parent((Container)(object)parent);
			((Control)this).set_Width(((Control)parent).get_Width() - 10);
			((Control)this).set_Padding(new Thickness(0f, 10f));
			Build(encounter.Name, encounter.Abbriviation, encounter.Id, labelColor);
		}

		public EncounterLabelCustomerizer(FlowPanel parent, Labelable labelable, RaidEncounter encounter, Color? labelColor = null)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			_labelable = labelable;
			((Control)this).set_Parent((Container)(object)parent);
			((Control)this).set_Width(((Control)parent).get_Width() - 10);
			((Control)this).set_Padding(new Thickness(0f, 10f));
			Build(encounter.Name, encounter.Abbriviation, encounter.ApiId, labelColor);
		}

		public EncounterLabelCustomerizer(FlowPanel parent, Labelable labelable, StrikeMission encounter, Color? labelColor = null)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			_labelable = labelable;
			((Control)this).set_Parent((Container)(object)parent);
			((Control)this).set_Width(((Control)parent).get_Width() - 10);
			((Control)this).set_Padding(new Thickness(0f, 10f));
			Build(encounter.Name, encounter.Abbriviation, encounter.Id, labelColor);
		}

		public EncounterLabelCustomerizer(FlowPanel parent, Labelable labelable, RaidWing encounter, Color? labelColor = null)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			_labelable = labelable;
			((Control)this).set_Parent((Container)(object)parent);
			((Control)this).set_Width(((Control)parent).get_Width() - 10);
			((Control)this).set_Padding(new Thickness(0f, 10f));
			Build(encounter.Name, encounter.Abbriviation, encounter.Id, labelColor);
		}

		public EncounterLabelCustomerizer(FlowPanel parent, Labelable labelable, ExpansionStrikes encounter, Color? labelColor = null)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			_labelable = labelable;
			((Control)this).set_Parent((Container)(object)parent);
			((Control)this).set_Width(((Control)parent).get_Width() - 10);
			((Control)this).set_Padding(new Thickness(0f, 10f));
			Build(encounter.Name, encounter.Abbriviation, encounter.Id, labelColor);
		}

		public EncounterLabelCustomerizer(FlowPanel parent, Labelable labelable, ExpansionRaid encounter, Color? labelColor = null)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			_labelable = labelable;
			((Control)this).set_Parent((Container)(object)parent);
			((Control)this).set_Width(((Control)parent).get_Width() - 10);
			((Control)this).set_Padding(new Thickness(0f, 10f));
			Build(encounter.Name, encounter.Abbriviation, encounter.Id, labelColor);
		}

		protected void Build(string Name, string abbriv, string id, Color? color = null)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Expected O, but got Unknown
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			string id2 = id;
			string abbriv2 = abbriv;
			if (!color.HasValue)
			{
				color = Color.get_White();
			}
			string userLabel = _labelable.GetEncounterLabel(id2);
			int col1 = (((Control)this).get_Width() - 30) / 3;
			int remainingWidth = 2 * col1 - 5;
			int inputWidth = remainingWidth / 3;
			int resetBtnWidth = 2 * remainingWidth / 3;
			Label val = new Label();
			val.set_Text(Name);
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(col1);
			val.set_TextColor(color.Value);
			title = val;
			TextBox val2 = new TextBox();
			((TextInputBase)val2).set_Text(userLabel);
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(col1 + 5, 0));
			((Control)val2).set_Width(inputWidth);
			input = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text(string.Format(Strings.EncounterLabelCustomerizer_ResetTo, abbriv2));
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(col1 + inputWidth + 10, 0));
			((Control)val3).set_Width(resetBtnWidth);
			resetBtn = val3;
			if (abbriv2 == userLabel)
			{
				((Control)resetBtn).Hide();
			}
			((TextInputBase)input).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				_labelable.SetEncounterLabel(id2, ((TextInputBase)input).get_Text());
				if (((TextInputBase)input).get_Text() == abbriv2)
				{
					((Control)resetBtn).Hide();
				}
				((Control)resetBtn).Show();
			});
			((Control)resetBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((TextInputBase)input).set_Text(abbriv2);
				_labelable.SetEncounterLabel(id2, abbriv2);
				((Control)resetBtn).Hide();
			});
		}
	}
}
