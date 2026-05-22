using System;
using System.Collections.Generic;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using TyriaPlanner.Hud.Settings;

namespace TyriaPlanner.Hud.Ui
{
	public sealed class ToastStack : IDisposable
	{
		private const int Margin = 12;

		private const int TopBarClearance = 36;

		private const int Spacing = 8;

		private const int ToastWidth = 380;

		private const int MaxVisible = 4;

		private readonly List<Container> _toasts = new List<Container>();

		private readonly Queue<Func<Container>> _deferred = new Queue<Func<Container>>();

		private readonly ModuleSettings _settings;

		private readonly Timer _combatPoller;

		public event Action<Container> ToastPushed;

		public ToastStack(ModuleSettings settings)
		{
			_settings = settings;
			_combatPoller = new Timer(delegate
			{
				TryDrainDeferred();
			}, null, TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0));
		}

		public void Push(Func<Container> factory)
		{
			if (factory == null)
			{
				return;
			}
			if (ShouldDefer())
			{
				lock (_deferred)
				{
					_deferred.Enqueue(factory);
				}
				return;
			}
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				try
				{
					Show(factory());
				}
				catch
				{
				}
			});
		}

		public void Push(Container toast)
		{
			if (toast == null)
			{
				return;
			}
			if (ShouldDefer())
			{
				lock (_deferred)
				{
					_deferred.Enqueue(() => toast);
				}
			}
			else
			{
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					Show(toast);
				});
			}
		}

		private bool ShouldDefer()
		{
			if (_settings?.PauseInCombat == null || !_settings.PauseInCombat.get_Value())
			{
				return false;
			}
			try
			{
				return GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat();
			}
			catch
			{
				return false;
			}
		}

		private void TryDrainDeferred()
		{
			if (ShouldDefer())
			{
				return;
			}
			List<Func<Container>> drained;
			lock (_deferred)
			{
				if (_deferred.Count == 0)
				{
					return;
				}
				drained = new List<Func<Container>>(_deferred);
				_deferred.Clear();
			}
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				foreach (Func<Container> current in drained)
				{
					try
					{
						Show(current());
					}
					catch
					{
					}
				}
			});
		}

		private void Show(Container toast)
		{
			Screen screen = GameService.Graphics.get_SpriteScreen();
			((Control)toast).set_Parent((Container)(object)screen);
			((Control)toast).set_Width(380);
			((Control)toast).add_Disposed((EventHandler<EventArgs>)OnToastDisposed);
			_toasts.Add(toast);
			if (_toasts.Count > 4)
			{
				Container obj = _toasts[0];
				_toasts.RemoveAt(0);
				((Control)obj).Dispose();
			}
			ReflowLocation((Control)(object)screen);
			this.ToastPushed?.Invoke(toast);
		}

		private void OnToastDisposed(object sender, EventArgs e)
		{
			Container t = (Container)((sender is Container) ? sender : null);
			if (t != null)
			{
				_toasts.Remove(t);
				ReflowLocation((Control)(object)GameService.Graphics.get_SpriteScreen());
			}
		}

		private void ReflowLocation(Control screen)
		{
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			if (screen == null)
			{
				return;
			}
			switch ((_settings?.ToastPosition?.get_Value()).GetValueOrDefault())
			{
			case ToastPositionPreference.TopRight:
			{
				int x = screen.get_Width() - 380 - 12;
				int y = 36;
				foreach (Container t in _toasts)
				{
					((Control)t).set_Location(new Point(x, y));
					y += ((Control)t).get_Height() + 8;
				}
				return;
			}
			case ToastPositionPreference.BottomRight:
			{
				int x2 = screen.get_Width() - 380 - 12;
				int y2 = screen.get_Height() - 12;
				for (int i = _toasts.Count - 1; i >= 0; i--)
				{
					Container t2 = _toasts[i];
					y2 -= ((Control)t2).get_Height();
					((Control)t2).set_Location(new Point(x2, y2));
					y2 -= 8;
				}
				return;
			}
			}
			int x3 = (screen.get_Width() - 380) / 2;
			int y3 = 36;
			foreach (Container t3 in _toasts)
			{
				((Control)t3).set_Location(new Point(x3, y3));
				y3 += ((Control)t3).get_Height() + 8;
			}
		}

		public void Clear()
		{
			Container[] array = _toasts.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				((Control)array[i]).Dispose();
			}
			_toasts.Clear();
			lock (_deferred)
			{
				_deferred.Clear();
			}
		}

		public void Dispose()
		{
			_combatPoller?.Dispose();
			Clear();
		}
	}
}
