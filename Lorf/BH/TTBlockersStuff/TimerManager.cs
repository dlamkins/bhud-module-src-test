using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Lorf.BH.TTBlockersStuff.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTBlockersStuff.Language;

namespace Lorf.BH.TTBlockersStuff
{
	internal class TimerManager
	{
		private static readonly Logger Logger = Logger.GetLogger<TimerManager>();

		private TimeSpan textBlinkDuration = new TimeSpan(2000000L);

		private TimeSpan lastBlinkRequestTime = TimeSpan.Zero;

		private int lastBlinkRequestSecond;

		private DateTime targetTime;

		public string Name { get; set; }

		public bool Active { get; private set; }

		public TimerBar TimerBar { get; set; }

		public TimerManager()
		{
			targetTime = DateTime.MinValue;
		}

		public void Activate(int time)
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			if (Active)
			{
				Active = false;
				TimerBar.BarText = Name + " (" + Translations.TimerBarTextReady + ")";
				TimerBar.Value = TimerBar.MaxValue;
				targetTime = DateTime.MinValue;
				return;
			}
			Vector3 position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			Logger.Debug($"Timer {Name} activated (x: {position.X}, y: {position.Y}, z: {position.Z}, time: {time})");
			targetTime = DateTime.UtcNow.AddSeconds(time);
			TimerBar.MaxValue = time;
			Active = true;
		}

		public void Update(GameTime gameTime)
		{
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			if (!Active)
			{
				return;
			}
			DateTime utcNow = DateTime.UtcNow;
			double totalSeconds = (targetTime - utcNow).TotalSeconds;
			if (targetTime > utcNow && totalSeconds > 1.0)
			{
				TimerBar.BarText = $"{Name} ({(int)totalSeconds}s)";
				TimerBar.Value = TimerBar.MaxValue - (float)((targetTime - utcNow).TotalMilliseconds / 1000.0);
				if (TimerBar.MaxValue != (float)(int)totalSeconds && ((int)totalSeconds % 10 == 0 || totalSeconds < 10.0) && lastBlinkRequestTime == TimeSpan.Zero && lastBlinkRequestSecond != (int)totalSeconds)
				{
					lastBlinkRequestTime = gameTime.get_TotalGameTime();
					lastBlinkRequestSecond = (int)totalSeconds;
				}
				if (lastBlinkRequestTime.Add(textBlinkDuration) > gameTime.get_TotalGameTime())
				{
					TimerBar.TextColor = Color.get_Red();
					return;
				}
				TimerBar.TextColor = Color.get_White();
				lastBlinkRequestTime = TimeSpan.Zero;
			}
			else
			{
				Active = false;
				ScreenNotification.ShowNotification(Name + " " + Translations.TimerBarTextReady + "!", (NotificationType)0, (Texture2D)null, 4);
				TimerBar.BarText = Name + " (" + Translations.TimerBarTextReady + ")";
				TimerBar.Value = TimerBar.MaxValue;
				TimerBar.TextColor = Color.get_White();
			}
		}
	}
}
