namespace Kenedia.Modules.Characters.Services
{
	public class ContextManager
	{
		public CharactersContext CharactersContext { get; }

		public ContextManager(CharactersContext charactersContext)
		{
			CharactersContext = charactersContext;
		}
	}
}
