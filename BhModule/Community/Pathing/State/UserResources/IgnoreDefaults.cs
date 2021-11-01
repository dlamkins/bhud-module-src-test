using System.Collections.Generic;
using System.Linq;

namespace BhModule.Community.Pathing.State.UserResources
{
	public class IgnoreDefaults
	{
		public const string FILENAME = "ignore.yaml";

		public HashSet<int> Compass { get; set; } = new HashSet<int>(new int[3] { 935, 895, 934 });


		public HashSet<int> Map { get; set; } = new HashSet<int>(new int[3] { 935, 895, 934 });


		public HashSet<int> World { get; set; } = new HashSet<int>(Enumerable.Empty<int>());

	}
}
