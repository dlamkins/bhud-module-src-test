namespace FarmingTracker
{
	public class UpdateLoop
	{
		public const int RETRY_AFTER_API_FAILURE_UPDATE_INTERVAL_MS = 5000;

		public const int WAIT_FOR_API_TOKEN_UPDATE_INTERVALL_MS = 2000;

		private const int FARMING_UPDATE_INTERVAL_MS = 1000;

		private double _runningTimeMs;

		private double _updateIntervalMs;

		private bool _uiHasToBeUpdated;

		private bool _statsHaveToBeUpdated;

		private bool _modelHasToBeSaved;

		public void AddToRunningTime(double milliseconds)
		{
			_runningTimeMs += milliseconds;
		}

		public void ResetRunningTime()
		{
			_runningTimeMs = 0.0;
		}

		public bool UpdateIntervalEnded()
		{
			return _runningTimeMs >= _updateIntervalMs;
		}

		public void UseFarmingUpdateInterval()
		{
			_updateIntervalMs = 1000.0;
		}

		public void UseRetryAfterApiFailureUpdateInterval()
		{
			_updateIntervalMs = 5000.0;
		}

		public void TriggerUpdateStats()
		{
			_statsHaveToBeUpdated = true;
			_runningTimeMs = _updateIntervalMs;
		}

		public bool HasToUpdateStats()
		{
			if (!_statsHaveToBeUpdated)
			{
				return false;
			}
			_statsHaveToBeUpdated = false;
			return true;
		}

		public void TriggerUpdateUi()
		{
			_uiHasToBeUpdated = true;
		}

		public bool HasToUpdateUi()
		{
			if (!_uiHasToBeUpdated)
			{
				return false;
			}
			_uiHasToBeUpdated = false;
			return true;
		}

		public void TriggerSaveModel()
		{
			_modelHasToBeSaved = true;
		}

		public bool HasToSaveModel()
		{
			if (!_modelHasToBeSaved)
			{
				return false;
			}
			_modelHasToBeSaved = false;
			return true;
		}
	}
}
