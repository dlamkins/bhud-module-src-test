using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using RaidClears.Features.Fractals.Services;
using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Enums.Extensions;

namespace RaidClears.Settings.Controls
{
	public class EncounterClearStatus : Panel
	{
		private bool IsFractal;

		private FractalMap fractal;

		private Encounters.StrikeMission mission;

		private Label title = new Label();

		private Label clearDate = new Label();

		public EncounterClearStatus(FlowPanel parent, FractalMap encounter, DateTime lastClear)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			IsFractal = true;
			fractal = encounter;
			((Control)this).set_Parent((Container)(object)parent);
			((Control)this).set_Width(((Control)parent).get_Width());
			Build(encounter.Label, lastClear);
		}

		public EncounterClearStatus(FlowPanel parent, Encounters.StrikeMission encounter, DateTime lastClear)
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			IsFractal = false;
			mission = encounter;
			((Control)this).set_Parent((Container)(object)parent);
			((Control)this).set_Width(((Control)parent).get_Width());
			Build(encounter.GetLabel(), lastClear);
		}

		protected void MarkClear()
		{
			if (IsFractal)
			{
				Service.FractalMapWatcher.MarkCompleted(fractal);
			}
			else
			{
				Service.MapWatcher.MarkStrikeCompleted(mission);
			}
			clearDate.set_Text(DateTime.UtcNow.ToShortDateString());
		}

		protected void RemoveClear()
		{
			if (IsFractal)
			{
				Service.FractalMapWatcher.MarkNotCompleted(fractal);
			}
			else
			{
				Service.MapWatcher.MarkStrikeNotCompleted(mission);
			}
			clearDate.set_Text("----------");
		}

		protected void Build(string Name, DateTime datetime)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Expected O, but got Unknown
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			int col1 = (((Control)this).get_Width() - 30) / 3;
			int colN = (2 * col1 - 5) / 3;
			Label val = new Label();
			val.set_Text(Name);
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(col1);
			title = val;
			Label val2 = new Label();
			val2.set_Text((datetime.Year == 1) ? "----------" : datetime.ToShortDateString());
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(col1 + 5, 0));
			((Control)val2).set_Width(colN);
			clearDate = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Mark Complete");
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(col1 + colN + 5, 0));
			((Control)val3).set_Width(colN);
			StandardButton complete = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Remove Clear");
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Location(new Point(col1 + colN + colN + 5, 0));
			((Control)val4).set_Width(colN);
			((Control)complete).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MarkClear();
			});
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RemoveClear();
			});
		}
	}
}
