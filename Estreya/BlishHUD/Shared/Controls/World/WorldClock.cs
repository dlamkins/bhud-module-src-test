using System;
using System.Collections.Generic;
using Blish_HUD.Entities;
using Estreya.BlishHUD.Shared.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Controls.World
{
	public class WorldClock : WorldEntity
	{
		private readonly float _radius;

		private readonly Func<DateTime> _getNow;

		private WorldCircle _circlePolygone;

		private WorldPolygone _hourPolygone;

		private WorldPolygone _minutePolygone;

		private WorldPolygone _secondPolygone;

		private List<WorldPolygone> _minuteMarkPolygones;

		public WorldClock(Vector3 position, float radius, Func<DateTime> getNow)
			: base(position, 1f)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			_radius = radius;
			_getNow = getNow;
			CreatePolygones();
		}

		private void CreatePolygones()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			_circlePolygone = new WorldCircle(base.Position, _radius);
			_hourPolygone = new WorldPolygone(base.Position, (Vector3[])(object)new Vector3[2]
			{
				Vector3.get_Zero(),
				Vector3.get_Zero()
			});
			_minutePolygone = new WorldPolygone(base.Position, (Vector3[])(object)new Vector3[2]
			{
				Vector3.get_Zero(),
				Vector3.get_Zero()
			});
			_secondPolygone = new WorldPolygone(base.Position, (Vector3[])(object)new Vector3[2]
			{
				Vector3.get_Zero(),
				Vector3.get_Zero()
			});
			_minuteMarkPolygones = CreateMinuteMarkPolygones();
		}

		private List<WorldPolygone> CreateMinuteMarkPolygones()
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			List<WorldPolygone> polygones = new List<WorldPolygone>();
			int marks = 12;
			for (int i = 0; i < marks; i++)
			{
				int degreeMinuteMark = i.Remap(0, marks, 0, 360);
				double angleMinuteMark = Math.PI * (double)degreeMinuteMark / 180.0;
				float angleMinuteMarkOuterX = _radius * (float)Math.Cos(angleMinuteMark);
				float angleMinuteMarkOuterY = _radius * (float)Math.Sin(angleMinuteMark);
				float angleMinuteMarkInnerX = _radius * 0.95f * (float)Math.Cos(angleMinuteMark);
				float angleMinuteMarkInnerY = _radius * 0.95f * (float)Math.Sin(angleMinuteMark);
				polygones.Add(new WorldPolygone(base.Position, (Vector3[])(object)new Vector3[2]
				{
					new Vector3(angleMinuteMarkOuterX, angleMinuteMarkOuterY, 0f),
					new Vector3(angleMinuteMarkInnerX, angleMinuteMarkInnerY, 0f)
				})
				{
					ScaleX = base.ScaleX,
					ScaleY = base.ScaleY,
					ScaleZ = base.ScaleZ,
					RotationX = base.RotationX,
					RotationY = base.RotationY,
					RotationZ = base.RotationZ
				});
			}
			return polygones;
		}

		public override bool IsPlayerInside(bool includeZAxis = true)
		{
			return false;
		}

		protected override void InternalRender(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			DateTime now = _getNow();
			ApplyProperties(_circlePolygone);
			int degreeHour = now.Hour.Remap(0, 60, 0, 360) * -1;
			double angleHour = Math.PI * (double)((float)degreeHour - 90f) / 180.0;
			float angleHourX = _radius * 0.5f * (float)Math.Cos(angleHour);
			float angleHourY = _radius * 0.5f * (float)Math.Sin(angleHour);
			_hourPolygone.Points = (Vector3[])(object)new Vector3[2]
			{
				Vector3.get_Zero(),
				new Vector3(angleHourX, angleHourY, 0f)
			};
			ApplyProperties(_hourPolygone);
			int degreeMinute = now.Minute.Remap(0, 60, 0, 360) * -1;
			double angleMinute = Math.PI * (double)((float)degreeMinute - 90f) / 180.0;
			float angleMinuteX = _radius * 0.75f * (float)Math.Cos(angleMinute);
			float angleMinuteY = _radius * 0.75f * (float)Math.Sin(angleMinute);
			_minutePolygone.Points = (Vector3[])(object)new Vector3[2]
			{
				Vector3.get_Zero(),
				new Vector3(angleMinuteX, angleMinuteY, 0f)
			};
			ApplyProperties(_minutePolygone);
			int degreeSecond = now.Second.Remap(0, 60, 0, 360) * -1;
			double angleSecond = Math.PI * (double)((float)degreeSecond - 90f) / 180.0;
			float angleSecondX = _radius * (float)Math.Cos(angleSecond);
			float angleSecondY = _radius * (float)Math.Sin(angleSecond);
			_secondPolygone.Points = (Vector3[])(object)new Vector3[2]
			{
				Vector3.get_Zero(),
				new Vector3(angleSecondX, angleSecondY, 0f)
			};
			ApplyProperties(_secondPolygone);
			_circlePolygone.Render(graphicsDevice, world, camera);
			_hourPolygone.Render(graphicsDevice, world, camera);
			_minutePolygone.Render(graphicsDevice, world, camera);
			_secondPolygone.Render(graphicsDevice, world, camera);
			foreach (WorldPolygone minuteMark in _minuteMarkPolygones)
			{
				ApplyProperties(minuteMark);
				minuteMark.Render(graphicsDevice, world, camera);
			}
		}

		private void ApplyProperties(WorldEntity entity)
		{
			entity.ScaleX = base.ScaleX;
			entity.ScaleY = base.ScaleY;
			entity.ScaleZ = base.ScaleZ;
			entity.RotationX = base.RotationX;
			entity.RotationY = base.RotationY;
			entity.RotationZ = base.RotationZ;
		}
	}
}
