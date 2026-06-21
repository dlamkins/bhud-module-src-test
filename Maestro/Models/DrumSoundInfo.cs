using Microsoft.Xna.Framework.Input;

namespace Maestro.Models
{
	public sealed class DrumSoundInfo
	{
		public DrumSound Sound { get; }

		public string Code { get; }

		public string DisplayName { get; }

		public bool NeedsAlt { get; }

		public Keys PrimaryKey { get; }

		public Keys SecondaryKey { get; }

		public bool HasPair => SecondaryKey != PrimaryKey;

		public DrumGroup Group
		{
			get
			{
				if (!NeedsAlt)
				{
					if (Sound != DrumSound.HighTom && Sound != DrumSound.MidTom && Sound != DrumSound.FloorTom)
					{
						return DrumGroup.Drums;
					}
					return DrumGroup.Toms;
				}
				return DrumGroup.Cymbals;
			}
		}

		public DrumSoundInfo(DrumSound sound, string code, string displayName, bool needsAlt, Keys primaryKey, Keys? secondaryKey = null)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			Sound = sound;
			Code = code;
			DisplayName = displayName;
			NeedsAlt = needsAlt;
			PrimaryKey = primaryKey;
			SecondaryKey = secondaryKey.GetValueOrDefault(primaryKey);
		}
	}
}
