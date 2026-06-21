using System;
using System.Collections.Generic;
using DavidRice.BlishHud.MidiControl.Input;

namespace DavidRice.BlishHud.MidiControl.Core
{
	public class KeySender
	{
		private readonly KeySendThread _keySendThread;

		private readonly Dictionary<uint, int> _heldKeys = new Dictionary<uint, int>();

		private readonly Dictionary<int, uint> _noteToScanCode = new Dictionary<int, uint>();

		private int _currentOctave;

		public int CurrentOctave => _currentOctave;

		public event Action<MidiNoteEvent, KeySendResult>? NoteProcessed;

		public KeySender(KeySendThread keySendThread)
		{
			_keySendThread = keySendThread ?? throw new ArgumentNullException("keySendThread");
		}

		public void Send(MidiNoteEvent noteEvent, Keymap keymap, bool autoSwap, int shiftDelayMs, bool enableKeyHold = false)
		{
			if (enableKeyHold && !noteEvent.IsNoteOn)
			{
				ProcessKeyHoldNoteOff(noteEvent, keymap);
				return;
			}
			KeySendResult candidate = Resolve(noteEvent, keymap, _currentOctave, autoSwap, shiftDelayMs, enableKeyHold);
			if (enableKeyHold && candidate.Actions.Length != 0)
			{
				int noteActionIndex = candidate.Actions.Length - 1;
				_noteToScanCode[noteEvent.NoteNumber] = candidate.Actions[noteActionIndex].ScanCode;
			}
			List<SendAction> actualActions = new List<SendAction>(candidate.Actions.Length);
			List<string> actualSentKeyNames = new List<string>(candidate.SentKeyNames.Length);
			bool wasSuppressed = false;
			for (int i = 0; i < candidate.Actions.Length; i++)
			{
				SendAction action = candidate.Actions[i];
				string keyName = candidate.SentKeyNames[i];
				switch (action.EventType)
				{
				case KeyEventType.KeyTap:
					actualActions.Add(action);
					actualSentKeyNames.Add(keyName);
					break;
				case KeyEventType.KeyDown:
				{
					_heldKeys.TryGetValue(action.ScanCode, out var previousCount);
					_heldKeys[action.ScanCode] = previousCount + 1;
					if (previousCount == 0)
					{
						actualActions.Add(action);
						actualSentKeyNames.Add(keyName);
					}
					else
					{
						wasSuppressed = true;
					}
					break;
				}
				case KeyEventType.KeyUp:
				{
					if (TryReleaseHeldScanCode(action.ScanCode, out var released))
					{
						actualActions.Add(action);
						actualSentKeyNames.Add(keyName);
					}
					else if (!released)
					{
						actualActions.Add(action);
						actualSentKeyNames.Add(keyName);
					}
					else
					{
						wasSuppressed = true;
					}
					break;
				}
				}
			}
			KeySendResult result = new KeySendResult(actualActions.ToArray(), candidate.NewOctave, candidate.PreviousOctave, actualSentKeyNames.ToArray(), wasSuppressed);
			SendAction[] actions = result.Actions;
			foreach (SendAction action2 in actions)
			{
				_keySendThread.Enqueue(action2);
			}
			this.NoteProcessed?.Invoke(noteEvent, result);
			_currentOctave = candidate.NewOctave;
		}

		public void ReleaseAllHeldKeys()
		{
			foreach (KeyValuePair<uint, int> kvp in _heldKeys)
			{
				_keySendThread.Enqueue(new SendAction(kvp.Key, 0, KeyEventType.KeyUp));
			}
			_heldKeys.Clear();
			_noteToScanCode.Clear();
		}

