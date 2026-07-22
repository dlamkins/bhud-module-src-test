using System;
using System.Collections.ObjectModel;
using System.Linq;
using Blish_HUD;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;

namespace Kenedia.Modules.Characters.Services
{
	public class CharacterRoutineService : IDisposable
	{
		public sealed class CharacterRoutineState
		{
			public StateVar<ObservableCollection<CharacterRoutineModel>> CharacterRoutines { get; } = new StateVar<ObservableCollection<CharacterRoutineModel>>();


			public StateVar<CharacterRoutineModel> SelectedRoutine { get; } = new StateVar<CharacterRoutineModel>();


			public StateVar<CharacterRoutineStep> TrackedStep { get; } = new StateVar<CharacterRoutineStep>();


			public StateVar<CharacterRoutineStepSwitchStatus> StepSwitchStatus { get; } = new StateVar<CharacterRoutineStepSwitchStatus>();

		}

		private readonly Action _requestSave;

		private CharacterRoutineStep _trackedStepPendingCompletion;

		private bool _trackedStepSwitchSucceeded;

		public CharacterRoutineModel SelectedRoutine { get; private set; }

		public ObservableCollection<CharacterRoutineModel> CharacterRoutines { get; }

		public CharacterRoutineState State { get; } = new CharacterRoutineState();


		public CharacterSwapping CharacterSwapping { get; }

		public ObservableCollection<Character_Model> CharacterModels { get; }

		public CharacterRoutineService(CharacterSwapping characterSwapping, ObservableCollection<Character_Model> characterModels, ObservableCollection<CharacterRoutineModel> characterRoutines, Action requestSave)
		{
			CharacterSwapping = characterSwapping;
			CharacterModels = characterModels;
			CharacterRoutines = characterRoutines;
			_requestSave = requestSave;
			State.CharacterRoutines.Value = CharacterRoutines;
			CharacterSwapping.Succeeded += new EventHandler(CharacterSwapping_Succeeded);
			CharacterSwapping.Failed += new EventHandler(CharacterSwapping_Failed);
		}

		public bool ApplyScheduledResets()
		{
			bool anyReset = false;
			foreach (CharacterRoutineModel characterRoutine in CharacterRoutines)
			{
				if (characterRoutine.CheckAndApplyScheduledReset())
				{
					anyReset = true;
				}
			}
			if (anyReset)
			{
				if (_trackedStepPendingCompletion != null && GetTrackedStepForSelectedRoutine() == null)
				{
					_trackedStepPendingCompletion = null;
					_trackedStepSwitchSucceeded = false;
					State.TrackedStep.Value = null;
					State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.None;
				}
				_requestSave?.Invoke();
			}
			return anyReset;
		}

		public void CreateNewRoutine()
		{
			CharacterRoutineModel newRoutine = new CharacterRoutineModel(string.Format(strings.CharacterRoutineDefaultName, CharacterRoutines.Count + 1));
			CharacterRoutines.Add(newRoutine);
			_requestSave?.Invoke();
			SelectRoutine(newRoutine);
		}

		public void CopySelectedRoutine()
		{
			if (SelectedRoutine != null)
			{
				string copyName = GetUniqueCopyName(SelectedRoutine.Name);
				CharacterRoutineModel copy = SelectedRoutine.Copy(copyName);
				CharacterRoutines.Add(copy);
				_requestSave?.Invoke();
				SelectRoutine(copy);
			}
		}

		public void DeleteSelectedRoutine()
		{
			if (SelectedRoutine != null)
			{
				CharacterRoutineModel previousRoutine = SelectedRoutine;
				CharacterRoutineStep trackedStepPendingCompletion = _trackedStepPendingCompletion;
				SelectedRoutine = null;
				_trackedStepPendingCompletion = null;
				_trackedStepSwitchSucceeded = false;
				State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.None;
				CharacterRoutines.Remove(previousRoutine);
				_requestSave?.Invoke();
				if (trackedStepPendingCompletion != null)
				{
					State.TrackedStep.Value = null;
				}
				State.SelectedRoutine.Value = null;
			}
		}

