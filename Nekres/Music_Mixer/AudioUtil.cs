using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Blish_HUD;
using Glide;
using Microsoft.Xna.Framework;
using NAudio.CoreAudioApi;

namespace Nekres.Music_Mixer
{
	public static class AudioUtil
	{
		private static Tween _animEase;

		private static List<SimpleAudioVolume> _volumes;

		public static float GetNormalizedVolume(float volume)
		{
			float masterVolume = MathHelper.Clamp(MusicMixer.Instance.MasterVolume.get_Value() / 1000f, 0f, 1f);
			if (volume >= masterVolume)
			{
				return masterVolume;
			}
			return MathHelper.Clamp(masterVolume - Math.Abs(volume - masterVolume), 0f, 1f);
		}

		public static void SetVolume(int processId, float targetVolume, float duration = 2f)
		{
			Tween animEase = _animEase;
			if (animEase != null)
			{
				animEase.Cancel();
			}
			Dispose(_volumes);
			List<SimpleAudioVolume> volumes = GetVolumes(processId);
			if (!volumes.Any())
			{
				return;
			}
			float currentVolume = volumes.Average((SimpleAudioVolume v) => v.get_Volume());
			_animEase = ((TweenerImpl)GameService.Animation.get_Tweener()).Timer(duration, 0f).Ease((Func<float, float>)delegate(float t)
			{
				float num = (targetVolume - currentVolume) * t;
				float num2 = currentVolume + num;
				foreach (SimpleAudioVolume item in volumes)
				{
					item.set_Volume((num2 > 1f) ? 1f : ((num2 < 0f) ? 0f : num2));
				}
				return num2;
			}).OnComplete((Action)delegate
			{
				Dispose(volumes);
			});
			_volumes = volumes.ToList();
		}

		private static void Dispose(List<SimpleAudioVolume> disposables)
		{
			if (disposables == null)
			{
				return;
			}
			try
			{
				lock (disposables)
				{
					foreach (SimpleAudioVolume disposable in disposables)
					{
						disposable.Dispose();
					}
					disposables.Clear();
				}
			}
			finally
			{
				_animEase = null;
				_volumes = null;
			}
		}

		private static List<SimpleAudioVolume> GetVolumes(int processId)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			List<SimpleAudioVolume> volumes = new List<SimpleAudioVolume>();
			MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
			try
			{
				foreach (MMDevice device in deviceEnumerator.EnumerateAudioEndPoints((DataFlow)0, (DeviceState)1))
				{
					SessionCollection sessionEnumerator = null;
					try
					{
						sessionEnumerator = device.get_AudioSessionManager().get_Sessions();
					}
					catch (COMException ex) when (ex.HResult == -2004287480)
					{
						continue;
					}
					catch (COMException ex2) when (ex2.HResult == -2147221164)
					{
						continue;
					}
					catch (COMException ex3) when (ex3.HResult == -2147023728)
					{
						continue;
					}
					catch (COMException ex4) when (ex4.HResult == -2147024891)
					{
						continue;
					}
					catch (Exception)
					{
						continue;
					}
					for (int i = 0; i < sessionEnumerator.get_Count(); i++)
					{
						AudioSessionControl audioSession = sessionEnumerator.get_Item(i);
						try
						{
							if (audioSession.get_GetProcessID() == processId)
							{
								volumes.Add(audioSession.get_SimpleAudioVolume());
							}
						}
						finally
						{
							((IDisposable)audioSession)?.Dispose();
						}
					}
					device.Dispose();
				}
				return volumes;
			}
			finally
			{
				((IDisposable)deviceEnumerator)?.Dispose();
			}
		}
	}
}
