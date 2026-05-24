using System;
using System.Collections.Generic;
using DavidRice.BlishHud.MidiControl.Input;

namespace DavidRice.BlishHud.MidiControl.Core
{
	public class KeySender
	{
		private readonly KeySendThread _keySendThread;

		private int _currentOctave;

		public int CurrentOctave => _currentOctave;

		public event Action<MidiNoteEvent, KeySendResult>? NoteProcessed;

		public KeySender(KeySendThread keySendThread)
		{
			_keySendThread = keySendThread ?? throw new ArgumentNullException("keySendThread");
		}

		public void Send(MidiNoteEvent noteEvent, Keymap keymap, bool autoSwap, int shiftDelayMs)
		{
			KeySendResult result = Resolve(noteEvent, keymap, _currentOctave, autoSwap, shiftDelayMs);
			SendAction[] actions = result.Actions;
			foreach (SendAction action in actions)
			{
				_keySendThread.Enqueue(action);
			}
			this.NoteProcessed?.Invoke(noteEvent, result);
			_currentOctave = result.NewOctave;
		}

		public static KeySendResult Resolve(MidiNoteEvent noteEvent, Keymap keymap, int currentOctave, bool autoSwap, int shiftDelayMs)
		{
			if (!noteEvent.IsNoteOn)
			{
				return new KeySendResult(Array.Empty<SendAction>(), currentOctave, currentOctave, Array.Empty<string>());
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
					actions.Add(new SendAction(sc.Value));
					string keyName = KeyToScanCode.GetKeyName(sc.Value) ?? noteKey;
					specialKeyNames.Add(keyName);
				}
				if (keymap.OctaveDownKey != null && noteKey.Equals(keymap.OctaveDownKey, StringComparison.OrdinalIgnoreCase))
				{
					newOctave = Math.Max(0, currentOctave - 1);
				}
				else if (keymap.OctaveUpKey != null && noteKey.Equals(keymap.OctaveUpKey, StringComparison.OrdinalIgnoreCase))
				{
					newOctave = currentOctave + 1;
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
				actions.Add(new SendAction(noteSc.Value));
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
