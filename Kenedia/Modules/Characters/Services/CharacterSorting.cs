using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Characters.Services
{
	public class CharacterSorting
	{
		private string _status;

		private readonly Settings _settings;

		private readonly GameState _gameState;

		private readonly ObservableCollection<Character_Model> _characters;

		private CancellationTokenSource _cancellationTokenSource;

		private List<Character_Model> _models;

		private SortingState _state;

		private string _lastName;

		private int _noNameChange;

		private int _currentIndex;

		private int NoNameChange
		{
			get
			{
				return _noNameChange;
			}
			set
			{
				_noNameChange = value;
				if (_noNameChange > 0)
				{
					Status = string.Format(strings.FixCharacter_NoChange, _noNameChange);
				}
				if (_noNameChange >= 2)
				{
					_state = SortingState.Done;
					Status = strings.Status_Done;
					AdjustCharacterLogins();
					this.Completed?.Invoke(null, null);
				}
			}
		}

		public OCR OCR { get; set; }

		public CharacterSwapping CharacterSwapping { get; set; }

		public Action UpdateCharacterList { get; set; }

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

		public event EventHandler Started;

		public event EventHandler Completed;

		public event EventHandler Finished;

		public event EventHandler StatusChanged;

		public CharacterSorting(Settings settings, GameState gameState, ObservableCollection<Character_Model> characters)
		{
			_settings = settings;
			_gameState = gameState;
			_characters = characters;
		}

		public bool Cancel()
		{
			bool result = _cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested;
			_state = SortingState.Canceled;
			CancellationTokenSource cancellationTokenSource = _cancellationTokenSource;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
				return result;
			}
			return result;
		}

		public async void Start()
		{
			_state = SortingState.Canceled;
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource = new CancellationTokenSource();
			_cancellationTokenSource.CancelAfter(180000);
			_models = _characters.OrderByDescending((Character_Model e) => e.LastLogin).ToList();
			_lastName = string.Empty;
			_state = SortingState.None;
			NoNameChange = 0;
			this.Started?.Invoke(null, null);
			Status = strings.FixCharacter_Start;
			int i = 0;
			while (true)
			{
				SortingState state = _state;
				if (state == SortingState.Done || state == SortingState.Canceled || _cancellationTokenSource.Token.IsCancellationRequested)
				{
					break;
				}
				i++;
				try
				{
					await Run(_cancellationTokenSource.Token);
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

		public async Task Run(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}
			switch (_state)
			{
			case SortingState.None:
				await MoveFirst(cancellationToken);
				await Delay(cancellationToken, 125);
				break;
			case SortingState.MovedToFirst:
			{
				string name = await FetchName(cancellationToken);
				if (name == _lastName)
				{
					NoNameChange++;
				}
				else
				{
					NoNameChange = 0;
				}
				_lastName = name;
				break;
			}
			case SortingState.FirstNameFetched:
			{
				await MoveNext(cancellationToken);
				string name = await FetchName(cancellationToken);
				if (name == _lastName)
				{
					NoNameChange++;
				}
				else
				{
					NoNameChange = 0;
				}
				_lastName = name;
				await Delay(cancellationToken, 250);
				break;
			}
			case SortingState.Selected:
				break;
			}
		}

		private async Task MoveFirst(CancellationToken cancellationToken)
		{
			Status = strings.CharacterSwap_MoveFirst;
			if (IsTaskCanceled(cancellationToken))
			{
				return;
			}
			Stopwatch stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < _models.Count; i++)
			{
				Keyboard.Stroke((VirtualKeyShort)37, false);
				await Delay(cancellationToken, null, 0.05);
				if (IsTaskCanceled(cancellationToken))
				{
					return;
				}
				if (stopwatch.ElapsedMilliseconds > 5000)
				{
					ExtendedInputService.MouseWiggle();
					stopwatch.Restart();
				}
			}
			_state = SortingState.MovedToFirst;
			_currentIndex = 0;
		}

		private async Task MoveNext(CancellationToken cancellationToken)
		{
			Status = strings.FixCharacter_MoveNext;
			ExtendedInputService.MouseWiggle();
			Keyboard.Stroke((VirtualKeyShort)39, false);
			await Delay(cancellationToken);
			_currentIndex++;
		}

		private (string, int, int, int, bool) GetBestMatch(string name)
		{
			List<(string, int, int, int, bool)> distances = new List<(string, int, int, int, bool)>();
			Character_Model expectedCharacter = _models.Find((Character_Model e) => e.OrderIndex == _currentIndex);
			string expectedCharacterName = ((expectedCharacter == null) ? name : expectedCharacter.Name);
			foreach (Character_Model c in _models)
			{
				int distance = name.LevenshteinDistance(c.Name);
				int listDistance = 0;
				distances.Add((c.Name, distance, listDistance, listDistance + distance, expectedCharacter != null && c.Name == expectedCharacter?.Name));
			}
			distances.Sort(((string, int, int, int, bool) a, (string, int, int, int, bool) b) => a.Item4.CompareTo(b.Item4));
			(string, int, int, int, bool)? bestMatch = distances?.FirstOrDefault();
			if (bestMatch.HasValue && bestMatch.HasValue)
			{
				foreach (var match in distances.Where(((string, int, int, int, bool) e) => e.Item4 == bestMatch.Value.Item4))
				{
					if (match.Item1 == expectedCharacterName)
					{
						return match;
					}
				}
			}
			return bestMatch.Value;
		}

		private async Task<string> FetchName(CancellationToken cancellationToken)
		{
			string name = await (OCR?.Read());
			Status = string.Format(strings.FixCharacter_FetchName, Environment.NewLine, name);
			if (name != null)
			{
				(string, int, int, int, bool) match = GetBestMatch(name);
				Character_Model c = _models.Find((Character_Model e) => e.Name == match.Item1);
				if (c != null)
				{
					c.OrderIndex = _currentIndex;
				}
				else
				{
					Status = string.Format(strings.CouldNotFindNamedItem, strings.Character, name);
				}
				await Delay(cancellationToken);
			}
			if (_state == SortingState.MovedToFirst)
			{
				_state = SortingState.FirstNameFetched;
			}
			return name;
		}

		private bool IsTaskCanceled(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				_state = SortingState.Canceled;
				return true;
			}
			return false;
		}

		private void AdjustCharacterLogins()
		{
			_models = _models.OrderBy((Character_Model e) => e.OrderIndex).ToList();
			bool messedUp = true;
			while (messedUp)
			{
				messedUp = false;
				for (int i = 0; i < _models.Count; i++)
				{
					Character_Model next = ((_models.Count > i + 1) ? _models[i + 1] : null);
					Character_Model current = _models[i];
					if (next != null && current.LastLogin <= next.LastLogin)
					{
						current.LastLogin = next.LastLogin.AddSeconds(1.0);
						messedUp = true;
					}
				}
			}
			UpdateCharacterList?.Invoke();
		}
	}
}
