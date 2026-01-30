using System;
using System.Collections.Generic;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Input;

namespace Maestro.Settings
{
	public class ModuleSettings
	{
		public SettingEntry<KeyBinding> NoteC { get; private set; }

		public SettingEntry<KeyBinding> NoteD { get; private set; }

		public SettingEntry<KeyBinding> NoteE { get; private set; }

		public SettingEntry<KeyBinding> NoteF { get; private set; }

		public SettingEntry<KeyBinding> NoteG { get; private set; }

		public SettingEntry<KeyBinding> NoteA { get; private set; }

		public SettingEntry<KeyBinding> NoteB { get; private set; }

		public SettingEntry<KeyBinding> NoteCHigh { get; private set; }

		public SettingEntry<KeyBinding> OctaveUp { get; private set; }

		public SettingEntry<KeyBinding> OctaveDown { get; private set; }

		public SettingEntry<KeyBinding> SharpC { get; private set; }

		public SettingEntry<KeyBinding> SharpD { get; private set; }

		public SettingEntry<KeyBinding> SharpF { get; private set; }

		public SettingEntry<KeyBinding> SharpG { get; private set; }

		public SettingEntry<KeyBinding> SharpA { get; private set; }

		public string ClientId { get; private set; }

		public ModuleSettings(SettingCollection settings)
		{
			DefineInstrumentKeys(settings);
			DefinePianoSharps(settings);
			DefineClientId(settings);
		}

		private void DefineInstrumentKeys(SettingCollection settings)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected O, but got Unknown
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Expected O, but got Unknown
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Expected O, but got Unknown
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Expected O, but got Unknown
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Expected O, but got Unknown
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Expected O, but got Unknown
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Expected O, but got Unknown
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Expected O, but got Unknown
			SettingCollection instrumentKeys = settings.AddSubCollection("InstrumentKeys", true, (Func<string>)(() => "Instrument Keys"));
			NoteC = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteC", new KeyBinding((Keys)97), (Func<string>)(() => "Note C"), (Func<string>)(() => "Match to Weapon Skill 1"));
			NoteD = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteD", new KeyBinding((Keys)98), (Func<string>)(() => "Note D"), (Func<string>)(() => "Match to Weapon Skill 2"));
			NoteE = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteE", new KeyBinding((Keys)99), (Func<string>)(() => "Note E"), (Func<string>)(() => "Match to Weapon Skill 3"));
			NoteF = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteF", new KeyBinding((Keys)100), (Func<string>)(() => "Note F"), (Func<string>)(() => "Match to Weapon Skill 4"));
			NoteG = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteG", new KeyBinding((Keys)101), (Func<string>)(() => "Note G"), (Func<string>)(() => "Match to Weapon Skill 5"));
			NoteA = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteA", new KeyBinding((Keys)102), (Func<string>)(() => "Note A"), (Func<string>)(() => "Match to Healing Skill"));
			NoteB = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteB", new KeyBinding((Keys)103), (Func<string>)(() => "Note B"), (Func<string>)(() => "Match to Utility Skill 1"));
			NoteCHigh = instrumentKeys.DefineSetting<KeyBinding>("KeyNoteCHigh", new KeyBinding((Keys)104), (Func<string>)(() => "Note C High"), (Func<string>)(() => "Match to Utility Skill 2"));
			OctaveDown = instrumentKeys.DefineSetting<KeyBinding>("KeyOctaveDown", new KeyBinding((Keys)96), (Func<string>)(() => "Octave Down"), (Func<string>)(() => "Match to Utility Skill 3"));
			OctaveUp = instrumentKeys.DefineSetting<KeyBinding>("KeyOctaveUp", new KeyBinding((Keys)105), (Func<string>)(() => "Octave Up"), (Func<string>)(() => "Match to Elite Skill"));
		}

		private void DefinePianoSharps(SettingCollection settings)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Expected O, but got Unknown
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Expected O, but got Unknown
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Expected O, but got Unknown
			SettingCollection pianoSharps = settings.AddSubCollection("PianoSharps", true, (Func<string>)(() => "Piano Only - Sharp Notes - Keys must not conflict with natural note keybinds"));
			SharpC = pianoSharps.DefineSetting<KeyBinding>("KeySharpC", new KeyBinding((ModifierKeys)2, (Keys)49), (Func<string>)(() => "Sharp C#"), (Func<string>)(() => "Match to Profession Skill 1"));
			SharpD = pianoSharps.DefineSetting<KeyBinding>("KeySharpD", new KeyBinding((ModifierKeys)2, (Keys)50), (Func<string>)(() => "Sharp D#"), (Func<string>)(() => "Match to Profession Skill 2"));
			SharpF = pianoSharps.DefineSetting<KeyBinding>("KeySharpF", new KeyBinding((ModifierKeys)2, (Keys)51), (Func<string>)(() => "Sharp F#"), (Func<string>)(() => "Match to Profession Skill 3"));
			SharpG = pianoSharps.DefineSetting<KeyBinding>("KeySharpG", new KeyBinding((ModifierKeys)2, (Keys)52), (Func<string>)(() => "Sharp G#"), (Func<string>)(() => "Match to Profession Skill 4"));
			SharpA = pianoSharps.DefineSetting<KeyBinding>("KeySharpA", new KeyBinding((ModifierKeys)2, (Keys)53), (Func<string>)(() => "Sharp A#"), (Func<string>)(() => "Match to Profession Skill 5"));
		}

		private void DefineClientId(SettingCollection settings)
		{
			SettingEntry<string> clientIdSetting = settings.AddSubCollection("Internal", false).DefineSetting<string>("ClientId", "", (Func<string>)null, (Func<string>)null);
			if (string.IsNullOrEmpty(clientIdSetting.get_Value()))
			{
				clientIdSetting.set_Value(Guid.NewGuid().ToString());
			}
			ClientId = clientIdSetting.get_Value();
		}

		public Dictionary<Keys, SettingEntry<KeyBinding>> GetKeyMappings()
		{
			return new Dictionary<Keys, SettingEntry<KeyBinding>>
			{
				{
					(Keys)97,
					NoteC
				},
				{
					(Keys)98,
					NoteD
				},
				{
					(Keys)99,
					NoteE
				},
				{
					(Keys)100,
					NoteF
				},
				{
					(Keys)101,
					NoteG
				},
				{
					(Keys)102,
					NoteA
				},
				{
					(Keys)103,
					NoteB
				},
				{
					(Keys)104,
					NoteCHigh
				},
				{
					(Keys)105,
					OctaveUp
				},
				{
					(Keys)96,
					OctaveDown
				}
			};
		}

		public Dictionary<Keys, SettingEntry<KeyBinding>> GetSharpMappings()
		{
			return new Dictionary<Keys, SettingEntry<KeyBinding>>
			{
				{
					(Keys)97,
					SharpC
				},
				{
					(Keys)98,
					SharpD
				},
				{
					(Keys)99,
					SharpF
				},
				{
					(Keys)100,
					SharpG
				},
				{
					(Keys)101,
					SharpA
				}
			};
		}
	}
}
