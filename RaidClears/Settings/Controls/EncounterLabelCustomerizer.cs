using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Models;

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

		protected void Build(string Name, string abbriv, string id, Color? color = null)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Expected O, but got Unknown
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Expected O, but got Unknown
			string id2 = id;
			string abbriv2 = abbriv;
			if (!color.HasValue)
			{
				color = Color.get_White();
			}
			string userLabel = _labelable.GetEncounterLabel(id2);
			int col1 = (((Control)this).get_Width() - 30) / 3;
			int colN = (2 * col1 - 5) / 3;
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
			((Control)val2).set_Width(colN);
			input = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Reset to '" + abbriv2 + "'");
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(col1 + colN + 10, 0));
			((Control)val3).set_Width(colN);
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
				Service.RaidSettings.SetEncounterLabel(id2, abbriv2);
				((Control)resetBtn).Hide();
			});
		}
	}
}
