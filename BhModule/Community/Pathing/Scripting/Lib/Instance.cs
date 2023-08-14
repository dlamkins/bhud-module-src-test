using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Content;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neo.IronLua;
using TmfLib;
using TmfLib.Pathable;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Scripting.Lib
{
	public class Instance
	{
		private readonly PathingGlobal _global;

		internal Instance(PathingGlobal global)
		{
			_global = global;
		}

		public Vector3 Vector3(float x, float y, float z)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3(x, y, z);
		}

		public Color Color(int r, int g, int b, int a = 255)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return new Color(r, g, b, a);
		}

		private AttributeCollection AttributeCollectionFromLuaTable(LuaTable luaTable)
		{
			return new AttributeCollection(luaTable.Members.Select((KeyValuePair<string, object> member) => new TmfLib.Prototype.Attribute(member.Key, string.Format(CultureInfo.InvariantCulture, "{0}", member.Value))));
		}

		public StandardMarker Marker(IPackResourceManager resourceManager, LuaTable attributes = null)
		{
			PointOfInterest poi = new PointOfInterest(resourceManager, PointOfInterestType.Marker, (attributes != null) ? AttributeCollectionFromLuaTable(attributes) : new AttributeCollection(), _global.ScriptEngine.Module.PackInitiator.PackState.RootCategory);
			StandardMarker marker = _global.ScriptEngine.Module.PackInitiator.PackState.InitPointOfInterest(poi) as StandardMarker;
			if (marker.MapId < 0)
			{
				marker.MapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			}
			return marker;
		}

		public Guid Guid(string base64)
		{
			return AttributeParsingUtil.InternalGetValueAsGuid(base64);
		}

		internal static AsyncTexture2D Texture(TextureResourceManager textureManager, string texturePath)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			AsyncTexture2D outTexture = new AsyncTexture2D();
			textureManager.PreloadTexture(texturePath, shouldSample: false);
			textureManager.LoadTextureAsync(texturePath).ContinueWith(delegate(Task<(Texture2D Texture, Color Sample)> textureTaskResult)
			{
				if (!textureTaskResult.IsFaulted && textureTaskResult.Result.Texture != null)
				{
					outTexture.SwapTexture(textureTaskResult.Result.Texture);
				}
			});
			return outTexture;
		}

		public AsyncTexture2D Texture(int textureId)
		{
			return AsyncTexture2D.FromAssetId(textureId) ?? AsyncTexture2D.op_Implicit(Textures.get_Error());
		}

		public AsyncTexture2D Texture(PackContext pack, string texturePath)
		{
			return Texture(TextureResourceManager.GetTextureResourceManager(pack.ResourceManager), texturePath);
		}
	}
}