		private void ProcessKeyHoldNoteOff(MidiNoteEvent noteEvent, Keymap keymap)
		{
			string keyName;
			if (_noteToScanCode.TryGetValue(noteEvent.NoteNumber, out var scanCode))
			{
				keyName = KeyToScanCode.GetKeyName(scanCode) ?? "?";
			}
			else
			{
				KeySendResult fallback = Resolve(noteEvent, keymap, _currentOctave, autoSwap: false, 0, enableKeyHold: true);
				if (fallback.Actions.Length == 0)
				{
					this.NoteProcessed?.Invoke(noteEvent, new KeySendResult(Array.Empty<SendAction>(), _currentOctave, _currentOctave, Array.Empty<string>()));
					return;
				}
				scanCode = fallback.Actions[0].ScanCode;
				keyName = fallback.SentKeyNames[0];
			}
			List<SendAction> actualActions = new List<SendAction>();
			List<string> actualSentKeyNames = new List<string>();
			bool wasSuppressed = false;
			if (TryReleaseHeldScanCode(scanCode, out var released))
			{
				actualActions.Add(new SendAction(scanCode, 0, KeyEventType.KeyUp));
				actualSentKeyNames.Add(keyName);
			}
			else if (!released)
			{
				actualActions.Add(new SendAction(scanCode, 0, KeyEventType.KeyUp));
				actualSentKeyNames.Add(keyName);
			}
			else
			{
				wasSuppressed = true;
			}
			if (_heldKeys.Count == 0 || !_heldKeys.ContainsKey(scanCode))
			{
				_noteToScanCode.Remove(noteEvent.NoteNumber);
			}
			KeySendResult result = new KeySendResult(actualActions.ToArray(), _currentOctave, _currentOctave, actualSentKeyNames.ToArray(), wasSuppressed);
			SendAction[] actions = result.Actions;
			foreach (SendAction action in actions)
			{
				_keySendThread.Enqueue(action);
			}
			this.NoteProcessed?.Invoke(noteEvent, result);
		}

		private bool TryReleaseHeldScanCode(uint scanCode, out bool released)
		{
			if (_heldKeys.TryGetValue(scanCode, out var heldCount))
			{
				released = true;
				int newCount = heldCount - 1;
				if (newCount == 0)
				{
					_heldKeys.Remove(scanCode);
					return true;
				}
				_heldKeys[scanCode] = newCount;
				return false;
			}
			released = false;
			return false;
		}