		public void SelectRoutine(CharacterRoutineModel characterRoutine)
		{
			if (SelectedRoutine != characterRoutine)
			{
				SelectedRoutine = characterRoutine;
				State.SelectedRoutine.Value = SelectedRoutine;
				State.TrackedStep.Value = GetTrackedStepForSelectedRoutine();
			}
		}

		public void UpdateSelectedRoutineName(string name)
		{
			if (SelectedRoutine != null)
			{
				SelectedRoutine.Name = name;
				_requestSave?.Invoke();
			}
		}

		public void UpdateSelectedRoutineResetFrequency(ResetFrequency frequency)
		{
			if (SelectedRoutine != null)
			{
				SelectedRoutine.ResetFrequency = frequency;
				if (SelectedRoutine.CheckAndApplyScheduledReset())
				{
					ClearTrackedStepIfReset();
				}
				_requestSave?.Invoke();
			}
		}

		public void AddRoutineStep(string characterName, string description)
		{
			SelectedRoutine?.AddRoutineStep(characterName, description);
			_requestSave?.Invoke();
		}

		public void RemoveRoutineStep(CharacterRoutineStep step)
		{
			if (SelectedRoutine != null && step != null && SelectedRoutine.RoutineSteps.Contains(step))
			{
				bool num = _trackedStepPendingCompletion == step;
				if (num)
				{
					_trackedStepPendingCompletion = null;
					_trackedStepSwitchSucceeded = false;
					State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.None;
				}
				SelectedRoutine.RemoveRoutineStep(step);
				_requestSave?.Invoke();
				if (num)
				{
					State.TrackedStep.Value = null;
				}
			}
		}

		public void ReorderRoutineStep(CharacterRoutineStep step, int targetIndex)
		{
			if (SelectedRoutine != null && step != null)
			{
				int currentIndex = SelectedRoutine.RoutineSteps.IndexOf(step);
				if (currentIndex >= 0 && currentIndex != targetIndex)
				{
					int insertAt = Math.Min(targetIndex, SelectedRoutine.RoutineSteps.Count - 1);
					SelectedRoutine.RoutineSteps.Move(currentIndex, insertAt);
					_requestSave?.Invoke();
				}
			}
		}

		public void UpdateRoutineStep(CharacterRoutineStep step, string characterName, string description)
		{
			if (step != null && FindRoutineByStep(step) != null)
			{
				step.CharacterName = characterName?.Trim();
				step.Description = description?.Trim();
				if (_trackedStepPendingCompletion == step)
				{
					_trackedStepSwitchSucceeded = IsCurrentCharacter(step.CharacterName);
					State.StepSwitchStatus.Value = (_trackedStepSwitchSucceeded ? CharacterRoutineStepSwitchStatus.ReadyToComplete : (string.IsNullOrWhiteSpace(step.CharacterName) ? CharacterRoutineStepSwitchStatus.CharacterNotAssigned : CharacterRoutineStepSwitchStatus.None));
				}
				_requestSave?.Invoke();
			}
		}

		public void SetRoutineStepCompletion(CharacterRoutineStep step, bool completed)
		{
			if (step != null && step.IsCompleted != completed)
			{
				step.SetCompleted(completed);
				if (completed && _trackedStepPendingCompletion == step)
				{
					_trackedStepPendingCompletion = null;
					_trackedStepSwitchSucceeded = false;
					State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.None;
					State.TrackedStep.Value = null;
				}
				_requestSave?.Invoke();
			}
		}

		public void SetAllRoutineStepsCompletion(bool completed)
		{
			if (SelectedRoutine == null)
			{
				return;
			}
			bool changedAny = false;
			foreach (CharacterRoutineStep routineStep in SelectedRoutine.RoutineSteps)
			{
				if (routineStep.IsCompleted != completed)
				{
					changedAny = true;
				}
				routineStep.SetCompleted(completed);
			}
			bool trackedStepCleared = false;
			if (completed && _trackedStepPendingCompletion != null && SelectedRoutine.RoutineSteps.Contains(_trackedStepPendingCompletion))
			{
				_trackedStepPendingCompletion = null;
				_trackedStepSwitchSucceeded = false;
				State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.None;
				trackedStepCleared = true;
			}
			if (changedAny || trackedStepCleared)
			{
				_requestSave?.Invoke();
				if (trackedStepCleared)
				{
					State.TrackedStep.Value = null;
				}
			}
		}

