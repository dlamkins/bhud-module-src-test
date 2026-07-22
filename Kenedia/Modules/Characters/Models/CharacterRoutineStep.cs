using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kenedia.Modules.Characters.Models
{
	public class CharacterRoutineStep : INotifyPropertyChanged
	{
		public string CharacterName
		{
			[CompilerGenerated]
			get
			{
				return _003CCharacterName_003Ek__BackingField;
			}
			set
			{
				SetField(ref _003CCharacterName_003Ek__BackingField, value, "CharacterName");
			}
		}

		public string Description
		{
			[CompilerGenerated]
			get
			{
				return _003CDescription_003Ek__BackingField;
			}
			set
			{
				SetField(ref _003CDescription_003Ek__BackingField, value, "Description");
			}
		}

		public DateTime? Completed
		{
			[CompilerGenerated]
			get
			{
				return _003CCompleted_003Ek__BackingField;
			}
			set
			{
				SetField(ref _003CCompleted_003Ek__BackingField, NormalizeUtc(value), "Completed");
			}
		}

		public bool IsCompleted => Completed.HasValue;

		public bool Enabled
		{
			[CompilerGenerated]
			get
			{
				return _003CEnabled_003Ek__BackingField;
			}
			set
			{
				SetField(ref _003CEnabled_003Ek__BackingField, value, "Enabled");
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public CharacterRoutineStep()
		{
			_003CCharacterName_003Ek__BackingField = string.Empty;
			_003CDescription_003Ek__BackingField = string.Empty;
			_003CEnabled_003Ek__BackingField = true;
			base._002Ector();
		}

		public CharacterRoutineStep(string characterName, string description)
		{
			_003CCharacterName_003Ek__BackingField = string.Empty;
			_003CDescription_003Ek__BackingField = string.Empty;
			_003CEnabled_003Ek__BackingField = true;
			base._002Ector();
			CharacterName = characterName;
			Description = description;
		}

		public CharacterRoutineStep Copy()
		{
			return new CharacterRoutineStep(CharacterName, Description)
			{
				Enabled = Enabled
			};
		}

		public void SetCompleted(bool completed, DateTime? completedAtUtc = null)
		{
			Completed = (completed ? NormalizeUtc(completedAtUtc ?? DateTime.UtcNow) : null);
		}

		private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals(field, value))
			{
				return false;
			}
			field = value;
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}

		private static DateTime? NormalizeUtc(DateTime? value)
		{
			if (!value.HasValue)
			{
				return null;
			}
			return value.Value.Kind switch
			{
				DateTimeKind.Utc => value.Value, 
				DateTimeKind.Local => value.Value.ToUniversalTime(), 
				_ => DateTime.SpecifyKind(value.Value, DateTimeKind.Utc), 
			};
		}
	}
}
