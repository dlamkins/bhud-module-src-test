using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager
{
	public static class API_Extension
	{
		public static readonly ConditionalWeakTable<GW2API.BaseObject, ObjectExtension> Flags = new ConditionalWeakTable<GW2API.BaseObject, ObjectExtension>();

		public static Texture2D GetTextureFile(GW2API.BaseObject obj, string path, object targetControl = null, IconTargets iconTarget = IconTargets.Icon)
		{
			return BuildsManager.s_moduleInstance.TextureManager._Icons[0];
		}

		public static Texture2D GetIcon(this GW2API.Item o, string path = null, object targetControl = null)
		{
			return GetTextureFile(o, (path == null && o != null && o.Icon != null) ? o.Icon.Path : path, targetControl);
		}

		public static Texture2D GetIcon(this GW2API.Trait o, string path = null, object targetControl = null)
		{
			return GetTextureFile(o, (path == null && o != null && o.Icon != null) ? o.Icon.Path : path, targetControl);
		}

		public static Texture2D GetIcon(this GW2API.Skill o, string path = null, object targetControl = null)
		{
			return GetTextureFile(o, (path == null && o != null && o.Icon != null) ? o.Icon.Path : path, targetControl);
		}

		public static Texture2D GetIcon(this GW2API.Specialization o, string path = null, object targetControl = null)
		{
			return GetTextureFile(o, (path == null && o != null && o.Icon != null) ? o.Icon.Path : path, targetControl);
		}

		public static Texture2D GetBackground(this GW2API.Specialization o, string path = null, object targetControl = null)
		{
			return GetTextureFile(o, (path == null && o != null && o.Background != null) ? o.Background.Path : path, targetControl, IconTargets.Background);
		}
	}
}
