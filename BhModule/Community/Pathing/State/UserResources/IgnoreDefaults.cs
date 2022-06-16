using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace BhModule.Community.Pathing.State.UserResources
{
	public class IgnoreDefaults
	{
		public const string FILENAME = "ignore.yaml";

		[YamlMember(Description = "A set of maps to not render marker/trails onto the minimap.")]
		public HashSet<int> Compass { get; set; } = new HashSet<int>(new int[3] { 935, 895, 934 });


		[YamlMember(Description = "A set of maps to not render marker/trails onto the fullscreen map.")]
		public HashSet<int> Map { get; set; } = new HashSet<int>(new int[3] { 935, 895, 934 });


		[YamlMember(Description = "A set of maps to not render marker/trails in-game.")]
		public HashSet<int> World { get; set; } = new HashSet<int>(Enumerable.Empty<int>());

	}
}
