using System;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Cronos;
using Microsoft.Xna.Framework;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Filter
{
	internal class ScheduleFilter : IBehavior, IUpdatable, ICanFilter
	{
		public const string PRIMARY_ATTR_NAME = "schedule";

		public const string ATTR_DURATION = "schedule-duration";

		private DateTime _nextTrigger = DateTime.MinValue;

		private bool _isFiltered;

		public CronExpression CronExpression { get; set; }

		public TimeSpan Duration { get; set; }

		public ScheduleFilter(CronExpression cronExpression, TimeSpan duration)
		{
			CronExpression = cronExpression;
			Duration = duration;
			UpdateNextTrigger(accountForDuration: true);
		}

		private void UpdateNextTrigger(bool accountForDuration)
		{
			DateTime? nextTrigger = (accountForDuration ? CronExpression.GetNextOccurrence(DateTime.UtcNow - Duration, inclusive: true) : CronExpression.GetNextOccurrence(DateTime.UtcNow, inclusive: true));
			if (nextTrigger.HasValue)
			{
				_nextTrigger = nextTrigger.Value;
			}
		}

		public void Update(GameTime gameTime)
		{
			DateTime now = DateTime.UtcNow;
			if (!_isFiltered && now > _nextTrigger + Duration)
			{
				UpdateNextTrigger(accountForDuration: false);
				_isFiltered = true;
			}
			else if (now >= _nextTrigger && now < _nextTrigger + Duration)
			{
				_isFiltered = false;
			}
			else
			{
				_isFiltered = true;
			}
		}

		public void Unload()
		{
		}

		public bool IsFiltered()
		{
			return _isFiltered;
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes)
		{
			if (attributes.TryGetAttribute("schedule", out var expressionAttr) && attributes.TryGetAttribute("schedule-duration", out var durationAttr))
			{
				CronExpression expression = expressionAttr.GetValueAsCronExpression();
				float duration = durationAttr.GetValueAsFloat();
				if (expression != null && duration > 0f)
				{
					return new ScheduleFilter(expression, TimeSpan.FromMinutes(duration));
				}
			}
			return null;
		}
	}
}
