using Blish_HUD;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public static class WarclawMeter
	{
		public const double RunningIPS = 688.0;

		public const double DashIPS = 1442.0;

		public const double MaxIPS = 2000.0;

		public const double VSpeedRange = 1024.0;

		public static AnchoredRect Construct(IMeasurer measurer)
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			AnchoredRect meter = new AnchoredRect();
			ArcMeterMaker arc = new ArcMeterMaker(measurer);
			Projection speedP = arc.Meter.AddProjection(Projection.ZeroTo(2000.0)).WithSoftMax(1442.0);
			meter.AddChild(arc.Meter);
			arc.AddSoftMaxSpeedIndicator(speedP);
			arc.AddZone(speedP, new Color(51, 170, 153), 688.0);
			arc.AddSoftMaxSpeedZone(speedP);
			arc.AddArc(speedP);
			arc.AddSpeedNeedle(speedP);
			arc.AddSpeedText(speedP);
			arc.AddAccelText(speedP);
			LineMeterMaker right = new LineMeterMaker(measurer);
			Projection vSpeedP = right.Meter.AddProjection(Projection.ZeroTo(1024.0));
			right.TackOn(right: true);
			meter.AddChild(right.Meter);
			right.AddUpSpeedZone(vSpeedP, (Color?)new Color(51, 68, 136));
			right.AddDownSpeedZone(vSpeedP, (Color?)new Color(34, 34, 102));
			right.AddVSpeedNeedle(vSpeedP, Color.get_White());
			right.AddOutline();
			meter.WithUpdate(delegate
			{
				meter.Visible = !GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode();
			});
			return meter;
		}
	}
}
