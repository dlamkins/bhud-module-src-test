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

		public ModuleSettings(SettingCollection settings)
		{
			DefineInstrumentKeys(settings);
			DefinePianoSharps(settings);
		}

		private void DefineInstrumentKeys(SettingCollection settings)
		{
			SettingCollection instrumentKeys = settings.AddSubCollection("InstrumentKeys", renderInUi: true, () => "Instrument Keys");
			NoteC = instrumentKeys.DefineSetting("KeyNoteC", new KeyBinding((Keys)97), () => "Note C", () => "Match to Weapon Skill 1");
			NoteD = instrumentKeys.DefineSetting("KeyNoteD", new KeyBinding((Keys)98), () => "Note D", () => "Match to Weapon Skill 2");
			NoteE = instrumentKeys.DefineSetting("KeyNoteE", new KeyBinding((Keys)99), () => "Note E", () => "Match to Weapon Skill 3");
			NoteF = instrumentKeys.DefineSetting("KeyNoteF", new KeyBinding((Keys)100), () => "Note F", () => "Match to Weapon Skill 4");
			NoteG = instrumentKeys.DefineSetting("KeyNoteG", new KeyBinding((Keys)101), () => "Note G", () => "Match to Weapon Skill 5");
			NoteA = instrumentKeys.DefineSetting("KeyNoteA", new KeyBinding((Keys)102), () => "Note A", () => "Match to Healing Skill");
			NoteB = instrumentKeys.DefineSetting("KeyNoteB", new KeyBinding((Keys)103), () => "Note B", () => "Match to Utility Skill 1");
			NoteCHigh = instrumentKeys.DefineSetting("KeyNoteCHigh", new KeyBinding((Keys)104), () => "Note C High", () => "Match to Utility Skill 2");
			OctaveDown = instrumentKeys.DefineSetting("KeyOctaveDown", new KeyBinding((Keys)96), () => "Octave Down", () => "Match to Utility Skill 3");
			OctaveUp = instrumentKeys.DefineSetting("KeyOctaveUp", new KeyBinding((Keys)105), () => "Octave Up", () => "Match to Elite Skill");
		}

		private void DefinePianoSharps(SettingCollection settings)
		{
			SettingCollection pianoSharps = settings.AddSubCollection("PianoSharps", renderInUi: true, () => "Piano Only - Sharp Notes (avoid modifier + any instrument key)");
			SharpC = pianoSharps.DefineSetting("KeySharpC", new KeyBinding(ModifierKeys.Alt, (Keys)49), () => "Sharp C#", () => "Match to Profession Skill 1");
			SharpD = pianoSharps.DefineSetting("KeySharpD", new KeyBinding(ModifierKeys.Alt, (Keys)50), () => "Sharp D#", () => "Match to Profession Skill 2");
			SharpF = pianoSharps.DefineSetting("KeySharpF", new KeyBinding(ModifierKeys.Alt, (Keys)51), () => "Sharp F#", () => "Match to Profession Skill 3");
			SharpG = pianoSharps.DefineSetting("KeySharpG", new KeyBinding(ModifierKeys.Alt, (Keys)52), () => "Sharp G#", () => "Match to Profession Skill 4");
			SharpA = pianoSharps.DefineSetting("KeySharpA", new KeyBinding(ModifierKeys.Alt, (Keys)53), () => "Sharp A#", () => "Match to Profession Skill 5");
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