		public CharacterRoutineStep GetTrackedStepForSelectedRoutine()
		{
			CharacterRoutineModel routine = SelectedRoutine;
			CharacterRoutineStep trackedStep = _trackedStepPendingCompletion;
			if (trackedStep != null && routine != null)
			{
				if (routine.RoutineSteps.Contains(trackedStep) && trackedStep.Enabled && !trackedStep.IsCompleted)
				{
					return trackedStep;
				}
				return null;
			}
			return null;
		}

		public void RequestSwitchToCharacter(string characterName)
		{
			string characterName2 = characterName;
			Character_Model character = CharacterModels.FirstOrDefault((Character_Model c) => c.Name.Equals(characterName2, StringComparison.OrdinalIgnoreCase));
			if (character != null)
			{
				CharacterSwapping.Start(character);
			}
		}

		public void SwitchToNextIncompleteRoutineStep()
		{
			if (SelectedRoutine == null)
			{
				return;
			}
			bool changedCompletion = false;
			CharacterRoutineStep trackedStep = GetTrackedStepForSelectedRoutine();
			CharacterRoutineStep previousTrackedStep = trackedStep;
			if (trackedStep != null)
			{
				if (!CanCompleteTrackedStep(trackedStep))
				{
					TryStartSwitchForStep(trackedStep);
					return;
				}
				trackedStep.SetCompleted(completed: true);
				changedCompletion = true;
				_trackedStepPendingCompletion = null;
				_trackedStepSwitchSucceeded = false;
				State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.None;
			}
			CharacterRoutineStep nextStep = GetNextIncompleteRoutineStep(SelectedRoutine);
			if (nextStep != null)
			{
				_trackedStepPendingCompletion = nextStep;
				_trackedStepSwitchSucceeded = IsCurrentCharacter(nextStep.CharacterName);
				if (_trackedStepSwitchSucceeded)
				{
					State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.ReadyToComplete;
				}
				else
				{
					TryStartSwitchForStep(nextStep);
				}
			}
			if (previousTrackedStep != _trackedStepPendingCompletion)
			{
				State.TrackedStep.Value = _trackedStepPendingCompletion;
			}
			if (changedCompletion)
			{
				_requestSave?.Invoke();
			}
		}

		public void SetRoutineStepEnabled(CharacterRoutineStep step, bool enabled)
		{
			if (step == null || step.Enabled == enabled)
			{
				return;
			}
			CharacterRoutineModel routine = FindRoutineByStep(step);
			if (routine == null)
			{
				return;
			}
			step.Enabled = enabled;
			bool trackedStepChanged = false;
			if (_trackedStepPendingCompletion == step && !enabled)
			{
				_trackedStepPendingCompletion = null;
				_trackedStepSwitchSucceeded = false;
				State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.None;
				trackedStepChanged = true;
			}
			if (routine == SelectedRoutine)
			{
				CharacterRoutineStep trackedStep = GetTrackedStepForSelectedRoutine();
				if (State.TrackedStep.Value != trackedStep)
				{
					State.TrackedStep.Value = trackedStep;
				}
				else if (trackedStepChanged)
				{
					State.TrackedStep.Value = null;
				}
			}
			_requestSave?.Invoke();
		}

		public CharacterRoutineStep GetNextIncompleteRoutineStep(CharacterRoutineModel characterRoutine)
		{
			return characterRoutine?.RoutineSteps.FirstOrDefault((CharacterRoutineStep step) => step.Enabled && !step.IsCompleted);
		}

