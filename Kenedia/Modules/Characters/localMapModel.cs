using System.Collections.Generic;
using Blish_HUD;
using Gw2Sharp.WebApi;

namespace Kenedia.Modules.Characters
{
	public class localMapModel
	{
		public _Names _Names = new _Names();

		public int Id;

		public IReadOnlyList<int> Floors;

		public int DefaultFloor;

		public int ContinentId;

		public string Name
		{
			get
			{
				return GameService.Overlay.UserLocale.Value switch
				{
					Locale.German => _Names.de, 
					Locale.French => _Names.fr, 
					Locale.Spanish => _Names.es, 
					_ => _Names.en, 
				};
			}
			set
			{
				switch (GameService.Overlay.UserLocale.Value)
				{
				case Locale.German:
					_Names.de = value;
					break;
				case Locale.French:
					_Names.fr = value;
					break;
				case Locale.Spanish:
					_Names.es = value;
					break;
				default:
					_Names.en = value;
					break;
				}
			}
		}
	}
}
