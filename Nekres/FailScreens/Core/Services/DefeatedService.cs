using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Nekres.FailScreens.Core.UI.Controls.Screens;

namespace Nekres.FailScreens.Core.Services
{
	internal class DefeatedService : IDisposable
	{
		public enum FailScreens
		{
			DarkSouls,
			GrandTheftAuto,
			RytlocksCritterRampage,
			Windows,
			AngryPepe,
			SekiroShadowsDieTwice
		}

		private Control _failScreen;

		private bool _isSuperAdventureBox;

		private int _dblClickCount;

		private Stopwatch _dblClickWatch;

		private uint _dblClickMs;

		private Point _dblClickPos;

		public DefeatedService()
		{
			OnMapChanged(GameService.Gw2Mumble.get_CurrentMap(), new ValueEventArgs<int>(GameService.Gw2Mumble.get_CurrentMap().get_Id()));
			FailScreensModule.Instance.State.StateChanged += OnStateChanged;
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseButtonReleased);
			_dblClickWatch = new Stopwatch();
		}

		[DllImport("user32.dll")]
		private static extern uint GetDoubleClickTime();

		private void OnLeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			if (_failScreen == null)
			{
				_dblClickCount = 0;
				return;
			}
			if (_dblClickCount == 0)
			{
				_dblClickPos = GameService.Input.get_Mouse().get_PositionRaw();
				_dblClickMs = GetDoubleClickTime();
				_dblClickWatch.Restart();
			}
			Point pos = GameService.Input.get_Mouse().get_PositionRaw();
			if (Math.Abs(_dblClickPos.X - pos.X) > 5 || Math.Abs(_dblClickPos.Y - pos.Y) > 5)
			{
				_dblClickCount = 0;
				return;
			}
			_dblClickCount++;
			if (_dblClickCount <= 1)
			{
				return;
			}
			_dblClickCount = 0;
			if (_dblClickWatch.ElapsedMilliseconds < _dblClickMs)
			{
				Control failScreen = _failScreen;
				if (failScreen != null)
				{
					failScreen.Dispose();
				}
				_failScreen = null;
			}
		}

		private async void OnMapChanged(object sender, ValueEventArgs<int> e)
		{
			try
			{
				Map map = await ((IBulkExpandableClient<Map, int>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
					.get_Maps()).GetAsync(e.get_Value(), default(CancellationToken));
				_isSuperAdventureBox = map != null && map.get_RegionId() == 29;
			}
			catch (Exception ex)
			{
				FailScreensModule.Logger.Info(ex, ex.Message);
				_isSuperAdventureBox = false;
			}
		}

		private void OnStateChanged(object sender, ValueEventArgs<StateService.State> e)
		{
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			if (e.get_Value() != StateService.State.Defeated)
			{
				if (!_isSuperAdventureBox)
				{
					Control failScreen = _failScreen;
					if (failScreen != null)
					{
						failScreen.Dispose();
					}
					_failScreen = null;
				}
				return;
			}
			Control failScreen2 = _failScreen;
			if (failScreen2 != null)
			{
				failScreen2.Dispose();
			}
			_failScreen = null;
			FailScreens screen = FailScreensModule.Instance.FailScreen.get_Value();
			if (FailScreensModule.Instance.Random.get_Value())
			{
				int num = Enum.GetValues(typeof(FailScreens)).Cast<int>().Min();
				int max = Enum.GetValues(typeof(FailScreens)).Cast<int>().Max();
				screen = (FailScreens)RandomUtil.GetRandom(num, max);
			}
			Control buildScreen;
			try
			{
				buildScreen = CreateFailScreen(screen);
			}
			catch (Exception ex)
			{
				FailScreensModule.Logger.Warn(ex, $"Failed to construct {screen}.");
				return;
			}
			if (buildScreen != null)
			{
				buildScreen.set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				buildScreen.set_Size(((Control)GameService.Graphics.get_SpriteScreen()).get_Size());
				_failScreen = buildScreen;
			}
		}

		private Control CreateFailScreen(FailScreens failScreen)
		{
			return (Control)(failScreen switch
			{
				FailScreens.DarkSouls => new DarkSouls(), 
				FailScreens.GrandTheftAuto => new GrantTheftAuto(), 
				FailScreens.RytlocksCritterRampage => new RytlocksCritterRampage(), 
				FailScreens.Windows => new WinXp(), 
				FailScreens.AngryPepe => new AngryPepe(), 
				FailScreens.SekiroShadowsDieTwice => new Sekiro(), 
				_ => null, 
			});
		}

		public void Dispose()
		{
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseButtonReleased);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			FailScreensModule.Instance.State.StateChanged -= OnStateChanged;
			Control failScreen = _failScreen;
			if (failScreen != null)
			{
				failScreen.Dispose();
			}
		}
	}
}
