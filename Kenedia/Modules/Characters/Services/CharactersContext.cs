using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Contexts;

namespace Kenedia.Modules.Characters.Services
{
	public class CharactersContext : Context
	{
		public enum ContextStatus
		{
			Ready,
			SwitchingRequested,
			SwitchingCharacter,
			LoadingCharacter,
			Error,
			Success
		}

		private static readonly Logger Logger = Blish_HUD.Logger.GetLogger<CharactersContext>();

		public ContextStatus Status { get; private set; }

		public CharactersContext(CharacterSwapping characterSwapping)
		{
		}

		public async Task SwitchCharacter(string name)
		{
			Status = ContextStatus.SwitchingRequested;
			Logger.Debug("WE SHOULD SWITCH CHARACTERS TO " + name);
			await Task.Delay(1000);
			Status = ContextStatus.SwitchingCharacter;
			Logger.Debug("Look for a character named " + name + "...");
			await Task.Delay(1000);
			Logger.Debug("Read the name of the character ...");
			await Task.Delay(1000);
			Logger.Debug("Confirm it matches " + name + " ....");
			await Task.Delay(1000);
			Logger.Debug("Load into the game...");
			await Task.Delay(5000);
			Status = ContextStatus.Success;
			Logger.Debug("Character switched to " + name);
			await Task.CompletedTask;
		}
	}
}
