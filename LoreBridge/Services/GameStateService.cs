using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.GameServices.ArcDps.V2;
using Blish_HUD.GameServices.ArcDps.V2.Models;
using LoreBridge.Models;
using LoreBridge.Services.GameState;
using LoreBridge.Utils;
using Microsoft.Xna.Framework;

namespace LoreBridge.Services
{
	public class GameStateService : Service
	{
		private GameStateType _gameState = GameStateType.None;

		private IArcDpsMessageListener<ImGuiCallback> _imGuiListener;

		private bool _isInCharacterSelectOrLoading;

		private bool _isInGame;

		private double _lastTick;

		private int _tickDelay = 25;

		public GameStateType CurrentGameState
		{
			get
			{
				return _gameState;
			}
			private set
			{
				if (_gameState != value)
				{
					_gameState = value;
					this.GameStateChanged?.Invoke(this, _gameState);
				}
			}
		}

		public event EventHandler<GameStateType> GameStateChanged;

		public override void Load(Settings settings)
		{
			_isInGame = GameService.GameIntegration.get_Gw2Instance().get_IsInGame();
			GameService.GameIntegration.get_Gw2Instance().add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChange);
			try
			{
				_isInCharacterSelectOrLoading = GameService.ArcDpsV2.get_HudIsActive();
				_imGuiListener = (IArcDpsMessageListener<ImGuiCallback>)(object)new ArcDpsMessageListener<ImGuiCallback>((MessageType)1, (Func<ImGuiCallback, CancellationToken, Task>)OnGuiChange);
				GameService.ArcDpsV2.RegisterMessageType<ImGuiCallback>(_imGuiListener);
			}
			catch
			{
			}
		}

		public override void Update(GameTime gameTime)
		{
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			if (gameTime.get_TotalGameTime().TotalMilliseconds - _lastTick <= (double)_tickDelay)
			{
				return;
			}
			_lastTick = gameTime.get_TotalGameTime().TotalMilliseconds;
			if (!GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus())
			{
				return;
			}
			GameStateType newStatus = GameStateType.Unknown;
			if (_isInCharacterSelectOrLoading)
			{
				newStatus = GameStateType.LoadingOrCharacterSelection;
			}
			if (_isInGame)
			{
				newStatus = GameStateType.InGame;
			}
			if (newStatus != 0 || _gameState != GameStateType.None)
			{
				if (!_isInGame && !_isInCharacterSelectOrLoading)
				{
					Point resolution = GameService.Graphics.get_Resolution();
					Point size = default(Point);
					((Point)(ref size))._002Ector(50, 50);
					Bitmap screen = Screen.GetScreen(new Point(resolution.X - size.X, 0), size);
					Bitmap bottomLeft = Screen.GetScreen(new Point(0, resolution.Y - size.Y), size);
					newStatus = ((IsBitmapBlack(screen) && IsBitmapBlack(bottomLeft)) ? GameStateType.Cutscene : GameStateType.Unknown);
				}
				_tickDelay = ((newStatus == GameStateType.Cutscene) ? 250 : 25);
				CurrentGameState = newStatus;
			}
		}

		public override void Unload()
		{
			GameService.GameIntegration.get_Gw2Instance().remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChange);
			((IDisposable)_imGuiListener)?.Dispose();
		}

		private static bool IsBitmapBlack(Bitmap bitmap)
		{
			for (int y = 0; y < bitmap.Height; y++)
			{
				for (int x = 0; x < bitmap.Width; x++)
				{
					Color pixelColor = bitmap.GetPixel(x, y);
					if (pixelColor.R > 1 || pixelColor.G > 1 || pixelColor.B > 1)
					{
						return false;
					}
				}
			}
			return true;
		}

		private void OnIsInGameChange(object o, ValueEventArgs<bool> e)
		{
			_isInGame = e.get_Value();
		}

		private Task OnGuiChange(ImGuiCallback state, CancellationToken cancellationToken)
		{
			_isInCharacterSelectOrLoading = ((ImGuiCallback)(ref state)).get_NotCharacterSelectOrLoading() == 0;
			return Task.CompletedTask;
		}
	}
}
