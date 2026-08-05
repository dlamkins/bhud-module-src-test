using Microsoft.Xna.Framework;

namespace Frtal.Wayfinder.Models
{
	public class CompassTarget
	{
		public string Id { get; }

		public string Label { get; }

		public TargetKind Kind { get; }

		public Vector2 ContinentPosition { get; }

		public bool ShowDistance { get; set; } = true;


		public CompassTarget(string id, string label, TargetKind kind, Vector2 continentPosition)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			Id = id;
			Label = label;
			Kind = kind;
			ContinentPosition = continentPosition;
		}

		public static Color ColorFor(TargetKind kind)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(kind switch
			{
				TargetKind.Waypoint => new Color(90, 170, 255), 
				TargetKind.PointOfInterest => new Color(220, 220, 220), 
				TargetKind.Vista => new Color(160, 120, 255), 
				TargetKind.Heart => new Color(255, 170, 60), 
				TargetKind.SkillPoint => new Color(255, 220, 90), 
				_ => Color.get_White(), 
			});
		}
	}
}
