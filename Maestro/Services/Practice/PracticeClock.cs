using System;

namespace Maestro.Services.Practice
{
	public class PracticeClock
	{
		private double _currentMs;

		private float _speed = 1f;

		private bool _paused;

		public int CurrentMs => (int)Math.Round(_currentMs);

		public bool IsPaused => _paused;

		public float Speed
		{
			get
			{
				return _speed;
			}
			set
			{
				_speed = Math.Max(0.1f, Math.Min(2f, value));
			}
		}

		public void Advance(int elapsedMs)
		{
			if (!_paused && elapsedMs > 0)
			{
				_currentMs += (float)elapsedMs * _speed;
			}
		}

		public void Pause()
		{
			_paused = true;
		}

		public void Resume()
		{
			_paused = false;
		}

		public void Seek(int absoluteMs)
		{
			_currentMs = absoluteMs;
		}
	}
}