		public static KeySendResult Resolve(MidiNoteEvent noteEvent, Keymap keymap, int currentOctave, bool autoSwap, int shiftDelayMs, bool enableKeyHold = false)
		{
			KeyEventType eventType;
			if (enableKeyHold)
			{
				eventType = (noteEvent.IsNoteOn ? KeyEventType.KeyDown : KeyEventType.KeyUp);
			}
			else
			{
				if (!noteEvent.IsNoteOn)
				{
					return new KeySendResult(Array.Empty<SendAction>(), currentOctave, currentOctave, Array.Empty<string>());
				}
				eventType = KeyEventType.KeyTap;
			}
			string noteName = MidiNote.GetNoteName(noteEvent.NoteNumber);
			if (!keymap.Notes.TryGetValue(noteName, out var definition))
			{
				return new KeySendResult(Array.Empty<SendAction>(), currentOctave, currentOctave, Array.Empty<string>());
			}
			if (definition.ForceInternalOctave.HasValue)
			{
				return new KeySendResult(Array.Empty<SendAction>(), definition.ForceInternalOctave.Value, currentOctave, Array.Empty<string>());
			}
			if (definition.Key == null)
			{
				return new KeySendResult(Array.Empty<SendAction>(), currentOctave, currentOctave, Array.Empty<string>());
			}
			string noteKey = definition.Key;
			List<SendAction> actions = new List<SendAction>();
			int newOctave = currentOctave;
			if (!definition.Octave.HasValue)
			{
				uint? sc = KeyToScanCode.For(noteKey);
				List<string> specialKeyNames = new List<string>();
				if (sc.HasValue)
				{
					actions.Add(new SendAction(sc.Value, 0, eventType));
					string keyName = KeyToScanCode.GetKeyName(sc.Value) ?? noteKey;
					specialKeyNames.Add(keyName);
				}
				if (noteEvent.IsNoteOn)
				{
					if (keymap.OctaveDownKey != null && noteKey.Equals(keymap.OctaveDownKey, StringComparison.OrdinalIgnoreCase))
					{
						newOctave = Math.Max(0, currentOctave - 1);
					}
					else if (keymap.OctaveUpKey != null && noteKey.Equals(keymap.OctaveUpKey, StringComparison.OrdinalIgnoreCase))
					{
						newOctave = currentOctave + 1;
					}
				}
				return new KeySendResult(actions.ToArray(), newOctave, currentOctave, specialKeyNames.ToArray());
			}
			int targetOctave = definition.Octave.Value;
			string targetKey = definition.Key;
			List<string> shiftKeyNames = new List<string>();
			if (autoSwap && currentOctave != targetOctave)
			{
				if (definition.AltOctave.HasValue && definition.AltOctave.Value == currentOctave && definition.AltOctaveKey != null)
				{
					targetOctave = currentOctave;
					targetKey = definition.AltOctaveKey;
				}
				else
				{
					int octavesToShift = targetOctave - currentOctave;
					if (!TryBuildOctaveShiftActions(keymap, octavesToShift, shiftDelayMs, actions, shiftKeyNames))
					{
						return new KeySendResult(Array.Empty<SendAction>(), currentOctave, currentOctave, Array.Empty<string>());
					}
					newOctave = targetOctave;
				}
			}
			uint? noteSc = KeyToScanCode.For(targetKey);
			if (noteSc.HasValue)
			{
				actions.Add(new SendAction(noteSc.Value, 0, eventType));
				List<string> allKeyNames = new List<string>(shiftKeyNames.Count);
				allKeyNames.AddRange(shiftKeyNames);
				string noteKeyName = KeyToScanCode.GetKeyName(noteSc.Value) ?? targetKey;
				allKeyNames.Add(noteKeyName);
				return new KeySendResult(actions.ToArray(), newOctave, currentOctave, allKeyNames.ToArray());
			}
			return new KeySendResult(Array.Empty<SendAction>(), currentOctave, currentOctave, Array.Empty<string>());
		}

		private static bool TryBuildOctaveShiftActions(Keymap keymap, int octavesToShift, int shiftDelayMs, List<SendAction> actions, List<string> keyNames)
		{
			if (octavesToShift > 0)
			{
				string upKey = keymap.OctaveUpKey;
				if (string.IsNullOrEmpty(upKey))
				{
					return false;
				}
				uint? sc2 = KeyToScanCode.For(upKey);
				if (!sc2.HasValue)
				{
					return false;
				}
				string keyName2 = KeyToScanCode.GetKeyName(sc2.Value);
				AppendShiftActions(sc2.Value, octavesToShift, shiftDelayMs, actions, keyName2);
				if (keyName2 != null)
				{
					for (int j = 0; j < octavesToShift; j++)
					{
						keyNames.Add(keyName2);
					}
				}
				return true;
			}
			if (octavesToShift < 0)
			{
				string downKey = keymap.OctaveDownKey;
				if (string.IsNullOrEmpty(downKey))
				{
					return false;
				}
				uint? sc = KeyToScanCode.For(downKey);
				if (!sc.HasValue)
				{
					return false;
				}
				string keyName = KeyToScanCode.GetKeyName(sc.Value);
				AppendShiftActions(sc.Value, Math.Abs(octavesToShift), shiftDelayMs, actions, keyName);
				if (keyName != null)
				{
					for (int i = 0; i < Math.Abs(octavesToShift); i++)
					{
						keyNames.Add(keyName);
					}
				}
				return true;
			}
			return true;
		}

		private static void AppendShiftActions(uint scanCode, int shiftCount, int shiftDelayMs, List<SendAction> actions, string? keyName)
		{
			for (int i = 0; i < shiftCount; i++)
			{
				int delay = ((i != shiftCount - 1) ? shiftDelayMs : 0);
				actions.Add(new SendAction(scanCode, delay));
			}
		}
	}
}
