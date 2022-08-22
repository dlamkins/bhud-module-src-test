using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Lorf.BH.TTBlockersStuff.UI;
using Microsoft.Xna.Framework;
using TTBlockersStuff.Language;

namespace Lorf.BH.TTBlockersStuff
{
	internal class TimerManager
	{
		private static readonly Logger Logger = Logger.GetLogger<TimerManager>();

		private DateTime targetTime;

		public string Name { get; set; }

		public bool Active { get; private set; }

		public TimerBar TimerBar { get; set; }

		public void Reset()
		{
			targetTime = DateTime.MinValue;
			TimerBar.MaxValue = 1f;
			TimerBar.Value = 1f;
		}

		public void Activate(int time)
		{
			Vector3 pos = GameService.Gw2Mumble.PlayerCharacter.Position;
			Logger.Debug($"Timer {Name} activated (x: {pos.X}, y: {pos.Y}, z: {pos.Z}, time: {time})");
			targetTime = DateTime.UtcNow.AddSeconds(time);
			TimerBar.MaxValue = time;
			Active = true;
		}

		public void Update()
		{
			if (Active)
			{
				DateTime now = DateTime.UtcNow;
				double secondsRemaining = (targetTime - now).TotalSeconds;
				if (targetTime > now && secondsRemaining > 1.0)
				{
					TimerBar.BarText = $"{Name} ({(int)secondsRemaining}s)";
					TimerBar.Value = TimerBar.MaxValue - (float)((targetTime - now).TotalMilliseconds / 1000.0);
					return;
				}
				Active = false;
				ScreenNotification.ShowNotification(Name + " " + Translations.TimerBarTextReady + "!");
				TimerBar.BarText = Name + " (" + Translations.TimerBarTextReady + ")";
				TimerBar.Value = TimerBar.MaxValue;
			}
		}
	}
}
