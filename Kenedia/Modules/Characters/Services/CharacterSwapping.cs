using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Gw2Mumble;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Views;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Characters.Services
{
	public class CharacterSwapping
	{
		private readonly Settings _settings;

		private readonly GameState _gameState;

		private readonly ObservableCollection<Character_Model> _characterModels;

		private bool _ignoreOCR;

		private CancellationTokenSource _cancellationTokenSource;

		private int _movedLeft;

		private string _status;

		private SwappingState _state;

		public OCR OCR { get; set; }

		public Action HideMainWindow { get; set; }

		public CharacterSorting CharacterSorting { get; set; }

		public string Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
				this.StatusChanged?.Invoke(null, EventArgs.Empty);
			}
		}

		public Character_Model Character { get; set; }

		public event EventHandler Succeeded;

		public event EventHandler Failed;

		public event EventHandler Started;

		public event EventHandler Finished;

		public event EventHandler StatusChanged;

		public CharacterSwapping(Settings settings, GameState gameState, ObservableCollection<Character_Model> characterModels)
		{
			_settings = settings;
			_gameState = gameState;
			_characterModels = characterModels;
		}

		private bool IsTaskCanceled(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				if (_state != SwappingState.LoggedOut)
				{
					_movedLeft = 0;
				}
				if (_state == SwappingState.MovedToStart)
				{
					_movedLeft = _characterModels.Count;
				}
				_state = SwappingState.Canceled;
				return true;
			}
			return false;
		}

		public async Task MoveRight(CancellationToken cancellationToken, int amount = 1)
		{
			Status = strings.CharacterSwap_Right;
			for (int i = 0; i < amount; i++)
			{
				Keyboard.Stroke((VirtualKeyShort)39, false);
				await Delay(cancellationToken);
			}
		}

		public async Task MoveLeft(CancellationToken cancellationToken, int amount = 1)
		{
			Status = strings.CharacterSwap_Left;
			for (int i = 0; i < amount; i++)
			{
				Keyboard.Stroke((VirtualKeyShort)37, false);
				await Delay(cancellationToken);
			}
		}

		public async Task<bool> IsNoKeyPressed(CancellationToken cancellationToken)
		{
			while (GameService.Input.get_Keyboard().get_KeysDown().Count > 0)
			{
				if (IsTaskCanceled(cancellationToken))
				{
					return false;
				}
				await Delay(cancellationToken, 250);
			}
			return true;
		}

		public async Task Run(CancellationToken cancellationToken)
		{
			if (IsTaskCanceled(cancellationToken))
			{
				return;
			}
			switch (_state)
			{
			case SwappingState.None:
				if (await LoggingOut(cancellationToken))
				{
					_state = SwappingState.LoggedOut;
				}
				await Delay(cancellationToken);
				break;
			case SwappingState.LoggedOut:
				await MoveToFirstCharacter(cancellationToken);
				_state = SwappingState.MovedToStart;
				_movedLeft = 0;
				await Delay(cancellationToken, 250);
				break;
			case SwappingState.MovedToStart:
				await MoveToCharacter(cancellationToken);
				_state = SwappingState.MovedToCharacter;
				await Delay(cancellationToken, 250);
				break;
			case SwappingState.MovedToCharacter:
			{
				if (await ConfirmName())
				{
					_state = SwappingState.CharacterFound;
					break;
				}
				_state = SwappingState.CharacterLost;
				await MoveLeft(cancellationToken, Math.Min(_characterModels.Count, _settings.CheckDistance.get_Value()));
				for (int i = 1; i < Math.Min(_characterModels.Count, _settings.CheckDistance.get_Value() * 2); i++)
				{
					await MoveRight(cancellationToken);
					await Delay(cancellationToken, 150);
					if (await ConfirmName())
					{
						_state = SwappingState.CharacterFound;
						return;
					}
				}
				_state = SwappingState.CharacterFullyLost;
				break;
			}
			case SwappingState.CharacterFound:
				if (await Login(cancellationToken))
				{
					_state = SwappingState.LoggingIn;
				}
				break;
			case SwappingState.LoggingIn:
				if (IsLoaded())
				{
					_state = SwappingState.Done;
				}
				break;
			}
		}

		public void Reset()
		{
			_state = SwappingState.None;
		}

		public bool Cancel()
		{
			bool result = _cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested;
			_state = SwappingState.Canceled;
			CancellationTokenSource cancellationTokenSource = _cancellationTokenSource;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
				return result;
			}
			return result;
		}

		public async void Start(Character_Model character, bool ignoreOCR = false, Logger logger = null)
		{
			PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
			bool inCharSelection = (_settings.UseBetaGamestate.get_Value() ? _gameState.IsCharacterSelection : (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame()));
			if (player != null && player.get_Name() == character.Name && !inCharSelection)
			{
				return;
			}
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource = new CancellationTokenSource();
			_cancellationTokenSource.CancelAfter(90000);
			Character = character;
			_state = ((!GameService.GameIntegration.get_Gw2Instance().get_IsInGame()) ? SwappingState.LoggedOut : SwappingState.None);
			_ignoreOCR = ignoreOCR;
			this.Started?.Invoke(null, null);
			Status = string.Format(strings.CharacterSwap_SwitchTo, Character.Name);
			while (true)
			{
				SwappingState state = _state;
				if (state == SwappingState.Done || state == SwappingState.CharacterFullyLost || state == SwappingState.Canceled || _cancellationTokenSource.Token.IsCancellationRequested)
				{
					break;
				}
				try
				{
					await Run(_cancellationTokenSource.Token);
					switch (_state)
					{
					case SwappingState.Done:
						Status = strings.Status_Done;
						if (GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && _settings.CloseWindowOnSwap.get_Value())
						{
							HideMainWindow?.Invoke();
						}
						break;
					case SwappingState.CharacterFullyLost:
						Status = string.Format(strings.CharacterSwap_FailedSwap, Character.Name);
						this.Failed?.Invoke(null, null);
						if (_settings.AutoSortCharacters.get_Value())
						{
							CharacterSorting.Start();
						}
						break;
					}
				}
				catch (TaskCanceledException)
				{
				}
			}
			this.Finished?.Invoke(null, null);
		}

		private async Task Delay(CancellationToken cancellationToken, int? delay = null, double? partial = null)
		{
			delay.GetValueOrDefault();
			if (!delay.HasValue)
			{
				int value = _settings.KeyDelay.get_Value();
				delay = value;
			}
			if (delay > 0)
			{
				if (partial.HasValue)
				{
					delay = delay / 100 * (int)(partial * 100.0).Value;
				}
				await Task.Delay(delay.Value, cancellationToken);
			}
		}

		private async Task<bool> LoggingOut(CancellationToken cancellationToken)
		{
			if (IsTaskCanceled(cancellationToken))
			{
				return false;
			}
			if (GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				Status = strings.CharacterSwap_Logout;
				await _settings.LogoutKey.get_Value().PerformPress(_settings.KeyDelay.get_Value(), triggerSystem: true, cancellationToken);
				Keyboard.Stroke((VirtualKeyShort)13, false);
				await Delay(cancellationToken);
				Stopwatch stopwatch = new Stopwatch();
				if (_settings.UseBetaGamestate.get_Value())
				{
					while (!_gameState.IsCharacterSelection && !cancellationToken.IsCancellationRequested)
					{
						await Delay(cancellationToken, 50);
						if (cancellationToken.IsCancellationRequested)
						{
							return _gameState.IsCharacterSelection;
						}
					}
					if (_settings.UseOCR.get_Value())
					{
						stopwatch.Start();
						string txt2 = await OCR.Read();
						while (stopwatch.ElapsedMilliseconds < 5000 && txt2.Length <= 2 && !cancellationToken.IsCancellationRequested)
						{
							BaseModule<Characters, MainWindow, Settings>.Logger.Debug("We are in the character selection but the OCR did only read '" + txt2 + "'. Waiting a bit longer!");
							await Delay(cancellationToken, 250);
							txt2 = await OCR.Read();
							if (cancellationToken.IsCancellationRequested)
							{
								return _gameState.IsCharacterSelection;
							}
						}
					}
				}
				else
				{
					await Delay(cancellationToken, _settings.SwapDelay.get_Value());
					if (_settings.UseOCR.get_Value())
					{
						stopwatch.Start();
						string txt = await OCR.Read();
						while (stopwatch.ElapsedMilliseconds < 5000 && txt.Length <= 2 && !cancellationToken.IsCancellationRequested)
						{
							BaseModule<Characters, MainWindow, Settings>.Logger.Debug("We should be in the character selection but the OCR did only read '" + txt + "'. Waiting a bit longer!");
							await Delay(cancellationToken, 250);
							txt = await OCR.Read();
							if (cancellationToken.IsCancellationRequested)
							{
								return _gameState.IsCharacterSelection;
							}
						}
					}
				}
				stopwatch.Stop();
			}
			return !GameService.GameIntegration.get_Gw2Instance().get_IsInGame();
		}

		private async Task MoveToFirstCharacter(CancellationToken cancellationToken)
		{
			Status = strings.CharacterSwap_MoveFirst;
			if (IsTaskCanceled(cancellationToken))
			{
				return;
			}
			Stopwatch stopwatch = Stopwatch.StartNew();
			int moves = _characterModels.Count - _movedLeft;
			for (int i = 0; i < moves; i++)
			{
				if (stopwatch.ElapsedMilliseconds > 5000)
				{
					ExtendedInputService.MouseWiggle();
					stopwatch.Restart();
				}
				Keyboard.Stroke((VirtualKeyShort)37, false);
				_movedLeft++;
				await Delay(cancellationToken, null, 0.05);
				if (IsTaskCanceled(cancellationToken))
				{
					break;
				}
			}
		}

		private async Task MoveToCharacter(CancellationToken cancellationToken)
		{
			Status = string.Format(strings.CharacterSwap_MoveTo, Character.Name);
			if (IsTaskCanceled(cancellationToken))
			{
				return;
			}
			List<Character_Model> order = _characterModels.OrderByDescending((Character_Model e) => e.LastLogin).ToList();
			Stopwatch stopwatch = Stopwatch.StartNew();
			using List<Character_Model>.Enumerator enumerator = order.GetEnumerator();
			while (enumerator.MoveNext() && enumerator.Current != Character)
			{
				if (stopwatch.ElapsedMilliseconds > 5000)
				{
					ExtendedInputService.MouseWiggle();
					stopwatch.Restart();
				}
				Keyboard.Stroke((VirtualKeyShort)39, false);
				await Delay(cancellationToken);
				if (IsTaskCanceled(cancellationToken))
				{
					return;
				}
			}
		}

		private async Task<bool> ConfirmName()
		{
			if (!_settings.UseOCR.get_Value() || _ignoreOCR)
			{
				return true;
			}
			if (Character == null || string.IsNullOrEmpty(Character.Name))
			{
				return false;
			}
			string text = ((!_settings.UseOCR.get_Value()) ? "No OCR" : (await OCR.Read()));
			string ocr_result = text;
			(string, int, int, int, bool) isBestMatch = ("No OCR enabled.", 0, 0, 0, false);
			if (_settings.UseOCR.get_Value())
			{
				Status = "Confirm name ..." + Environment.NewLine + ocr_result;
				BaseModule<Characters, MainWindow, Settings>.Logger.Info("OCR Result: " + ocr_result + ".");
				if (_settings.OnlyEnterOnExact.get_Value())
				{
					return Character.Name == ocr_result;
				}
				isBestMatch = Character.NameMatches(ocr_result);
				BaseModule<Characters, MainWindow, Settings>.Logger.Info($"Swapping to {Character.Name} - Best result for : '{ocr_result}' is '{isBestMatch.Item1}' with edit distance of: {isBestMatch.Item2} and which is {isBestMatch.Item3} steps away in the character list. Resulting in a total difference of {isBestMatch.Item4}.");
				return isBestMatch.Item5;
			}
			return isBestMatch.Item5;
		}

		private async Task<bool> Login(CancellationToken cancellationToken)
		{
			if (IsTaskCanceled(cancellationToken))
			{
				return false;
			}
			if (_settings.EnterOnSwap.get_Value())
			{
				Status = string.Format(strings.CharacterSwap_LoginTo, Character.Name);
				Keyboard.Stroke((VirtualKeyShort)13, false);
				await Delay(cancellationToken);
				if (_settings.UseBetaGamestate.get_Value())
				{
					while (!_gameState.IsIngame && !cancellationToken.IsCancellationRequested)
					{
						await Delay(cancellationToken, 500);
						if (cancellationToken.IsCancellationRequested)
						{
							return false;
						}
					}
				}
				else
				{
					while (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && !cancellationToken.IsCancellationRequested)
					{
						await Delay(cancellationToken, 500);
						if (cancellationToken.IsCancellationRequested)
						{
							return false;
						}
					}
				}
			}
			if (_settings.OpenInventoryOnEnter.get_Value())
			{
				await _settings.InventoryKey.get_Value().PerformPress(_settings.KeyDelay.get_Value(), triggerSystem: false);
			}
			PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
			if (player != null && player.get_Name() == Character.Name)
			{
				Character.UpdateCharacter(player);
				this.Succeeded?.Invoke(null, null);
			}
			return true;
		}

		private bool IsLoaded()
		{
			if (_settings.EnterOnSwap.get_Value())
			{
				return GameService.GameIntegration.get_Gw2Instance().get_IsInGame();
			}
			return true;
		}
	}
}
