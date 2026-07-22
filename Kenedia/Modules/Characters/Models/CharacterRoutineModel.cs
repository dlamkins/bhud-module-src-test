using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kenedia.Modules.Characters.Models
{
	public class CharacterRoutineModel : INotifyPropertyChanged
	{
		private ObservableCollection<CharacterRoutineStep> _steps = new ObservableCollection<CharacterRoutineStep>();

		public Guid Id
		{
			[CompilerGenerated]
			get
			{
				return _003CId_003Ek__BackingField;
			}
			set
			{
				SetField(ref _003CId_003Ek__BackingField, value, "Id");
			}
		}

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return _003CName_003Ek__BackingField;
			}
			set
			{
				SetField(ref _003CName_003Ek__BackingField, value, "Name");
			}
		}

		public ObservableCollection<CharacterRoutineStep> RoutineSteps
		{
			get
			{
				return _steps;
			}
			set
			{
				SetRoutineSteps(value ?? new ObservableCollection<CharacterRoutineStep>());
			}
		}

		public DateTimeOffset Created
		{
			[CompilerGenerated]
			get
			{
				return _003CCreated_003Ek__BackingField;
			}
			set
			{
				SetField(ref _003CCreated_003Ek__BackingField, value, "Created");
			}
		}

		[JsonConverter(typeof(StringEnumConverter))]
		public ResetFrequency ResetFrequency
		{
			[CompilerGenerated]
			get
			{
				return _003CResetFrequency_003Ek__BackingField;
			}
			set
			{
				SetField(ref _003CResetFrequency_003Ek__BackingField, value, "ResetFrequency");
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public CharacterRoutineModel()
		{
			_003CId_003Ek__BackingField = Guid.NewGuid();
			_003CName_003Ek__BackingField = string.Empty;
			_003CCreated_003Ek__BackingField = DateTimeOffset.UtcNow;
			base._002Ector();
			HookRoutineSteps(_steps);
		}

		public CharacterRoutineModel(string name)
			: this()
		{
			Name = name;
		}

		public void AddRoutineStep(string characterName, string description)
		{
			RoutineSteps.Add(new CharacterRoutineStep(characterName, description));
		}

		public CharacterRoutineModel Copy(string name)
		{
			CharacterRoutineModel copy = new CharacterRoutineModel(name)
			{
				ResetFrequency = ResetFrequency
			};
			foreach (CharacterRoutineStep step in RoutineSteps)
			{
				copy.RoutineSteps.Add(step.Copy());
			}
			return copy;
		}

		public void RemoveRoutineStep(CharacterRoutineStep step)
		{
			RoutineSteps.Remove(step);
		}

		public void ResetCompletion()
		{
			foreach (CharacterRoutineStep routineStep in RoutineSteps)
			{
				routineStep.Completed = null;
			}
		}

		public bool CheckAndApplyScheduledReset()
		{
			if (ResetFrequency == ResetFrequency.None)
			{
				return false;
			}
			DateTimeOffset now = DateTimeOffset.UtcNow;
			DateTimeOffset? boundary = GetMostRecentResetBoundary(now);
			if (!boundary.HasValue)
			{
				return false;
			}
			bool resetAny = false;
			foreach (CharacterRoutineStep step in RoutineSteps)
			{
				if (step.Completed.HasValue && step.Completed.Value < boundary.Value.UtcDateTime)
				{
					step.Completed = null;
					resetAny = true;
				}
			}
			return resetAny;
		}

		private DateTimeOffset? GetMostRecentResetBoundary(DateTimeOffset now)
		{
			switch (ResetFrequency)
			{
			case ResetFrequency.Daily:
				return new DateTimeOffset(now.UtcDateTime.Date, TimeSpan.Zero);
			case ResetFrequency.Weekly:
			{
				DateTime utcNow = now.UtcDateTime;
				int daysSinceMonday = (int)(utcNow.DayOfWeek - 1 + 7) % 7;
				DateTime monday = utcNow.Date.AddDays(-daysSinceMonday);
				DateTimeOffset mondayReset = new DateTimeOffset(monday.Year, monday.Month, monday.Day, 7, 30, 0, TimeSpan.Zero);
				if (now < mondayReset)
				{
					mondayReset = mondayReset.AddDays(-7.0);
				}
				return mondayReset;
			}
			default:
				return null;
			}
		}

		private void SetRoutineSteps(ObservableCollection<CharacterRoutineStep> steps)
		{
			if (_steps != steps)
			{
				UnhookRoutineSteps(_steps);
				_steps = steps;
				HookRoutineSteps(_steps);
				OnPropertyChanged("RoutineSteps");
			}
		}

		private void HookRoutineSteps(ObservableCollection<CharacterRoutineStep> steps)
		{
			if (steps == null)
			{
				return;
			}
			steps.CollectionChanged += RoutineSteps_CollectionChanged;
			foreach (CharacterRoutineStep step in steps)
			{
				HookStep(step);
			}
		}

		private void UnhookRoutineSteps(ObservableCollection<CharacterRoutineStep> steps)
		{
			if (steps == null)
			{
				return;
			}
			steps.CollectionChanged -= RoutineSteps_CollectionChanged;
			foreach (CharacterRoutineStep step in steps)
			{
				UnhookStep(step);
			}
		}

		private void RoutineSteps_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
			{
				foreach (CharacterRoutineStep step2 in e.OldItems)
				{
					UnhookStep(step2);
				}
			}
			if (e.NewItems != null)
			{
				foreach (CharacterRoutineStep step in e.NewItems)
				{
					HookStep(step);
				}
			}
			OnPropertyChanged("RoutineSteps");
		}

		private void HookStep(CharacterRoutineStep step)
		{
			if (step != null)
			{
				step.PropertyChanged += new PropertyChangedEventHandler(Step_PropertyChanged);
			}
		}

		private void UnhookStep(CharacterRoutineStep step)
		{
			if (step != null)
			{
				step.PropertyChanged -= new PropertyChangedEventHandler(Step_PropertyChanged);
			}
		}

		private void Step_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnPropertyChanged("RoutineSteps");
		}

		private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals(field, value))
			{
				return false;
			}
			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