		public void Dispose()
		{
			CharacterSwapping.Succeeded -= new EventHandler(CharacterSwapping_Succeeded);
			CharacterSwapping.Failed -= new EventHandler(CharacterSwapping_Failed);
		}

		private CharacterRoutineModel FindRoutineByStep(CharacterRoutineStep step)
		{
			CharacterRoutineStep step2 = step;
			if (step2 != null)
			{
				return CharacterRoutines.FirstOrDefault((CharacterRoutineModel routine) => routine.RoutineSteps.Contains(step2));
			}
			return null;
		}

		private string GetUniqueCopyName(string name)
		{
			string copyName;
			string baseName = (copyName = string.Format(strings.CharacterRoutineCopyName, string.IsNullOrWhiteSpace(name) ? strings.CharacterRoutines : name));
			int copyNumber = 2;
			while (CharacterRoutines.Any((CharacterRoutineModel routine) => string.Equals(routine.Name, copyName, StringComparison.OrdinalIgnoreCase)))
			{
				copyName = $"{baseName} {copyNumber++}";
			}
			return copyName;
		}

		private void CharacterSwapping_Succeeded(object sender, EventArgs e)
		{
			CharacterRoutineStep trackedStep = _trackedStepPendingCompletion;
			if (trackedStep != null && DoesStepMatchCharacter(trackedStep, CharacterSwapping.Character))
			{
				_trackedStepSwitchSucceeded = true;
				State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.ReadyToComplete;
			}
		}

		private void CharacterSwapping_Failed(object sender, EventArgs e)
		{
			CharacterRoutineStep trackedStep = _trackedStepPendingCompletion;
			if (trackedStep != null && DoesStepMatchCharacter(trackedStep, CharacterSwapping.Character))
			{
				_trackedStepSwitchSucceeded = false;
				State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.Failed;
			}
		}

		private bool CanCompleteTrackedStep(CharacterRoutineStep step)
		{
			if (step != null)
			{
				if (!_trackedStepSwitchSucceeded)
				{
					return IsCurrentCharacter(step.CharacterName);
				}
				return true;
			}
			return false;
		}

		private bool TryStartSwitchForStep(CharacterRoutineStep step)
		{
			string characterName = step?.CharacterName?.Trim();
			if (string.IsNullOrEmpty(characterName))
			{
				_trackedStepSwitchSucceeded = false;
				State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.CharacterNotAssigned;
				return false;
			}
			Character_Model character = CharacterModels.FirstOrDefault((Character_Model c) => c.Name.Equals(characterName, StringComparison.OrdinalIgnoreCase));
			if (character == null)
			{
				_trackedStepSwitchSucceeded = false;
				State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.CharacterNotFound;
				return false;
			}
			_trackedStepSwitchSucceeded = IsCurrentCharacter(character.Name);
			if (!_trackedStepSwitchSucceeded)
			{
				State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.Switching;
				CharacterSwapping.Start(character);
			}
			else
			{
				State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.ReadyToComplete;
			}
			return true;
		}

		private void ClearTrackedStepIfReset()
		{
			if (_trackedStepPendingCompletion != null && GetTrackedStepForSelectedRoutine() == null)
			{
				_trackedStepPendingCompletion = null;
				_trackedStepSwitchSucceeded = false;
				State.TrackedStep.Value = null;
				State.StepSwitchStatus.Value = CharacterRoutineStepSwitchStatus.None;
			}
		}

		private bool IsCurrentCharacter(string characterName)
		{
			string currentCharacterName = GameService.Gw2Mumble.PlayerCharacter?.Name;
			if (!string.IsNullOrWhiteSpace(characterName) && !string.IsNullOrWhiteSpace(currentCharacterName) && currentCharacterName.Equals(characterName.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				return GameService.GameIntegration.Gw2Instance.IsInGame;
			}
			return false;
		}

		private static bool DoesStepMatchCharacter(CharacterRoutineStep step, Character_Model character)
		{
			if (step != null && character != null && !string.IsNullOrWhiteSpace(step.CharacterName))
			{
				return step.CharacterName.Equals(character.Name, StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}
	}
}
