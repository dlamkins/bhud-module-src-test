using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class Racer : IDisposable
	{
		public enum Mode
		{
			Racing,
			Edit,
			Online
		}

		private FullRace? _fullRace;

		private FullGhost? _fullGhost;

		private bool _specificGhostLoaded;

		private RaceHolder _currentMode;

		private readonly CommandList _commands;

		public FullRace? FullRace
		{
			get
			{
				return _fullRace;
			}
			set
			{
				FullGhost = null;
				_currentMode.FullRace = (_fullRace = value);
			}
		}

		public FullGhost? FullGhost
		{
			get
			{
				return _fullGhost;
			}
			set
			{
				if (value != null)
				{
					string ghostRaceId = value!.Ghost?.RaceId;
					if (ghostRaceId == null || FullRace?.Meta?.Id != ghostRaceId)
					{
						return;
					}
				}
				_fullGhost = value;
				RaceRunner runner = _currentMode as RaceRunner;
				if (runner != null)
				{
					runner.FullGhost = value;
				}
			}
		}

		public bool SpecificGhostLoaded
		{
			get
			{
				return _specificGhostLoaded;
			}
			set
			{
				_specificGhostLoaded = value;
				RaceRunner runner = _currentMode as RaceRunner;
				if (runner != null)
				{
					runner.SpecificGhostLoaded = value;
				}
			}
		}

		public Mode CurrentMode
		{
			get
			{
				RaceHolder currentMode = _currentMode;
				if (!(currentMode is RaceEditor))
				{
					if (currentMode is RaceOnline)
					{
						return Mode.Online;
					}
					return Mode.Racing;
				}
				return Mode.Edit;
			}
			set
			{
				SwitchMode(value);
			}
		}

		public event Action<FullRace>? RaceLoaded;

		public event Action<FullGhost>? GhostLoaded;

		public event Action? LocalRacesChanged;

		public event Action<Ghost>? GhostSaved;

		public Racer()
		{
			_commands = new CommandList(new EditState());
			_currentMode = new RaceRunner();
			RacingModule.Server.RemoteRacesChanged += RemoteRacesChanged;
		}

		public void Run(int testCheckpoint)
		{
			SwitchMode(Mode.Racing, testCheckpoint);
		}

		private void SwitchMode(Mode edit, int testCheckpoint = -1)
		{
			((Control)_currentMode).Dispose();
			_currentMode = edit switch
			{
				Mode.Edit => CreateEditor(), 
				Mode.Online => CreateOnline(), 
				_ => CreateRunner(testCheckpoint), 
			};
		}

		private RaceRunner CreateRunner(int testCheckpoint)
		{
			RaceRunner raceRunner = new RaceRunner(testCheckpoint);
			((Control)raceRunner).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			raceRunner.FullRace = FullRace;
			raceRunner.FullGhost = FullGhost;
			raceRunner.SpecificGhostLoaded = SpecificGhostLoaded;
			raceRunner.RaceLoaded += delegate(FullRace race)
			{
				this.RaceLoaded?.Invoke(race);
			};
			raceRunner.GhostLoaded += delegate(FullGhost ghost)
			{
				this.GhostLoaded?.Invoke(ghost);
			};
			raceRunner.GhostSaved += delegate(Ghost ghost)
			{
				this.GhostSaved?.Invoke(ghost);
			};
			return raceRunner;
		}

		private RaceEditor CreateEditor()
		{
			if (FullRace == null)
			{
				RacingModule.Racer.FullRace = DataExtensions.NewRace();
			}
			else if (!FullRace!.IsLocal)
			{
				RacingModule.Racer.FullRace = FullRace!.CloneNew(StringExtensions.Format(Strings.CopyOf, FullRace!.Race.Name));
			}
			RaceEditor raceEditor = new RaceEditor(_commands);
			((Control)raceEditor).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			raceEditor.FullRace = FullRace;
			raceEditor.RaceLoaded += delegate(FullRace race)
			{
				this.RaceLoaded?.Invoke(race);
			};
			raceEditor.LocalRacesChanged += delegate
			{
				this.LocalRacesChanged?.Invoke();
			};
			return raceEditor;
		}

		private RaceOnline CreateOnline()
		{
			FullRace = null;
			FullGhost = null;
			RaceOnline raceOnline = new RaceOnline();
			((Control)raceOnline).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			return raceOnline;
		}

		private void RemoteRacesChanged(RemoteRaces _)
		{
			FullRace = null;
		}

		public void Dispose()
		{
			RacingModule.Server.RemoteRacesChanged -= RemoteRacesChanged;
			((Control)_currentMode).Dispose();
		}
	}
}