using System.Collections.Generic;
using Newtonsoft.Json;
using flakysalt.CharacterKeybinds.Data;

namespace flakysalt.CharacterKeybinds.Util
{
	internal class CharacterKeybindJsonUtil
	{
		public static string SerializeCharacterList(List<CharacterKeybind> characterList)
		{
			return JsonConvert.SerializeObject((object)characterList, (Formatting)1);
		}

		public static List<CharacterKeybind> DeserializeCharacterList(string jsonString)
		{
			return JsonConvert.DeserializeObject<List<CharacterKeybind>>(jsonString);
		}
	}
}
