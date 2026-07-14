using Maestro.Services.Playback;
using Microsoft.Xna.Framework.Input;

namespace Maestro.Services.Practice
{
	public class KeyboardServiceKeySender : IKeySender
	{
		private readonly KeyboardService _keyboard;

		public KeyboardServiceKeySender(KeyboardService keyboard)
		{
			_keyboard = keyboard;
		}

		public void SendOctaveUp()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			_keyboard.MarkJustSent(_keyboard.GetConfiguredPrimaryKey((Keys)105));
			_keyboard.PlayOctaveChange(up: true);
		}

		public void SendOctaveDown()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			_keyboard.MarkJustSent(_keyboard.GetConfiguredPrimaryKey((Keys)96));
			_keyboard.PlayOctaveChange(up: false);
		}
	}
}
